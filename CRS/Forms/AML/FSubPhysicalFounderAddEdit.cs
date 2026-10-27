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
using static CRS.Class.Enum;
using CRS.Class.Views;
using CRS.Class.DataAccess;

namespace CRS.Forms.AML
{
    public partial class FSubPhysicalFounderAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FSubPhysicalFounderAddEdit()
        {
            InitializeComponent();
        }
        public string FounderName;
        public int JuridicalPersonID, ParentFounderID;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int localPoliticalDutyID = 0,
            foreignPoliticalDutyID = 0,
            relativePoliticalDutyID = 0,
            customerBehaviorID = 0,
            customerIdentificationMethodID = 0,
            birthplaceID = 0,
            countryID = 0,
            documentCountryID = 0;

        private void FPhysicalFounderAddEdit_Load(object sender, EventArgs e)
        {
            FounderNameText.EditValue = FounderName;
            LoadCountry();
            LoadCustomerBehavior();
            LoadCustomerIdentificationMethod();
            LoadPoliticalDuty();
            LoadBirthplace();
            BirthdayDate.Properties.MaxValue = DateOfIssueDocument.Properties.MaxValue = DateTime.Today;
            LocalPoliticalFigureRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
            ForeignPoliticalFigureRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
            RelativePoliticalFigureRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
            SanctionCheckRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
        }        

        private void InsertFounder()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.JURIDICAL_AML_FOUNDER_TEMP(ID,
                                                                                  JURIDICAL_PERSON_ID,
                                                                                  PARENT_ID,
                                                                                  PERSON_TYPE_ID,
                                                                                  FOUNDER_SHARE,                                                                                    
                                                                                  NAME,
                                                                                  PINCODE,
                                                                                  DOCUMENT_NUMBER,
                                                                                  DOCUMENT_ISSUE_DATE,
                                                                                  DOCUMENT_RELIABLE_DATE,
                                                                                  DOCUMENT_COUNTRY_ID,
                                                                                  REGISTRATION_ADDRESS,
                                                                                  BIRTHDAY,
                                                                                  BIRTHPLACE_ID,
                                                                                  COUNTRY_ID,
                                                                                  VOEN,
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
                                                                                  IS_SANCTION_CHECK,
                                                                                  SANCTION_CHECK_URL,
                                                                                  NOTE,
                                                                                  IS_CHANGE,
                                                                                  USED_USER_ID)
                                    VALUES(CRS_USER.JURIDICAL_FOUNDER_SEQUENCE.NEXTVAL,
                                           {JuridicalPersonID},
                                           {ParentFounderID},
                                           {(int)PersonTypeEnum.Fiziki},
                                           {Math.Round(FounderShareValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                           '{FullNameText.Text.Trim()}',
                                           '{PinCodeText.Text.Trim()}',
                                           '{DocumentNumberText.Text.Trim()}',
                                           TO_DATE('{DateOfIssueDocument.Text}','DD/MM/YYYY'),
                                           TO_DATE('{ReliableDateDocument.Text}','DD/MM/YYYY'),
                                           {documentCountryID},
                                           '{RegistrationAddressText.Text.Trim()}',
                                           TO_DATE('{BirthdayDate.Text}','DD/MM/YYYY'),
                                           {birthplaceID},
                                           {countryID},
                                           '{VoenText.Text.Trim()}',
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
                                           {SanctionCheckRadioGroup.SelectedIndex},
                                           '{SanctionCheckURLText.Text.Trim()}',    
                                           '{NoteText.Text.Trim()}',
                                           1,
                                           {GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Təsisçi temp cədvələ daxil edilmədi.", this.Name + "/InsertFounder");
        }

        private void FPhysicalFounderAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (FullNameText.Text.Length == 0)
            {
                FullNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təsisçinin adı daxil edilməyib.");
                FullNameText.Focus();
                FullNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PinCodeText.Text.Length != 7)
            {
                PinCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Fin kod 7 simvol olmalıdır.");
                PinCodeText.Focus();
                PinCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DocumentNumberText.Text.Length == 0)
            {
                DocumentNumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin nömrəsi daxil edilməyib.");
                DocumentNumberText.Focus();
                DocumentNumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DateOfIssueDocument.Text.Length == 0)
            {
                DateOfIssueDocument.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi daxil edilməyib.");
                DateOfIssueDocument.Focus();
                DateOfIssueDocument.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ReliableDateDocument.Text.Length == 0)
            {
                ReliableDateDocument.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin bitmə tarixi daxil edilməyib.");
                ReliableDateDocument.Focus();
                ReliableDateDocument.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (documentCountryID == 0)
            {
                DocumentCountryLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədi verən ölkə seçilməyib.");
                DocumentCountryLookUp.Focus();
                DocumentCountryLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (RegistrationAddressText.Text.Length == 0)
            {
                RegistrationAddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qeydiyyatda olduğu ünvan daxil edilməyib.");
                RegistrationAddressText.Focus();
                RegistrationAddressText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BirthdayDate.Text.Length == 0)
            {
                BirthdayDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Doğum tarixi daxil edilməyib.");
                BirthdayDate.Focus();
                BirthdayDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (birthplaceID == 0)
            {
                BirthplaceLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Doğum yeri seçilməyib.");
                BirthplaceLookUp.Focus();
                BirthplaceLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (countryID == 0)
            {
                CountryLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Vətəndaşlığı seçilməyib.");
                CountryLookUp.Focus();
                CountryLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (FounderShareValue.Value <= 0)
            {
                FounderShareValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təsisçi payı sıfırdan böyük olmalıdır.");
                FounderShareValue.Focus();
                FounderShareValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length > 0 && !GlobalFunctions.Regexp("[0-9]{9}2", VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Fiziki şəxsin vöen-i yalnız rəqəmlərdən ibarət olmalıdır, ümumi uzunluğu 10 simvol olmalıdır və sonuncu simvol <b><color=104,0,0>2</color></b> olmalıdır.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor();
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
                GlobalProcedures.ShowErrorMessage("AML üçün müştərinin davranışı seçilməyib.");
                CustomerBehaviorLookUp.Focus();
                CustomerBehaviorLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (customerIdentificationMethodID == 0)
            {
                CustomerIdentificationMethodLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("AML üçün müştərinin identifikasiya üsulu seçilməyib.");
                CustomerIdentificationMethodLookUp.Focus();
                CustomerIdentificationMethodLookUp.BackColor = GlobalFunctions.ElementColor();
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
                InsertFounder();                
                this.Close();
            }
        }

        void LoadBirthplace()
        {
            GlobalProcedures.FillLookUpEdit(BirthplaceLookUp, "BIRTHPLACE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        void LoadCountry()
        {
            GlobalProcedures.FillLookUpEdit(CountryLookUp, "COUNTRIES", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillLookUpEdit(DocumentCountryLookUp, "COUNTRIES", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        private void LoadFCountryAddEdit(string transaction, string countryid)
        {
            Dictionaries.FCountryAddEdit fp = new Dictionaries.FCountryAddEdit
            {
                TransactionName = transaction,
                CountryID = countryid
            };
            fp.RefreshCountriesDataGridView += new Dictionaries.FCountryAddEdit.DoEvent(LoadCountry);
            fp.ShowDialog();
        }

        private void LoadFBirthplaceAddEdit(string transaction, string BirthplaceID)
        {
            Dictionaries.FBirthplaceAddEdit fse = new Dictionaries.FBirthplaceAddEdit
            {
                TransactionName = transaction,
                BirthplaceID = BirthplaceID
            };
            fse.RefreshBirthplaceDataGridView += new Dictionaries.FBirthplaceAddEdit.DoEvent(LoadBirthplace);
            fse.ShowDialog();
        }

        private void CountryLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFCountryAddEdit("INSERT", null);
            else
                LoadFCountryAddEdit("EDIT", countryID.ToString());
        }

        private void BirthplaceLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFBirthplaceAddEdit("INSERT", null);
            else
                LoadFBirthplaceAddEdit("EDIT", birthplaceID.ToString());
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

        private void LocalPoliticalDutyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFPoliticalDutyAddEdit("INSERT", null);
            else if (e.Button.Index == 2 && localPoliticalDutyID != 0)
                LoadFPoliticalDutyAddEdit("EDIT", localPoliticalDutyID);
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

        private void ForeignPoliticalDutyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            foreignPoliticalDutyID = GlobalFunctions.GetLookUpID(sender);
        }

        private void RelativePoliticalFigureLookUp_EditValueChanged(object sender, EventArgs e)
        {
            relativePoliticalDutyID = GlobalFunctions.GetLookUpID(sender);
        }

        private void CustomerBehaviorLookUp_EditValueChanged(object sender, EventArgs e)
        {
            customerBehaviorID = GlobalFunctions.GetLookUpID(sender);
        }

        private void CustomerIdentificationMethodLookUp_EditValueChanged(object sender, EventArgs e)
        {
            customerIdentificationMethodID = GlobalFunctions.GetLookUpID(sender);
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

        private void SactionsHyperlinkLabelControl_HyperlinkClick(object sender, DevExpress.Utils.HyperlinkClickEventArgs e)
        {
            SactionsHyperlinkLabelControl.LinkVisited = true;
            System.Diagnostics.Process.Start(e.Link);
        }

        private void DateOfIssueDocument_EditValueChanged(object sender, EventArgs e)
        {
            ReliableDateDocument.Properties.MinValue = DateOfIssueDocument.DateTime;
        }

        private void VoenText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(sender, VoenLengthLabel);
        }

        private void PinCodeText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(sender, PinCodeLengthLabel);
        }

        private void DocumentNumberText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(sender, NumberLengthLabel);
        }

        private void BirthdayDate_EditValueChanged(object sender, EventArgs e)
        {
            AgeLabel.Text = GlobalFunctions.CalculationAgeWithYear(BirthdayDate.DateTime, DateTime.Today);
        }

        private void LocalPoliticalDutyFromDate_EditValueChanged(object sender, EventArgs e)
        {
            LocalPoliticalDutyToDate.Properties.MinValue = LocalPoliticalDutyFromDate.DateTime;
        }

        private void ForeignPoliticalDutyFromDate_EditValueChanged(object sender, EventArgs e)
        {
            ForeignPoliticalDutyToDate.Properties.MinValue = ForeignPoliticalDutyFromDate.DateTime;
        }

        private void DocumentCountryLookUp_EditValueChanged(object sender, EventArgs e)
        {
            documentCountryID = GlobalFunctions.GetLookUpID(sender);
        }

        private void RelativePoliticalDutyFromDate_EditValueChanged(object sender, EventArgs e)
        {
            RelativePoliticalDutyToDate.Properties.MinValue = RelativePoliticalDutyFromDate.DateTime;
        }

        private void CountryLookUp_EditValueChanged(object sender, EventArgs e)
        {
            countryID = GlobalFunctions.GetLookUpID(sender);
        }

        private void BirthplaceLookUp_EditValueChanged(object sender, EventArgs e)
        {
            birthplaceID = GlobalFunctions.GetLookUpID(sender);
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

        private void RelativePoliticalFigureRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            RelativeNameText.ReadOnly = RelativePoliticalDutyFromDate.ReadOnly = RelativePoliticalDutyToDate.ReadOnly = RelativePoliticalFigureLookUp.ReadOnly = RelativePoliticalFigureRadioGroup.SelectedIndex == 0;
            if (RelativePoliticalFigureRadioGroup.SelectedIndex == 0)
            {
                RelativeNameText.Text =
                    RelativePoliticalDutyFromDate.Text =
                    RelativePoliticalDutyToDate.Text =
                    RelativePoliticalFigureLookUp.Text = null;
                RelativePoliticalFigureLookUp.EditValue = null;
                RelativeStarLabel.Visible = RelativePoliticalStarLabel.Visible = false;
            }
            else
                RelativeStarLabel.Visible = RelativePoliticalStarLabel.Visible = true;
        }

        private void LocalPoliticalDutyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            localPoliticalDutyID = GlobalFunctions.GetLookUpID(sender);
        }
    }
}