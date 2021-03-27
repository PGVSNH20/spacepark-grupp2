using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpacePark;
using SpacePark.Classes;

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
