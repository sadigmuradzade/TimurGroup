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
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.ObjectRegistration
{
    public partial class FBurdenAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FBurdenAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int RegistrationID, ContractID;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FObjectRegistrationAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
            {
                SentDate.DateTime = DateTime.Today;
                List<Commitments> commitments = CommitmentsDAL.SelectAllCommitmentByContractID(ContractID).ToList<Commitments>();
                if (commitments.Count > 0)
                    FullNameText.EditValue = commitments.LastOrDefault().COMMITMENT_NAME;
                else
                {
                    DataTable dataTable = GlobalFunctions.GenerateDataTable($@"SELECT CUS.CUSTOMER_NAME
                                                                                      FROM CRS_USER.V_CUSTOMERS CUS, CRS_USER.CONTRACTS CON
                                                                                     WHERE     CON.CUSTOMER_ID = CUS.ID
                                                                                           AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                                                                           AND CON.ID = {ContractID}", this.Name + "/FObjectRegistrationAddEdit_Load", "Müştərinin adı tapılmadı.");

                    if (dataTable.Rows.Count > 0)
                        FullNameText.EditValue = dataTable.Rows[0]["CUSTOMER_NAME"];
                }
            }
            else
                LoadDetail();
        }

        private void LoadDetail()
        {
            string sql = $@"SELECT OB.FULLNAME,
                                   OB.SENT_DATE,
                                   OB.ARRIVAL_DATE,
                                   OB.NOTE,
                                   OB.USED_USER_ID
                              FROM CRS_USER.OBJECT_BURDEN_REGISTRATION OB
                             WHERE OB.ID = {ID}";

            DataTable dataTable = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetail", "Detallar açılmadı");

            if (dataTable.Rows.Count > 0)
            {
                FullNameText.EditValue = dataTable.Rows[0]["FULLNAME"];
                SentDate.EditValue = dataTable.Rows[0]["SENT_DATE"];
                ArrivalDate.EditValue = dataTable.Rows[0]["ARRIVAL_DATE"];
                NoteText.EditValue = dataTable.Rows[0]["NOTE"];
            }
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (FullNameText.Text.Length == 0)
            {
                FullNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsin tam adı daxil edilmıyib.");
                FullNameText.Focus();
                FullNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;            

            if (SentDate.Text.Length == 0)
            {
                SentDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");
                SentDate.Focus();
                SentDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        public void InsertDetail()
        {
            string sql = $@"INSERT INTO CRS_USER.OBJECT_BURDEN_REGISTRATION(FULLNAME,OBJECT_REGISTRATION_ID,SENT_DATE,ARRIVAL_DATE,INSERT_USER)
                                VALUES('{FullNameText.Text.Trim()}',{RegistrationID},TO_DATE('{SentDate.Text}','DD.MM.YYYY'),TO_DATE('{ArrivalDate.Text}','DD.MM.YYYY'),{GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Ərizə göndərilmədi.", this.Name + "/InsertDetail");
        }

        public void UpdateDetail()
        {
            string sql = $@"UPDATE CRS_USER.OBJECT_BURDEN_REGISTRATION SET FULLNAME = '{FullNameText.Text.Trim()}',
                                                                    SENT_DATE = TO_DATE('{SentDate.Text}','DD.MM.YYYY'),
                                                                    ARRIVAL_DATE = TO_DATE('{ArrivalDate.Text}','DD.MM.YYYY'),
                                                                    NOTE = '{NoteText.Text.Trim()}',
                                                                    UPDATE_USER = {GlobalVariables.V_UserID},
                                                                    UPDATE_DATE = SYSDATE
                                WHERE ID = {ID}";

            GlobalProcedures.ExecuteQuery(sql, "Ərizə dəyişdirilmədi.", this.Name + "/InsertDetail");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    InsertDetail();
                else
                    UpdateDetail();
                this.Close();
            }
        }

        private void FObjectRegistrationAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }
    }
}