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
using Oracle.ManagedDataAccess.Client;

namespace CRS.Forms.Contracts
{
    public partial class FStopMediation : DevExpress.XtraEditors.XtraForm
    {
        public FStopMediation()
        {
            InitializeComponent();
        }
        public int MediationID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int mediationStatusID = 0,
            lawStatusID = 0,
            lawID = 0,
            contractID;

        private void FStopMediation_Load(object sender, EventArgs e)
        {
            LoadMediationDetail();
            GlobalProcedures.FillLookUpEdit(MediationStatusLookUp, "MEDIATION_STATUS", "ID", "NAME", "ID != 1 ORDER BY ID");
            this.Size = new Size(601, 204);
        }

        private void MediationStatusLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (MediationStatusLookUp.EditValue == null)
                return;

            mediationStatusID = Convert.ToInt32(MediationStatusLookUp.EditValue);

            StartDateLabel.Visible =
                StartDateStarLabel.Visible = 
                StartDate.Visible = 
                LawLabel.Visible = 
                LawStarLabel.Visible = 
                LawComboBox.Visible = 
                JudgeNameLabel.Visible = 
                JudgeNameText.Visible = 
                StatusLabel.Visible = 
                StatusComboBox.Visible = 
                StatusStarLabel.Visible = 
                NoteLabel.Visible = 
                NoteText.Visible = mediationStatusID == 3;

            if (mediationStatusID == 3)
            {
                this.Size = new Size(601, 335);
                RefreshLaws();
                RefreshLawStatus();
                StartDate.DateTime = DateTime.Today;
            }
            else
                this.Size = new Size(601, 204);
        }

        private void FStopMediation_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        void RefreshLawStatus()
        {
            GlobalProcedures.FillComboBoxEdit(StatusComboBox, "LAW_STATUS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
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

        private void StatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lawStatusID = GlobalFunctions.FindComboBoxSelectedValue("LAW_STATUS", "NAME", "ID", StatusComboBox.Text);
        }

        private void LoadMediationDetail()
        {
            string sql = $@"SELECT C.CONTRACT_CODE, M.DEFENDANT, C.CONTRACT_ID
                                  FROM CRS_USER.MEDIATION M, CRS_USER.V_CONTRACTS C
                                 WHERE M.CONTRACT_ID = C.CONTRACT_ID AND M.ID = {MediationID}";

            DataTable dataTable = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadMediationDetail", "Mediasiyanın məlumatları açılmadı.");

            if(dataTable.Rows.Count > 0)
            {
                ContractCodeText.EditValue = dataTable.Rows[0]["CONTRACT_CODE"];
                DefendantNameText.EditValue = dataTable.Rows[0]["DEFENDANT"];
                contractID = Convert.ToInt32(dataTable.Rows[0]["CONTRACT_ID"]);
            }
        }

        void RefreshLaws()
        {
            GlobalProcedures.FillComboBoxEdit(LawComboBox, "LAWS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
        }

        private void LawComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lawID = GlobalFunctions.FindComboBoxSelectedValue("LAWS", "NAME", "ID", LawComboBox.Text);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlDetail())
            {
                GlobalFunctions.RunInOneTransaction<int>(tran =>
                {
                    GlobalProcedures.ExecuteQuery(tran, $@"UPDATE CRS_USER.MEDIATION SET MEDIATION_STATUS_ID = {mediationStatusID} WHERE ID = {MediationID}");
                    if (mediationStatusID == 3)
                        InsertProsecute(tran);
                    return 1;
                }, "Mediasiyanın statusu bazada dəyişdirilmədi.");
                this.Close();
            }
        }

        private void InsertProsecute(OracleTransaction tran)
        {
            GlobalProcedures.ExecuteQuery(tran, $@"INSERT INTO CRS_USER.CONTRACT_LAWS(ID,
                                                                                        CONTRACT_ID,
                                                                                        DEFANDANT_NAME,
                                                                                        LAW_ID,
                                                                                        JUDGE_NAME,
                                                                                        LAW_STATUS_ID,
                                                                                        START_DATE,
                                                                                        NOTE,
                                                                                        CREATED_USER_NAME,
                                                                                        APPLICANT,
                                                                                        MEDIATION_ID)
                                            VALUES(CRS_USER.CONTRACT_LAWS_SEQUENCE.NEXTVAL,
                                                    {contractID},
                                                    '{DefendantNameText.Text.Trim()}',
                                                    {lawID},
                                                    '{JudgeNameText.Text.Trim()}',                                                    
                                                    {lawStatusID},
                                                    TO_DATE('{StartDate.Text}','DD.MM.YYYY'),
                                                    '{NoteText.Text.Trim()}',
                                                    '{GlobalVariables.V_UserName}',
                                                    '{GlobalFunctions.ReadSetting("Company")}',
                                                    {MediationID})");
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

        private bool ControlDetail()
        {
            bool b = false;

            if (mediationStatusID == 0)
            {
                MediationStatusLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Mediasiyanın statusu seçilməyib.");
                MediationStatusLookUp.Focus();
                MediationStatusLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (mediationStatusID == 3 && StartDate.Text.Length == 0)
            {
                StartDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məhkəməyə verilmə tarixi seçilməyib.");
                StartDate.Focus();
                StartDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (mediationStatusID == 3 && lawID <= 0)
            {
                LawComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məhkəmə seçilməyib.");
                LawComboBox.Focus();
                LawComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (mediationStatusID == 3 && lawStatusID <= 0)
            {
                StatusComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İclasın statusu seçilməyib.");
                StatusComboBox.Focus();
                StatusComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }
    }
}