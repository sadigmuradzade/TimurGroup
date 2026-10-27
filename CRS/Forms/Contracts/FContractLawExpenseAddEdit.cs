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
    public partial class FContractLawExpenseAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FContractLawExpenseAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID;
        public int ContractLawID;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int lawExpenseID = 0;

        private void FContractLawExpenseAddEdit_Load(object sender, EventArgs e)
        {
            RefreshDictionaries(18);
            if (TransactionName == "EDIT")
                LoadDetail();
        }

        private void LoadDetail()
        {
            string sql = $@"SELECT LE.NAME, CL.AMOUNT, CL.NOTE
                              FROM CRS_USER_TEMP.CONTRACT_LAW_EXPENSE_TEMP CL, CRS_USER.LAW_EXPENSE LE
                             WHERE CL.IS_CHANGE != 2 AND CL.LAW_EXPENSE_ID = LE.ID AND CL.ID = {ID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetail", "Xərcin detalları açılmadı.");

            if(dt.Rows.Count > 0)
            {
                GlobalProcedures.LookUpEditValue(ExpenseLookUp, dt.Rows[0]["NAME"].ToString());
                AmountValue.EditValue = dt.Rows[0]["AMOUNT"];
                NoteText.EditValue = dt.Rows[0]["NOTE"];
            }
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(ExpenseLookUp, "LAW_EXPENSE", "ID", "NAME", "1 = 1 ORDER BY NAME");
        }

        private void ExpenseLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 19);
        }

        private void ExpenseLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (ExpenseLookUp.EditValue == null)
                return;

            lawExpenseID = Convert.ToInt32(ExpenseLookUp.EditValue);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (lawExpenseID == 0)
            {
                ExpenseLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Xərc seçilməyib.");
                ExpenseLookUp.Focus();
                ExpenseLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AmountValue.Value <= 0)
            {
                AmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Xərcin məbləği sıfırdan böyük olmalıdır.");
                AmountValue.Focus();
                AmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void FContractLawExpenseAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void Insert()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_LAW_EXPENSE_TEMP(ID,
                                                                                CONTRACT_ID,
                                                                                CONTRACT_LAW_ID,
                                                                                LAW_EXPENSE_ID,
                                                                                AMOUNT,
                                                                                NOTE,
                                                                                USED_USER_ID,
                                                                                IS_CHANGE)
                            VALUES(CRS_USER.CONTRACT_LAW_EXPENSE_SEQUENCE.NEXTVAL,
                                   {ContractID},
                                   {ContractLawID},
                                   {lawExpenseID},
                                   {AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                   '{NoteText.Text.Trim()}',
                                   {GlobalVariables.V_UserID},
                                   1)";

            GlobalProcedures.ExecuteQuery(sql, "Xərc temp cədvələ daxil edilmədi.", this.Name + "/Insert");
        }

        private void Update()
        {
            string sql = $@"UPDATE CRS_USER_TEMP.CONTRACT_LAW_EXPENSE_TEMP SET LAW_EXPENSE_ID = {lawExpenseID},
                                                                               AMOUNT = {AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                               NOTE = '{NoteText.Text.Trim()}',
                                                                               IS_CHANGE = 1
                            WHERE ID = {ID}";

            GlobalProcedures.ExecuteQuery(sql, "Xərc temp cədvəldə dəyişdirilmədi.", this.Name + "/Update");
        }
    }
}