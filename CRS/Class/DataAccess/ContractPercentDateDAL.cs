using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class ContractPercentDateDAL
    {
        public static DataSet SelectContractPercentDate(int contractID)
        {
            string sql = $@"SELECT ID,CONTRACT_ID,START_DATE,END_DATE FROM CRS_USER.CONTRACT_PERCENT_DATE WHERE CONTRACT_ID = {contractID} ORDER BY ID";

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
                return null;
                GlobalProcedures.LogWrite("Müqavilənin faizinin hesablanmama tarixləri açılmadı.", sql, GlobalVariables.V_UserName, "ContractPercentDateDAL", "SelectIssuingByID", exx);
            }
        }
    }
}
