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
    public partial class FObjectRegistrationAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FObjectRegistrationAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int contractID = 0, UsedUserID = -1;
        bool Used = false, CurrentStatus = false;

        private void FObjectRegistrationAddEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.FillLookUpEditWithSqlText(ContractLookUp, $@"SELECT CONTRACT_ID ID, CONTRACT_CODE || ' - ' || CUS.CUSTOMER_NAME NAME
                                                                              FROM CRS_USER.V_CONTRACTS CON, CRS_USER.V_CUSTOMERS CUS
                                                                             WHERE     CON.CREDIT_NAME_ID = 5
                                                                                   AND CON.CUSTOMER_ID = CUS.ID
                                                                                   AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID", "ID", "NAME");

            if (TransactionName == "INSERT")
                SentDate.DateTime = DateTime.Today;
            else
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OBJECT_REGISTRATION", GlobalVariables.V_UserID, "WHERE ID = " + ID + " AND USED_USER_ID = -1");
                LoadDetail();
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
        }

        private void ComponentEnable(bool status)
        {
            ContractLookUp.Enabled =
                SentDate.Enabled =
                ArrivalDate.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadDetail()
        {
            string sql = $@"SELECT CON.CONTRACT_CODE || ' - ' || CUS.CUSTOMER_NAME CONTRACT,
                                   OB.SENT_DATE,
                                   OB.ARRIVAL_DATE,
                                   OB.NOTE,
                                   OB.USED_USER_ID
                              FROM CRS_USER.V_CONTRACTS CON,
                                   CRS_USER.V_CUSTOMERS CUS,
                                   CRS_USER.OBJECT_REGISTRATION OB
                             WHERE     CON.CREDIT_NAME_ID = 5
                                   AND CON.CUSTOMER_ID = CUS.ID
                                   AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND CON.CONTRACT_ID = OB.CONTRACT_ID AND OB.ID = {ID}";

            DataTable dataTable = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetail", "Detallar açılmadı");

            if (dataTable.Rows.Count > 0)
            {
                GlobalProcedures.LookUpEditValue(ContractLookUp, dataTable.Rows[0]["CONTRACT"].ToString());
                SentDate.EditValue = dataTable.Rows[0]["SENT_DATE"];
                ArrivalDate.EditValue = dataTable.Rows[0]["ARRIVAL_DATE"];
                NoteText.EditValue = dataTable.Rows[0]["NOTE"];
                UsedUserID = Convert.ToInt16(dataTable.Rows[0]["USED_USER_ID"]);
            }
        }

        private void ContractLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (ContractLookUp.EditValue == null)
                return;

            contractID = Convert.ToInt32(ContractLookUp.EditValue);
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (contractID == 0)
            {
                ContractLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müqavilə seçilməyib.");
                ContractLookUp.Focus();
                ContractLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.OBJECT_REGISTRATION WHERE CONTRACT_ID = {contractID}{(ID.HasValue ? $@" AND ID != {ID}" : null)}") > 0)
            {
                ContractLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Seçilmiş müqavilə artıq daxil edilib. Zəhmət olmasa başqa müqavilə seçin.");
                ContractLookUp.Focus();
                ContractLookUp.BackColor = GlobalFunctions.ElementColor();
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
            string sql = $@"INSERT INTO CRS_USER.OBJECT_REGISTRATION(CONTRACT_ID,SENT_DATE,ARRIVAL_DATE,INSERT_USER)
                                VALUES({contractID},TO_DATE('{SentDate.Text}','DD.MM.YYYY'),TO_DATE('{ArrivalDate.Text}','DD.MM.YYYY'),{GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Ərizə göndərilmədi.", this.Name + "/InsertDetail");
        }

        public void UpdateDetail()
        {
            string sql = $@"UPDATE CRS_USER.OBJECT_REGISTRATION SET CONTRACT_ID = {contractID},
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
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OBJECT_REGISTRATION", -1, "WHERE ID = " + ID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }
    }
}