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
    public partial class FBurden : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FBurden()
        {
            InitializeComponent();
        }
        public string ContractCode;
        public int ContractID, RegistrationID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int topindex, old_row_num, id;
        string url;

        private void FObjectRegistration_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OBJECT_REGISTRATION", GlobalVariables.V_UserID, "WHERE ID = " + RegistrationID + " AND USED_USER_ID = -1");
            this.Text = ContractCode + " saylı müqavilənin yüklülükləri";
        }

        private void LoadContract()
        {
            string sql = $@"SELECT OB.ID,
                                     OB.FULLNAME,
                                     OB.SENT_DATE,
                                     OB.ARRIVAL_DATE,
                                     OB.REGISTRATION_NUMBER,
                                     OB.REGISTRATION_DATE,
                                     OB.URL,
                                     OB.NOTE,
                                     (SELECT USER_FULLNAME
                                        FROM CRS_USER.V_USERS
                                       WHERE ID = UPDATE_USER)
                                        LAST_UPDATE_USER,
                                     UPDATE_DATE,
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
                                        WARNING
                                FROM CRS_USER.OBJECT_BURDEN_REGISTRATION OB
                               WHERE OB.OBJECT_REGISTRATION_ID = {RegistrationID}
                            ORDER BY OB.ID DESC";

            ContractsGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContract", "Yüklülərin siyahısı açılmadı.");

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
                url = row["URL"].ToString();
                ViewUrlBarButton.Enabled = url.Length > 0;
            }
        }

        private void ViewUrlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (url.Length != 0)
                System.Diagnostics.Process.Start(url);
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ContractsGridControl);
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = ContractsGridView.FocusedRowHandle;
            topindex = ContractsGridView.TopRowIndex;
            if (GlobalFunctions.CallDialogResult("Seçilmiş yüklülüyü silmək istəyirsiniz?", "Yüklülüyün silinməsi") == DialogResult.Yes)
                GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.OBJECT_BURDEN_REGISTRATION WHERE ID = {id}", "Yüklülük silinmədi", this.Name + "/DeleteBarButton_ItemClick");
            LoadContract();
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void ContractsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;            

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
            FBurdenAddEdit fObjectRegistrationAddEdit = new FBurdenAddEdit
            {
                TransactionName = transaction,
                ID = id,
                RegistrationID = RegistrationID,
                ContractID = ContractID
            };
            fObjectRegistrationAddEdit.RefreshDataGridView += new FBurdenAddEdit.DoEvent(LoadContract);
            fObjectRegistrationAddEdit.ShowDialog();
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void NewBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFObjectRegistrationAddEdit("INSERT", null);
        }

        private void FBurden_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OBJECT_REGISTRATION", -1, "WHERE ID = " + RegistrationID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void EditBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFObjectRegistrationAddEdit("EDIT", id);
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

        private void ContractsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractsGridView, PopupMenu, e);
        }

        void Confirm()
        {
            topindex = ContractsGridView.TopRowIndex;
            old_row_num = ContractsGridView.FocusedRowHandle;
            FConfirmBurden fConfirm = new FConfirmBurden
            {
                ID = id,
                ContractCode = ContractCode
            };
            fConfirm.RefreshDataGridView += new FConfirmBurden.DoEvent(LoadContract);
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