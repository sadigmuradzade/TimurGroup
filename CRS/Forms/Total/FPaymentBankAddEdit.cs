using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CRS.Forms.Total
{
    public partial class FPaymentBankAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPaymentBankAddEdit()
        {
            InitializeComponent();
            MaximumSize = MinimumSize = Size;
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string TransactionName,
            LastDate,
            Currency,
            PaymentID,
            CustomerName,
            ContractCode,
            ContractStartDate,
            BankName,
            BranchName,
            AccountName,
            PDate,
            CommitmentName;

        public int PaymentCount = 0,
            Interest,
            CustomerID = 0,
            ContractID = 0,
            CustomerTypeID,
            InsuranceID,
            PenaltyID;

        public decimal InsuranceAmount = 0, PrevPaymentInterestDebt = 0;

        public double Amount,
            Debt,
            MonthlyAmount,
            PaymentAmount,
            PaymentInterestDebt = 0,
            DebtPenalty = 0;

        double one_day_interest = 0,
            interest_amount = 0,
            debt = 0,
            residual_percent = 0,
            normal_debt = 0,
            cur,
            totaldebtamount = 0,
            requiredamount = 0,
            penalty_amount = 0;

        int diff_day,
            order_id,
            currency_id,
            bank_id = 0,
            accounting_plan_id = 0,
            is_penalty = 1,
            is_changed_interest = 0;
        decimal changed_interest_amount = 0;

        private void ChangedInterestCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ChangedInterestCheck.Checked)
            {
                PayedPercentValue.Focus();
                changed_interest_amount = PayedPercentValue.Value;
                PayedPercentValue.Properties.ReadOnly = false;
                PayedPercentValue.TabStop = true;
            }
            else
            {
                PayedPercentValue.EditValue = changed_interest_amount; //TransactionName == "INSERT" ? PaymentInterestDebtValue.Value : (decimal)interest_amount + PaymentInterestDebtValue.Value;
                changed_interest_amount = 0;
                PayedPercentValue.Properties.ReadOnly = true;
                PayedPercentValue.TabStop = false;
            }
        }

        private void PayedPenaltyValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            TotalDebtValue.Value = DebtValue.Value + PaymentInterestDebtValue.Value + PayedPenaltyValue.Value;
        }

        void InsuranceNote()
        {
            if (InsurancePayedCheck.Checked && InsurancePayedAmount.Value > 0)
                NoteText.EditValue = "S - " + InsurancePayedAmount.Value;
            else
                NoteText.EditValue = null;
        }

        private void InsurancePayedAmount_EditValueChanged(object sender, EventArgs e)
        {
            if (InsurancePayedAmount.EditorContainsFocus)
                CalcLeasingPayAmount();

            InsuranceNote();
        }

        private void InsurancePayedCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (InsurancePayedCheck.Checked)
            {
                if (FormStatus)
                    InsurancePayedAmount.EditValue = InsuranceAmount;
                InsurancePayedAmount.Focus();
                InsurancePayedAmount.ReadOnly = false;
            }
            else
            {
                InsurancePayedAmount.ReadOnly = true;
                if (InsuranceAmount == 0)
                    InsurancePayedAmount.EditValue = InsuranceAmount;
            }
            InsuranceNote();
            CalcLeasingPayAmount();
        }

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bank_id = Convert.ToInt32(BankLookUp.EditValue);

            GlobalProcedures.FillLookUpEdit(OperationAccountLookUp, "ACCOUNTING_PLAN", "ID", "SUB_ACCOUNT", "BANK_ID = " + bank_id + " ORDER BY  ID,ACCOUNT_NUMBER");
            OperationAccountLookUp.ItemIndex = 0;
        }

        private void OperationAccountLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Bookkeeping.FAccountingPlan fap = new Bookkeeping.FAccountingPlan();
                fap.RefreshAccountList += new Bookkeeping.FAccountingPlan.DoEvent(RefreshOperationsAccount);
                fap.ShowDialog();
            }
        }

        private void OperationAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (OperationAccountLookUp.EditValue == null)
                return;

            accounting_plan_id = Convert.ToInt32(OperationAccountLookUp.EditValue);
        }

        bool FormStatus = false;
        string ldate;
        List<ContractPercentDate> lstPercentDate = null;

        public delegate void DoEvent(decimal a, string p);
        public event DoEvent RefreshPaymentsDataGridView;

        private void FPaymentBank_Load(object sender, EventArgs e)
        {
            CurrencyRateValue.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            CurrencyRateValue.Properties.DisplayFormat.FormatString = "### ##0.0000";

            RefreshDictionaries(11);
            BankLookUp.ItemIndex = 0;
            InsurancePayedCheck.ReadOnly = InsuranceAmount == 0;
            InsurancePayedCheck.Checked = InsuranceAmount > 0;

            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                currency_id = (int)GlobalFunctions.GetAmount($@"SELECT ID FROM CRS_USER.CURRENCY WHERE CODE = '{Currency}'");
                if (PaymentCount == 0)
                    LastDateLabel.Visible = LastDateText.Visible = false;
                else
                {
                    LastDateLabel.Visible = LastDateText.Visible = true;
                    LastDateLabel.Text = "Ən son ödənişin tarixi";
                }
                PenaltyAmountValue.EditValue = DebtPenalty;
                PaymentDate.EditValue = DateTime.Today;
                PaymentID = GlobalFunctions.GetOracleSequenceValue("CUSTOMER_PAYMENT_SEQUENCE").ToString();
                ContractCodeText.Text = ContractCode;
                ContractStartDateText.Text = ContractStartDate;
                CustomerNameText.Text = CustomerName;

                if (String.IsNullOrEmpty(CommitmentName))
                    NameText.Text = CustomerName;
                else
                    NameText.Text = CommitmentName;
                LastDateText.Text = LastDate;
                DebtValue.EditValue = Debt;
                InterestText.Text = Interest.ToString();
                one_day_interest = Math.Round(Debt * Interest / 100 / 360, 2);
                OneDayInterestValue.EditValue = one_day_interest;
                MonthlyPaymentValue.EditValue = MonthlyAmount;
            }
            else if (TransactionName == "EDIT")
            {
                InsertBankOperationsTemp();
                LoadPaymentDetails();
            }
            else
            {
                ContractCodeText.TabIndex = 0;
                ContractCodeText.TabStop = true;
                PaymentID = GlobalFunctions.GetOracleSequenceValue("CUSTOMER_PAYMENT_SEQUENCE").ToString();
                BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(BankName);
                BankLookUp.Enabled = false;
                PaymentDate.Enabled = false;
                ContractCodeText.Properties.ReadOnly = false;
            }

            if (currency_id == 1)
            {
                PaymentAZNLabel.Visible = PaymentAZNValue.Visible = false;
                PaymentLabel.Text = "Ödənişin məbləği";
            }
            else
            {
                PaymentAZNLabel.Visible = PaymentAZNValue.Visible = true;
                PaymentLabel.Text = "Ödənişin məbləği (" + Currency + " - ilə)";
            }

            if (TransactionName != "BINSERT")
            {
                PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
                DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT MIN(PAYMENT_DATE) PDATE, COUNT(*) RCOUNT FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND ID > {PaymentID}");
                if (Convert.ToInt16(dt.Rows[0]["RCOUNT"]) > 0)
                    PaymentDate.Properties.MaxValue = Convert.ToDateTime(dt.Rows[0]["PDATE"]);
                PaymentDate_EditValueChanged(sender, EventArgs.Empty);
            }
            InsurancePayedCheck_CheckedChanged(sender, EventArgs.Empty);
            FormStatus = true;

            CurrencyLabel.Text =
               PenaltyCurrencyLabel.Text =
               PayedPenaltyCurrencyLabel.Text =
               OverduePercentCurrencyLabel.Text =
               PayedPercentCurrencyLabel.Text = Currency;
        }

        private void InsertBankOperationsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_BO_PAYMENT_TEMP", "P_PAYMENT_ID", PaymentID, "Ödəniş temp cədvələ daxil edilmədi.");
        }

        private void LoadPaymentDetails()
        {
            double leasing_amount = 0;
            string s = null;
            try
            {
                s = $@"SELECT C.CONTRACT_CODE,
                                   TO_CHAR (C.START_DATE, 'DD.MM.YYYY') S_DATE,
                                   CUS.CUSTOMER_NAME,
                                   CP.PAYMENT_NAME,
                                   CP.PAYMENT_DATE P_DATE,
                                   CP.DAY_COUNT,
                                   C.INTEREST,
                                   CP.REQUIRED_CLOSE_AMOUNT,
                                   CP.ONE_DAY_INTEREST_AMOUNT,
                                   CP.INTEREST_AMOUNT,
                                   NVL2 (CE.MONTHLY_AMOUNT, CE.MONTHLY_AMOUNT, C.MONTHLY_AMOUNT)
                                            MONTHLY_AMOUNT,
                                   CP.PAYMENT_INTEREST_AMOUNT,
                                   CP.REQUIRED_AMOUNT,
                                   CP.PAYED_AMOUNT,
                                   CP.PAYED_AMOUNT_AZN,
                                   C.CURRENCY_ID,
                                   C.CURRENCY_CODE,
                                   CP.NOTE,
                                   CP.CUSTOMER_ID,
                                   CP.CONTRACT_ID,
                                   C.AMOUNT,
                                   CP.CURRENCY_RATE,
                                   NVL(LP.LAST_PAYMENT_INTEREST_DEBT, 0) LAST_PAYMENT_INTEREST_DEBT,
                                   CP.PENALTY_AMOUNT,
                                   CP.PENALTY_DEBT,
                                   CP.IS_PENALTY,
                                   CP.PAYED_PENALTY,
                                   BO.BANK_NAME,                                   
                                   LP.LAST_DATE,
                                   LP.LAST_DEBT,
                                   CP.IS_BASIC_AMOUNT,
                                   CP.INSURANCE_CHECK,
                                   CP.INSURANCE_AMOUNT,
                                   CP.OVERDUE_PERCENT,
                                   CP.NORMAL_DEBT,
                                   CP.DELAYS_AMOUNT,
                                   CP.IS_CHANGED_INTEREST
                              FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP,
                                   CRS_USER.V_CONTRACTS C,       
                                   CRS_USER.V_CUSTOMERS CUS,
                                   CRS_USER_TEMP.V_BANK_OPERATION_TEMP BO,
                                   CRS_USER_TEMP.V_CONTRACT_LAST_PAYMENT_TEMP LP,
                                   CRS_USER.V_LAST_CONTRACT_EXTEND CE
                             WHERE     C.CUSTOMER_ID = CUS.ID
                                   AND C.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND CP.CONTRACT_ID = C.CONTRACT_ID    
                                   AND CP.CONTRACT_ID = CE.CONTRACT_ID(+)
                                   AND CP.ID = BO.CONTRACT_PAYMENT_ID  
                                   AND CP.ID = LP.ID      
                                   AND CP.IS_CHANGE != 2
                                   AND BO.USED_USER_ID = {GlobalVariables.V_UserID}    
                                   AND CP.ID = {PaymentID}";
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    ContractCodeText.Text = dr["CONTRACT_CODE"].ToString();
                    ContractStartDateText.Text = dr["S_DATE"].ToString();
                    CustomerNameText.Text = dr["CUSTOMER_NAME"].ToString();
                    NameText.Text = dr["PAYMENT_NAME"].ToString();
                    PaymentDate.EditValue = DateTime.Parse(dr["P_DATE"].ToString());
                    PaymentDate.Enabled = false;
                    ldate = PaymentDate.Text;
                    DayCountText.Text = dr["DAY_COUNT"].ToString();
                    diff_day = Convert.ToInt32(dr["DAY_COUNT"]);
                    InterestText.Text = dr["INTEREST"].ToString();
                    Interest = Convert.ToInt32(dr["INTEREST"]);
                    TotalDebtValue.Value = Convert.ToDecimal(dr["REQUIRED_CLOSE_AMOUNT"]);
                    OneDayInterestValue.Value = Convert.ToDecimal(dr["ONE_DAY_INTEREST_AMOUNT"]);
                    interest_amount = Convert.ToDouble(dr["INTEREST_AMOUNT"]);
                    ChangedInterestCheck.Checked = Convert.ToInt32(dr["IS_CHANGED_INTEREST"]) == 1 ? true : false;
                    PayedPercentValue.Value = Convert.ToDecimal(dr["PAYMENT_INTEREST_AMOUNT"]);
                    MonthlyPaymentValue.Value = Convert.ToDecimal(dr["MONTHLY_AMOUNT"]);
                    RequiredValue.Value = Convert.ToDecimal(dr["REQUIRED_AMOUNT"]);
                    if (dr["CURRENCY_CODE"].ToString() != "AZN")
                    {
                        CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                        CurrencyRateLabel.Text = "1 " + dr["CURRENCY_CODE"] + " = ";
                        CurrencyRateValue.Value = Convert.ToDecimal(dr["CURRENCY_RATE"]);
                    }
                    PaymentValue.Value = Convert.ToDecimal(dr["PAYED_AMOUNT"]);
                    PaymentAZNValue.Value = Convert.ToDecimal(dr["PAYED_AMOUNT_AZN"]);
                    InsurancePayedCheck.Checked = Convert.ToInt16(dr["INSURANCE_CHECK"]) == 1 ? true : false;
                    InsurancePayedAmount.Value = Convert.ToDecimal(dr["INSURANCE_AMOUNT"]);
                    Currency = dr["CURRENCY_CODE"].ToString();
                    currency_id = Convert.ToInt32(dr["CURRENCY_ID"]);
                    NoteText.Text = dr["NOTE"].ToString();
                    CustomerID = Convert.ToInt32(dr["CUSTOMER_ID"]);
                    ContractID = Convert.ToInt32(dr["CONTRACT_ID"]);
                    leasing_amount = Convert.ToDouble(dr["AMOUNT"]);
                    PaymentInterestDebtValue.EditValue = Convert.ToDecimal(dr["LAST_PAYMENT_INTEREST_DEBT"]);
                    PayedPenaltyValue.EditValue = Convert.ToDecimal(dr["PAYED_PENALTY"]);
                    PenaltyAmountValue.EditValue = Convert.ToDecimal(dr["PENALTY_AMOUNT"]);
                    BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_NAME"].ToString());
                    if (String.IsNullOrWhiteSpace(dr["LAST_DATE"].ToString()))
                    {
                        LastDateText.Visible = LastDateLabel.Visible = false;
                        LastDateText.Text = ContractStartDateText.Text;
                        Debt = leasing_amount;
                        DebtValue.Value = (decimal)Debt;
                    }
                    else
                    {
                        LastDateText.Visible = LastDateLabel.Visible = true;
                        LastDateText.Text = dr["LAST_DATE"].ToString().Substring(0, 10);
                        Debt = Convert.ToDouble(dr["LAST_DEBT"]);
                        DebtValue.EditValue = Debt;
                    }
                    OverduePercentValue.EditValue = Convert.ToDecimal(dr["OVERDUE_PERCENT"]);
                    NormDebtValue.EditValue = Convert.ToDouble(dr["NORMAL_DEBT"]);
                    DelaysAmountValue.EditValue = Convert.ToDouble(dr["DELAYS_AMOUNT"]);
                }


                one_day_interest = Math.Round(Debt * Interest / 100 / 360, 2);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void CalcLeasingPayAmount()
        {
            decimal a = PaymentAZNValue.Value - (InsurancePayedCheck.Checked ? InsurancePayedAmount.Value : 0);
            LeasingPayedAmount.EditValue = a > 0 ? a : 0;
        }

        private void PaymentValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!PaymentValue.EditorContainsFocus)
                return;

            PaymentAZNValue.Value = PaymentValue.Value * CurrencyRateValue.Value;
            if (cur != (double)CurrencyRateValue.Value)
                CurrencyRateValue.BackColor = Color.GreenYellow;
            else
                CurrencyRateValue.BackColor = GlobalFunctions.ElementColor();

            CalcLeasingPayAmount();
        }

        private void PaymentAZNValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!PaymentAZNValue.EditorContainsFocus)
                return;

            if (CurrencyRateValue.Value != 0)
                PaymentValue.Value = PaymentAZNValue.Value / CurrencyRateValue.Value;
            else
                PaymentValue.Value = 0;

            CalcLeasingPayAmount();
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                PaymentCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        private bool ControlPaymentDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(ContractCodeText.Text))
            {
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müqavilənin nömrəsi daxil edilməyib");
                ContractCodeText.Focus();
                ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CustomerID == 0 || ContractID == 0)
            {
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text + " saylı lizinq müqaviləsinə uyğun məlumatlar tapılmadı. Zəhmət olmasa müqavilələrin siyahısına baxın.");
                ContractCodeText.Focus();
                ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(NameText.Text))
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödəyənin tam adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(PaymentDate.Text))
            {
                PaymentDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödəniş tarixi seçilməyib.");
                PaymentDate.Focus();
                PaymentDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BankLookUp.EditValue == null)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank seçilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (OperationAccountLookUp.EditValue == null)
            {
                OperationAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Daxili mühasibatlıq hesab daxil edilməyib.");
                OperationAccountLookUp.Focus();
                OperationAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (PaymentValue.Value <= 0)
            //{
            //    PaymentValue.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Ödənişin məbləği sıfırdan böyük olmalıdır.");
            //    PaymentValue.Focus();
            //    PaymentValue.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            if (PaymentAZNValue.Value <= 0)
            {
                CurrencyRateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(PaymentDate.Text + " tarixi üçün " + CurrencyLabel.Text + " valyutasının AZN qarşılığı daxil edilməyib.");
                CurrencyRateValue.Focus();
                CurrencyRateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            decimal payedAmount = currency_id == 1 ? LeasingPayedAmount.Value : Math.Round(LeasingPayedAmount.Value / CurrencyRateValue.Value, 2);

            if (payedAmount > TotalDebtValue.Value)
            {
                LeasingPayedAmount.BackColor = Color.Red;
                TotalDebtValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödənişin məbləği 'Tam bağlamaq üçün tələb olunan məbləğ' - dən çox ola bilməz.");
                PaymentValue.Focus();
                LeasingPayedAmount.BackColor = GlobalFunctions.ElementColor();
                TotalDebtValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (!BasicAmountCheck.Checked && payedAmount > DebtValue.Value)
            //{
            //    LeasingPayedAmount.BackColor = Color.Red;
            //    DebtValue.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Ödənişin məbləği 'Qalıq borcdan' - dən çox ola bilməz.");
            //    PaymentValue.Focus();
            //    LeasingPayedAmount.BackColor = GlobalFunctions.ElementColor();
            //    DebtValue.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            if (InsurancePayedCheck.Checked && InsurancePayedAmount.Value > InsuranceAmount)
            {
                InsurancePayedAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage($@"Sığorta ödənişi maksimum {InsuranceAmount.ToString("N2")} AZN ola bilər.");
                InsurancePayedAmount.Focus();
                InsurancePayedAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (LeasingPayedAmount.Value <= 0)
            //{
            //    LeasingPayedAmount.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Lizinqə ödəniləcək məbləğ sıfırdan böyük olmalıdır.");
            //    PaymentValue.Focus();
            //    LeasingPayedAmount.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (PenaltyAmountValue.Value > 0 && LeasingPayedAmount.Value < ((BasicAmountCheck.Checked ? InterestAmountValue.Value : 0) + PenaltyAmountValue.Value))
            //{
            //    LeasingPayedAmount.BackColor = InterestAmountValue.BackColor = PenaltyAmountValue.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Lizinqə ödəniləcək məbləğ silinəcək faizlə hesablanmış cərimənin cəmindən kiçik ola bilməz.");
            //    PaymentValue.Focus();
            //    LeasingPayedAmount.BackColor = InterestAmountValue.BackColor = PenaltyAmountValue.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            return b;
        }

        private void InsertPayment()
        {
            decimal payment_interest_amount = 0, 
                currenct_payment_interest_debt = 0, 
                basic_amount = 0, 
                current_debt = 0, 
                total = 0, 
                payment_value_AZN = 0, 
                cdebt, penalty = 0,
                totalpayedinterest = 0;
            decimal penaltyDebt = 0, overdue_percent_debt = 0;

            cdebt = (decimal)Debt;
            PenaltyID = PenaltyAmountValue.Value == 0 ? 0 : PenaltyID;

            decimal paymentValue = Math.Round(CurrencyRateValue.Value > 0 ? LeasingPayedAmount.Value / CurrencyRateValue.Value : 0, 2);
            
            if (paymentValue > 0) // eger odenis sifirdan boyuk olarsa
            {
                if (totalpayedinterest > paymentValue) // eger ödənilən faiz edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                    payment_interest_amount = paymentValue;
                else
                    payment_interest_amount = PayedPercentValue.Value;
            }

            if (paymentValue < PayedPenaltyValue.Value)
            {
                basic_amount = 0;
                penalty_amount = (double)paymentValue;
                payment_interest_amount = 0;
            }
            else
            {
                penalty = paymentValue - PayedPenaltyValue.Value;
                if (penalty < payment_interest_amount)
                {
                    payment_interest_amount = penalty;
                    basic_amount = 0;
                }
                else
                    basic_amount = penalty - payment_interest_amount;
                penalty_amount = (double)PayedPenaltyValue.Value;
            }

            currenct_payment_interest_debt = (decimal)(PaymentInterestDebt + interest_amount) - payment_interest_amount;
            current_debt = cdebt - basic_amount;
            total = current_debt + currenct_payment_interest_debt;

            payment_value_AZN = LeasingPayedAmount.Value;
            overdue_percent_debt = OverduePercentValue.Value - payment_interest_amount;
            overdue_percent_debt = overdue_percent_debt < 0 ? 0 : overdue_percent_debt;
            if (TransactionName == "INSERT" && InsurancePayedCheck.Checked && InsurancePayedAmount.Value > 0)
                GlobalProcedures.ExecuteThreeQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP(ID,
                                                                                                        CUSTOMER_ID,
                                                                                                        CONTRACT_ID,
                                                                                                        PAYMENT_DATE,
                                                                                                        PAYMENT_AMOUNT,
                                                                                                        BASIC_AMOUNT,
                                                                                                        DEBT,
                                                                                                        DAY_COUNT,
                                                                                                        ONE_DAY_INTEREST_AMOUNT,
                                                                                                        INTEREST_AMOUNT,
                                                                                                        PAYMENT_INTEREST_AMOUNT,
                                                                                                        PAYMENT_INTEREST_DEBT,
                                                                                                        TOTAL,
                                                                                                        CURRENCY_RATE,
                                                                                                        PAYMENT_AMOUNT_AZN,
                                                                                                        REQUIRED_CLOSE_AMOUNT,
                                                                                                        REQUIRED_AMOUNT,
                                                                                                        PAYMENT_NAME,
                                                                                                        NOTE,
                                                                                                        USED_USER_ID,
                                                                                                        IS_CHANGE,
                                                                                                        BANK_CASH,
                                                                                                        IS_PENALTY,
                                                                                                        PENALTY_AMOUNT,
                                                                                                        PAYED_PENALTY,
                                                                                                        PENALTY_DEBT,
                                                                                                        BANK_ID,
                                                                                                        CUSTOMER_TYPE_ID,
                                                                                                        IS_BASIC_AMOUNT,
                                                                                                        PAYED_AMOUNT,
                                                                                                        PAYED_AMOUNT_AZN,
                                                                                                        INSURANCE_CHECK,
                                                                                                        INSURANCE_AMOUNT,
                                                                                                        CONTRACT_PERCENT,
                                                                                                        OVERDUE_PERCENT,
                                                                                                        OVERDUE_PERCENT_DEBT,
                                                                                                        DELAYS_AMOUNT,
                                                                                                        NORMAL_DEBT,
                                                                                                        IS_CHANGED_INTEREST)
                                                            VALUES({PaymentID},
                                                                    {CustomerID},
                                                                    {ContractID},
                                                                    TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                                                    {paymentValue.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {DayCountText.Text},
                                                                    {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {requiredamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    '{NameText.Text.Trim()}',
                                                                    '{NoteText.Text.Trim()}',
                                                                    {GlobalVariables.V_UserID},
                                                                    1,
                                                                    'B',                                                                    
                                                                    {is_penalty},
                                                                    {Math.Round(PenaltyAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(PayedPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(penaltyDebt, 2).ToString(GlobalVariables.V_CultureInfoEN)}, 
                                                                    {bank_id},
                                                                    {CustomerTypeID},
                                                                    1,
                                                                    {Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(PaymentAZNValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {(InsurancePayedCheck.Checked ? 1 : 0)},
                                                                    {(InsurancePayedCheck.Checked ? InsurancePayedAmount.Value.ToString(GlobalVariables.V_CultureInfoEN) : "0")},
                                                                    {Interest.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(OverduePercentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(overdue_percent_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(DelaysAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {Math.Round(NormDebtValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    {(ChangedInterestCheck.Checked ? 1 : 0)})",
                                                 $@"INSERT INTO CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP (ID,
                                                                                                       INSURANCE_ID,
                                                                                                       PAY_DATE,
                                                                                                       PAYED_AMOUNT, 
                                                                                                       IS_LEGAL,
                                                                                                       CUSTOMER_PAYMENT_ID,
                                                                                                       IS_CHANGE,
                                                                                                       USED_USER_ID)
                                                                        VALUES(CRS_USER.INSURANCE_PAYMENT_SEQUENCE.NEXTVAL,
                                                                                {InsuranceID},
                                                                                TO_DATE('{PaymentDate.Text}','DD.MM.YYYY'),
                                                                                {Math.Round(InsurancePayedAmount.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                                                          
                                                                                1,
                                                                                {PaymentID},
                                                                                1,
                                                                                {GlobalVariables.V_UserID})",
                                                $@"UPDATE CRS_USER_TEMP.CONTRACT_PENALTY_TEMP SET PAYED_PENALTY = {Math.Round(PenaltyAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                                  PENALTY_DEBT = {Math.Round(penaltyDebt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                                  CUSTOMER_PAYMENT_ID = {PaymentID},
                                                                                                  IS_CHANGE = 1
                                                            WHERE CONTRACT_ID = {ContractID} AND ID = {PenaltyID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Ödəniş temp cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertPayment");
            else if (TransactionName == "INSERT" && ((InsurancePayedCheck.Checked && InsurancePayedAmount.Value == 0) || !InsurancePayedCheck.Checked))
                GlobalProcedures.ExecuteThreeQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP(ID,
                                                                                                    CUSTOMER_ID,
                                                                                                    CONTRACT_ID,
                                                                                                    PAYMENT_DATE,
                                                                                                    PAYMENT_AMOUNT,
                                                                                                    BASIC_AMOUNT,
                                                                                                    DEBT,
                                                                                                    DAY_COUNT,
                                                                                                    ONE_DAY_INTEREST_AMOUNT,
                                                                                                    INTEREST_AMOUNT,
                                                                                                    PAYMENT_INTEREST_AMOUNT,
                                                                                                    PAYMENT_INTEREST_DEBT,
                                                                                                    TOTAL,
                                                                                                    CURRENCY_RATE,
                                                                                                    PAYMENT_AMOUNT_AZN,
                                                                                                    REQUIRED_CLOSE_AMOUNT,
                                                                                                    REQUIRED_AMOUNT,
                                                                                                    PAYMENT_NAME,
                                                                                                    NOTE,
                                                                                                    USED_USER_ID,
                                                                                                    IS_CHANGE,
                                                                                                    BANK_CASH,                                                                                                    
                                                                                                    IS_PENALTY,
                                                                                                    PENALTY_AMOUNT,
                                                                                                    PAYED_PENALTY,
                                                                                                    PENALTY_DEBT,
                                                                                                    BANK_ID,
                                                                                                    CUSTOMER_TYPE_ID,
                                                                                                    IS_BASIC_AMOUNT,
                                                                                                    PAYED_AMOUNT,
                                                                                                    PAYED_AMOUNT_AZN,
                                                                                                    INSURANCE_CHECK,
                                                                                                    INSURANCE_AMOUNT,
                                                                                                    CONTRACT_PERCENT,
                                                                                                    OVERDUE_PERCENT,
                                                                                                    OVERDUE_PERCENT_DEBT,
                                                                                                    DELAYS_AMOUNT,
                                                                                                    NORMAL_DEBT,
                                                                                                    IS_CHANGED_INTEREST)
                                                        VALUES({PaymentID},
                                                                {CustomerID},
                                                                {ContractID},
                                                                TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                                                {paymentValue.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {DayCountText.Text},
                                                                {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {requiredamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                '{NameText.Text.Trim()}',
                                                                '{NoteText.Text.Trim()}',
                                                                {GlobalVariables.V_UserID},
                                                                1,
                                                                'B',                                                                
                                                                {is_penalty},
                                                                {Math.Round(PenaltyAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(PayedPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(penaltyDebt, 2).ToString(GlobalVariables.V_CultureInfoEN)}, 
                                                                {bank_id},
                                                                {CustomerTypeID},1,
                                                                {Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(PaymentAZNValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {(InsurancePayedCheck.Checked ? 1 : 0)},
                                                                {(InsurancePayedCheck.Checked ? InsurancePayedAmount.Value.ToString(GlobalVariables.V_CultureInfoEN) : "0")},
                                                                {Interest.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(OverduePercentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(overdue_percent_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(DelaysAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {Math.Round(NormDebtValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {(ChangedInterestCheck.Checked ? 1 : 0)})",
                                                 $@"INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,
                                                                                                    BANK_ID,
                                                                                                    OPERATION_DATE,
                                                                                                    APPOINTMENT_ID,
                                                                                                    INCOME,
                                                                                                    EXPENSES,
                                                                                                    DEBT,
                                                                                                    NOTE,
                                                                                                    IS_CHANGE,
                                                                                                    USED_USER_ID,
                                                                                                    CONTRACT_PAYMENT_ID,
                                                                                                    CONTRACT_CODE,
                                                                                                    ACCOUNTING_PLAN_ID) 
                                                                            VALUES(CRS_USER.BANK_OPERATION_SEQUENCE.NEXTVAL,
                                                                                    {bank_id},
                                                                                    TO_DATE('{PaymentDate.Text.Trim()}','DD/MM/YYYY'),
                                                                                    3,
                                                                                    {Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                    0,
                                                                                    0,'{NoteText.Text.Trim()}',
                                                                                    1,
                                                                                    {GlobalVariables.V_UserID},
                                                                                    {PaymentID},
                                                                                    '{ContractCodeText.Text.Trim()}',
                                                                                    {accounting_plan_id})",
                                                    $@"UPDATE CRS_USER_TEMP.CONTRACT_PENALTY_TEMP SET PAYED_PENALTY = {Math.Round(PenaltyAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                                  PENALTY_DEBT = {Math.Round(penaltyDebt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                                  CUSTOMER_PAYMENT_ID = {PaymentID},
                                                                                                  IS_CHANGE = 1
                                                            WHERE CONTRACT_ID = {ContractID} AND ID = {PenaltyID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Ödəniş temp cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertPayment");
            else
                GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP(ID,
                                                                                                    CUSTOMER_ID,
                                                                                                    CONTRACT_ID,
                                                                                                    PAYMENT_DATE,
                                                                                                    PAYMENT_AMOUNT,
                                                                                                    BASIC_AMOUNT,
                                                                                                    DEBT,
                                                                                                    DAY_COUNT,
                                                                                                    ONE_DAY_INTEREST_AMOUNT,
                                                                                                    INTEREST_AMOUNT,
                                                                                                    PAYMENT_INTEREST_AMOUNT,
                                                                                                    PAYMENT_INTEREST_DEBT,
                                                                                                    TOTAL,
                                                                                                    CURRENCY_RATE,
                                                                                                    PAYMENT_AMOUNT_AZN,
                                                                                                    REQUIRED_CLOSE_AMOUNT,
                                                                                                    REQUIRED_AMOUNT,
                                                                                                    PAYMENT_NAME,
                                                                                                    NOTE,
                                                                                                    USED_USER_ID,
                                                                                                    IS_CHANGE,
                                                                                                    BANK_CASH,                                                                                                    
                                                                                                    IS_PENALTY,
                                                                                                    PENALTY_AMOUNT,
                                                                                                    PAYED_PENALTY,
                                                                                                    PENALTY_DEBT,
                                                                                                    BANK_ID,
                                                                                                    CUSTOMER_TYPE_ID,
                                                                                                    IS_BASIC_AMOUNT,
                                                                                                    PAYED_AMOUNT,
                                                                                                    PAYED_AMOUNT_AZN,
                                                                                                    INSURANCE_CHECK,
                                                                                                    INSURANCE_AMOUNT,
                                                                                                    CONTRACT_PERCENT,
                                                                                                    OVERDUE_PERCENT,
                                                                                                    OVERDUE_PERCENT_DEBT,
                                                                                                    DELAYS_AMOUNT,
                                                                                                    NORMAL_DEBT,
                                                                                                    IS_CHANGED_INTEREST)
                                                    VALUES({PaymentID},
                                                            {CustomerID},
                                                            {ContractID},
                                                            TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                                            {paymentValue.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {DayCountText.Text},
                                                            {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) },
                                                            {Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {requiredamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            '{NameText.Text.Trim()}',
                                                            '{NoteText.Text.Trim()}',
                                                            {GlobalVariables.V_UserID},
                                                            1,
                                                            'B',                                                            
                                                            {is_penalty},
                                                            {Math.Round(PenaltyAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(PayedPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(penaltyDebt, 2).ToString(GlobalVariables.V_CultureInfoEN)},           
                                                            {bank_id},
                                                            {CustomerTypeID},
                                                            1,
                                                            {Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(PaymentAZNValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {(InsurancePayedCheck.Checked ? 1 : 0)},
                                                            {(InsurancePayedCheck.Checked ? InsurancePayedAmount.Value.ToString(GlobalVariables.V_CultureInfoEN) : "0")},
                                                            {Interest.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(OverduePercentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(overdue_percent_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(DelaysAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(NormDebtValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {(ChangedInterestCheck.Checked ? 1 : 0)})",
                                                   $@"UPDATE CRS_USER_TEMP.CONTRACT_PENALTY_TEMP SET PAYED_PENALTY = {Math.Round(PenaltyAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                                  PENALTY_DEBT = {Math.Round(penaltyDebt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                                  CUSTOMER_PAYMENT_ID = {PaymentID},
                                                                                                  IS_CHANGE = 1
                                                            WHERE CONTRACT_ID = {ContractID} AND ID = {PenaltyID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Ödəniş temp cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertPayment");
            GlobalProcedures.InsertOperationJournal(PaymentDate.Text, CurrencyRateValue.Value, currency_id, paymentValue, payment_value_AZN, basic_amount, payment_interest_amount, ContractID.ToString(), PaymentID, OperationAccountLookUp.Text, 1, PayedPenaltyValue.Value);
        }

        private void PaymentDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            if ((String.IsNullOrEmpty(PaymentDate.Text)) || (String.IsNullOrEmpty(LastDateText.Text)))
                return;

            if (currency_id != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(currency_id, PaymentDate.Text);
                CurrencyRateLabel.Text = "1 " + Currency + " = ";
                CurrencyRateValue.Value = (decimal)cur;
            }
            else if (currency_id == 1)
                cur = 1;
            OverduePercentValue.EditValue = FPayment.OverduePercent(ContractID, PaymentDate.Text);
            PaymentInterestDebtValue.EditValue = PaymentInterestDebt;
            double startPercent = GlobalFunctions.ContractStartPercent(ContractID);

            DateTime todate = PaymentDate.DateTime,
                     fromDate = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
            DateTime? prevPercentEndDate = null;
            diff_day = 0;
            lstPercentDate = ContractPercentDateDAL.SelectContractPercentDate(ContractID).ToList<ContractPercentDate>();
            if (lstPercentDate.Count > 0)
            {
                lstPercentDate = lstPercentDate.Where(item => item.START_DATE >= fromDate || (item.END_DATE.HasValue && fromDate <= item.END_DATE)).ToList();
                foreach (var percent in lstPercentDate)
                {
                    if (todate <= percent.START_DATE.AddDays(-1))
                        continue;

                    todate = percent.START_DATE.AddDays(-1);

                    if (prevPercentEndDate.HasValue)
                        fromDate = prevPercentEndDate.Value.AddDays(1);

                    diff_day = diff_day + GlobalFunctions.Days360(fromDate, todate);

                    prevPercentEndDate = percent.END_DATE;
                }

                if (prevPercentEndDate.HasValue && prevPercentEndDate > fromDate)
                {
                    fromDate = prevPercentEndDate.Value.AddDays(1);
                    todate = PaymentDate.DateTime;
                }

                if (prevPercentEndDate.HasValue)
                    diff_day = diff_day + GlobalFunctions.Days360(fromDate, todate);
                else if (lstPercentDate.Count == 0)
                    diff_day = GlobalFunctions.Days360(fromDate, todate);
            }
            else
                diff_day = GlobalFunctions.Days360(fromDate, todate);            
            DayCountText.EditValue = diff_day;
            interest_amount = (diff_day * one_day_interest) + startPercent;
            residual_percent = Math.Round(interest_amount + PaymentInterestDebt, 2);
            PayedPercentValue.EditValue = ChangedInterestCheck.Checked ? PayedPercentValue.EditValue : (decimal)residual_percent;

            totaldebtamount = Math.Round(Debt + residual_percent, 2);

            TotalDebtValue.Value = (decimal)totaldebtamount + PayedPenaltyValue.Value;

            List<PaymentSchedules> lstSchedules = PaymentSchedulesDAL.SelectPaymentSchedules(ContractID).ToList<PaymentSchedules>();
            var schedules = lstSchedules.Where(s => s.MONTH_DATE <= GlobalFunctions.ChangeStringToDate(PaymentDate.Text, "ddmmyyyy")).ToList<PaymentSchedules>();
            if (schedules.Count == 0)
                order_id = 0;
            else
                order_id = schedules.Max(s => s.ORDER_ID);

            if (order_id == 0)
                normal_debt = Debt;
            else
                normal_debt = Math.Round(lstSchedules.Find(s => s.ORDER_ID == order_id).DEBT, 2); //qrafik uzre odenis
            NormDebtValue.EditValue = normal_debt;
            double delaysAmount = Debt - normal_debt;
            DelaysAmountValue.EditValue = delaysAmount > 0 ? delaysAmount : 0;
            requiredamount = delaysAmount + (double)OverduePercentValue.Value;
                
            RequiredValue.EditValue = requiredamount < 0? 0 : requiredamount;
            PaymentValue_EditValueChanged(sender, EventArgs.Empty);
            PaymentAZNValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(PaymentDate.Text);
            if (currency_id != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(currency_id, PaymentDate.Text);
                CurrencyRateLabel.Text = "1 " + Currency + " = ";
                CurrencyRateValue.EditValue = cur;
                PaymentValue_EditValueChanged(sender, EventArgs.Empty);
                PaymentAZNValue_EditValueChanged(sender, EventArgs.Empty);
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FPaymentBankAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshPaymentsDataGridView(PaymentAZNValue.Value, PaymentID);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPaymentDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPayment();
                else
                {
                    GlobalProcedures.ShowWarningMessage("Əgər " + ldate + " tarixinə olan ödənişi dəyişib yadda saxlasaz, bu zaman portfel və bank əməliyyatlarının qalığı yenidən hesablanacaq.");
                    GlobalProcedures.UpdateCustomerPayment(1,
                                                                PaymentDate.Text,
                                                                ContractID,
                                                                Math.Round(PaymentValue.Value, 2),
                                                                Math.Round(PayedPercentValue.Value, 2),
                                                                Math.Round(PaymentAZNValue.Value, 2),
                                                                CurrencyRateValue.Value,
                                                                NameText.Text.Trim(),
                                                                Convert.ToInt32(DayCountText.Text.Trim()),
                                                                OneDayInterestValue.Value,
                                                                (decimal)Math.Round(interest_amount, 2),
                                                                TotalDebtValue.Value,
                                                                RequiredValue.Value,
                                                                Convert.ToInt32(PaymentID),
                                                                PayedPenaltyValue.Value,
                                                                is_penalty,
                                                                OperationAccountLookUp.Text,
                                                                NoteText.Text.Trim(),
                                                                InsurancePayedCheck.Checked ? 1 : 0,
                                                                InsurancePayedCheck.Checked ? InsurancePayedAmount.Value : 0,
                                                                OverduePercentValue.Value);

                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP SET BANK_ID = {bank_id} WHERE ID = {PaymentID}",                                                     
                                                            "Bank əməliyyatı dəyişdirilmədi.",
                                                       this.Name + "/BOK_Click");

                }
                if (ReceiptCheck.Checked)
                    GlobalProcedures.LoadBankReceipt(PaymentDate.Text, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, currency_id, (decimal)CurrencyRateValue.Value, Currency, NameText.Text.Trim(), ContractID, ContractCodeText.Text.Trim(), "saylı lizinq müqaviləsi üzrə ödəniş", NameText.Text.Trim());
                this.Close();
            }
        }

        private void ContractCodeText_EditValueChanged(object sender, EventArgs e)
        {
            //if (FormStatus && TransactionName == "BINSERT")
            //{
            //    double amount = 0;
            //    int interest = 24;

            //    if (ContractCodeText.Text.Length != 4)
            //        return;

            //    string sql = $@"SELECT C.CONTRACT_ID,
            //                           C.CUSTOMER_ID,
            //                           TO_CHAR (C.START_DATE, 'DD.MM.YYYY') START_DATE,
            //                           CUS.CUSTOMER_NAME,
            //                           C.CURRENCY_ID,
            //                           C.AMOUNT,
            //                           CUR.CODE CURRENCY_CODE,
            //                           C.INTEREST,
            //                           C.MONTHLY_AMOUNT
            //                      FROM CRS_USER.V_CONTRACTS C,       
            //                           CRS_USER.V_CUSTOMERS CUS,
            //                           CRS_USER.CURRENCY CUR
            //                     WHERE     C.USED_USER_ID = -1
            //                           AND C.STATUS_ID = 5
            //                           AND CUR.ID = C.CURRENCY_ID       
            //                           AND C.CUSTOMER_ID = CUS.ID
            //                           AND C.CONTRACT_CODE = '{ContractCodeText.Text.Trim()}'";
            //    DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/ContractCodeText_EditValueChanged");

            //    if (dt.Rows.Count > 0)
            //    {
            //        try
            //        {
            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                ContractID = Convert.ToInt32(dr["CONTRACT_ID"].ToString());
            //                CustomerID = Convert.ToInt32(dr["CUSTOMER_ID"].ToString());
            //                ContractStartDateText.Text = dr["START_DATE"].ToString();
            //                CustomerNameText.Text = dr["CUSTOMER_NAME"].ToString();
            //                currency_id = Convert.ToInt32(dr["CURRENCY_ID"].ToString());
            //                amount = Convert.ToDouble(dr["AMOUNT"].ToString());
            //                Currency = dr["CURRENCY_CODE"].ToString();
            //                interest = Convert.ToInt32(dr["INTEREST"].ToString());
            //                MonthlyPaymentValue.Value = Convert.ToDecimal(dr["MONTHLY_AMOUNT"].ToString());
            //            }

            //            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");

            //            int isTemp = 0;

            //            if (PaymentsDAL.PaymentTempIsEmpty(ContractID).HasValue && PaymentsDAL.PaymentTempIsEmpty(ContractID).Value == true)
            //                isTemp = 1;


            //            List<Payments> lstPayments = PaymentsDAL.SelectPayments(isTemp, ContractID).ToList<Payments>();

            //            if (lstPayments.Count == 0)
            //            {
            //                LastDateText.Visible = LastDateLabel.Visible = false;
            //                LastDateText.Text = ContractStartDateText.Text;
            //                debt = amount;
            //                PaymentInterestDebt = 0;
            //            }
            //            else
            //            {
            //                LastDateText.Visible = LastDateLabel.Visible = true;
            //                var lastPayments = lstPayments.Where(item => item.IS_CHANGE == 0 || item.IS_CHANGE == 1).LastOrDefault();

            //                LastDateText.Text = lastPayments.PAYMENT_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
            //                debt = Math.Round((double)lastPayments.DEBT, 2);
            //                PaymentInterestDebt = Math.Round((double)lastPayments.PAYMENT_INTEREST_DEBT, 2);
            //            }

            //            PaymentInterestDebtValue.EditValue = PaymentInterestDebt;

            //            if (currency_id == 1)
            //                PaymentLabel.Text = "Ödənişin məbləği (" + Currency + " - ilə)";
            //            CurrencyLabel.Text = Currency;
            //            DebtValue.Value = (decimal)debt;
            //            InterestText.Text = interest.ToString();
            //            one_day_interest = Math.Round(((debt * interest) / 100) / 360, 2);
            //            OneDayInterestValue.Value = (decimal)one_day_interest;

            //            string commitment_name = GlobalFunctions.GetName($@"SELECT CC.COMMITMENT_NAME FROM CRS_USER.V_COMMITMENTS CC WHERE CC.CONTRACT_ID = {ContractID}");
            //            if (String.IsNullOrEmpty(commitment_name))
            //            {
            //                CommitmentLabel.Visible = false;
            //                NameText.Text = CustomerNameText.Text;
            //            }
            //            else
            //            {
            //                CommitmentLabel.Visible = true;
            //                NameText.Text = commitment_name;
            //            }

            //            PenaltyCurrencyLabel.Text =
            //                PayedPenaltyCurrencyLabel.Text = Currency;
            //            PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
            //            PaymentDate.EditValue = GlobalFunctions.ChangeStringToDate(PDate, "ddmmyyyy");
            //        }
            //        catch (Exception exx)
            //        {
            //            GlobalProcedures.LogWrite("Müqavilənin rekvizitləri tapılmadı.", sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            //        }
            //    }
            //    else
            //    {
            //        ContractStartDateText.Text =
            //            CustomerNameText.Text =
            //            LastDateText.Text =
            //            NameText.Text =
            //            InterestText.Text =
            //            CurrencyLabel.Text =
            //            DayCountText.Text = null;

            //        DebtValue.Value =
            //            PaymentInterestDebtValue.Value =
            //            OneDayInterestValue.Value =
            //            MonthlyPaymentValue.Value =
            //            TotalDebtValue.Value =
            //            RequiredValue.Value =
            //            PaymentAZNValue.Value =
            //            PaymentValue.Value =
            //            PenaltyAmountValue.Value = 0;

            //        CurrencyRateLabel.Visible =
            //            CurrencyRateValue.Visible =
            //            RateAZNLabel.Visible = false;

            //        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            //        CustomerID = 0;
            //        ContractID = 0;
            //        PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate("01.01.1900", "ddmmyyyy");
            //    }
            //}
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        void RefreshOperationsAccount()
        {
            GlobalProcedures.FillLookUpEdit(OperationAccountLookUp, "ACCOUNTING_PLAN", "ID", "SUB_ACCOUNT", "BANK_ID = " + bank_id + " ORDER BY  ID,ACCOUNT_NUMBER");
        }
    }
}