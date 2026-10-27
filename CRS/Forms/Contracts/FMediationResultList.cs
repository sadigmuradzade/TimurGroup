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

namespace CRS.Forms.Contracts
{
    public partial class FMediationResultList : DevExpress.XtraEditors.XtraForm
    {
        public FMediationResultList()
        {
            InitializeComponent();
        }
        string resultID;
        int topindex, old_row_id;

        public delegate void DoEvent();
        public event DoEvent RefreshGridViewData;

        private void FMediationResultList_Load(object sender, EventArgs e)
        {
            LoadDetail();
        }

        private void LoadDetail()
        {
            string sql = $@"SELECT ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.MEDIATION_RESULT ORDER BY NAME";

            ResultGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDtail", "Mediasiya nəticələri açılmadı.");

            EnabledButton();
        }

        void EnabledButton()
        {
            EditBarButton.Enabled = DeleteBarButton.Enabled = ResultGridView.RowCount > 0;
        }

        private void ResultGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ResultGridView, PopupMenu, e);
        }

        private void ResultGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Result_SS, e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ResultGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ResultGridControl, "xls");
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadDetail();
        }

        private void ResultGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EnabledButton();
        }

        private void ResultGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ResultGridView.GetFocusedDataRow();
            if (row != null)            
                resultID = row["ID"].ToString();            
        }
        
        private void LoadFMeadiationResultAddEdit(string transaction, string id)
        {
            topindex = ResultGridView.TopRowIndex;
            old_row_id = ResultGridView.FocusedRowHandle;
            FMediationResultAddEdit flae = new FMediationResultAddEdit();
            flae.TransactionName = transaction;
            flae.ResultID = id;
            flae.RefreshDataGridView += new FMediationResultAddEdit.DoEvent(LoadDetail);
            flae.ShowDialog();
            ResultGridView.TopRowIndex = topindex;
            ResultGridView.FocusedRowHandle = old_row_id;
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMeadiationResultAddEdit("INSERT", null);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMeadiationResultAddEdit("EDIT", resultID);
        }

        private void FMediationResultList_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshGridViewData();
        }

        private void ResultGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditBarButton.Enabled)
                LoadFMeadiationResultAddEdit("EDIT", resultID);
        }
    }
}