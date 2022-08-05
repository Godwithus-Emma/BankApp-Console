
using BankApp.Core;

namespace BankServicesUnitTests
{
    public class Tests
    {
        

        [Test]
        public void GetUserInputValid()
        {
            //string source = "100";
            int expected = 100;
            int actual = DataManager.GetInput<int>("enter a number"); //needed console!
            Assert.AreEqual(expected, actual);
        }
    }
}