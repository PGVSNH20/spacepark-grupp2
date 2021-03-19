using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePark.App.Classes
{
    public class SwStarship
    {
        //public int StarshipID { get; set; }
        public string Model { get; set; }
        public double LengthInM { get; set; }

        public SwStarship(string model, double length)
        {
            Model = model;
            LengthInM = length;
        }
    }
}
