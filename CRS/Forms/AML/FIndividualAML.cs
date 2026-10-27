using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using CRS.Class.Views;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CRS.Forms.AML
{
    public partial class FIndividualAML : DevExpress.XtraEditors.XtraForm
    {
        public FIndividualAML()
        {
            InitializeComponent();
            MaximumSize = MinimumSize = Size;
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public int CustomerID;
        public string CustomerName;

        public delegate void DoEvent();
        public event DoEvent RefreshCustomersDataGridView;

        int riskDegreeID = 0,
            socialStatusID = 0,
            professionID = 0,
            sourceIncomeID = 0,
            localPoliticalDutyID = 0,
            foreignPoliticalDutyID = 0,
            relativePoliticalDutyID = 0,
            customerBehaviorID = 0,
            customerIdentificationMethodID = 0,
            purposeBusinessRelationshipID = 0,
            workID,
            usedUserID = -1;

        bool isWorkPlace = false;
        string transaction = null;

        private void RelativePoliticalFigureRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            RelativeNameText.ReadOnly = RelativePoliticalDutyFromDate.ReadOnly = RelativePoliticalDutyToDate.ReadOnly = RelativePoliticalFigureLookUp.ReadOnly = RelativePoliticalFigureRadioGroup.SelectedIndex == 0;
            if (RelativePoliticalFigureRadioGroup.SelectedIndex == 0)
            {
                RelativeNameText.Text =
                    RelativePoliticalDutyFromDate.Text =
                    RelativePoliticalDutyToDate.Text = null;
                RelativePoliticalFigureLookUp.EditValue = null;
                RelativeStarLabel.Visible = RelativePoliticalStarLabel.Visible = false;
            }
            else
                RelativeStarLabel.Visible = RelativePoliticalStarLabel.Visible = true;
        }

        private void ForeignPoliticalFigureRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ForeignPoliticalDutyFromDate.ReadOnly = ForeignPoliticalDutyToDate.ReadOnly = ForeignPoliticalDutyLookUp.ReadOnly = ForeignPoliticalFigureRadioGroup.SelectedIndex == 0;
            if (ForeignPoliticalFigureRadioGroup.SelectedIndex == 0)
            {
                ForeignPoliticalDutyFromDate.Text = ForeignPoliticalDutyToDate.Text = null;
                ForeignPoliticalDutyLookUp.EditValue = null;
                ForeignPoliticalStarLabel.Visible = false;
            }
            else
                ForeignPoliticalStarLabel.Visible = true;
        }

        private void WorkGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditWorkBarButton.Enabled = DeleteWorkBarButton.Enabled = WorkGridView.RowCount > 0;
        }

        private void WorkGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, CustomerWork_SS, e);
        }

        private void ProfessionLookUp_EditValueChanged(object sender, EventArgs e)
        {
            professionID = GlobalFunctions.GetLookUpID(sender);
        }

        private void SocialStatusLookUp_EditValueChanged(object sender, EventArgs e)
        {
            socialStatusID = GlobalFunctions.GetLookUpID(sender);

            List<SosialStatus> lstSosialStatus = SosialStatusDAL.SelectSosialStatusByID(socialStatusID).ToList<SosialStatus>();
            if (lstSosialStatus.Count > 0)
            {
                var sosialStatus = lstSosialStatus.LastOrDefault();
                isWorkPlace = sosialStatus.IS_WORK_PLACE == 1;
            }
        }

        private void CustomerBehaviorLookUp_EditValueChanged(object sender, EventArgs e)
        {
            customerBehaviorID = GlobalFunctions.GetLookUpID(sender);
        }

        private void LoadFSocialStatusAddEdit(string transaction, int? id)
        {
            FSocialStatusAddEdit fg = new FSocialStatusAddEdit
            {
                TransactionName = transaction,
                ID = id
            };
            fg.RefreshDataGridView += new FSocialStatusAddEdit.DoEvent(LoadSocialStatus);
            fg.ShowDialog();
        }

        private void LoadFProfessionAddEdit(string transaction, int? id)
        {
            FProfessionAddEdit fg = new FProfessionAddEdit
            {
                TransactionName = transaction,
                ID = id
            };
            fg.RefreshDataGridView += new FProfessionAddEdit.DoEvent(LoadProfession);
            fg.ShowDialog();
        }

        private void LoadFSourceIncomeAddEdit(string transaction, int? id)
        {
            FSourceIncomeAddEdit fg = new FSourceIncomeAddEdit
            {
                TransactionName = transaction,
                ID = id
            };
            fg.RefreshDataGridView += new FSourceIncomeAddEdit.DoEvent(LoadSourceIncome);
            fg.ShowDialog();
        }

        private void LoadFPoliticalDutyAddEdit(string transaction, int? id)
        {
            FPoliticalDutyAddEdit fg = new FPoliticalDutyAddEdit
            {
                TransactionName = transaction,
                ID = id
            };
            fg.RefreshDataGridView += new FPoliticalDutyAddEdit.DoEvent(LoadPoliticalDuty);
            fg.ShowDialog();
        }

        private void LoadFCustomerBehaviorAddEdit(string transaction, int? id)
        {
            FCustomerBehaviorAddEdit fg = new FCustomerBehaviorAddEdit
            {
                TransactionName = transaction,
                ID = id
            };
            fg.RefreshDataGridView += new FCustomerBehaviorAddEdit.DoEvent(LoadCustomerBehavior);
            fg.ShowDialog();
        }

        private void LoadFCustomerIdentificationMethodAddEdit(string transaction, int? id)
        {
            FCustomerIdentificationMethodAddEdit fg = new FCustomerIdentificationMethodAddEdit
            {
                TransactionName = transaction,
                ID = id
            };
            fg.RefreshDataGridView += new FCustomerIdentificationMethodAddEdit.DoEvent(LoadCustomerIdentificationMethod);
            fg.ShowDialog();
        }

        private void LoadFPurposeBusinessAddEdit(string transaction, int? id)
        {
            FPurposeBusinessAddEdit fg = new FPurposeBusinessAddEdit
            {
                TransactionName = transaction,
                ID = id
            };
            fg.RefreshDataGridView += new FPurposeBusinessAddEdit.DoEvent(LoadPurposeBusinessRelationship);
            fg.ShowDialog();
        }

        private void CustomerIdentificationMethodLookUp_EditValueChanged(object sender, EventArgs e)
        {
            customerIdentificationMethodID = GlobalFunctions.GetLookUpID(sender);
        }

        private void RelativePoliticalFigureLookUp_EditValueChanged(object sender, EventArgs e)
        {
            relativePoliticalDutyID = GlobalFunctions.GetLookUpID(sender);
        }

        private void ProfessionLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFProfessionAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && professionID != 0)
                LoadFProfessionAddEdit("EDIT", professionID);
        }

        private void SocialStatusLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFSocialStatusAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && socialStatusID != 0)
                LoadFSocialStatusAddEdit("EDIT", socialStatusID);
        }

        private void SourceIncomeLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFSourceIncomeAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && sourceIncomeID != 0)
                LoadFSourceIncomeAddEdit("EDIT", sourceIncomeID);
        }

        private void NewWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAMLWorkAddEdit("INSERT", CustomerID, null);
        }

        void EditWork()
        {
            LoadFAMLWorkAddEdit("EDIT", CustomerID, workID);
        }

        private void EditWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            EditWork();
        }

        private void WorkGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditWorkBarButton.Enabled)
                EditWork();
        }

        private void RefreshWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadWorkDataGridView();
        }

        private void CellCopyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ClipboardGridViewCell(WorkGridView);
        }

        private void DeleteWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalFunctions.CallDialogResult("Seçilmiş iş yerini silmək istəyirsiniz?", "Müştərinin iş yerinin silinməsi") == DialogResult.Yes)
                GlobalProcedures.ExecuteQuery($@"UPDATE COMS_USER_TEMP.CUSTOMER_AML_WORK_TEMP SET IS_CHANGE = 2 WHERE ID = {workID}",
                                                "Müştərinin iş yeri üçün olan temp məlumatlar silinmədi.",
                                                this.Name + "/DeleteWork");
            LoadWorkDataGridView();
        }

        private void InsertCustomerAML(OracleTransaction tran)
        {
            string sql = $@"INSERT INTO CRS_USER.CUSTOMER_AML(CUSTOMER_ID,
                                                               RISK_DEGREE_ID,
                                                               SOCIAL_STATUS_ID,
                                                               PROFESSION_ID,
                                                               MONTHLY_AMOUNT,
                                                               SOURCE_INCOME_ID,
                                                               IS_LOCAL_POLITICAL_FIGURE,
                                                               LOCAL_POLITICAL_DUTY_ID,
                                                               LOCAL_POLITICAL_START_DATE,
                                                               LOCAL_POLITICAL_END_DATE,
                                                               IS_FOREIGN_POLITICAL_FIGURE,
                                                               FOREIGN_POLITICAL_DUTY_ID,
                                                               FOREIGN_POLITICAL_START_DATE,
                                                               FOREIGN_POLITICAL_END_DATE,
                                                               IS_RELATIVE_POLITICAL_FIGURE,
                                                               RELATIVE_NAME,
                                                               RELATIVE_POLITICAL_DUTY_ID,
                                                               RELATIVE_POLITICAL_START_DATE,
                                                               RELATIVE_POLITICAL_END_DATE,
                                                               CUSTOMER_BEHAVIOR_ID,
                                                               CUSTOMER_IDENTIFICATION_ID,
                                                               PURPOSE_BUSINESS_RELATIONSHIP,
                                                               IS_SANCTION_CHECK,
                                                               SANCTION_CHECK_URL,
                                                               NOTE,
                                                               INSERT_USER)
                        VALUES({CustomerID},
                               {riskDegreeID},
                               {socialStatusID},
                               {professionID},
                               {MonthlyAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                               {sourceIncomeID},
                               {LocalPoliticalFigureRadioGroup.SelectedIndex},
                               {localPoliticalDutyID},
                               TO_DATE('{LocalPoliticalDutyFromDate.Text}','DD.MM.YYYY'),
                               TO_DATE('{LocalPoliticalDutyToDate.Text}','DD.MM.YYYY'),
                               {ForeignPoliticalFigureRadioGroup.SelectedIndex},
                               {foreignPoliticalDutyID},
                               TO_DATE('{ForeignPoliticalDutyFromDate.Text}','DD.MM.YYYY'),
                               TO_DATE('{ForeignPoliticalDutyToDate.Text}','DD.MM.YYYY'),
                               {RelativePoliticalFigureRadioGroup.SelectedIndex},
                               '{RelativeNameText.Text.Trim()}',
                               {relativePoliticalDutyID},
                               TO_DATE('{RelativePoliticalDutyFromDate.Text}','DD.MM.YYYY'),
                               TO_DATE('{RelativePoliticalDutyToDate.Text}','DD.MM.YYYY'),
                               {customerBehaviorID},
                               {customerIdentificationMethodID},
                               {purposeBusinessRelationshipID},
                               {SanctionCheckRadioGroup.SelectedIndex},
                               '{SanctionCheckURLText.Text.Trim()}',    
                               '{NoteText.Text.Trim()}',
                               {GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(tran, sql);
        }

        private void UpdateCustomerAML(OracleTransaction tran)
        {
            string sql = $@"UPDATE CRS_USER.CUSTOMER_AML SET RISK_DEGREE_ID = {riskDegreeID},
                                                               SOCIAL_STATUS_ID = {socialStatusID},
                                                               PROFESSION_ID = {professionID},
                                                               MONTHLY_AMOUNT = {MonthlyAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                               SOURCE_INCOME_ID = {sourceIncomeID},
                                                               IS_LOCAL_POLITICAL_FIGURE = {LocalPoliticalFigureRadioGroup.SelectedIndex},
                                                               LOCAL_POLITICAL_DUTY_ID = {localPoliticalDutyID},
                                                               LOCAL_POLITICAL_START_DATE = TO_DATE('{LocalPoliticalDutyFromDate.Text}','DD.MM.YYYY'),
                                                               LOCAL_POLITICAL_END_DATE = TO_DATE('{LocalPoliticalDutyToDate.Text}','DD.MM.YYYY'),
                                                               IS_FOREIGN_POLITICAL_FIGURE = {ForeignPoliticalFigureRadioGroup.SelectedIndex},
                                                               FOREIGN_POLITICAL_DUTY_ID = {foreignPoliticalDutyID},
                                                               FOREIGN_POLITICAL_START_DATE = TO_DATE('{ForeignPoliticalDutyFromDate.Text}','DD.MM.YYYY'),
                                                               FOREIGN_POLITICAL_END_DATE = TO_DATE('{ForeignPoliticalDutyToDate.Text}','DD.MM.YYYY'),
                                                               IS_RELATIVE_POLITICAL_FIGURE = {RelativePoliticalFigureRadioGroup.SelectedIndex},
                                                               RELATIVE_NAME = '{RelativeNameText.Text.Trim()}',
                                                               RELATIVE_POLITICAL_DUTY_ID = {relativePoliticalDutyID},
                                                               RELATIVE_POLITICAL_START_DATE = TO_DATE('{RelativePoliticalDutyFromDate.Text}','DD.MM.YYYY'),
                                                               RELATIVE_POLITICAL_END_DATE = TO_DATE('{RelativePoliticalDutyToDate.Text}','DD.MM.YYYY'),
                                                               CUSTOMER_BEHAVIOR_ID = {customerBehaviorID},
                                                               CUSTOMER_IDENTIFICATION_ID = {customerIdentificationMethodID},
                                                               PURPOSE_BUSINESS_RELATIONSHIP = {purposeBusinessRelationshipID},
                                                               IS_SANCTION_CHECK = {SanctionCheckRadioGroup.SelectedIndex},
                                                               SANCTION_CHECK_URL = '{SanctionCheckURLText.Text.Trim()}',  
                                                               NOTE = '{NoteText.Text.Trim()}',
                                                               UPDATE_USER = {GlobalVariables.V_UserID},
                                                               UPDATE_DATE = SYSDATE
                        WHERE CUSTOMER_ID = {CustomerID}";

            GlobalProcedures.ExecuteQuery(tran, sql);
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (riskDegreeID == 0)
            {
                RiskDegreeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin riks dərəcəsi seçilməyib.");
                RiskDegreeLookUp.Focus();
                RiskDegreeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (socialStatusID == 0)
            {
                SocialStatusLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin sosial status seçilməyib.");
                SocialStatusLookUp.Focus();
                SocialStatusLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (professionID == 0)
            {
                ProfessionLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin peşəsi seçilməyib.");
                ProfessionLookUp.Focus();
                ProfessionLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (MonthlyAmountValue.Value <= 0)
            {
                MonthlyAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin aylıq gəliri daxil edilməyib.");
                MonthlyAmountValue.Focus();
                MonthlyAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (MonthlyAmountValue.Value > 0 && sourceIncomeID == 0)
            {
                SourceIncomeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin aylıq gəlirinin mənbəyi seçilməyib.");
                SourceIncomeLookUp.Focus();
                SourceIncomeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (isWorkPlace && WorkGridView.RowCount == 0)
            {
                GlobalProcedures.ShowErrorMessage("Müştərinin iş yeri daxil edilməyib.");
                return false;
            }
            else
                b = true;

            if (LocalPoliticalFigureRadioGroup.SelectedIndex == 1 && localPoliticalDutyID == 0)
            {
                LocalPoliticalDutyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Siyasi vəzifə seçilməyib.");
                LocalPoliticalDutyLookUp.Focus();
                LocalPoliticalDutyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (LocalPoliticalFigureRadioGroup.SelectedIndex == 1 && localPoliticalDutyID > 0 && LocalPoliticalDutyFromDate.Text.Length == 0)
            {
                LocalPoliticalDutyFromDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Siyasi vəzifədə nə vaxtdan çalışdığı seçilməyib.");
                LocalPoliticalDutyFromDate.Focus();
                LocalPoliticalDutyFromDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ForeignPoliticalFigureRadioGroup.SelectedIndex == 1 && foreignPoliticalDutyID == 0)
            {
                ForeignPoliticalDutyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Siyasi vəzifə seçilməyib.");
                ForeignPoliticalDutyLookUp.Focus();
                ForeignPoliticalDutyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ForeignPoliticalFigureRadioGroup.SelectedIndex == 1 && foreignPoliticalDutyID > 0 && ForeignPoliticalDutyFromDate.Text.Length == 0)
            {
                ForeignPoliticalDutyFromDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Siyasi vəzifədə nə vaxtdan çalışdığı seçilməyib.");
                ForeignPoliticalDutyFromDate.Focus();
                ForeignPoliticalDutyFromDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (RelativePoliticalFigureRadioGroup.SelectedIndex == 1 && RelativeNameText.Text.Length == 0)
            {
                RelativeNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qohumun adı daxil edilməyib.");
                RelativeNameText.Focus();
                RelativeNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (RelativePoliticalFigureRadioGroup.SelectedIndex == 1 && relativePoliticalDutyID == 0)
            {
                RelativePoliticalFigureLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Siyasi vəzifə seçilməyib.");
                RelativePoliticalFigureLookUp.Focus();
                RelativePoliticalFigureLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (RelativePoliticalFigureRadioGroup.SelectedIndex == 1 && relativePoliticalDutyID > 0 && RelativePoliticalDutyFromDate.Text.Length == 0)
            {
                RelativePoliticalDutyFromDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Siyasi vəzifədə nə vaxtdan çalışdığı seçilməyib.");
                RelativePoliticalDutyFromDate.Focus();
                RelativePoliticalDutyFromDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (customerBehaviorID == 0)
            {
                CustomerBehaviorLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin davranışı seçilməyib.");
                CustomerBehaviorLookUp.Focus();
                CustomerBehaviorLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (customerIdentificationMethodID == 0)
            {
                CustomerIdentificationMethodLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin identifikasiya üsulu seçilməyib.");
                CustomerIdentificationMethodLookUp.Focus();
                CustomerIdentificationMethodLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (purposeBusinessRelationshipID == 0)
            {
                PurposeBusinessRelationshipLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin işgüzar münasibətin məqsədi seçilməyib.");
                PurposeBusinessRelationshipLookUp.Focus();
                PurposeBusinessRelationshipLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SanctionCheckRadioGroup.SelectedIndex == 1 && SanctionCheckURLText.Text.Length == 0)
            {
                SanctionCheckURLText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstinad linki daxil edilməyib.");
                SanctionCheckURLText.Focus();
                SanctionCheckURLText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCustomerSaveWait));

                GlobalFunctions.RunInOneTransaction<int>(tran =>
                {
                    if (transaction == "INSERT")
                        InsertCustomerAML(tran);
                    else
                        UpdateCustomerAML(tran);

                    GlobalProcedures.ExecuteProcedureWithTwoParametr(tran, "CRS_USER.PROC_INSERT_AML_WORK", "P_CUSTOMER_ID", CustomerID, "P_USED_USER_ID", GlobalVariables.V_UserID);
                    return 1;
                }, "Müştərinin AML məlumatları bazada dəyişdirilmədi.");

                GlobalProcedures.SplashScreenClose();
                this.Close();
            }
        }

        private void WorkGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = WorkGridView.GetFocusedDataRow();
            if (row != null)
                workID = Convert.ToInt32(row["ID"]);
        }

        private void SactionsHyperlinkLabelControl_HyperlinkClick(object sender, DevExpress.Utils.HyperlinkClickEventArgs e)
        {
            SactionsHyperlinkLabelControl.LinkVisited = true;
            System.Diagnostics.Process.Start(e.Link);
        }

        private void SanctionCheckRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SanctionCheckRadioGroup.SelectedIndex == 0)
            {
                SanctionCheckURLText.Text = null;
                SanctionCheckURLText.ReadOnly = true;
            }
            else
            {
                SanctionCheckURLText.ReadOnly = false;
                SanctionCheckURLText.Focus();
            }
        }

        private void PurposeBusinessRelationshipLookUp_EditValueChanged(object sender, EventArgs e)
        {
            purposeBusinessRelationshipID = GlobalFunctions.GetLookUpID(sender);
        }

        private void ForeignPoliticalDutyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            foreignPoliticalDutyID = GlobalFunctions.GetLookUpID(sender);
        }

        private void RiskDegreeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            riskDegreeID = GlobalFunctions.GetLookUpID(sender);
        }

        private void LocalPoliticalDutyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            localPoliticalDutyID = GlobalFunctions.GetLookUpID(sender);
        }

        private void LocalPoliticalDutyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFPoliticalDutyAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && localPoliticalDutyID != 0)
                LoadFPoliticalDutyAddEdit("EDIT", localPoliticalDutyID);
        }

        private void LocalPoliticalFigureRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocalPoliticalDutyFromDate.ReadOnly = LocalPoliticalDutyToDate.ReadOnly = LocalPoliticalDutyLookUp.ReadOnly = LocalPoliticalFigureRadioGroup.SelectedIndex == 0;
            if (LocalPoliticalFigureRadioGroup.SelectedIndex == 0)
            {
                LocalPoliticalDutyFromDate.Text = LocalPoliticalDutyToDate.Text = null;
                LocalPoliticalDutyLookUp.EditValue = null;
                LocalPoliticalStarLabel.Visible = false;
            }
            else
                LocalPoliticalStarLabel.Visible = true;
        }

        private void ForeignPoliticalDutyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFPoliticalDutyAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && foreignPoliticalDutyID != 0)
                LoadFPoliticalDutyAddEdit("EDIT", foreignPoliticalDutyID);
        }

        private void RelativePoliticalFigureLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFPoliticalDutyAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && relativePoliticalDutyID != 0)
                LoadFPoliticalDutyAddEdit("EDIT", relativePoliticalDutyID);
        }

        private void FIndividualAML_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CUSTOMERS", GlobalVariables.V_UserID, "WHERE ID = " + CustomerID + " AND USED_USER_ID = -1");
            LoadCustomerDetails();

            if (GlobalVariables.V_UserID != usedUserID)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == usedUserID).FULLNAME;
                GlobalProcedures.ShowWarningMessage("Seçilmiş şəxsin məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Buna görə də, bu müştəri üçün AML məlumatları daxil edilə bilməz.", "Seçilmiş müştərinin hal-hazırkı statusu");
                return;
            }

            CustomerNameText.EditValue = CustomerName;

            LoadRiskDegree();
            LoadSocialStatus();
            LoadProfession();
            LoadSourceIncome();
            LoadPoliticalDuty();
            LoadCustomerBehavior();
            LoadCustomerIdentificationMethod();
            LoadPurposeBusinessRelationship();
            InsertWorkTemp();
            LoadWorkDataGridView();
            LocalPoliticalFigureRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
            ForeignPoliticalFigureRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
            SanctionCheckRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
            RelativePoliticalFigureRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);

            LoadCustomerAML();
        }

        private void LoadCustomerDetails()
        {
            string s = $@"SELECT USED_USER_ID
                              FROM CRS_USER.CUSTOMERS C
                             WHERE   C.ID = {CustomerID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCustomerDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    usedUserID = Convert.ToInt32(dr["USED_USER_ID"]);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müştərinin parametrləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        void InsertWorkTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INS_AML_WORK_TEMP", "P_CUSTOMER_ID", CustomerID, "İş yerləri temp cədvələ daxil edilmədi.");
        }

        void LoadRiskDegree()
        {
            GlobalProcedures.FillLookUpEdit(RiskDegreeLookUp, "RISK_DEGREE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        private void CustomerBehaviorLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFCustomerBehaviorAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && customerBehaviorID != 0)
                LoadFCustomerBehaviorAddEdit("EDIT", customerBehaviorID);
        }

        private void CustomerIdentificationMethodLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFCustomerIdentificationMethodAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && customerIdentificationMethodID != 0)
                LoadFCustomerIdentificationMethodAddEdit("EDIT", customerIdentificationMethodID);
        }

        private void PurposeBusinessRelationshipLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFPurposeBusinessAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && purposeBusinessRelationshipID != 0)
                LoadFPurposeBusinessAddEdit("EDIT", purposeBusinessRelationshipID);
        }

        private void WorkGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(WorkGridView, PopupMenu, e);
        }

        private void SourceIncomeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            sourceIncomeID = GlobalFunctions.GetLookUpID(sender);
        }

        private void FIndividualAML_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CUSTOMERS", -1, "WHERE ID = " + CustomerID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCustomersDataGridView();
        }

        private void LocalPoliticalDutyFromDate_EditValueChanged(object sender, EventArgs e)
        {
            LocalPoliticalDutyToDate.Properties.MinValue = LocalPoliticalDutyFromDate.DateTime;
        }

        private void ForeignPoliticalDutyFromDate_EditValueChanged(object sender, EventArgs e)
        {
            ForeignPoliticalDutyToDate.Properties.MinValue = ForeignPoliticalDutyFromDate.DateTime;
        }

        private void RelativePoliticalDutyFromDate_EditValueChanged(object sender, EventArgs e)
        {
            RelativePoliticalDutyToDate.Properties.MinValue = RelativePoliticalDutyFromDate.DateTime;
        }

        private void BRemove_Click(object sender, EventArgs e)
        {
            if (GlobalFunctions.CallDialogResult("AML-ni silmək istəyirsiniz?", "AML-nin silinməsi") == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DELETE_CUSTOMER_AML", "P_CUSTOMER_ID", CustomerID, "AML silinmədi");
                LoadCustomerAML();
                LoadWorkDataGridView();
            }
        }

        void LoadSocialStatus()
        {
            GlobalProcedures.FillLookUpEdit(SocialStatusLookUp, "SOCIAL_STATUS", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        void LoadProfession()
        {
            GlobalProcedures.FillLookUpEdit(ProfessionLookUp, "PROFESSION", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        void LoadSourceIncome()
        {
            GlobalProcedures.FillLookUpEdit(SourceIncomeLookUp, "SOURCE_INCOME", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        void LoadPoliticalDuty()
        {
            GlobalProcedures.FillLookUpEdit(LocalPoliticalDutyLookUp, "POLITICAL_DUTY", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillLookUpEdit(ForeignPoliticalDutyLookUp, "POLITICAL_DUTY", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillLookUpEdit(RelativePoliticalFigureLookUp, "POLITICAL_DUTY", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        void LoadCustomerBehavior()
        {
            GlobalProcedures.FillLookUpEdit(CustomerBehaviorLookUp, "CUSTOMER_BEHAVIOR", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        void LoadCustomerIdentificationMethod()
        {
            GlobalProcedures.FillLookUpEdit(CustomerIdentificationMethodLookUp, "CUSTOMER_IDENTIFICATION_METHOD", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        void LoadPurposeBusinessRelationship()
        {
            GlobalProcedures.FillLookUpEdit(PurposeBusinessRelationshipLookUp, "PURPOSE_BUSINESS_RELATIONSHIP", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        private void LoadCustomerAML()
        {
            List<CustomerAMLView> customerAML = CustomerAMLDAL.SelectCustomerAML(CustomerID).ToList<CustomerAMLView>();
            if (customerAML.Count > 0)
            {
                transaction = "UPDATE";
                var aml = customerAML.LastOrDefault();

                GlobalProcedures.LookUpEditValue(RiskDegreeLookUp, aml.RISK_DEGREE_NAME);
                GlobalProcedures.LookUpEditValue(SocialStatusLookUp, aml.SOCIAL_STATUS_NAME);
                GlobalProcedures.LookUpEditValue(ProfessionLookUp, aml.PROFESSION_NAME);
                MonthlyAmountValue.EditValue = aml.MONTHLY_AMOUNT;
                GlobalProcedures.LookUpEditValue(SourceIncomeLookUp, aml.SOURCE_INCOME_NAME);
                LocalPoliticalFigureRadioGroup.SelectedIndex = aml.IS_LOCAL_POLITICAL_FIGURE;
                GlobalProcedures.LookUpEditValue(LocalPoliticalDutyLookUp, aml.LOCAL_POLITICAL_DUTY_NAME);
                if (aml.LOCAL_POLITICAL_START_DATE > DateTime.MinValue)
                    LocalPoliticalDutyFromDate.EditValue = aml.LOCAL_POLITICAL_START_DATE;
                if (aml.LOCAL_POLITICAL_END_DATE > DateTime.MinValue)
                    LocalPoliticalDutyToDate.EditValue = aml.LOCAL_POLITICAL_END_DATE;
                ForeignPoliticalFigureRadioGroup.SelectedIndex = aml.IS_FOREIGN_POLITICAL_FIGURE;
                GlobalProcedures.LookUpEditValue(ForeignPoliticalDutyLookUp, aml.FOREIGN_POLITICAL_DUTY_NAME);
                if (aml.FOREIGN_POLITICAL_START_DATE > DateTime.MinValue)
                    ForeignPoliticalDutyFromDate.EditValue = aml.FOREIGN_POLITICAL_START_DATE;
                if (aml.FOREIGN_POLITICAL_END_DATE > DateTime.MinValue)
                    ForeignPoliticalDutyToDate.EditValue = aml.FOREIGN_POLITICAL_END_DATE;
                RelativePoliticalFigureRadioGroup.SelectedIndex = aml.IS_RELATIVE_POLITICAL_FIGURE;
                RelativeNameText.EditValue = aml.RELATIVE_NAME;
                GlobalProcedures.LookUpEditValue(RelativePoliticalFigureLookUp, aml.RELATIVE_POLITICAL_DUTY_NAME);
                if (aml.RELATIVE_POLITICAL_START_DATE > DateTime.MinValue)
                    RelativePoliticalDutyFromDate.EditValue = aml.RELATIVE_POLITICAL_START_DATE;
                if (aml.RELATIVE_POLITICAL_END_DATE > DateTime.MinValue)
                    RelativePoliticalDutyToDate.EditValue = aml.RELATIVE_POLITICAL_END_DATE;
                GlobalProcedures.LookUpEditValue(CustomerBehaviorLookUp, aml.CUSTOMER_BEHAVIOR_NAME);
                GlobalProcedures.LookUpEditValue(CustomerIdentificationMethodLookUp, aml.CUSTOMER_IDENTIFICATION_NAME);
                GlobalProcedures.LookUpEditValue(PurposeBusinessRelationshipLookUp, aml.PURPOSE_BUSINESS_NAME);
                SanctionCheckRadioGroup.SelectedIndex = aml.IS_SANCTION_CHECK;
                SanctionCheckURLText.EditValue = aml.SANCTION_CHECK_URL;
                NoteText.EditValue = aml.NOTE;
            }
            else
            {
                RiskDegreeLookUp.EditValue =
                    SocialStatusLookUp.EditValue = 
                    ProfessionLookUp.EditValue = 
                    SourceIncomeLookUp.EditValue = 
                    LocalPoliticalDutyLookUp.EditValue =
                    ForeignPoliticalDutyLookUp.EditValue =
                    CustomerBehaviorLookUp.EditValue =
                    CustomerIdentificationMethodLookUp.EditValue =
                    PurposeBusinessRelationshipLookUp.EditValue =
                    RelativePoliticalFigureLookUp.EditValue =
                    RelativeNameText.Text =                                    
                    SanctionCheckURLText.Text = 
                    NoteText.Text = 
                    LocalPoliticalDutyFromDate.Text =
                    LocalPoliticalDutyToDate.Text =
                    ForeignPoliticalDutyFromDate.Text =
                    ForeignPoliticalDutyToDate.Text =
                    RelativePoliticalDutyFromDate.Text =
                    RelativePoliticalDutyToDate.Text = null;

                MonthlyAmountValue.EditValue = 
                    LocalPoliticalFigureRadioGroup.SelectedIndex = 
                    ForeignPoliticalFigureRadioGroup.SelectedIndex = 
                    RelativePoliticalFigureRadioGroup.SelectedIndex = 
                    SanctionCheckRadioGroup.SelectedIndex = 0;
                
                transaction = "INSERT";
            }

            BRemove.Visible = transaction == "UPDATE";
        }

        private void LoadWorkDataGridView()
        {
            string s = $@"SELECT ID,
                                   PLACE_NAME,
                                   POSITION,
                                   START_DATE,
                                   END_DATE,
                                   NOTE,
                                   ADDRESS,
                                   EMAIL,
                                   PHONE
                              FROM CRS_USER_TEMP.CUSTOMER_AML_WORK_TEMP
                             WHERE IS_CHANGE <> 2 AND CUSTOMER_ID = {CustomerID}";
            WorkGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            EditWorkBarButton.Enabled = DeleteWorkBarButton.Enabled = CellCopyBarButton.Enabled = WorkGridView.RowCount > 0;
        }

        public void LoadFAMLWorkAddEdit(string transaction, int CustomerID, int? WorkID)
        {
            FAMLWorkAddEdit fp = new FAMLWorkAddEdit
            {
                TransactionName = transaction,
                WorkID = WorkID,
                CustomerID = CustomerID
            };
            fp.RefresWorkPlaceDataGridView += new FAMLWorkAddEdit.DoEvent(LoadWorkDataGridView);
            fp.ShowDialog();
        }
    }
}