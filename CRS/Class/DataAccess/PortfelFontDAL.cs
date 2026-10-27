using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class PortfelFontDAL
    {
        public static DataSet SelectPortfelFontByID(int? id)
        {
            string sql = null;
            if (id == null)
                sql = $@"SELECT ID,
                               START_VALUE,
                               END_VALUE,
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
                          FROM CRS_USER.PORTFEL_FONTS 
                            ORDER BY ID";
            else
                sql = $@"SELECT ID,
                               START_VALUE,
                               END_VALUE,FONTNAME,
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
                          FROM CRS_USER.PORTFEL_FONTS 
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
                GlobalProcedures.LogWrite("Portfelin rəngləri açılmadı.", sql, GlobalVariables.V_UserName, "PortfelFontDAL", "SelectPortfelFontByID", exx);
            }
        }
    }
}
