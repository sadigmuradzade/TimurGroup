using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Views;
using DevExpress.Utils;
using DevExpress.XtraTreeList;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static CRS.Class.Enum;

namespace CRS.Forms.AML
{
    public partial class FJuridicalAML : DevExpress.XtraEditors.XtraForm
    {
        public FJuridicalAML()
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
            purposeBusinessRelationshipID = 0,
            organizationLegalID = 0,
            focusedID,
            founderPersonTypeID,
            parentFounderID,
            founderID,
            usedUserID = -1;

        string transaction = null, founderName;

        private void FJuridicalAML_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.JURIDICAL_PERSONS", GlobalVariables.V_UserID, "WHERE ID = " + CustomerID + " AND USED_USER_ID = -1");
            GlobalProcedures.FillLookUpEdit(OrganizationLegalLookUp, "ORGANIZATIONAL_LEGAL", "ID", "NAME", "1 = 1 ORDER BY ID");
            RegistrationDateValue.Properties.MaxValue = DRCIssueDateValue.Properties.MaxValue = DateTime.Today;
            LoadRiskDegree();
            LoadPurposeBusinessRelationship();
            LoadCustomerDetails();
            SanctionCheckRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
            if (GlobalVariables.V_UserID != usedUserID)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == usedUserID).FULLNAME;
                GlobalProcedures.ShowWarningMessage("Seçilmiş şəxsin məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Buna görə də, bu müştəri üçün AML məlumatları daxil edilə bilməz.", "Seçilmiş müştərinin hal-hazırkı statusu");
                return;
            }

            CustomerNameText.EditValue = CustomerName;
            LoadCustomerAML();
            GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER_TEMP.PROC_INS_JP_FOUNDER_TEMP", "P_CUSTOMER_ID", CustomerID, "P_USED_USER_ID", GlobalVariables.V_UserID, "Təsisçilər temp cədvələ daxil edilmədi.");
            LoadJuridicalFounder();
        }

        private void LoadCustomerAML()
        {

            List<JuridicalAMLView> customerAML = JuridicalAMLDAL.SelectCustomerAML(CustomerID).ToList<JuridicalAMLView>();
            if (customerAML.Count > 0)
            {
                transaction = "UPDATE";
                var aml = customerAML.LastOrDefault();

                GlobalProcedures.LookUpEditValue(RiskDegreeLookUp, aml.RISK_DEGREE_NAME);
                GlobalProcedures.LookUpEditValue(OrganizationLegalLookUp, aml.ORGANIZATIONAL_LEGAL_NAME);
                RegistrationDateValue.EditValue = aml.REGISTRATION_DATE;
                AuthorizedCapitalValue.EditValue = aml.AUTHORIZED_CAPITAL;
                DRCIssueDateValue.EditValue = aml.DRC_ISSUE_DATE;
                EmployeesNumberValue.EditValue = aml.EMPLOYEES_NUMBER;
                BranchCountValue.EditValue = aml.BRANCH_COUNT;
                ValueAssetsValue.EditValue = aml.VALUE_ASSETS;
                OldCompanyNameText.EditValue = aml.OLD_COMPANY_NAME;
                GlobalProcedures.LookUpEditValue(PurposeBusinessRelationshipLookUp, aml.PURPOSE_BUSINESS_NAME);
                SanctionCheckRadioGroup.SelectedIndex = aml.IS_SANCTION_CHECK;
                SanctionCheckURLText.EditValue = aml.SANCTION_CHECK_URL;
                WebsiteText.EditValue = aml.WEBSITE;
                NoteText.EditValue = aml.NOTE;
            }
            else
            {
                RiskDegreeLookUp.EditValue =
                    OrganizationLegalLookUp.EditValue =
                    PurposeBusinessRelationshipLookUp.EditValue =
                    OldCompanyNameText.Text =
                    SanctionCheckURLText.Text =
                    NoteText.Text =
                    RegistrationDateValue.Text =
                    DRCIssueDateValue.Text =
                    WebsiteText.Text = null;

                AuthorizedCapitalValue.EditValue =
                    EmployeesNumberValue.EditValue =
                    BranchCountValue.EditValue =
                    ValueAssetsValue.EditValue =
                    SanctionCheckRadioGroup.SelectedIndex = 0;

                transaction = "INSERT";
            }

            BRemove.Visible = transaction == "UPDATE";
        }

        private void RiskDegreeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            riskDegreeID = GlobalFunctions.GetLookUpID(sender);
        }

        private void LoadCustomerDetails()
        {
            string s = $@"SELECT USED_USER_ID
                              FROM CRS_USER.JURIDICAL_PERSONS C
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

        private void FJuridicalAML_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.JURIDICAL_PERSONS", -1, "WHERE ID = " + CustomerID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCustomersDataGridView();
        }

        private void LoadJuridicalFounder()
        {
            string sql = $@"SELECT F.ID,
                                   F.PARENT_ID,
                                   P.NAME ||' - '||F.NAME FOUNDER_NAME,
                                   F.PERSON_TYPE_ID,
                                   F.FOUNDER_SHARE || ' %',
                                   F.NAME
                              FROM CRS_USER_TEMP.JURIDICAL_AML_FOUNDER_TEMP F, CRS_USER.PERSON_TYPE P
                             WHERE F.JURIDICAL_PERSON_ID = {CustomerID} AND F.PERSON_TYPE_ID = P.ID AND IS_CHANGE != 2";

            TreeList.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadJuridicalFounder", "Təsisçilərin siyahısı açılmadı.");
            TreeList.PopulateColumns();
            TreeList.KeyFieldName = "ID";
            TreeList.ParentFieldName = "PARENT_ID";
            TreeList.Columns[0].Caption = "Təsisçinin adı";
            TreeList.Columns[1].Visible = false;
            TreeList.Columns[2].Caption = "Təsisçi payı";
            TreeList.Columns[2].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            TreeList.Columns[2].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            TreeList.Columns[3].Visible = false;
            TreeList.ExpandAll();
            this.BeginInvoke(new MethodInvoker(delegate
            {
                TreeList.BestFitColumns();
            }));

            EditFounderBarButton.Enabled = DeleteFounderBarButton.Enabled = SubNewFounderBarSubItem.Enabled = TreeList.AllNodesCount > 0;
        }

        private void LoadPhysicalFounderAddEdit(string transaction, int juridicalPersonID, int? id)
        {
            if (TreeList.FocusedNode != null)
                focusedID = TreeList.FocusedNode.Id;

            FPhysicalFounderAddEdit fPhysicalFounderAddEdit = new FPhysicalFounderAddEdit()
            {
                TransactionName = transaction,
                JuridicalPersonID = juridicalPersonID,
                ID = id
            };
            fPhysicalFounderAddEdit.RefreshDataGridView += new FPhysicalFounderAddEdit.DoEvent(LoadJuridicalFounder);
            fPhysicalFounderAddEdit.ShowDialog();
            TreeList.FocusedNode = TreeList.FindNodeByID(focusedID);
        }

        private void LoadJuridicalFounderAddEdit(string transaction, int juridicalPersonID, int? id)
        {
            if (TreeList.FocusedNode != null)
                focusedID = TreeList.FocusedNode.Id;

            FJuridicalFounderAddEdit fPhysicalFounderAddEdit = new FJuridicalFounderAddEdit()
            {
                TransactionName = transaction,
                JuridicalPersonID = juridicalPersonID,
                ID = id
            };
            fPhysicalFounderAddEdit.RefreshDataGridView += new FJuridicalFounderAddEdit.DoEvent(LoadJuridicalFounder);
            fPhysicalFounderAddEdit.ShowDialog();
            TreeList.FocusedNode = TreeList.FindNodeByID(focusedID);
        }

        private void SactionsHyperlinkLabelControl_HyperlinkClick(object sender, HyperlinkClickEventArgs e)
        {
            SactionsHyperlinkLabelControl.LinkVisited = true;
            System.Diagnostics.Process.Start(e.Link);
        }

        private void TreeList_DoubleClick(object sender, EventArgs e)
        {
            if (EditFounderBarButton.Enabled)
                EditFounder();
        }

        private void LoadSubJuridicalFounderAddEdit(int juridicalPersonID, int parentID, int? id)
        {
            FSubJuridicalFounderAddEdit fJuridicalFounderAddEdit = new FSubJuridicalFounderAddEdit()
            {
                JuridicalPersonID = juridicalPersonID,
                ParentFounderID = parentID,
                FounderName = founderName,
                ID = id
            };
            fJuridicalFounderAddEdit.RefreshDataGridView += new FSubJuridicalFounderAddEdit.DoEvent(LoadJuridicalFounder);
            fJuridicalFounderAddEdit.ShowDialog();
        }

        private void LoadSubPhysicalFounderAddEdit(int juridicalPersonID, int parentID, int? id)
        {
            FSubPhysicalFounderAddEdit fPhysicalFounderAddEdit = new FSubPhysicalFounderAddEdit()
            {
                JuridicalPersonID = juridicalPersonID,
                ParentFounderID = parentID,
                FounderName = founderName,
                ID = id
            };
            fPhysicalFounderAddEdit.RefreshDataGridView += new FSubPhysicalFounderAddEdit.DoEvent(LoadJuridicalFounder);
            fPhysicalFounderAddEdit.ShowDialog();
        }

        private void NewSubPhysicalFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (parentFounderID == 0)
            {
                GlobalProcedures.ShowWarningMessage("Alt təsisçini əlavə etmək üçün bir təsisçi seçin");
                return;
            }

            LoadSubPhysicalFounderAddEdit(CustomerID, parentFounderID, null);
        }

        private void NewSubJuridicalFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (parentFounderID == 0)
            {
                GlobalProcedures.ShowWarningMessage("Alt təsisçini əlavə etmək üçün bir təsisçi seçin");
                return;
            }

            LoadSubJuridicalFounderAddEdit(CustomerID, parentFounderID, null);
        }

        private void DeleteFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalFunctions.CallDialogResult("Seçilmiş təsisçini silmək istəyirsiniz?", "Təsisçinin silinməsi") == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("COMS_USER_TEMP.PROC_DEL_JUR_FOUNDER_TEMP", "P_ID", founderID, "Təsisçi silinmədi");
                LoadJuridicalFounder();
            }
        }

        private void RefreshFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadJuridicalFounder();
        }

        void LoadPurposeBusinessRelationship()
        {
            GlobalProcedures.FillLookUpEdit(PurposeBusinessRelationshipLookUp, "PURPOSE_BUSINESS_RELATIONSHIP", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
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

        private void PurposeBusinessRelationshipLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFPurposeBusinessAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && purposeBusinessRelationshipID != 0)
                LoadFPurposeBusinessAddEdit("EDIT", purposeBusinessRelationshipID);
        }

        private void InsertCustomerAML(OracleTransaction tran)
        {
            string sql = $@"INSERT INTO CRS_USER.JURIDICAL_PERSON_AML(JURIDICAL_PERSON_ID,
                                                                       RISK_DEGREE_ID,
                                                                       PURPOSE_BUSINESS_RELATIONSHIP,
                                                                       IS_SANCTION_CHECK,
                                                                       SANCTION_CHECK_URL,
                                                                       ORGANIZATIONAL_LEGAL_ID,
                                                                       REGISTRATION_DATE,
                                                                       AUTHORIZED_CAPITAL,
                                                                       DRC_ISSUE_DATE,
                                                                       EMPLOYEES_NUMBER,
                                                                       BRANCH_COUNT,
                                                                       VALUE_ASSETS,
                                                                       OLD_COMPANY_NAME,  
                                                                       WEBSITE,
                                                                       NOTE,
                                                                       INSERT_USER)
                        VALUES({CustomerID},
                               {riskDegreeID},
                               {purposeBusinessRelationshipID},
                               {SanctionCheckRadioGroup.SelectedIndex},
                               '{SanctionCheckURLText.Text.Trim()}',      
                               {organizationLegalID},
                               TO_DATE('{RegistrationDateValue.Text}','DD.MM.YYYY'),
                               {AuthorizedCapitalValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                               TO_DATE('{DRCIssueDateValue.Text}','DD.MM.YYYY'),
                               {EmployeesNumberValue.Value},
                               {BranchCountValue.Value},
                               {ValueAssetsValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                               '{OldCompanyNameText.Text.Trim()}',
                               '{WebsiteText.Text.Trim()}',
                               '{NoteText.Text.Trim()}',
                               {GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(tran, sql);
        }

        private void UpdateCustomerAML(OracleTransaction tran)
        {
            string sql = $@"UPDATE CRS_USER.JURIDICAL_PERSON_AML SET RISK_DEGREE_ID = {riskDegreeID},
                                                                       PURPOSE_BUSINESS_RELATIONSHIP = {purposeBusinessRelationshipID},
                                                                       IS_SANCTION_CHECK = {SanctionCheckRadioGroup.SelectedIndex},
                                                                       SANCTION_CHECK_URL = '{SanctionCheckURLText.Text.Trim()}',  
                                                                       ORGANIZATIONAL_LEGAL_ID = {organizationLegalID},
                                                                       REGISTRATION_DATE = TO_DATE('{RegistrationDateValue.Text}','DD.MM.YYYY'),
                                                                       AUTHORIZED_CAPITAL = {AuthorizedCapitalValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                       DRC_ISSUE_DATE = TO_DATE('{DRCIssueDateValue.Text}','DD.MM.YYYY'),
                                                                       EMPLOYEES_NUMBER = {EmployeesNumberValue.Value},
                                                                       BRANCH_COUNT = {BranchCountValue.Value},
                                                                       VALUE_ASSETS = {ValueAssetsValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                       OLD_COMPANY_NAME = '{OldCompanyNameText.Text.Trim()}',
                                                                       WEBSITE = '{WebsiteText.Text.Trim()}',
                                                                       NOTE = '{NoteText.Text.Trim()}',
                                                                       UPDATE_USER = {GlobalVariables.V_UserID},
                                                                       UPDATE_DATE = SYSDATE
                        WHERE JURIDICAL_PERSON_ID = {CustomerID}";

            GlobalProcedures.ExecuteQuery(tran, sql);
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (riskDegreeID == 0)
            {
                RiskDegreeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("AML üçün müştərinin riks dərəcəsi seçilməyib.");
                RiskDegreeLookUp.Focus();
                RiskDegreeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (purposeBusinessRelationshipID == 0)
            {
                PurposeBusinessRelationshipLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("AML üçün müştərinin işgüzar münasibətin məqsədi seçilməyib.");
                PurposeBusinessRelationshipLookUp.Focus();
                PurposeBusinessRelationshipLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (organizationLegalID == 0)
            {
                OrganizationLegalLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("AML üçün təşkilatın hüquqi forması seçilməyib.");
                OrganizationLegalLookUp.Focus();
                OrganizationLegalLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (RegistrationDateValue.Text.Length == 0)
            {
                RegistrationDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("AML üçün təşkilatın qeydiyyat tarixi seçilməyib.");
                RegistrationDateValue.Focus();
                RegistrationDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AuthorizedCapitalValue.Value <= 0)
            {
                AuthorizedCapitalValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("AML üçün təşkilatın nizamnamə kapitalı sıfırdan böyük olmalıdır.");
                AuthorizedCapitalValue.Focus();
                AuthorizedCapitalValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DRCIssueDateValue.Text.Length == 0)
            {
                DRCIssueDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("AML üçün DRÇ verilmə tarixi seçilməyib.");
                DRCIssueDateValue.Focus();
                DRCIssueDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ValueAssetsValue.Value <= 0)
            {
                ValueAssetsValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("AML üçün təşkilatın aktivlərinin dəyəri sıfırdan böyük olmalıdır.");
                ValueAssetsValue.Focus();
                ValueAssetsValue.BackColor = GlobalFunctions.ElementColor();
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

                    GlobalProcedures.ExecuteProcedureWithTwoParametr(tran, "CRS_USER.PROC_INSERT_JP_FOUNDER", "P_CUSTOMER_ID", CustomerID, "P_USED_USER_ID", GlobalVariables.V_UserID);
                    return 1;
                }, transaction == "INSERT"? "Müştərinin AML məlumatları bazaya daxil edilmədi." : "Müştərinin AML məlumatları bazada dəyişdirilmədi.");

                GlobalProcedures.SplashScreenClose();
                this.Close();
            }
        }

        private void NewPhysicalBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhysicalFounderAddEdit("INSERT", CustomerID, null);
        }

        private void BRemove_Click(object sender, EventArgs e)
        {
            if (GlobalFunctions.CallDialogResult("AML-ni silmək istəyirsiniz?", "AML-nin silinməsi") == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DELETE_JURIDICAL_AML", "P_CUSTOMER_ID", CustomerID, "AML silinmədi");
                LoadCustomerAML();
                LoadJuridicalFounder();
            }
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

        void EditFounder()
        {
            if (founderPersonTypeID == (int)PersonTypeEnum.Fiziki)
                LoadPhysicalFounderAddEdit("EDIT", CustomerID, founderID);
            else
                LoadJuridicalFounderAddEdit("EDIT", CustomerID, founderID);
        }

        private void TreeList_Click(object sender, EventArgs e)
        {
            TreeList tree = sender as TreeList;
            TreeListHitInfo hi = tree.CalcHitInfo(tree.PointToClient(Control.MousePosition));
            if (hi.Node != null)
            {
                object selNode = hi.Node.GetValue(tree.KeyFieldName);
                parentFounderID = int.Parse(selNode.ToString());
                founderID = int.Parse(selNode.ToString());
                founderName = hi.Node.GetValue("NAME").ToString();
                founderPersonTypeID = int.Parse(hi.Node.GetValue("PERSON_TYPE_ID").ToString());
            }
        }

        private void EditFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            EditFounder();
        }

        private void OrganizationLegalLookUp_EditValueChanged(object sender, EventArgs e)
        {
            organizationLegalID = GlobalFunctions.GetLookUpID(sender);
        }

        private void PurposeBusinessRelationshipLookUp_EditValueChanged(object sender, EventArgs e)
        {
            purposeBusinessRelationshipID = GlobalFunctions.GetLookUpID(sender);
        }

        private void NewJuridicalBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadJuridicalFounderAddEdit("INSERT", CustomerID, null);
        }

        void LoadRiskDegree()
        {
            GlobalProcedures.FillLookUpEdit(RiskDegreeLookUp, "RISK_DEGREE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
            this.RefreshCustomersDataGridView();
        }
    }
}