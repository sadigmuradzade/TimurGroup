using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace CRS.Class.DataAccess
{
    public class SosialStatusDAL
    {
        public static DataSet SelectSosialStatusByID(int? statusID)
        {
            string sql = null;
            if (!statusID.HasValue)
                sql = "SELECT ID,NAME,NOTE,IS_WORK_PLACE,ORDER_ID,USED_USER_ID FROM CRS_USER.SOCIAL_STATUS ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,NAME,NOTE,IS_WORK_PLACE,ORDER_ID,USED_USER_ID FROM CRS_USER.SOCIAL_STATUS WHERE ID = {statusID}";

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
                GlobalProcedures.LogWrite("Sosial statuslar açılmadı.", sql, GlobalVariables.V_UserName, "SosialStatusDAL", "SelectSosialStatusByID", exx);
                return null;
            }
        }
    }
}
