using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SpacePark;
using SpacePark.Classes;
using System.Collections.Generic;
using System.Linq;

namespace SpaceParks.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LukeFinder()
        {
            User user = new User("Luke");
            string output = user.UsersDestroyTheDarkSide();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Luke", output);
        }

        [TestMethod]
        public void CheckIfCharacterExists()
        {
            NUnit.Framework.Assert.True(SpacePark.Program.FindPerson("Jabba Desilijic Tiure").Result);
        }

        [TestMethod]
        public void CheckIfStarshipExists()
        {
            List<SwStarship> FindShip = SpacePark.Program.FetchStarships().Result;
            NUnit.Framework.Assert.True(FindShip.Find(x => x.Model.Contains("YT-1300 light freighter")) != null);
        }
    }
}
