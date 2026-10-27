using CRS.Class;
using System;
using System.ComponentModel;

namespace CRS.Forms
{
    public partial class FCreditCalculator : DevExpress.XtraEditors.XtraForm
    {
        public FCreditCalculator()
        {
            InitializeComponent();
        }

        public class ScheduleDetail
        {
            public int SS { get; set; }
            public decimal MonthlyPayment { get; set; }
            public decimal BasicAmount { get; set; }
            public decimal InterestAmount { get; set; }
            public decimal Debt { get; set; }
            public DateTime MonthDate { get; set; }
        }

        BindingList<ScheduleDetail> scheduleDetails = new BindingList<ScheduleDetail>();

        private void FCreditCalculator_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadSchedule()
        {
            PaymentSchedulesGridControl.DataSource = scheduleDetails;
            PaymentSchedulesGridView.RefreshData();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreditAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            MonthlyAmountValue.EditValue = Math.Round(GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, (double)PeriodValue.Value, (double)InterestValue.Value), 2);                        
        }

        private void CalcShedule()
        {
            scheduleDetails.Clear();
            int month_count = (int)PeriodValue.Value;
            DateTime d_start, d_end;
            decimal interest_amount = 0, basic_amount = 0, debt = CreditAmountValue.Value, monthly_amount = MonthlyAmountValue.Value, sumPayment = 0, sumInterest = 0;
            d_end = DateTime.Today.AddMonths(month_count);
            for (int i = 1; i <= month_count; i++)
            {
                d_start = DateTime.Today;
                d_start = d_start.AddMonths(i);

                if (d_start > d_end)
                    d_start = d_end;

                interest_amount = ((debt * InterestValue.Value) / 100) / 360 * 30;

                if (i < month_count)
                    basic_amount = monthly_amount - interest_amount;
                else
                {
                    basic_amount = debt;
                    monthly_amount = debt + interest_amount;
                }

                debt = debt - basic_amount;

                scheduleDetails.Add(new ScheduleDetail
                {
                    SS = i,
                    MonthDate = d_start,
                    MonthlyPayment = monthly_amount,
                    BasicAmount = basic_amount,
                    InterestAmount = interest_amount,
                    Debt = debt
                });
                sumPayment += monthly_amount;
                sumInterest += interest_amount;                
            }

            SumPaymentText.Text = sumPayment.ToString("N2");
            InterestAmountValue.EditValue = sumInterest;
        }

        private void MonthlyAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            CalcShedule();
            LoadSchedule();
        }

        private void PaymentSchedulesGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Shedule_BasicAmount, "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell(Shedule_MonthlyPayment, "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell(Shedule_InterestAmount, "Far", e);
        }

        private void InterestAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            AverageInterestPercentValue.EditValue = Math.Round(Math.Round(InterestAmountValue.Value / CreditAmountValue.Value * 100, 2) / PeriodValue.Value, 2);
        }
    }
}