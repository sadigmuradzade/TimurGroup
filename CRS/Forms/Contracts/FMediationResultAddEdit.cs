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
    public partial class FMediationResultAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FMediationResultAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ResultID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int UsedUserID = -1;

        private void FMediationResultAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
                LoadDetails();
        }

        private void LoadDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.MEDIATION_RESULT WHERE ID = {ResultID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDetails", "Nəticə açılmadı.");

            if (dt.Rows.Count > 0)
            {
                NameText.EditValue = dt.Rows[0]["NAME"];
                NoteText.EditValue = dt.Rows[0]["NOTE"];
                UsedUserID = Convert.ToInt16(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void FMediationResultAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void Insert()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.MEDIATION_RESULT(NAME,NOTE,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{GlobalVariables.V_UserID})",
                                                "Mediasiya nəticəsi bazaya daxil edilmədi.");
        }

        private void Update()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.MEDIATION_RESULT SET NAME = '{NameText.Text.Trim()}',NOTE = '{NoteText.Text.Trim()}',UPDATE_USER = {GlobalVariables.V_UserID} WHERE ID = {ResultID}",
                                                "Mediasiya nəticəsi bazada dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                XtraMessageBox.Show("Nəticənin adı daxil edilməyib.");
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