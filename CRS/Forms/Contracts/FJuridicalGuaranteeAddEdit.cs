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
    public partial class FJuridicalGuaranteeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FJuridicalGuaranteeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CustomerName;
        public int ContractID;
        public int? ID;

        int phoneID = 0,
            code = 1;
        string phoneNumber = null;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FGuaranteeAddEdit_Load(object sender, EventArgs e)
        {    
            if (TransactionName == "INSERT")
            {
                ID = GlobalFunctions.GetOracleSequenceValue("GUARANTEE_SEQUENCE");
                List<Guarantee> guarantees = GuaranteeDAL.SelectGuarantee(1, ContractID).ToList<Guarantee>();
                if (guarantees.Count > 0)
                    code = guarantees.LastOrDefault().CODE + 1;

                CodeText.EditValue = code.ToString().PadLeft(2, '0');
            }
            else
                LoadDetails();
            LoadPhoneDataGridView();
        }

        private void LoadDetails()
        {
            string sql = $@"SELECT LPAD (G.CODE, 2, '0') CODE,                                       
                                       G.NAME,                                      
                                       G.NOTE,
                                       G.VOEN,
                                       G.LEADING_NAME,
                                       G.REGISTRATION_ADDRESS,
                                       G.USED_USER_ID
                                  FROM CRS_USER_TEMP.GUARANTEE_TEMP G
                                 WHERE G.CONTRACT_ID = {ContractID}
                                       AND G.ID = {ID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", "Zaminin məlumatları açılmadı.");

            if (dt.Rows.Count > 0)
            {
                CodeText.EditValue = dt.Rows[0]["CODE"];
                NameText.EditValue = dt.Rows[0]["NAME"];                
                RegistrationAddressText.EditValue = dt.Rows[0]["REGISTRATION_ADDRESS"];
                LeadingNameText.EditValue = dt.Rows[0]["LEADING_NAME"];
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

            if (VoenText.Text.Length == 0)
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Vöen daxil edilməyib.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length > 0 && !GlobalFunctions.Regexp("[0-9]{9}1", VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin vöen-i yalnız rəqəmlərdən ibarət olmalıdır, ümumi uzunluğu 10 simvol olmalıdır və sonuncu simvol <b><color=104,0,0>1</color></b> olmalıdır.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor(); ;
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

            if (LeadingNameText.Text.Length == 0)
            {
                LeadingNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Rəhbər şəxsin adı daxil edilməyib.");
                LeadingNameText.Focus();
                LeadingNameText.BackColor = GlobalFunctions.ElementColor(); ;
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
            if (VoenText.Text.Trim().Length == 0)
                NumberLengthLabel.Visible = false;
            else
            {
                NumberLengthLabel.Visible = true;
                NumberLengthLabel.Text = VoenText.Text.Trim().Length.ToString();
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
                                                                         VOEN,
                                                                         LEADING_NAME,
                                                                         REGISTRATION_ADDRESS,
                                                                         USED_USER_ID,
                                                                         NOTE,
                                                                         PERSON_TYPE_ID,
                                                                         IS_CHANGE)
                                VALUES({ID},
                                       {ContractID},
                                       {code},
                                       '{CustomerName}',
                                       '{NameText.Text.Trim()}',
                                       '{VoenText.Text.Trim()}',                                                                         
                                       '{LeadingNameText.Text.Trim()}',
                                       '{RegistrationAddressText.Text.Trim()}',
                                       {GlobalVariables.V_UserID},
                                       '{NoteText.Text.Trim()}',
                                       2,
                                       1)";

            GlobalProcedures.ExecuteQuery(tran, sql);
        }

        private void UpdateGuarantee(OracleTransaction tran)
        {
            string sql = $@"UPDATE CRS_USER_TEMP.GUARANTEE_TEMP SET NAME = '{NameText.Text.Trim()}',                                                                         
                                                                         VOEN = '{VoenText.Text.Trim()}',     
                                                                         LEADING_NAME = '{LeadingNameText.Text.Trim()}',
                                                                         REGISTRATION_ADDRESS = '{RegistrationAddressText.Text.Trim()}',
                                                                         USED_USER_ID = {GlobalVariables.V_UserID},
                                                                         NOTE = '{NoteText.Text.Trim()}',
                                                                         IS_CHANGE = 1
                                WHERE ID = {ID} AND CONTRACT_ID = {ContractID}";

            GlobalProcedures.ExecuteQuery(tran, sql);
        }
    }
}