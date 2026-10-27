using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class;

namespace CRS.Forms.Total
{
    public partial class FPenalty : DevExpress.XtraEditors.XtraForm
    {
        public FPenalty()
        {
            InitializeComponent();
        }
        public string ContractID, CustomerID, ContractCode, Currency, ContractStartDate;

        string penaltyid, penaltystatus;
        int commit, PaymentUsedUserID, penaltyID;
        double debt = 0, penalty_amount, discount_penalty, payment_penalty, last_debt, PenaltySum = 0;
        decimal lastDebt = 0;
        bool CurrentStatus = false, PaymentClosed = false, PaymentUsed = false;
        DateTime? lastEndDate = null;
        

        public delegate void DoEvent();
        public event DoEvent RefreshPenaltyValue;

        private void FPenalty_Load(object sender, EventArgs e)
        {
            this.Text = ContractCode + " saylı lizinq müqaviləsi üzrə cərimə faizləri";

            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT STATUS_ID, USED_USER_ID FROM CRS_USER.CONTRACTS WHERE CUSTOMER_ID = {CustomerID} AND ID = {ContractID}");

            if (dt.Rows.Count > 0)
            {
                PaymentClosed = Convert.ToInt16(dt.Rows[0]["STATUS_ID"]) == 6;
                PaymentUsedUserID = Convert.ToInt16(dt.Rows[0]["USED_USER_ID"]);
                PaymentUsed = PaymentUsedUserID >= 0;
            }

            if ((PaymentClosed && PaymentUsed) || (PaymentClosed && !PaymentUsed))
                CurrentStatus = true;
            else if (PaymentUsed && !PaymentClosed)
            {
                if (GlobalVariables.V_UserID != PaymentUsedUserID)
                    CurrentStatus = true;
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;

            ComponentEnable(CurrentStatus);
            LoadPenaltyDataGridView();
        }

        private void PenaltyGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PenaltyGridView.GetFocusedDataRow();
            if (row != null)
            {
                penaltyID = Convert.ToInt32(row["ID"]);
                lastDebt = Convert.ToDecimal(row["LAST_PENALTY_DEBT"]);
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal payedPenalty = Convert.ToDecimal(GlobalFunctions.GridGetRowCellValue(PenaltyGridView, "PAYED_PENALTY"));

            if (payedPenalty > 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş cərimə ödənildiyi üçün bu cəriməni silmək olmaz.");
                return;
            }

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.CONTRACT_PENALTY_TEMP WHERE IS_CHANGE != 2 AND ID > {penaltyID} AND CONTRACT_ID = {ContractID}") > 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş cərimədən sonra başqa cərimələrdə olduğu üçün bu cəriməni silmək olmaz.");
                return;
            }

            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş cəriməni silmək istəyirsiniz?", "Cərimənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));
                PenaltyDelete();
                LoadPenaltyDataGridView();
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void PenaltyDelete()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER_TEMP.PROC_DELETE_CONTRACT_PENALTY", "P_CONTRACT_ID", int.Parse(ContractID), "P_ID", penaltyID, "Cərimə silinmədi.");
        }

        private void PenaltyGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, CalcPenalty_SS, e);
        }

        public void ComponentEnable(bool status)
        {
            CalcStandaloneBarDockControl.Enabled = !status;
            if (status == false)
                CalcPopupMenu.Manager = CalcBarManager;
            else
                CalcPopupMenu.Manager = null;
        }

        void Edit()
        {
            decimal payedPenalty = Convert.ToDecimal(GlobalFunctions.GridGetRowCellValue(PenaltyGridView, "PAYED_PENALTY"));

            if (payedPenalty > 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş cərimə ödənildiyi üçün bu cəriməni dəyişmək olmaz.");
                return;
            }

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACT_CALCULATED_PENALTIES WHERE ID > {penaltyID} AND CONTRACT_ID = {ContractID}") > 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş cərimədən sonra başqa cərimələrdə olduğu üçün bu cəriməni dəyişmək olmaz.");
                return;
            }

            LoadFPenaltyAddEdit("EDIT", penaltyID);
        }

        private void EditPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Edit();
        }

        private void PenaltyGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPenaltyBarButton.Enabled)
                Edit();
        }

        private void PenaltyGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EnableButton();
        }

        private void LoadPenaltyDataGridView()
        {
            string s = $@"SELECT CP.ID,
                                 CP.LAST_PAYMENT_DATE,
                                 CP.START_DATE,
                                 CP.END_DATE,
                                 CP.DAY_COUNT,
                                 CP.DEBT,
                                 IP.INTEREST,
                                 CP.PENALTY_AMOUNT,
                                 CP.PAYED_PENALTY,
                                 CP.PENALTY_DEBT,
                                 CP.PRESENT_PENALTY,
                                 NVL (LAG (CP.PENALTY_DEBT, 1) OVER (ORDER BY CP.ID), 0) LAST_PENALTY_DEBT,
                                 NVL (LAG (CP.END_DATE, 1) OVER (ORDER BY CP.ID), C.START_DATE - 1)
                                    LAST_END_DATE
                            FROM CRS_USER_TEMP.CONTRACT_PENALTY_TEMP CP,
                                 CRS_USER.CONTRACT_INTEREST_PENALTIES IP,
                                 CRS_USER.CONTRACTS C
                           WHERE     CP.CONTRACT_ID = C.ID
                                 AND CP.INTEREST_PENALTIES_ID = IP.ID
                                 AND CP.IS_CHANGE != 2
                                 AND CP.CONTRACT_ID = {ContractID}
                        ORDER BY CP.ID";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPenaltyDataGridView", "Hesablanmış cərimələr cədvələ yüklənmədi.");
            PenaltyGridControl.DataSource = dt;

            EnableButton();
            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView();
                dv = new DataView(dt);

                object maxObject = dt.Compute("MAX(END_DATE)", null);

                lastEndDate = Convert.ToDateTime(maxObject);
            }
            else
                lastEndDate = null;
        }

        void EnableButton()
        {
            EditPenaltyBarButton.Enabled = DeleteBarButton.Enabled = PenaltyGridView.RowCount > 0;
        }

        private void RefreshPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPenaltyDataGridView();
        }

        private void PenaltyGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("DAY_COUNT", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("PENALTY_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYED_PENALTY", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PENALTY_DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PRESENT_PENALTY", "Far", e);            
        }

        private void FPenalty_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPenaltyValue();
        }

        private void UpdatePenaltyTemp()
        {
            GlobalProcedures.ExecuteTwoQuery("UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET IS_COMMIT = 1 WHERE IS_COMMIT = 0 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "UPDATE CRS_USER.CONTRACT_CALCULATED_PENALTIES SET IS_BALANCE = 1, IS_COMMIT = 0 WHERE IS_BALANCE = 0 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND EXISTS (SELECT ID FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE = 1 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + ")",
                                                "Cərimələr temp cədvələ təsdiq edilmədi.");
        }

        private void PrintPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PenaltyGridControl);
        }

        private void ExportPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PenaltyGridControl, "xls");
        }

        private void PenaltyGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PenaltyGridView, CalcPopupMenu, e);
        }

        private void NewPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPenaltyAddEdit("INSERT", null);
        }

        private void LoadFPenaltyAddEdit(string transaction, int? id)
        {
            FPenaltyAddEdit fp = new FPenaltyAddEdit();
            fp.TransactionName = transaction;
            fp.ContractID = ContractID;
            fp.ID = id;
            fp.LastPenaltyDebt = lastDebt;
            fp.LastPenaltyDate = lastEndDate;
            fp.RefreshDataGridView += new FPenaltyAddEdit.DoEvent(LoadPenaltyDataGridView);
            fp.ShowDialog();
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