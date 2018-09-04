using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CQCM
{
    /// <summary>
    /// Excel转dt
    /// </summary>
    public class ExcelToDt
    {
        public string filePath = "";

        private IWorkbook inWorkbook = null;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="filePath"></param>
        public ExcelToDt(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            this.filePath = filePath;
        }

        public ExcelToDt()
        {

        }

        /// <summary>
        /// 获取Excel一个sheet页的数据
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public DataTable Translate(string sheetName)
        {
            GetWorkbook();
            ISheet sheet = null;
            sheet = inWorkbook.GetSheet(sheetName);

            if (sheet == null)
            {
                throw new Exception();
            }

            IRow headRow = sheet.GetRow(0);

            DataTable outDt = new DataTable();

            //存储表头
            for (int i = headRow.FirstCellNum; i < headRow.LastCellNum; i++)
            {
                if (string.IsNullOrWhiteSpace(headRow.Cells[i].ToString()))
                    break;

                outDt.Columns.Add(headRow.Cells[i].StringCellValue.Trim());
            }

            //"需求"的列号
            int Index = GetCounterIndex(outDt.Columns);

            //遍历行数据
            for (int i = sheet.FirstRowNum+1; i <= sheet.LastRowNum; i++)
            {
                IRow tmpRow = sheet.GetRow(i);
                //空行则停止数据读取
                if (tmpRow == null || string.IsNullOrWhiteSpace(tmpRow.GetCell(0).StringCellValue))
                {
                    break;
                }
                //old为不需要处理的数据行
                if (!CheckValue(tmpRow,Index))
                {
                    continue;
                }
                else
                {
                    DataRow row = GetRow(outDt, tmpRow);
                    outDt.Rows.Add(row);
                }
                
            }
            return outDt;
        }

        /// <summary>
        /// 得到Excel的一行数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tmpRow"></param>
        /// <returns></returns>
        public DataRow GetRow(DataTable dt, IRow tmpRow)
        {
            DataRow row = dt.NewRow();

            for (int r = 0; r < dt.Columns.Count; r++)
            {
                ICell cell = tmpRow.GetCell(r);
                row[r] = GetCell(cell);
            }
            return row;
        }

        /// <summary>
        /// 获取Excel一个单元格数据
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public string GetCell(ICell cell)
        {
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue.Trim();
                    case CellType.Numeric:
                        return cell.NumericCellValue.ToString().Trim();
                    case CellType.Boolean:
                        return cell.BooleanCellValue.ToString().Trim();
                    case CellType.Error:
                        return cell.ToString().Trim();
                    default:
                        return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 解析Excel
        /// </summary>
        public void GetWorkbook()
        {
            string extension = Path.GetExtension(filePath);

            try
            {
                using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    inWorkbook = extension.Equals(".xls") ? (IWorkbook)new HSSFWorkbook(fs) : (IWorkbook)new XSSFWorkbook(fs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取一个指标列索引值
        /// </summary>
        /// <param name="column">列头</param>
        /// <returns>索引值</returns>
        public virtual int GetCounterIndex(DataColumnCollection column)
        {
            return 1;
        }

        /// <summary>
        /// 判断一个值是否满足要求
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual bool CheckValue(IRow row, int index)
        {
            return false;
        }
    }

    /// <summary>
    /// 解析database input
    /// </summary>
    public class ParseDatabase : ExcelToDt
    {
        public static List<string> siteLst = new List<string>();

        public ParseDatabase(string filePath) : base(filePath) { }

        public ParseDatabase() { }

        public override int GetCounterIndex(DataColumnCollection column)
        {
            return column.IndexOf(StaticField.site) < 0 ? 1 : column.IndexOf(StaticField.site);
        }

        public override bool CheckValue(IRow row, int index)
        {           
            return siteLst.Contains(GetCell(row.GetCell(index)).Trim());
        }

        public int GetCellIndex(DataColumnCollection column)
        {
            return column.IndexOf(StaticField.cell) < 0 ? 12 : column.IndexOf(StaticField.cell);
        }
    }

    /// <summary>
    /// 解析扩容站点
    /// </summary>
    public class ParseCMSite : ExcelToDt
    {
        public ParseCMSite(string filePath) : base(filePath) { }

        public ParseCMSite() { }

        public override int GetCounterIndex(DataColumnCollection column)
        {
            return column.IndexOf(StaticField.require) < 0 ? 3 : column.IndexOf(StaticField.require);
        }

        public override bool CheckValue(IRow row, int index)
        {
            return row.GetCell(index).StringCellValue.Contains(StaticField.twoCarrier) ||
                row.GetCell(index).StringCellValue.Contains(StaticField.threeCarrier);
        }

        public int GetCellIndex(DataColumnCollection column)
        {
            return column.IndexOf(StaticField.cell) < 0 ? 2 : column.IndexOf(StaticField.cell);
        }

        /// <summary>
        /// 获取待扩容的site
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<string> GetSite(DataTable dt)
        {
            int cellIndex = GetCellIndex(dt.Columns);
            List<string> cmSite = new List<string>();
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string cell = dt.Rows[i][cellIndex].ToString();
                string pattern = @"([a-z|A-Z]+[0-9]+)[A-Z|a-z]*";
                var siteGroup = Regex.Match(cell, pattern).Groups;
                cmSite.Add(siteGroup[1].Value);
            }
            return cmSite.Distinct().ToList();
        }
    }

    public class StaticField
    {
        public static string site = "SITE";
        public static string cell = "CELL";
        public static string require = "需求";
        public static string enbId = "ENBID";
        public static string twoCarrier = "双载波";
        public static string threeCarrier = "三载波";
        public static string cellId = "小区ID";
        public static string earfcndl = "earfcndl";
        public static string earfcnul = "earfcnul";
        public static string neighbor = "neighbor";
    }
}
