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
        string? TST { get; set; }
        string? ERR { get; set; }
        string? XPP { get; set; }
        string? ME { get; set; }
        string? MI { get; set; }
        string? FOE_FRI { get; set; }
        //string? ADSB_EP { get; set; }
        //string? ADSB_VAL { get; set; }
        //string? SCN_EP { get; set; }
        //string? SCN_VAL { get; set; }
        //string? PAI_EP { get; set; }
        //string? PAI_VAL { get; set; }
        //string? SPARE { get; set; }

        public string V { get; set; }
        public string G { get; set; }
        public string L { get; set; }
        public string mode3A { get; set; }

        public string V { get; set; }
        public string G { get; set; }
        public double FL { get; set; }

        public double SRL { get; set; }
        public double SSR { get; set; }
        public double SAM { get; set; }
        public double PRL { get; set; }
        public double PAM { get; set; }
        public double RPD { get; set; }
        public double APD { get; set; }

        public string fullmodeS { get; set; };//mirar-ho bé
        public BDSCode4 modeBDS4 { get; set; }
        public BDSCode5 modeBDS5 { get; set; }
        public BDSCode6 modeBDS6 { get; set; }

        public double MCPstatus { get; set; }
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

        int sim = (data[0] >> 4) & 0b00000001; //cuarto bit del array de data
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
        
        public string SIPDecoding(byte[] data)
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
        //ADSB????













    }