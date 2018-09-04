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
    public class TestNeighbor
    {
        public string excelPath = @"C:\Users\25456\Desktop\CQPDC_CQCM扩容.xlsx";

        [Test]
        public void TestNeighborCell()
        {
            CmProducer cm = new CmProducer();

            ParseCMSite siteExcel = new ParseCMSite(excelPath);
            DataTable dt = siteExcel.Translate("扩容站点");
            Assert.AreEqual(13, dt.Rows.Count);

            ParseDatabase.siteLst = siteExcel.GetSite(dt);
            ParseDatabase db = new ParseDatabase(excelPath);
            DataTable dbDt = db.Translate("database input");

            DataTable cmDt = cm.Produce(ParseDatabase.siteLst, dbDt, dt);

   

            Neighbor neighbor = new Neighbor();
            var result = neighbor.GetNeighbor(cmDt);

            //Assert.AreEqual(132, outDt.Rows.Count);
            // TODO: Add your test code here
        }
    }
}
