using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogTool.Tests
{
    [TestClass]
    public class SampleTest
    {
        [TestMethod]
        public void BasicTest()
        {
            int i = 0;
            Assert.AreEqual(0, i);
        }
    }
}