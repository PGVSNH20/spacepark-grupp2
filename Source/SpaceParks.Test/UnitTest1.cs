using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpacePark.App;
using SpacePark.App.Classes;

namespace SpaceParks.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            User user = new User("Luke");
            string output = user.UsersDestroyTheDarkSide();
            Assert.AreEqual("Luke", output);
        }
    }
}
