using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQCM;
using System.Data;
using NUnit.Framework;
using System.IO;

namespace TestExcelToDt
{
    [TestFixture]
    public class TestExcelToDt
    {
        public ParseCMSite excel = null;
        public string excelPath = @"C:\Users\25456\Desktop\CQPDC_CQCM扩容.xlsx";

        [SetUp]
        public void TestSetUp()
        {
            excel = new ParseCMSite(excelPath);
        }

        /// <summary>
        /// 文件不存在异常
        /// </summary>
        [Test]
        public void TestFileNotExist()
        {
            string excelPath = "";
            var ex = Assert.Catch<Exception>(() => new ExcelToDt(excelPath));
            StringAssert.Contains("无法找到指定文件", ex.Message);
        }

        [Test]
        [Ignore("文件没有打开不用测试")]
        public void TestFileOpened()
        {
            var ex = Assert.Catch<Exception>(() => excel.GetWorkbook());
            StringAssert.Contains("另一进程使用", ex.Message);
        }

       [Test]
       public void TestSheetNotExist()
        {
            DataTable dt = excel.Translate("扩容站点");
            Assert.AreEqual(13, dt.Rows.Count);
        }

        [Test]
        public void TestGetEnbId()
        {
            ParseCMSite siteExcel = new ParseCMSite(excelPath);
            DataTable dt = siteExcel.Translate("扩容站点");
            Assert.IsTrue(dt != null && dt.Rows.Count > 1);
            var cmEnbId = siteExcel.GetSite(dt);
            Assert.AreEqual(4, cmEnbId.Count());
            Assert.IsTrue(cmEnbId.Contains("CQCMLBB159403"));
            Assert.IsTrue(cmEnbId.Contains("CQCMLTL898616"));
        }

        [Test]
        public void TestParseDatabase()
        {
            ParseCMSite siteExcel = new ParseCMSite(excelPath);
            DataTable dt = siteExcel.Translate("扩容站点");
            ParseDatabase.siteLst = siteExcel.GetSite(dt);

            ParseDatabase db = new ParseDatabase(excelPath);
            DataTable dbDt = db.Translate("database input");
            Assert.IsTrue(dbDt.Columns.Contains("电下倾角"));

            List<string> ls = new List<string>(); //存放你一整列所有的值

            foreach (DataRow dr in dt.Rows)
            {               
                ls.Add(dr["SITE"].ToString());
            }
            Assert.IsFalse(ls.Contains("CQCMLBB155718"));
            Assert.IsTrue(ls.Contains("CQCMLBB155924"));
            Assert.AreEqual(11, dbDt.Rows.Count);
        }

        [Test]
        public void TestDbCount()
        {
            ParseDatabase db = new ParseDatabase(excelPath);
            DataTable dbDt = db.Translate("database input");
        }
    }
}
