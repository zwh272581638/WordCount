using Microsoft.VisualStudio.TestTools.UnitTesting;
using wordCount;

namespace wordCount.Tests
{
    [TestClass()]
    public class ReadTests
    {
        [TestMethod()]
        public void Read1Test()
        {
            Read a = new Read();
            
            string ans = a.Read1(@"D:\wordCount\wordCount\wordCount\bin\Debug\wordCount.txt");
            Assert.Fail("as58",ans);
        }
    }
}