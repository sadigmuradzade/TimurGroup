using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms.Contracts
{
    public partial class FProsecute : DevExpress.XtraEditors.XtraForm
    {
        public FProsecute()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID, CustomerName, DetailID, ContractStartDate;

        public delegate void DoEvent();
        public event DoEvent RefreshDetailsDataGridView;

        int law_id, law_status_id, expenseID, topindex, old_row_num;

        private void FProsecute_Load(object sender, EventArgs e)
        {
            GlobalProcedures.DateEditFormat(StartDate);
            StartDate.EditValue = DateTime.Today;
            StartDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(ContractStartDate, "ddmmyyyy");
            GlobalProcedures.FillComboBoxEdit(LawComboBox, "LAWS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
            GlobalProcedures.FillComboBoxEdit(StatusComboBox, "LAW_STATUS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");

            if (TransactionName == "EDIT")
            {
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_LAW_EXPENSE_TEMP", "P_CONTRACT_ID", ContractID, "Məhkəmə xərcləri temp cədvələ daxil edilmədi.");
                LoadProsecuteDetails();                
            }
            else
            {
                DetailID = GlobalFunctions.GetOracleSequenceValue("CONTRACT_LAWS_SEQUENCE").ToString();
                DefandantNameText.Text = CustomerName;
                ApplicantText.Text = GlobalFunctions.ReadSetting("Company");
            }

            LoadExpense();
        }

        private void LoadProsecuteDetails()
        {
            string s = "SELECT START_DATE,L.NAME LAW_NAME,CL.JUDGE_NAME,CL.REPRESENTATIVE,LS.NAME STATUS_NAME,CL.NOTE,CL.DEFANDANT_NAME,CL.APPLICANT,CL.WORK_CODE,CL.RELATED_WORK_CODE,CL.RELATED_WORK_LAW_NAME FROM CRS_USER.CONTRACT_LAWS CL,CRS_USER.LAWS L,CRS_USER.LAW_STATUS LS WHERE CL.LAW_ID = L.ID AND CL.LAW_STATUS_ID = LS.ID AND CL.ID = " + DetailID;

            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadProsecuteDetails", "Məhkəmə iclasının məlumatları tapılmadı.").Rows)
            {
                StartDate.EditValue = dr["START_DATE"];
                LawComboBox.EditValue = dr["LAW_NAME"].ToString();
                JudgeNameText.EditValue = dr["JUDGE_NAME"];
                RepresentativeText.EditValue = dr["REPRESENTATIVE"];
                StatusComboBox.EditValue = dr["STATUS_NAME"];
                NoteText.EditValue = dr["NOTE"];
                DefandantNameText.EditValue = dr["DEFANDANT_NAME"];
                ApplicantText.EditValue = dr["APPLICANT"];
                WorkCodeText.EditValue = dr["WORK_CODE"];
                RelatedWorkCodeText.EditValue = dr["RELATED_WORK_CODE"];
                RelatedWorkLawNameText.EditValue = dr["RELATED_WORK_LAW_NAME"];
            }
        }

        private void FProsecute_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDetailsDataGridView();
        }

        void RefreshLaws()
        {
            GlobalProcedures.FillComboBoxEdit(LawComboBox, "LAWS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
        }

        void RefreshLawStatus()
        {
            GlobalProcedures.FillComboBoxEdit(StatusComboBox, "LAW_STATUS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
        }

        private void LawComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FLaws fl = new FLaws();
                fl.RefreshLawsData += new FLaws.DoEvent(RefreshLaws);
                fl.ShowDialog();
            }
        }

        private void StatusComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FLawStatus fls = new FLawStatus();
                fls.RefreshLawStatusData += new FLawStatus.DoEvent(RefreshLawStatus);
                fls.ShowDialog();
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            law_status_id = GlobalFunctions.FindComboBoxSelectedValue("LAW_STATUS", "NAME", "ID", StatusComboBox.Text);
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        private void BRepeat_Click(object sender, EventArgs e)
        {
            string a = ApplicantText.Text, b = DefandantNameText.Text;

            ApplicantText.Text = b;
            DefandantNameText.Text = a;
        }

        private void ExpenseGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ExpenseGridView, PopupMenu, e);
        }

        private void ExpenseGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Expense_SS, e);
        }

        private void LoadExpense()
        {
            string sql = $@"SELECT CL.ID,
                                   LE.NAME,
                                   CL.AMOUNT,
                                   CL.NOTE
                              FROM CRS_USER_TEMP.CONTRACT_LAW_EXPENSE_TEMP CL, CRS_USER.LAW_EXPENSE LE
                             WHERE CL.IS_CHANGE != 2 AND CL.LAW_EXPENSE_ID = LE.ID AND CL.CONTRACT_ID = {ContractID} AND CL.CONTRACT_LAW_ID = {DetailID}";

            ExpenseGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadExpense", "Məhkəmə xərcləri açılmadı.");

            DeleteBarButton.Enabled = EditBarButton.Enabled = ExpenseGridView.RowCount > 0;
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadExpense();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ExpenseGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ExpenseGridControl, "xls");
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFContractExpenseAddEdit("EDIT", expenseID);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş məhkəmə xərcini silmək istəyirsiniz?", "Məhkəmə xərcinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CONTRACT_LAW_EXPENSE_TEMP SET IS_CHANGE = 2 WHERE CONTRACT_ID = {ContractID} AND ID = {expenseID}",
                                                    "Məhkəmə xərci silinmədi.",
                                                  this.Name + "/DeleteBarButton_ItemClick");
                LoadExpense();
            }
        }

        private void ExpenseGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFContractExpenseAddEdit("EDIT", expenseID);
        }

        private void ExpenseGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Expense_SS, "Center", e);
        }

        private void RelatedWorkCodeText_EditValueChanged(object sender, EventArgs e)
        {
            string sql = $@"SELECT L.NAME FROM CRS_USER.CONTRACT_LAWS CL,CRS_USER.LAWS L WHERE CL.LAW_ID = L.ID AND CL.WORK_CODE = '{RelatedWorkCodeText.Text.Trim()}'";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/RelatedWorkCodeText_EditValueChanged", "Əlaqəli işin məhkəməsi tapılmadı.");

            if (dt.Rows.Count > 0)
                RelatedWorkLawNameText.EditValue = dt.Rows[0]["NAME"];
            else
                RelatedWorkLawNameText.EditValue = null;
        }

        private void LoadFContractExpenseAddEdit(string transaction, int? id)
        {
            topindex = ExpenseGridView.TopRowIndex;
            old_row_num = ExpenseGridView.FocusedRowHandle;
            FContractLawExpenseAddEdit fc = new FContractLawExpenseAddEdit();
            fc.TransactionName = transaction;
            fc.ContractID = ContractID;
            fc.ID = id;
            fc.ContractLawID = int.Parse(DetailID);
            fc.RefreshDataGridView += new FContractLawExpenseAddEdit.DoEvent(LoadExpense);
            fc.ShowDialog();
            ExpenseGridView.TopRowIndex = topindex;
            ExpenseGridView.FocusedRowHandle = old_row_num;
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFContractExpenseAddEdit("INSERT", null);
        }

        private void ExpenseGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ExpenseGridView.GetFocusedDataRow();
            if (row != null)
                expenseID = Convert.ToInt32(row["ID"]);
        }

        private void LawComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            law_id = GlobalFunctions.FindComboBoxSelectedValue("LAWS", "NAME", "ID", LawComboBox.Text);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlProsecute())
            {
                if (TransactionName == "INSERT")
                    InsertProsecute();
                else
                    UpdateProsecute();
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_CON_LAW_EXPENSE", "P_CONTRACT_ID", ContractID, "Məhkəmə xərcləri əsas cədvələ daxil edilmədi.");
                this.Close();
            }
        }

        private bool ControlProsecute()
        {
            bool b = false;

            if (String.IsNullOrWhiteSpace(StartDate.Text))
            {
                StartDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məhkəməyə verilmə tarixi daxil edilməyib.");
                StartDate.Focus();
                StartDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (law_id <= 0)
            {
                LawComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məhkəmənin adı daxil edilməyib.");
                LawComboBox.Focus();
                LawComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (law_status_id <= 0)
            {
                StatusComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İclasın statusu daxil edilməyib.");
                StatusComboBox.Focus();
                StatusComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrWhiteSpace(ApplicantText.Text))
            {
                ApplicantText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İddiaçı daxil edilməyib.");
                ApplicantText.Focus();
                ApplicantText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrWhiteSpace(DefandantNameText.Text))
            {
                DefandantNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cavabdeh şəxs daxil edilməyib.");
                DefandantNameText.Focus();
                DefandantNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertProsecute()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CONTRACT_LAWS(ID,
                                                                                CONTRACT_ID,
                                                                                DEFANDANT_NAME,
                                                                                LAW_ID,
                                                                                JUDGE_NAME,
                                                                                REPRESENTATIVE,
                                                                                LAW_STATUS_ID,
                                                                                START_DATE,
                                                                                NOTE,
                                                                                CREATED_USER_NAME,
                                                                                APPLICANT,
                                                                                WORK_CODE,
                                                                                RELATED_WORK_CODE,
                                                                                RELATED_WORK_LAW_NAME)
                                            VALUES({DetailID},
                                                    {ContractID},
                                                    '{DefandantNameText.Text.Trim()}',
                                                    {law_id},
                                                    '{JudgeNameText.Text.Trim()}',
                                                    '{RepresentativeText.Text.Trim()}',
                                                    {law_status_id},
                                                    TO_DATE('{StartDate.Text}','DD.MM.YYYY'),
                                                    '{NoteText.Text.Trim()}',
                                                    '{GlobalVariables.V_UserName}',
                                                    '{ApplicantText.Text.Trim()}',
                                                    '{WorkCodeText.Text.Trim()}',
                                                    '{RelatedWorkCodeText.Text.Trim()}',
                                                    '{RelatedWorkLawNameText.Text.Trim()}')",
                                                 "Məhkəməyə verilmə daxil edilmədi.",
                                             this.Name + "/InsertProsecute");
        }

        private void UpdateProsecute()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_LAWS SET DEFANDANT_NAME='{DefandantNameText.Text.Trim()}',
                                                                                LAW_ID={law_id},
                                                                                JUDGE_NAME='{JudgeNameText.Text.Trim()}',
                                                                                REPRESENTATIVE='{RepresentativeText.Text.Trim()}',
                                                                                LAW_STATUS_ID={law_status_id},
                                                                                START_DATE=TO_DATE('{StartDate.Text}','DD.MM.YYYY'),
                                                                                NOTE ='{NoteText.Text.Trim()}',
                                                                                APPLICANT='{ApplicantText.Text.Trim()}',
                                                                                WORK_CODE='{WorkCodeText.Text.Trim()}',
                                                                                RELATED_WORK_CODE='{RelatedWorkCodeText.Text.Trim()}',
                                                                                RELATED_WORK_LAW_NAME='{RelatedWorkLawNameText.Text.Trim()}'
                                            WHERE ID = {DetailID}",
                                                "Məhkəməyə verilmə dəyişdirilmədi.",
                                             this.Name + "/UpdateProsecute");
        }
    }
}