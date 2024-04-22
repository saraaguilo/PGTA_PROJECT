using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;


namespace OurClasses
{

    public class Decoder48
    {
        public DateTime timeOfDay { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }

        public int SAC { get; set; }
        public int SIC { get; set; }

        public string FX { get; set; }

        string TYP { get; set; }
        string SIM { get; set; }
        string RDP { get; set; }
        string SPI { get; set; }
        string RAB { get; set; }
        string TST { get; set; }
        string ERR { get; set; }
        string XPP { get; set; }
        string ME { get; set; }
        string MI { get; set; }
        string FOE_FRI { get; set; }
        string ADSB_EP { get; set; }
        string ADSB_VAL { get; set; }
        string SCN_EP { get; set; }
        string SCN_VAL { get; set; }
        string PAI_EP { get; set; }
        string PAI_VAL { get; set; }
        //string? SPARE { get; set; }

        public string V { get; set; }
        public string G { get; set; }
        public string L { get; set; }
        public string mode3A { get; set; }

        public string V2 { get; set; }
        public string G2 { get; set; }
        public double FL { get; set; }

        public string SRL { get; set; }
        public string SSR { get; set; }
        public string SAM { get; set; }
        public string PRL { get; set; }
        public string PAM { get; set; }
        public string RPD { get; set; }
        public string APD { get; set; }
        public double SRL2 { get; set; };
        public int SRR2 { get; set; };
        public double SAM2 { get; set; };
        public double PRL2 { get; set; };
        public double PAM2 { get; set; };
        public double RPD2 { get; set; };
        public double APD2 { get; set; };

        public List<int> fullModeS { get; set; };//mirar-ho bé
        public BDSCode4 modeBDS4 { get; set; }
        public BDSCode5 modeBDS5 { get; set; }
        public BDSCode6 modeBDS6 { get; set; }

        public int MCPstatus { get; set; }
        public double MCPalt { get; set; }
        public int FMstatus { get; set; }
        public double FMalt { get; set; }
        public int BPstatus { get; set; }
        public double BPpres { get; set; }
        public int modeStat { get; set; }
        public int VNAV { get; set; }
        public int ALThold { get; set; }
        public int App { get; set; } //approach
        public int targetalt_status { get; set; }
        public string targetalt_source { get; set; }

        public int RAstatus { get; set; }
        public double RA { get; set; } //roll angle
        public int TTAstatus { get; set; }
        public double TTA { get; set; } //true track angle
        public int GSstatus { get; set; }
        public double GS { get; set; } //ground speed
        public int TARstatus { get; set; }
        public double TAR { get; set; } //track angle rate
        public int TASstatus { get; set; }
        public double TAS { get; set; } //true airspeed

        public int HDGstatus { get; set; }
        public double HDG { get; set; }
        public int IASstatus { get; set; }
        public double IAS { get; set; }
        public int MACHstatus { get; set; }
        public double MACH { get; set; }
        public int BARstatus { get; set; }
        public double BAR { get; set; }
        public int IVVstatus { get; set; }
        public double IVV { get; set; }

        public int tracknumber { get; set; }

        public double x_component { get; set; }
        public double y_component { get; set; }

        public double rho { get; set; }
        public double theta { get; set; }

        public double groundspeedpolar;
        public double headingpolar;

        public string CNF { get; set; }
        public string RAD { get; set; }
        public string DOU { get; set; }
        public string MAH { get; set; }
        public string CDM { get; set; }
        public string TRE { get; set; }
        public string GHO { get; set; }
        public string SUP { get; set; }
        public string TCC { get; set; }

        public double measuredheight { get; set; };

        public string COM { get; set; }
        public string STAT { get; set; }
        public string SI { get; set; }
        public string MSCC { get; set; }
        public string ARC { get; set; }
        public string AIC { get; set; }
        public string B1A { get; set; }
        public string B1B { get; set; }

        public Coordinates RadarCoordinates { get; private set; }
        public GeoUtils GeoUtils { get; private set; } = new GeoUtils();



        public TimeSpan decodeTimeDay(byte[] data)
        {
            double resolution = Math.Pow(2, -7); //in seconds
            double seconds = ByteToDoubleDecoding(data, resolution);
            TimeSpan TimeofDay = TimeSpan.FromSeconds(seconds);
            return TimeofDay;
        }

        private double ByteToDoubleDecoding(byte[] data, double resolution)
        {
            double bytesValue = 0;

            for (int i = 0; i < data.Length; i++)
            {
                bytesValue *= 256; // Multiplicar por 2^8, ya que un byte tiene 8 bits
                bytesValue += data[i]; 
            }

            return bytesValue * resolution;
        }

        public double latitudeDecoding(double cooR, double cooTheta, double FL)
        {
            int Rt = 6370000;
            double Fl = 0;
            if (FL > 0)
            {
                Fl = (FL * 100) * GeoUtils.FEET2METERS;
            }
            double hr = 27.257; //m
            double El = Math.Asin((2 * Rt * (Fl - hr) + Math.Pow(Fl, 2) - Math.Pow(hr, 2) - Math.Pow(cooR * GeoUtils.NM2METERS, 2)) / (2 * cooR * GeoUtils.NM2METERS * (Rt + hr)));
            CoordinatesPolar objectPolar = new CoordinatesPolar(cooR * GeoUtils.NM2METERS, cooTheta * GeoUtils.DEGS2RADS, El);
            CoordinatesWGS84 radarWGS84 = new CoordinatesWGS84(GeoUtils.LatLon2Degrees(41, 18, 2.5284, 0) * (Math.PI / 180.0), GeoUtils.LatLon2Degrees(2, 6, 7.4095, 0) * (Math.PI / 180.0), 27.257);

            CoordinatesXYZ objectRadarCart = GeoUtils.change_radar_spherical2radar_cartesian(objectPolar);
            CoordinatesXYZ objectGeocentric = GeoUtils.change_radar_cartesian2geocentric(radarWGS84, objectRadarCart);
            CoordinatesWGS84 geodesicWGS84 = GeoUtils.change_geocentric2geodesic(objectGeocentric);

            this.latitude = geodesicWGS84.Lat * (180.0 / Math.PI);
            return this.latitude;
        }
        public double longitudeDecoding(double cooR, double cooTheta, double FL)
        {
            int Rt = 6370000;
            double Fl = 0;
            if (FL > 0)
            {
                Fl = (FL * 100) * GeoUtils.FEET2METERS;
            }
            double hr = 27.257; //m
            double El = Math.Asin((2 * Rt * (Fl - hr) + Math.Pow(Fl, 2) - Math.Pow(hr, 2) - Math.Pow(cooR * GeoUtils.NM2METERS, 2)) / (2 * cooR * GeoUtils.NM2METERS * (Rt + hr)));
            CoordinatesPolar objectPolar = new CoordinatesPolar(cooR * GeoUtils.NM2METERS, cooTheta * GeoUtils.DEGS2RADS, El);
            CoordinatesWGS84 radarWGS84 = new CoordinatesWGS84(GeoUtils.LatLon2Degrees(41, 18, 2.5284, 0) * (Math.PI / 180.0), GeoUtils.LatLon2Degrees(2, 6, 7.4095, 0) * (Math.PI / 180.0), 27.257);

            CoordinatesXYZ objectRadarCart = GeoUtils.change_radar_spherical2radar_cartesian(objectPolar);
            CoordinatesXYZ objectGeocentric = GeoUtils.change_radar_cartesian2geocentric(radarWGS84, objectRadarCart);
            CoordinatesWGS84 geodesicWGS84 = GeoUtils.change_geocentric2geodesic(objectGeocentric);

            this.longitude = geodesicWGS84.Lon * (180.0 / Math.PI);
            return this.longitude;
        }

        public string TYPDecoding(byte[] data)
        {
            int typ = (data[0] >> 5) & 0b00000111; //obtenemos los tres primeros bits del array de data
            Dictionary<int, string> typDescriptions = new Dictionary<int, string>() //mapeamos el valor de typ
        {
            { 0, "No detection" },
            { 1, "PSR" },
            { 2, "SSR" },
            { 3, "SSR + PSR" },
            { 4, "ModeS All-Cal" },
            { 5, "ModeS Roll-Call" },
            { 6, "ModeS All-Call + PSR" },
            { 7, "ModeS Roll-Call + PSR" }
        };

        return typDescriptions.ContainsKey(typ)? typDescriptions[typ] : ""; //si no se encuentra, devuelve cadena vacía 
        }
        
        public string SIMDecoding(byte[] data)
        {

        int sim = (data[0] >> 4) & 0b00000001; //cuarto bit del array de data 00010000
        Dictionary<int, string> simDescriptions = new Dictionary<int, string>()
    {
        { 0, "Actual target report" },
        { 1, "Simulated target report" }
    };

        return simDescriptions.ContainsKey(sim) ? simDescriptions[sim] : "";
        }

        public string RDPDecoding(byte[] data)
        {
        
        int rdp = (data[0] >> 3) & 0b00000001;
        Dictionary<int, string> rdpDescriptions = new Dictionary<int, string>()
        {
        { 0, "Report from RDP Chain 1" },
        { 1, "Report from RDP Chain 2" }
        };
        return rdpDescriptions.ContainsKey(rdp) ? rdpDescriptions[rdp] : "";
        }
        
        public string SPIDecoding(byte[] data)
        {
       
        int spi = (data[0] >> 2) & 0b00000001;
        Dictionary<int, string> spiDescriptions = new Dictionary<int, string>()
        {
        { 0, "Absence of SPI" },
        { 1, "Special Position Identification" }
        };

        return spiDescriptions.ContainsKey(spi) ? spiDescriptions[spi] : "";
        }

        public string RABDecoding(byte[] data)
        {

        int rab = (data[0] >> 1) & 0b00000001;
        Dictionary<int, string> rabDescriptions = new Dictionary<int, string>()
        {
        { 0, "Report from aircraft transponder" },
        { 1, "Report from field monitor (fixed transponder)" }
        };

            return rabDescriptions.ContainsKey(rab) ? rabDescriptions[rab] : "";
        }

        public string FXDecoding(byte[] data)
        {
            
            int fx = (data[0] >> 0) & 0b00000001;
            Dictionary<int, string> fxDescriptions = new Dictionary<int, string>()
        {
        { 0, "End of data item" },
        { 1, "Extension into next extent" }
        };
            return fxDescriptions.ContainsKey(fx) ? fxDescriptions[fx] : "";

        }

        public string TSTDecoding(byte[] data)//es un campo opcional
        {
            return this.TST;

        }
        public string ERRDecoding(byte[] data)//es un campo opcional
        {
            return this.ERR;

        }
        public string XPPDecoding(byte[] data)//es un campo opcional
        {
            return this.XPP;

        }
        public string MEDecoding(byte[] data)//es un campo opcional
        {
            return this.ME;

        }
        public string MIDecoding(byte[] data)//es un campo opcional
        {
            return this.MI;

        }
        public string FOE_FRIDecoding(byte[] data)//es un campo opcional
        {
            return this.FOE_FRI;

        }
        public string ADSB_EPDecoding(byte[] data)
        {

            int adsb_ep = (data[0] >> 7) & 0b00000001;
            Dictionary<int, string> adsb_epDescriptions = new Dictionary<int, string>()
    {
        { 0, "ADSB not populated" },
        { 1, "ADSB populated" }
    };
            return adsb_epDescriptions.ContainsKey(adsb_ep) ? adsb_epDescriptions[adsb_ep] : "";

        }
        public string ADSB_VALDecoding(byte[] data)
        {

            int adsb_val = (data[0] >> 6) & 0b00000001;
            Dictionary<int, string> adsb_valDescriptions = new Dictionary<int, string>()
    {
        { 0, "not available" },
        { 1, "available" }
    };
            return adsb_valDescriptions.ContainsKey(adsb_val) ? adsb_valDescriptions[adsb_val] : "";

        }
        public string SCN_EPDecoding(byte[] data)
        {

            int scn_ep = (data[0] >> 5) & 0b00000001;
            Dictionary<int, string> scn_epDescriptions = new Dictionary<int, string>()
    {
        { 0, "SCN not populated" },
        { 1, "SCN populated" }
    };
            return scn_epDescriptions.ContainsKey(scn_ep) ? scn_epDescriptions[scn_ep] : "";

        }
        public string SCN_VALDecoding(byte[] data)
        {

            int scn_val = (data[0] >> 4) & 0b00000001;
            Dictionary<int, string> scn_valDescriptions = new Dictionary<int, string>()
    {
        { 0, "not available" },
        { 1, "available" }
    };
            return scn_valDescriptions.ContainsKey(scn_val) ? scn_valDescriptions[scn_val] : "";

        }
        public string PAI_EPDecoding(byte[] data)
        {

            int pai_ep = (data[0] >> 3) & 0b00000001;
            Dictionary<int, string> pai_epDescriptions = new Dictionary<int, string>()
    {
        { 0, "PAI not populated" },
        { 1, "PAI populated" }
    };
            return pai_epDescriptions.ContainsKey(pai_ep) ? pai_epDescriptions[pai_ep] : "";

        }
        public string PAI_VALDecoding(byte[] data)
        {

            int pai_val = (data[0] >> 2) & 0b00000001;
            Dictionary<int, string> pai_valDescriptions = new Dictionary<int, string>()
    {
        { 0, "not available" },
        { 1, "available" }
    };
            return pai_valDescriptions.ContainsKey(pai_val) ? pai_valDescriptions[pai_val] : "";

        }

        //ahora vamos a decodificara los parámetros de Mode-3A, en representación octal

        public string VDecoding(byte[] data)
        {
            
            int[] v = (data[0] >> 7) & 0b00000001;
            Dictionary<int, string> vDescriptions = new Dictionary<int, string>()
    {
        { 0, "Code validated" },
        { 1, "Code not validated" }
    };
            return vDescriptions.ContainsKey(v) ? vDescriptions[v] : "";

        }
        public string GDecoding(byte[] data)
        {

            int[] g = (data[0] >> 6) & 0b00000001;
            Dictionary<int, string> gDescriptions = new Dictionary<int, string>()
    {
        { 0, "Default" },
        { 1, "Garbled code" }
    };
            return gDescriptions.ContainsKey(g) ? gDescriptions[g] : "";

        }
        public string LDecoding(byte[] data)
        {

            int[] l = (data[0] >> 5) & 0b00000001;
            Dictionary<int, string> lDescriptions = new Dictionary<int, string>()
    {
        { 0, "Mode-3/A code derived from the reply of the transponder" },
        { 1, "Mode-3/A code not extracted during the last scan" }
    };
            return lDescriptions.ContainsKey(l) ? lDescriptions[l] : "";

        }
        public string mode3ADecoding(byte[] dataItem)
        {
            // Aquí extraemos los bits A, B, C y D del modo 3A
            int A = (dataItem[0] >> 1) & 0b00000111;
            int B = ((dataItem[0] & 0b00000001) << 2) | ((dataItem[1] >> 6) & 0b00000011);
            int C = (dataItem[1] >> 3) & 0b00000111;
            int D = dataItem[1] & 0b00000111;
            int mode3A = A * 1000 + B * 100 + C * 10 + D;
            // Queremos que el resultado tenga al menos 3 dígitos
            string mode3AFormatted = mode3A.ToString("D3");
            return mode3AFormatted;
        }
        //Ahora vamos a decodificar el flight level (binary representation)
        public string V2Decoding(byte[] data)
        {
            int V2 = (data[0] >> 7) & 0b00000001;
            Dictionary<int, string> V2Descriptions = new Dictionary<int, string>()
    {
        { 0, "Code validated" },
        { 1, "Code not validated" }
    };

            return V2Descriptions.ContainsKey(V2) ? V2Descriptions[V2] : "";
        }
        public string G2Decoding(byte[] data)
        {
            int G2 = (data[0] >> 6) & 0b00000001;
            Dictionary<int, string> G2Descriptions = new Dictionary<int, string>()
        {
            { 0, " Default" },
            { 1, " Garbled code" }
        };

            return G2Descriptions.ContainsKey(G2) ? G2Descriptions[G2] : "";
        }
        public double FLDecoding(byte[] data)
        {
            byte firstByte = data[0];
            byte secondByte = data[1];

            int FLBits = (firstByte & 0b00111111) << 8 | secondByte;
            bool isNegative = (firstByte & 0b00100000) != 0;
            double resolutionLSB = 1.0 / 4.0; // Resolución de FL

            if (isNegative)
            {
                FLBits = (~FLBits + 1) & 0x3FFF;
            }

            double flightLevel = FLBits * resolutionLSB;
            this.FL = flightLevel;
            return this.FL;

        }
        //Ahora se decodificará Radar Plot Characteristics
        public string SRLDecoding(byte[] data)
        {
            int srl = (data[0] >> 7) & 0b00000001;
            Dictionary<int, string> srlDescriptions = new Dictionary<int, string>()
        {
            { 0, "Absence of Subfield #1: SSR plot runlength" },
            { 1, "Presence of Subfield #1: SSR plot runlength" }
        };

            return srlDescriptions.ContainsKey(srl) ? srlDescriptions[srl] : "";
        }
        public double SRL2Decoding(byte[] data)
        {
            double resolution = 360.0 / 213.0; //en dgs
            this.SRL2 = ByteToDoubleDecoding(data, resolution);
            return this.SRL2;
        }
        public string SSRDecoding(byte[] data)
        {
            int ssr = (data[0] >> 6) & 0b00000001;
            Dictionary<int, string> ssrDescriptions = new Dictionary<int, string>()
        {
            { 0, "Absence of Subfield #2: Number of received replies for M(SSR)" },
            { 1, "Presence of Subfield #2: Number of received replies for M(SSR)\" " }
        };

            return ssrDescriptions.ContainsKey(ssr) ? ssrDescriptions[ssr] : "";
        }
        public int SSR2Decoding(byte[] data)
        {
            double resolution = 1; //no tiene unidades
            this.SSR2 = ByteToDoubleDecoding(data, resolution);
            return this.SSR2;
        }
        public string SAMDecoding(byte[] data)
        {
            int sam = (data[0] >> 5) & 0b00000001;
            Dictionary<int, string> samDescriptions = new Dictionary<int, string>()
        {
            { 0, "Absence of Subfield #3: Amplitude of received replies for M(SSR)" },
            { 1, "Presence of Subfield #3: Amplitude of received replies for M(SSR)" }
        };
            return samDescriptions.ContainsKey(sam) ? samDescriptions[sam] : "";
        }
        public double SAM2Decoding(byte[] data)
        {
            double resolution = 1; // dBm
            double bytesValue = 0;
            for (int i = 0; i < data.Length; i++)
            {
                bytesValue *= 256; 
                bytesValue += data[i];
            }
            if ((data[0] & 128) != 0)
            {
                // Conversión de dos complementos para valores negativos
                bytesValue = (ushort)~(bytesValue - 1);
                bytesValue *= -1;
            }
            this.SAM2 = bytesValue * resolution;
            return this.SAM2;
        }
        public string PRLDecoding(byte[] data)
        {
            int prl = (data[0] >> 4) & 0b00000001;
            Dictionary<int, string> prlDescriptions = new Dictionary<int, string>()
        {
            { 0, "Absence of Subfield #4: PSR plot runlength" },
            { 1, "Presence of Subfield #4: PSR plot runlength" }
        };
            return prlDescriptions.ContainsKey(prl) ? prlDescriptions[prl] : "";
        }
        public double PRL2Decoding(byte[] data)
        {
            double resolution = 360.0 / 213.0; //en dgs
            this.PRL2 = ByteToDoubleDecoding(data, resolution);
            return this.PRL2;
        }
        public string PAMDecoding(byte[] data)
        {
            int pam = (data[0] >> 3) & 0b00000001;
            Dictionary<int, string> pamDescriptions = new Dictionary<int, string>()
        {
            { 0, "Absence of Subfield #5: PSR amplitude" },
            { 1, "Presence of Subfield #5: PSR amplitude" }
        };
            return pamDescriptions.ContainsKey(pam) ? pamDescriptions[pam] : "";
        }
        public double PAM2Decoding(byte[] data)
        {
            double resolution = 1; // dBm
            double bytesValue = 0;
            for (int i = 0; i < data.Length; i++)
            {
                bytesValue *= 256;
                bytesValue += data[i];
            }
            if ((data[0] & 128) != 0)
            {
                // Hacemos la conversión de dos complementos para valores negativos
                bytesValue = (ushort)~(bytesValue - 1);
                bytesValue *= -1;
            }
            this.PAM2 = bytesValue * resolution;
            return this.PAM2;
        }
        public string RPDDecoding(byte[] data)
        {
            int rpd = (data[0] >> 2) & 0b00000001;
            Dictionary<int, string> rpdDescriptions = new Dictionary<int, string>()
        {
            { 0, "Absence of Subfield #6: Difference in Range between PSR and SSR plot" },
            { 1, "Presence of Subfield #6: Difference in Range between PSR and SSR plot" }
        };
            return rpdDescriptions.ContainsKey(rpd) ? rpdDescriptions[rpd] : "";
        }
        public double RPD2Decoding(byte[] data)
        {
            double resolution = 1 / 256; // NM
            double bytesValue = 0;
            for (int i = 0; i < data.Length; i++)
            {
                bytesValue *= 256;
                bytesValue += data[i];
            }
            if ((data[0] & 128) != 0)
            {
                // Hacemos la conversión de dos complementos para valores negativos
                bytesValue = (ushort)~(bytesValue - 1);
                bytesValue *= -1;
            }
            this.RPD2 = bytesValue * resolution;
            return this.RPD2;
        }
        public string APDDecoding(byte[] data)
        {
            int apd = (data[0] >> 1) & 0b00000001;
            Dictionary<int, string> apdDescriptions = new Dictionary<int, string>()
        {
            { 0, "Absence of Subfield #7: Difference in Azimuth between PSR and SSR plot" },
            { 1, "Presence of Subfield #7: Difference in Azimuth between PSR and SSR plot" }
        };
            return apdDescriptions.ContainsKey(apd) ? apdDescriptions[apd] : "";
        }
        public double APD2Decoding(byte[] data)
        {
            double resolution = 360.0 / Math.Pow(2, 14); // dgs
            double bytesValue = 0;
            for (int i = 0; i < data.Length; i++)
            {
                bytesValue *= 256;
                bytesValue += data[i];
            }
            if ((data[0] & 128) != 0)
            {
                // Hacemos la conversión de dos complementos para valores negativos
                bytesValue = (ushort)~(bytesValue - 1);
                bytesValue *= -1;
            }
            this.APD2 = bytesValue * resolution;
            return this.APD2;
        }
        public int[] fullModeSDecoding(byte[] data)
        {
            int bdsByte = data[7];

            int bds1 = (bdsByte & 0xF0) >> 4; // Entrada
            int bds2 = bdsByte & 0x0F;        // Salida

            return new int[] { bds1, bds2 };
        }
        public int MCPStatusDecoding(byte[] data)
        {
            int mcpstatus = (data[0] >> 7) & 0b00000001;
            this.MCPstatus = mcpstatus;
            return this.MCPstatus;
        }
        public double MCPaltDecoding(byte[] data)
        {
            byte mcpalt1 = (byte)((data[0] >> 0) & 0b01111111);
            byte mcpalt2 = (byte)((data[1] >> 3) & 0b00011111);

            int combinedValue = (mcpalt1 << 5) | mcpalt2;

            double resolution = 16; // Resolución en ft
            double mcpAltitude = combinedValue * resolution;
            this.MCPalt = mcpAltitude;
            return mcpAltitude;
        }
        public int FMstatusDecoding(byte[] data)
        {
            int fmstatus = (data[0] >> 2) & 0b00000001;
            this.FMstatus = fmstatus;
            return this.FMstatus;

        }
        public double FMaltDecoding(byte[] data)
        {
            byte fmalt1 = (byte)((data[0] >> 0) & 0b00000011);
            byte fmalt2 = (byte)((data[1] >> 0) & 0b11111111);
            byte fmalt3 = (byte)((data[2] >> 6) & 0b00000011);

            int combinedValue = (fmalt1 << 16) | (fmalt2 << 6) | fmalt3;

            double resolution = 16; // Resolución en ft
            double fmAltitude = combinedValue * resolution;
            this.FMalt = fmAltitude;
            return fmAltitude;

        }
        public int BPstatusDecoding(byte[] data)
        {
            int bpstatus = (data[0] >> 5) & 0b00000001;
            this.BPstatus = bpstatus;
            return this.BPstatus;
        }
        public double BPpresDecoding(byte[] data)
        {
            byte bppres1 = (byte)((data[0] >> 0) & 0b00011111);
            byte bppres2 = (byte)((data[1] >> 1) & 0b01111111);

            int combinedValue = (bppres1 << 7) | bppres2;

            double resolution = 0.1; // Resolución en mb
            double bppres = combinedValue * resolution;
            this.BPpres = bppres + 800;
            return this.BPpres;

        }
        public int modeStatDecoding(byte[] data)
        {
            int modeStat = (data[0] >> 0) & 0b00000001;
            this.modeStat = modeStat;
            return this.modestatus;
        }
        public int VNAVDecoding(byte[] data)
        {
            int vnav = (data[0] >> 7) & 0b00000001;
            this.VNAV = vnav;
            return this.VNAV;
        }
        public int ALTholdDecoding(byte[] data)
        {
            int ALThold = (data[0] >> 6) & 0b00000001;
            this.ALThold = ALThold;
            return this.ALThold;
        }
        public int AppDecoding(byte[] data)
        {
            int App = (data[0] >> 5) & 0b00000001;
            this.App = App;
            return this.App;
        }
        public int targetalt_statusDecoding(byte[] data)
        {
            int targetalt_status = (data[0] >> 2) & 0b00000001;
            this.targetalt_status = targetalt_status;
            return this.targetalt_status;
        }
        public string targetalt_sourceDecoding(byte[] data)
        {
            int targetalt_source = (data[0] >> 0) & 0b00000011;
            Dictionary<int, string> targetalt_sourceDescriptions = new Dictionary<int, string>()
        {
            { 0, "Unknown" },
            { 1, "Aircraft altitude" },
            { 2, "FCU/MCP selected altitude" },
            { 3, "FMS selected altitude" }
        };
            return targetalt_sourceDescriptions.ContainsKey(targetalt_source) ? targetalt_sourceDescriptions[targetalt_source] : "";
        }
        public int RAstatusDecoding(byte[] data)
        {
            int RAstatus = (data[0] >> 7) & 0b00000001;
            this.RAstatus = RAstatus;
            return this.RAstatus;
        }
        public double RADecoding(byte[] data) //valores negativos?
        {
            byte RA1 = (byte)((data[0] >> 0) & 0b01111111);
            byte RA2 = (byte)((data[1] >> 5) & 0b00000111);

            // Determinar el signo
            byte signByte = (byte)((data[0] >> 6) & 0b00000001);
            int combinedValue;
            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (RA1 << 3) | RA2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((RA1 << 3) | RA2);
                combinedValue = -(~combinedValue + 1);
            }
            double resolution = 45.0 / 256.0; //resolución en grados
            double RA = combinedValue * resolution;
            this.RA = RA;
            return this.RA;
        }
        public int TTAstatusDecoding(byte[] data)
        {
            int TTAstatus = (data[0] >> 4) & 0b00000001;
            this.TTAstatus = TTAstatus;
            return this.TTAstatus;
        }
        public double TTADecoding(byte[] data)
        {
            byte TTA1 = (byte)((data[0] >> 0) & 0b00001111);
            byte TTA2 = (byte)((data[1] >> 1) & 0b01111111);
            byte signByte = (byte)((data[0] >> 3) & 0b00000001);
            
            int combinedValue;
            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (TTA1 << 7) | TTA2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((TTA1 << 7) | TTA2);
                combinedValue = -(~combinedValue + 1);
            }
            double resolution = 90.0 / 512.0;
            double TTA = combinedValue * resolution;
            this.TTA = TTA;
            return this.TTA;
        }
        public int GSstatusDecoding(byte[] data)
        {
            int GSstatus = (data[0] >> 0) & 0b00000001;
            this.GSstatus = GSstatus;
            return this.GSstatus;

        }
        public double GSDecoding(byte[] data)
        {
            byte gs1 = (byte)((data[0] >> 0) & 0b11111111);
            byte gs2 = (byte)((data[1] >> 6) & 0b00000011);

            int combinedValue = (gs1 << 2) | gs2;

            double resolution = (double)(1024 / 512); // Resolución en kt
            double gs = combinedValue * resolution;
            this.GS = gs;
            return this.GS;
        }
        public int TARstatusDecoding(byte[] data)
        {
            int TARstatus = (data[0] >> 5) & 0b00000001;
            this.TARstatus = TARstatus;
            return this.TARstatus;

        }
        public double TARDecoding(byte[] data)
        {
            byte TAR1 = (byte)((data[0] >> 0) & 0b00011111);
            byte TAR2 = (byte)((data[1] >> 3) & 0b00011111);
            byte signByte = (byte)((data[0] >> 4) & 0b00000001);

            int combinedValue;
            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (TAR1 << 5) | TAR2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((TAR1 << 5) | TAR2);
                combinedValue = -(~combinedValue + 1);
            }
            double resolution = (double)(8 / 256); //resolución en grados/s
            double TAR = combinedValue * resolution;
            this.TAR = TAR;
            return this.TAR;
        }
        public int TASstatusDecoding(byte[] data)
        {
            int TASstatus = (data[0] >> 2) & 0b00000001;
            this.TASstatus = TASstatus;
            return this.TASstatus;

        }
        public double TASDecoding(byte[] data)
        {
            byte tas1 = (byte)((data[0] >> 0) & 0b00000011);
            byte tas2 = (byte)((data[1] >> 0) & 0b11111111);

            int combinedValue = (tas1 << 8) | tas2;

            double resolution = 2; // Resolución en kt
            double tas = combinedValue * resolution;
            this.TAS = tas;
            return this.TAS;

        }
        public int HDGstatusDecoding(byte[] data)
        {
            int HDGstatus = (data[0] >> 7) & 0b00000001;
            this.HDGstatus = HDGstatus;
            return this.HDGstatus;

        }
        public double HDGDecoding(byte[] data)
        {
            byte HDG1 = (byte)((data[0] >> 0) & 0b01111111);
            byte HDG2 = (byte)((data[1] >> 4) & 0b00001111);
            byte signByte = (byte)((data[0] >> 6) & 0b00000001);

            int combinedValue;
            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (HDG1 << 4) | HDG2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((HDG1 << 4) | HDG2);
                combinedValue = -(~combinedValue + 1);
            }
            double resolution = (double)(90 / 512); //resolución en grados/s
            double HDG = combinedValue * resolution;
            this.HDG = HDG;
            return this.HDG;

        }
        public int IASstatusDecoding(byte[] data)
        {
            int IASstatus = (data[0] >> 3) & 0b00000001;
            this.IASstatus = IASstatus;
            return this.IASstatus;

        }
        public double IASDecoding(byte[] data)
        {
            byte ias1 = (byte)((data[0] >> 0) & 0b00000111);
            byte ias2 = (byte)((data[1] >> 1) & 0b01111111);

            int combinedValue = (ias1 << 7) | ias2;

            double resolution = 1; // Resolución en kt
            double ias = combinedValue * resolution;
            this.IAS = ias;
            return this.IAS;
        }
        public int MACHstatusDecoding(byte[] data)
        {
            int MACHstatus = (data[0] >> 0) & 0b00000001;
            this.MACHstatus = MACHstatus;
            return this.MACHstatus;

        }
        public double MACHDecoding(byte[] data)
        {
            byte mach1 = (byte)((data[0] >> 0) & 0b11111111);
            byte mach2 = (byte)((data[1] >> 6) & 0b00000011);

            int combinedValue = (mach1 << 2) | mach2;

            double resolution = 2048.0/512.0; // Resolución en kt
            double mach = combinedValue * resolution;
            this.MACH = mach;
            return this.MACH;
        }
        public int BARstatusDecoding(byte[] data)
        {
            int BARstatus = (data[0] >> 5) & 0b00000001;
            this.BARstatus = BARstatus;
            return this.BARstatus;

        }
        public double BARDecoding(byte[] data)
        {
            byte BAR1 = (byte)((data[0] >> 0) & 0b00011111);
            byte BAR2 = (byte)((data[1] >> 3) & 0b00011111);
            byte signByte = (byte)((data[0] >> 4) & 0b00000001);

            int combinedValue;
            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (BAR1 << 5) | BAR2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((BAR1 << 5) | BAR2);
                combinedValue = -(~combinedValue + 1);
            }
            double resolution = (double)(32.0); //resolución en ft/min
            double BAR = combinedValue * resolution;
            this.BAR = BAR;
            return this.BAR;
        }
        public int IVVstatusDecoding(byte[] data)
        {
            int IVVstatus = (data[0] >> 2) & 0b00000001;
            this.IVVstatus = IVVstatus;
            return this.IVVstatus;
        }
        public double IVVDecoding(byte[] data)
        {
            byte IVV1 = (byte)((data[0] >> 0) & 0b00000011);
            byte IVV2 = (byte)((data[1] >> 0) & 0b11111111);
            byte signByte = (byte)((data[0] >> 1) & 0b00000001);

            int combinedValue;
            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (IVV1 << 8) | IVV2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((IVV1 << 8) | IVV2);
                combinedValue = -(~combinedValue + 1);
            }
            double resolution = (double)(32.0); //resolución en ft/min
            double IVV = combinedValue * resolution;
            this.IVV = IVV;
            return this.IVV;
        }
        public int tracknumberDecoding(byte[] data)
        {
            byte track1 = (byte)((data[0] >> 0) & 0b00001111);
            byte track2 = (byte)((data[1] >> 0) & 0b11111111);
            double resolution = 1;
            int combinedValue;
            combinedValue = (track1 << 8) | track2;
            double tracknumber = combinedValue * resolution;
            this.tracknumber = tracknumber;
            return this.tracknumber;
        }
        public double x_componentDecoding(byte[] data)
        {
            byte x1 = (byte)((data[0] >> 0) & 0b11111111);
            byte x2 = (byte)((data[1] >> 0) & 0b11111111);
            byte signByte = (byte)((data[0] >> 7) & 0b00000001); 

            int combinedValue;

            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (x1 << 8) | x2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((x1 << 8) | x2);
                combinedValue = -(~combinedValue + 1);
            }

            double resolution = (double)(1 / 128.0); // Resolución en NM
            double x = combinedValue * resolution;
            this.x_component = x;
            return this.x_component;
        }
        public double y_componentDecoding(byte[] data)
        {
            byte y1 = (byte)((data[0] >> 0) & 0b11111111);
            byte y2 = (byte)((data[1] >> 0) & 0b11111111);
            byte signByte = (byte)((data[0] >> 7) & 0b00000001);

            int combinedValue;

            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (y1 << 8) | y2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((y1 << 8) | y2);
                combinedValue = -(~combinedValue + 1);
            }

            double resolution = (double)(1 / 128.0); // Resolución en NM
            double y = combinedValue * resolution;
            this.y_component = y;
            return this.y_component;
        }

        public double PolarRhoDecoding(byte[] data)
        {
            byte[] rhovec = new byte[] { data[0], data[1] }; //se cogen 16 bits
            double resolutionLSB = 1.0 / Math.Pow(2, 8);
            this.rho = ByteToDoubleDecoding(rhovec, resolutionLSB);
            return this.rho;
        }

        public double PolarThetaDecoding(byte[] data)
        {
            byte[] thetavec = new byte[] { data[2], data[3] }; //se cogen 16 bits
            double resolutionLSB = 360.0 / Math.Pow(2, 16);
            this.theta = ByteToDoubleDecoding(thetavec, resolutionLSB);
            return this.theta;
        }

        public double groundspeedpolarDecoding(byte[] data)
        {
     
            int combinedValue = (data[0] << 8) | data[1];
            bool isNegative = (combinedValue & 0x8000) != 0;
            if (isNegative)
            {
                combinedValue = -(~combinedValue + 1);
            }

            double resolution = Math.Pow(2, -14) * 3600; // Resolución en NM
            this.groundspeedpolar = combinedValue * resolution;

            return this.groundspeedpolar;
        }

        public double headingpolarDecoding(byte[] data)
        {
            int combinedValue = (data[2] << 8) | data[3];
            bool isNegative = (combinedValue & 0x8000) != 0;
            if (isNegative)
            {
                combinedValue = -(~combinedValue + 1);
            }
            double resolution = 360.0 / Math.Pow(2, 16); // Resolución en grados
            this.headingpolar = combinedValue * resolution;

            return this.headingpolar;
        }

        public string CNFDecoding(byte[] data)
        {
            int cnf = (data[0] >> 7) & 0b00000001;
            Dictionary<int, string> cnfDescriptions = new Dictionary<int, string>()
        {
            { 0, "Confirmed Track" },
            { 1, "Tentative Track" }
        };
            return cnfDescriptions.ContainsKey(cnf) ? cnfDescriptions[cnf] : "";

        }
        public string RADDecoding(byte[] data)
        {
            int rad = (data[0] >> 5) & 0b00000011;
            Dictionary<int, string> radDescriptions = new Dictionary<int, string>()
        {
            { 0, "Combined Track" },
            { 1, "PSR Track" },
            { 2, "SSR/Mode S Track" },
            { 3, "Invalid" }
        };
            return radDescriptions.ContainsKey(rad) ? radDescriptions[rad] : "";
        }
        public string DOUDecoding(byte[] data)
        {
            int dou = (data[0] >> 4) & 0b00000001;
            Dictionary<int, string> douDescriptions = new Dictionary<int, string>()
        {
            { 0, "Normal confidence" },
            { 1, "Low confidence in plot to track association" }
        };
            return douDescriptions.ContainsKey(dou) ? douDescriptions[dou] : "";

        }
        public string MAHDecoding(byte[] data)
        {
            int mah = (data[0] >> 3) & 0b00000001;
            Dictionary<int, string> mahDescriptions = new Dictionary<int, string>()
        {
            { 0, "No horizontal man.sensed" },
            { 1, "Horizontal man. sensed" }
        };
            return mahDescriptions.ContainsKey(mah) ? mahDescriptions[mah] : "";

        }
        public string CDMDecoding(byte[] data)
        {
            int cdm = (data[0] >> 1) & 0b00000011;
            Dictionary<int, string> cdmDescriptions = new Dictionary<int, string>()
        {
            { 0, "Maintaining" },
            { 1, "Climbing" },
            { 2, "Descending" },
            { 3, "Unknown" }
        };
            return cdmDescriptions.ContainsKey(cdm) ? cdmDescriptions[cdm] : "";
        }
        public string TREDecoding(byte[] data)
        {
            return this.TRE;

        }
        public string GHODecoding(byte[] data)
        {
            return this.GHO;

        }
        public string SUPDecoding(byte[] data)
        {
            return this.SUP;

        }
        public string TCCDecoding(byte[] data)
        {
            return this.TCC;

        }
        public double measuredheightDecoding(byte[] data)
        {
            byte h1 = (byte)((data[0] >> 0) & 0b00111111);
            byte h2 = (byte)((data[1] >> 0) & 0b11111111);
            byte signByte = (byte)((data[0] >> 5) & 0b00000001);

            int combinedValue;

            if (signByte == 0)
            {
                // Valor positivo
                combinedValue = (h1 << 8) | h2;
            }
            else
            {
                // Valor negativo
                combinedValue = ((h1 << 8) | h2);
                combinedValue = -(~combinedValue + 1);
            }

            double resolution = (double)(25.0); // Resolución en ft
            double h = combinedValue * resolution;
            this.measuredheight = h;
            return this.measuredheight;
        }
        public string COMDecoding(byte[] data)
        {
            int com = (data[0] >> 5) & 0b00000111;
            Dictionary<int, string> comDescriptions = new Dictionary<int, string>()
        {
            { 0, "No communications capability (surveillance only)" },
            { 1, "Comm. A and Comm. B capability" },
            { 2, "Comm. A, Comm. B and Uplink ELM" },
            { 3, "Comm. A, Comm. B, Uplink ELM and Downlink ELM" },
            { 4, "Level 5 Transponder capability" },
            { 5, "Not assigned" },
            { 6, "Not assigned" },
            { 7, "Not assigned" }
        };
            return comDescriptions.ContainsKey(com) ? comDescriptions[com] : "";
        }
        public string STATDecoding(byte[] data)
        {
            int stat = (data[0] >> 2) & 0b00000111;
            Dictionary<int, string> statDescriptions = new Dictionary<int, string>()
        {
            { 0, "No alert, no SPI, aircraft airborne" },
            { 1, "No alert, no SPI, aircraft on ground" },
            { 2, "Alert, no SPI, aircraft airborne" },
            { 3, "Alert, no SPI, aircraft on ground" },
            { 4, "Alert, SPI, aircraft airborne or on ground" },
            { 5, "No alert, SPI, aircraft airborne or on ground" },
            { 6, "Not assigned" },
            { 7, "Unknown" }
        };
            return statDescriptions.ContainsKey(stat) ? statDescriptions[stat] : "";
        }
        public string SIDecoding(byte[] data)
        {
            int si = (data[0] >> 1) & 0b00000001;
            Dictionary<int, string> siDescriptions = new Dictionary<int, string>()
        {
            { 0, "SI-Code Capable" },
            { 1, "II-Code Capable" }
        };
            return siDescriptions.ContainsKey(si) ? siDescriptions[si] : "";
        }
        public string MSCCDecoding(byte[] data)
        {
            int mscc = (data[1] >> 7) & 0b00000001;
            Dictionary<int, string> msccDescriptions = new Dictionary<int, string>()
        {
            { 0, "No" },
            { 1, "Yes" }
        };
            return msccDescriptions.ContainsKey(mscc) ? msccDescriptions[mscc] : "";
        }
        public string ARCDecoding(byte[] data)
        {
            int arc = (data[1] >> 6) & 0b00000001;
            Dictionary<int, string> arcDescriptions = new Dictionary<int, string>()
        {
            { 0, "100 ft resolution" },
            { 1, "25 ft resolution" }
        };
            return arcDescriptions.ContainsKey(arc) ? arcDescriptions[arc] : "";
        }
        public string AICDecoding(byte[] data)
        {
            int aic = (data[1] >> 5) & 0b00000001;
            Dictionary<int, string> aicDescriptions = new Dictionary<int, string>()
        {
            { 0, "No" },
            { 1, "Yes" }
        };
            return aicDescriptions.ContainsKey(aic) ? aicDescriptions[aic] : "";
        }
        public string B1ADecoding(byte[] data)
        {
            int b1a = (data[1] >> 4) & 0b00000001;
            Dictionary<int, string> b1aDescriptions = new Dictionary<int, string>()
        {
            { 0, "BDS 1,0 bit 16 = 0" },
            { 1, "BDS 1,0 bit 16 = 1" }
        };
            return b1aDescriptions.ContainsKey(b1a) ? b1aDescriptions[b1a] : "";
        }
        public string B1BDecoding(byte[] data)
        {
            int B1B = (data[1] >> 0) & 0b00001111;
            this.B1B= "BDS 1,0 bits 37/40 = " + Convert.ToString(B1B, 2).PadLeft(4, '0');
            return this.B1B;
        }
               

    }
























    }