using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePark.App.Classes
{
    class ParkingSpot
    {
        public int ParkingSpotID { get; set; }
        public int ParkingID { get; set; }
        public int? UserID { get; set; }
        public string Vehicle { get; set; }
        public double VehicleLength { get; set; }
        public DateTime ParkingStarted { get; set; }
    }
}
