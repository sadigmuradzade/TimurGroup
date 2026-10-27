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
    public partial class FChangeJoinWork : DevExpress.XtraEditors.XtraForm
    {
        public FChangeJoinWork()
        {
            InitializeComponent();
        }
        public int DetailID;

        public delegate void DoEvent();
        public event DoEvent RefreshDetailsDataGridView;

        private void RelatedWorkCodeText_EditValueChanged(object sender, EventArgs e)
        {
            string sql = $@"SELECT L.NAME FROM CRS_USER.CONTRACT_LAWS CL,CRS_USER.LAWS L WHERE CL.LAW_ID = L.ID AND CL.WORK_CODE = '{RelatedWorkCodeText.Text.Trim()}'";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/RelatedWorkCodeText_EditValueChanged", "Əlaqəli işin məhkəməsi tapılmadı.");

            if (dt.Rows.Count > 0)
                RelatedWorkLawNameText.EditValue = dt.Rows[0]["NAME"];
            else
                RelatedWorkLawNameText.EditValue = null;
        }

        private void FChangeJoinWork_Load(object sender, EventArgs e)
        {
            LoadProsecuteDetails();
        }

        private void LoadProsecuteDetails()
        {
            string s = "SELECT CL.WORK_CODE,CL.RELATED_WORK_CODE,CL.RELATED_WORK_LAW_NAME FROM CRS_USER.CONTRACT_LAWS CL WHERE CL.ID = " + DetailID;

            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadProsecuteDetails", "Məhkəmə iclasının məlumatları tapılmadı.").Rows)
            {                
                WorkCodeText.EditValue = dr["WORK_CODE"];
                RelatedWorkCodeText.EditValue = dr["RELATED_WORK_CODE"];
                RelatedWorkLawNameText.EditValue = dr["RELATED_WORK_LAW_NAME"];
            }
        }

        private void FChangeJoinWork_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDetailsDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            UpdateProsecute();
            this.Close();
        }

        private void UpdateProsecute()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_LAWS SET WORK_CODE='{WorkCodeText.Text.Trim()}',
                                                                                RELATED_WORK_CODE='{RelatedWorkCodeText.Text.Trim()}',
                                                                                RELATED_WORK_LAW_NAME='{RelatedWorkLawNameText.Text.Trim()}'
                                            WHERE ID = {DetailID}",
                                                "Məhkəmə işinin nömrələri dəyişdirilmədi.",
                                             this.Name + "/UpdateProsecute");
        }
    }
}