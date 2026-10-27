using CRS.Class;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CRS.Forms.AML
{
    public partial class FProfessionAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FProfessionAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int? ID;
        bool CurrentStatus = false, Used = false;
        int UsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FProfessionAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PROFESSION", GlobalVariables.V_UserID, "WHERE ID = " + ID + " AND USED_USER_ID = -1");
                LoadDetails();
                Used = UsedUserID > 0;

                if (Used)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        GlobalProcedures.ShowWarningMessage("Seçilmiş peşə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş peşənin hal-hazırkı statusu");
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
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.PROFESSION WHERE ID = {ID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDetails", "Peşənin məlumatları açılmadı.");
            if (dt.Rows.Count > 0)
            {
                NameText.EditValue = dt.Rows[0]["NAME"];
                NoteText.EditValue = dt.Rows[0]["NOTE"];
                UsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void FProfessionAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PROFESSION", -1, "WHERE ID = " + ID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void InsertDetail()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.PROFESSION(NAME,NOTE,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{GlobalVariables.V_UserID})",
                                                "Peşə daxil edilmədi.",
                                            this.Name + "/InsertDetail");
        }

        private void UpdateDetail()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.PROFESSION SET NAME = '{NameText.Text.Trim()}', NOTE = '{NoteText.Text.Trim()}', UPDATE_DATE = SYSDATE, UPDATE_USER = {GlobalVariables.V_UserID} WHERE ID = {ID}",
                                                "Peşə dəyişdirilmədi.",
                                                this.Name + "/UpdateDetail");
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
                GlobalProcedures.ShowErrorMessage("Peşənin adı daxil edilməyib.");
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