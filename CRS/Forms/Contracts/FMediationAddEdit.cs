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
    public partial class FMediationAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FMediationAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int resultID = 0, UsedUserID = -1, contractID = 0;
        bool CurrentStatus = false,
             Used = false;
        string sessionDate = null;

        private void FMediationAddEdit_Load(object sender, EventArgs e)
        {
            LoadResult();
            GlobalProcedures.FillLookUpEdit(ContractCodeLookUp, "V_CONTRACTS", "CONTRACT_ID", "CONTRACT_CODE", "1 = 1 ORDER BY CONTRACT_CODE");
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.MEDIATION", GlobalVariables.V_UserID, "WHERE ID = " + ID.Value + " AND USED_USER_ID = -1");
                LoadDetail();
                Used = (UsedUserID >= 0);
                if (Used)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş məlumatlar hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş mediasiyanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoadDetail()
        {
            string sql = $@"SELECT C.CONTRACT_CODE,
                                     M.DEFENDANT,
                                     M.SEND_DATE,
                                     M.DATE_OF_APPLICATION,
                                     M.MEDIATOR,
                                     M.SESSION_DATE,
                                     MR.NAME RESULT,
                                     M.FIRST_PAYMENT_DATE,
                                     M.REPRESENTATIVE,
                                     M.NOTE,
                                     M.USED_USER_ID
                                FROM CRS_USER.MEDIATION M, CRS_USER.MEDIATION_RESULT MR, CRS_USER.V_CONTRACTS C
                               WHERE M.RESULT_ID = MR.ID(+) AND M.CONTRACT_ID = C.CONTRACT_ID AND M.ID = {ID}";

            DataTable dataTable = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetail", "Mediasiyanın detalları açılmadı.");

            if(dataTable.Rows.Count > 0)
            {
                GlobalProcedures.LookUpEditValue(ContractCodeLookUp, dataTable.Rows[0]["CONTRACT_CODE"].ToString());
                DefendantNameText.EditValue = dataTable.Rows[0]["DEFENDANT"];
                SendDateValue.EditValue = dataTable.Rows[0]["SEND_DATE"];
                DateOfApplicationValue.EditValue = dataTable.Rows[0]["DATE_OF_APPLICATION"];
                NameText.EditValue = dataTable.Rows[0]["MEDIATOR"];
                if (dataTable.Rows[0]["SESSION_DATE"].ToString() != string.Empty)
                {
                    DateTime dateTime = Convert.ToDateTime(dataTable.Rows[0]["SESSION_DATE"]);
                    SessionDateValue.EditValue = dateTime.Date;
                    SessionTime.EditValue = dateTime.TimeOfDay;
                }
                GlobalProcedures.LookUpEditValue(ResultLookUp, dataTable.Rows[0]["RESULT"].ToString());
                FirstPaymentDateValue.EditValue = dataTable.Rows[0]["FIRST_PAYMENT_DATE"];
                RepresentativeText.EditValue = dataTable.Rows[0]["REPRESENTATIVE"];
                NoteText.EditValue = dataTable.Rows[0]["NOTE"];
                UsedUserID = Convert.ToInt16(dataTable.Rows[0]["USED_USER_ID"]);
            }
        }

        public void ComponentEnable(bool status)
        {
            DefendantNameText.Enabled =
                SendDateValue.Enabled =
                DateOfApplicationValue.Enabled =
                NameText.Enabled =
                SessionDateValue.Enabled =  
                RepresentativeText.Enabled =
                NoteText.Enabled = BOK.Visible = !status;
        }

        void LoadResult()
        {
            GlobalProcedures.FillLookUpEdit(ResultLookUp, "MEDIATION_RESULT", "ID", "NAME", "1 = 1 ORDER BY NAME");
        }

        private void ResultLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FMediationResultList fMediationResultList = new FMediationResultList();
                fMediationResultList.RefreshGridViewData += new FMediationResultList.DoEvent(LoadResult);
                fMediationResultList.ShowDialog();
            }
        }

        private void ResultLookUp_EditValueChanged(object sender, EventArgs e)
        {
            resultID = GlobalFunctions.GetLookUpValueMember(ResultLookUp);
        }

        private void Insert()
        {
            if (SessionDateValue.Text.Length > 0)
                sessionDate = SessionDateValue.Text + " " + SessionTime.Text;
            string sql = $@"INSERT INTO CRS_USER.MEDIATION (CONTRACT_ID,
                                                            DEFENDANT,
                                                            SEND_DATE,
                                                            DATE_OF_APPLICATION,
                                                            MEDIATOR,
                                                            SESSION_DATE,
                                                            RESULT_ID,
                                                            FIRST_PAYMENT_DATE,
                                                            REPRESENTATIVE,
                                                            NOTE,
                                                            INSERT_USER)
                                 VALUES({contractID},
                                        '{DefendantNameText.Text.Trim()}',
                                        TO_DATE('{SendDateValue.Text}','DD.MM.YYYY'),
                                        TO_DATE('{DateOfApplicationValue.Text}','DD.MM.YYYY'),
                                        '{NameText.Text.Trim()}',
                                        TO_DATE('{sessionDate}','DD.MM.YYYY HH24:MI:SS'),
                                        {resultID},
                                        TO_DATE('{FirstPaymentDateValue.Text}','DD.MM.YYYY'),
                                        '{RepresentativeText.Text.Trim()}',
                                        '{NoteText.Text.Trim()}',
                                        {GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Mediasiya bazaya daxil edilmədi.", this.Name + "/Insert");
        }

        private void Update()
        {
            if (SessionDateValue.Text.Length > 0)
                sessionDate = SessionDateValue.Text + " " + SessionTime.Text;

            string sql = $@"UPDATE CRS_USER.MEDIATION SET CONTRACT_ID = {contractID},
                                                            DEFENDANT = '{DefendantNameText.Text.Trim()}',
                                                            SEND_DATE = TO_DATE('{SendDateValue.Text}','DD.MM.YYYY'),
                                                            DATE_OF_APPLICATION = TO_DATE('{DateOfApplicationValue.Text}','DD.MM.YYYY'),
                                                            MEDIATOR = '{NameText.Text.Trim()}',
                                                            SESSION_DATE = TO_DATE('{sessionDate}','DD.MM.YYYY HH24:MI:SS'),
                                                            RESULT_ID = {resultID},
                                                            FIRST_PAYMENT_DATE = TO_DATE('{FirstPaymentDateValue.Text}','DD.MM.YYYY'),
                                                            REPRESENTATIVE = '{RepresentativeText.Text.Trim()}',
                                                            NOTE = '{NoteText.Text.Trim()}',
                                                            UPDATE_USER = {GlobalVariables.V_UserID},
                                                            UPDATE_DATE = SYSDATE
                                WHERE ID = {ID}";

            GlobalProcedures.ExecuteQuery(sql, "Mediasiya bazada dəyişdirilmədi.", this.Name + "/Update");
        }

        private void ContractCodeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            contractID = GlobalFunctions.GetLookUpValueMember(ContractCodeLookUp);
        }

        private bool ControlDetail()
        {
            bool b = false;

            if (contractID == 0)
            {
                ContractCodeLookUp.BackColor = Color.Red;
                XtraMessageBox.Show("Müqavilə seçilməyib.");
                ContractCodeLookUp.Focus();
                ContractCodeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DefendantNameText.Text.Length == 0)
            {
                DefendantNameText.BackColor = Color.Red;
                XtraMessageBox.Show("Cavabdehin adı daxil edilməyib.");
                DefendantNameText.Focus();
                DefendantNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SendDateValue.Text.Length == 0)
            {
                SendDateValue.BackColor = Color.Red;
                XtraMessageBox.Show("Mediasiya təklifinin göndərilmə tarixi seçilməyib.");
                SendDateValue.Focus();
                SendDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (DateOfApplicationValue.Text.Length == 0)
            //{
            //    DateOfApplicationValue.BackColor = Color.Red;
            //    XtraMessageBox.Show("Mediasiya təşkilatına müraciət olunduğu tarix seçilməyib.");
            //    DateOfApplicationValue.Focus();
            //    DateOfApplicationValue.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (NameText.Text.Length == 0)
            //{
            //    NameText.BackColor = Color.Red;
            //    XtraMessageBox.Show("Madiatorun adı daxil edilməyib.");
            //    NameText.Focus();
            //    NameText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (SessionDateValue.Text.Length == 0)
            //{
            //    SessionDateValue.BackColor = Color.Red;
            //    XtraMessageBox.Show("Mediasiya sessiyasının tarixi seçilməyib.");
            //    SessionDateValue.Focus();
            //    SessionDateValue.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (resultID == 0)
            //{
            //    ResultLookUp.BackColor = Color.Red;
            //    XtraMessageBox.Show("Nəticə seçilməyib.");
            //    ResultLookUp.Focus();
            //    ResultLookUp.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (RepresentativeText.Text.Length == 0)
            //{
            //    RepresentativeText.BackColor = Color.Red;
            //    XtraMessageBox.Show("Nümayəndənin adı daxil edilməyib.");
            //    RepresentativeText.Focus();
            //    RepresentativeText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlDetail())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private void FMediationAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.MEDIATION", -1, "WHERE ID = " + ID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }
    }
}