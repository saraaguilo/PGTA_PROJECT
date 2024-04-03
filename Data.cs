using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace OurClasses
{
    public class Data
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public MessageDetails messages { get; set; }
    }
    
    public class MessageDetails
    {
        public Messages messages { get; set; }
        public DataSourceIdentifier dataSourceIdentifier { get; set; }
        public DateTime timeOfDay { get; set; } //string?
        public TargetReportDescriptor targetReportDescriptor { get; set; }
        public PolarCoordinates measuredPositionSlantPolarCoordinates { get; set; }
        public Mode3ACodeOctalRepresentation mode3ACodeinOctalRepresentation { get; set; }
        public FlightLevelBinaryRepresentation flightLevelBinaryRepresentation { get; set; }
        public RadarPlotCharacteristics radarPlotCharacteristics { get; set; }
        public string aircraftAddress { get; set; }
        public string[] aircraftIdentification { get; set; }
        public BDSRegisterData BDSRegisterData { get; set; }//poner?
        public BDSCode4 modeBDS4 { get; set; } 
        public BDSCode5 modeBDS5 { get; set; }
        public BDSCode6 modeBDS6 { get; set; }
        public int trackNumber { get; set; }
        public CartesianCoordinates calculatedPositionCartesianCoordinates { get; set; }
        public PolarCoordinates calculatedTrackVelocityPolarCoordinates { get; set; }
        public TrackStatus trackStatus { get; set; }
        public HeightMeasuredBy3DRad heightMeasuredBy3DRadar { get; set; }
        public CommunicationsACASCapabilityFlightStatus communicationsACASCapabilityFlightStatus { get; set; }
        public LLACoordinates calculatedPositionLLACoordinates { get; set; } //WGS84   
        public int modeCcorrected { get; set; } 
    }

    public class Messages
    {
        public string type { get; set; }
        public List<int> data { get; set; }
    }

    public class DataSourceIdentifier
    {
        public int SAC { get; set; }
        public int SIC { get; set; }
    }

    public class TargetReportDescriptor
    {
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
        string ADSB_EP { get; set; }
        string ADSB_VAL { get; set; }
        string SCN_EP { get; set; }
        string SCN_VAL { get; set; }
        string PAI_EP { get; set; }
        string PAI_VAL { get; set; }
        //string? SPARE { get; set; }
    }

    public class Mode3ACodeOctalRepresentation
    {
        public string V { get; set; }
        public string G { get; set; }
        public string L { get; set; }
        public string mode3A { get; set; }
    }

    public class FlightLevelBinaryRepresentation
    {
        public string V { get; set; }
        public string G { get; set; }
        public double FL { get; set; }
    }

    public class RadarPlotCharacteristics  //posar en strings?
    {
        public double SRL { get; set; }
        public double SSR { get; set; }
        public double SAM { get; set; }
        public double PRL { get; set; }
        public double PAM { get; set; }
        public double RPD { get; set; }
        public double APD { get; set; }

    }

    public class BDSRegisterData
    {
        public string fullmodeS { get; set; };//mirar-ho bé
        public BDSCode4 modeBDS4 { get; set; }
        public BDSCode5 modeBDS5 { get; set; }
        public BDSCode6 modeBDS6 { get; set; }

    }

    public class BDSCode4 //path and altitude report
    {
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

    }

    public class BDSCode5 //speed and track report
    {
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

    }

    public class BDSCode6 //ground referenced state vector
    {

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
    }

    public class CartesianCoordinates
    {
        public double x_component { get; set; }
        public double y_component { get; set; }
    }

    public class PolarCoordinates
    {
        public double rho { get; set; }
        public double theta { get; set; }
    }

    public class TrackStatus
    {
        public string CNF { get; set; }
        public string RAD { get; set; }
        public string DOU { get; set; }
        public string MAH { get; set; }
        public string CDM { get; set; }
        public string? TRE { get; set; }
        public string? GHO { get; set; }
        public string? SUP { get; set; }
        public string? TCC { get; set; }
    }

    public class HeightMeasuredBy3DRad
    {
        public double measuredheight { get; set; };
    }

    public class CommunicationsACASCapabilityFlightStatus
    {
        public string COM { get; set; }
        public string STAT { get; set; }
        public string SI { get; set; }
        public string MSCC { get; set; }
        public string ARC { get; set; }
        public string AIC { get; set; }
        public string B1A { get; set; }
        public string B1B { get; set; }
    }

    public class LLACoordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    
}











