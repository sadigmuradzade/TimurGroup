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
    public partial class FPhoneBook : DevExpress.XtraEditors.XtraForm
    {
        public FPhoneBook()
        {
            InitializeComponent();
        }

        private void FPhoneBook_Load(object sender, EventArgs e)
        {
            LoadPhone();
        }

        private void LoadPhone()
        {
            string sql = $@"SELECT P.OWNER_ID ID, 'Müştəri' OWNER_TYPE, C.CUSTOMER_NAME OWNER_NAME, P.PHONE
                                  FROM CRS_USER.V_PHONE P, CRS_USER.V_CUSTOMERS C
                                 WHERE     OWNER_TYPE IN ('C')
                                       AND C.ID = P.OWNER_ID
                                       AND C.PERSON_TYPE_ID = 1
                                UNION ALL       
                                SELECT P.OWNER_ID ID, 'Müştəri' OWNER_TYPE, C.CUSTOMER_NAME OWNER_NAME, P.PHONE
                                  FROM CRS_USER.V_PHONE P, CRS_USER.V_CUSTOMERS C
                                 WHERE     OWNER_TYPE IN ('JP')
                                       AND C.ID = P.OWNER_ID
                                       AND C.PERSON_TYPE_ID = 2       
                                UNION ALL       
                                SELECT P.OWNER_ID ID, 'Satıcı' OWNER_TYPE, C.FULLNAME OWNER_NAME, P.PHONE
                                  FROM CRS_USER.V_PHONE P, CRS_USER.V_SELLERS C
                                 WHERE     OWNER_TYPE IN ('S')
                                       AND C.ID = P.OWNER_ID
                                       AND C.PERSON_TYPE_ID = 1       
                                UNION ALL       
                                SELECT P.OWNER_ID ID, 'Satıcı' OWNER_TYPE, C.FULLNAME OWNER_NAME, P.PHONE
                                  FROM CRS_USER.V_PHONE P, CRS_USER.V_SELLERS C
                                 WHERE     OWNER_TYPE IN ('JP')
                                       AND C.ID = P.OWNER_ID
                                       AND C.PERSON_TYPE_ID = 2    
                                UNION ALL       
                                SELECT P.OWNER_ID ID, 'Öhdəlik götürən' OWNER_TYPE, C.COMMITMENT_NAME OWNER_NAME, P.PHONE
                                  FROM CRS_USER.V_PHONE P, CRS_USER.CONTRACT_ALL_COMMITMENTS  C
                                 WHERE     OWNER_TYPE IN ('CC')
                                       AND C.ID = P.OWNER_ID
                                       AND C.PERSON_TYPE_ID = 1 ";

            PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPhone", "Telefonların siyahısı açılmadı.");
        }

        private void PhoneGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Phone_SS, e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhone();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PhoneGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PhoneGridControl, "xls");
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PopupMenu, e);
        }
    }
}