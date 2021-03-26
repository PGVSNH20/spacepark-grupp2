using System;

namespace SpacePark.Classes
{
    class ParkingSpot
    {
        public int ParkingSpotID { get; set; }
        public int ParkingID { get; set; }
        public int? UserID { get; set; }
        public string Vehicle { get; set; }
        public double VehicleLength { get; set; }
        public DateTime ParkingStarted { get; set; }

        public ParkingSpot(int parkingID, int? userID, string vehicle, double vehicleLength)
        {
            ParkingID = parkingID;
            UserID = userID;
            Vehicle = vehicle;
            VehicleLength = vehicleLength;
            ParkingStarted = DateTime.Now;
        }
    }
}
