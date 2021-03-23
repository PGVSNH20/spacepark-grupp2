using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("SpacePark.Test")]
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

        public string UsersDestroyTheDarkSide()
        {
            return this.Name;
        }
    }
}
