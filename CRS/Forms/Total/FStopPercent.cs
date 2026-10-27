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

namespace CRS.Forms.Total
{
    public partial class FStopPercent : DevExpress.XtraEditors.XtraForm
    {
        public FStopPercent()
        {
            InitializeComponent();
        }
        public int ContractID;
        public string ContractCode;
        public DateTime LastPaymentDate;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int id, topindex, old_row_num;
        DataView dv = new DataView();
        DataTable dt = new DataTable();
        DateTime startDate;

        private void FStopPercent_Load(object sender, EventArgs e)
        {
            this.Text = ContractCode + " saylı müqavilənin faizinin dayandırılması";
            LoadDetail();
        }

        private void LoadDetail()
        {
            string sql = $@"SELECT ID,START_DATE,END_DATE,INSERT_DATE FROM CRS_USER.CONTRACT_PERCENT_DATE WHERE CONTRACT_ID = {ContractID}";

            dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetail", "Tarixlər açılmadı.");
            DateGridControl.DataSource = dt;

            dv = new DataView(dt);

            EditBarButton.Enabled = DeleteBarButton.Enabled = DateGridView.RowCount > 0;
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadDetail();
        }

        private void DateGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(DateGridView, PopupMenu, e);
        }

        private void DateGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Date_SS, e);
        }

        private void FStopPercent_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void DateGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = DateGridView.GetFocusedDataRow();
            if (row != null)
            {
                id = Convert.ToInt32(row["ID"]);
                startDate = DateTime.Parse(row["START_DATE"].ToString());
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFStopPercentDateAddEdit("EDIT", id);
        }

        private void RefreshBarButton_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadDetail();
        }

        private void DateGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFStopPercentDateAddEdit("EDIT", id);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dv.RowFilter = $@"ID > {id}";

            object countObject = dt.Compute("Count(ID)", dv.RowFilter);

            if (Convert.ToInt32(countObject) > 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş tarix intervalından sonra basqa faiz dayandırma intervalları olduğu üçün bu intervalı silmək olmaz.");
                return;
            }

            if (LastPaymentDate > startDate)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş tarix intervalından sonra ödəniş olduğu üçün bu intervalı silmək olmaz.");
                return;
            }

            topindex = DateGridView.TopRowIndex;
            old_row_num = DateGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş tarix intervalını silmək istəyirsiniz?", "Tarix intervalının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CONTRACT_PERCENT_DATE WHERE ID = {id} AND CONTRACT_ID = {ContractID}", "Tarix intervalı silinmədi.", this.Name + "/DeleteBarButton_ItemClick");
            LoadDetail();
            DateGridView.TopRowIndex = topindex;
            DateGridView.FocusedRowHandle = old_row_num;
        }

        private void LoadFStopPercentDateAddEdit(string transaction, int? id)
        {
            topindex = DateGridView.TopRowIndex;
            old_row_num = DateGridView.FocusedRowHandle;
            FStopPercentDateAddEdit fs = new FStopPercentDateAddEdit();
            fs.TransactionName = transaction;
            fs.ContractID = ContractID;
            fs.ID = id;
            fs.LastPaymentDate = LastPaymentDate;
            fs.RefreshDataGridView += new FStopPercentDateAddEdit.DoEvent(LoadDetail);
            fs.ShowDialog();
            DateGridView.TopRowIndex = topindex;
            DateGridView.FocusedRowHandle = old_row_num;
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFStopPercentDateAddEdit("INSERT", null);
        }
    }
}