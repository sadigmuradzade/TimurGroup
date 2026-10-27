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
    public partial class FInsuranceAdditionalAmountEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceAdditionalAmountEdit()
        {
            InitializeComponent();
        }
        public int InsuranceID;
        public string ContractCode, Note;
        public DateTime StartDate, EndDate;
        public decimal AdditionalAmount = 0;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void BOK_Click(object sender, EventArgs e)
        {
            string sql = $@"UPDATE CRS_USER.INSURANCES SET ADDITIONAL_AMOUNT = {AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)}, ADDITIONAL_AMOUNT_NOTE = '{NoteText.Text.Trim()}' WHERE ID = {InsuranceID}";
            GlobalProcedures.ExecuteQuery(sql, "Sığortanı haqqına əlavə olunan məbləğ daxil edilmədi.", this.Name + "/BOK_Click");
            this.Close();
        }

        private void FInsuranceAdditionalAmountEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void FInsuranceAdditionalAmountEdit_Load(object sender, EventArgs e)
        {
            ContractCodeText.Text = ContractCode;
            InsuranceStartDate.EditValue = StartDate;
            InsuranceEndDate.EditValue = EndDate;
            AmountValue.EditValue = AdditionalAmount;
            NoteText.EditValue = Note;
        }
    }
}