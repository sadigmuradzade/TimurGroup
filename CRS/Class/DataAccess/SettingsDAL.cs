using CRS.Class.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class SettingsDAL
    {
        public static DataSet SelectSettings(int? id)
        {
            string sql = null;
            if (id == null)
                sql = "SELECT ID,SETTING_NAME,SETTING_VALUE FROM CRS_USER.CRS_SETTINGS ORDER BY SETTING_NAME";
            else
                sql = $@"SELECT ID,SETTING_NAME,SETTING_VALUE FROM CRS_USER.CRS_SETTINGS WHERE ID = {id}";

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
                GlobalProcedures.LogWrite("Settingslər açılmadı.", sql, GlobalVariables.V_UserName, "SettingsDAL", "SelectSettings", exx);
            }
        }

        public static string GetSetting(string settingName)
        {
            string res = null;

            List<Settings> lstSettings = SelectSettings(null).ToList<Settings>();
            if (lstSettings.Count > 0)
            {
                var setting = lstSettings.Where(item => item.SETTING_NAME == settingName);
                if (setting != null)
                    res = setting.LastOrDefault().SETTING_VALUE;
            }

            return res;
        }
    }
}
