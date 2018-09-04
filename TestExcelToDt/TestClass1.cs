using CQCM;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExcelToDt
{
    [TestFixture]
    public class TestClass1
    {
        public ParseCMSite excel = null;
        public string excelPath = @"C:\Users\25456\Desktop\CQPDC_CQCM扩容.xlsx";
        public bool result = false;

        [SetUp]
        public void TestSetUp()
        {
            excel = new ParseCMSite(excelPath);
        }

        [Test]
        public void Test()
        {
        }

        [Test]
        public void TestMain()
        {
        }
    }
}
