using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using System.Collections;

namespace CRS.Forms.Bookkeeping
{
    public partial class FJournal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FJournal()
        {
            InitializeComponent();
        }
        string debit_account, credit_account, debit_account_number, credit_account_number, odate, operationid, filter_debit_account, filter_credit_account, contract_id, active_filter_string = null;
        double amount = 0;
        DateTime d;
        int debit_account_type, credit_account_type, topindex, row_num, old_row_num = 0, is_manual, type_id, operationID;
        bool FormStatus = false;
        List<AccountingPlan> lstAccountingPlan = AccountingPlanDAL.SelectAccountingPlan(null).ToList<AccountingPlan>();
        List<OperationJournal> lstOperationJournal = null;

        public delegate void DoEvent();
        public event DoEvent RefreshOperationsDataGridView;

        private void FJournal_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                NewBarButton.Enabled = GlobalVariables.AddJournal;
                AgainBarSubItem.Enabled = GlobalVariables.AgainCalculated;
                LogBarButton.Enabled = GlobalVariables.OperationHistory;
            }

            SearchDockPanel.Hide();
            FromDateValue.EditValue = DateTime.Today.AddMonths(-2);
            ToDateValue.EditValue = DateTime.Today;
            FormStatus = true;
            GlobalProcedures.SplashScreenClose();
        }

        private void LoadOperations()
        {
            string filterDate = null, filterUser = null;

            if (FromDateValue.Text.Length > 0 && ToDateValue.Text.Length > 0)
            {
                filterDate = $@" AND OJ.OPERATION_DATE BETWEEN TO_DATE('{FromDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}','DD.MM.YYYY')";
                OperationsGridView.OptionsView.ShowViewCaption = true;
                OperationsGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalına olan əməliyyatlar";
            }
            else
                OperationsGridView.OptionsView.ShowViewCaption = false;

            if (UserComboBox.Text.Length > 0)
                filterUser = " AND NVL (CU.NAME, 'Avtomatik') IN ('" + UserComboBox.Text.Replace("; ", "','") + "')";

            string s = $@"SELECT OJ.OPERATION_DATE ODATE,
                         NVL (CU.NAME, 'Avtomatik') USERNAME,
                         OJ.APPOINTMENT,
                         OJ.DEBIT_ACCOUNT,
                         OJ.CREDIT_ACCOUNT,
                         OJ.CURRENCY_RATE,
                         OJ.AMOUNT_CUR,
                         OJ.AMOUNT_AZN,
                         OJ.ID,
                         OJ.USED_USER_ID,
                         OJ.YR_MNTH_DY,
                         SUBSTR (OJ.DEBIT_ACCOUNT, 0, 3) DACCOUNT,
                         SUBSTR (OJ.CREDIT_ACCOUNT, 0, 3) CACCOUNT,
                         OJ.IS_MANUAL,
                         OJ.ACCOUNT_OPERATION_TYPE_ID,
                         OJ.CONTRACT_ID,
                         ROW_NUMBER () OVER (ORDER BY OJ.OPERATION_DATE, OJ.ID) ROW_NUM,
                         TO_CHAR(OJ.ETL_DT_TM,'YYYYMMDD') ETL_DT_TM,
                         OJ.NOTE
                    FROM CRS_USER.OPERATION_JOURNAL OJ
                         LEFT JOIN CRS_USER.CRS_USERS CU ON OJ.CREATED_USER_ID = CU.ID
                    WHERE 1 = 1{filterDate}{filterUser}
                ORDER BY OJ.OPERATION_DATE, OJ.ID";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperations", "Jurnal yüklənmədi.");

            OperationsGridControl.DataSource = dt;

            if (OperationsGridView.RowCount > 0)
            {
                EditBarButton.Enabled = (GlobalVariables.EditJournal && is_manual == 1);
                DeleteBarButton.Enabled = (GlobalVariables.DeleteJournal && is_manual == 1);
                PaymentsBarButton.Enabled = (type_id == 1);
                CopyBarButton.Enabled = (GlobalVariables.CopyJournal && is_manual == 1);
            }
            else
            {
                EditBarButton.Enabled = DeleteBarButton.Enabled = PaymentsBarButton.Enabled = CopyBarButton.Enabled = false;
                CurrentDebitDebtValue.Value = CurrentCreditDebtValue.Value = FirstDebitDebtValue.Value = FirstCreditDebtValue.Value = 0;
            }

            //if (String.IsNullOrWhiteSpace(active_filter_string))
            //    OperationsGridView.ActiveFilter.Add(OperationsGridView.Columns["YR_MNTH_DY"],
            //        new ColumnFilterInfo("[YR_MNTH_DY] >= '" + (new DateTime(DateTime.Today.Year, 1, 1)).ToString("yyyyMMdd") + "' AND [YR_MNTH_DY] <= '" + (new DateTime(DateTime.Today.Year, 12, 31)).ToString("yyyyMMdd") + "'", ""));
            //else
            //    OperationsGridView.ActiveFilterString = active_filter_string;
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            active_filter_string = OperationsGridView.ActiveFilterString;
            LoadOperations();
        }

        private void OperationsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(OperationsGridView, PopupMenu, e);
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
            {
                GlobalProcedures.FillCheckedComboBoxWithSqlText(DebitAccountComboBox, "SELECT DISTINCT DEBIT_ACCOUNT,DEBIT_ACCOUNT,DEBIT_ACCOUNT FROM CRS_USER.OPERATION_JOURNAL ORDER BY 1");
                GlobalProcedures.FillCheckedComboBoxWithSqlText(CreditAccountComboBox, "SELECT DISTINCT CREDIT_ACCOUNT,CREDIT_ACCOUNT,CREDIT_ACCOUNT FROM CRS_USER.OPERATION_JOURNAL ORDER BY 1");
                GlobalProcedures.FillCheckedComboBoxWithSqlText(UserComboBox, $@"SELECT DISTINCT NVL (CU.NAME, 'Avtomatik') USERNAME
                                                                                      FROM CRS_USER.OPERATION_JOURNAL OJ
                                                                                           LEFT JOIN CRS_USER.CRS_USERS CU ON OJ.CREATED_USER_ID = CU.ID ORDER BY 1");
                SearchDockPanel.Show();
            }
            else
                SearchDockPanel.Hide();
        }

        private void OperationsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(OperationsGridView, e);
        }

        void RefreshOperation()
        {
            active_filter_string = OperationsGridView.ActiveFilterString;
            LoadOperations();
        }

        private void LoadFAccountingOperationAddEdit(string transaction, int? operationid)
        {
            topindex = OperationsGridView.TopRowIndex;
            old_row_num = OperationsGridView.FocusedRowHandle;
            FAccountingOperationAddEdit faoa = new FAccountingOperationAddEdit();
            faoa.TransactionName = transaction;
            faoa.OperationID = operationid;
            faoa.RefreshOperationsDataGridView += new FAccountingOperationAddEdit.DoEvent(RefreshOperation);
            faoa.ShowDialog();
            OperationsGridView.TopRowIndex = topindex;
            OperationsGridView.FocusedRowHandle = old_row_num;
        }

        private void NewBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFAccountingOperationAddEdit("INSERT", null);
        }

        private void OperationsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = OperationsGridView.GetFocusedDataRow();
            if (row != null)
            {
                operationid = row["ID"].ToString();
                odate = row["ODATE"].ToString().Substring(0, 10).Trim();
                contract_id = row["CONTRACT_ID"].ToString();
                type_id = Convert.ToInt32(row["ACCOUNT_OPERATION_TYPE_ID"].ToString());
                is_manual = Convert.ToInt32(row["IS_MANUAL"].ToString());
                amount = Convert.ToDouble(row["AMOUNT_AZN"].ToString());

                DateTime prev_date = GlobalFunctions.ChangeStringToDate(odate, "ddmmyyyy").AddDays(-1);
                debit_account = row["DEBIT_ACCOUNT"].ToString();
                credit_account = row["CREDIT_ACCOUNT"].ToString();
                if (debit_account.Length > 0)
                {
                    debit_account_number = debit_account.Substring(0, 3);
                    debit_account_type = lstAccountingPlan.Find(p => p.ACCOUNT_NUMBER == int.Parse(debit_account_number)).ACCOUNT_TYPE_ID;
                    DebitAccountDescriptionLabel.Text = debit_account_number + " hesabının debetinin " + prev_date.ToString("d", GlobalVariables.V_CultureInfoAZ) + " tarixinə olan qalığı";
                }

                if (credit_account.Length > 0)
                {
                    credit_account_number = credit_account.Substring(0, 3);
                    credit_account_type = lstAccountingPlan.Find(p => p.ACCOUNT_NUMBER == int.Parse(credit_account_number)).ACCOUNT_TYPE_ID;
                    CreditAccountDescriptionLabel.Text = credit_account_number + " hesabının kreditinin " + prev_date.ToString("d", GlobalVariables.V_CultureInfoAZ) + " tarixinə olan qalığı";
                }

                CurrentDebtDebitAccountLabel.Text = "Cari qalıq (" + odate + ")";
                CurrentDebtCreditAccountLabel.Text = "Cari qalıq (" + odate + ")";
                FirstDebitDebtValue.Value = FindDebt(odate, debit_account_number, 1);
                FirstCreditDebtValue.Value = FindDebt(odate, credit_account_number, 2);
                if (debit_account_type == 1)
                    CurrentDebitDebtValue.Value = FirstDebitDebtValue.Value + FindAmount(odate, debit_account_number, 1) - FindAmount(odate, debit_account_number, 2);
                else
                    CurrentDebitDebtValue.Value = FirstDebitDebtValue.Value + FindAmount(odate, debit_account_number, 2) - FindAmount(odate, debit_account_number, 1);
                if (credit_account_type == 1)
                    CurrentCreditDebtValue.Value = FirstCreditDebtValue.Value + FindAmount(odate, credit_account_number, 1) - FindAmount(odate, credit_account_number, 2);
                else
                    CurrentCreditDebtValue.Value = FirstCreditDebtValue.Value + FindAmount(odate, credit_account_number, 2) - FindAmount(odate, credit_account_number, 1);


                row_num = Convert.ToInt32(row["ROW_NUM"].ToString());

                EditBarButton.Enabled = DeleteBarButton.Enabled = (is_manual == 1);
                PaymentsBarButton.Enabled = (type_id == 1);
                CopyBarButton.Enabled = (GlobalVariables.CopyJournal && is_manual == 1);
            }
        }

        private decimal FindDebt(string date, string accountnumber, int amounttype)
        {
            decimal res = 0;
            string s = null;

            switch (amounttype)
            {
                case 1:
                    s = $@" SELECT SUM (DEBIT) - SUM (CREDIT) DEBT
                                FROM (SELECT SUBSTR (ACCOUNT, 0, 3) ACCOUNT_NUMBER, DEBIT, CREDIT
                                        FROM CRS_USER.OPERATION_DEBT OD
                                       WHERE DEBT_DATE =
                                                (SELECT MAX (DEBT_DATE)
                                                   FROM CRS_USER.OPERATION_DEBT
                                                  WHERE     OD.ACCOUNT = ACCOUNT
                                                        AND DEBT_DATE <
                                                               TO_DATE ('{date}', 'DD/MM/YYYY')))
                            WHERE ACCOUNT_NUMBER = {accountnumber}                                   
                            GROUP BY ACCOUNT_NUMBER";
                    break;
                case 2:
                    s = $@" SELECT SUM (CREDIT) - SUM (DEBIT) DEBT
                                FROM (SELECT SUBSTR (ACCOUNT, 0, 3) ACCOUNT_NUMBER, DEBIT, CREDIT
                                        FROM CRS_USER.OPERATION_DEBT OD
                                       WHERE DEBT_DATE =
                                                (SELECT MAX (DEBT_DATE)
                                                   FROM CRS_USER.OPERATION_DEBT
                                                  WHERE     OD.ACCOUNT = ACCOUNT
                                                        AND DEBT_DATE <
                                                               TO_DATE ('{date}', 'DD/MM/YYYY')))
                            WHERE ACCOUNT_NUMBER = {accountnumber}                                   
                            GROUP BY ACCOUNT_NUMBER";
                    break;

            }
            return res;
        }

        private decimal FindAmount(string date, string accountnumber, int amounttype)
        {
            decimal res;
            if (amounttype == 1)
                res = (decimal)GlobalFunctions.GetAmount($@"SELECT NVL(SUM(OJ.AMOUNT_AZN),0) FROM CRS_USER.OPERATION_JOURNAL OJ WHERE SUBSTR(OJ.DEBIT_ACCOUNT,0,3) = {accountnumber} AND OJ.OPERATION_DATE = TO_DATE('{date}','DD/MM/YYYY')");
            else
                res = (decimal)GlobalFunctions.GetAmount($@"SELECT NVL(SUM(OJ.AMOUNT_AZN),0) FROM CRS_USER.OPERATION_JOURNAL OJ WHERE SUBSTR(OJ.CREDIT_ACCOUNT,0,3) = {accountnumber} AND OJ.OPERATION_DATE = TO_DATE('{date}','DD/MM/YYYY')");
            return res;
        }

        void Edit()
        {
            operationID = Convert.ToInt32(GlobalFunctions.GetGridRowCellValue(OperationsGridView, "ID"));
            LoadFAccountingOperationAddEdit("EDIT", operationID);
        }

        private void EditBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Edit();
        }

        private void OperationsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                Edit();
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(OperationsGridControl);
        }

        private void ExcellBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "mht");
        }

        private void DeleteOperation(int id)
        {
            string date, debit_account, credit_account, sql = null, logSQL = null, sqlParent = null;
            lstOperationJournal = OperationJournalDAL.SelectOperationJournal(int.Parse(operationid)).ToList<OperationJournal>();
            var journal = lstOperationJournal.First();
            date = journal.OPERATION_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
            debit_account = journal.DEBIT_ACCOUNT;
            credit_account = journal.CREDIT_ACCOUNT;
            sql = $@"DELETE FROM CRS_USER.OPERATION_JOURNAL WHERE ID = {id}";
            sqlParent = $@"DELETE FROM CRS_USER.OPERATION_JOURNAL WHERE PARENT_ID = {id}";
            logSQL = $@"INSERT INTO CRS_USER.OPERATION_JOURNAL_LOG (ID,
                                            OPERATION_DATE,
                                            CREATED_USER_ID,
                                            DEBIT_ACCOUNT,
                                            CREDIT_ACCOUNT,
                                            CURRENCY_RATE,
                                            AMOUNT_CUR,
                                            AMOUNT_AZN,
                                            APPOINTMENT,
                                            CONTRACT_ID,
                                            CUSTOMER_PAYMENT_ID,
                                            ACCOUNT_OPERATION_TYPE_ID,
                                            IS_MANUAL,
                                            USER_ID,
                                            YR_MNTH_DY,
                                            LOG_TYPE)
                           SELECT ID,
                                  OPERATION_DATE,
                                  CREATED_USER_ID,
                                  DEBIT_ACCOUNT,
                                  CREDIT_ACCOUNT,
                                  CURRENCY_RATE,
                                  AMOUNT_CUR,
                                  AMOUNT_AZN,
                                  APPOINTMENT,
                                  CONTRACT_ID,
                                  CUSTOMER_PAYMENT_ID,
                                  ACCOUNT_OPERATION_TYPE_ID,
                                  IS_MANUAL,
                                  {GlobalVariables.V_UserID} USER_ID,
                                  YR_MNTH_DY,
                                  3 LOG_TYPE
                             FROM CRS_USER.OPERATION_JOURNAL
                            WHERE ID = {id}";
            GlobalProcedures.ExecuteThreeQuery(logSQL, sqlParent, sql, "Əməliyyat silinmədi.", this.Name + "/DeleteOperation");
        }

        private void OperationsGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void YearGroupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (YearGroupBarButton.Down)
                OperationsGridView.Columns[18].GroupIndex = 0;
            else
                OperationsGridView.Columns[18].GroupIndex = -1;
            LoadOperations();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FilterOperations();
        }

        private void OperationsGridControl_Load(object sender, EventArgs e)
        {
            ((GridView)OperationsGridControl.MainView).MoveLast();
        }

        private void AgainAllPaymentsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FCalcAgainPayments fca = new FCalcAgainPayments();
            fca.ShowDialog();
            LoadOperations();
        }

        private void AgainOnePaymentBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (type_id == 1)
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_ONE_PAYMENT_INS_JOURNAL", "P_PAYMENT_ID", int.Parse(operationid), "Seçilmiş müqavilə və tarix üçün lizinq ödənişi mühasibatlıqda təkrar hesablamadı");
            else
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_ONE_CONTRACT_INS_JOURNAL", "P_CONTRACT_ID", int.Parse(contract_id), "Seçilmiş müqavilə və tarix üçün alqı-satqı ödənişi mühasibatlıqda təkrar hesablamadı");
            LoadOperations();
        }

        private void FromInsertDate_EditValueChanged(object sender, EventArgs e)
        {
            ToInsertDateValue.Properties.MinValue = GlobalFunctions.ChangeStringToDate(FromInsertDateValue.Text, "ddmmyyyy");
            FilterOperations();
        }

        private void LogBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FJournalLog fjl = new FJournalLog();
            fjl.RefreshDataGridView += new FJournalLog.DoEvent(RefreshOperation);
            fjl.ShowDialog();
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = OperationsGridView.FocusedRowHandle;
            topindex = OperationsGridView.TopRowIndex;
            active_filter_string = OperationsGridView.ActiveFilterString;

            ArrayList rows = new ArrayList();

            Int32[] selectedRowHandles = OperationsGridView.GetSelectedRows();
            for (int i = 0; i < selectedRowHandles.Length; i++)
            {
                int selectedRowHandle = selectedRowHandles[i];
                if (selectedRowHandle >= 0)
                    rows.Add(OperationsGridView.GetDataRow(selectedRowHandle));
            }

            if (rows.Count == 0)
            {
                GlobalProcedures.ShowWarningMessage("Silmək istədiyiniz əməliyyatları seçin.");
                return;
            }

            if (GlobalFunctions.CallDialogResult("Seçilmiş əməliyyatları silmək istəyirsiniz?", "Əməliyyatların silinməsi") == DialogResult.Yes)
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    if (Convert.ToInt32(row["IS_MANUAL"]) == 0)
                        continue;

                    int UsedUserID = Convert.ToInt32(row["USED_USER_ID"]);
                    operationID = Convert.ToInt32(row["ID"]);
                    if (UsedUserID >= 0)
                    {
                        if (GlobalVariables.V_UserID != UsedUserID)
                        {
                            string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                            GlobalProcedures.ShowWarningMessage(row["APPOINTMENT"].ToString() + " əməliyyatı hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş əməliyyatın hal-hazırkı statusu");
                        }
                        else
                            DeleteOperation(operationID);
                    }
                    else
                        DeleteOperation(operationID);
                }

            LoadOperations();
            OperationsGridView.FocusedRowHandle = old_row_num - 1;
            OperationsGridView.TopRowIndex = topindex;
        }

        private void CopyBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            operationID = Convert.ToInt32(GlobalFunctions.GetGridRowCellValue(OperationsGridView, "ID"));
            LoadFAccountingOperationAddEdit("COPY", operationID);
        }

        private void CellCopyBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ClipboardGridViewCell(OperationsGridView);
        }

        private void FJournal_Activated(object sender, EventArgs e)
        {
            LoadOperations();
            OperationsGridView.FocusedRowHandle = OperationsGridView.TopRowIndex = OperationsGridView.RowCount - 1;
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = GlobalFunctions.ChangeStringToDate(FromDateValue.Text, "ddmmyyyy");
            LoadOperations();
        }

        private void FilterOperations()
        {
            if (FormStatus)
            {
                ColumnView view = OperationsGridView;

                //Debit
                if (!String.IsNullOrEmpty(DebitAccountComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["DEBIT_ACCOUNT"],
                        new ColumnFilterInfo(filter_debit_account, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["DEBIT_ACCOUNT"]);

                //Credit
                if (!String.IsNullOrEmpty(CreditAccountComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CREDIT_ACCOUNT"],
                        new ColumnFilterInfo(filter_credit_account, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CREDIT_ACCOUNT"]);

                //Appointment
                if (!String.IsNullOrEmpty(AppointmentText.Text))
                    view.ActiveFilter.Add(view.Columns["APPOINTMENT"],
                      new ColumnFilterInfo("[APPOINTMENT] Like '%" + AppointmentText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["APPOINTMENT"]);

                //Amount
                if (IsZeroCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["AMOUNT_AZN"],
                      new ColumnFilterInfo("[AMOUNT_AZN] >= 0", ""));
                else
                    view.ActiveFilter.Add(view.Columns["AMOUNT_AZN"],
                      new ColumnFilterInfo("[AMOUNT_AZN] > 0", ""));

                //Type
                if (ContractCheck.Checked && !PaymentCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["ACCOUNT_OPERATION_TYPE_ID"],
                      new ColumnFilterInfo("[ACCOUNT_OPERATION_TYPE_ID] = 2", ""));
                else if (PaymentCheck.Checked && !ContractCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["ACCOUNT_OPERATION_TYPE_ID"],
                      new ColumnFilterInfo("[ACCOUNT_OPERATION_TYPE_ID] = 1", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["ACCOUNT_OPERATION_TYPE_ID"]);

                //InsertDate
                if (!String.IsNullOrEmpty(FromInsertDateValue.Text) && !String.IsNullOrEmpty(ToInsertDateValue.Text))
                {
                    view.ActiveFilter.Remove(view.Columns["YEAR"]);
                    view.ActiveFilter.Add(view.Columns["ETL_DT_TM"],
                        new ColumnFilterInfo("[ETL_DT_TM] >= '" + FromInsertDateValue.DateTime.ToString("yyyyMMdd") + "' AND [ETL_DT_TM] <= '" + ToInsertDateValue.DateTime.ToString("yyyyMMdd") + "'", ""));
                }
                else
                    view.ActiveFilter.Remove(view.Columns["ETL_DT_TM"]);
            }
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            LoadOperations();
        }

        private void DebitAccountComboBox_EditValueChanged(object sender, EventArgs e)
        {
            filter_debit_account = " [DEBIT_ACCOUNT] IN ('" + DebitAccountComboBox.Text.Replace("; ", "','") + "')";
            FilterOperations();
        }

        private void FilterClearBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FromDateValue.Text =
            ToDateValue.Text =
            AppointmentText.Text =
            DebitAccountComboBox.Text =
            CreditAccountComboBox.Text =
            FromInsertDateValue.Text =
            UserComboBox.Text =
            ToInsertDateValue.Text = null;
            OperationsGridView.ClearColumnsFilter();
        }

        private void CreditAccountComboBox_EditValueChanged(object sender, EventArgs e)
        {
            filter_credit_account = " [CREDIT_ACCOUNT] IN ('" + CreditAccountComboBox.Text.Replace("; ", "','") + "')";
            FilterOperations();
        }

        private void IsZeroCheck_CheckedChanged(object sender, EventArgs e)
        {
            FilterOperations();
        }

        private void FJournal_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshOperationsDataGridView();
        }

        private void PaymentsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = OperationsGridView.TopRowIndex;
            old_row_num = OperationsGridView.FocusedRowHandle;
            FShowPayments fsp = new FShowPayments();
            fsp.ContractID = contract_id;
            fsp.ShowDialog();
            OperationsGridView.TopRowIndex = topindex;
            OperationsGridView.FocusedRowHandle = old_row_num;
            active_filter_string = OperationsGridView.ActiveFilterString;
        }

        private void CalculatorBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void ExchangeBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ));
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void OperationsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (OperationsGridView.RowCount > 0)
            {
                EditBarButton.Enabled = (GlobalVariables.EditJournal && is_manual == 1);
                DeleteBarButton.Enabled = (GlobalVariables.DeleteJournal && is_manual == 1);
                PaymentsBarButton.Enabled = (type_id == 1);
                CopyBarButton.Enabled = GlobalVariables.CopyJournal && is_manual == 1;
            }
            else
            {
                EditBarButton.Enabled = DeleteBarButton.Enabled = PaymentsBarButton.Enabled = CopyBarButton.Enabled = false;
                CurrentDebitDebtValue.Value = CurrentCreditDebtValue.Value = FirstDebitDebtValue.Value = FirstCreditDebtValue.Value = 0;
            }
        }

        private void ExpandBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            CollapseBarButton.Down = !ExpandBarButton.Down;
            LoadOperations();
        }

        private void CollapseBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ExpandBarButton.Down = !CollapseBarButton.Down;
            LoadOperations();
        }
    }
}