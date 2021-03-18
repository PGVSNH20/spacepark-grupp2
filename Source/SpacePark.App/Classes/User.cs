using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePark.App.Classes
{
    public class User
    {
        public string ForeName { get; set; }
        public string SurName { get; set; }

        public User(string foreName, string surName)
        {
            ForeName = foreName;
            SurName = surName;
        }
    }
}
