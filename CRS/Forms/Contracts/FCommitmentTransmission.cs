using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using CRS.Class.Views;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CRS.Forms.Contracts
{
    public partial class FCommitmentTransmission : DevExpress.XtraEditors.XtraForm
    {
        public FCommitmentTransmission()
        {
            InitializeComponent();
            MaximumSize = MinimumSize = Size;
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string TransactionName,
            CommitmentID,
            Customer,
            CustomerName,
            CustomerCode,
            CustomerPhone,
            CustomerAddress,
            CustomerCard,
            CustomerPincode,
            ContractCode,
            Account,
            ContractStartDate,
            ContractEndDate,
            CustomerID,
            ContractID,
            Lizinq,
            Currency,
            CreditName,
            DebtCurrency,
            LetterHostage,
            CarNumber,
            CarColor,
            CarType,
            CarYear,
            CarEngineNumber,
            BrandAndModel,
            Voen,
            Ban,
            Chassis;

        public int Interest,
            Period,
            CreditNameID,
            Count,
            LiquidCurrencyID,
            LiquidBankID,
            DebtCurrencyID,
            ParentID,
            CustomerTypeID,
            ParentPersonTypeID;

        public decimal Debt,
            Amount,
            LiquidValue;
        public bool Commit;

        public DateTime AgreementDateMinValue;

        decimal currecnt_debt, interest_debt = 0;
        double monthly_amount = 0;
        string PhoneID, PhoneNumber, copy_file_name, letterNumber, referenceNumber, letterCustomerName, customerDrivingLicense, letterCommitmentName, commitmentDrivingLicense;
        int card_series_id, card_issuing_id, scheduleVersion = 0, month_count, birthplace_id = 0;
        static int file_count = 0;
        bool CurrentStatus = false, letterDetailsOK = false, FormStatus = false, clickOK = false;

        public delegate void DoEvent();
        public event DoEvent RefreshCommitmentsDataGridView;

        private void PeriodValue_EditValueChanged(object sender, EventArgs e)
        {
            if (PeriodValue.EditorContainsFocus)
                PeriodDate.DateTime = AgreementDate.DateTime.AddMonths((int)PeriodValue.Value);

            MonthlyAmount();
        }

        private void PrizeInterestValue_EditValueChanged(object sender, EventArgs e)
        {
            MonthlyAmount();
        }

        private void PeriodDate_EditValueChanged(object sender, EventArgs e)
        {
            if (AgreementDate.Text.Length > 0 && PeriodDate.Text.Length > 0 && !PeriodValue.EditorContainsFocus)
                PeriodValue.Value = GlobalFunctions.DifferenceTwoDateWithMonth(AgreementDate.DateTime, PeriodDate.DateTime);
        }

        private void PenaltyDebtValue_EditValueChanged(object sender, EventArgs e)
        {
            TotalDebt();
        }

        private void BirthdayDate_EditValueChanged(object sender, EventArgs e)
        {
            AgeLabel.Text = GlobalFunctions.CalculationAgeWithYear(BirthdayDate.DateTime, DateTime.Today);
        }

        private void BirthplaceLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 4);
        }

        private void BirthplaceLookUp_EditValueChanged(object sender, EventArgs e)
        {
            birthplace_id = GlobalFunctions.FindComboBoxSelectedValue("BIRTHPLACE", "NAME", "ID", BirthplaceLookUp.Text);
        }

        private void MonthlyAmount()
        {
            if (!FormStatus)
                return;

            if (AgreementDate.Text.Length < 0 || PeriodDate.Text.Length < 0 || PrizeInterestValue.Value == 0)
                return;

            monthly_amount = GlobalFunctions.CalcPayment((double)TotalDebtValue.Value, (double)PeriodValue.Value, (double)PrizeInterestValue.Value);
            MonthlyAmountValue.EditValue = monthly_amount;
        }

        void TotalDebt()
        {
            if (!FormStatus)
                return;

            TotalDebtValue.EditValue = DebtValue.Value + InterestDebtValue.Value + PenaltyDebtValue.Value;
        }

        private void DebtValue_EditValueChanged(object sender, EventArgs e)
        {
            TotalDebt();
        }

        private void InterestDebtValue_EditValueChanged(object sender, EventArgs e)
        {
            TotalDebt();
        }

        private void FCommitmentTransmission_Load(object sender, EventArgs e)
        {
            RefreshDictionaries(4);
            DateTime? lastClosedDay = DateTime.Today;
            DateTime dateTime = AgreementMinDate(CommitmentID);
            AgreementDate.Properties.MinValue = lastClosedDay.HasValue && lastClosedDay >= dateTime ? lastClosedDay.Value : dateTime;
            AgreementDate.Properties.MaxValue = GlobalFunctions.ChangeStringToDate(ContractEndDate, "ddmmyyyy");

            IssueDate.Properties.MaxValue = ReliableDate.Properties.MinValue = DateTime.Today;
            CustomerNameText.Text = CustomerName;
            LizinqObjectText.Text = Lizinq;
            List<ContractExtend> lstExtend = ContractExtendDAL.SelectContractExtend(0, int.Parse(ContractID)).ToList<ContractExtend>();
            GlobalProcedures.FillComboBoxEdit(SeriesComboBox, "CARD_SERIES", "SERIES,SERIES,SERIES", "1 = 1 ORDER BY ORDER_ID");
            SeriesComboBox.SelectedIndex = 0;
            GlobalProcedures.FillComboBoxEdit(IssuingComboBox, "CARD_ISSUING", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");

            if (TransactionName == "INSERT")
            {
                FormStatus = true;

                AgreementDate.EditValue = AgreementDate.Properties.MinValue;
                CommitmentID = GlobalFunctions.GetOracleSequenceValue("COMMITMENT_SEQUENCE").ToString();
                PeriodDate.EditValue = GlobalFunctions.ChangeStringToDate(ContractEndDate, "ddmmyyyy");
                PeriodDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(ContractStartDate, "ddmmyyyy");

                DebtCurrencyLabel.Text = InterestDebtCurrencyLabel.Text = TotalDebtCurrencyLabel.Text = MonthlyAmountCurrencyLabel.Text = DebtCurrency;

                if (DebtValue.Value == 0)
                    RefreshTotals();

                BCreateFile.Visible = GlobalVariables.CommitContract;
            }
            else
            {
                InsertPhonesTemp();
                LoadCommitmentDetails();
                FormStatus = true;

                int pay_count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT CONTRACT_ID,PAYMENT_DATE FROM CRS_USER.CUSTOMER_PAYMENTS UNION ALL SELECT CONTRACT_ID,PAYMENT_DATE FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP) WHERE PAYMENT_DATE > TO_DATE('{AgreementDate.Text}','DD.MM.YYYY') AND CONTRACT_ID = {ContractID}");
                if (pay_count > 0)
                {
                    GlobalProcedures.ShowWarningMessage("Bu öhdəlik ilə " + AgreementDate.Text + " tarixindən sonra ödəniş olduğu üçün bu öhdəliyi dəyişmək olmaz. Siz yalnız məlumatlara baxa bilərsiz.");
                    CurrentStatus = true;
                }
                else
                {
                    var lstCommitment = CommitmentsDAL.SelectAllCommitmentTempByContractID(int.Parse(ContractID)).ToList<ContractCommitment>();
                    if (lstCommitment.Count(item => item.ID > int.Parse(CommitmentID)) > 0)
                    {
                        GlobalProcedures.ShowWarningMessage("Bu öhdəlik özündən sonra başqa öhdəliklər olduğu üçün dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.");
                        CurrentStatus = true;
                    }
                    else if (lstExtend.Count(item => item.START_DATE > AgreementDate.DateTime) > 0)
                    {
                        GlobalProcedures.ShowWarningMessage("Bu öhdəlik özündən sonra yeni qrafik olduğu üçün dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.");
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }

                ComponentEnable(CurrentStatus);
            }
            LoadPhoneDataGridView();

            scheduleVersion = GlobalFunctions.GetMax($@"SELECT NVL(MAX(VERSION), 0) FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP WHERE CONTRACT_ID = {ContractID} AND COMMITMENT_ID < {CommitmentID}", this.Name + "/FCommitmentTransmission_Load") + 1;
        }

        private void ComponentEnable(bool status)
        {
            AgreementDate.Enabled =
                NameText.Enabled =
                BirthdayDate.Enabled =
                BirthplaceLookUp.Enabled =
                SeriesComboBox.Enabled =
                NumberText.Enabled =
                PinCodeText.Enabled =
                VoenText.Enabled =
                AccountText.Enabled =
                IssueDate.Enabled =
                ReliableDate.Enabled =
                IssuingComboBox.Enabled =
                AddressText.Enabled =
                PhoneStandaloneBarDockControl.Enabled =
                PeriodDate.Enabled =
                PeriodValue.Enabled =
                DebtValue.Enabled =
                PrizeInterestValue.Enabled =
                AdvancePaymentValue.Enabled =
                ServiceAmountValue.Enabled =
                CheckPaymentSchedule.Enabled =
                PenaltyDebtValue.Enabled =
                BOK.Visible = !status;
        }

        private DateTime AgreementMinDate(string id)
        {
            DateTime res = AgreementDateMinValue;
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT NVL (MAX (PAYMENT_DATE), TO_DATE('{ContractStartDate}','DD.MM.YYYY'))
                                                                      FROM (SELECT PAYMENT_DATE
                                                                              FROM CRS_USER.CUSTOMER_PAYMENTS
                                                                             WHERE CONTRACT_ID = {ContractID}
                                                                                  {(!string.IsNullOrEmpty(id) ? $@" AND PAYMENT_DATE < (SELECT MAX(AGREEMENTDATE)
                                                                                                                                             FROM CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP
                                                                                                                                            WHERE     IS_CHANGE != 2
                                                                                                                                                  AND CONTRACT_ID = {ContractID}
                                                                                                                                                      AND ID = {id})" : null)} 
                                                                            UNION ALL
                                                                            SELECT AGREEMENTDATE + 1 PAYMENT_DATE
                                                                              FROM CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP
                                                                             WHERE IS_CHANGE != 2 AND CONTRACT_ID = {ContractID}{(id != null ? $@" AND ID != {id}" : null)}
                                                                            UNION ALL
                                                                            SELECT START_DATE + 1 PAYMENT_DATE
                                                                              FROM CRS_USER.CONTRACT_EXTEND
                                                                             WHERE CONTRACT_ID = {ContractID}
                                                                            UNION ALL
                                                                            SELECT START_DATE + 1 PAYMENT_DATE
                                                                              FROM CRS_USER_TEMP.CONTRACT_EXTEND_TEMP
                                                                             WHERE IS_CHANGE != 2 AND CONTRACT_ID = {ContractID})", this.Name + "/AgreementMinDate", "Öhdəlik üçün ən kiçik tarix açılmadı.");

            if (dt.Rows.Count > 0)
                res = Convert.ToDateTime(dt.Rows[0][0]);

            return res;
        }

        private void LoadCommitmentDetails()
        {
            string s = $@"SELECT CC.AGREEMENTDATE,
                               CC.COMMITMENT_NAME,
                               CC.BIRTHDAY,
                               B.NAME BIRTHPLACE_NAME,
                               CS.SERIES,
                               CC.CARD_NUMBER,
                               CC.CARD_PINCODE,
                               CC.CARD_ISSUE_DATE,
                               CC.CARD_RELIABLE_DATE,
                               CI.NAME,
                               CC.ADDRESS,
                               CC.DEBT,
                               CC.PAYMENT_INTEREST_DEBT,
                               CC.PENALTY_DEBT,
                               CC.IS_ADD_PAYMENT_DEBT,
                               C.CODE,
                               CC.PERIOD_DATE,
                               CC.INTEREST,
                               CC.ADVANCE_PAYMENT,
                               CC.SERVICE_AMOUNT,
                               CC.VOEN,
                               CC.ACCOUNT_NUMBER,
                               CC.IS_PAYMENT_SCHEDULE,
                               CC.TOTAL_DEBT,
                               CC.MONTHLY_AMOUNT
                          FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP CC,
                               CRS_USER.CARD_SERIES CS,
                               CRS_USER.CARD_ISSUING CI,
                               CRS_USER.CURRENCY C,
                               CRS_USER.BIRTHPLACE B
                         WHERE CC.CARD_SERIES_ID = CS.ID
                               AND CC.CARD_ISSUING_ID = CI.ID
                               AND CC.CURRENCY_ID = C.ID
                               AND CC.BIRTHPLACE_ID = B.ID(+)
                               AND CC.ID = {CommitmentID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCommitmentDetails").Rows)
                {
                    AgreementDate.EditValue = DateTime.Parse(dr["AGREEMENTDATE"].ToString());
                    NameText.Text = dr["COMMITMENT_NAME"].ToString();
                    if (dr["BIRTHDAY"].ToString() != string.Empty)
                        BirthdayDate.EditValue = DateTime.Parse(dr["BIRTHDAY"].ToString());
                    BirthplaceLookUp.EditValue = BirthplaceLookUp.Properties.GetKeyValueByDisplayText(dr["BIRTHPLACE_NAME"].ToString());
                    SeriesComboBox.EditValue = dr["SERIES"].ToString();
                    NumberText.Text = dr["CARD_NUMBER"].ToString();
                    PinCodeText.Text = dr["CARD_PINCODE"].ToString();
                    IssueDate.EditValue = DateTime.Parse(dr["CARD_ISSUE_DATE"].ToString());
                    ReliableDate.EditValue = DateTime.Parse(dr["CARD_RELIABLE_DATE"].ToString());
                    IssuingComboBox.EditValue = dr["NAME"].ToString();
                    AddressText.Text = dr["ADDRESS"].ToString();
                    DebtValue.Value = Convert.ToDecimal(dr["DEBT"]);
                    InterestDebtValue.Value = Convert.ToDecimal(dr["PAYMENT_INTEREST_DEBT"]);
                    PenaltyDebtValue.Value = Convert.ToDecimal(dr["PENALTY_DEBT"]);
                    TotalDebtValue.Value = Convert.ToDecimal(dr["TOTAL_DEBT"]);
                    DebtCurrencyLabel.Text = InterestDebtCurrencyLabel.Text = TotalDebtCurrencyLabel.Text = dr["CODE"].ToString();
                    PeriodDate.EditValue = DateTime.Parse(dr["PERIOD_DATE"].ToString());
                    PrizeInterestValue.EditValue = Convert.ToInt32(dr["INTEREST"]);
                    AdvancePaymentValue.Value = Convert.ToDecimal(dr["ADVANCE_PAYMENT"]);
                    ServiceAmountValue.Value = Convert.ToDecimal(dr["SERVICE_AMOUNT"]);
                    VoenText.Text = dr["VOEN"].ToString();
                    AccountText.Text = dr["ACCOUNT_NUMBER"].ToString();
                    CheckPaymentSchedule.Checked = Convert.ToInt16(dr["IS_PAYMENT_SCHEDULE"]) == 1;
                    MonthlyAmountValue.EditValue = Convert.ToDecimal(dr["MONTHLY_AMOUNT"]);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Öhdəliyin parametrləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                clickOK = true;
                GlobalFunctions.RunInOneTransaction<int>(tran =>
                {
                    CalculatedPaymentScheduleTemp(tran);

                    if (TransactionName == "INSERT")
                        InsertCommitment(tran);
                    else
                        UpdateCommitment(tran);
                    //InsertOperation(tran);
                    UpdatePhoneSendSms(tran);
                    return 1;
                }, TransactionName == "INSERT" ? "Öhdəlik ötürmənin məlumatları temp cədvələ daxil edilmədi." : "Öhdəlik ötürmənin məlumatları temp cədvəldə dəyişdirilmədi.");
                this.Close();
            }
        }

        private void InsertOperation(OracleTransaction tran)
        {
            OracleCommand command = tran.Connection.CreateCommand();
            command.CommandText = "CRS_USER.PROC_PERCENT_OPERATION_JOURNAL";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("P_ID", OracleDbType.Int32).Value = Convert.ToInt32(CommitmentID);
            command.Parameters.Add("P_DATE", OracleDbType.Varchar2).Value = AgreementDate.Text;
            command.Parameters.Add("P_PERCENT_AMOUNT", OracleDbType.Decimal).Value = InterestDebtValue.Value;
            command.Parameters.Add("P_PENALTY_AMOUNT", OracleDbType.Decimal).Value = PenaltyDebtValue.Value;
            command.Parameters.Add("P_CONTRACT_ID", OracleDbType.Int32).Value = ContractID;
            command.Parameters.Add("P_ACCOUNT_OPERATION_TYPE", OracleDbType.Int32).Value = 5;
            command.Parameters.Add("P_USED_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;

            if (tran != null)
                command.Transaction = tran;

            command.ExecuteNonQuery();
            command.Dispose();
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(NameText.Text.Trim()))
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Öhdəlik götürənin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.ChangeStringToDate(BirthdayDate.Text, "ddmmyyyy") >= DateTime.Today)
            {
                BirthdayDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin doğum tarixi cari tarixdən kiçik olmalıdır.");
                BirthdayDate.Focus();
                BirthdayDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BirthplaceLookUp.EditValue == null)
            {
                BirthplaceLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin doğum yeri daxil edilməyib.");
                BirthplaceLookUp.Focus();
                BirthplaceLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(SeriesComboBox.Text))
            {
                SeriesComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təyin edən sənədin seriyası daxil edilməyib.");
                SeriesComboBox.Focus();
                SeriesComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(NumberText.Text.Trim()))
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin nömrəsi daxil edilməyib.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((card_series_id == 2) && (NumberText.Text.Length != 8))
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi 8 simvol olmalıdır.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((card_series_id == 2 || card_series_id == 30) && PinCodeText.Text.Length != 7)
            {
                PinCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin pin kodu 7 simvol olmalıdır.");
                PinCodeText.Focus();
                PinCodeText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (!GlobalFunctions.Regexp("[0-9]{9}2", VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Fiziki şəxsin vöen-i yalnız rəqəmlərdən ibarət olmalıdır, ümumi uzunluğu 10 simvol olmalıdır və sonuncu simvol <b><color=104,0,0>2</color></b> olmalıdır.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(IssueDate.Text))
            {
                IssueDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin verilmə tarixi daxil edilməyib.");
                IssueDate.Focus();
                IssueDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (String.IsNullOrEmpty(ReliableDate.Text))
            {
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin etibarlılıq tarixi daxil edilməyib.");
                ReliableDate.Focus();
                ReliableDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (IssueDate.DateTime == ReliableDate.DateTime)
            {
                IssueDate.BackColor = Color.Red;
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi ilə etibarlı olma tarixi eyni ola bilməz.");
                IssueDate.Focus();
                IssueDate.BackColor = GlobalFunctions.ElementColor();
                ReliableDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(IssuingComboBox.Text))
            {
                IssuingComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təyin edən sənədin seriyası daxil edilməyib.");
                IssuingComboBox.Focus();
                IssuingComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(AddressText.Text))
            {
                AddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ünvan daxil edilməyib.");
                AddressText.Focus();
                AddressText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PhoneGridView.RowCount == 0)
            {
                PhoneGridControl.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Telefon nömrələri daxil edilməyib.");
                PhoneGridControl.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DebtValue.Value <= 0)
            {
                DebtValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qalıq borc sıfırdan böyük olmalıdır.");
                DebtValue.Focus();
                DebtValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(PeriodDate.Text))
            {
                PeriodDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Lizinq müddəti daxil edilməyib.");
                PeriodDate.Focus();
                PeriodDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PrizeInterestValue.Value <= 0)
            {
                PrizeInterestValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Lizinq verənin mükafatı daxil edilməyib.");
                PrizeInterestValue.Focus();
                PrizeInterestValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ServiceAmountValue.Value < 0)
            {
                ServiceAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Xidmət haqqı mənfi ədəd ola bilməz.");
                ServiceAmountValue.Focus();
                ServiceAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void AccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(AccountText, AccountLengthLabel);
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 2:
                    GlobalProcedures.FillComboBoxEdit(SeriesComboBox, "CARD_SERIES", "SERIES,SERIES,SERIES", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 3:
                    GlobalProcedures.FillComboBoxEdit(IssuingComboBox, "CARD_ISSUING", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 4:
                    GlobalProcedures.FillLookUpEdit(BirthplaceLookUp, "BIRTHPLACE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
        }

        private void AgreementDate_EditValueChanged(object sender, EventArgs e)
        {
            RefreshTotals();
            PeriodDate.Properties.MinValue = AgreementDate.DateTime;            
            if (AgreementDate.EditorContainsFocus && PeriodDate.Text.Length > 0)
                PeriodValue.Value = GlobalFunctions.DifferenceTwoDateWithMonth(AgreementDate.DateTime, PeriodDate.DateTime);
        }

        private void ApplicantionCheck_CheckedChanged(object sender, EventArgs e)
        {
            GlobalProcedures.ChangeCheckStyle(ApplicantionCheck);
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void SeriesComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 2);
        }

        private void VoenText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(VoenText, VoenLengthLabel);
        }

        private void IssuingComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 3);
        }

        void RefreshTotals()
        {
            if (!FormStatus)
                return;

            string sql = $@"SELECT T.DEBT,
                                   CRS_USER.TOTALS.PAYMENT_INTEREST_DEBT (T.P_STARTDATE,
                                                                       T.P_ENDDATE,
                                                                       T.DEBT,
                                                                       T.INTEREST,
                                                                       T.CONTRACT_ID,
                                                                       T.PAYMENT_INTEREST_DEBT)
                                            PAYMENT_INTEREST_DEBT,
                                   T.INTEREST
                          FROM (WITH T1
                                     AS (SELECT CON.INTEREST,
                                                CON.START_DATE,
                                                CON.AMOUNT,
                                                CON.CONTRACT_ID
                                           FROM CRS_USER.V_CONTRACTS CON
                                          WHERE     CON.IS_COMMIT = 1
                                                AND CON.START_DATE <=
                                                       TO_DATE ('{AgreementDate.Text}', 'DD/MM/YYYY')),
                                     T2
                                     AS (  SELECT CP.CONTRACT_ID,
                                                  SUM (CP.BASIC_AMOUNT) BASIC_AMOUNT,
                                                  SUM (CP.PAYMENT_INTEREST_AMOUNT)
                                                     PAYMENT_INTEREST_AMOUNT,
                                                  COUNT(*) PAY_COUNT
                                             FROM CRS_USER.CUSTOMER_PAYMENTS CP
                                            WHERE CP.PAYMENT_DATE <=
                                                     TO_DATE ('{AgreementDate.Text}', 'DD/MM/YYYY')
                                         GROUP BY CP.CONTRACT_ID),
                                     T3 AS (SELECT * FROM CRS_USER.V_CUSTOMER_LAST_PAYMENT)
                                SELECT NVL(T3.CONTRACT_PERCENT, T1.INTEREST) INTEREST,
                                       NVL (T3.DEBT, T1.AMOUNT) DEBT,
                                       NVL (T2.PAYMENT_INTEREST_AMOUNT, 0) PAYMENT_INTEREST_AMOUNT,
                                       NVL(T3.PAYMENT_INTEREST_DEBT, 0) PAYMENT_INTEREST_DEBT,
                                       T1.CONTRACT_ID,
                                       (CASE
                                           WHEN T3.PAYMENT_DATE IS NULL THEN T1.START_DATE
                                           ELSE T3.PAYMENT_DATE
                                        END)
                                          P_STARTDATE,
                                       TO_DATE ('{AgreementDate.Text}', 'DD/MM/YYYY') P_ENDDATE,
                                       NVL(T2.PAY_COUNT, 0) PAY_COUNT
                                  FROM T1
                                       LEFT JOIN T2 ON T1.CONTRACT_ID = T2.CONTRACT_ID
                                       LEFT JOIN T3 ON T1.CONTRACT_ID = T3.CONTRACT_ID) T
                         WHERE CONTRACT_ID = {ContractID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/FExtendTime_Load", "Qalıq tapılmadı.");
            if (dt.Rows.Count > 0)
            {
                currecnt_debt = Convert.ToDecimal(dt.Rows[0]["DEBT"]);
                interest_debt = Convert.ToDecimal(dt.Rows[0]["PAYMENT_INTEREST_DEBT"]);
                PrizeInterestValue.Value = Convert.ToInt32(dt.Rows[0]["INTEREST"]);
            }

            DebtValue.Value = currecnt_debt == 0 ? Debt : currecnt_debt;
            InterestDebtValue.Value = interest_debt;
        }

        private void LoadFPayment(string contract_id, string contract_code, int interest, int period, string s_date, string e_date, string lizinq, string customer_code, string customer_name, string customer_id, decimal amount, string currency)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FWait));
            Total.FPayment fp = new Total.FPayment();
            fp.ContractID = contract_id;
            fp.ContractCode = contract_code;
            fp.Interest = interest;
            fp.Period = period;
            fp.SDate = s_date;
            fp.EDate = e_date;
            fp.Lizinq = lizinq;
            fp.CustomerID = customer_id;
            fp.CustomerCode = customer_code;
            fp.CustomerName = customer_name;
            fp.Amount = (double)amount;
            fp.Currency = currency;
            fp.CustomerTypeID = CustomerTypeID;
            fp.RefreshTotalsDataGridView += new Total.FPayment.DoEvent(RefreshTotals);
            GlobalProcedures.SplashScreenClose();
            fp.ShowDialog();
        }

        private void CustomerPaymentsLabel_Click(object sender, EventArgs e)
        {
            LoadFPayment(ContractID, ContractCode, Interest, Period, ContractStartDate, ContractEndDate, Lizinq, CustomerCode, CustomerName, CustomerID, Amount, DebtCurrency);
        }

        private void PhoneGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PhoneGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void LoadPhoneDataGridView()
        {
            string s = $@"SELECT 1 SS,
                             P.ID,
                             PD.DESCRIPTION_AZ DESCRIPTION,
                             '+' || C.CODE || P.PHONE_NUMBER PHONE_NUMBER,
                             P.IS_SEND_SMS,
                             KR.NAME KINDSHIP_RATE_NAME
                        FROM CRS_USER_TEMP.PHONES_TEMP P,
                             CRS_USER.PHONE_DESCRIPTIONS PD,
                             CRS_USER.COUNTRIES C,
                             CRS_USER.KINDSHIP_RATE KR
                       WHERE     P.IS_CHANGE IN (0, 1)
                             AND P.PHONE_DESCRIPTION_ID = PD.ID
                             AND P.COUNTRY_ID = C.ID
                             AND P.OWNER_TYPE = 'CC'
                             AND P.KINDSHIP_RATE_ID = KR.ID(+)
                             AND P.OWNER_ID = {CommitmentID}
                    ORDER BY P.ORDER_ID";
            try
            {
                PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");

                if (PhoneGridView.RowCount > 0)
                    EditPhoneBarButton.Enabled = DeletePhoneBarButton.Enabled = true;
                else
                    EditPhoneBarButton.Enabled = DeletePhoneBarButton.Enabled = false;

                try
                {
                    PhoneGridView.BeginUpdate();
                    for (int i = 0; i < PhoneGridView.RowCount; i++)
                    {
                        DataRow row = PhoneGridView.GetDataRow(PhoneGridView.GetVisibleRowHandle(i));
                        if (Convert.ToInt32(row["IS_SEND_SMS"].ToString()) == 1)
                            PhoneGridView.SelectRow(i);
                    }
                }
                finally
                {
                    PhoneGridView.EndUpdate();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Telefon nömrələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void UpdatePhoneSendSms(OracleTransaction tran)
        {
            GlobalProcedures.ExecuteQuery(tran, $@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0 WHERE IS_SEND_SMS = 1 AND OWNER_TYPE = 'CC' AND OWNER_ID IN (SELECT ID FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID})");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < PhoneGridView.SelectedRowsCount; i++)
            {
                rows.Add(PhoneGridView.GetDataRow(PhoneGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteQuery(tran, $@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 1 WHERE ID = {row["ID"]}");
            }

            GlobalProcedures.ExecuteQuery(tran, $@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'CC' AND OWNER_ID IN (SELECT ID FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID})");
        }

        private void InsertPhonesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_INSERT_COMMIT_PHONE_TEMP", "P_CONTRACT_ID", int.Parse(ContractID), "P_COMMITMENT_ID", int.Parse(CommitmentID), "Öhdəlik götürənin telefonları temp cədvələ daxil edilmədi.");
        }

        void RefreshPhone()
        {
            LoadPhoneDataGridView();
        }

        public void LoadFPhoneAddEdit(string transaction, string OwnerID, string OwnerType, string PhoneID)
        {
            FPhoneAddEdit fp = new FPhoneAddEdit();
            fp.TransactionName = transaction;
            fp.OwnerID = OwnerID;
            fp.OwnerType = OwnerType;
            fp.PhoneID = PhoneID;
            fp.RefreshPhonesDataGridView += new FPhoneAddEdit.DoEvent(RefreshPhone);
            fp.ShowDialog();
        }

        private void NewPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("INSERT", CommitmentID, "CC", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", CommitmentID, "CC", PhoneID);
        }

        private void DeletePhone()
        {
            if (GlobalFunctions.CallDialogResult(PhoneNumber + " nömrəsini silmək istəyirsiniz?", "Telefon nömrəsinin silinməsi") == DialogResult.Yes)
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'CC' AND ID = " + PhoneID, "Telefon nömrəsi temp cədvəldən silinmədi.");
        }

        private void DeletePhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhone();
            LoadPhoneDataGridView();
        }

        private void RefreshPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhoneDataGridView();
        }

        private void PhoneGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PhoneGridView.GetFocusedDataRow();
            if (row != null)
            {
                PhoneID = row["ID"].ToString();
                PhoneNumber = row["PHONE_NUMBER"].ToString();
            }
        }

        private void FCommitmentTransmission_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!clickOK && TransactionName == "EDIT" && !CurrentStatus)
            {
                if (GlobalFunctions.CallDialogResult("Əgər hər hansısa bir dəyişiklik etmisinizsə mütləq <b><color=105,0,0>Yadda saxla</color></b> düyməsini klikləyin. Əks halda dəyişikliklər bazada saxlanılmayacaq. Dəyişiklik saxlanılsın?", "Xatırlatma") == DialogResult.Yes)
                    e.Cancel = true;
            }

            this.RefreshCommitmentsDataGridView();
        }

        private void InsertCommitment(OracleTransaction tran)
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP(ID,
                                                                                    PARENT_ID,
                                                                                    CONTRACT_ID,
                                                                                    AGREEMENTDATE,
                                                                                    COMMITMENT_NAME,
                                                                                    BIRTHDAY,
                                                                                    BIRTHPLACE_ID,
                                                                                    CARD_SERIES_ID,
                                                                                    CARD_NUMBER,
                                                                                    CARD_PINCODE,
                                                                                    CARD_ISSUE_DATE,
                                                                                    CARD_RELIABLE_DATE,
                                                                                    CARD_ISSUING_ID,
                                                                                    ADDRESS,
                                                                                    DEBT,
                                                                                    PAYMENT_INTEREST_DEBT,
                                                                                    PENALTY_DEBT,
                                                                                    TOTAL_DEBT,
                                                                                    IS_ADD_PAYMENT_DEBT,
                                                                                    CURRENCY_ID,
                                                                                    PERIOD_DATE,
                                                                                    INTEREST,
                                                                                    ADVANCE_PAYMENT,
                                                                                    SERVICE_AMOUNT,
                                                                                    USED_USER_ID,
                                                                                    IS_CHANGE,
                                                                                    VOEN,
                                                                                    ACCOUNT_NUMBER,
                                                                                    MONTH_COUNT,
                                                                                    MONTHLY_AMOUNT,
                                                                                    VERSION,
                                                                                    IS_PAYMENT_SCHEDULE) 
                            VALUES({CommitmentID},
                                        {ParentID},
                                        {ContractID},
                                        TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                        '{NameText.Text.Trim()}',
                                        TO_DATE('{BirthdayDate.Text}','DD/MM/YYYY'),
                                        {birthplace_id},
                                        {card_series_id},
                                        '{NumberText.Text.Trim()}',
                                        '{PinCodeText.Text.Trim()}',
                                        TO_DATE('{IssueDate.Text}','DD/MM/YYYY'),
                                        TO_DATE('{ReliableDate.Text}','DD/MM/YYYY'),
                                        {card_issuing_id},
                                        '{AddressText.Text.Trim()}',
                                        {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {PenaltyDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        1,
                                        {DebtCurrencyID},
                                        TO_DATE('{PeriodDate.Text}','DD/MM/YYYY'),
                                        {PrizeInterestValue.Value},
                                        {AdvancePaymentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {ServiceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {GlobalVariables.V_UserID},
                                        1,
                                        '{VoenText.Text.Trim()}',
                                        '{AccountText.Text.Trim()}',
                                        {month_count},
                                        {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {scheduleVersion},
                                        {(CheckPaymentSchedule.Checked ? 1 : 0)})",
                                sql2 = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP(ID,
                                                                                                    PARENT_ID,
                                                                                                    CONTRACT_ID,
                                                                                                    AGREEMENTDATE,
                                                                                                    COMMITMENT_NAME,                                                                                    
                                                                                                    DEBT,
                                                                                                    PAYMENT_INTEREST_DEBT,
                                                                                                    PENALTY_DEBT,
                                                                                                    TOTAL_DEBT,
                                                                                                    IS_ADD_PAYMENT_DEBT,
                                                                                                    CURRENCY_ID,
                                                                                                    PERIOD_DATE,
                                                                                                    INTEREST,
                                                                                                    ADVANCE_PAYMENT,
                                                                                                    SERVICE_AMOUNT,
                                                                                                    VOEN,
                                                                                                    ACCOUNT_NUMBER,
                                                                                                    USED_USER_ID,
                                                                                                    IS_CHANGE,
                                                                                                    MONTH_COUNT,
                                                                                                    MONTHLY_AMOUNT,
                                                                                                    VERSION,
                                                                                                    IS_PAYMENT_SCHEDULE,
                                                                                                    ADDRESS) 
                                                    VALUES({CommitmentID},
                                                                {ParentID},
                                                                {ContractID},
                                                                TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                                                '{NameText.Text.Trim()}',                                        
                                                                {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {PenaltyDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                1,
                                                                {DebtCurrencyID},
                                                                TO_DATE('{PeriodDate.Text}','DD/MM/YYYY'),
                                                                {PrizeInterestValue.Value},
                                                                {AdvancePaymentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {ServiceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                '{VoenText.Text.Trim()}',
                                                                '{AccountText.Text.Trim()}',
                                                                {GlobalVariables.V_UserID},
                                                                1,
                                                                {month_count},
                                                                {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {scheduleVersion},
                                                                {(CheckPaymentSchedule.Checked ? 1 : 0)},
                                                                '{AddressText.Text.Trim()}')",
                                sql3 = $@"UPDATE CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP SET IS_ACTIVE = 0, IS_CHANGE = 1 WHERE ID = {ParentID}";

            GlobalProcedures.ExecuteThreeQuery(tran, sql, sql2, sql3);
        }

        private void UpdateCommitment(OracleTransaction tran)
        {
            string sql1 = $@"UPDATE CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP SET 
                                    AGREEMENTDATE = TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                    COMMITMENT_NAME = '{NameText.Text.Trim()}',
                                    BIRTHDAY = TO_DATE('{BirthdayDate.Text}','DD/MM/YYYY'),
                                    BIRTHPLACE_ID = {birthplace_id},
                                    CARD_SERIES_ID = {card_series_id},
                                    CARD_NUMBER = '{NumberText.Text.Trim()}',
                                    CARD_PINCODE = '{PinCodeText.Text.Trim()}',
                                    CARD_ISSUE_DATE = TO_DATE('{IssueDate.Text}','DD/MM/YYYY'),
                                    CARD_RELIABLE_DATE = TO_DATE('{ReliableDate.Text}','DD/MM/YYYY'),
                                    CARD_ISSUING_ID = {card_issuing_id},
                                    ADDRESS = '{AddressText.Text.Trim()}',
                                    DEBT = {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    PAYMENT_INTEREST_DEBT = {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    PENALTY_DEBT = {PenaltyDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    TOTAL_DEBT = {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    CURRENCY_ID = {DebtCurrencyID},
                                    PERIOD_DATE = TO_DATE('{PeriodDate.Text}','DD/MM/YYYY'),
                                    INTEREST = {PrizeInterestValue.Value},
                                    ADVANCE_PAYMENT = {AdvancePaymentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    SERVICE_AMOUNT = {ServiceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    IS_CHANGE = 1,
                                    VOEN = '{VoenText.Text.Trim()}',
                                    ACCOUNT_NUMBER = '{AccountText.Text.Trim()}',
                                    MONTH_COUNT = {month_count},
                                    MONTHLY_AMOUNT = {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                    VERSION = {scheduleVersion},
                                    IS_PAYMENT_SCHEDULE = {(CheckPaymentSchedule.Checked ? 1 : 0)},
                                    ETL_DT_TM = SYSDATE
                            WHERE CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {CommitmentID}",
                    sql2 = $@"UPDATE CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP SET 
                                    AGREEMENTDATE = TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                    COMMITMENT_NAME = '{NameText.Text.Trim()}',                                    
                                    DEBT = {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    PAYMENT_INTEREST_DEBT = {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    PENALTY_DEBT = {PenaltyDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    TOTAL_DEBT = {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    CURRENCY_ID = {DebtCurrencyID},
                                    PERIOD_DATE = TO_DATE('{PeriodDate.Text}','DD/MM/YYYY'),
                                    INTEREST = {PrizeInterestValue.Value},
                                    ADVANCE_PAYMENT = {AdvancePaymentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    SERVICE_AMOUNT = {ServiceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    VOEN = '{VoenText.Text.Trim()}',
                                    ACCOUNT_NUMBER = '{AccountText.Text.Trim()}',
                                    MONTH_COUNT = {month_count},
                                    MONTHLY_AMOUNT = {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                    VERSION = {scheduleVersion},
                                    IS_PAYMENT_SCHEDULE = {(CheckPaymentSchedule.Checked ? 1 : 0)},
                                    ADDRESS = '{AddressText.Text.Trim()}',
                                    IS_CHANGE = 1
                            WHERE CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {CommitmentID}";

            GlobalProcedures.ExecuteTwoQuery(tran, sql1, sql2);
        }

        private void SeriesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            card_series_id = GlobalFunctions.FindComboBoxSelectedValue("CARD_SERIES", "SERIES", "ID", SeriesComboBox.Text);
        }

        private void IssuingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            card_issuing_id = GlobalFunctions.FindComboBoxSelectedValue("CARD_ISSUING", "NAME", "ID", IssuingComboBox.Text);
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PopupMenu, e);
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled && PhoneStandaloneBarDockControl.Enabled)
                LoadFPhoneAddEdit("EDIT", CommitmentID, "CC", PhoneID);
        }

        private void NumberText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(NumberText, NumberLengthLabel);
            if (TransactionName == "INSERT")
                LoadOldData(NumberText.Text.Trim());
        }

        private void LoadOldData(string cardNumber)
        {
            if (cardNumber.Length < 7)
            {
                LoadPhoneDataGridView();
                return;
            }

            string sql = $@"SELECT CC.COMMITMENT_NAME,
                                   CC.CARD_PINCODE,
                                   CC.CARD_ISSUE_DATE,
                                   CC.CARD_RELIABLE_DATE,
                                   CC.ADDRESS,
                                   CS.SERIES,
                                   CI.NAME,
                                   CC.VOEN,
                                   CC.ID
                              FROM CRS_USER.CONTRACT_COMMITMENTS CC,
                                   CRS_USER.CARD_SERIES CS,
                                   CRS_USER.CARD_ISSUING CI
                             WHERE     CC.CARD_SERIES_ID = CS.ID
                                   AND CC.CARD_ISSUING_ID = CI.ID
                                   AND CC.ID = (SELECT MAX(ID) FROM CRS_USER.CONTRACT_COMMITMENTS WHERE CARD_NUMBER LIKE '%{cardNumber}')";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/", "Sənəd nömrəsinə görə öhdəlik götürən tapılmadı.");

            if (dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["COMMITMENT_NAME"].ToString();
                SeriesComboBox.EditValue = dt.Rows[0]["SERIES"];
                PinCodeText.Text = dt.Rows[0]["CARD_PINCODE"].ToString();
                IssueDate.EditValue = dt.Rows[0]["CARD_ISSUE_DATE"];
                ReliableDate.EditValue = dt.Rows[0]["CARD_RELIABLE_DATE"];
                IssuingComboBox.EditValue = dt.Rows[0]["NAME"];
                AddressText.Text = dt.Rows[0]["ADDRESS"].ToString();
                VoenText.Text = dt.Rows[0]["VOEN"].ToString();
                InsertOldPhone(int.Parse(dt.Rows[0]["ID"].ToString()));
            }
            LoadPhoneDataGridView();
        }

        private void InsertOldPhone(int oldCommitmentID)
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.PHONES_TEMP (ID,
                                             PHONE_DESCRIPTION_ID,
                                             COUNTRY_ID,
                                             PHONE_NUMBER,
                                             KINDSHIP_RATE_ID,
                                             KINDSHIP_NAME,
                                             IS_SEND_SMS,
                                             OWNER_ID,
                                             OWNER_TYPE,
                                             ORDER_ID,
                                             USED_USER_ID)
                             SELECT CRS_USER.PHONES_SEQUENCE.NEXTVAL ID,
                                    PHONE_DESCRIPTION_ID,
                                    COUNTRY_ID,
                                    PHONE_NUMBER,
                                    KINDSHIP_RATE_ID,
                                    KINDSHIP_NAME,
                                    IS_SEND_SMS,
                                    {CommitmentID} OWNER_ID,
                                    OWNER_TYPE,
                                    ORDER_ID,
                                    {GlobalVariables.V_UserID} USED_USER_ID
                               FROM CRS_USER.PHONES
                              WHERE OWNER_TYPE = 'CC' AND OWNER_ID ={oldCommitmentID}",
                   sql_delete = $@"DELETE FROM CRS_USER_TEMP.PHONES_TEMP WHERE OWNER_TYPE = 'CC' AND OWNER_ID = {CommitmentID} AND USED_USER_ID = {GlobalVariables.V_UserID}";

            GlobalProcedures.ExecuteTwoQuery(sql_delete, sql, "Telefon nömrələri temp cədvələ daxil edilmədi.", this.Text + "/InsertOldPhone");
        }

        private void NullData()
        {
            NameText.Text =
                   PinCodeText.Text =
                   AddressText.Text =
                   VoenText.Text = null;
            IssueDate.EditValue =
            ReliableDate.EditValue =
            IssuingComboBox.EditValue = null;
            SeriesComboBox.EditValue = null;
            GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER_TEMP.PROC_COMMITMENT_PHONE_DELETE", "P_COMMITMENT_ID", int.Parse(CommitmentID), "P_USED_USER_ID", GlobalVariables.V_UserID, "Öhdəlik götürənin telefonları temp cədvəldən silinmədi.");
        }

        private void PinCodeText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(PinCodeText, PinCodeLengthLabel);
        }

        private void UpPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "up", out orderid);
            LoadPhoneDataGridView();
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "down", out orderid);
            LoadPhoneDataGridView();
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        void GetLetterDetails(string number, string number2, string customerName, string drivingLicense1, string commitmentName, string drivingLicense2, bool cancel)
        {
            letterNumber = number;
            referenceNumber = number2;
            letterCustomerName = customerName;
            customerDrivingLicense = drivingLicense1;
            letterCommitmentName = commitmentName;
            commitmentDrivingLicense = drivingLicense2;
            letterDetailsOK = cancel;
        }

        private void LoadLetterDetails()
        {
            FCommitmentLetterDetails fl = new FCommitmentLetterDetails();
            fl.CustomerName = CustomerNameText.Text.Trim();
            fl.CommitmentName = NameText.Text.Trim();
            fl.GetDetails += new FCommitmentLetterDetails.DoEvent(GetLetterDetails);
            fl.ShowDialog();
        }

        private void InsertInterestPenaltiesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_PENELTIES_TEMP", "P_CONTRACT_ID", ContractID, "Cərimə faizləri temp cədvələ daxil edilmədi.");
        }

        private void BCreateFile_Click(object sender, EventArgs e)
        {
            object fileName = null, saveAs;

            if (CreditNameID == 5)
            {
                fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Öhdəlik ötürmə.docx");
                letterDetailsOK = true;
            }
            else if (CreditNameID == 1 && ApplicantionCheck.Checked)
            {
                LoadLetterDetails();
                fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Öhdəlik ötürmə (məktub ilə).docx");
            }
            else
            if (CreditNameID == 1 && !ApplicantionCheck.Checked)
            {
                fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Öhdəlik ötürmə.docx");
                letterDetailsOK = true;
            }

            if (!letterDetailsOK)
                return;

            if (!File.Exists((string)fileName))
            {
                GlobalProcedures.ShowWarningMessage("Öhdəlik ötürmənin şablon faylı " + GlobalVariables.V_ExecutingFolder + "\\Documents ünvanında yoxdur. Zəhmət olmasa şablon faylı göstərilən ünvanda yaradandan sonra bu düyməni klikləyin.");
                return;
            }

            if (ControlDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCommitmentFileWait));
                List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();
                string amount_with_word,
                    amount_with_word2,
                    amount_with_word3,
                    qep = null,
                    qep2 = null,
                    qep3 = null,
                    object_desc = null,
                    phone = null,
                    s_bankaccount = null,
                    date = GlobalFunctions.DateWithDayMonthYear(AgreementDate.DateTime),
                    letterDate = GlobalFunctions.DateWithYear(AgreementDate.DateTime),
                    bank_account1 = " ",
                    bank_account2 = " ",
                    liquid_currency_name = lstCurrency.Find(c => c.ID == LiquidCurrencyID).NAME,
                    liquid_currency_small_name = lstCurrency.Find(c => c.ID == LiquidCurrencyID).SMALL_NAME,
                    currency_name = lstCurrency.Find(c => c.ID == DebtCurrencyID).NAME,
                    currency_small_name = lstCurrency.Find(c => c.ID == DebtCurrencyID).SMALL_NAME;

                if (CreditNameID == 1)
                    object_desc = "Marka, Model, Tip, Buraxılış ili, Rəngi, BAN";
                else if (CreditNameID == 5)
                    object_desc = "Ünvan, çıxarış, sahəsi";

                double d = (double)LiquidValue * 100;

                int div = (int)(d / 100), mod = (int)(d % 100);
                if (mod > 0)
                    qep = " " + mod.ToString() + " " + liquid_currency_small_name;

                amount_with_word = " (" + GlobalFunctions.IntegerToWritten(div) + ") " + liquid_currency_name + qep;

                double d2 = (double)Amount * 100;

                int div2 = (int)(d2 / 100), mod2 = (int)(d2 % 100);
                if (mod2 > 0)
                    qep2 = " " + mod2.ToString() + " " + currency_small_name;

                amount_with_word2 = " (" + GlobalFunctions.IntegerToWritten(div2) + ") " + currency_name + qep2;

                double d3 = (double)TotalDebtValue.Value * 100;

                int div3 = (int)(d3 / 100), mod3 = (int)(d3 % 100), i = 1;
                if (mod3 > 0)
                    qep3 = " " + mod3.ToString() + " " + currency_small_name;

                amount_with_word3 = " (" + GlobalFunctions.IntegerToWritten(div3) + " " + currency_name + qep3 + ") ";

                List<PhonesView> lstPhone = PhonesViewDAL.SelectPhone(int.Parse(CommitmentID), "CC").ToList<PhonesView>();
                if (lstPhone.Count > 0)
                    phone = lstPhone.First().PHONE;

                string voen1 = (String.IsNullOrWhiteSpace(VoenText.Text)) ? "" : "Vöen : " + VoenText.Text.Trim(),
                       voen = (String.IsNullOrWhiteSpace(Voen)) ? "" : "Vöen : " + Voen;
                try
                {
                    s_bankaccount = $@"SELECT B.LONG_NAME || ', HESAB:' || B.ACCOUNT BANK_ACCOUNT
                                          FROM CRS_USER.BANKS B
                                         WHERE B.IS_USED = 1";

                    foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s_bankaccount).Rows)
                    {
                        if (i > 2)
                            continue;

                        if (i == 1)
                            bank_account1 = dr[0].ToString();
                        if (i == 2)
                            bank_account2 = dr[0].ToString();
                        i++;
                    }
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Bank hesabları tapılmadı.", s_bankaccount, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }

                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                try
                {
                    copy_file_name = ContractCode.Replace(@"/", null) + "_Öhdəlik ötürmə_F" + (file_count + 1).ToString().PadLeft(3, '0');
                    object readOnly = false;
                    object isVisible = false;
                    wordApp.Visible = false;

                    aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    saveAs = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + copy_file_name + ".docx");
                    aDoc.Activate();
                    GlobalProcedures.FindAndReplace(wordApp, "[$currentdate]", date);
                    GlobalProcedures.FindAndReplace(wordApp, "[$code]", ContractCode);
                    GlobalProcedures.FindAndReplace(wordApp, "[$persontype]", "fiziki şəxs");
                    GlobalProcedures.FindAndReplace(wordApp, "[$commitment]", NameText.Text.Trim() + " (" + SeriesComboBox.Text + ", №: " + NumberText.Text + ", " + IssueDate.Text + " tarixdə " + IssuingComboBox.Text + " tərəfindən verilib)");
                    GlobalProcedures.FindAndReplace(wordApp, "[$amount]", LiquidValue.ToString("N2") + " " + liquid_currency_name);
                    GlobalProcedures.FindAndReplace(wordApp, "[$objecttype]", CreditName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$objectdescription]", object_desc);
                    GlobalProcedures.ReplaceTextInWordDoc(wordApp, "[$object]", Lizinq);
                    GlobalProcedures.FindAndReplace(wordApp, "[$objectcount]", Count.ToString());
                    GlobalProcedures.FindAndReplace(wordApp, "[$amountwithstring]", LiquidValue.ToString("N2") + amount_with_word);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$seller]", NameText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$advancepayment]", AdvancePaymentValue.Value.ToString("N2") + " " + currency_name);
                    GlobalProcedures.FindAndReplace(wordApp, "[$period]", PeriodDate.Text);
                    GlobalProcedures.FindAndReplace(wordApp, "[$prizeinterest]", "maliyyələşdirilən məbləğin illik " + PrizeInterestValue.Value.ToString() + " faiz");
                    GlobalProcedures.FindAndReplace(wordApp, "[$serviceamount]", ServiceAmountValue.Value.ToString("N2") + " " + currency_name);
                    GlobalProcedures.FindAndReplace(wordApp, "[$debt]", TotalDebtValue.Value.ToString("N2") + amount_with_word3);
                    GlobalProcedures.FindAndReplace(wordApp, "[$bankaccount1]", bank_account1);
                    GlobalProcedures.FindAndReplace(wordApp, "[$bankaccount2]", bank_account2);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer1]", CustomerName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerName);
                    if (ParentPersonTypeID == 1)
                    {
                        GlobalProcedures.FindAndReplace(wordApp, "[$sv]", CustomerCard);
                        GlobalProcedures.FindAndReplace(wordApp, "[$voen]", voen);
                    }
                    else
                    {
                        GlobalProcedures.FindAndReplace(wordApp, "[$sv]", voen);
                        GlobalProcedures.FindAndReplace(wordApp, "[$voen]", null);
                    }
                    GlobalProcedures.FindAndReplace(wordApp, "[$address]", "Ünvan:" + CustomerAddress);
                    GlobalProcedures.FindAndReplace(wordApp, "[$phone]", "Telefon:" + CustomerPhone);
                    GlobalProcedures.FindAndReplace(wordApp, "[$account]", Account);
                    GlobalProcedures.FindAndReplace(wordApp, "[$commitment1]", NameText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$sv1]", SeriesComboBox.Text + ", №: " + NumberText.Text + ", " + IssueDate.Text + " tarixdə " + IssuingComboBox.Text + " tərəfindən verilib");
                    GlobalProcedures.FindAndReplace(wordApp, "[$address1]", "Ünvan:" + AddressText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$phone1]", "Telefon:" + phone);
                    GlobalProcedures.FindAndReplace(wordApp, "[$voen1]", voen1);
                    GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", ContractStartDate);
                    GlobalProcedures.FindAndReplace(wordApp, "[$carnumber]", CarNumber);
                    GlobalProcedures.FindAndReplace(wordApp, "[$letterhostage]", LetterHostage);
                    GlobalProcedures.FindAndReplace(wordApp, "[$letterdate]", letterDate);
                    GlobalProcedures.FindAndReplace(wordApp, "[$lettercommitmentname2]", letterCommitmentName + ((!String.IsNullOrWhiteSpace(letterCommitmentName) && letterCommitmentName.IndexOf("oğlu") > 0) ? "nun" : "nın"));
                    GlobalProcedures.FindAndReplace(wordApp, "[$lettercode]", letterNumber);
                    GlobalProcedures.FindAndReplace(wordApp, "[$referencecode]", referenceNumber);
                    GlobalProcedures.FindAndReplace(wordApp, "[$lettercustomername]", letterCustomerName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$lettercommitmentname]", letterCommitmentName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customerdrivinglicense]", customerDrivingLicense);
                    GlobalProcedures.FindAndReplace(wordApp, "[$sellerdrivinglicense]", commitmentDrivingLicense);
                    file_count++;
                    aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                    if (File.Exists((string)saveAs))
                        Process.Start((string)saveAs);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.SplashScreenClose();
                    GlobalProcedures.LogWrite(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + copy_file_name + ".docx faylı açılmadı", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    aDoc.Close(ref missing, ref missing, ref missing);
                    GlobalProcedures.SplashScreenClose();
                }
            }
        }

        private void CalculatedPaymentScheduleTemp(OracleTransaction tran)
        {
            if (!CheckPaymentSchedule.Checked)
                return;

            int grace_period = 0, interest = (int)PrizeInterestValue.Value, is_change_date = 0;
            month_count = GlobalFunctions.DifferenceTwoDateWithMonth(AgreementDate.DateTime, PeriodDate.DateTime);

            DateTime d_start, real_date;
            DayOfWeek day_of_week;
            double interest_amount = 0, basic_amount = 0, debt = (double)TotalDebtValue.Value;

            grace_period = 0;

            GlobalProcedures.ExecuteQuery(tran, $@"UPDATE CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP SET IS_CHANGE = 2 
                                                            WHERE CONTRACT_ID = {ContractID} 
                                                                AND COMMITMENT_ID = {CommitmentID}
                                                                AND USED_USER_ID = {GlobalVariables.V_UserID}");

            monthly_amount = GlobalFunctions.CalcPayment((double)TotalDebtValue.Value, month_count - grace_period, interest);

            for (int i = grace_period + 1; i <= month_count; i++)
            {
                d_start = AgreementDate.DateTime;
                d_start = d_start.AddMonths(i);


                if (d_start > PeriodDate.DateTime)
                    d_start = PeriodDate.DateTime;

                real_date = d_start;

                day_of_week = d_start.DayOfWeek;//heftenin gunu
                is_change_date = 0;
                if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 6)
                {
                    d_start = d_start.AddDays(-1);
                    is_change_date = 1;
                }
                else if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 7)
                {
                    d_start = d_start.AddDays(-2);
                    is_change_date = 1;
                }
                interest_amount = ((debt * interest) / 100) / 360 * 30;
                if (i < month_count)
                    basic_amount = monthly_amount - interest_amount;
                else
                {
                    basic_amount = debt;
                    monthly_amount = debt + interest_amount;
                }
                debt = debt - basic_amount;
                GlobalProcedures.ExecuteQuery(tran, $@"INSERT INTO CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP(CONTRACT_ID,
                                                                                                            COMMITMENT_ID,
                                                                                                            MONTH_DATE,
                                                                                                            REAL_DATE,
                                                                                                            MONTHLY_PAYMENT,
                                                                                                            BASIC_AMOUNT,
                                                                                                            INTEREST_AMOUNT,
                                                                                                            DEBT,
                                                                                                            CURRENCY_ID,
                                                                                                            USED_USER_ID,
                                                                                                            IS_CHANGE,
                                                                                                            IS_CHANGE_DATE,
                                                                                                            ORDER_ID,
                                                                                                            VERSION)
                                                          VALUES({ContractID},
                                                                        {CommitmentID},
                                                                        TO_DATE('{d_start.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                        TO_DATE('{real_date.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                        {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        {basic_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        {interest_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        {debt.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        {DebtCurrencyID},
                                                                        {GlobalVariables.V_UserID},
                                                                        1,
                                                                        {is_change_date},
                                                                        {i},
                                                                        {scheduleVersion})");
                is_change_date = 0;
            }
        }
    }
}