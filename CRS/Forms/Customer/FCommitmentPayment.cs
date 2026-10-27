using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class.Tables;
using CRS.Class;
using Oracle.ManagedDataAccess.Client;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Configuration;
using CRS.Class.Views;
using CRS.Class.DataAccess;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WordToPDF;
using Excel = Microsoft.Office.Interop.Excel;

namespace CRS.Forms.Customer
{
    public partial class FCommitmentPayment : DevExpress.XtraEditors.XtraForm
    {
        public FCommitmentPayment()
        {
            InitializeComponent();
            MaximumSize = MinimumSize = Size;
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string Lizinq, Currency, StartDate, EndDate, DebtDate;
        public decimal Amount;
        public int CommitmentID;

        decimal debt,
            payment_amount,
            total,
            basic_amount,
            interest_amount,
            payment_interest_amount,
            calc_debt = 0,
            interest_debt = 0;

        decimal a;
        int ContractID, Interest;
        int? parentContractID = null;
        bool CurrentStatus = false, isActive = false;
        string ldate, paymentid, bank_cash, paymentdate, rate, ContractImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\ContractImages";
        
        List<Contract> lstContract = null;
        DataTable dtPayment = new DataTable();

        private void FPayment_Load(object sender, EventArgs e)
        {
            List<ContractCommitment> lstContractCommitment = CommitmentsDAL.SelectAllCommitmentByID(CommitmentID).ToList<ContractCommitment>();
            ContractID = lstContractCommitment.LastOrDefault().CONTRACT_ID;
            lstContract = ContractDAL.SelectContract(ContractID).ToList<Contract>();
            var contract = lstContract.LastOrDefault();
            var commitment = lstContractCommitment.LastOrDefault();
            ContractCodeText.Text = contract.CODE;

            StartDateText.Text = StartDate;
            DateTime startDate = GlobalFunctions.ChangeStringToDate(StartDate, "ddmmyyyy");
            DateTime endDate = GlobalFunctions.ChangeStringToDate(EndDate, "ddmmyyyy");
            FromDateValue.Properties.MinValue = startDate;
            ToDateValue.DateTime = endDate;
            ToDateValue.Properties.MaxValue = endDate;
            FromDateValue.DateTime = startDate;
            EndDateText.Text = EndDate;
            CommitmentNameValue.Text = commitment.COMMITMENT_NAME;
            ObjectText.EditValue = Lizinq;
            GenerateViewCaption();
            Interest = commitment.INTEREST;
            InterestText.Text = Interest + " %";
            parentContractID = contract.PARENT_ID;       
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

        private void InsertPaymentsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_PAYMENTS_TEMP", "P_CONTRACT_ID", ContractID, "Ödənişlər temp cədvələ daxil edilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void PaymentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentsGridView, PopupMenu, e);
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

        private void PdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

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
            PaymentsGridView.FocusedRowHandle = PaymentsGridView.RowCount - 1;
        }

        private void PaymentsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (PaymentsGridView.GetListSourceRowCellValue(rowIndex, "NOTE").ToString().Length > 0)
                e.Value = Properties.Resources.notes_16;
            else
                e.Value = null;
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            LoadPaymentsDataGridView(Currency);
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            LoadPaymentsDataGridView(Currency);
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

        private void BDateCalculator_Click(object sender, EventArgs e)
        {
            FDateCalculator fdc = new FDateCalculator();
            fdc.ShowDialog();
        }

        private void PaymentsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
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
            string s = null, sortType = (GlobalVariables.V_DefaultDateSort == 1) ? "DESC" : "ASC";

            s = $@"SELECT CP.CUSTOMER_ID,
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
                             ELSE 'Bağışlanılıb' END) PAYMENT_TYPE,                             
                             BANK_CASH,
                             ROW_NUMBER () OVER (ORDER BY CP.PAYMENT_DATE, CP.ID) ROW_NUM,
                             CP.NOTE,
                             CP.INSURANCE_CHECK,
                             CP.INSURANCE_AMOUNT,
                             CP.CONTRACT_PERCENT
                        FROM CRS_USER.CUSTOMER_PAYMENTS CP, CRS_USER.BANKS B
                       WHERE     CP.BANK_ID = B.ID(+)
                             AND CP.PAYMENT_DATE BETWEEN TO_DATE('{FromDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}','DD.MM.YYYY')
                             AND CP.CONTRACT_ID = {ContractID}
                    ORDER BY CP.PAYMENT_DATE {sortType}, CP.ID {sortType}";

            dtPayment = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentsDataGridView", "Ödənişlər cədvələ yüklənmədi.");
            PaymentsGridControl.DataSource = dtPayment;

            Payments_CurrencyRate.Visible = Payments_AmountAZN.Visible = !(currency == "AZN");

            EnabledButton();
        }

        private void PaymentsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaligi hesabliyir
                {
                    basic_amount = Convert.ToDecimal(PaymentsGridView.Columns.ColumnByFieldName("BASIC_AMOUNT").SummaryItem.SummaryValue);
                    calc_debt = Amount - basic_amount;
                    e.TotalValue = calc_debt;
                }

                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) // qaliq faizi hesabliyir
                {
                    interest_amount = Convert.ToDecimal(PaymentsGridView.Columns.ColumnByFieldName("INTEREST_AMOUNT").SummaryItem.SummaryValue);
                    payment_interest_amount = Convert.ToDecimal(PaymentsGridView.Columns.ColumnByFieldName("PAYMENT_INTEREST_AMOUNT").SummaryItem.SummaryValue);
                    interest_debt = interest_amount - payment_interest_amount;
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
        }

        private void PaymentsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PaymentsGridView.GetFocusedDataRow();
            if (row != null)
            {
                paymentid = row["ID"].ToString();
                paymentdate = row["PAYMENT_DATE"].ToString().Substring(0, 10);
                bank_cash = row["BANK_CASH"].ToString();
                debt = Convert.ToDecimal(row["DEBT"].ToString());
                payment_amount = Convert.ToDecimal(row["PAYMENT_AMOUNT"].ToString());
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
                                FROM CRS_USER.CUSTOMER_PAYMENTS CP
                               WHERE CP.CONTRACT_ID = {ContractID}
                                 AND CP.PAYMENT_DATE BETWEEN TO_DATE('{FromDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}','DD.MM.YYYY')
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
                    interestAmount = 0;

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
                GlobalProcedures.FindAndReplace(wordApp, "[$period]", null);
                GlobalProcedures.FindAndReplace(wordApp, "[$percent]", InterestText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CommitmentNameValue.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$object]", ObjectText.Text);
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
                dayCount = GlobalFunctions.Days360(lastPayDate, ToDateValue.DateTime);
                one_day_interest = Math.Round((debt * Interest / 100) / 360, 2);
                interestAmount = one_day_interest * dayCount;

                table1.Cell(i, 1).Range.Text = (i - 1).ToString();
                table1.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                table1.Cell(i, 2).Range.Text = ToDateValue.Text;
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

                GlobalProcedures.FindAndReplace(wordApp, "[$insurancelabel]", null);

                //string sql = $@"SELECT ID,
                //                   TO_CHAR(PAY_DATE,'DD.MM.YYYY') PAY_DATE,
                //                   PAYED_AMOUNT,                                   
                //                   NOTE,
                //                   IS_LEGAL,
                //                   CUSTOMER_PAYMENT_ID
                //              FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                //             WHERE IS_CHANGE != 2 AND INSURANCE_ID = {insuranceID}
                //            ORDER BY PAY_DATE, ID";

                //DataTable dtInsurance = GlobalFunctions.GenerateDataTable(sql, this.Name + "/PdfBarButton_ItemClick", "Sığortanın ödənişləri açılmadı.");

                //if (dtInsurance.Rows.Count == 0)
                //{
                //    table2.Delete();
                //    GlobalProcedures.FindAndReplace(wordApp, "[$insurancelabel]", null);
                //}
                //else
                //{
                //    GlobalProcedures.FindAndReplace(wordApp, "[$insurancelabel]", "Sığorta ödənişləri");
                //    i = 2;
                //    for (int j = 0; j < dtInsurance.Rows.Count; j++)
                //    {
                //        table2.Rows.Add(table2.Rows[i]);

                //        table2.Cell(i, 1).Range.Text = (i - 1).ToString();
                //        table2.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                //        table2.Cell(i, 2).Range.Text = dtInsurance.Rows[j]["PAY_DATE"].ToString();
                //        table2.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                //        sum_insurance_amount = sum_insurance_amount + Convert.ToDouble(dtInsurance.Rows[j]["PAYED_AMOUNT"]);

                //        table2.Cell(i, 3).Range.Text = Convert.ToDecimal(dtInsurance.Rows[j]["PAYED_AMOUNT"]).ToString("N2");
                //        table2.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                //        i++;
                //    }

                //    table2.Rows[i].Delete();

                //    table2.Cell(i, 3).Range.Text = sum_insurance_amount.ToString("N2");
                //    table2.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //}



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
                        interestAmount = 0;

                DateTime lastPayDate = GlobalFunctions.ChangeStringToDate(PaymentsViewDAL.lstPayments.LastOrDefault().PaymentDate, "ddmmyyyy");
                int dayCount = 0, rowCount = 0;

                Excel.Application excelApp = new Excel.Application();
                // open the template in Edit mode
                Excel.Workbook xlWorkbook = excelApp.Workbooks.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\PaymentList.xlsx", 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkbook.Sheets["Sheet1"];
                object misValue = System.Reflection.Missing.Value;
                rowCount = PaymentsViewDAL.lstPayments.Count;

                xlWorkSheet.Cells[1, 4] = ContractCodeText.Text;
                xlWorkSheet.Cells[2, 4] = null;
                xlWorkSheet.Cells[3, 4] = InterestText.Text;
                xlWorkSheet.Cells[4, 4] = StartDateText.Text;
                xlWorkSheet.Cells[5, 4] = EndDateText.Text;
                xlWorkSheet.Cells[6, 4] = ObjectText.Text;
                xlWorkSheet.Cells[1, 7] = CommitmentNameValue.Text;
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
                dayCount = GlobalFunctions.Days360(lastPayDate, ToDateValue.DateTime);
                one_day_interest = Math.Round((debt * Interest / 100) / 360, 2);
                interestAmount = one_day_interest * dayCount;

                xlWorkSheet.Cells[r, 1] = (rowCount + 1).ToString();
                xlWorkSheet.Cells[r, 2] = ToDateValue.Text;
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

                //string sql = $@"SELECT ID,
                //                   TO_CHAR(PAY_DATE,'DD.MM.YYYY') PAY_DATE,
                //                   PAYED_AMOUNT,                                   
                //                   NOTE,
                //                   IS_LEGAL,
                //                   CUSTOMER_PAYMENT_ID
                //              FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                //             WHERE IS_CHANGE != 2 AND INSURANCE_ID = {insuranceID}
                //            ORDER BY PAY_DATE, ID";

                //DataTable dtInsurance = GlobalFunctions.GenerateDataTable(sql, this.Name + "/PdfBarButton_ItemClick", "Sığortanın ödənişləri açılmadı.");

                //if (dtInsurance.Rows.Count > 0)
                //{
                //    r = r + 3;
                //    xlWorkSheet.Cells[r, 1] = "Sığorta ödənişləri";
                //    xlWorkSheet.Range[$"A{r}:C{r}"].Merge(true);
                //    xlWorkSheet.Range[$"A{r}:C{r}"].Font.Bold = true;
                //    xlWorkSheet.Range[$"A{r}:C{r}"].Interior.Color = Color.LightGray;
                //    xlWorkSheet.Range[$"A{r}:C{r}"].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                //    xlWorkSheet.Range[$"A{r}:C{r}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //    int startInsuranceRow = r;
                //    int iRow = 1;
                //    for (int j = 0; j < dtInsurance.Rows.Count; j++)
                //    {
                //        r++;
                //        xlWorkSheet.Cells[r, 1] = iRow.ToString();
                //        xlWorkSheet.Cells[r, 2] = dtInsurance.Rows[j]["PAY_DATE"].ToString();
                //        sum_insurance_amount = sum_insurance_amount + Convert.ToDouble(dtInsurance.Rows[j]["PAYED_AMOUNT"]);
                //        xlWorkSheet.Cells[r, 3] = Convert.ToDecimal(dtInsurance.Rows[j]["PAYED_AMOUNT"]).ToString("N2");
                //        iRow++;
                //    }

                //    r++;
                //    xlWorkSheet.Cells[r, 2] = "CƏMİ";
                //    xlWorkSheet.Cells[r, 2].Font.Bold = true;
                //    xlWorkSheet.Cells[r, 3] = sum_insurance_amount.ToString("N2");
                //    xlWorkSheet.Cells[r, 3].Font.Bold = true;
                //    xlWorkSheet.Cells[r, 3].Interior.Color = Color.LightBlue;

                //    for (int row = startInsuranceRow; row <= r; row++)
                //    {
                //        for (int col = 1; col <= 3; col++)
                //        {
                //            xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                //            xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                //            xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
                //            xlWorkSheet.Cells[row, col].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                //        }
                //    }
                //}


                string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text + " saylı müqavilənin ödənişləri.xlsx";
                object saveFile = Path.Combine(filePath);

                if (File.Exists(filePath))
                {
                    FileInfo info = new FileInfo(filePath);
                    if (!GlobalFunctions.IsFileLocked(info))
                        File.Delete(filePath);
                    else
                        XtraMessageBox.Show(ContractCodeText.Text + " saylı müqavilənin ödənişləri.xls faylı artıq yaradılıb və açıqdır.");
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


        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string p = null;
            RefreshPayments(a, p);
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(DateTime.Today.ToString("d", Class.GlobalVariables.V_CultureInfoAZ));
        }
    }
}