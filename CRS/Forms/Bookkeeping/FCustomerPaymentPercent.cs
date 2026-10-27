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
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace CRS.Forms.Bookkeeping
{
    public partial class FCustomerPaymentPercent : DevExpress.XtraEditors.XtraForm
    {
        public FCustomerPaymentPercent()
        {
            InitializeComponent();
        }
        string contractID, currency, contractCode;
        decimal amount;
        int topindex, old_row_num;

        private void FCustomerPaymentPercent_Load(object sender, EventArgs e)
        {
            FromDateValue.EditValue = ToDateValue.EditValue = DateTime.Today;
        }

        private void LoadData()
        {
            string sql = $@"SELECT *
                                FROM (SELECT CON.CONTRACT_ID,
                                             COM.CUSTOMER_NAME,
                                             CP.PAYMENT_NAME,
                                             CON.CONTRACT_CODE,
                                             CP.PAYMENT_DATE,
                                             CP.CURRENCY_RATE,
                                             CP.INTEREST_AMOUNT,
                                             ROUND (CP.INTEREST_AMOUNT * CP.CURRENCY_RATE, 2)
                                                INTEREST_AMOUNT_AZN, 
                                             CP.PAYMENT_INTEREST_AMOUNT,
                                             ROUND (CP.PAYMENT_INTEREST_AMOUNT * CP.CURRENCY_RATE, 2)
                                                PAYMENT_INTEREST_AMOUNT_AZN,
                                             CP.BASIC_AMOUNT,
                                             ROUND (CP.BASIC_AMOUNT * CP.CURRENCY_RATE, 2) BASIC_AMOUNT_AZN,
                                             CON.CURRENCY_CODE,
                                             DECODE (
                                                CP.BANK_CASH,
                                                'C', 'Kassa',
                                                   'Bank - '
                                                || (SELECT B.SHORT_NAME
                                                      FROM CRS_USER.BANKS B
                                                     WHERE B.ID IN (SELECT BANK_ID
                                                                      FROM CRS_USER.BANK_OPERATIONS
                                                                     WHERE CONTRACT_PAYMENT_ID = CP.ID)))
                                                PAYMENT_TYPE,
                                             COM.VOEN,
                                             COM.AMOUNT,
                                             CP.INSURANCE_AMOUNT,
                                             ROUND (CP.PAYED_PENALTY * CP.CURRENCY_RATE, 2) PAYED_PENALTY,
                                             (CASE
                                                 WHEN LP.DEBT IS NOT NULL AND LP.DEBT = 0 THEN 1
                                                 ELSE 0
                                              END)
                                                CONTROL_DEBT
                                        FROM CRS_USER.CUSTOMER_PAYMENTS CP,
                                             CRS_USER.V_CONTRACTS CON,
                                             CRS_USER.V_CUSTOMER_PAYMENT_OWNER COM,
                                             CRS_USER.V_CUSTOMER_LAST_PAYMENT LP
                                       WHERE     CON.CONTRACT_ID = CP.CONTRACT_ID
                                             AND CON.CONTRACT_ID = COM.CONTRACT_ID
                                             AND CP.ID = COM.PAYMENT_ID
                                             AND CP.BANK_CASH IN ('C', 'B')
                                             AND CP.PAYMENT_DATE BETWEEN TO_DATE ('{FromDateValue.Text}',
                                                                                  'DD.MM.YYYY')
                                                                     AND TO_DATE ('{ToDateValue.Text}',
                                                                                  'DD.MM.YYYY')
                                            AND CP.ID = LP.ID(+))                                             
                            ORDER BY PAYMENT_DATE";

            ListGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadData", "Müştərilərin ödədiyi faizlər açılmadı.");

            if (VoenCheck.Checked)
                ListGridView.ActiveFilter.Add(ListGridView.Columns["VOEN"],
                          new ColumnFilterInfo("[VOEN] IS NOT NULL", "VÖEN-i olanlar"));
            else
                ListGridView.ActiveFilter.Remove(ListGridView.Columns["VOEN"]);
        }

        private void StandaloneBarDockControl_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            LoadData();
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ListGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ListGridControl, "xls");
        }

        private void ListGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ListGridView.GetFocusedDataRow();
            if (row != null)
            {
                contractID = row["CONTRACT_ID"].ToString();
                currency = row["CURRENCY_CODE"].ToString();
                contractCode = row["CONTRACT_CODE"].ToString();
                amount = Convert.ToDecimal(row["AMOUNT"]);
            }
        }

        private void PaymentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = ListGridView.TopRowIndex;
            old_row_num = ListGridView.FocusedRowHandle;
            FShowPayments fsp = new FShowPayments();
            fsp.ContractID = contractID;
            fsp.ShowDialog();
            ListGridView.TopRowIndex = topindex;
            ListGridView.FocusedRowHandle = old_row_num;
        }

        private void ListGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (e.RowHandle >= 0)
            {
                int controlDebt = int.Parse(currentView.GetRowCellDisplayText(e.RowHandle, currentView.Columns["CONTROL_DEBT"]));
                if (controlDebt == 1)                
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.YellowGreen;
                
            }
        }

        private void ListGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ListGridView, PopupMenu, e);
        }

        private void ListGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, List_SS, e);
        }

        private void PaymentSheduleBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = ListGridView.TopRowIndex;
            old_row_num = ListGridView.FocusedRowHandle;
            Total.FPaymentSchedules fps = new Total.FPaymentSchedules();
            fps.ContractID = contractID;
            fps.Amount = amount.ToString("N2") + " " + currency;
            fps.ContractCode = contractCode;
            fps.ShowDialog();
            ListGridView.TopRowIndex = topindex;
            ListGridView.FocusedRowHandle = old_row_num;
        }
    }
}