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

namespace CRS.Forms.Total
{
    public partial class FStopPercentDateAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FStopPercentDateAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int ContractID;
        public DateTime LastPaymentDate;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        DateTime minDate;

        private void FStopPercentDateAddEdit_Load(object sender, EventArgs e)
        {
            List<ContractPercentDate> lstPercentDate = ContractPercentDateDAL.SelectContractPercentDate(ContractID).ToList<ContractPercentDate>();
            if (TransactionName == "EDIT")
            {
                if (lstPercentDate.Count(item => item.ID > ID) > 0)
                {
                    GlobalProcedures.ShowWarningMessage("Seçilmiş tarix intervalından sonra başqa intervallar olduğu üçün bu tarix intervalı dəyişdirilə bilməz.");
                    this.Close();
                }
                else
                    LoadDetail();

                StartDate.Enabled = StartDate.DateTime > LastPaymentDate;

                if (EndDate.DateTime > DateTime.MinValue && EndDate.DateTime < LastPaymentDate)
                {
                    GlobalProcedures.ShowWarningMessage("Seçilmiş tarix intervalından sonra ödənişlər olduğu üçün bu tarix intervalı dəyişdirilə bilməz.");
                    this.Close();
                }
                else
                    EndDate.Properties.MinValue = LastPaymentDate.AddDays(1);
            }
            else
            {
                minDate = LastPaymentDate.AddDays(1);
                if (lstPercentDate.Count > 0)
                {
                    var lastDate = lstPercentDate.LastOrDefault();
                    if (!lastDate.END_DATE.HasValue)
                    {
                        GlobalProcedures.ShowWarningMessage("Bundan əvvəlki faiz dayandırılmağın bitmə tarixi boş olduğu üçün yeni bir tarix intervalı daxil edilə bilməz.");
                        this.Close();
                    }
                    else if (minDate < lastDate.END_DATE.Value.AddDays(1))
                        minDate = lastDate.END_DATE.Value.AddDays(1);
                }

                StartDate.Properties.MinValue = minDate;
                StartDate.EditValue = minDate > DateTime.Today ? minDate : DateTime.Today;
            }
        }

        private void FStopPercentDateAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (StartDate.Text.Length == 0)
            {
                StartDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Başlanma tarixi seçilməyib");
                StartDate.Focus();
                StartDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void Insert()
        {
            string sql = $@"INSERT INTO CRS_USER.CONTRACT_PERCENT_DATE(CONTRACT_ID,START_DATE,END_DATE,INSERT_USER)VALUES({ContractID},TO_DATE('{StartDate.Text}','DD.MM.YYYY'),TO_DATE('{EndDate.Text}','DD.MM.YYYY'),{GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Tarix daxil edilmədi.", this.Name + "/Insert");
        }

        private void Update()
        {
            string sql = $@"UPDATE CRS_USER.CONTRACT_PERCENT_DATE SET START_DATE = TO_DATE('{StartDate.Text}','DD.MM.YYYY'), END_DATE = TO_DATE('{EndDate.Text}','DD.MM.YYYY'),UPDATE_USER = {GlobalVariables.V_UserID}, UPDATE_DATE = SYSDATE WHERE ID = {ID}";

            GlobalProcedures.ExecuteQuery(sql, "Tarix dəyişdirilmədi.", this.Name + "/Update");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private void LoadDetail()
        {
            string sql = $@"SELECT START_DATE, END_DATE FROM CRS_USER.CONTRACT_PERCENT_DATE WHERE ID = {ID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetail", "Tarix açılmadı.");

            if (dt.Rows.Count > 0)
            {
                StartDate.EditValue = dt.Rows[0]["START_DATE"];
                EndDate.EditValue = dt.Rows[0]["END_DATE"];
            }
        }

        private void StartDate_EditValueChanged(object sender, EventArgs e)
        {
            EndDate.Properties.MinValue = StartDate.DateTime;
        }
    }
}