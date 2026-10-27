using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CreditTypeFontDAL
    {
        public static DataSet SelectCreditTypeFontByID(int? id)
        {
            string sql = null;
            if (id == null)
                sql = $@"SELECT ID,
                               CREDIT_TYPE_ID,
                               FONTNAME,
                               FONTSIZE,
                               FONTSTYLE,
                               BACKCOLOR_A,
                               BACKCOLOR_R,
                               BACKCOLOR_G,
                               BACKCOLOR_B,
                               BACKCOLOR_TYPE,
                               BACKCOLOR_NAME,
                               FONTCOLOR_A,
                               FONTCOLOR_R,
                               FONTCOLOR_G,
                               FONTCOLOR_B,
                               FONTCOLOR_TYPE,
                               FONTCOLOR_NAME
                          FROM CRS_USER.CREDIT_TYPE_FONTS 
                            ORDER BY ID";
            else
                sql = $@"SELECT ID,
                               CREDIT_TYPE_ID,
                               FONTNAME,
                               FONTSIZE,
                               FONTSTYLE,
                               BACKCOLOR_A,
                               BACKCOLOR_R,
                               BACKCOLOR_G,
                               BACKCOLOR_B,
                               BACKCOLOR_TYPE,
                               BACKCOLOR_NAME,
                               FONTCOLOR_A,
                               FONTCOLOR_R,
                               FONTCOLOR_G,
                               FONTCOLOR_B,
                               FONTCOLOR_TYPE,
                               FONTCOLOR_NAME
                          FROM CRS_USER.CREDIT_TYPE_FONTS
                WHERE ID = {id}";

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
                GlobalProcedures.LogWrite("Kodun fontları açılmadı.", sql, GlobalVariables.V_UserName, "CreditTypeFontDAL", "SelectCreditTypeFontByID", exx);
            }
        }
    }
}
