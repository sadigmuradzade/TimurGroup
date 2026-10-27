using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    class JuridicalFounderDAL
    {
        public static DataSet SelectJuridicalFounder(long id)
        {
            string sql = $@"SELECT CA.NAME,
                                   CA.PINCODE,
                                   CA.DOCUMENT_NUMBER,
                                   CA.DOCUMENT_ISSUE_DATE,
                                   CA.DOCUMENT_RELIABLE_DATE,
                                   DC.NAME DOCUMENT_COUNTRY_NAME,
                                   CA.REGISTRATION_ADDRESS,
                                   CA.FOUNDER_SHARE,
                                   CA.VOEN,
                                   CA.BIRTHDAY,
                                   B.NAME BIRTHPLACE_NAME,
                                   C.NAME COUNTRY_NAME,       
                                   CA.IS_LOCAL_POLITICAL_FIGURE,
                                   PDL.NAME LOCAL_POLITICAL_DUTY_NAME,
                                   CA.LOCAL_POLITICAL_START_DATE,
                                   CA.LOCAL_POLITICAL_END_DATE,
                                   CA.IS_FOREIGN_POLITICAL_FIGURE,
                                   PDF.NAME FOREIGN_POLITICAL_DUTY_NAME,
                                   CA.FOREIGN_POLITICAL_START_DATE,
                                   CA.FOREIGN_POLITICAL_END_DATE,
                                   CA.IS_RELATIVE_POLITICAL_FIGURE,
                                   PDR.NAME RELATIVE_POLITICAL_DUTY_NAME,
                                   CA.RELATIVE_NAME,
                                   CA.RELATIVE_POLITICAL_START_DATE,
                                   CA.RELATIVE_POLITICAL_END_DATE,
                                   CB.NAME CUSTOMER_BEHAVIOR_NAME,
                                   CIM.NAME CUSTOMER_IDENTIFICATION_NAME,
                                   CA.IS_SANCTION_CHECK,
                                   CA.SANCTION_CHECK_URL,
                                   CA.NOTE
                              FROM CRS_USER_TEMP.JURIDICAL_AML_FOUNDER_TEMP CA,
                                   CRS_USER.COUNTRIES C,
                                   CRS_USER.COUNTRIES DC,
                                   CRS_USER.BIRTHPLACE B,
                                   CRS_USER.POLITICAL_DUTY PDL,
                                   CRS_USER.POLITICAL_DUTY PDF,
                                   CRS_USER.POLITICAL_DUTY PDR,
                                   CRS_USER.CUSTOMER_BEHAVIOR CB,
                                   CRS_USER.CUSTOMER_IDENTIFICATION_METHOD CIM
                             WHERE     CA.COUNTRY_ID = C.ID
                                   AND CA.DOCUMENT_COUNTRY_ID = DC.ID
                                   AND CA.BIRTHPLACE_ID = B.ID(+)
                                   AND CA.LOCAL_POLITICAL_DUTY_ID = PDL.ID(+)
                                   AND CA.FOREIGN_POLITICAL_DUTY_ID = PDF.ID(+)
                                   AND CA.RELATIVE_POLITICAL_DUTY_ID = PDR.ID(+)
                                   AND CA.CUSTOMER_BEHAVIOR_ID = CB.ID(+)
                                   AND CA.CUSTOMER_IDENTIFICATION_ID = CIM.ID(+)
                                AND CA.ID = {id}";

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
                GlobalProcedures.LogWrite("Hüquqi şəxsin təsisçilərinin məlumatları açılmadı.", sql, GlobalVariables.V_UserName, "JuridicalFounderDAL", "SelectJuridicalFounder", exx);
                return null;
            }
        }
    }
}
