using LogTool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogTool.Tests
{
    [TestClass]
    public class CalculateServiceTest
    {
        [DataTestMethod]
        [DataRow(1, 2, 3)]
        [DataRow(2, 3, 5)]
        [DataRow(3, 5, 8)]
        public void AddNumbersTest(int number1, int number2, int expectedResult)
        {
            CalculateService service = new CalculateService();

            var result = service.AddTwoNumbers(number1, number2);

            Assert.AreEqual(expectedResult, result);
        }
    }
}