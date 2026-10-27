using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;
using Oracle.ManagedDataAccess.Client;

namespace CRS.Forms.Bookkeeping
{
    public partial class FSalary : DevExpress.XtraEditors.XtraForm
    {
        public FSalary()
        {
            InitializeComponent();
        }
        public delegate void DoEvent();
        public event DoEvent RefreshOperationsDataGridView;

        string recruiterAccount = null,
               employeeAccount = null,
               itsAccount = null,
               insuranceAccount = null,
               sumITSAccount = null,
               sumInsuranceAccount = null,
               payedAccount = null;

        private void InsuranceAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            insuranceAccount = (sender as LookUpEdit).EditValue.ToString();
        }

        private void SumITSAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            sumITSAccount = (sender as LookUpEdit).EditValue.ToString();
        }

        private void SumInsuranceAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            sumInsuranceAccount = (sender as LookUpEdit).EditValue.ToString();
        }

        private void PayedAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            payedAccount = (sender as LookUpEdit).EditValue.ToString();
        }

        private void FSalary_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshOperationsDataGridView();
        }

        private void ITSAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            itsAccount = (sender as LookUpEdit).EditValue.ToString();
        }

        private void MDSSEmployeeAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            employeeAccount = (sender as LookUpEdit).EditValue.ToString();
        }

        private void MDSSRecruiterAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            recruiterAccount = (sender as LookUpEdit).EditValue.ToString();
        }

        private void FSalary_Load(object sender, EventArgs e)
        {
            OperationDate.DateTime = DateTime.Today;
            GlobalProcedures.FillLookUpEditWithSqlText(MDSSRecruiterAccountLookUp, $@"SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = 223", "ID", "NAME");
            GlobalProcedures.FillLookUpEditWithSqlText(MDSSEmployeeAccountLookUp, $@"SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = 223", "ID", "NAME");
            GlobalProcedures.FillLookUpEditWithSqlText(ITSAccountLookUp, $@"SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = 223", "ID", "NAME");
            GlobalProcedures.FillLookUpEditWithSqlText(InsuranceAccountLookUp, $@"SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = 223", "ID", "NAME");
            GlobalProcedures.FillLookUpEditWithSqlText(SumITSAccountLookUp, $@"SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = 223", "ID", "NAME");
            GlobalProcedures.FillLookUpEditWithSqlText(SumInsuranceAccountLookUp, $@"SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = 223", "ID", "NAME");
            GlobalProcedures.FillLookUpEditWithSqlText(PayedAccountLookUp, $@"SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = 223", "ID", "NAME");
        }

        private bool ControlDetail()
        {
            bool b = false;

            if (OperationDate.Text.Length == 0)
            {
                OperationDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əməliyyatın tarixi seçilməyib.");
                OperationDate.Focus();
                OperationDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SalaryDate.Text.Length == 0)
            {
                SalaryDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əməkhaqqıının hansı tarix üçün olduğu seçilməyib.");
                SalaryDate.Focus();
                SalaryDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SalaryDate.DateTime != new DateTime(SalaryDate.DateTime.Year, SalaryDate.DateTime.Month,
                                    DateTime.DaysInMonth(SalaryDate.DateTime.Year, SalaryDate.DateTime.Month)))
            {
                SalaryDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix ayın son günü olmalıdır.");
                SalaryDate.Focus();
                SalaryDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TotalAmount.Value <= 0)
            {
                TotalAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cəmi hesablanmış əməkhaqqı sıfırdan böyük olmalıdır.");
                TotalAmount.Focus();
                TotalAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (MDSSRecruiterAmount.Value < 0)
            {
                MDSSRecruiterAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("MDSS İşə götürən sıfırdan kiçik ola bilməz.");
                MDSSRecruiterAmount.Focus();
                MDSSRecruiterAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (MDSSRecruiterAmount.Value > 0 && recruiterAccount == null)
            {
                MDSSRecruiterAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab seçilməyib.");
                MDSSRecruiterAccountLookUp.Focus();
                MDSSRecruiterAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (MDSSEmployeeAmount.Value < 0)
            {
                MDSSEmployeeAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("MDSS İşçidən sıfırdan kiçik ola bilməz.");
                MDSSEmployeeAmount.Focus();
                MDSSEmployeeAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (MDSSEmployeeAmount.Value> 0 && employeeAccount == null)
            {
                MDSSEmployeeAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab seçilməyib.");
                MDSSEmployeeAccountLookUp.Focus();
                MDSSEmployeeAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ITSAmount.Value < 0)
            {
                ITSAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İcbari Tibbi Sığorta 2% sıfırdan kiçik ola bilməz.");
                ITSAmount.Focus();
                ITSAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ITSAmount.Value> 0 && itsAccount == null)
            {
                ITSAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab seçilməyib.");
                ITSAccountLookUp.Focus();
                ITSAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (UnemploymentInsuranceAmount.Value < 0)
            {
                UnemploymentInsuranceAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İşsizlikdən sığorta 0,5% sıfırdan kiçik ola bilməz.");
                UnemploymentInsuranceAmount.Focus();
                UnemploymentInsuranceAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (UnemploymentInsuranceAmount.Value > 0 && insuranceAccount == null)
            {
                InsuranceAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab seçilməyib.");
                InsuranceAccountLookUp.Focus();
                InsuranceAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SumITSAmount.Value < 0)
            {
                SumITSAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İcbari Tibbi Sığorta 2% sıfırdan kiçik ola bilməz.");
                SumITSAmount.Focus();
                SumITSAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SumITSAmount.Value > 0 && sumITSAccount == null)
            {
                SumITSAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab seçilməyib.");
                SumITSAccountLookUp.Focus();
                SumITSAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SumUnemploymentInsuranceAmount.Value < 0)
            {
                SumUnemploymentInsuranceAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İşsizlikdən sığorta 0,5% sıfırdan kiçik ola bilməz.");
                SumUnemploymentInsuranceAmount.Focus();
                SumUnemploymentInsuranceAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SumUnemploymentInsuranceAmount.Value > 0 && sumInsuranceAccount == null)
            {
                SumInsuranceAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab seçilməyib.");
                SumInsuranceAccountLookUp.Focus();
                SumInsuranceAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PayedAmount.Value <= 0)
            {
                PayedAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Verilən məbləğ sıfırdan böyük olmalıdır.");
                PayedAmount.Focus();
                PayedAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PayedAmount.Value > 0 && payedAccount == null)
            {
                PayedAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab seçilməyib.");
                PayedAccountLookUp.Focus();
                PayedAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlDetail())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));
                using (OracleConnection connection = new OracleConnection())
                {
                    OracleTransaction transaction = null;
                    OracleCommand command = null;
                    try
                    {
                        if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                        {
                            connection.ConnectionString = GlobalFunctions.GetConnectionString();
                            connection.Open();
                        }
                        transaction = connection.BeginTransaction();
                        command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandText = "CRS_USER.PROC_SALARY_OPERATION";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("P_DATE", OracleDbType.Varchar2).Value = OperationDate.Text;
                        command.Parameters.Add("P_YEAR", OracleDbType.Int16).Value = SalaryDate.DateTime.Year;
                        command.Parameters.Add("P_MONTH", OracleDbType.Varchar2).Value = GlobalFunctions.FindMonth(SalaryDate.DateTime.Month);
                        command.Parameters.Add("P_TOTAL", OracleDbType.Decimal).Value = TotalAmount.Value;
                        command.Parameters.Add("P_MDSS_RECRUITER", OracleDbType.Decimal).Value = MDSSRecruiterAmount.Value;
                        command.Parameters.Add("P_MDSS_RECRUITER_ACCOUNT", OracleDbType.Varchar2).Value = recruiterAccount;
                        command.Parameters.Add("P_MDSS_EMPLOYEE", OracleDbType.Decimal).Value = MDSSEmployeeAmount.Value;
                        command.Parameters.Add("P_MDSS_EMPLOYEE_ACCOUNT", OracleDbType.Varchar2).Value = employeeAccount;
                        command.Parameters.Add("P_ITS", OracleDbType.Decimal).Value = ITSAmount.Value;
                        command.Parameters.Add("P_ITS_ACCOUNT", OracleDbType.Varchar2).Value = itsAccount;
                        command.Parameters.Add("P_UNEMPLOYMENT_INSURANCE", OracleDbType.Decimal).Value = UnemploymentInsuranceAmount.Value;
                        command.Parameters.Add("P_INSURANCE_ACCOUNT", OracleDbType.Varchar2).Value = insuranceAccount;
                        command.Parameters.Add("P_SUM_ITS", OracleDbType.Decimal).Value = SumITSAmount.Value;
                        command.Parameters.Add("P_SUM_ITS_ACCOUNT", OracleDbType.Varchar2).Value = sumITSAccount;
                        command.Parameters.Add("P_SUM_UNEMPLOYMENT_INSURANCE", OracleDbType.Decimal).Value = SumUnemploymentInsuranceAmount.Value;
                        command.Parameters.Add("P_SUM_INSURANCE_ACCOUNT", OracleDbType.Varchar2).Value = sumInsuranceAccount;
                        command.Parameters.Add("P_PAYED", OracleDbType.Decimal).Value = PayedAmount.Value;
                        command.Parameters.Add("P_PAYED_ACCOUNT", OracleDbType.Varchar2).Value = payedAccount;
                        command.Parameters.Add("P_NOTE", OracleDbType.Varchar2).Value = NoteText.Text.Trim();
                        command.Parameters.Add("P_USED_USER_ID", OracleDbType.Int16).Value = GlobalVariables.V_UserID;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception exx)
                    {
                        GlobalProcedures.SplashScreenClose();
                        GlobalProcedures.LogWrite("Əməkhaqqının ödənişləri jurnala daxil edilmədi.", "CRS_USER.PROC_SALARY_OPERATION", GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                        transaction.Rollback();
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Dispose();
                    }
                }
                GlobalProcedures.SplashScreenClose();
                this.Close();
            }
        }
    }
}