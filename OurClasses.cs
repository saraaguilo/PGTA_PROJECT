using System;
using System.Collections.Generic;

namespace OurClasses
{
    public class Flight
    {
        string ID;
        List<Coordinates> coordinates;
        double altitude;
        TimeSpan flightduration;

        public Flight()
            {
            }
        public string getID
        {
            return this.ID;
        }
        public <Coordinates> getCoordinates
        {
            return this.coordinates;
        }
        public double altitude
            {
            return this.altitude;
        }
        public TimeSpan flighduration
        {
            return this.flighduration;
        }

}

    public class Coordinates
    {
        double latitude;
        double longitude;
        public Coordinates(double latitude, double longitude)
        {
              this.latitude = latitude;   
              this.longitude = longitude;
        }
   }

}

}


