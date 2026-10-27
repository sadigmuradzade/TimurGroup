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
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace CRS.Forms.Contracts
{
    public partial class FMediation : DevExpress.XtraEditors.XtraForm
    {
        public FMediation()
        {
            InitializeComponent();
        }
        int mediationID, topindex, old_row_id, mediationStatusID;

        private void FMediation_Load(object sender, EventArgs e)
        {
            NewBarButton.Enabled = GlobalVariables.AddLawDetail;
            LoadMediation();
        }

        private void LoadMediation()
        {
            string sql = $@"SELECT C.CONTRACT_CODE,
                                     M.ID,
                                     M.DEFENDANT,
                                     M.SEND_DATE,
                                     M.DATE_OF_APPLICATION,
                                     M.MEDIATOR,
                                     SESSION_DATE,
                                     MR.NAME RESULT,
                                     M.FIRST_PAYMENT_DATE,
                                     M.REPRESENTATIVE,
                                     M.NOTE,
                                     M.USED_USER_ID,
                                     MS.NAME STATUS_NAME,
                                     M.MEDIATION_STATUS_ID
                                FROM CRS_USER.MEDIATION M, CRS_USER.MEDIATION_RESULT MR, CRS_USER.V_CONTRACTS C, CRS_USER.MEDIATION_STATUS MS
                               WHERE M.RESULT_ID = MR.ID(+)
                                 AND M.CONTRACT_ID = C.CONTRACT_ID
                                 AND M.MEDIATION_STATUS_ID = MS.ID
                            ORDER BY M.ID";

            MediationGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadMediation", "Mediasiyaların siyahısı açılmadı.");

            //EnabledButtons();
        }

        //void EnabledButtons()
        //{
        //    if (MediationGridView.RowCount > 0)
        //    {
        //        StopBarButton.Enabled = GlobalVariables.CloseProsecute && mediationStatusID == 1;
        //        EditBarButton.Enabled = DeleteBarButton.Enabled = mediationStatusID == 1;
        //    }
        //    //EditBarButton.Enabled = DeleteBarButton.Enabled = StopBarButton.Enabled = ;
        //}

        private void MediationGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(MediationGridView, PopupMenu, e);
        }

        private void MediationGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Mediation_SS, e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //GlobalProcedures.ShowGridPreview(MediationGridControl);

            string path = GlobalVariables.V_ExecutingFolder + "\\Documents\\Mediasiya çapı.docx";
           
            if (!File.Exists(path))
            {
                GlobalProcedures.ShowErrorMessage("Ödəniş qrafikinin şablon faylı tapılmadı.");
                return;
            }

            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Mediasiya.docx";

            try
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
               
                object missing = Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object fileName = Path.Combine(path),
                       saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                aDoc.Activate();

                string sqlData = $@"SELECT C.CONTRACT_CODE,
                                     M.DEFENDANT,
                                     TO_CHAR(M.SEND_DATE,'DD.MM.YYYY') SEND_DATE,
                                     TO_CHAR(M.DATE_OF_APPLICATION, 'DD.MM.YYYY') DATE_OF_APPLICATION,
                                     M.MEDIATOR,
                                     TO_CHAR(SESSION_DATE, 'DD.MM.YYYY') SESSION_DATE,
                                     MR.NAME RESULT,
                                     TO_CHAR(M.FIRST_PAYMENT_DATE, 'DD.MM.YYYY') FIRST_PAYMENT_DATE,
                                     M.REPRESENTATIVE,
                                     M.NOTE,
                                     MS.NAME STATUS_NAME
                                FROM CRS_USER.MEDIATION M, CRS_USER.MEDIATION_RESULT MR, CRS_USER.V_CONTRACTS C, CRS_USER.MEDIATION_STATUS MS
                               WHERE M.RESULT_ID = MR.ID(+)
                                 AND M.CONTRACT_ID = C.CONTRACT_ID
                                 AND M.MEDIATION_STATUS_ID = MS.ID
                            ORDER BY M.ID";
                DataTable dt = GlobalFunctions.GenerateDataTable(sqlData, this.Name + "/BDocument_Click", "Ödəniş qrafiki açılmadı.");

                
                int j = 2, r = 1;
                Microsoft.Office.Interop.Word.Table table = aDoc.Tables[1];

                foreach (DataRow dr in dt.Rows)
                {
                    table.Rows.Add(table.Rows[j]);

                    table.Cell(j, 1).Range.Text = r.ToString();
                    table.Cell(j, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(j, 2).Range.Text = dr["CONTRACT_CODE"].ToString();
                    table.Cell(j, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(j, 3).Range.Text = dr["DEFENDANT"].ToString();
                    table.Cell(j, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    table.Cell(j, 4).Range.Text = dr["SEND_DATE"].ToString();
                    table.Cell(j, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(j, 5).Range.Text = dr["DATE_OF_APPLICATION"].ToString();
                    table.Cell(j, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(j, 6).Range.Text = dr["MEDIATOR"].ToString();
                    table.Cell(j, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    table.Cell(j, 7).Range.Text = dr["SESSION_DATE"].ToString();
                    table.Cell(j, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(j, 8).Range.Text = dr["RESULT"].ToString();
                    table.Cell(j, 8).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    table.Cell(j, 9).Range.Text = dr["REPRESENTATIVE"].ToString();
                    table.Cell(j, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    table.Cell(j, 10).Range.Text = dr["FIRST_PAYMENT_DATE"].ToString();
                    table.Cell(j, 10).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(j, 11).Range.Text = dr["NOTE"].ToString();
                    table.Cell(j, 11).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    table.Cell(j, 12).Range.Text = dr["STATUS_NAME"].ToString();
                    table.Cell(j, 12).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    j++;
                    r++;
                }

                table.Rows[j].Delete();

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                Process.Start(filePath);
                GlobalProcedures.SplashScreenClose();
            }
            catch (Exception exx)
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage("Mediasiya.docx faylı yaradılmadı.", exx);
            }
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(MediationGridControl, "xls");
        }

        private void MediationGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = MediationGridView.GetFocusedDataRow();
            if (row != null)
            {
                mediationID = Convert.ToInt32(row["ID"]);
                mediationStatusID = Convert.ToInt32(row["MEDIATION_STATUS_ID"]);

                StopBarButton.Enabled = GlobalVariables.CloseProsecute && mediationStatusID == 1;
                EditBarButton.Enabled = DeleteBarButton.Enabled = mediationStatusID == 1;
                ActiveBarButton.Enabled = mediationStatusID != 1;
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMediationAddEdit("EDIT", mediationID);
        }

        private void MediationGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFMediationAddEdit("EDIT", mediationID);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadMediation();
        }

        private void MediationGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            //EnabledButtons();
        }

        void Delete()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.MEDIATION WHERE ID = {mediationID}");
            if (UsedUserID == -1 || GlobalVariables.V_UserID == UsedUserID)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş mediasiyanı silmək istəyirsiniz?", "Mediasiyanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.MEDIATION WHERE ID = {mediationID}", "Mediasiya bazadan silinmədi.", this.Name + "/Delete");
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Silmək istədiyiniz mediasiya " + used_user_name + " tərəfindən istifadə edilir. Bu mediasiyanı silmək olmaz.", "Seçilmiş mediasiyanln hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Delete();
            LoadMediation();
        }

        private void MediationGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (e.RowHandle >= 0 && int.Parse(MediationGridView.GetRowCellDisplayText(e.RowHandle, MediationGridView.Columns["USED_USER_ID"])) < 0
                     && int.Parse(MediationGridView.GetRowCellDisplayText(e.RowHandle, MediationGridView.Columns["MEDIATION_STATUS_ID"])) != 1)
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor2);
            }
            else if (int.Parse(MediationGridView.GetRowCellDisplayText(e.RowHandle, MediationGridView.Columns["USED_USER_ID"])) >= 0)
                GlobalProcedures.GridRowCellStyleForBlock(MediationGridView, e);
        }

        private void ActiveBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = MediationGridView.TopRowIndex;
            old_row_id = MediationGridView.FocusedRowHandle;
            if (mediationStatusID == 3)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş mediasiya üçün məhkəmə iclası yaradıldığından bu mediasiyanı aktiv etmək olmaz. Mediasiyanı aktiv etmək üçün əvvəlcə məhkəməni silin.");
                return;
            }

            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş mediasiyanı yenidən aktiv etmək istəyirsiniz?", "Mediasiyanın yenidən aktiv edilməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.MEDIATION SET MEDIATION_STATUS_ID = 1 WHERE ID = {mediationID}", "Seçilmiş mediasiya yenidən aktiv edilmədi.", this.Name + "/ActiveBarButton_ItemClick");
            LoadMediation();
            MediationGridView.TopRowIndex = topindex;
            MediationGridView.FocusedRowHandle = old_row_id;
        }

        void StopMediation()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.MEDIATION WHERE ID = {mediationID}");
            if (UsedUserID == -1 || GlobalVariables.V_UserID == UsedUserID)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş mediasiyanı bitirmək istəyirsiniz?", "Mediasiyanın bitirilməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    topindex = MediationGridView.TopRowIndex;
                    old_row_id = MediationGridView.FocusedRowHandle;
                    FStopMediation fm = new FStopMediation();
                    fm.MediationID = mediationID;
                    fm.RefreshDataGridView += new FStopMediation.DoEvent(LoadMediation);
                    fm.ShowDialog();
                    MediationGridView.TopRowIndex = topindex;
                    MediationGridView.FocusedRowHandle = old_row_id;
                }
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Bitirmək istədiyiniz mediasiya " + used_user_name + " tərəfindən istifadə edilir. Buna görə də bu mediasiyanı bitirmək olmaz.", "Seçilmiş mediasiyanln hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MediationGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void StopBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            StopMediation();
        }

        private void LoadFMediationAddEdit(string transaction, int? id)
        {
            topindex = MediationGridView.TopRowIndex;
            old_row_id = MediationGridView.FocusedRowHandle;
            FMediationAddEdit fm = new FMediationAddEdit();
            fm.TransactionName = transaction;
            fm.ID = id;
            fm.RefreshDataGridView += new FMediationAddEdit.DoEvent(LoadMediation);
            fm.ShowDialog();
            MediationGridView.TopRowIndex = topindex;
            MediationGridView.FocusedRowHandle = old_row_id;
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMediationAddEdit("INSERT", null);
        }
    }
}