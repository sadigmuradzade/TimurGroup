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

namespace CRS.Forms.Total
{
    public partial class FPenaltyAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPenaltyAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID;
        public int? ID;
        public decimal LastPenaltyDebt = 0;
        public DateTime? LastPenaltyDate;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        decimal debt = 0, penaltyInterest = 0, penaltyAmount = 0;
        DateTime startDate;
        int penaltyInterestID = 0;
        bool FormStatus = false;

        private void FPenaltyAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                LoadContractDetail();
            }
            else
            {
                LoadDetail();
                FormStatus = true;
            }
        }

        private void LoadContractDetail()
        {
            string sql = $@"SELECT CON.CONTRACT_CODE,
                                   CON.START_DATE,
                                   CON.INTEREST,
                                   CUS.CUSTOMER_NAME,
                                   CON.CURRENCY_CODE,
                                   CON.AMOUNT,
                                   TO_CHAR(CLP.PAYMENT_DATE,'DD.MM.YYYY') PAYMENT_DATE,
                                   NVL (CLP.DEBT, CON.AMOUNT) DEBT
                              FROM CRS_USER.V_CONTRACTS CON,
                                   CRS_USER.V_CUSTOMERS CUS,
                                   CRS_USER.V_CUSTOMER_LAST_PAYMENT CLP
                             WHERE     CON.CUSTOMER_ID = CUS.ID
                                   AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND CON.CONTRACT_ID = CLP.CONTRACT_ID(+)
                                   AND CON.CONTRACT_ID = {ContractID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/", "Müqavilənin parametrləri açılmadı.");

            if (dt.Rows.Count > 0)
            {
                ContractCodeText.EditValue = dt.Rows[0]["CONTRACT_CODE"];
                startDate = Convert.ToDateTime(dt.Rows[0]["START_DATE"]);
                ContractStartDateText.EditValue = startDate.ToString("dd.MM.yyyy");
                if (LastPenaltyDate.HasValue)
                    FromDateValue.Properties.MinValue = LastPenaltyDate > startDate ? LastPenaltyDate.Value.AddDays(1) : startDate.AddDays(1);
                else
                    FromDateValue.Properties.MinValue = startDate.AddDays(1);
                CustomerNameText.EditValue = dt.Rows[0]["CUSTOMER_NAME"];
                InterestText.EditValue = dt.Rows[0]["INTEREST"];
                LastDateText.EditValue = dt.Rows[0]["PAYMENT_DATE"];
                debt = Convert.ToDecimal(dt.Rows[0]["DEBT"]);
                DebtValue.EditValue = debt;
                AmountCurrencyLabel.Text = DebtCurrencyLabel.Text = PenaltyAmountCurrencyLabel.Text = GracePenaltyAmountCurrencyLabel.Text = dt.Rows[0]["CURRENCY_CODE"].ToString();
            }
        }

        private void LoadDetail()
        {
            string sql = $@"SELECT TO_CHAR(CP.LAST_PAYMENT_DATE,'DD.MM.YYYY') LAST_PAYMENT_DATE,
                                   CP.START_DATE FROMDATE,
                                   CP.END_DATE TODATE,
                                   CP.DAY_COUNT,
                                   CP.DEBT,
                                   IP.INTEREST PENALTY_INTEREST,
                                   CP.PENALTY_AMOUNT,
                                   CP.PENALTY_DEBT,
                                   CON.CONTRACT_CODE,
                                   CON.START_DATE,
                                   CON.INTEREST,
                                   CUS.CUSTOMER_NAME,
                                   CON.CURRENCY_CODE,
                                   NVL (LAG (CP.PENALTY_DEBT, 1) OVER (ORDER BY CP.ID), 0) LAST_PENALTY_DEBT,
                                   LAG (CP.END_DATE, 1) OVER (ORDER BY CP.ID) LAST_PENALTY_DATE,
                                   CP.PRESENT_PENALTY,
                                   CP.INTEREST_PENALTIES_ID 
                              FROM CRS_USER_TEMP.CONTRACT_PENALTY_TEMP CP,
                                   CRS_USER.CONTRACT_INTEREST_PENALTIES IP,
                                   CRS_USER.V_CONTRACTS CON,
                                   CRS_USER.V_CUSTOMERS CUS
                             WHERE     CP.INTEREST_PENALTIES_ID = IP.ID
                                   AND CON.CUSTOMER_ID = CUS.ID
                                   AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND CP.CONTRACT_ID = CON.CONTRACT_ID
                                   AND CP.IS_CHANGE != 2
                                   AND CP.CONTRACT_ID = {ContractID}
                                   AND CP.ID = {ID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetail", "Cərimənin məlumatları açılmadı.");

            if (dt.Rows.Count > 0)
            {
                ContractCodeText.EditValue = dt.Rows[0]["CONTRACT_CODE"];
                startDate = Convert.ToDateTime(dt.Rows[0]["START_DATE"]);
                ContractStartDateText.EditValue = startDate.ToString("dd.MM.yyyy");
                CustomerNameText.EditValue = dt.Rows[0]["CUSTOMER_NAME"];
                InterestText.EditValue = dt.Rows[0]["INTEREST"];
                LastDateText.EditValue = dt.Rows[0]["LAST_PAYMENT_DATE"];
                FromDateValue.EditValue = dt.Rows[0]["FROMDATE"];
                ToDateValue.EditValue = dt.Rows[0]["TODATE"];
                DaysCountValue.EditValue = dt.Rows[0]["DAY_COUNT"];
                DebtValue.EditValue = dt.Rows[0]["DEBT"];
                PenaltyInterestValue.EditValue = dt.Rows[0]["PENALTY_INTEREST"];
                PenaltyAmountValue.EditValue = dt.Rows[0]["PENALTY_AMOUNT"];                
                PresentPenaltyValue.EditValue = dt.Rows[0]["PRESENT_PENALTY"];
                penaltyInterestID = Convert.ToInt32(dt.Rows[0]["INTEREST_PENALTIES_ID"]);
                AmountCurrencyLabel.Text = DebtCurrencyLabel.Text = PenaltyAmountCurrencyLabel.Text = dt.Rows[0]["CURRENCY_CODE"].ToString();
                if (dt.Rows[0]["LAST_PENALTY_DATE"].ToString() != string.Empty)
                    LastPenaltyDate = Convert.ToDateTime(dt.Rows[0]["LAST_PENALTY_DATE"]);
                else
                    LastPenaltyDate = null;
                if (LastPenaltyDate.HasValue)
                    FromDateValue.Properties.MinValue = LastPenaltyDate > startDate ? LastPenaltyDate.Value.AddDays(1) : startDate.AddDays(1);
                else
                    FromDateValue.Properties.MinValue = startDate.AddDays(1);
            }
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            DayCount();
            PenaltyDetail();
            PenaltyAmount();
        }

        void PenaltyDetail()
        {
            if (!FormStatus)
                return;

            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT ID,INTEREST
                                                                                      FROM CRS_USER.CONTRACT_INTEREST_PENALTIES P
                                                                                     WHERE ID =
                                                                                              (SELECT MAX (ID)
                                                                                                 FROM CRS_USER.CONTRACT_INTEREST_PENALTIES
                                                                                                WHERE     CONTRACT_ID = P.CONTRACT_ID
                                                                                                      AND CALC_DATE <= TO_DATE ('{FromDateValue.Text}', 'DD.MM.YYYY'))
                                                                                        AND P.CONTRACT_ID = {ContractID}", this.Name + "/PenaltyDetail", "Cərimə faizi açılmadı.");

            if (dt.Rows.Count > 0)
            {
                penaltyInterest = Convert.ToDecimal(dt.Rows[0]["INTEREST"]);
                penaltyInterestID = Convert.ToInt16(dt.Rows[0]["ID"]);
            }

            PenaltyInterestValue.EditValue = penaltyInterest;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPenalty();
                else
                    UpdatePenalty();
                this.Close();
            }
        }

        private void InsertPenalty()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_PENALTY_TEMP(ID,
                                                                                CONTRACT_ID,
                                                                                LAST_PAYMENT_DATE,
                                                                                START_DATE,
                                                                                END_DATE,
                                                                                DAY_COUNT,
                                                                                DEBT,
                                                                                PENALTY_AMOUNT,
                                                                                PENALTY_DEBT,
                                                                                INTEREST_PENALTIES_ID,
                                                                                PRESENT_PENALTY,
                                                                                IS_CHANGE,
                                                                                USED_USER_ID)
                            VALUES(CRS_USER.CALCULATED_PENALTIES_SEQUENCE.NEXTVAL,
                                    {ContractID},
                                    TO_DATE('{LastDateText.Text}','DD.MM.YYYY'),
                                    TO_DATE('{FromDateValue.Text}','DD.MM.YYYY'),
                                    TO_DATE('{ToDateValue.Text}','DD.MM.YYYY'),
                                    {DaysCountValue.Value},
                                    {debt.ToString(GlobalVariables.V_CultureInfoEN)},
                                    {PenaltyAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    {TotalPenaltyAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    {penaltyInterestID},
                                    {PresentPenaltyValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    1,
                                    {GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Hesablanmış cərimə faizi bazaya daxil edilmədi.", this.Name + "/Insert");
        }

        private void FPenaltyAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            if (ToDateValue.EditorContainsFocus)
            {
                DayCount();
                PenaltyAmount();
            }
        }

        private void PenaltyAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            TotalPenalty();
        }

        private void TotalPenalty()
        {
            TotalPenaltyAmountValue.EditValue = PenaltyAmountValue.Value - PresentPenaltyValue.Value;
        }

        private void UpdatePenalty()
        {
            string sql = $@"UPDATE CRS_USER_TEMP.CONTRACT_PENALTY_TEMP SET START_DATE = TO_DATE('{FromDateValue.Text}','DD.MM.YYYY'),
                                                                                END_DATE = TO_DATE('{ToDateValue.Text}','DD.MM.YYYY'),
                                                                                DAY_COUNT = {DaysCountValue.Value},                                                                                
                                                                                PENALTY_AMOUNT = {PenaltyAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                PENALTY_DEBT = {TotalPenaltyAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},   
                                                                                INTEREST_PENALTIES_ID = {penaltyInterestID},
                                                                                PRESENT_PENALTY = {PresentPenaltyValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                IS_CHANGE = 1
                            WHERE CONTRACT_ID = {ContractID} AND ID = {ID}";

            GlobalProcedures.ExecuteQuery(sql, "Hesablanmış cərimə faizi bazada dəyişdirilmədi.", this.Name + "/Update");
        }

        private void PresentPenaltyValue_EditValueChanged(object sender, EventArgs e)
        {
            if (PresentPenaltyValue.EditorContainsFocus)
                TotalPenalty();
        }

        private void CheckPenaltyDebt_CheckedChanged(object sender, EventArgs e)
        {
            TotalPenalty();
        }

        private void PenaltyAmount()
        {
            if (!FormStatus)
                return;

            penaltyAmount = Math.Round(DebtValue.Value * PenaltyInterestValue.Value / 100, 2) * DaysCountValue.Value + LastPenaltyDebt;
            PenaltyAmountValue.EditValue = penaltyAmount;
        }

        void DayCount()
        {
            if (!FormStatus)
                return;

            if (FromDateValue.Text.Length == 0 || ToDateValue.Text.Length == 0)
                DaysCountValue.EditValue = 0;
            else
                DaysCountValue.EditValue = GlobalFunctions.Days360(FromDateValue.DateTime, ToDateValue.DateTime);
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (FromDateValue.Text.Length == 0)
            {
                FromDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cərimənin başlama tarixi seçilməyib.");
                FromDateValue.Focus();
                FromDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ToDateValue.Text.Length == 0)
            {
                ToDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cərimənin bitmə tarixi seçilməyib.");
                ToDateValue.Focus();
                ToDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }
    }
}