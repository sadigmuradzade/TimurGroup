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
using Oracle.ManagedDataAccess.Client;

namespace CRS.Forms.Contracts
{
    public partial class FGuaranteeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FGuaranteeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CustomerName;
        public int ContractID;
        public int? ID;

        int cardIssuingID = 0,
            sexID = 0,
            birthplaceID = 0,
            countryID = 0,
            phoneID = 0,
            code = 1;
        string phoneNumber = null;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FGuaranteeAddEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.FillLookUpEdit(SexLookUp, "SEX", "ID", "NAME", "1 = 1 ORDER BY ID");
            DateOfBirthValue.Properties.MaxValue = DateTime.Today;
            RefreshDictionaries(3);
            RefreshDictionaries(4);
            RefreshDictionaries(13);
            DateOfIssueDate.Properties.MaxValue = DateTime.Today;
            if (TransactionName == "INSERT")
            {
                ID = GlobalFunctions.GetOracleSequenceValue("GUARANTEE_SEQUENCE");
                List<Guarantee> guarantees = GuaranteeDAL.SelectGuarantee(1, ContractID).ToList<Guarantee>();
                if (guarantees.Count > 0)
                    code = guarantees.LastOrDefault().CODE + 1;

                CodeText.EditValue = code.ToString().PadLeft(2, '0');
                DateOfBirthValue.Properties.MaxValue = DateTime.Today.AddYears(-18);
            }
            else
                LoadDetails();
            LoadPhoneDataGridView();
        }

        private void LoadDetails()
        {
            string sql = $@"SELECT LPAD (G.CODE, 2, '0') CODE,
                                       G.CARD_NUMBER,
                                       G.CARD_PINCODE,
                                       G.NAME,
                                       G.BIRTHDAY,
                                       B.NAME BIRTHPLACE_NAME,
                                       S.NAME SEX_NAME,
                                       G.CARD_NUMBER,
                                       G.CARD_PINCODE,
                                       G.CARD_ISSUE_DATE,
                                       G.RELIABLE_DATE,
                                       CI.NAME ISSUING_NAME,
                                       G.NOTE,
                                       G.ADDRESS,
                                       G.REGISTRATION_ADDRESS,
                                       G.USED_USER_ID,
                                       G.VOEN
                                  FROM CRS_USER_TEMP.GUARANTEE_TEMP G,
                                       CRS_USER.SEX S,
                                       CRS_USER.BIRTHPLACE B,
                                       CRS_USER.CARD_ISSUING CI
                                 WHERE     G.SEX_ID = S.ID
                                       AND G.BIRTHPLACE_ID = B.ID
                                       AND G.CARD_ISSUING_ID = CI.ID                                       
                                       AND G.CONTRACT_ID = {ContractID}
                                       AND G.ID = {ID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", "Zaminin məlumatları açılmadı.");

            if (dt.Rows.Count > 0)
            {
                CodeText.EditValue = dt.Rows[0]["CODE"];
                NameText.EditValue = dt.Rows[0]["NAME"];
                GlobalProcedures.LookUpEditValue(SexLookUp, dt.Rows[0]["SEX_NAME"].ToString());
                DateOfBirthValue.EditValue = dt.Rows[0]["BIRTHDAY"];
                GlobalProcedures.LookUpEditValue(BirthplaceLookUp, dt.Rows[0]["BIRTHPLACE_NAME"].ToString());
                NumberText.EditValue = dt.Rows[0]["CARD_NUMBER"];
                PinCodeText.EditValue = dt.Rows[0]["CARD_PINCODE"];
                DateOfIssueDate.EditValue = dt.Rows[0]["CARD_ISSUE_DATE"];
                ReliableDate.EditValue = dt.Rows[0]["RELIABLE_DATE"];
                GlobalProcedures.LookUpEditValue(IssuingLookUp, dt.Rows[0]["ISSUING_NAME"].ToString());
                RegistrationAddressText.EditValue = dt.Rows[0]["REGISTRATION_ADDRESS"];
                AddressText.EditValue = dt.Rows[0]["ADDRESS"];
                NoteText.EditValue = dt.Rows[0]["NOTE"];
                VoenText.EditValue = dt.Rows[0]["VOEN"];
            }
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PopupMenu, e);
        }

        private void PhoneGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Phone_SS, e);
        }

        private void FGuaranteeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void IssuingLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (IssuingLookUp.EditValue == null)
                return;

            cardIssuingID = Convert.ToInt32(IssuingLookUp.EditValue);
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Zaminin adı daxil edilməyib. Adın uzunluğu maksimum 80 simvol, yalnız latın hərfləri, rəqəm və defis işarəsindən ibarət ola bilər.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (!GlobalFunctions.Regexp("[0-9A-Za-z-]{1,80}", NameText.Text.Trim()) || NameText.Text.Length > 80 || NameText.Text.IndexOf("w") > -1 || NameText.Text.IndexOf("W") > -1)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Zaminin adı düz deyil. Adın uzunluğu maksimum 80 simvol, yalnız latın hərfləri, rəqəm və defis işarəsindən ibarət ola bilər.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (sexID == 0)
            {
                SexLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cins seçilməyib.");
                SexLookUp.Focus();
                SexLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DateOfBirthValue.Text.Length == 0)
            {
                DateOfBirthValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Zaminin doğum tarixi seçilməyib.");
                DateOfBirthValue.Focus();
                DateOfBirthValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (birthplaceID == 0)
            {
                BirthplaceLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Zaminin doğulduğu yer seçilməyib.");
                BirthplaceLookUp.Focus();
                BirthplaceLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NumberText.Text.Length == 0)
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin seria və nömrəsi daxil edilməyib.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (NumberText.Text.Substring(0, 3) == "AZE" && NumberText.Text.Length != 11)
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin seria və nömrəsinin uzunluğu 11 simvol olmalıdır.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (PinCodeText.Text.Length != 7)
            {
                PinCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Fin kod 7 simvol olmalıdır.");
                PinCodeText.Focus();
                PinCodeText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(DateOfIssueDate.Text))
            {
                DateOfIssueDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi daxil edilməyib.");
                DateOfIssueDate.Focus();
                DateOfIssueDate.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else if (String.IsNullOrEmpty(ReliableDate.Text))
            {
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin etibarlı olma tarixi daxil edilməyib.");
                ReliableDate.Focus();
                ReliableDate.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else if (GlobalFunctions.ChangeStringToDate(DateOfIssueDate.Text, "ddmmyyyy") == GlobalFunctions.ChangeStringToDate(ReliableDate.Text, "ddmmyyyy"))
            {
                DateOfIssueDate.BackColor = Color.Red;
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi ilə etibarlı olma tarixi eyni ola bilməz.");
                DateOfIssueDate.Focus();
                DateOfIssueDate.BackColor = GlobalFunctions.ElementColor(); ;
                ReliableDate.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (cardIssuingID == 0)
            {
                IssuingLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədi verən orqanın adı seçilməyib.");
                IssuingLookUp.Focus();
                IssuingLookUp.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (RegistrationAddressText.Text.Length == 0)
            {
                RegistrationAddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qeydiyyatda olduğu ünvan daxil edilməyib.");
                RegistrationAddressText.Focus();
                RegistrationAddressText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length > 0 && !GlobalFunctions.Regexp("[0-9]{9}1", VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Fiziki şəxsin vöen-i yalnız rəqəmlərdən ibarət olmalıdır, ümumi uzunluğu 10 simvol olmalıdır və sonuncu simvol <b><color=104,0,0>2</color></b> olmalıdır.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (PhoneGridView.RowCount == 0)
            {
                PhoneGridControl.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Telefon nömrələri daxil edilməyib.");
                PhoneGridControl.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BirthplaceLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BirthplaceLookUp.EditValue == null)
                return;

            birthplaceID = Convert.ToInt32(BirthplaceLookUp.EditValue);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FWait));
                GlobalFunctions.RunInOneTransaction<int>(tran =>
                {
                    if (TransactionName == "INSERT")
                        InsertGuarantee(tran);
                    else
                        UpdateGuarantee(tran);

                    return 1;
                }, TransactionName == "INSERT" ? "Zaminin məlumatları temp cədvələ daxil edilmədi." : "Zaminin məlumatları temp cədvəldə dəyişdirilmədi.");
                GlobalProcedures.SplashScreenClose();
                this.Close();
            }
        }

        private void SexLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SexLookUp.EditValue == null)
                return;

            sexID = Convert.ToInt32(SexLookUp.EditValue);
        }

        private void NameText_EditValueChanged(object sender, EventArgs e)
        {
            if (NameText.EditorContainsFocus)
                if (NameText.Text.IndexOf("oğlu") > -1)
                    GlobalProcedures.LookUpEditValue(SexLookUp, "Kişi");
                else if (NameText.Text.IndexOf("qızı") > -1)
                    GlobalProcedures.LookUpEditValue(SexLookUp, "Qadın");
                else
                    GlobalProcedures.LookUpEditValue(SexLookUp, null);

            GlobalProcedures.TextEditCharCount(NameText, NameLengthLabel);
        }

        private void BirthplaceLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 4);
        }

        private void CountryLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 13);
        }

        private void IssuingLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 3);
        }

        private void LoadPhoneDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 P.ID,
                                 PD.DESCRIPTION_AZ DESCRIPTION,
                                 '+' || C.CODE || P.PHONE_NUMBER PHONENUMBER,
                                 P.IS_SEND_SMS,
                                 KR.NAME KINDSHIP_RATE_NAME
                            FROM CRS_USER_TEMP.PHONES_TEMP P,
                                 CRS_USER.PHONE_DESCRIPTIONS PD,
                                 CRS_USER.COUNTRIES C,
                                 CRS_USER.KINDSHIP_RATE KR
                           WHERE     P.IS_CHANGE IN (0, 1)
                                 AND P.PHONE_DESCRIPTION_ID = PD.ID
                                 AND P.COUNTRY_ID = C.ID
                                 AND P.KINDSHIP_RATE_ID = KR.ID (+)
                                 AND P.OWNER_TYPE = 'G'
                                 AND P.OWNER_ID = {ID}
                        ORDER BY P.ORDER_ID";
            try
            {
                PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");
                if (PhoneGridView.RowCount > 0)
                {
                    EditPhoneBarButton.Enabled =
                        DeletePhoneBarButton.Enabled = true;

                    try
                    {
                        PhoneGridView.BeginUpdate();
                        for (int i = 0; i < PhoneGridView.RowCount; i++)
                        {
                            DataRow row = PhoneGridView.GetDataRow(PhoneGridView.GetVisibleRowHandle(i));
                            if (Convert.ToInt32(row["IS_SEND_SMS"].ToString()) == 1)
                                PhoneGridView.SelectRow(i);
                        }
                    }
                    finally
                    {
                        PhoneGridView.EndUpdate();
                    }
                }
                else
                    EditPhoneBarButton.Enabled =
                        DeletePhoneBarButton.Enabled = false;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Telefon nömrələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public void LoadFPhoneAddEdit(string transaction, int OwnerID, string OwnerType, string PhoneID)
        {
            FPhoneAddEdit fp = new FPhoneAddEdit();
            fp.TransactionName = transaction;
            fp.OwnerID = OwnerID.ToString();
            fp.OwnerType = OwnerType;
            fp.PhoneID = PhoneID;
            fp.RefreshPhonesDataGridView += new FPhoneAddEdit.DoEvent(LoadPhoneDataGridView);
            fp.ShowDialog();
        }

        private void PhoneGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PhoneGridView.GetFocusedDataRow();
            if (row != null)
            {
                phoneID = Convert.ToInt32(row["ID"]);
                phoneNumber = row["PHONENUMBER"].ToString();
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", ID.Value, "G", phoneID.ToString());
        }

        private void DeletePhone()
        {
            DialogResult dialogResult = XtraMessageBox.Show(phoneNumber + " nömrəsini silmək istəyirsiniz?", "Telefon nömrəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'G' AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {phoneID}",
                                                "Telefon nömrəsi temp cədvəldən silinmədi.",
                                                this.Name + "/DeletePhone");
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhone();
            LoadPhoneDataGridView();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhoneDataGridView();
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFPhoneAddEdit("EDIT", ID.Value, "G", phoneID.ToString());
        }

        private void NumberText_EditValueChanged(object sender, EventArgs e)
        {
            if (NumberText.Text.Trim().Length == 0)
                NumberLengthLabel.Visible = false;
            else
            {
                NumberLengthLabel.Visible = true;
                NumberLengthLabel.Text = NumberText.Text.Trim().Length.ToString();
            }
        }

        private void PinCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (PinCodeText.Text.Trim().Length == 0)
                PinCodeLengthLabel.Visible = false;
            else
            {
                PinCodeLengthLabel.Visible = true;
                PinCodeLengthLabel.Text = PinCodeText.Text.Trim().Length.ToString();
            }
        }

        private void DateOfIssueDate_EditValueChanged(object sender, EventArgs e)
        {
            ReliableDate.Properties.MinValue = DateOfIssueDate.DateTime;
        }

        private void VoenText_EditValueChanged(object sender, EventArgs e)
        {
            if (VoenText.Text.Trim().Length == 0)
                VoenLengthLabel.Visible = false;
            else
            {
                VoenLengthLabel.Visible = true;
                VoenLengthLabel.Text = VoenText.Text.Trim().Length.ToString();
            }
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("INSERT", ID.Value, "G", null);
        }

        private void InsertGuarantee(OracleTransaction tran)
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.GUARANTEE_TEMP (ID,
                                                                         CONTRACT_ID,
                                                                         CODE,
                                                                         CUSTOMER_NAME,
                                                                         NAME,
                                                                         SEX_ID,
                                                                         COUNTRY_ID,
                                                                         BIRTHDAY,
                                                                         BIRTHPLACE_ID,
                                                                         CARD_NUMBER,
                                                                         CARD_PINCODE,
                                                                         CARD_ISSUE_DATE,
                                                                         RELIABLE_DATE,
                                                                         CARD_ISSUING_ID,
                                                                         ADDRESS,
                                                                         REGISTRATION_ADDRESS,
                                                                         USED_USER_ID,
                                                                         NOTE,
                                                                         VOEN,
                                                                         LEADING_NAME,
                                                                         IS_CHANGE)
                                VALUES({ID},
                                       {ContractID},
                                       {code},
                                       '{CustomerName}',
                                       '{NameText.Text.Trim()}',
                                       {sexID},
                                       {countryID},
                                       TO_DATE('{DateOfBirthValue.Text}','DD.MM.YYYY'),
                                       {birthplaceID},                                      
                                       '{NumberText.Text.Trim()}',
                                       '{PinCodeText.Text.Trim()}',
                                       TO_DATE('{DateOfIssueDate.Text}','DD.MM.YYYY'),
                                       TO_DATE('{ReliableDate.Text}','DD.MM.YYYY'),
                                       {cardIssuingID},
                                       '{AddressText.Text.Trim()}',
                                       '{RegistrationAddressText.Text.Trim()}',
                                       {GlobalVariables.V_UserID},
                                       '{NoteText.Text.Trim()}',
                                       '{VoenText.Text.Trim()}',
                                       '{NameText.Text.Trim()}',
                                       1)";

            GlobalProcedures.ExecuteQuery(tran, sql);
        }

        private void UpdateGuarantee(OracleTransaction tran)
        {
            string sql = $@"UPDATE CRS_USER_TEMP.GUARANTEE_TEMP SET NAME = '{NameText.Text.Trim()}',
                                                                         SEX_ID = {sexID},
                                                                         COUNTRY_ID = {countryID},
                                                                         BIRTHDAY = TO_DATE('{DateOfBirthValue.Text}','DD.MM.YYYY'),
                                                                         BIRTHPLACE_ID = {birthplaceID},                                                                         
                                                                         CARD_NUMBER = '{NumberText.Text.Trim()}',
                                                                         CARD_PINCODE = '{PinCodeText.Text.Trim()}',
                                                                         CARD_ISSUE_DATE = TO_DATE('{DateOfIssueDate.Text}','DD.MM.YYYY'),
                                                                         RELIABLE_DATE = TO_DATE('{ReliableDate.Text}','DD.MM.YYYY'),
                                                                         CARD_ISSUING_ID = {cardIssuingID},
                                                                         ADDRESS = '{AddressText.Text.Trim()}',
                                                                         REGISTRATION_ADDRESS = '{RegistrationAddressText.Text.Trim()}',
                                                                         USED_USER_ID = {GlobalVariables.V_UserID},
                                                                         NOTE = '{NoteText.Text.Trim()}',
                                                                         VOEN = '{VoenText.Text.Trim()}',
                                                                         LEADING_NAME = '{NameText.Text.Trim()}',
                                                                         IS_CHANGE = 1
                                WHERE ID = {ID} AND CONTRACT_ID = {ContractID}";

            GlobalProcedures.ExecuteQuery(tran, sql);
        }

        private void DateOfBirthValue_EditValueChanged(object sender, EventArgs e)
        {
            AgeLabel.Text = GlobalFunctions.CalculationAgeWithYear(DateOfBirthValue.DateTime, DateTime.Today);
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 3:
                    GlobalProcedures.FillLookUpEdit(IssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 4:
                    GlobalProcedures.FillLookUpEdit(BirthplaceLookUp, "BIRTHPLACE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }
    }
}