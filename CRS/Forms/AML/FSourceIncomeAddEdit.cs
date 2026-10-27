using CRS.Class;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CRS.Forms.AML
{
    public partial class FSourceIncomeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FSourceIncomeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int? ID;
        bool CurrentStatus = false, Used = false;
        int UsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FSourceIncomeAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.SOURCE_INCOME", GlobalVariables.V_UserID, "WHERE ID = " + ID + " AND USED_USER_ID = -1");
                LoadDetails();
                Used = UsedUserID > 0;

                if (Used)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        GlobalProcedures.ShowWarningMessage("Seçilmiş gəlir mənbəyi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş mənbənin hal-hazırkı statusu");
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
            }
        }

        private void LoadDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.SOURCE_INCOME WHERE ID = {ID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDetails", "Gəlir mənbənin məlumatları açılmadı.");
            if (dt.Rows.Count > 0)
            {
                NameText.EditValue = dt.Rows[0]["NAME"];
                NoteText.EditValue = dt.Rows[0]["NOTE"];
                UsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void InsertDetail()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.SOURCE_INCOME(NAME,NOTE,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{GlobalVariables.V_UserID})",
                                                "Gəlir mənbəyi daxil edilmədi.",
                                            this.Name + "/InsertDetail");
        }

        private void UpdateDetail()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.SOURCE_INCOME SET NAME = '{NameText.Text.Trim()}', NOTE = '{NoteText.Text.Trim()}', UPDATE_DATE = SYSDATE, UPDATE_USER = {GlobalVariables.V_UserID} WHERE ID = {ID}",
                                                "Gəlir mənbəyi dəyişdirilmədi.",
                                                this.Name + "/UpdateDetail");
        }

        private void FSourceIncomeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.SOURCE_INCOME", -1, "WHERE ID = " + ID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetail())
            {
                if (TransactionName == "INSERT")
                    InsertDetail();
                else
                    UpdateDetail();
                this.Close();
            }
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private bool ControlDetail()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Gəlir mənbəyinin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }
    }
}