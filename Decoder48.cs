using System;
using System.Collections.Generic;
using System.Runtime.Remoting;


namespace OurClasses
{

    public class Decoder48
    {
        public DateTime timeOfDay { get; set; }

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
        public double FMstatus { get; set; }
        public double FMalt { get; set; }
        public double BPSstatus { get; set; }
        public double BPSpres { get; set; }
        public double modeStat { get; set; }
        public double VNAV { get; set; }
        public double ALThold { get; set; }
        public double App { get; set; } //approach
        public double targetalt_status { get; set; }
        public double targetalt_source { get; set; }

        public double RAstatus { get; set; }
        public double RA { get; set; } //roll angle
        public double TTAstatus { get; set; }
        public double TTA { get; set; } //true track angle
        public double GSstatus { get; set; }
        public double GS { get; set; } //ground speed
        public double TARstatus { get; set; }
        public double TAR { get; set; } //track angle rate
        public double TASstatus { get; set; }
        public double TAS { get; set; } //true airspeed

        public double HDGstatus { get; set; }
        public double HDG { get; set; }
        public double IASstatus { get; set; }
        public double IAS { get; set; }
        public double MACHstatus { get; set; }
        public double MACH { get; set; }
        public double BARstatus { get; set; }
        public double BAR { get; set; }
        public double IVVstatus { get; set; }
        public double IVV { get; set; }

        public double x_component { get; set; }
        public double y_component { get; set; }

        public double rho { get; set; }
        public double theta { get; set; }

        public string CNF { get; set; }
        public string RAD { get; set; }
        public string DOU { get; set; }
        public string MAH { get; set; }
        public string CDM { get; set; }
        public string? TRE { get; set; }
        public string? GHO { get; set; }
        public string? SUP { get; set; }
        public string? TCC { get; set; }

        public double measuredheight { get; set; };

        public string COM { get; set; }
        public string STAT { get; set; }
        public string SI { get; set; }
        public string MSCC { get; set; }
        public string ARC { get; set; }
        public string AIC { get; set; }
        public string B1A { get; set; }
        public string B1B { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }

        public TimeSpan decodeTimeDay(byte[] data)
        {
            double resolution = Math.Pow(2, -7); //in seconds
            double seconds = ByteToDoubleDecoding(data, resolution);
            this.TimeofDay = TimeSpan.FromSeconds(seconds);
            return this.TimeofDay;
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
            return this.rho;
        }

        




    














 }