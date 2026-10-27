using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;

namespace CRS.Forms.Dictionaries
{
    public partial class FInsuranceCompanyAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceCompanyAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CompanyID;
        bool CurrentStatus = false, CompanyUsed = false;
        int CompanyUsedUserID = -1;
        string FilePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\InsuranceTempFile.docx";

        public delegate void DoEvent();
        public event DoEvent RefreshInsuranceCompanyDataGridView;


        private void FInsuranceCompanyAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")  
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.INSURANCE_COMPANY", GlobalVariables.V_UserID, "WHERE ID = " + CompanyID + " AND USED_USER_ID = -1");
                CompanyUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.INSURANCE_COMPANY WHERE ID = {CompanyID}");
                CompanyUsed = (CompanyUsedUserID > 0);
                
                if (CompanyUsed)
                {
                    if (GlobalVariables.V_UserID != CompanyUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CompanyUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş şirkət hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş şirkətin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadCompanyDetails();
            }
        }

        private void LoadCompanyDetails()
        {
            string s = "SELECT NAME,NOTE,ADDRESS,ADDRESS_DESCRIPTION,PHONE_NUMBER,FAX,TEMP_FILE FROM CRS_USER.INSURANCE_COMPANY WHERE ID = " + CompanyID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCompanyDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr[0].ToString();
                    NoteText.Text = dr[1].ToString();
                    AddressText.Text = dr[2].ToString();
                    AddressDescriptionText.Text = dr[3].ToString();
                    PhoneText.Text = dr[4].ToString();
                    FaxText.Text = dr[5].ToString();

                    if (!DBNull.Value.Equals(dr["TEMP_FILE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["TEMP_FILE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);

                        GlobalProcedures.DeleteFile(FilePath);
                        FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(fs);
                        fs.Close();
                        stmBLOBData.Close();
                        TempFileButtonEdit.Text = FilePath;
                    }
                    
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Şirkətin məlumatları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private bool ControlCompanyDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şirkətin adı daxil edilməyib.");               
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AddressText.Text.Length == 0)
            {
                AddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şirkətin ünvanı daxil edilməyib.");                
                AddressText.Focus();
                AddressText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PhoneText.Text.Length == 0)
            {
                PhoneText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şirkətin telefon nömrəsi daxil edilməyib.");                
                PhoneText.Focus();
                PhoneText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCompany()
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    if (TempFileButtonEdit.Text.Length > 0)
                    {
                        FileStream fls = new FileStream(TempFileButtonEdit.Text, FileMode.Open, FileAccess.Read);
                        byte[] blob = new byte[fls.Length];
                        fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                        fls.Close();
                        command.CommandText = "INSERT INTO CRS_USER.INSURANCE_COMPANY(NAME,ADDRESS,ADDRESS_DESCRIPTION,PHONE_NUMBER,FAX,NOTE,TEMP_FILE) VALUES('" + NameText.Text.Trim() + "','" + AddressText.Text.Trim() + "','" + AddressDescriptionText.Text.Trim() + "','"
                                                                            + PhoneText.Text.Trim() + "','" + FaxText.Text.Trim() + "','" + NoteText.Text.Trim() + "',:BlobFile)";
                        OracleParameter blobParameter = new OracleParameter();
                        blobParameter.OracleDbType = OracleDbType.Blob;
                        blobParameter.ParameterName = "BlobFile";
                        blobParameter.Value = blob;
                        command.Parameters.Add(blobParameter);
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO CRS_USER.INSURANCE_COMPANY(NAME,ADDRESS,ADDRESS_DESCRIPTION,PHONE_NUMBER,FAX,NOTE) VALUES('" + NameText.Text.Trim() + "','" + AddressText.Text.Trim() + "','" + AddressDescriptionText.Text.Trim() + "','"
                                                                            + PhoneText.Text.Trim() + "','" + FaxText.Text.Trim() + "','" + NoteText.Text.Trim() + "')";
                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Şirkətin məlumatları sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                    
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void UpdateCompany()
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    if (TempFileButtonEdit.Text.Length > 0)
                    {
                        FileStream fls = new FileStream(TempFileButtonEdit.Text, FileMode.Open, FileAccess.Read);
                        byte[] blob = new byte[fls.Length];
                        fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                        fls.Close();
                        command.CommandText = "UPDATE CRS_USER.INSURANCE_COMPANY SET NAME = '" + NameText.Text.Trim() + "',ADDRESS = '" + AddressText.Text.Trim() + "',ADDRESS_DESCRIPTION = '" + AddressDescriptionText.Text.Trim() + "',PHONE_NUMBER = '" + PhoneText.Text.Trim() + "',FAX = '" + FaxText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "',TEMP_FILE = :BlobFile WHERE ID = " + CompanyID;
                        OracleParameter blobParameter = new OracleParameter();
                        blobParameter.OracleDbType = OracleDbType.Blob;
                        blobParameter.ParameterName = "BlobFile";
                        blobParameter.Value = blob;
                        command.Parameters.Add(blobParameter);
                    }
                    else
                    {
                        command.CommandText = "UPDATE CRS_USER.INSURANCE_COMPANY SET NAME = '" + NameText.Text.Trim() + "',ADDRESS = '" + AddressText.Text.Trim() + "',ADDRESS_DESCRIPTION = '" + AddressDescriptionText.Text.Trim() + "',PHONE_NUMBER = '" + PhoneText.Text.Trim() + "',FAX = '" + FaxText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "',TEMP_FILE = NULL WHERE ID = " + CompanyID;
                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Şirkətin məlumatları dəyişdirilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                    
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void TempFileButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Şablon faylı seçin";
                    dlg.Filter = "All files (*.doc;*.docx)|*.doc;*.docx";

                    if (dlg.ShowDialog() == DialogResult.OK)
                        TempFileButtonEdit.Text = dlg.FileName;
                    dlg.Dispose();
                }
            }
            else if (e.Button.Index == 1)
                TempFileButtonEdit.Text = null;
            else
            {
                try
                {
                    if (TempFileButtonEdit.Text.Length != 0)
                        System.Diagnostics.Process.Start(TempFileButtonEdit.Text);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Şablon faylın ünvanı düz deyil.", TempFileButtonEdit.Text, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void FInsuranceCompanyAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.INSURANCE_COMPANY", -1, "WHERE ID = " + CompanyID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            GlobalProcedures.DeleteFile(FilePath);
            this.RefreshInsuranceCompanyDataGridView();
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = !status;
            AddressText.Enabled = !status;
            AddressDescriptionText.Enabled = !status;
            PhoneText.Enabled = !status;
            FaxText.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCompanyDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCompany();
                else
                    UpdateCompany();
                this.Close();
            }
        }
    }
}