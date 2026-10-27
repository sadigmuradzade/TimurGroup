using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CRS.Forms.Total
{
    public partial class FCalcPercent : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FCalcPercent()
        {
            InitializeComponent();
        }
        string contract_id,
            contract_code,
            s_date,
            e_date,
            lizinq,
            customer_code,
            customer_name,
            commitment_name,
            customer_id,
            currency,
            seller_id,
            car_number,
            hostageName,
            extend_start_date,
            phone;

        decimal amount,
            calc_debt,
            calc_amount,
            calc_payment_amount,
            calc_basic_amount,
            calc_interest_amount,
            calc_current_interest_amount,
            calc_payment_interest_amount,
            calc_payment_interest_debt,
            calc_total,
            rateAmount = 1,
            extendDebt = 0;

        int interest, period, topindex, old_row_num, status_id, commitment_id, customer_type_id, commitmentPersonTypeID = 0, delayedMonth, isSpecialAttention, credit_name_id, currency_id;
        bool isExtend = false;
        DateTime endDate;

        List<CurrencyRates> lstRate = null;
        List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();
        Dictionary<string, Color> portfelFont = new Dictionary<string, Color>();
        List<PortfelFont> lstPortfelFont = new List<PortfelFont>();
        List<CreditTypeFont> lstCreditTypeFont = new List<CreditTypeFont>();

        private void TotalsGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        void RefreshTotals()
        {
            LoadTotalsDataGridView();
        }


        private void FTotal_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                PrintBarButton.Enabled = GlobalVariables.PrintPortfel;
                ExportBarSubItem.Enabled = GlobalVariables.ExportPortfel;                
            }
            
            lstRate = CurrencyRatesDAL.SelectCurrencyRatesLastDate().ToList<CurrencyRates>();
            CurBarStatic.Caption = GlobalVariables.V_LastRate;
            FromDateValue.DateTime = ToDateValue.DateTime = DateTime.Today;
            BSearch_Click(sender, e);
        }

        static bool IsNullOrEmpty(string[] myStringArray)
        {
            return myStringArray == null || myStringArray.Length < 1;
        }

        private void LoadTotalsDataGridView()
        {
            if (FromDateValue.Text.Length == 0 || ToDateValue.Text.Length == 0)
            {
                TotalsGridControl.DataSource = null;
                return;
            }

            string s = $@"SELECT CUS.CODE CUSTOMER_CODE,
                               CUS.FULLNAME FULL_CUSTOMER_NAME,
                               CON.CONTRACT_CODE,
                               NVL2 (CE.INTEREST, CE.INTEREST, NVL(COM.INTEREST, CON.INTEREST)) INTEREST,
                               NVL2 (CE.MONTH_COUNT, CE.MONTH_COUNT, CON.PERIOD) PERIOD,
                               CON.START_DATE,
                               NVL(CE.START_DATE, CON.START_DATE) GIVEN_DATE,
                               NVL(CE.END_DATE, CON.END_DATE) END_DATE,
                               CON.AMOUNT,
                               CON.CURRENCY_CODE,
                               CON.CURRENCY_ORDER,
                               LT.PAYMENT_AMOUNT,
                               LT.BASIC_AMOUNT,
                               LT.DEBT,
                               LT.DAY_COUNT,
                               LT.INTEREST_AMOUNT,
                               CRS_USER.TOTALS.CALC_CURRENT_INTEREST_AMOUNT(TO_DATE('{FromDateValue.Text}','DD.MM.YYYY'), TO_DATE('{ToDateValue.Text}','DD.MM.YYYY'), CON.CONTRACT_ID) CURRENT_INTEREST_AMOUNT,
                               LT.PAYMENT_INTEREST_AMOUNT,
                               LT.PAYMENT_INTEREST_DEBT,
                               LT.TOTAL,
                               H.HOSTAGE,
                               H.LIQUID_AMOUNT,
                               S.FULLNAME SELLER_NAME,
                               COM.COMMITMENT_NAME,
                               (CASE
                                   WHEN COM.COMMITMENT_NAME IS NULL THEN CUS.PHONE
                                   ELSE COM.PHONE
                                END)
                                  PHONES,
                               LT.REQUIRED,
                               LT.DELAYS,
                               LT.FULL_MONTH_COUNT,
                               NVL2 (CE.MONTHLY_AMOUNT, CE.MONTHLY_AMOUNT, CON.MONTHLY_AMOUNT)
                                        MONTHLY_AMOUNT,
                               LT.NORM_DEBT,
                               LT.MAX_PAYMENT_DATE,
                               LT.ROW_NUM,
                               CON.CONTRACT_NOTE,
                               CON.NOTE_CHANGE_USER,
                               CON.NOTE_CHANGE_DATE,
                               CON.USED_USER_ID,
                               LT.CONTRACT_ID,
                               CON.CUSTOMER_ID,
                               CON.CUSTOMER_TYPE_ID,
                               CON.SELLER_ID,
                               CON.SELLER_TYPE_ID,
                               CON.STATUS_ID,
                               CON.CREDIT_TYPE_ID,
                               CON.CREDIT_NAME_ID,
                               NVL (COM.ID, 0) COMMITMENT_ID,
                               CUS.CUSTOMER_NAME CUSTOMER_NAME,
                               CON.PARENT_ID,
                               COM.PERSON_TYPE_ID COMMITMENT_PERSON_TYPE_ID,
                               CON.CONTRACT_EVALUATE_NAME,
                               EXTRACT (DAY FROM NVL2 (CE.START_DATE, CE.START_DATE, CON.START_DATE))
                                        PAYMENT_DAY,
                               CE.MONTHLY_AMOUNT EXTEND_MONTHLY_AMOUNT,
                               (CASE WHEN CON.END_DATE < TRUNC(SYSDATE) THEN 1 ELSE 0 END) IS_OLD,
                               NVL2(CE.MONTH_COUNT, CE.MONTH_COUNT,  0) INT_EXTEND_MONTH_COUNT,
                               INS.END_DATE INSURANCE_END_DATE,
                               (CASE
                                     WHEN INS.INSURANCE_PERIOD = CON.PERIOD AND INS.END_DATE < TRUNC(SYSDATE) THEN 103
                                     WHEN INS.INSURANCE_PERIOD = CON.PERIOD AND (INS.END_DATE - TRUNC(SYSDATE)) <= 31 THEN 1
                                     WHEN INS.INSURANCE_PERIOD = CON.PERIOD AND (INS.END_DATE - TRUNC(SYSDATE)) > 31 THEN 100                  
                                     WHEN INS.IS_CANCEL = 1 THEN 101
                                     WHEN INS.END_DATE < TRUNC(SYSDATE) THEN 102 
                                     WHEN (INS.END_DATE - TRUNC(SYSDATE)) < 31  THEN 1
                                     ELSE 0
                                  END)
                                    INSURANCE_DIFF_MONTH,
                               CON.IS_SPECIAL_ATTENTION,
                               CON.CONTRACT_EVALUATE_ID,
                               H.HOSTAGE_NAME,
                               CE.START_DATE EXTEND_START_DATE,
                               NVL(CE.DEBT, 0) EXTEND_DEBT,
                               LT.DELAYS_DAY_COUNT,
                               CON.CURRENCY_ID,
                               CON.START_DATE CONTRACT_START_DATE,
                               CON.END_DATE CONTRACT_END_DATE,
                               CLP.PAYMENT_DATE LAST_PAYMENT_DATE,
                               CRS_USER.TOTALS.DEBT_WITH_DATE(CON.CONTRACT_ID, '{FromDateValue.Text}') DEBT_WITH_DATE
                          FROM CRS_USER.LEASING_TOTAL LT,
                               CRS_USER.V_CUSTOMERS CUS,
                               CRS_USER.V_HOSTAGE H,
                               CRS_USER.V_SELLERS S,
                               CRS_USER.V_CONTRACT_COMMITMENTS COM,
                               CRS_USER.V_CONTRACTS CON,
                               CRS_USER.V_LAST_CONTRACT_EXTEND CE,
                               CRS_USER.V_LAST_INSURANCES INS,
                               CRS_USER.V_CUSTOMER_LAST_PAYMENT CLP
                         WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                               AND CON.CUSTOMER_ID = CUS.ID
                               AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                               AND CON.CONTRACT_ID = H.CONTRACT_ID(+)
                               AND CON.SELLER_ID = S.ID(+)
                               AND CON.SELLER_TYPE_ID = S.PERSON_TYPE_ID(+)
                               AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                               AND CON.CONTRACT_ID = CE.CONTRACT_ID(+)
                               AND CON.CONTRACT_ID = INS.CONTRACT_ID(+)
                               AND CON.CONTRACT_ID = CLP.CONTRACT_ID(+)
                               AND CON.START_DATE <= TO_DATE('{FromDateValue.Text}','DD.MM.YYYY')
                               AND CON.STATUS_ID IN (5, 6)
                               AND CON.IS_COMMIT = 1                             
                        ORDER BY CON.CONTRACT_CODE DESC";
            
            TotalsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadTotalsDataGridView", "Portfel yüklənmədi.");

            EnabledButton();
        }

        void EnabledButton()
        {
            PaymentBarButton.Enabled = TotalsGridView.RowCount > 0;
        }

        private void TotalsGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CONTRACT_CODE", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("CURRENT_INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell(Totals_Total, "Far", e);

            if (e.Column.FieldName == "DEBT")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }

            if (e.Column.FieldName == "DAY_COUNT")
            {
                e.Handled = true;
                e.Appearance.FontStyleDelta = FontStyle.Regular;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void TotalsGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = TotalsGridView.GetFocusedDataRow();
            if (row != null)
            {
                customer_id = row["CUSTOMER_ID"].ToString();
                contract_id = row["CONTRACT_ID"].ToString();
                seller_id = row["SELLER_ID"].ToString();
                customer_name = row["CUSTOMER_NAME"].ToString();
                commitment_name = row["COMMITMENT_NAME"].ToString();
                customer_code = row["CUSTOMER_CODE"].ToString();
                contract_code = row["CONTRACT_CODE"].ToString();
                period = Convert.ToInt32(row["PERIOD"].ToString());
                currency_id = Convert.ToInt32(row["CURRENCY_ID"].ToString());
                lizinq = row["HOSTAGE"].ToString();
                s_date = row["CONTRACT_START_DATE"].ToString().Substring(0, 10);
                e_date = row["CONTRACT_END_DATE"].ToString().Substring(0, 10);
                if (row["EXTEND_START_DATE"].ToString() != string.Empty)
                    extend_start_date = row["EXTEND_START_DATE"].ToString().Substring(0, 10);
                currency = row["CURRENCY_CODE"].ToString();
                phone = row["PHONES"].ToString();
                amount = Convert.ToDecimal(row["AMOUNT"].ToString());
                extendDebt = Convert.ToDecimal(row["EXTEND_DEBT"].ToString());
                interest = Convert.ToInt32(row["INTEREST"].ToString());
                commitment_id = Convert.ToInt32(row["COMMITMENT_ID"].ToString());
                if (!String.IsNullOrWhiteSpace(row["COMMITMENT_PERSON_TYPE_ID"].ToString()))
                    commitmentPersonTypeID = Convert.ToInt32(row["COMMITMENT_PERSON_TYPE_ID"].ToString());
                else
                    commitmentPersonTypeID = 0;
                customer_type_id = Convert.ToInt32(row["CUSTOMER_TYPE_ID"].ToString());
                status_id = Convert.ToInt32(row["STATUS_ID"].ToString());
                credit_name_id = Convert.ToInt32(row["CREDIT_NAME_ID"]);
                
                car_number = lizinq.Substring(0, 7);
                
                decimal required = Convert.ToDecimal(row["REQUIRED"]);
                decimal monthlyAmount = Convert.ToDecimal(row["MONTHLY_AMOUNT"]);
                delayedMonth = (int)(required / ((monthlyAmount != 0) ? monthlyAmount : 1));
                hostageName = row["HOSTAGE_NAME"].ToString();
                isExtend = !(String.IsNullOrWhiteSpace(row["EXTEND_MONTHLY_AMOUNT"].ToString()));
                isSpecialAttention = Convert.ToInt32(row["IS_SPECIAL_ATTENTION"].ToString());
                endDate = DateTime.Parse(row["END_DATE"].ToString());
            }
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void FilterTotals()
        {
            ColumnView view = TotalsGridView;

            //Status
            if ((ActiveContractCheck.Checked) && (!ClosedContractCheck.Checked))
                view.ActiveFilter.Add(view.Columns["STATUS_ID"],
              new ColumnFilterInfo("[STATUS_ID] = 5", ""));
            else if ((!ActiveContractCheck.Checked) && (ClosedContractCheck.Checked))
                view.ActiveFilter.Add(view.Columns["STATUS_ID"],
              new ColumnFilterInfo("[STATUS_ID] = 6", ""));
            else
                view.ActiveFilter.Remove(view.Columns["STATUS_ID"]);
        }

        private void ActiveContractCheck_CheckedChanged(object sender, EventArgs e)
        {
            FilterTotals();
        }

        private void PaymentBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            Bookkeeping.FShowPayments fsp = new Bookkeeping.FShowPayments();
            fsp.ContractID = contract_id;
            fsp.ShowDialog();
            TotalsGridView.TopRowIndex = topindex;
            TotalsGridView.FocusedRowHandle = old_row_num;
        }

        private void DateCalculatorBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FDateCalculator fdc = new FDateCalculator();
            fdc.ShowDialog();
        }

        private void BSearch_Click(object sender, EventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));
            LoadTotalsDataGridView();
            GlobalProcedures.SplashScreenClose();
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            CurBarStatic.Caption = CurrencyRatesDAL.LastRateString();
            LoadTotalsDataGridView();
        }

        private void TemporaryPaymentsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FTemporaryPayments ft = new FTemporaryPayments();
            ft.ShowDialog();
        }

        private void AttentionBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FAttentions fa = new FAttentions();
            fa.ContractCode = contract_code;
            fa.CustomerName = (commitment_name.Length != 0) ? commitment_name : customer_name;
            fa.ContractID = contract_id;
            fa.ShowDialog();
        }        

        void RefreshContracts(string contract_id)
        {
            LoadTotalsDataGridView();
        }

        private void GroupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FContractGroups fcg = new FContractGroups();
            fcg.ShowDialog();
        }

        private void LoadFContractAddEdit(string transaction, string contractid, string customerid, string sellerid)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            Contracts.FContractAddEdit fcae = new Contracts.FContractAddEdit();
            fcae.TransactionName = transaction;
            fcae.ContractID = contractid;
            fcae.CustomerID = customerid;
            fcae.SellerID = sellerid;
            fcae.Commit = 1;
            fcae.IsExtend = isExtend;
            fcae.RefreshContractsDataGridView += new Contracts.FContractAddEdit.DoEvent(RefreshContracts);
            fcae.ShowDialog();
            TotalsGridView.FocusedRowHandle = old_row_num;
            TotalsGridView.TopRowIndex = topindex;
        }

        private void NewContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFContractAddEdit("INSERT", null, null, null);
        }

        private void LoadFContractStatusEdit()
        {
            Contracts.FContractStatusEdit fs = new Contracts.FContractStatusEdit();
            fs.ContractID = contract_id;
            fs.ContractCode = contract_code;
            fs.CustomerName = customer_name;
            fs.CommitmentName = commitment_name;
            fs.CommitmentID = commitment_id;
            fs.CommitmentPersonTypeID = commitmentPersonTypeID;
            fs.RefreshTotalsDataGridView += new Contracts.FContractStatusEdit.DoEvent(RefreshTotals);
            fs.ShowDialog();
        }

        private void LettersBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FContractWarningLetters fl = new FContractWarningLetters();
            fl.ShowDialog();
        }        

        private void EditCustomerBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (customer_type_id == 1)
                LoadFCustomerAddEdit("EDIT", customer_id, customer_name);
            else
                LoadFJuridicalPersonAddEdit("EDIT", customer_id);
        }

        void RefreshCustomers(string code)
        {
            LoadTotalsDataGridView();
        }

        private void LoadFCustomerAddEdit(string transaction, string customer_id, string fullname)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            Customer.FCustomerAddEdit fcae = new Customer.FCustomerAddEdit();
            fcae.TransactionName = transaction;
            fcae.CustomerID = customer_id;
            fcae.CustomerFullName = fullname;
            fcae.RefreshCustomersDataGridView += new Customer.FCustomerAddEdit.DoEvent(RefreshCustomers);
            fcae.ShowDialog();

            TotalsGridView.FocusedRowHandle = old_row_num;
            TotalsGridView.TopRowIndex = topindex;
        }

        private void AddSpeacialAttentionBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş " + contract_code + " saylı Lizinq Müqaviləsinə xüsusi nəzarət əlavə etmək istəyirsiniz?", "Xüsusi nəzarət", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACTS SET IS_SPECIAL_ATTENTION = 1 WHERE ID = {contract_id}", "Xüsusi nəzarət əlavə edilmədi.", this.Name + "/AddSpeacialAttentionBarButton_ItemClick");
            }
            LoadTotalsDataGridView();
            TotalsGridView.TopRowIndex = topindex;
            TotalsGridView.FocusedRowHandle = old_row_num;
        }

        private void CellCopybarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ClipboardGridViewCell(TotalsGridView);
        }        

        private void FontsComboBox_Properties_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            e.AllowDrawSkinBackground = false;
            Color value;
            if (portfelFont.TryGetValue(e.Item.ToString(), out value))
                e.Appearance.BackColor = value;
        }

        private void LoadFJuridicalPersonAddEdit(string transaction, string person_id)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;

            Customer.FJuridicalPersonAddEdit fj = new Customer.FJuridicalPersonAddEdit();
            fj.TransactionName = transaction;
            fj.CustomerID = person_id;
            fj.RefreshCustomersDataGridView += new Customer.FJuridicalPersonAddEdit.DoEvent(RefreshCustomers);
            fj.ShowDialog();

            TotalsGridView.FocusedRowHandle = old_row_num;
            TotalsGridView.TopRowIndex = topindex;
        }

        private void ReportsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FPortfelReports fpr = new FPortfelReports();
            fpr.ShowDialog();
        }

        private void ContractStatusEditBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFContractStatusEdit();
        }

        private void TotalsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(TotalsGridView, PopupMenu, e);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "mht");
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(TotalsGridControl);
        }

        private void TotalsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            GlobalProcedures.GridRowCellStyleForClose(6, TotalsGridView, e);
        }

        private void TotalsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (currentView.RowCount == 0)
                return;

            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                calc_debt =
                    calc_amount =
                    calc_basic_amount =
                    calc_interest_amount =
                    calc_current_interest_amount =
                    calc_payment_amount =
                    calc_payment_interest_amount =
                    calc_payment_interest_debt =
                    calc_total = 0;
            }


            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0) //gelir
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_AMOUNT") == 0) //odenilen
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_payment_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("BASIC_AMOUNT") == 0) //esas mebleg
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_basic_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //gelir
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_debt += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST_AMOUNT") == 0) //hesablanmis faiz
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_interest_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("CURRENT_INTEREST_AMOUNT") == 0) //hesablanmis faiz
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_current_interest_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_AMOUNT") == 0) //odenilen faiz
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_payment_interest_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) //qaliq faiz
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_payment_interest_debt += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("Totals_Total") == 0) //cemi
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_total += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0) //verilen              
                    e.TotalValue = calc_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_AMOUNT") == 0) //odenilen
                    e.TotalValue = calc_payment_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("BASIC_AMOUNT") == 0) //esas mebleg
                    e.TotalValue = calc_basic_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST_AMOUNT") == 0) //hesablanmis faiz
                    e.TotalValue = calc_interest_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("CURRENT_INTEREST_AMOUNT") == 0) //hesablanmis faiz
                    e.TotalValue = calc_current_interest_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_AMOUNT") == 0) //odenilen faiz
                    e.TotalValue = calc_payment_interest_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) //qaliq faiz
                    e.TotalValue = calc_payment_interest_debt;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("Totals_Total") == 0) //cemi
                    e.TotalValue = calc_total;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("FULL_CUSTOMER_NAME") == 0 && TotalsGridView.RowCount > 0) //cemi
                {
                    e.TotalValue = e.GetValue("CURRENCY_CODE").ToString() + " üzrə cəmi";
                    if (e.IsTotalSummary)
                        e.TotalValue = "YEKUN (AZN - ilə)";
                }
            }
        }

        private void TotalsGridView_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            info.GroupText = string.Format("{0} : {1}   (cəmi  {2} müqavilə)", caption, info.GroupValueText, view.GetChildRowCount(e.RowHandle));
        }

        private void TotalsGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "FULL_CUSTOMER_NAME")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
            if (e.Column.FieldName == "DEBT")
                e.Appearance.ForeColor = Color.Red;
        }

        private void FTotal_Activated(object sender, EventArgs e)
        {
            lstPortfelFont = PortfelFontDAL.SelectPortfelFontByID(null).ToList<PortfelFont>();
            lstCreditTypeFont = CreditTypeFontDAL.SelectCreditTypeFontByID(null).ToList<CreditTypeFont>();
            LoadTotalsDataGridView();
            if (SearchBarButton.Down)
                FilterTotals();
        }

        private void TotalsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EnabledButton();
        }

        private void CurrentPaymentsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FCurrentPayments fcp = new FCurrentPayments();
            fcp.ShowDialog();
        }

        private void TotalsGridView_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (e.Column == Totals_Note)
            {
                if (TotalsGridView.GetListSourceRowCellValue(rowIndex, "CONTRACT_NOTE").ToString().Length > 0)
                    e.Value = Properties.Resources.notes_16;
                else
                    e.Value = null;
            }

            if (e.Column == Totals_Interest)
            {
                object interest = TotalsGridView.GetListSourceRowCellValue(rowIndex, "INTEREST");
                e.Value = interest.ToString() + " %";
            }

            if (e.Column == Totals_Period)
            {
                object period = TotalsGridView.GetListSourceRowCellValue(rowIndex, "PERIOD");
                e.Value = period.ToString() + " ay";
            }
        }

        private void EditContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFContractAddEdit("EDIT", contract_id, customer_id, seller_id);
        }
    }
}