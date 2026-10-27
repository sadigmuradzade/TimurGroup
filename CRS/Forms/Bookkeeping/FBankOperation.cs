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
using DevExpress.Data;
using DevExpress.XtraGrid;

namespace CRS.Forms.Bookkeeping
{
    public partial class FBankOperation : DevExpress.XtraEditors.XtraForm
    {
        public FBankOperation()
        {
            InitializeComponent();
        }
        double debit1, credit1, debit2, credit2, ddebt, cdebt;

        private void FBankOperation_Load(object sender, EventArgs e)
        {
            FromDateValue.EditValue = new DateTime(DateTime.Today.Year, 1, 1);
            ToDateValue.EditValue = new DateTime(DateTime.Today.Year, 12, 31);
            SearchDockPanel.Hide();
        }

        private void LoadOperations()
        {
            string s = $@"SELECT 1 SS,
                                     T.ACCOUNT,
                                     AP.SUB_ACCOUNT_NAME ACCOUNT_NAME,
                                     NVL ( (CASE WHEN T.T1D - T.T1C < 0 THEN 0 ELSE T.T1D - T.T1C END), 0)
                                        T1D,
                                     NVL ( (CASE WHEN T.T1C - T.T1D < 0 THEN 0 ELSE T.T1C - T.T1D END), 0)
                                        T1C,
                                     NVL (T.T2D, 0) T2D,
                                     NVL (T.T2C, 0) T2C,
                                     (CASE
                                         WHEN   (NVL (T.T1D, 0) + NVL (T.T2D, 0))
                                              - (NVL (T.T1C, 0) + NVL (T.T2C, 0)) < 0
                                         THEN
                                            0
                                         ELSE
                                              (NVL (T.T1D, 0) + NVL (T.T2D, 0))
                                            - (NVL (T.T1C, 0) + NVL (T.T2C, 0))
                                      END)
                                        DDEBT,
                                     (CASE
                                         WHEN   (NVL (T.T1C, 0) + NVL (T.T2C, 0))
                                              - (NVL (T.T1D, 0) + NVL (T.T2D, 0)) < 0
                                         THEN
                                            0
                                         ELSE
                                              (NVL (T.T1C, 0) + NVL (T.T2C, 0))
                                            - (NVL (T.T1D, 0) + NVL (T.T2D, 0))
                                      END)
                                        CDEBT
                                FROM (WITH T1
                                           AS (  SELECT ACCOUNT, NVL(SUM (DEBIT), 0) T1D, NVL(SUM (CREDIT), 0) T1C
                                                   FROM (SELECT ACCOUNT,
                                                                DEBT_DATE OPERATION_DATE,
                                                                DEBIT,
                                                                CREDIT
                                                           FROM CRS_USER.OPERATION_DEBT
                                                          WHERE YR_MNTH_DY = 20151231
                                                         UNION ALL
                                                         SELECT *
                                                           FROM (SELECT DEBIT_ACCOUNT ACCOUNT,
                                                                        AMOUNT_AZN,
                                                                        OPERATION_DATE,
                                                                        1 TYPE
                                                                   FROM CRS_USER.OPERATION_JOURNAL
                                                                 UNION ALL
                                                                 SELECT CREDIT_ACCOUNT ACCOUNT,
                                                                        AMOUNT_AZN,
                                                                        OPERATION_DATE,
                                                                        2 TYPE
                                                                   FROM CRS_USER.OPERATION_JOURNAL) PIVOT (SUM (
                                                                                                              AMOUNT_AZN)
                                                                                                    FOR TYPE
                                                                                                    IN  (1 AS DEBIT,
                                                                                                        2 AS CREDIT)))
                                                  WHERE OPERATION_DATE < TO_DATE ('{FromDateValue.Text}', 'DD.MM.YYYY')
                                               GROUP BY ACCOUNT),
                                           T2
                                           AS (  SELECT ACCOUNT, NVL(SUM (DEBIT), 0) T2D, NVL(SUM (CREDIT), 0) T2C
                                                   FROM (SELECT OJ.OPERATION_DATE,
                                                                OJ.DEBIT_ACCOUNT ACCOUNT,
                                                                1 AMOUNT_TYPE,
                                                                OJ.AMOUNT_AZN AMOUNT
                                                           FROM CRS_USER.OPERATION_JOURNAL OJ
                                                         UNION ALL
                                                         SELECT OJ.OPERATION_DATE,
                                                                OJ.CREDIT_ACCOUNT ACCOUNT,
                                                                2 AMOUNT_TYPE,
                                                                OJ.AMOUNT_AZN AMOUNT
                                                           FROM CRS_USER.OPERATION_JOURNAL OJ) PIVOT (SUM (
                                                                                                         AMOUNT)
                                                                                               FOR AMOUNT_TYPE
                                                                                               IN  (1 AS DEBIT,
                                                                                                   2 AS CREDIT))
                                                  WHERE OPERATION_DATE BETWEEN TO_DATE ('{FromDateValue.Text}',
                                                                                        'DD.MM.YYYY')
                                                                           AND TO_DATE ('{ToDateValue.Text}',
                                                                                        'DD.MM.YYYY')
                                               GROUP BY ACCOUNT)
                                      SELECT NVL (T2.ACCOUNT, T1.ACCOUNT) ACCOUNT,
                                             T1.T1D,
                                             T1C,
                                             T2D,
                                             T2C
                                        FROM T1 FULL JOIN T2 ON T1.ACCOUNT = T2.ACCOUNT) T,
                                      (SELECT NVL (SUB_ACCOUNT, ACCOUNT_NUMBER) SUB_ACCOUNT,
                                             SUB_ACCOUNT_NAME
                                        FROM CRS_USER.ACCOUNTING_PLAN
                                           WHERE ACCOUNT_NUMBER = 223) AP
                               WHERE (CASE
                                         WHEN SUBSTR (T.ACCOUNT, 0, 3) IN (174,
                                                                           543,
                                                                           216,
                                                                           243)
                                         THEN
                                            SUBSTR (T.ACCOUNT, 0, 3)
                                         ELSE
                                            T.ACCOUNT
                                        END) = AP.SUB_ACCOUNT(+) AND SUBSTR (T.ACCOUNT, 0, 3) = 223
                            ORDER BY T.ACCOUNT";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperations");
            OperationsGridControl.DataSource = dt;
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            OperationsGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalına olan əməliyyatlar";
            LoadOperations();
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void OperationsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("T1D", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("T1C", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("T2D", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("T2C", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DDEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("CDEBT", "Far", e);
            if (e.Column.FieldName == "DDEBT" || e.Column.FieldName == "CDEBT")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadOperations();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(OperationsGridControl);
        }

        private void ExcellBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "txt");
        }

        private void OperationsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(OperationsGridView, PopupMenu, e);
        }

        private void MhtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "mht");
        }

        private void CsvBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "csv");
        }

        private void OperationsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Details_SS, e);
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void OperationsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DDEBT") == 0) //qaligi hesabliyir
                {
                    debit1 = Convert.ToDouble(OperationsGridView.Columns.ColumnByFieldName("T1D").SummaryItem.SummaryValue);
                    debit2 = Convert.ToDouble(OperationsGridView.Columns.ColumnByFieldName("T2D").SummaryItem.SummaryValue);
                    credit1 = Convert.ToDouble(OperationsGridView.Columns.ColumnByFieldName("T1C").SummaryItem.SummaryValue);
                    credit2 = Convert.ToDouble(OperationsGridView.Columns.ColumnByFieldName("T2C").SummaryItem.SummaryValue);
                    ddebt = (debit1 + debit2) - (credit1 + credit2);
                    if (ddebt > 0)
                        e.TotalValue = ddebt;
                    else
                        e.TotalValue = 0;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("CDEBT") == 0) //qaligi hesabliyir
                {
                    debit1 = Convert.ToDouble(OperationsGridView.Columns.ColumnByFieldName("T1D").SummaryItem.SummaryValue);
                    debit2 = Convert.ToDouble(OperationsGridView.Columns.ColumnByFieldName("T2D").SummaryItem.SummaryValue);
                    credit1 = Convert.ToDouble(OperationsGridView.Columns.ColumnByFieldName("T1C").SummaryItem.SummaryValue);
                    credit2 = Convert.ToDouble(OperationsGridView.Columns.ColumnByFieldName("T2C").SummaryItem.SummaryValue);
                    cdebt = (credit1 + credit2) - (debit1 + debit2);
                    if (cdebt > 0)
                        e.TotalValue = cdebt;
                    else
                        e.TotalValue = 0;
                }
            }
        }
    }
}