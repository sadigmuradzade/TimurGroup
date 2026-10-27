using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace CRS.Class.DataAccess
{
    class JuridicalAMLDAL
    {
        public static DataSet SelectCustomerAML(long customerID)
        {
            string sql = $@"SELECT RD.NAME RISK_DEGREE_NAME,
                                   OL.NAME ORGANIZATIONAL_LEGAL_NAME,
                                   CA.REGISTRATION_DATE,
                                   CA.AUTHORIZED_CAPITAL,
                                   CA.DRC_ISSUE_DATE,
                                   CA.EMPLOYEES_NUMBER,
                                   CA.BRANCH_COUNT,
                                   CA.VALUE_ASSETS,
                                   CA.OLD_COMPANY_NAME,
                                   PB.NAME PURPOSE_BUSINESS_NAME,
                                   CA.IS_SANCTION_CHECK,
                                   CA.SANCTION_CHECK_URL,
                                   CA.WEBSITE,
                                   CA.NOTE
                              FROM CRS_USER.JURIDICAL_PERSON_AML CA,
                                   CRS_USER.RISK_DEGREE RD,
                                   CRS_USER.PURPOSE_BUSINESS_RELATIONSHIP PB,
                                   CRS_USER.ORGANIZATIONAL_LEGAL OL
                             WHERE     CA.RISK_DEGREE_ID = RD.ID(+)
                                   AND CA.PURPOSE_BUSINESS_RELATIONSHIP = PB.ID(+)
                                   AND CA.ORGANIZATIONAL_LEGAL_ID = OL.ID(+)
                                AND CA.JURIDICAL_PERSON_ID = {customerID}";

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
                GlobalProcedures.LogWrite("AML məlumatları açılmadı.", sql, GlobalVariables.V_UserName, "CustomerAMLDAL", "SelectCustomerAML", exx);
                return null;
            }
        }
    }
}
