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
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CRS.Forms.Total
{
    public partial class FMediationLetter : DevExpress.XtraEditors.XtraForm
    {
        public FMediationLetter()
        {
            InitializeComponent();
        }
        public string CustomerName, ContractDate, ContractNumber, Phone, ContractAmount, PaymentDay, CarNumber, BrandAndModel, Delayes, ContractID, HostageName, CustomerID;

        int code_number;

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlFileDetails())
            {
                switch (LetterTypeComboBox.SelectedIndex)
                {
                    case 0:
                        GenerateType1();
                        break;
                    case 1:
                        GenerateType2();
                        break;
                    case 2:
                        GenerateType3();
                        break;
                }
                this.Close();
            }
        }

        public int CreditNameID, CustomerPersonTypeID, CommitmentID, CommitmentPersonTypeID;
        public DateTime ContractEndDate;

        private void FMediationLetter_Load(object sender, EventArgs e)
        {
            DateValue.DateTime = DateTime.Today;
            ContractCodeText.Text = ContractNumber;
            CustomerNameText.Text = CustomerName;
            LoadDetails();
        }

        private void LoadDetails()
        {
            if (CommitmentID == 0)
                CustomerAddressText.Text = GlobalFunctions.GetName($@"SELECT REGISTRATION_ADDRESS FROM CRS_USER.V_CUSTOMER_CARDS_DETAILS WHERE CUSTOMER_ID = {CustomerID} AND PERSON_TYPE_ID = {CustomerPersonTypeID}");
            else
            {
                if (CommitmentPersonTypeID == 1)
                    CustomerAddressText.Text = GlobalFunctions.GetName($@"SELECT ADDRESS FROM CRS_USER.CONTRACT_COMMITMENTS CC WHERE ID = {CommitmentID}");
                else
                    CustomerAddressText.Text = GlobalFunctions.GetName($@"SELECT ADDRESS FROM CRS_USER.CONTRACT_JURIDICAL_COMMITMENTS CC WHERE ID = {CommitmentID}");
            }
        }

        private bool ControlFileDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(LetterCodeText.Text))
            {
                LetterCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məktubun nömrəsi daxil edilməyib.");
                LetterCodeText.Focus();
                LetterCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(DateValue.Text))
            {
                DateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məktubun tarixi daxil edilməyib.");
                DateValue.Focus();
                DateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (LetterTypeComboBox.SelectedIndex == -1)
            {
                LetterTypeComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məktubun növü seçilməyib");
                LetterTypeComboBox.Focus();
                LetterTypeComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void GenerateType1()
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mediasiya üçün müraciət.docx"))
            {
                GlobalProcedures.ShowErrorMessage("Mediasiya üçün müraciətin şablon faylı tapılmadı.");
                return;
            }

            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));

            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));

            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Mediasiya1_" + code_number + ".docx";

            try
            {
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mediasiya üçün müraciət.docx"),
                       saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();
                GlobalProcedures.FindAndReplace(wordApp, "[$code]", LetterCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", DateValue.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$registrationaddress]", CustomerAddressText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$phone]", Phone);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                string insert = $@"INSERT INTO CRS_USER.WARNING_LETTERS(TYPE_NAME,CONTRACT_ID,LETTER_CODE,LETTER_DATE,LETTER_FILE,ADDRESS,INSERT_USER)
                                   VALUES('Mediasiya üçün müraciət',{ContractID},'{LetterCodeText.Text.Trim()}',TO_DATE('{DateValue.Text}','DD.MM.YYYY'),:BlobFile,'{CustomerAddressText.Text.Trim()}',{GlobalVariables.V_UserID})";
                GlobalFunctions.ExecuteQueryWithBlob(insert, filePath, "Mediasiya üçün müraciət yadda saxlanılmadı.", this.Name + "/GenerateFile1");

                Process.Start(filePath);
                GlobalProcedures.SplashScreenClose();
            }
            catch (Exception exx)
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage("Mediasiya1_" + code_number + ".docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.", exx);
            }

        }

        private void GenerateType2()
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mediasiya təklifi (vaxtı davam edən müqavilə üzrə).docx"))
            {
                GlobalProcedures.ShowErrorMessage("Mediasiya üçün müraciətin şablon faylı tapılmadı.");
                return;
            }

            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));

            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));

            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Mediasiya2_" + code_number + ".docx";

            try
            {
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mediasiya təklifi (vaxtı davam edən müqavilə üzrə).docx"),
                       saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();
                GlobalProcedures.FindAndReplace(wordApp, "[$code]", LetterCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", DateValue.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractstartdate]", ContractDate);
                GlobalProcedures.FindAndReplace(wordApp, "[$delays]", Delayes);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractday]", PaymentDay);
                GlobalProcedures.FindAndReplace(wordApp, "[$address]", CustomerAddressText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractamount]", ContractAmount);
                if (CreditNameID == 1)
                    GlobalProcedures.FindAndReplace(wordApp, "[$object]", HostageName + " markalı " + CarNumber + " d.q.n.-lı avtomobil");
                else
                    GlobalProcedures.FindAndReplace(wordApp, "[$object]", HostageName + " ünvanında olan mənzil");

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);
                string insert = $@"INSERT INTO CRS_USER.WARNING_LETTERS(TYPE_NAME,CONTRACT_ID,LETTER_CODE,LETTER_DATE,LETTER_FILE,ADDRESS,INSERT_USER)
                                   VALUES('Mediasiya təklifi (vaxtı davam edən müqavilə üzrə)',{ContractID},'{LetterCodeText.Text.Trim()}',TO_DATE('{DateValue.Text}','DD.MM.YYYY'),:BlobFile,'{CustomerAddressText.Text.Trim()}',{GlobalVariables.V_UserID})";
                GlobalFunctions.ExecuteQueryWithBlob(insert, filePath, "Mediasiya təklifi (vaxtı davam edən müqavilə üzrə) yadda saxlanılmadı.", this.Name + "/GenerateFile2");
                Process.Start(filePath);
                GlobalProcedures.SplashScreenClose();
            }
            catch (Exception exx)
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage("Mediasiya2_" + code_number + ".docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.", exx);
            }

        }

        private void GenerateType3()
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mediasiya təklifi (vaxtı bitmiş müqavilə üzrə).docx"))
            {
                GlobalProcedures.ShowErrorMessage("Mediasiya üçün müraciətin şablon faylı tapılmadı.");
                return;
            }

            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));

            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));

            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Mediasiya3_" + code_number + ".docx";

            try
            {
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mediasiya təklifi (vaxtı bitmiş müqavilə üzrə).docx"),
                       saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();
                GlobalProcedures.FindAndReplace(wordApp, "[$code]", LetterCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", DateValue.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractstartdate]", ContractDate);
                GlobalProcedures.FindAndReplace(wordApp, "[$delays]", Delayes);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractday]", PaymentDay);
                GlobalProcedures.FindAndReplace(wordApp, "[$address]", CustomerAddressText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractamount]", ContractAmount);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractenddate]", ContractEndDate.ToString("dd.MM.yyyy"));
                if (CreditNameID == 1)
                    GlobalProcedures.FindAndReplace(wordApp, "[$object]", HostageName + " markalı " + CarNumber + " d.q.n.-lı avtomobil");
                else
                    GlobalProcedures.FindAndReplace(wordApp, "[$object]", HostageName + " ünvanında olan mənzil");

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);
                string insert = $@"INSERT INTO CRS_USER.WARNING_LETTERS(TYPE_NAME,CONTRACT_ID,LETTER_CODE,LETTER_DATE,LETTER_FILE,ADDRESS,INSERT_USER)
                                   VALUES('Mediasiya təklifi (vaxtı bitmiş müqavilə üzrə)',{ContractID},'{LetterCodeText.Text.Trim()}',TO_DATE('{DateValue.Text}','DD.MM.YYYY'),:BlobFile,'{CustomerAddressText.Text.Trim()}',{GlobalVariables.V_UserID})";
                GlobalFunctions.ExecuteQueryWithBlob(insert, filePath, "Mediasiya təklifi (vaxtı bitmiş müqavilə üzrə) yadda saxlanılmadı.", this.Name + "/GenerateFile3");
                Process.Start(filePath);
                GlobalProcedures.SplashScreenClose();
            }
            catch (Exception exx)
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage("Mediasiya3_" + code_number + ".docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.", exx);
            }

        }
    }
}