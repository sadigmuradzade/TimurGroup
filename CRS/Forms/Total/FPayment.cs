using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WordToPDF;
using Excel = Microsoft.Office.Interop.Excel;

namespace CRS.Forms.Total
{
    public partial class FPayment : DevExpress.XtraEditors.XtraForm
    {
        public FPayment()
        {
            InitializeComponent();
            MaximumSize = MinimumSize = Size;
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string ContractID, CustomerID, CustomerName, ContractCode, CustomerCode, SDate, EDate, Lizinq, Currency, CommitmentName, DebtDate;
        public double Amount;
        public int Interest, Period, CommitmentID, CommitmentPersonTypeID, CustomerTypeID, IsSpecialAttention, CreditNameID, CurrencyID;
        public bool IsExtend;

        double debt,
            monthly_amount,
            payment_amount,
            total,
            basic_amount,
            interest_amount,
            payment_interest_amount,
            payment_interest_debt = 0,
            calc_debt = 0,
            interest_debt = 0,
            one_day_interest,
            debt_penalty = 0,
            startPercent = 0;

        decimal a, prev_payment_interest_debt = 0;
        int PaymentUsedUserID = -1, diff_day = 0, topindex, old_row_num, insuranceID, extendInterest = 0, penaltyID = 0;
        int? parentContractID = null;
        bool CurrentStatus = false, PaymentUsed = false, PaymentClosed = false, isActive = false;
        string ldate, paymentid, bank_cash, paymentdate, rate, ContractImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\ContractImages";
        List<Contract> lstContract = null;
        DataTable dtPayment = new DataTable();

        public delegate void DoEvent();
        public event DoEvent RefreshTotalsDataGridView;

        private void FPayment_Load(object sender, EventArgs e)
        {
            ContractCodeText.Text = ContractCode;
            IDText.EditValue = ContractID;
            PeriodText.Text = Period + " ay";
            StartDateText.Text = SDate;
            EndDateText.Text = EDate;
            ObjectText.Text = Lizinq;
            CustomerCodeText.Text = CustomerCode;
            CustomerNameText.Text = CustomerName;
            CommitmentNameValue.Text = CommitmentName;
            CommitmentNameLabel.Visible = CommitmentNameValue.Visible = !(CommitmentID == 0);
            LoadCustomerImage();

            GenerateViewCaption();

            InterestCurrencyLabel.Text =
                ResidualPercentCurrencyLabel.Text =
                TotalDebtCurrencyLabel.Text =
                PenaltyCurrencyLabel.Text = Currency;
            InsertPaymentsTemp();
            InsertOperationJournalTemp();

            ExtendLabel.Visible = ExtendStartDateText.Visible = ExtendEndDateText.Visible = ExtendSimvolLabel.Visible = ExtendPercentLabel.Visible = ExtendInterestText.Visible = IsExtend;
            if (IsExtend)
            {
                List<ContractExtend> lstContractExtend = ContractExtendDAL.SelectContractExtend(0, int.Parse(ContractID)).ToList<ContractExtend>();
                if (lstContractExtend.Count > 0)
                {
                    var extend = lstContractExtend.LastOrDefault();
                    ExtendStartDateText.Text = extend.START_DATE.ToString("dd.MM.yyyy");
                    ExtendEndDateText.Text = extend.END_DATE.ToString("dd.MM.yyyy");
                    extendInterest = extend.INTEREST;
                    ExtendInterestText.Text = extendInterest.ToString() + " %";
                    monthly_amount = (double)extend.MONTHLY_AMOUNT;
                }
            }

            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");

            lstContract = ContractDAL.SelectContract(int.Parse(ContractID)).ToList<Contract>();
            var contract = lstContract.First();
            PaymentUsedUserID = contract.USED_USER_ID;
            monthly_amount = (!IsExtend) ? (double)contract.MONTHLY_AMOUNT : monthly_amount;
            PaymentUsed = (PaymentUsedUserID >= 0);
            PaymentClosed = (contract.STATUS_ID == 6);
            InterestText.Text = (contract.CHECK_INTEREST > -1 ? contract.CHECK_INTEREST : contract.INTEREST) + " %";
            parentContractID = contract.PARENT_ID;

            if ((PaymentClosed && PaymentUsed) || (PaymentClosed && !PaymentUsed))
            {
                GlobalProcedures.ShowWarningMessage(ContractCode + " saylı lizinq müqaviləsi bağlanılıb. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu");
                CurrentStatus = true;
            }
            else if (PaymentUsed && !PaymentClosed)
            {
                if (GlobalVariables.V_UserID != PaymentUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == PaymentUsedUserID).FULLNAME;
                    GlobalProcedures.ShowWarningMessage(CustomerName + "na məxsus olan ödənişlər hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş ödənişlərin hal-hazırkı statusu");
                    CurrentStatus = true;
                }
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;
            if (!CurrentStatus)
                CurrentStatus = !(String.IsNullOrWhiteSpace(DebtDate));

            ComponentEnable(CurrentStatus);
            LoadPaymentsDataGridView(Currency);
            CalculatedPenalties();
            CalculatedPercents();
            InsuranceCostCurrencyLabel.Visible = InsuranceLabel.Visible = InsuranceDebtValue.Visible = BInsurance.Visible = CreditNameID == 1;
            InsuranceDebt();
            DescriptionPicture.Visible = contract.CONTRACT_IMAGE_COUNT > 0;
        }

        private void InsuranceDebt()
        {
            if (CreditNameID != 1)
                return;

            string sql = $@"SELECT I.AMOUNT + I.ADDITIONAL_AMOUNT
                                   - NVL (IP.PAYED_AMOUNT, 0)
                                      DEBT,
                                  I.ID
                              FROM CRS_USER_TEMP.V_SUM_INSURANCE_PAYMENT_TEMP IP, CRS_USER.V_LAST_INSURANCES I
                             WHERE I.ID = IP.INSURANCE_ID(+) AND I.CONTRACT_ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/InsuranceDebt", "Sığortanın borcu açılmadı.");
            if (dt.Rows.Count > 0)
            {
                InsuranceDebtValue.EditValue = Convert.ToDecimal(dt.Rows[0]["DEBT"]);
                insuranceID = Convert.ToInt32(dt.Rows[0]["ID"]);
            }
        }

        private void InsuranceWarningMessage()
        {
            if (InsuranceDebtValue.Visible && InsuranceDebtValue.Value > 0)
            {
                Contracts.FSpecialWarning fs = new Contracts.FSpecialWarning();
                fs.Description = "<color=red>DİQQƏT!!!</color>  Bu müqavilə üzrə " + InsuranceDebtValue.Value.ToString("n2") + " AZN sığorta borcu var.";
                fs.ShowDialog();
            }
        }

        private void GenerateViewCaption()
        {
            double percent = GlobalFunctions.GetAmount($@"SELECT START_PERCENT FROM CRS_USER.CONTRACT_START_PERCENT WHERE CONTRACT_ID = {ContractID}", this.Name + "/CalculatedPercents");

            if (Currency == "AZN")
                PaymentsGridView.ViewCaption = "Verilən : " + Amount.ToString("N2") + " " + Currency + ((percent > 0) ? " [ Başlanğıc qalıq faiz = " + percent.ToString("N2") + " ]" : null);
            else
            {
                rate = GlobalFunctions.GetName($@"SELECT CUR.VALUE||' '||CUR.CODE||' = '||C.CURRENCY_RATE||' AZN' FROM CRS_USER.CONTRACTS C,CRS_USER.CURRENCY CUR WHERE C.CURRENCY_ID = CUR.ID AND C.ID = {ContractID}");
                PaymentsGridView.ViewCaption = "Verilən : " + Amount.ToString("N2") + " " + Currency + "   (" + rate + ")" + ((percent > 0) ? " [ Başlanğıc qalıq faiz = " + percent.ToString("N2") + " ]" : null);
            }
        }

        private void FindLastDateAndDebt()
        {
            ldate = null;
            try
            {
                if (PaymentsGridView.RowCount == 0)
                {
                    ldate = StartDateText.Text;
                    debt = Amount;
                    payment_interest_debt = 0;
                }
                else
                {
                    List<Payments> lstPayments = PaymentsDAL.SelectPayments(1, int.Parse(ContractID)).ToList<Payments>();
                    if (lstPayments.Count == 0)
                        return;

                    var lastPayments = lstPayments.Where(item => item.IS_CHANGE == 0 || item.IS_CHANGE == 1).LastOrDefault();

                    ldate = lastPayments.PAYMENT_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
                    debt = Math.Round((double)lastPayments.DEBT, 2);
                    payment_interest_debt = Math.Round((double)lastPayments.PAYMENT_INTEREST_DEBT, 2);
                    prev_payment_interest_debt = lastPayments.PREV_PAYMENT_INTEREST_DEBT;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Sonuncu tarix və qaliq tapılmadı", ldate, GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void CalculatedPercents()
        {
            try
            {
                FindLastDateAndDebt();
                startPercent = GlobalFunctions.ContractStartPercent(int.Parse(ContractID));
                DateTime todate = DateTime.Today,
                         fromDate = GlobalFunctions.ChangeStringToDate(ldate, "ddmmyyyy");
                DateTime? prevPercentEndDate = null;
                diff_day = 0;
                List<ContractPercentDate> lstPercentDate = ContractPercentDateDAL.SelectContractPercentDate(int.Parse(ContractID)).ToList<ContractPercentDate>();
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

                    if (prevPercentEndDate.HasValue && fromDate < prevPercentEndDate.Value)
                    {
                        fromDate = prevPercentEndDate.Value.AddDays(1);
                        todate = DateTime.Today;
                    }

                    if (prevPercentEndDate.HasValue)
                        diff_day = diff_day + GlobalFunctions.Days360(fromDate, todate);
                    else if (lstPercentDate.Count == 0)
                        diff_day = GlobalFunctions.Days360(fromDate, todate);
                }
                else
                    diff_day = GlobalFunctions.Days360(fromDate, todate);

                one_day_interest = Math.Round(debt * Interest / 100 / 360, 2);
                InterestValue.EditValue = (diff_day * one_day_interest) + startPercent;
                ResidualPercentValue.EditValue = (double)InterestValue.Value + payment_interest_debt;
                CalculatedTotalDebt();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bu günə olan faiz hesablanmadı.", diff_day.ToString(), GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void CalculatedTotalDebt()
        {
            TotalDebtValue.Value = Math.Round(PenaltyValue.Value + (decimal)debt + ResidualPercentValue.Value, 2);
        }

        private void CalculatedPenalties()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT ID,PENALTY_DEBT
                                                                  FROM CRS_USER_TEMP.CONTRACT_PENALTY_TEMP CP
                                                                 WHERE     ID = (SELECT MAX(ID)
                                                                                   FROM CRS_USER_TEMP.CONTRACT_PENALTY_TEMP
                                                                                  WHERE IS_CHANGE != 2 
                                                                                    AND CONTRACT_ID = CP.CONTRACT_ID)
                                                                       AND CONTRACT_ID = {ContractID}", this.Name + "/CalculatedPenalties", "Hesablanmış cərimə açılmadı.");
            if (dt.Rows.Count > 0)
            {
                debt_penalty = Convert.ToDouble(dt.Rows[0]["PENALTY_DEBT"]);
                penaltyID = Convert.ToInt32(dt.Rows[0]["ID"]);
            }
            else
            {
                debt_penalty = 0;
                penaltyID = 0;
            }
            PenaltyValue.Value = (decimal)debt_penalty;
        }

        public void ComponentEnable(bool status)
        {
            if (GlobalVariables.V_UserID > 0 && !status)
            {
                NewBarSub.Enabled = GlobalVariables.AddPaymentToCash || GlobalVariables.AddPaymentToBank || GlobalVariables.AddPaymentToOther;
                NewCashBarButton.Enabled = GlobalVariables.AddPaymentToCash;
                NewBankBarButton.Enabled = GlobalVariables.AddPaymentToBank;
                OtherBarButton.Enabled = GlobalVariables.AddPaymentToOther;
                StopPercentBarButton.Enabled = GlobalVariables.StopPercent;
            }
            else if (status)
                NewBarSub.Enabled =
                    ChangeBarSubItem.Enabled =
                    AttentionBarButton.Enabled =
                    OtherBarButton.Enabled =
                    StopPercentBarButton.Enabled = false;

            BOK.Visible = !status;
        }

        private void InsertOperationJournalTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_JOURNAL_TEMP", "P_CONTRACT_ID", ContractID, "Əməliyyatlar jurnalın temp cədvəlinə daxil edilmədi.");
        }

        private void InsertPaymentsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_PAYMENTS_TEMP", "P_CONTRACT_ID", ContractID, "Ödənişlər temp cədvələ daxil edilmədi.");
        }

        private void InsertPayments(int isClosed)
        {
            GlobalProcedures.ExecuteProcedureWithFourParametrAndUser("CRS_USER.PROC_INSERT_CUSTOMER_PAYMENT", "P_CONTRACT_ID", int.Parse(ContractID), "P_INSURANCE_ID", insuranceID, "P_TYPE", 1, "P_IS_CLOSED", isClosed, "Ödənişlər əsas cədvələ daxil edilmədi.");            
        }

        private void UpdateContractStatus()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACTS SET STATUS_ID = 6 WHERE ID = {ContractID}",
                                                    "Müqavilənin statusu dəyişdirilmədi.",
                                                    this.Name + "/UpdateContractStatus");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadCustomerImage()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.IMAGE FROM CRS_USER.CUSTOMER_IMAGE T WHERE T.CUSTOMER_ID = {CustomerID}", this.Name + "/LoadCustomerImage");

            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["IMAGE"]))
                {
                    Byte[] BLOBData = (byte[])dr["IMAGE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    CustomerPictureBox.Image = Image.FromStream(stmBLOBData);
                    stmBLOBData.Close();
                }
                else
                {
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "RU":
                            CustomerPictureBox.Properties.NullText = "Фотография клиентов";
                            break;
                        case "EN":
                            CustomerPictureBox.Properties.NullText = "Customer picture";
                            break;
                    }
                }
            }
        }

        private void PaymentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentsGridView, PopupMenu, e);
        }

        private void FPayment_Shown(object sender, EventArgs e)
        {
            if (CurrentStatus)
                return;

            InsuranceWarningMessage();

            if (IsSpecialAttention == 1)
            {
                Contracts.FSpecialWarning fs = new Contracts.FSpecialWarning();
                fs.Description = "<color=red>DİQQƏT!!!</color>  Bu müqaviləyə xüsusi nəzarət var";
                fs.ShowDialog();
            }

            string sql = $@"SELECT U.USER_FULLNAME,PN.NOTE,TO_CHAR(PN.INSERT_DATE, 'DD.MM.YYYY HH24:MI:SS') INSERT_DATE
                                      FROM CRS_USER.PAYMENT_NOTES PN, CRS_USER.V_USERS U
                                     WHERE     PN.INSERT_USER_ID = U.ID
                                           AND PN.CONTRACT_ID = {ContractID}
                                           AND PN.ID = (SELECT MAX (ID)
                                                          FROM CRS_USER.PAYMENT_NOTES
                                                         WHERE CONTRACT_ID = PN.CONTRACT_ID)";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/FPayment_Shown");
            if (dt.Rows.Count == 0)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                XtraMessageBox.Show("<b><color=104,0,0>" + dr["NOTE"] + "</color></b>\n\n\n<b>Qeyd yazan istifadəçi : </b>" + dr["USER_FULLNAME"] + "\n<b>Tarix : </b>" + dr["INSERT_DATE"], "Diqqət ediləcək qeyd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PaymentsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EnabledButton();
        }

        private void EnabledButton()
        {
            bool donate = false;

            DataView dv = new DataView();

            dv = new DataView(dtPayment);
            dv.RowFilter = $@"BANK_CASH = 'BB'";

            object objCompute = dtPayment.Compute("COUNT(ID)", dv.RowFilter);

            if (objCompute != DBNull.Value)
                donate = Convert.ToInt16(objCompute) > 0;

            if (PaymentsGridView.RowCount > 0 && !donate)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditPayment && !CurrentStatus;
                    DeleteBarButton.Enabled = GlobalVariables.DeletePayment && !CurrentStatus;
                    CashToBankBarButton.Enabled = GlobalVariables.CashToBank && !CurrentStatus;
                    BankToCashBarButton.Enabled = GlobalVariables.BankToCash && !CurrentStatus;
                    DonateDebtBarButton.Enabled = GlobalVariables.DonateDebt && !CurrentStatus;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = ChangeBarSubItem.Enabled = DonateDebtBarButton.Enabled = true;
            }
            else
            {
                EditBarButton.Enabled = ChangeBarSubItem.Enabled = DonateDebtBarButton.Enabled = false;
                DeleteBarButton.Enabled = PaymentsGridView.RowCount > 0;
            }

            StartPercentBarButton.Enabled = PaymentsGridView.RowCount == 0;
            NewBarSub.Enabled = !donate;
        }

        private void OtherBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindLastDateAndDebt();
            LoadFPaymentAddEdit("INSERT", PaymentsGridView.RowCount, ldate, Amount, debt, Currency, Interest, monthly_amount, CustomerID, ContractID, 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, "D");
        }

        private void PdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void BInsurance_Click(object sender, EventArgs e)
        {
            Contracts.FInsurancePayment fi = new Contracts.FInsurancePayment();
            fi.InsuranceID = insuranceID;
            fi.TypeID = 1;
            fi.TruncateTable = false;
            fi.FromCustomerPayment = 0;
            fi.RefreshDataGridView += new Contracts.FInsurancePayment.DoEvent(InsuranceDebt);
            fi.ShowDialog();
        }

        void RefreshStopPercent()
        {
            LoadPaymentsDataGridView(Currency);
            CalculatedPercents();
        }

        private void StopPercentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FStopPercent fs = new FStopPercent();
            fs.ContractCode = ContractCode;
            fs.ContractID = int.Parse(ContractID);
            fs.LastPaymentDate = GlobalFunctions.ChangeStringToDate(ldate, "ddmmyyyy");
            fs.RefreshDataGridView += new FStopPercent.DoEvent(RefreshStopPercent);
            fs.ShowDialog();
        }

        void RefreshDonate()
        {
            LoadPaymentsDataGridView(Currency);
        }

        private void PaymentsGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            string payType = currentView.GetRowCellValue(e.RowHandle, "BANK_CASH").ToString();
            if (payType == "E" || payType == "CC" || payType == "BB")
            {
                e.Appearance.BackColor = Color.PapayaWhip;
                e.Appearance.BackColor2 = Color.PapayaWhip;
            }
        }

        private void PaymentsGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }

        private void PaymentsGridView_RowCountChanged(object sender, EventArgs e)
        {

        }

        private void DonateDebtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FDonateDebt fdd = new FDonateDebt();
            fdd.ContractID = int.Parse(ContractID);
            fdd.Currency = Currency;
            fdd.Interest = extendInterest != 0 ? extendInterest : Interest;
            fdd.CustomerTypeID = CustomerTypeID;
            fdd.CustomerID = int.Parse(CustomerID);
            fdd.CurrencyID = CurrencyID;
            fdd.CustomerName = String.IsNullOrEmpty(CommitmentName) ? CustomerNameText.Text.Trim() : CommitmentName;
            fdd.RefreshPaymentsDataGridView += new FDonateDebt.DoEvent(RefreshDonate);
            fdd.ShowDialog();
        }

        private void PrintPdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Print();
        }

        private void PrintWordBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Print(false);
        }

        private void FPayment_Activated(object sender, EventArgs e)
        {
            if (isActive)
                return;

            isActive = true;
        }

        private void DescriptionPicture_Click(object sender, EventArgs e)
        {
            //FContractImages fci = new FContractImages();
            //fci.ContractID = int.Parse(ContractID);
            //fci.ContractCode = ContractCodeText.Text.Trim();
            //fci.ShowDialog();

            LoadContractImages();
        }

        private void LoadContractImages()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.IMAGE FROM CRS_USER.CONTRACT_IMAGES T WHERE T.CONTRACT_ID = {ContractID}", this.Name + "/LoadContractImages");

            int iCount = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["IMAGE"]))
                {
                    Byte[] BLOBData = (byte[])dr["IMAGE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);

                    if (!Directory.Exists(ContractImagePath))
                    {
                        Directory.CreateDirectory(ContractImagePath);
                    }
                    string filePath = ContractImagePath + "\\" + iCount + "_" + ContractCode + ".jpeg";
                    GlobalProcedures.DeleteFile(filePath);
                    FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                    iCount++;
                    Process.Start(filePath);
                }
            }
        }

        private void PaymentsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (PaymentsGridView.GetListSourceRowCellValue(rowIndex, "NOTE").ToString().Length > 0)
                e.Value = Properties.Resources.notes_16;
            else
                e.Value = null;
        }

        private void NoteToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            GridHitInfo hitInfo = PaymentsGridView.CalcHitInfo(e.ControlMousePosition);
            DataRow drCurrentRow = PaymentsGridView.GetDataRow(hitInfo.RowHandle);
            if (drCurrentRow == null)
                return;

            if (String.IsNullOrEmpty(drCurrentRow["NOTE"].ToString()))
                return;

            if (hitInfo.InRow == false)
                return;

            if (hitInfo.Column == null)
                return;

            if (hitInfo.Column != Payments_Notes)
                return;

            SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
            toolTipArgs.AllowHtmlText = DefaultBoolean.True;

            toolTipArgs.Title.Text = "<color=255,0,0>Qeyd</color>";
            toolTipArgs.Contents.Text = drCurrentRow["NOTE"].ToString();
            toolTipArgs.Contents.Image = Properties.Resources.notes_32;


            e.Info = new ToolTipControlInfo();
            e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
            e.Info.ToolTipType = ToolTipType.SuperTip;
            e.Info.SuperTip = new SuperToolTip();
            e.Info.SuperTip.Setup(toolTipArgs);
        }

        void RefresfGridCaption(decimal a)
        {
            GenerateViewCaption();
        }

        private void StartPercentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FStartPercentAdd fs = new FStartPercentAdd();
            fs.ContractID = int.Parse(ContractID);
            fs.RefreshPaymentsDataGridView += new FStartPercentAdd.DoEvent(RefresfGridCaption);
            fs.ShowDialog();
        }

        private void AttentionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FAttentions fa = new FAttentions();
            fa.ContractCode = ContractCode;
            fa.CustomerName = (CommitmentName.Length != 0) ? CommitmentName : CustomerName;
            fa.ContractID = ContractID;
            fa.ShowDialog();
        }

        private void BDateCalculator_Click(object sender, EventArgs e)
        {
            FDateCalculator fdc = new FDateCalculator();
            fdc.ShowDialog();
        }

        private void PaymentsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void CommitmentNameValue_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                if (CommitmentPersonTypeID == 1)
                {
                    FCommitmentInfo fci = new FCommitmentInfo();
                    fci.CommitmentID = CommitmentID;
                    fci.ShowDialog();
                }
                else if (CommitmentPersonTypeID == 2)
                {
                    FJuridicalCommitmentInfo fjc = new FJuridicalCommitmentInfo();
                    fjc.CommitmentID = CommitmentID;
                    fjc.ShowDialog();
                }
            }
            else
            {
                Customer.FCommitments fc = new Customer.FCommitments();
                fc.ContractCode = ContractCodeText.Text;
                fc.ContractID = ContractID;
                fc.ShowDialog();
            }
        }

        private void PaymentsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Payments_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("DAY_COUNT", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("TOTAL", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYED_PENALTY", "Far", e);
            //if (Currency != "AZN")
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT_AZN", "Far", e);
            if (e.Column.FieldName == "TOTAL")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void LoadPaymentsDataGridView(string currency)
        {
            string s = null, debtDate = (String.IsNullOrWhiteSpace(DebtDate)) ? "" : $@" AND CP.PAYMENT_DATE <= TO_DATE('{DebtDate}','DD.MM.YYYY')",
                   sortType = (GlobalVariables.V_DefaultDateSort == 1) ? "DESC" : "ASC";

            s = $@"SELECT 1 SS,
                             CP.CUSTOMER_ID,
                             CP.CONTRACT_ID,
                             CP.PAYMENT_DATE,
                             CP.CURRENCY_RATE,
                             CP.PAYMENT_AMOUNT_AZN,
                             CP.PAYMENT_AMOUNT,
                             CP.BASIC_AMOUNT,
                             CP.DEBT,
                             CP.DAY_COUNT,
                             CP.INTEREST_AMOUNT,
                             CP.PAYMENT_INTEREST_AMOUNT,
                             CP.PAYMENT_INTEREST_DEBT,
                             CP.TOTAL,
                             CP.ID,
                             PAYED_PENALTY,
                             (CASE WHEN CP.BANK_CASH = 'C' THEN 'Kassa' 
                                    WHEN CP.BANK_CASH = 'B' THEN 'Bank, ' || B.SHORT_NAME 
                                    WHEN CP.BANK_CASH = 'D' THEN 'Digər' 
                                    WHEN CP.BANK_CASH = 'E' THEN 'Uzatma' 
                                    WHEN CP.BANK_CASH = 'CC' THEN 'Öhdəlik' 
                                    WHEN CP.BANK_CASH = 'BB' THEN 'Bağışlama'
                             ELSE NULL END) PAYMENT_TYPE,                                              
                             BANK_CASH,
                             ROW_NUMBER () OVER (ORDER BY CP.PAYMENT_DATE, CP.ID) ROW_NUM,
                             CP.NOTE,
                             CP.INSURANCE_CHECK,
                             CP.INSURANCE_AMOUNT,
                             CP.CONTRACT_PERCENT
                        FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP, CRS_USER.BANKS B
                       WHERE     CP.IS_CHANGE IN (0, 1)
                             AND CP.BANK_ID = B.ID(+)
                             AND CP.CONTRACT_ID = {ContractID}{debtDate}
                    ORDER BY CP.PAYMENT_DATE {sortType}, CP.ID {sortType}";

            dtPayment = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentsDataGridView", "Ödənişlər cədvələ yüklənmədi.");
            PaymentsGridControl.DataSource = dtPayment;

            Payments_CurrencyRate.Visible = Payments_AmountAZN.Visible = !(currency == "AZN");
            EnabledButton();

            if (GlobalVariables.V_DefaultDateSort == 0)
                GlobalProcedures.SetScrollToBottom(PaymentsGridView);
        }

        private void PaymentsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            List<CustomerPayments> lstCustomerPayments = CustomerPaymentsDAL.SelectCustomerLastPayment(Convert.ToInt32(ContractID)).ToList<CustomerPayments>();
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaligi hesabliyir
                {
                    if (lstCustomerPayments.Count == 0)
                    {
                        basic_amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("BASIC_AMOUNT").SummaryItem.SummaryValue);
                        calc_debt = Amount - basic_amount;
                    }
                    else
                        calc_debt = (double)lstCustomerPayments.LastOrDefault().DEBT;

                    e.TotalValue = calc_debt;
                }

                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) // qaliq faizi hesabliyir
                {
                    if (lstCustomerPayments.Count == 0)
                        interest_debt = 0;
                    else
                        interest_debt = (double)lstCustomerPayments.LastOrDefault().PAYMENT_INTEREST_DEBT;
                    e.TotalValue = interest_debt;
                }

                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("TOTAL") == 0) // cemi hesabliyir
                {
                    e.TotalValue = interest_debt + calc_debt;
                    total = interest_debt + calc_debt;
                }
            }
        }

        void RefreshPayments(decimal a, string p)
        {
            LoadPaymentsDataGridView(Currency);
            CalculatedPenalties();
            InsuranceDebt();
            CalculatedPercents();
            InsuranceWarningMessage();
        }

        private void LoadFPaymentAddEdit(string transaction, int paymentcount, string lastdate, double amount, double debt, string currency, int interest, double monthly_amount, string customer_id, string contract_id, double payment_amount, string customername, string contractcode, string startdate, string paymentid, double debtpenalty, string paymentType)
        {
            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            FPaymentAddEdit fpae = new FPaymentAddEdit();
            fpae.TransactionName = transaction;
            fpae.PaymentCount = paymentcount;
            fpae.LastDate = lastdate;
            fpae.Amount = amount;
            fpae.Debt = debt;
            fpae.Currency = currency;
            fpae.Interest = interest;
            fpae.MonthlyAmount = monthly_amount;
            fpae.CustomerID = customer_id;
            fpae.ContractID = contract_id;
            fpae.PaymentAmount = payment_amount;
            fpae.PaymentInterestDebt = payment_interest_debt;
            fpae.CustomerName = customername;
            fpae.CommitmentName = CommitmentName;
            fpae.ContractCode = contractcode;
            fpae.ContractStartDate = startdate;
            fpae.PaymentID = paymentid;
            fpae.DebtPenalty = debtpenalty;
            fpae.CustomerTypeID = CustomerTypeID;
            fpae.PaymentType = paymentType;
            fpae.RefreshPaymentsDataGridView += new FPaymentAddEdit.DoEvent(RefreshPayments);
            fpae.ShowDialog();
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        private void LoadFPaymentBankAddEdit(string transaction, int paymentcount, string lastdate, double amount, double debt, string currency, int interest, double monthly_amount, int customer_id, int contract_id, double payment_amount, string customername, string contractcode, string startdate, string paymentid, double debtpenalty)
        {
            decimal insuranceDebt = InsuranceDebtValue.Value;

            if (transaction == "EDIT")
                insuranceDebt = (decimal)GlobalFunctions.GetAmount($@"SELECT ROUND (I.INSURANCE_AMOUNT * I.INSURANCE_INTEREST / 100, 2)
                                                                               - NVL(IP.PAYED_AMOUNT, 0)
                                                                                  DEBT
                                                                          FROM (  SELECT INSURANCE_ID, SUM (PAYED_AMOUNT) PAYED_AMOUNT
                                                                                    FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                                                                                   WHERE CUSTOMER_PAYMENT_ID != {paymentid}
                                                                                GROUP BY INSURANCE_ID) IP,
                                                                               CRS_USER.INSURANCES I
                                                                         WHERE I.ID = IP.INSURANCE_ID(+) AND I.CONTRACT_ID = {contract_id}");

            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            FPaymentBankAddEdit fpae = new FPaymentBankAddEdit();
            fpae.TransactionName = transaction;
            fpae.PaymentCount = paymentcount;
            fpae.LastDate = lastdate;
            fpae.Amount = amount;
            fpae.Debt = debt;
            fpae.Currency = currency;
            fpae.Interest = interest;
            fpae.MonthlyAmount = monthly_amount;
            fpae.CustomerID = customer_id;
            fpae.ContractID = contract_id;
            fpae.PaymentAmount = payment_amount;
            fpae.PaymentInterestDebt = payment_interest_debt;
            fpae.PrevPaymentInterestDebt = prev_payment_interest_debt;
            fpae.CustomerName = customername;
            fpae.ContractCode = contractcode;
            fpae.ContractStartDate = startdate;
            fpae.PaymentID = paymentid;
            fpae.DebtPenalty = debtpenalty;
            fpae.CommitmentName = CommitmentName;
            fpae.CustomerTypeID = CustomerTypeID;
            fpae.InsuranceAmount = insuranceDebt;
            fpae.InsuranceID = insuranceID;
            fpae.PenaltyID = penaltyID;
            fpae.RefreshPaymentsDataGridView += new FPaymentBankAddEdit.DoEvent(RefreshPayments);
            fpae.ShowDialog();
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        private void PaymentsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PaymentsGridView.GetFocusedDataRow();
            if (row != null)
            {
                paymentid = row["ID"].ToString();
                paymentdate = row["PAYMENT_DATE"].ToString().Substring(0, 10);
                bank_cash = row["BANK_CASH"].ToString();
                debt = Convert.ToDouble(row["DEBT"]);
                payment_amount = Convert.ToDouble(row["PAYMENT_AMOUNT"]);
                if (bank_cash == "B")
                {
                    BankToCashBarButton.Enabled = (GlobalVariables.BankToCash && !CurrentStatus);
                    CashToBankBarButton.Enabled = false;
                }
                else
                {
                    BankToCashBarButton.Enabled = false;
                    CashToBankBarButton.Enabled = (GlobalVariables.CashToBank && !CurrentStatus);
                }
                EditBarButton.Enabled = bank_cash != "BB";
            }
        }

        private void FPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            TruncatePaymentsTemp();
            GlobalProcedures.DeleteAllFilesInDirectory(ContractImagePath);
            GlobalProcedures.DeleteAllFilesInDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents");
        }

        private void TruncatePaymentsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_PAYMENTS_TEMP_TRUNCATE", "P_CONTRACT_ID", ContractID, "Ödənişə aid olan məlumatlar temp cədvəllərdən silinmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));            
            int isClosed = 0;
            if (Math.Round(TotalDebtValue.Value, 2) <= 0 && PaymentsGridView.RowCount > 0)
            {
                isClosed = 1;
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowWarningMessage(ContractCode + " saylı Lizinq Müqaviləsi üzrə ödənişlər tam başa çatdığı üçün bu müqavilə sistem tərəfindən avtomatik olaraq bağlanıldı.");
            }
            InsertPayments(isClosed);
            Close();
            RefreshTotalsDataGridView();
            GlobalProcedures.SplashScreenClose();
        }

        private void LoadPaymentsToList()
        {
            string sql = $@"SELECT TO_CHAR(CP.PAYMENT_DATE, 'DD.MM.YYYY') PAYMENT_DATE,
                                     CP.CURRENCY_RATE,
                                     CP.PAYMENT_AMOUNT_AZN,
                                     CP.PAYMENT_AMOUNT,
                                     CP.BASIC_AMOUNT,
                                     CP.DEBT,
                                     CP.DAY_COUNT,
                                     CP.INTEREST_AMOUNT,
                                     CP.PAYMENT_INTEREST_AMOUNT,
                                     CP.PAYMENT_INTEREST_DEBT,
                                     CP.TOTAL,
                                     ROW_NUMBER () OVER (ORDER BY CP.PAYMENT_DATE, CP.ID) ROW_NUM
                                FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP
                               WHERE CP.IS_CHANGE IN (0, 1) AND CP.CONTRACT_ID = {ContractID}
                            ORDER BY CP.PAYMENT_DATE, CP.ID";

            PaymentsViewDAL.RemoveAllPayments();

            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql).Rows)
            {
                PaymentsViewDAL.InsertPayment(int.Parse(dr["ROW_NUM"].ToString()),
                                                 dr["PAYMENT_DATE"].ToString(),
                                                 double.Parse(dr["CURRENCY_RATE"].ToString()),
                                                 double.Parse(dr["PAYMENT_AMOUNT_AZN"].ToString()),
                                                 double.Parse(dr["PAYMENT_AMOUNT"].ToString()),
                                                 double.Parse(dr["BASIC_AMOUNT"].ToString()),
                                                 double.Parse(dr["DEBT"].ToString()),
                                                 int.Parse(dr["DAY_COUNT"].ToString()),
                                                 double.Parse(dr["INTEREST_AMOUNT"].ToString()),
                                                 double.Parse(dr["PAYMENT_INTEREST_AMOUNT"].ToString()),
                                                 double.Parse(dr["PAYMENT_INTEREST_DEBT"].ToString()),
                                                 double.Parse(dr["TOTAL"].ToString()));
            }
        }

        void Print(bool isPdf = true)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));

            LoadPaymentsToList();
            if (PaymentsViewDAL.lstPayments.Count == 0)
                return;

            double sum_basic_amount = PaymentsViewDAL.lstPayments.Sum(p => p.BasicAmount),
                    sum_interest_amount = PaymentsViewDAL.lstPayments.Sum(p => p.InterestAmount),
                    sum_payment_interest_amount = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentInterestAmount),
                    sum_payment_amount = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentAmount),
                    sum_payment_amount_azn = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentAmountAZN),
                    debt = PaymentsViewDAL.lstPayments.LastOrDefault().Debt,
                    lastInterestDebt = PaymentsViewDAL.lstPayments.LastOrDefault().PaymentInterestDebt,
                    one_day_interest = 0,
                    interestAmount = 0,
                    sum_insurance_amount = 0;

            DateTime lastPayDate = GlobalFunctions.ChangeStringToDate(PaymentsViewDAL.lstPayments.LastOrDefault().PaymentDate, "ddmmyyyy");
            int dayCount = 0;

            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Ödənişlərin siyahısı.docx");
            if (!File.Exists(fileName.ToString()))
            {
                XtraMessageBox.Show("Ödənişlərin siyahısının şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }
            int code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));
            string filePathWord = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "1_Ödənişlər.docx",
                   filePathPdf = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "2_Ödənişlər.docx",
                   pdfFilePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Ödənişlər.pdf";


            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;
            object saveAs = Path.Combine(filePathWord);
            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;

            aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

            aDoc.Activate();

            try
            {
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$startdate]", StartDateText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$enddate]", EndDateText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$period]", PeriodText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$percent]", InterestText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerNameText.Text);
                GlobalProcedures.ReplaceTextInWordDoc(wordApp, "[$object]", ObjectText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$commitmentlabel]", !(String.IsNullOrEmpty(CommitmentNameValue.Text)) ? "Öhdəlik götürən:" : null);
                GlobalProcedures.FindAndReplace(wordApp, "[$commitment]", !(String.IsNullOrEmpty(CommitmentNameValue.Text)) ? CommitmentNameValue.Text : null);
                GlobalProcedures.FindAndReplace(wordApp, "[$amount]", Amount.ToString("N2") + " " + Currency + ((Currency == "AZN") ? null : "   (" + rate + ")"));

                Microsoft.Office.Interop.Word.Table table1 = aDoc.Tables[2];
                int i = 2;
                foreach (var item in PaymentsViewDAL.lstPayments)
                {
                    table1.Rows.Add(table1.Rows[i]);

                    table1.Cell(i, 1).Range.Text = item.SS.ToString();
                    table1.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table1.Cell(i, 2).Range.Text = item.PaymentDate;
                    table1.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table1.Cell(i, 3).Range.Text = item.CurrencyRate.ToString("#.###0");
                    table1.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table1.Cell(i, 4).Range.Text = item.PaymentAmountAZN.ToString("N2");
                    table1.Cell(i, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 5).Range.Text = item.PaymentAmount.ToString("N2");
                    table1.Cell(i, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 6).Range.Text = item.BasicAmount.ToString("N2");
                    table1.Cell(i, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 7).Range.Text = item.Debt.ToString("N2");
                    table1.Cell(i, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 8).Range.Text = item.DayCount.ToString();
                    table1.Cell(i, 8).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table1.Cell(i, 9).Range.Text = item.InterestAmount.ToString("N2");
                    table1.Cell(i, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 10).Range.Text = item.PaymentInterestAmount.ToString("N2");
                    table1.Cell(i, 10).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 11).Range.Text = item.PaymentInterestDebt.ToString("N2");
                    table1.Cell(i, 11).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 12).Range.Text = item.Total.ToString("N2");
                    table1.Cell(i, 12).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    i++;
                }
                dayCount = GlobalFunctions.Days360(lastPayDate, DateTime.Today);
                one_day_interest = Math.Round((debt * Interest / 100) / 360, 2);
                interestAmount = one_day_interest * dayCount;

                table1.Cell(i, 1).Range.Text = (i - 1).ToString();
                table1.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                table1.Cell(i, 2).Range.Text = DateTime.Today.ToString("dd.MM.yyyy");
                table1.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                table1.Cell(i, 7).Range.Text = debt.ToString("N2");
                table1.Cell(i, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i, 8).Range.Text = dayCount.ToString();
                table1.Cell(i, 8).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                table1.Cell(i, 9).Range.Text = interestAmount.ToString("N2");
                table1.Cell(i, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i, 11).Range.Text = (lastInterestDebt + interestAmount).ToString("N2");
                table1.Cell(i, 11).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i, 12).Range.Text = (debt + lastInterestDebt + interestAmount).ToString("N2");
                table1.Cell(i, 12).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 4).Range.Text = sum_payment_amount_azn.ToString("N2");
                table1.Cell(i + 1, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 5).Range.Text = sum_payment_amount.ToString("N2");
                table1.Cell(i + 1, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 6).Range.Text = sum_basic_amount.ToString("N2");
                table1.Cell(i + 1, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 7).Range.Text = debt.ToString("N2");
                table1.Cell(i + 1, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 9).Range.Text = (interestAmount + sum_interest_amount).ToString("N2");
                table1.Cell(i + 1, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 10).Range.Text = sum_payment_interest_amount.ToString("N2");
                table1.Cell(i + 1, 10).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 11).Range.Text = (lastInterestDebt + interestAmount).ToString("N2");
                table1.Cell(i + 1, 11).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 12).Range.Text = (debt + lastInterestDebt + interestAmount).ToString("N2");
                table1.Cell(i + 1, 12).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                Microsoft.Office.Interop.Word.Table table2 = aDoc.Tables[3];

                string sql = $@"SELECT ID,
                                   TO_CHAR(PAY_DATE,'DD.MM.YYYY') PAY_DATE,
                                   PAYED_AMOUNT,                                   
                                   NOTE,
                                   IS_LEGAL,
                                   CUSTOMER_PAYMENT_ID
                              FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                             WHERE IS_CHANGE != 2 AND INSURANCE_ID = {insuranceID}
                            ORDER BY PAY_DATE, ID";

                DataTable dtInsurance = GlobalFunctions.GenerateDataTable(sql, this.Name + "/PdfBarButton_ItemClick", "Sığortanın ödənişləri açılmadı.");

                if (dtInsurance.Rows.Count == 0)
                {
                    table2.Delete();
                    GlobalProcedures.FindAndReplace(wordApp, "[$insurancelabel]", null);
                }
                else
                {
                    GlobalProcedures.FindAndReplace(wordApp, "[$insurancelabel]", "Sığorta ödənişləri");
                    i = 2;
                    for (int j = 0; j < dtInsurance.Rows.Count; j++)
                    {
                        table2.Rows.Add(table2.Rows[i]);

                        table2.Cell(i, 1).Range.Text = (i - 1).ToString();
                        table2.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        table2.Cell(i, 2).Range.Text = dtInsurance.Rows[j]["PAY_DATE"].ToString();
                        table2.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        sum_insurance_amount = sum_insurance_amount + Convert.ToDouble(dtInsurance.Rows[j]["PAYED_AMOUNT"]);

                        table2.Cell(i, 3).Range.Text = Convert.ToDecimal(dtInsurance.Rows[j]["PAYED_AMOUNT"]).ToString("N2");
                        table2.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                        i++;
                    }

                    table2.Rows[i].Delete();

                    table2.Cell(i, 3).Range.Text = sum_insurance_amount.ToString("N2");
                    table2.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                }



                aDoc.PageSetup.PaperSize = Microsoft.Office.Interop.Word.WdPaperSize.wdPaperA4;
                aDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;

                aDoc.PageSetup.LeftMargin = 20;
                aDoc.PageSetup.RightMargin = 20;

                if (isPdf)
                {
                    GlobalProcedures.DeleteFile(filePathPdf);

                    saveAs = Path.Combine(filePathPdf);
                    aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Close(ref missing, ref missing, ref missing);

                    Word2Pdf objWorPdf = new Word2Pdf();
                    object FromLocation = filePathPdf;
                    object ToLocation = pdfFilePath;
                    objWorPdf.InputLocation = FromLocation;
                    objWorPdf.OutputLocation = ToLocation;
                    objWorPdf.Word2PdfCOnversion();

                    GlobalProcedures.DeleteFile(filePathPdf);

                    GlobalProcedures.SplashScreenClose();
                    Process.Start(pdfFilePath);
                }
                else
                {
                    GlobalProcedures.DeleteFile(filePathWord);

                    aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Close(ref missing, ref missing, ref missing);

                    GlobalProcedures.SplashScreenClose();
                    Process.Start(filePathWord);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(code_number + "_Ödənişlər.pdf faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.", exx);
            }
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //GlobalProcedures.GridExportToFile(PaymentsGridControl, "xls");

            PaymentExportToExcel();
        }

        private void PaymentExportToExcel()
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FWait));
            try
            {
                if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\PaymentList.xlsx"))
                {
                    XtraMessageBox.Show("Ödənişləri köçürmək üçün excel faylın şablonu tapılmadı.");
                    return;
                }

                int startTableRow = 9;
                LoadPaymentsToList();
                if (PaymentsViewDAL.lstPayments.Count == 0)
                    return;

                double sum_basic_amount = PaymentsViewDAL.lstPayments.Sum(p => p.BasicAmount),
                        sum_interest_amount = PaymentsViewDAL.lstPayments.Sum(p => p.InterestAmount),
                        sum_payment_interest_amount = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentInterestAmount),
                        sum_payment_amount = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentAmount),
                        sum_payment_amount_azn = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentAmountAZN),
                        debt = PaymentsViewDAL.lstPayments.LastOrDefault().Debt,
                        lastInterestDebt = PaymentsViewDAL.lstPayments.LastOrDefault().PaymentInterestDebt,
                        one_day_interest = 0,
                        interestAmount = 0,
                        sum_insurance_amount = 0;

                DateTime lastPayDate = GlobalFunctions.ChangeStringToDate(PaymentsViewDAL.lstPayments.LastOrDefault().PaymentDate, "ddmmyyyy");
                int dayCount = 0, rowCount = 0;

                Excel.Application excelApp = new Excel.Application();
                // open the template in Edit mode
                Excel.Workbook xlWorkbook = excelApp.Workbooks.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\PaymentList.xlsx", 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkbook.Sheets["Sheet1"];
                object misValue = System.Reflection.Missing.Value;
                rowCount = PaymentsViewDAL.lstPayments.Count;

                xlWorkSheet.Cells[1, 4] = ContractCodeText.Text;
                xlWorkSheet.Cells[2, 4] = PeriodText.Text;
                xlWorkSheet.Cells[3, 4] = InterestText.Text;
                xlWorkSheet.Cells[4, 4] = StartDateText.Text;
                xlWorkSheet.Cells[5, 4] = EndDateText.Text;
                xlWorkSheet.Cells[6, 4] = ObjectText.Text;
                xlWorkSheet.Cells[1, 7] = CustomerNameText.Text;
                xlWorkSheet.Cells[4, 7] = Amount.ToString("N2");

                int r = startTableRow;
                foreach (var item in PaymentsViewDAL.lstPayments)
                {
                    xlWorkSheet.Cells[r, 1] = item.SS.ToString();
                    xlWorkSheet.Cells[r, 2] = item.PaymentDate;
                    xlWorkSheet.Cells[r, 3] = item.CurrencyRate.ToString("#.###0");
                    xlWorkSheet.Cells[r, 4] = item.PaymentAmountAZN.ToString("N2");
                    xlWorkSheet.Cells[r, 5] = item.PaymentAmount.ToString("N2");
                    xlWorkSheet.Cells[r, 6] = item.BasicAmount.ToString("N2");
                    xlWorkSheet.Cells[r, 7] = item.Debt.ToString("N2");
                    xlWorkSheet.Cells[r, 8] = item.DayCount.ToString();
                    xlWorkSheet.Cells[r, 9] = item.InterestAmount.ToString("N2");
                    xlWorkSheet.Cells[r, 10] = item.PaymentInterestAmount.ToString("N2");
                    xlWorkSheet.Cells[r, 11] = item.PaymentInterestDebt.ToString("N2");
                    xlWorkSheet.Cells[r, 12] = item.Total.ToString("N2");

                    r++;
                }
                dayCount = GlobalFunctions.Days360(lastPayDate, DateTime.Today);
                one_day_interest = Math.Round((debt * Interest / 100) / 360, 2);
                interestAmount = one_day_interest * dayCount;

                xlWorkSheet.Cells[r, 1] = (rowCount + 1).ToString();
                xlWorkSheet.Cells[r, 2] = DateTime.Today.ToString("dd.MM.yyyy");
                xlWorkSheet.Cells[r, 7] = debt.ToString("N2");
                xlWorkSheet.Cells[r, 8] = dayCount.ToString();
                xlWorkSheet.Cells[r, 9] = interestAmount.ToString("N2");
                xlWorkSheet.Cells[r, 11] = (lastInterestDebt + interestAmount).ToString("N2");
                xlWorkSheet.Cells[r, 12] = (debt + lastInterestDebt + interestAmount).ToString("N2");
                r++;
                xlWorkSheet.Cells[r, 2] = "YEKUN";
                xlWorkSheet.Cells[r, 2].Font.Bold = true;
                xlWorkSheet.Cells[r, 4] = sum_payment_amount_azn.ToString("N2");
                xlWorkSheet.Cells[r, 4].Font.Bold = true;
                xlWorkSheet.Cells[r, 5] = sum_payment_amount.ToString("N2");
                xlWorkSheet.Cells[r, 5].Font.Bold = true;
                xlWorkSheet.Cells[r, 6] = sum_basic_amount.ToString("N2");
                xlWorkSheet.Cells[r, 6].Font.Bold = true;
                xlWorkSheet.Cells[r, 7] = debt.ToString("N2");
                xlWorkSheet.Cells[r, 7].Font.Bold = true;
                xlWorkSheet.Cells[r, 7].Interior.Color = Color.LightBlue;
                xlWorkSheet.Cells[r, 9] = (interestAmount + sum_interest_amount).ToString("N2");
                xlWorkSheet.Cells[r, 9].Font.Bold = true;
                xlWorkSheet.Cells[r, 10] = sum_payment_interest_amount.ToString("N2");
                xlWorkSheet.Cells[r, 10].Font.Bold = true;
                xlWorkSheet.Cells[r, 11] = (lastInterestDebt + interestAmount).ToString("N2");
                xlWorkSheet.Cells[r, 11].Font.Bold = true;
                xlWorkSheet.Cells[r, 12] = (debt + lastInterestDebt + interestAmount).ToString("N2");
                xlWorkSheet.Cells[r, 12].Font.Bold = true;
                xlWorkSheet.Cells[r, 12].Interior.Color = Color.LightBlue;

                for (int row = startTableRow; row <= r; row++)
                {
                    for (int col = 1; col <= 12; col++)
                    {
                        xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                    }
                }

                string sql = $@"SELECT ID,
                                   TO_CHAR(PAY_DATE,'DD.MM.YYYY') PAY_DATE,
                                   PAYED_AMOUNT,                                   
                                   NOTE,
                                   IS_LEGAL,
                                   CUSTOMER_PAYMENT_ID
                              FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                             WHERE IS_CHANGE != 2 AND INSURANCE_ID = {insuranceID}
                            ORDER BY PAY_DATE, ID";

                DataTable dtInsurance = GlobalFunctions.GenerateDataTable(sql, this.Name + "/PdfBarButton_ItemClick", "Sığortanın ödənişləri açılmadı.");

                if (dtInsurance.Rows.Count > 0)
                {
                    r = r + 3;
                    xlWorkSheet.Cells[r, 1] = "Sığorta ödənişləri";
                    xlWorkSheet.Range[$"A{r}:C{r}"].Merge(true);
                    xlWorkSheet.Range[$"A{r}:C{r}"].Font.Bold = true;
                    xlWorkSheet.Range[$"A{r}:C{r}"].Interior.Color = Color.LightGray;
                    xlWorkSheet.Range[$"A{r}:C{r}"].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.Range[$"A{r}:C{r}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    int startInsuranceRow = r;
                    int iRow = 1;
                    for (int j = 0; j < dtInsurance.Rows.Count; j++)
                    {
                        r++;
                        xlWorkSheet.Cells[r, 1] = iRow.ToString();
                        xlWorkSheet.Cells[r, 2] = dtInsurance.Rows[j]["PAY_DATE"].ToString();
                        sum_insurance_amount = sum_insurance_amount + Convert.ToDouble(dtInsurance.Rows[j]["PAYED_AMOUNT"]);
                        xlWorkSheet.Cells[r, 3] = Convert.ToDecimal(dtInsurance.Rows[j]["PAYED_AMOUNT"]).ToString("N2");
                        iRow++;
                    }

                    r++;
                    xlWorkSheet.Cells[r, 2] = "CƏMİ";
                    xlWorkSheet.Cells[r, 2].Font.Bold = true;
                    xlWorkSheet.Cells[r, 3] = sum_insurance_amount.ToString("N2");
                    xlWorkSheet.Cells[r, 3].Font.Bold = true;
                    xlWorkSheet.Cells[r, 3].Interior.Color = Color.LightBlue;

                    for (int row = startInsuranceRow; row <= r; row++)
                    {
                        for (int col = 1; col <= 3; col++)
                        {
                            xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                            xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                            xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
                            xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                        }
                    }
                }


                string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCode + " saylı müqavilənin ödənişləri.xlsx";
                object saveFile = Path.Combine(filePath);

                if (File.Exists(filePath))
                {
                    FileInfo info = new FileInfo(filePath);
                    if (!GlobalFunctions.IsFileLocked(info))
                        File.Delete(filePath);
                    else
                        XtraMessageBox.Show(ContractCode + " saylı müqavilənin ödənişləri.xls faylı artıq yaradılıb və açıqdır.");
                }

                xlWorkbook.SaveAs(saveFile, misValue, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkbook.Close(true, misValue, misValue);
                excelApp.Quit();
                GlobalProcedures.SplashScreenClose();
                Process.Start(filePath);

                GlobalProcedures.ReleaseObject(xlWorkSheet);
                GlobalProcedures.ReleaseObject(xlWorkbook);
                GlobalProcedures.ReleaseObject(excelApp);
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(exx.Message);
            }
        }

        void Edit()
        {
            if (GlobalVariables.V_UserID > 0 && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE != 2 AND CONTRACT_ID = {ContractID} AND ID > {paymentid}") > 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş ödənişdən sonra başqa ödənişlər olduğu üçün bu ödənişi dəyişmək olmaz.");
                return;
            }

            if (bank_cash == "BB")
                return;

            if (bank_cash == "B")
                LoadFPaymentBankAddEdit("EDIT", PaymentsGridView.RowCount, paymentdate, Amount, debt, Currency, Interest, monthly_amount, int.Parse(CustomerID), int.Parse(ContractID), 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty);
            else if (bank_cash == "C")
                LoadFPaymentAddEdit("EDIT", PaymentsGridView.RowCount, paymentdate, Amount, debt, Currency, Interest, monthly_amount, CustomerID, ContractID, payment_amount, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, bank_cash);
            else if (bank_cash == "E")
                GlobalProcedures.ShowWarningMessage("Seçilmiş ödəniş uzadılma üçün temp ödəniş olduğundan bu ödənişi dəyişmək olmaz.");
            else if (bank_cash == "CC")
                GlobalProcedures.ShowWarningMessage("Seçilmiş ödəniş öhdəlik üçün temp ödəniş olduğundan bu ödənişi dəyişmək olmaz.");
            else if (bank_cash == "BB")
                GlobalProcedures.ShowWarningMessage("Seçilmiş ödəniş bağışlanıldığı üçün bu ödənişi dəyişmək olmaz.");
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Edit();
        }

        private void PaymentsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                Edit();
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int insuranceCheck = Convert.ToInt16(GlobalFunctions.GridGetRowCellValue(PaymentsGridView, "INSURANCE_CHECK"));
            decimal insuranceAmount = Convert.ToDecimal(GlobalFunctions.GridGetRowCellValue(PaymentsGridView, "INSURANCE_AMOUNT"));

            string message = null, p = null;
            if (bank_cash == "C")
            {
                if (insuranceCheck == 0)
                    message = paymentdate + " tarixinə olan ödənişi silmək istəyirsiniz? Əgər bu ödənişi silib yadda saxlasaz, bu zaman portfel və kassa qalığı yenidən hesablanacaq.";
                else
                    message = paymentdate + $@" tarixinə olan ödənişin içində <b><color=104,0,0>{insuranceAmount.ToString("N2")} AZN sığorta ödənişi</color></b> mövcuddur.Bu ödənişi silsəz sığorta ödənişidə silinəcək. Buna razısınız? Əgər bu ödənişi silib yadda saxlasaz, bu zaman portfel və kassa qalığı yenidən hesablanacaq.";
            }
            else
            {
                if (insuranceCheck == 0)
                    message = paymentdate + " tarixinə olan ödənişi silmək istəyirsiniz? Əgər bu ödənişi silib yadda saxlasaz, bu zaman portfel və bank əməliyyatlarının qalığı yenidən hesablanacaq.";
                else
                    message = paymentdate + $@" tarixinə olan ödənişin içində <b><color=104,0,0>{insuranceAmount.ToString("N2")} AZN sığorta ödənişi</color></b> mövcuddur.Bu ödənişi silsəz sığorta ödənişidə silinəcək. Buna razısınız? Əgər bu ödənişi silib yadda saxlasaz, bu zaman portfel və bank əməliyyatlarının qalığı yenidən hesablanacaq.";
            }

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE != 2 AND CONTRACT_ID = {ContractID} AND ID > {paymentid}") > 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş ödənişdən sonra başqa ödənişlər olduğu üçün bu ödənişi silmək olmaz.");
                return;
            }

            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show(message, "Ödənişin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));
                PaymentDelete();
                GlobalProcedures.UpdateCustomerPaymentChange(1, int.Parse(ContractID), Convert.ToInt32(paymentid));
                GlobalProcedures.UpdateBalancePenalty(1, ContractID, paymentdate);
                GlobalProcedures.SplashScreenClose();
            }
            RefreshPayments(a, p);
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        private void PaymentDelete()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_PAYMENTS_TEMP_DELETE", "P_CONTRACT_ID", int.Parse(ContractID), "P_ID", int.Parse(paymentid), "Ödənişə aid olan məlumatlar temp cədvəllərdən silinmədi.");
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string p = null;
            RefreshPayments(a, p);
        }

        private void PaymentScheduleLabel_Click(object sender, EventArgs e)
        {
            FPaymentSchedules fps = new FPaymentSchedules();
            fps.ContractID = ContractID;
            fps.Amount = Amount.ToString("N2") + " " + Currency;
            fps.ContractCode = ContractCodeText.Text;
            fps.ShowDialog();
        }

        private void NewCashBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindLastDateAndDebt();
            LoadFPaymentAddEdit("INSERT", PaymentsGridView.RowCount, ldate, Amount, debt, Currency, Interest, monthly_amount, CustomerID, ContractID, 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, "C");
        }

        private void NewBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindLastDateAndDebt();
            LoadFPaymentBankAddEdit("INSERT", PaymentsGridView.RowCount, ldate, Amount, debt, Currency, Interest, monthly_amount, int.Parse(CustomerID), int.Parse(ContractID), 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty);
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(DateTime.Today.ToString("d", Class.GlobalVariables.V_CultureInfoAZ));
        }

        void RefreshPenaltiesAndPercents()
        {
            CalculatedPenalties();
            CalculatedPercents();
        }

        private void LoadFPenalty()
        {
            FPenalty fp = new FPenalty();
            fp.ContractCode = ContractCodeText.Text;
            fp.ContractID = ContractID;
            fp.CustomerID = CustomerID;
            fp.Currency = Currency;
            fp.ContractStartDate = SDate;
            fp.RefreshPenaltyValue += new FPenalty.DoEvent(RefreshPenaltiesAndPercents);
            fp.ShowDialog();
        }

        private void BPenalty_Click(object sender, EventArgs e)
        {
            LoadFPenalty();
        }

        private void BankToCashBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            FBankToCash fbc = new FBankToCash();
            fbc.ContractCode = ContractCode;
            fbc.CustomerName = CustomerName;
            fbc.PaymentID = paymentid;
            fbc.RefreshPaymentsDataGridView += new FBankToCash.DoEvent(RefreshPayments);
            fbc.ShowDialog();
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        private void CashToBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            FCashToBank fbc = new FCashToBank();
            fbc.ContractCode = ContractCode;
            fbc.CustomerName = CustomerName;
            fbc.PaymentID = paymentid;
            fbc.RefreshPaymentsDataGridView += new FCashToBank.DoEvent(RefreshPayments);
            fbc.ShowDialog();
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        public static decimal OverduePercent(int conractID, string clearingDate)
        {
            decimal overdue = 0;

            using (OracleConnection connection = new OracleConnection())
            {
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }

                    command = connection.CreateCommand();
                    command.CommandText = "CRS_USER_TEMP.PROC_OVERDUE_PERCENT_TEMP";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_CONTRACT_ID", GlobalFunctions.ConvertObjectToOracleDBType(conractID)).Value = conractID;
                    command.Parameters.Add("P_CLEARING_DATE", GlobalFunctions.ConvertObjectToOracleDBType(clearingDate)).Value = clearingDate;
                    command.Parameters.Add("OVERDUE_PERCENT", OracleDbType.Decimal, ParameterDirection.Output);

                    command.ExecuteNonQuery();

                    overdue = Convert.ToDecimal(command.Parameters["OVERDUE_PERCENT"].Value.ToString(), GlobalVariables.V_CultureInfoEN);
                }
                catch (Exception exx)
                {

                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }

            return overdue;
        }
    }
}