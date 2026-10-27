using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    class GuaranteeDAL
    {
        public static DataSet SelectGuarantee(int istemp, int contractID)
        {
            string sql = null;

            if (istemp == 1)
                sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   CODE,
                                   CUSTOMER_NAME,
                                   NAME,
                                   SEX_ID,
                                   COUNTRY_ID,
                                   BIRTHDAY,
                                   BIRTHPLACE_ID,
                                   CARD_NUMBER,
                                   CARD_PINCODE,
                                   CARD_ISSUE_DATE,
                                   RELIABLE_DATE,
                                   CARD_ISSUING_ID,
                                   ADDRESS,
                                   REGISTRATION_ADDRESS,
                                   USED_USER_ID,
                                   NOTE,
                                   IS_CHANGE
                              FROM CRS_USER_TEMP.GUARANTEE_TEMP
                              WHERE CONTRACT_ID = {contractID}
                                AND USED_USER_ID = {GlobalVariables.V_UserID}
                        ORDER BY CODE,ID";
            else
                sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   CODE,
                                   CUSTOMER_NAME,
                                   NAME,
                                   SEX_ID,
                                   COUNTRY_ID,
                                   BIRTHDAY,
                                   BIRTHPLACE_ID,
                                   CARD_NUMBER,
                                   CARD_PINCODE,
                                   CARD_ISSUE_DATE,
                                   RELIABLE_DATE,
                                   CARD_ISSUING_ID,
                                   ADDRESS,
                                   REGISTRATION_ADDRESS,
                                   USED_USER_ID,
                                   NOTE
                              FROM CRS_USER.GUARANTEE
                              WHERE CONTRACT_ID = {contractID}
                        ORDER BY CODE, ID";

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
                GlobalProcedures.LogWrite("Zaminlər açılmadı.", sql, GlobalVariables.V_UserName, "GuaranteeDAL", "SelectGuarantee", exx);
                return null;
            }
        }
    }
}
