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
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;

namespace CRS.Forms.Contracts
{
    public partial class FStatistic : DevExpress.XtraEditors.XtraForm
    {
        public FStatistic()
        {
            InitializeComponent();
        }

        private void FStatistic_Load(object sender, EventArgs e)
        {
            GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
            LoadContractDataGridView();
        }

        private void LoadContractDataGridView()
        {
            string status = null, sql = null, closed_day_filter = null, start_day_filter = null;

            if (ClosedCheck.Checked && !ActiveCheck.Checked)
                status = " AND STATUS_ID = 2 ";
            else if (ActiveCheck.Checked && !ClosedCheck.Checked)
                status = " AND STATUS_ID = 1 ";

            if (ClosedFromDateValue.Text.Length > 0 && ClosedToDateValue.Text.Length > 0)
                closed_day_filter = $@" AND CLOSED_DAY BETWEEN TO_DATE ('{ClosedFromDateValue.Text}', 'DD/MM/YYYY')
                                                                        AND TO_DATE ('{ClosedToDateValue.Text}', 'DD/MM/YYYY')";

            if (FromDateValue.Text.Length > 0 && ToDateValue.Text.Length > 0)
                start_day_filter = $@" AND CDATE BETWEEN TO_DATE ('{FromDateValue.Text}', 'DD/MM/YYYY')
                                                    AND TO_DATE ('{ToDateValue.Text}', 'DD/MM/YYYY')";

            sql = $@"SELECT *
                          FROM (SELECT CON.CONTRACT_ID,
                                       CON.CONTRACT_CODE,
                                       CUS.FULLNAME CUSTOMER_NAME,
                                       CON.START_DATE CDATE,
                                       CON.END_DATE,
                                       CON.AMOUNT,
                                       CON.CURRENCY_CODE,
                                       CON.CURRENCY_RATE,
                                       CON.AMOUNT * CON.CURRENCY_RATE AMOUNT_AZN,
                                       HOS.HOSTAGE,
                                       'Verilmiş müqavilələr' CONTRACT_STATUS,
                                       1 STATUS_ID,
                                       NULL CLOSED_DAY
                                  FROM CRS_USER.V_CONTRACTS CON,
                                       CRS_USER.V_CUSTOMERS CUS,
                                       CRS_USER.V_HOSTAGE HOS
                                 WHERE     CON.CUSTOMER_ID = CUS.ID
                                       AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                       AND CON.CONTRACT_ID = HOS.CONTRACT_ID
                                UNION ALL
                                SELECT CON.CONTRACT_ID,
                                       CON.CONTRACT_CODE,
                                       NVL (COM.COMMITMENT_NAME, CUS.FULLNAME) CUSTOMER_NAME,
                                       CON.START_DATE CDATE,
                                       CON.END_DATE,
                                       PAY.BASIC_AMOUNT AMOUNT,
                                       CON.CURRENCY_CODE,
                                       PAY.CURRENCY_RATE CURRENCY_RATE,
                                       PAY.BASIC_AMOUNT * PAY.CURRENCY_RATE AMOUNT_AZN,
                                       HOS.HOSTAGE,
                                       'Bağlanılmış müqavilələr' CONTRACT_STATUS,
                                       2 STATUS_ID,
                                       PAY.PAYMENT_DATE CLOSED_DAY
                                  FROM CRS_USER.V_CONTRACTS CON,
                                       CRS_USER.V_CUSTOMERS CUS,
                                       CRS_USER.V_HOSTAGE HOS,
                                       CRS_USER.V_CUSTOMER_LAST_PAYMENT PAY,
                                       CRS_USER.V_COMMITMENTS COM
                                 WHERE     CON.CUSTOMER_ID = CUS.ID
                                       AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                       AND CON.STATUS_ID = 6
                                       AND CON.CONTRACT_ID = HOS.CONTRACT_ID
                                       AND CON.CONTRACT_ID = PAY.CONTRACT_ID(+)
                                       AND CON.CONTRACT_ID = COM.CONTRACT_ID(+))
                         WHERE 1 = 1
                            {status}{closed_day_filter}{start_day_filter}
                        ORDER BY CONTRACT_CODE DESC,CDATE DESC,CURRENCY_CODE";

            ContractGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContractDataGridView", "Müqavilələrin statistikası yüklənmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            ToDateValue_EditValueChanged(sender, EventArgs.Empty);
        }    

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ContractGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalına olan statistika";
            LoadContractDataGridView();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractDataGridView();
        }

        private void ContractGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void ContractGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("AMOUNT_AZN", "Far", e);
        }

        private void ActiveCheck_CheckedChanged(object sender, EventArgs e)
        {
            GlobalProcedures.ChangeCheckStyle((sender as CheckEdit));
            LoadContractDataGridView();
        }

        private void ContractGridView_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            info.GroupText = string.Format("{0} : {1}   (cəmi  {2} müqavilə)", caption, info.GroupValueText, view.GetChildRowCount(e.RowHandle));
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ContractGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractGridControl, "xls");
        }

        private void ContractGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractGridView, PopupMenu, e);
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            string currency_name = " [CURRENCY_CODE] IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                ContractGridView.ActiveFilter.Add(ContractGridView.Columns["CURRENCY_CODE"],
                    new ColumnFilterInfo(currency_name, ""));
            else
                ContractGridView.ActiveFilter.Remove(ContractGridView.Columns["CURRENCY_CODE"]);
        }

        private void ContractGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "CONTRACT_CODE")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
        }

        private void ContractGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (e.Column.FieldName == "CDATE" || e.Column.FieldName == "END_DATE")
            {
                DateTime cdate = Convert.ToDateTime(currentView.GetRowCellValue(e.RowHandle, "CDATE")),
                         edate = Convert.ToDateTime(currentView.GetRowCellValue(e.RowHandle, "END_DATE"));

                if (cdate > edate)
                {
                    e.Appearance.BackColor = GlobalFunctions.CreateColor(-1048576);
                    e.Appearance.BackColor2 = GlobalFunctions.CreateColor(-1048576);
                }
            }
        }

        private void ClosedFromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ClosedToDateValue.Properties.MinValue = ClosedFromDateValue.DateTime;
            LoadContractDataGridView();
        }
    }
}