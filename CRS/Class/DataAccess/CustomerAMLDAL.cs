using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace CRS.Class.DataAccess
{
    public class CustomerAMLDAL
    {
        public static DataSet SelectCustomerAML(int customerID)
        {
            string sql = $@"SELECT RD.NAME RISK_DEGREE_NAME,
                                   SS.NAME SOCIAL_STATUS_NAME,
                                   P.NAME PROFESSION_NAME,
                                   CA.MONTHLY_AMOUNT,
                                   SI.NAME SOURCE_INCOME_NAME,
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
                                   PB.NAME PURPOSE_BUSINESS_NAME,
                                   CA.IS_SANCTION_CHECK,
                                   CA.SANCTION_CHECK_URL,
                                   CA.NOTE
                              FROM CRS_USER.CUSTOMER_AML CA,
                                   CRS_USER.RISK_DEGREE RD,
                                   CRS_USER.SOCIAL_STATUS SS,
                                   CRS_USER.PROFESSION P,
                                   CRS_USER.SOURCE_INCOME SI,
                                   CRS_USER.POLITICAL_DUTY PDL,
                                   CRS_USER.POLITICAL_DUTY PDF,
                                   CRS_USER.POLITICAL_DUTY PDR,
                                   CRS_USER.CUSTOMER_BEHAVIOR CB,
                                   CRS_USER.CUSTOMER_IDENTIFICATION_METHOD CIM,
                                   CRS_USER.PURPOSE_BUSINESS_RELATIONSHIP PB
                             WHERE CA.RISK_DEGREE_ID = RD.ID(+)
                                AND CA.SOCIAL_STATUS_ID = SS.ID(+)
                                AND CA.PROFESSION_ID = P.ID(+)
                                AND CA.SOURCE_INCOME_ID = SI.ID(+)
                                AND CA.LOCAL_POLITICAL_DUTY_ID = PDL.ID(+)
                                AND CA.FOREIGN_POLITICAL_DUTY_ID = PDF.ID(+)
                                AND CA.RELATIVE_POLITICAL_DUTY_ID = PDR.ID(+)
                                AND CA.CUSTOMER_BEHAVIOR_ID = CB.ID(+)
                                AND CA.CUSTOMER_IDENTIFICATION_ID = CIM.ID(+)
                                AND CA.PURPOSE_BUSINESS_RELATIONSHIP = PB.ID(+)
                                AND CA.CUSTOMER_ID = {customerID}";

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
