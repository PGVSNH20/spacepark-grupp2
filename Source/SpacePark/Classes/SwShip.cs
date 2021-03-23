using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePark.App.Classes
{
    class SwShip
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Result
        {
            public string name { get; set; }
            public string model { get; set; }
            public string manufacturer { get; set; }
            public string cost_in_credits { get; set; }
            public string length { get; set; }
            public string max_atmosphering_speed { get; set; }
            public string crew { get; set; }
            public string passengers { get; set; }
            public string cargo_capacity { get; set; }
            public string consumables { get; set; }
            public string hyperdrive_rating { get; set; }
            public string MGLT { get; set; }
            public string starship_class { get; set; }
            public List<string> pilots { get; set; }
            public List<string> films { get; set; }
            public DateTime created { get; set; }
            public DateTime edited { get; set; }
            public string url { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public string next { get; set; }
            public object previous { get; set; }
            public List<Result> results { get; set; }
        }
    }
}
