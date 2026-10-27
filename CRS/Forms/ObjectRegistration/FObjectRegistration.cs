using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using CRS.Class;
using DevExpress.XtraGrid.Views.Grid;

namespace CRS.Forms.ObjectRegistration
{
    public partial class FObjectRegistration : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FObjectRegistration()
        {
            InitializeComponent();
        }
        int topindex, old_row_num, id, contractID;
        string url, burdenURL, contractCode;

        private void FObjectRegistration_Load(object sender, EventArgs e)
        {

        }

        private void LoadContract()
        {
            string sql = $@"SELECT OB.ID,
                                     OB.CONTRACT_ID,
                                     C.CONTRACT_CODE,
                                     OB.SENT_DATE,
                                     OB.ARRIVAL_DATE,
                                     OB.REGISTRATION_NUMBER,
                                     OB.REGISTRATION_DATE,
                                     OB.URL,
                                     OB.NOTE,
                                     HO.ADDRESS,
                                     HO.AREA,
                                     (SELECT USER_FULLNAME
                                        FROM CRS_USER.V_USERS
                                       WHERE ID = OB.UPDATE_USER)
                                        LAST_UPDATE_USER,
                                     OB.UPDATE_DATE,
                                     OB.USED_USER_ID,
                                     (CASE
                                         WHEN     OB.ARRIVAL_DATE = TRUNC (SYSDATE)
                                              AND OB.REGISTRATION_NUMBER IS NULL
                                         THEN
                                            1
                                         WHEN     OB.ARRIVAL_DATE < TRUNC (SYSDATE)
                                              AND OB.REGISTRATION_NUMBER IS NULL
                                         THEN
                                            2
                                         ELSE
                                            0
                                      END)
                                        WARNING,
                                     BR.REGISTRATION_NUMBER BURDEN_REGISTRATION_NUMBER,
                                     BR.URL BURDEN_URL
                                FROM CRS_USER.OBJECT_REGISTRATION OB,
                                     CRS_USER.V_CONTRACTS C,
                                     CRS_USER.HOSTAGE_OBJECT HO,
                                     CRS_USER.V_LAST_BURDEN_REGISTRATION BR
                               WHERE     OB.CONTRACT_ID = C.CONTRACT_ID
                                     AND C.CONTRACT_ID = HO.CONTRACT_ID
                                     AND OB.ID = BR.OBJECT_REGISTRATION_ID(+)
                            ORDER BY C.CONTRACT_CODE DESC";

            ContractsGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContract", "Müqavilələrin siyahısı açılmadı.");

            EnabledButton();
        }

        void EnabledButton()
        {
            ConfirmBarButton.Enabled =
                DeleteBarButton.Enabled = ContractsGridView.RowCount > 0;
        }

        private void FObjectRegistration_Activated(object sender, EventArgs e)
        {
            LoadContract();
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadContract();
        }

        private void ContractsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Contract_SS, e);
        }

        private void ContractsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (ConfirmBarButton.Enabled)
                Confirm();
        }

        private void ContractsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EnabledButton();
        }

        private void ContractsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractsGridView.GetFocusedDataRow();
            if (row != null)
            {
                id = int.Parse(row["ID"].ToString());
                contractID = int.Parse(row["CONTRACT_ID"].ToString());
                url = row["URL"].ToString();
                burdenURL = row["BURDEN_URL"].ToString();
                contractCode = row["CONTRACT_CODE"].ToString();
                ViewBarSubItem.Enabled = url.Length > 0;
            }
        }

        private void ViewUrlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ContractsGridControl);
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = ContractsGridView.FocusedRowHandle;
            topindex = ContractsGridView.TopRowIndex;
            if (GlobalFunctions.CallDialogResult(contractCode + " saylı müqavilənin çıxarışını silmək istəyirsiniz?", "Çıxarışın silinməsi") == DialogResult.Yes)
                GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.OBJECT_REGISTRATION WHERE ID = {id}", "Çıxarış silinmədi", this.Name + "/DeleteBarButton_ItemClick");
            LoadContract();
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void ContractsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (int.Parse(ContractsGridView.GetRowCellDisplayText(e.RowHandle, ContractsGridView.Columns["USED_USER_ID"])) >= 0)
                GlobalProcedures.GridRowCellStyleForBlock(ContractsGridView, e);

            if (e.Column.FieldName == "ARRIVAL_DATE")
            {
                int warning = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "WARNING"));
                if (warning == 1)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.LimeGreen;
                else if (warning == 2)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.Red;
            }
        }

        private void LoadFObjectRegistrationAddEdit(string transaction, int? id)
        {
            topindex = ContractsGridView.TopRowIndex;
            old_row_num = ContractsGridView.FocusedRowHandle;
            FObjectRegistrationAddEdit fObjectRegistrationAddEdit = new FObjectRegistrationAddEdit
            {
                TransactionName = transaction,
                ID = id
            };
            fObjectRegistrationAddEdit.RefreshDataGridView += new FObjectRegistrationAddEdit.DoEvent(LoadContract);
            fObjectRegistrationAddEdit.ShowDialog();
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void NewBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFObjectRegistrationAddEdit("INSERT", null);
        }

        private void EditBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFObjectRegistrationAddEdit("EDIT", id);
        }

        private void BurdenUrlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (burdenURL.Length != 0)
                System.Diagnostics.Process.Start(burdenURL);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "mht");
        }

        private void MainUrlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (url.Length != 0)
                System.Diagnostics.Process.Start(url);
        }

        private void BurdenBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = ContractsGridView.TopRowIndex;
            old_row_num = ContractsGridView.FocusedRowHandle;
            FBurden fBurden = new FBurden
            {
                ContractCode = contractCode,
                ContractID = contractID,
                RegistrationID = id
            };
            fBurden.RefreshDataGridView += new FBurden.DoEvent(LoadContract);
            fBurden.ShowDialog();
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void ContractsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractsGridView, PopupMenu, e);
        }

        void Confirm()
        {
            topindex = ContractsGridView.TopRowIndex;
            old_row_num = ContractsGridView.FocusedRowHandle;
            FConfirm fConfirm = new FConfirm
            {
                ID = id,
                ContractCode = contractCode
            };
            fConfirm.RefreshDataGridView += new FConfirm.DoEvent(LoadContract);
            fConfirm.ShowDialog();
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void ConfirmBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Confirm();
        }
    }
}