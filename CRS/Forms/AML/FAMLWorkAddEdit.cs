using CRS.Class;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CRS.Forms.AML
{
    public partial class FAMLWorkAddEdit: DevExpress.XtraEditors.XtraForm
    {
        public FAMLWorkAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int? WorkID;
        public int CustomerID;

        public delegate void DoEvent();
        public event DoEvent RefresWorkPlaceDataGridView;

        private void FWorkAddEdit_Load(object sender, EventArgs e)
        {  
            if (TransactionName == "EDIT")
                LoadWorkDetails();
        }

        private void LoadWorkDetails()
        {
            string s = $@"SELECT PLACE_NAME,POSITION,START_DATE,END_DATE,ADDRESS,EMAIL,PHONE,NOTE FROM CRS_USER_TEMP.CUSTOMER_WORKPLACE_TEMP WHERE ID = {WorkID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadWorkDetails", "İş yerinin detalları tapılmadı.");
            if (dt.Rows.Count > 0)
            {
                OfficeText.EditValue = dt.Rows[0]["PLACE_NAME"];
                PositionText.EditValue = dt.Rows[0]["POSITION"];
                StartDate.EditValue = Convert.ToDateTime(dt.Rows[0]["START_DATE"]);
                EndDate.EditValue = Convert.ToDateTime(dt.Rows[0]["END_DATE"]);
                NoteText.EditValue = dt.Rows[0]["NOTE"];
                AddressText.EditValue = dt.Rows[0]["ADDRESS"];
                MailText.EditValue = dt.Rows[0]["EMAIL"];
                PhoneText.EditValue = dt.Rows[0]["PHONE"];
            }
        }

        private void InsertWork()
        {           
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_AML_WORK_TEMP(ID,
                                                                                               CUSTOMER_ID,
                                                                                               PLACE_NAME,
                                                                                               POSITION,
                                                                                               START_DATE,
                                                                                               END_DATE,
                                                                                               ADDRESS,
                                                                                               EMAIL,
                                                                                               PHONE,
                                                                                               NOTE,
                                                                                               USED_USER_ID,
                                                                                               IS_CHANGE) 
                                                            VALUES(CRS_USER.CUSTOMER_WORKPLACE_SEQUENCE.NEXTVAL,
                                                                    {CustomerID},
                                                                    '{OfficeText.Text.Trim()}',
                                                                    '{PositionText.Text.Trim()}',
                                                                    TO_DATE('{StartDate.Text}','DD/MM/YYYY'),
                                                                    TO_DATE('{EndDate.Text}','DD/MM/YYYY'),
                                                                    '{AddressText.Text.Trim()}',
                                                                    '{MailText.Text.Trim()}',
                                                                    '{PhoneText.Text.Trim()}',
                                                                    '{NoteText.Text.Trim()}',
                                                                    {GlobalVariables.V_UserID},
                                                                    1)", 
                                                        "İş yeri daxil edilmədi.");
        }

        private void UpdateWork()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CUSTOMER_AML_WORK_TEMP SET IS_CHANGE = 1, 
                                                                                                PLACE_NAME = '{OfficeText.Text.Trim()}',
                                                                                                POSITION = '{PositionText.Text.Trim()}',
                                                                                                START_DATE = TO_DATE('{StartDate.Text}','DD/MM/YYYY'),
                                                                                                END_DATE = TO_DATE('{EndDate.Text}','DD/MM/YYYY'),
                                                                                                ADDRESS = '{AddressText.Text.Trim()}',
                                                                                                EMAIL = '{MailText.Text.Trim()}',
                                                                                                PHONE = '{PhoneText.Text.Trim()}',
                                                                                                NOTE = '{NoteText.Text.Trim()}' 
                                                WHERE ID = {WorkID}", "İş yeri dəyişdirilmədi.");
        }

        private bool ControlWorkDetails()
        {
            bool b = false;

            if (OfficeText.Text.Length == 0)
            {
                OfficeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İş yerinin adı daxil edilməyib.");               
                OfficeText.Focus();
                OfficeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PositionText.Text.Length == 0)
            {
                PositionText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Vəzifə daxil edilməyib.");                
                PositionText.Focus();
                PositionText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (StartDate.Text.Length > 0 && EndDate.Text.Length > 0 && StartDate.DateTime == EndDate.DateTime)
            {
                StartDate.BackColor = Color.Red;
                EndDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Başlanğıc tarixi ilə son tarix eyni ola bilməz.");                
                StartDate.Focus();
                StartDate.BackColor = GlobalFunctions.ElementColor();
                EndDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FWorkAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefresWorkPlaceDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlWorkDetails())
            {
                if (TransactionName == "INSERT")
                    InsertWork();
                else
                    UpdateWork();
                this.Close();
            }
        }
    }
}