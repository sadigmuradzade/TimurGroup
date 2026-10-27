using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CustomerPaymentsDAL
    {
        public static DataSet SelectCustomerLastPayment(int contractID)
        {
            string sql = $@"SELECT * FROM CRS_USER.V_LAST_CUSTOMER_PAYMENT WHERE CONTRACT_ID = {contractID}";

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
                GlobalProcedures.LogWrite("Müqavilənin sonuncu ödənişi açılmadı.", sql, GlobalVariables.V_UserName, "CustomerPaymentsDAL", "SelectCustomerLastPayment", exx);
                return null;
            }
        }
    }
}
