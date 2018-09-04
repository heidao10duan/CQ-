using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQCM
{
    public class CmProducer
    {
        /// <summary>
        /// 输出扩容结果
        /// </summary>
        /// <param name="cmSite"></param>
        /// <param name="dbDt"></param>
        /// <param name="cmDt"></param>
        /// <returns></returns>
        public DataTable Produce(List<string> cmSite, DataTable dbDt, DataTable cmDt)
        {
            DataTable outDt = dbDt.Clone();

            foreach (var site in cmSite)
            {
                List<string> cellNum = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K","L"};
                foreach (var num in cellNum)
                {
                    //构造CELL
                    string cell = string.Format("{0}{1}", site, num);

                    //输出第一载波数据
                    var query = GetDbCarrier(dbDt,cell,"");

                    //没有待扩容数据，不输出，不扩容
                    if (query == null)
                    {
                        break;
                    }

                    outDt.Rows.Add(query[0].ItemArray);
                    //二载波扩容
                    GetTwoCarrierData(ref outDt, dbDt, cmDt, cell, query[0]);
                    //三载波扩容
                    GetThreeCarrierData(ref outDt, dbDt, cmDt, cell, query[0]);
                }


            }
            return outDt;
        }

        /// <summary>
        /// db数据中的
        /// </summary>
        /// <param name="dbDt"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public List<DataRow> GetDbCarrier(DataTable dbDt, string cell, string carrier)
        {
            var query = from row in dbDt.AsEnumerable()
                        where row[new ParseDatabase().GetCellIndex(dbDt.Columns)].Equals(cell + carrier)
                        select row;

            if (query == null || query.Count() == 0)
            {
                return null;
            }
            return query.ToList();
        }

        /// <summary>
        /// 二载波扩容
        /// </summary>
        /// <param name="outDt"></param>
        /// <param name="dbDt"></param>
        /// <param name="cmDt"></param>
        /// <param name="cell"></param>
        /// <param name="firstCarrier"></param>
        public void GetTwoCarrierData(ref DataTable outDt, DataTable dbDt, DataTable cmDt, string cell, DataRow firstCarrier)
        {
            //db输入数据检查二载波数据
            var dbTwoCarrier = GetDbCarrier(dbDt, cell, "1");

            if (dbTwoCarrier == null)
            {
                //扩容数据是否存在二载波扩容
                //二载波数据
                var cmTwoCarrier = from row in cmDt.AsEnumerable()
                                   where row[new ParseCMSite().GetCellIndex(cmDt.Columns)].ToString().Contains(cell)
                                   && row[new ParseCMSite().GetCounterIndex(cmDt.Columns)].ToString().Contains(StaticField.twoCarrier)
                                   select row;

                //检测原始数据有没有二载波数据
                if (cmTwoCarrier == null || cmTwoCarrier.Count() == 0) { }
                else
                {
                    DataRow newRow = CmCarrier(outDt, cell, firstCarrier, 2);
                    outDt.Rows.Add(newRow);
                }
            }
            else
            {
                outDt.Rows.Add(dbTwoCarrier[0].ItemArray);
            }
        }

        /// <summary>
        /// 三载波扩容
        /// </summary>
        /// <param name="outDt"></param>
        /// <param name="dbDt"></param>
        /// <param name="cmDt"></param>
        /// <param name="cell"></param>
        /// <param name="firstCarrier"></param>
        /// <param name="carrier"></param>
        public void GetThreeCarrierData(ref DataTable outDt, DataTable dbDt, DataTable cmDt, string cell, DataRow firstCarrier)
        {
            //db输入数据检查三载波数据
            var dbThreeCarrier = GetDbCarrier(dbDt,cell,"2");

            if (dbThreeCarrier == null)
            {
                //扩容数据是否存在三载波扩容
                var cmThreeCarrier = from row in cmDt.AsEnumerable()
                                     where row[new ParseCMSite().GetCellIndex(cmDt.Columns)].ToString().Contains(cell)
                                     && row[new ParseCMSite().GetCounterIndex(cmDt.Columns)].ToString().Contains(StaticField.threeCarrier)
                                     select row;

                //检测原始数据有没有三载波数据
                if (cmThreeCarrier == null || cmThreeCarrier.Count() == 0) { }
                else
                { 
                    //三载波扩容
                    DataRow newRow = CmCarrier(outDt, cell, firstCarrier,3);
                    outDt.Rows.Add(newRow);
                }
            }
            else
            {
                outDt.Rows.Add(dbThreeCarrier[0].ItemArray);
            }
        }

        /// <summary>
        /// 扩容后的数据
        /// </summary>
        /// <param name="outDt"></param>
        /// <param name="cell"></param>
        /// <param name="firstCarrier"></param>
        /// <returns></returns>
        public DataRow CmCarrier(DataTable outDt, string cell, DataRow firstCarrier, int carrier)
        {
            //三载波扩容
            DataRow newRow = outDt.NewRow();
            var colNames = outDt.Columns;

            for (int i = 0; i < outDt.Columns.Count; i++)
            {
                string colName = colNames[i].ToString();
                if (colName.Equals(StaticField.cell))
                {
                    newRow[i] = string.Format("{0}{1}", cell,carrier-1);
                }
                else if (colName.Equals(StaticField.cellId))
                {
                    newRow[i] = int.Parse(firstCarrier[i].ToString()) + (16*carrier);
                }
                else if (colName.Equals(StaticField.earfcndl) || colName.Equals(StaticField.earfcnul))
                {
                    string earfcndl = firstCarrier[i].ToString();
                    newRow[i] = carrier.Equals(3)? GetEarfcndl(earfcndl):GetEarfcnul(earfcndl);
                }
                else
                {
                    newRow[i] = firstCarrier[i].ToString();
                }
            }

            return newRow;
        }

        /// <summary>
        /// 根据第一载波earfcndl计算第三载波
        /// </summary>
        /// <param name="earfcndl">第一载波的值</param>
        /// <returns>第三载波的值</returns>
        public string GetEarfcndl(string earfcndl)
        {
            if (earfcndl.Equals("37900"))
            {
                return "40936";
            }
            else if (earfcndl.Equals("38950"))
            {
                return "39292";
            }
            else if (earfcndl.Equals("38400"))
            {
                return string.Empty;
            }
            else
            {
                return "请检查第一载波是否有问题";
            }
        }

        /// <summary>
        /// 根据第一载波earfcnul计算第二载波
        /// </summary>
        /// <param name="earfcnul"></param>
        /// <returns></returns>
        public string GetEarfcnul(string earfcnul)
        {
            if (earfcnul.Equals("37900"))
            {
                return "38098";
            }
            else if (earfcnul.Equals("38950"))
            {
                return "39148";
            }
            else if (earfcnul.Equals("38400"))
            {
                return "38544";
            }
            else
            {
                return "请检查第一载波是否有问题";
            }
        }
    }
}
