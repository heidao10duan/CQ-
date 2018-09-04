using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace CQCM
{
    public class SaveExcel
    {
        private IWorkbook inWorkbook = null;

        private string filePath = "";

        public SaveExcel(string filePath)
        {
            this.filePath = filePath;
            
        }

        /// <summary>
        /// 解决保存文件存在出错的问题
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="outSheetName"></param>
        /// <param name="dt1"></param>
        /// <param name="outSheetName1"></param>
        public void SaveToExcel(DataTable dt, string outSheetName, DataTable dt1, string outSheetName1)
        {
            GetWorkbook();
            DtToExcel(dt, outSheetName);
            DtToExcel(dt1, outSheetName1);
        }

        /// <summary>
        /// 输出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="outSheetName">sheet名称</param>
        public void DtToExcel(DataTable dt, string outSheetName)
        {
            ISheet sheet = GetSheet(outSheetName);            
            int colNum = dt.Columns.Count;
            //输出表头
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < colNum; i++)
            {
                row.CreateCell(i).SetCellValue(dt.Columns[i].ToString());
            }

            //输出行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow newRow = sheet.CreateRow(i+1);

                for (int j = 0; j < colNum; j++)
                {
                    if (IsNumeric(dt.Rows[i][j].ToString()))
                    {
                        newRow.CreateCell(j).SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                    }
                    else
                    {
                        newRow.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
            }

            using (FileStream fs = File.OpenWrite(filePath))
            {
                inWorkbook.Write(fs);
            }
        }

        /// <summary>
        /// 判断sheet是否存在于Excel中
        /// </summary>
        /// <param name="outSheetName">sheet名称</param>
        /// <returns>sheet</returns>
        public ISheet GetSheet(string outSheetName)
        {
            try
            {
                return inWorkbook.CreateSheet(outSheetName);
            }
            catch
            {
                return inWorkbook.GetSheet(outSheetName);
            }
        }

        /// <summary>
        /// 判断字符串是否为数字字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?(0|[1-9]\d*[.]?\d*)$");
        }

        /// <summary>
        /// 解析Excel
        /// </summary>
        public void GetWorkbook()
        {
            string extension = Path.GetExtension(filePath);
            inWorkbook = extension.Equals(".xls") ? (IWorkbook)new HSSFWorkbook() : (IWorkbook)new XSSFWorkbook();
        }
    }
}
