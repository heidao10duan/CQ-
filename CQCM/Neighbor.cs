using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQCM
{
    public class Neighbor
    {
        public DataTable GetNeighbor(DataTable cmDt)
        {
            //构造自邻区的datatable
            DataTable neighborDt = CreateDt();

            //cmDt分组（site）
            var cellGroup = from row in cmDt.AsEnumerable()
                             group row.Field<string>(StaticField.cell)
                             by row.Field<string>(StaticField.site);

            if (cellGroup == null || cellGroup.Count() == 0)
            {
                return null;
            }

            GenerateNeighbor(ref neighborDt, cellGroup);

            return neighborDt;
        }

        /// <summary>
        /// 构造自邻区的datatable
        /// </summary>
        /// <returns></returns>
        public DataTable CreateDt()
        {
            //构造自邻区的datatable
            DataTable neighborDt = new DataTable();
            neighborDt.Columns.Add(StaticField.cell);
            neighborDt.Columns.Add(StaticField.neighbor);
            return neighborDt;
        }

        public void GenerateNeighbor(ref DataTable neighborDt, IEnumerable<IGrouping<string,string>> cellGroup)
        {
            foreach (var item in cellGroup)
            {
                List<string> cells = item.ToList();

                foreach (string cell in cells)
                {
                    foreach (string neighbor in cells)
                    {
                        if (cell != neighbor)
                        {
                            DataRow row = neighborDt.NewRow();
                            row[StaticField.cell] = cell;
                            row[StaticField.neighbor] = neighbor;
                            neighborDt.Rows.Add(row);
                        }
                    }
                }
            }
        }
    }
}
