using CRS.Class;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CRS.Forms.AML
{
    public partial class FSocialStatusAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FSocialStatusAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int? ID;
        bool CurrentStatus = false, Used = false;
        int UsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FSocialStatusAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.SOCIAL_STATUS", GlobalVariables.V_UserID, "WHERE ID = " + ID + " AND USED_USER_ID = -1");
                LoadDetails();
                Used = UsedUserID > 0;

                if (Used)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        GlobalProcedures.ShowWarningMessage("Seçilmiş sosial status hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş statusun hal-hazırkı statusu");
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
            string s = $@"SELECT NAME,NOTE,IS_WORK_PLACE,USED_USER_ID FROM CRS_USER.SOCIAL_STATUS WHERE ID = {ID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDetails", "Sosial statusun məlumatları açılmadı.");
            if (dt.Rows.Count > 0)
            {
                NameText.EditValue = dt.Rows[0]["NAME"];
                NoteText.EditValue = dt.Rows[0]["NOTE"];
                UsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
                IsWorkPlaceCheck.Checked = Convert.ToInt32(dt.Rows[0]["IS_WORK_PLACE"]) == 1 ? true : false;
            }
        }

        private void InsertDetail()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.SOCIAL_STATUS(NAME,NOTE,IS_WORK_PLACE,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{(IsWorkPlaceCheck.Checked ? 1 : 0)},{GlobalVariables.V_UserID})",
                                                "Sosial status daxil edilmədi.",
                                            this.Name + "/InsertDetail");
        }

        private void UpdateDetail()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.SOCIAL_STATUS SET NAME = '{NameText.Text.Trim()}', NOTE = '{NoteText.Text.Trim()}', IS_WORK_PLACE = {(IsWorkPlaceCheck.Checked ? 1 : 0)}, UPDATE_DATE = SYSDATE, UPDATE_USER = {GlobalVariables.V_UserID} WHERE ID = {ID}",
                                                "Sosial status dəyişdirilmədi.",
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

        private void FSocialStatusAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.SOCIAL_STATUS", -1, "WHERE ID = " + ID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
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
                GlobalProcedures.ShowErrorMessage("Sosial statusun adı daxil edilməyib.");
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