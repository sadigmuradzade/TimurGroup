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

namespace CRS.Forms.ObjectRegistration
{
    public partial class FConfirm : DevExpress.XtraEditors.XtraForm
    {
        public FConfirm()
        {
            InitializeComponent();
        }
        public int ID;
        public string ContractCode;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int UsedUserID = -1;
        bool Used = false, CurrentStatus = false;

        private void FConfirm_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OBJECT_REGISTRATION", GlobalVariables.V_UserID, "WHERE ID = " + ID + " AND USED_USER_ID = -1");
            LoadDetail();
            ContractCodeText.EditValue = ContractCode;
            Used = UsedUserID >= 0;
            if (Used)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    GlobalProcedures.ShowWarningMessage("Seçilmiş məlumatlar hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.");
                    CurrentStatus = true;
                }
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;
            ComponentEnable(CurrentStatus);
        }

        private void ComponentEnable(bool status)
        {
            CodeText.Enabled =
                GivenDate.Enabled =
                URLText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadDetail()
        {
            string sql = $@"SELECT REGISTRATION_NUMBER,REGISTRATION_DATE,URL,USED_USER_ID FROM CRS_USER.OBJECT_REGISTRATION WHERE ID = {ID}";

            DataTable dataTable = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetail", "Detallar açılmadı");

            if (dataTable.Rows.Count > 0)
            {
                CodeText.EditValue = dataTable.Rows[0]["REGISTRATION_NUMBER"];
                GivenDate.EditValue = dataTable.Rows[0]["REGISTRATION_DATE"];
                URLText.EditValue = dataTable.Rows[0]["URL"];
                UsedUserID = Convert.ToInt16(dataTable.Rows[0]["USED_USER_ID"]);
            }
            else
                GivenDate.EditValue = DateTime.Today;
        }

        private void FConfirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OBJECT_REGISTRATION", -1, "WHERE ID = " + ID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private bool ControlDetail()
        {
            bool b = false;

            if (CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Çıxarışın nömrəsi daxil edilməyib.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GivenDate.Text.Length == 0)
            {
                GivenDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Çıxarışın verilmə tarixi daxil edilməyib.");
                GivenDate.Focus();
                GivenDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (URLText.Text.Length == 0)
            {
                URLText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Çıxarışın linki daxil edilməyib.");
                URLText.Focus();
                URLText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetail())
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.OBJECT_REGISTRATION SET REGISTRATION_NUMBER = '{CodeText.Text.Trim()}', REGISTRATION_DATE = TO_DATE('{GivenDate.Text.Trim()}','DD.MM.YYYY'), URL = '{URLText.Text.Trim()}', UPDATE_USER = {GlobalVariables.V_UserID}, UPDATE_DATE = SYSDATE WHERE ID = {ID}", "Çıxarış təsdiq edilmədi", this.Name + "/BOK_Click");
                this.Close();
            }
        }

        private void URLText_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (URLText.Text.Length != 0)
                    System.Diagnostics.Process.Start(URLText.Text);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Link açılmadı.", URLText.Text, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }
    }
}