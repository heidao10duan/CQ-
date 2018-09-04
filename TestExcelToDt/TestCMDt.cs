using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQCM;

namespace TestExcelToDt
{
    [TestFixture]
    public class TestCMDt
    {
        public string excelPath = @"C:\Users\25456\Desktop\CQPDC_CQCM扩容.xlsx";

        [Test]
        public void TestProducer()
        {
            CmProducer cm = new CmProducer();

            ParseCMSite siteExcel = new ParseCMSite(excelPath);
            DataTable dt = siteExcel.Translate("扩容站点");
            Assert.AreEqual(13, dt.Rows.Count);
                     
            ParseDatabase.siteLst = siteExcel.GetSite(dt);
            ParseDatabase db = new ParseDatabase(excelPath);
            DataTable dbDt = db.Translate("database input");

            DataTable cmDt = cm.Produce(ParseDatabase.siteLst, dbDt, dt);

            //Assert.AreEqual(null, cmDt);
            
            Assert.AreEqual(20, cmDt.Columns.Count);
            Assert.AreEqual(24, cmDt.Rows.Count);
            // TODO: Add your test code here
            //Assert.Pass("Your first passing test");
        }

        [Test]
        public void TestDbDt()
        {
            ParseCMSite siteExcel = new ParseCMSite(excelPath);
            DataTable dt = siteExcel.Translate("扩容站点");
        }
    }
}
