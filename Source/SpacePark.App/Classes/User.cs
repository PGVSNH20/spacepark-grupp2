using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePark.App.Classes
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}
