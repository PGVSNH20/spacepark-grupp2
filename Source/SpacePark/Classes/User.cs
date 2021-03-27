using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SpacePark.Test")]
namespace SpacePark.Classes
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
