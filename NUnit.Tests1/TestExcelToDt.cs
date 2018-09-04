using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQCM;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            string filePath = "";
            ExcelToDt excel = new ExcelToDt(filePath);
            DataTable dt = excel.Translate();
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }
    }
}
