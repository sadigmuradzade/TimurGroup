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

namespace CRS.Forms.Bookkeeping
{
    public partial class FPaymentList : DevExpress.XtraEditors.XtraForm
    {
        public FPaymentList()
        {
            InitializeComponent();
        }

        private void FPaymentList_Load(object sender, EventArgs e)
        {
            this.Text = "Qrafik üzrə bu gün və ya növbəti 3 gün ərzində ödəniş etməli olan müştərilər";
            LoadPaymentsDataGridView();
        }

        private void LoadPaymentsDataGridView()
        {
            string s = $@"SELECT NVL (COM.COMMITMENT_NAME, C.FULLNAME) CUSTOMERFULLNAME,
                                 CON.CONTRACT_CODE,
                                 PS.MONTH_DATE PDATE,
                                 ROUND (PS.MONTHLY_PAYMENT, 2) AMOUNT,
                                 CON.CURRENCY_CODE CUR_CODE,
                                 CON.CREDIT_TYPE_ID,
                                 CON.CONTRACT_ID,
                                 C.VOEN
                            FROM CRS_USER.V_LAST_PAYMENT_SCHEDULES PS,
                                 CRS_USER.V_CUSTOMERS C,
                                 CRS_USER.V_CONTRACTS CON,
                                 CRS_USER.V_COMMITMENTS COM
                           WHERE     PS.CONTRACT_ID = CON.CONTRACT_ID
                                 AND CON.CUSTOMER_ID = C.ID
                                 AND CON.CUSTOMER_TYPE_ID = C.PERSON_TYPE_ID
                                 AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                                 AND CON.STATUS_ID = 5
                                 AND NOT EXISTS
                                           (SELECT 1
                                              FROM CRS_USER.CUSTOMER_PAYMENTS
                                             WHERE     CONTRACT_ID = CON.CONTRACT_ID
                                                   AND TO_CHAR (PAYMENT_DATE, 'YYYYMMDD') BETWEEN TO_CHAR (
                                                                                                     SYSDATE,
                                                                                                     'YYYYMMDD')
                                                                                              AND TO_CHAR (
                                                                                                       SYSDATE
                                                                                                     + 3,
                                                                                                     'YYYYMMDD'))
                                 AND TO_CHAR (PS.MONTH_DATE, 'YYYYMMDD') BETWEEN TO_CHAR (SYSDATE,
                                                                                          'YYYYMMDD')
                                                                             AND TO_CHAR (SYSDATE + 3,
                                                                                          'YYYYMMDD')
                        ORDER BY PS.MONTH_DATE, C.FULLNAME";
            PaymentsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentsDataGridView");
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPaymentsDataGridView();
        }

        private void PaymentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentsGridView, PopupMenu, e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PaymentsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "xls");
        }
    }
}