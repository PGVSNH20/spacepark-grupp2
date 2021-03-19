using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePark.App.Classes
{
    class Parking
    {
        public int ParkingID { get; set; }
        public double Length { get; set; }

        //Galactic Credits, rounded up to closest hour
        public double HourlyRatePerMeter { get; set; }

        //Need bigger budget for things like name and location
    }
}