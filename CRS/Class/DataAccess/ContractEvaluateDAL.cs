using CRS.Class.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    class ContractEvaluateDAL
    {
        public static DataSet SelectContractEvaluate(int? id)
        {
            string sql = null;

            if (id.HasValue)
                sql = $@"SELECT ID,
                                   NAME,
                                   USED_USER_ID,
                                   COLOR
                              FROM CRS_USER.CONTRACT_EVALUATE
                              WHERE ID = {id}";
            else
                sql = $@"SELECT ID,
                                   NAME,
                                   USED_USER_ID,
                                   COLOR
                              FROM CRS_USER.CONTRACT_EVALUATE
                        ORDER BY ORDER_ID";

            try
            {
                using (OracleDataAdapter adapter = new OracleDataAdapter(sql, GlobalFunctions.GetConnectionString()))
                {
                    DataSet dsAdapter = new DataSet();
                    adapter.Fill(dsAdapter);
                    return dsAdapter;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müqavilənin qiymətləndirmələri açılmadı.", sql, GlobalVariables.V_UserName, "ContractEvaluateDAL", "SelectContractEvaluate", exx);
                return null;
            }
        }

        public static Color EvaluateColor(int evaluateID)
        {
            Color color = new Color();
            List<ContractEvaluate> contractEvaluates = SelectContractEvaluate(evaluateID).ToList<ContractEvaluate>();
            if (contractEvaluates.Count > 0)
                color = GlobalFunctions.CreateColor(contractEvaluates.LastOrDefault().COLOR);

            return color;
        }
    }
}
