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
using CRS.Class.Tables;
using CRS.Class;
using CRS.Class.DataAccess;

namespace CRS.Forms.Total
{
    public partial class FDonateDebt : DevExpress.XtraEditors.XtraForm
    {
        public FDonateDebt()
        {
            InitializeComponent();
        }
        public int ContractID, Interest, CustomerID, CustomerTypeID, CurrencyID;
        public string Currency, CustomerName;

        public delegate void DoEvent();
        public event DoEvent RefreshPaymentsDataGridView;

        DateTime lastDate;

        private void FDonateDebt_Load(object sender, EventArgs e)
        {
            CurrencyLabel.Text = PaymentInterestDebtCurrencyLabel.Text = Currency;
            PaymentDate.EditValue = DateTime.Today;
            FindLastDateAndDebt();
        }

        private void FindLastDateAndDebt()
        {
            try
            {
                List<Payments> lstPayments = PaymentsDAL.SelectPayments(1, ContractID).ToList<Payments>();
                if (lstPayments.Count == 0)
                    return;

                var lastPayments = lstPayments.Where(item => item.IS_CHANGE == 0 || item.IS_CHANGE == 1).LastOrDefault();


                DebtValue.EditValue = Math.Round((double)lastPayments.DEBT, 2);
                PaymentInterestDebtValue.EditValue = Math.Round((double)lastPayments.PAYMENT_INTEREST_DEBT, 2);
                lastDate = lastPayments.PAYMENT_DATE;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Qalıq tapılmadı.", null, GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPaymentDetails())
            {
                InsertPayment();
                this.Close();
            }
        }

        private bool ControlPaymentDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(PaymentDate.Text))
            {
                PaymentDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix seçilməyib.");
                PaymentDate.Focus();
                PaymentDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void FDonateDebt_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPaymentsDataGridView();
        }

        private void InsertPayment()
        {
            int diff_day;
            decimal payment_interest_amount = 0,
                currenct_payment_interest_debt = 0,
                basic_amount = 0,
                current_debt = 0,
                total = 0,
                payment_value_AZN = 0,
                one_day_interest = 0,
                interest_amount = 0,
                rateValue = 1,
                totaldebtamount = 0,
                requiredamount = 0;

            diff_day = GlobalFunctions.Days360(lastDate, PaymentDate.DateTime);

            if (CurrencyID != 1)
                rateValue = (decimal)GlobalFunctions.CurrencyLastRate(CurrencyID, PaymentDate.Text);

            one_day_interest = Math.Round(((DebtValue.Value * Interest) / 100) / 360, 2);
            interest_amount = diff_day * one_day_interest;

            decimal paymentValue = DebtValue.Value + PaymentInterestDebtValue.Value + interest_amount;

            payment_interest_amount = interest_amount + PaymentInterestDebtValue.Value;

            basic_amount = paymentValue - payment_interest_amount;

            currenct_payment_interest_debt = PaymentInterestDebtValue.Value + interest_amount - payment_interest_amount;
            current_debt = DebtValue.Value - basic_amount;
            total = current_debt + currenct_payment_interest_debt;
            
            payment_value_AZN = Math.Round(rateValue > 0 ? paymentValue / rateValue : 0, 2);            
            
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP(ID,
                                                                                                CUSTOMER_ID,
                                                                                                CONTRACT_ID,
                                                                                                PAYMENT_DATE,
                                                                                                PAYMENT_AMOUNT,
                                                                                                BASIC_AMOUNT,
                                                                                                DEBT,
                                                                                                DAY_COUNT,
                                                                                                ONE_DAY_INTEREST_AMOUNT,
                                                                                                INTEREST_AMOUNT,
                                                                                                PAYMENT_INTEREST_AMOUNT,
                                                                                                PAYMENT_INTEREST_DEBT,
                                                                                                TOTAL,
                                                                                                CURRENCY_RATE,
                                                                                                PAYMENT_AMOUNT_AZN,
                                                                                                REQUIRED_CLOSE_AMOUNT,
                                                                                                REQUIRED_AMOUNT,
                                                                                                PAYMENT_NAME,
                                                                                                NOTE,
                                                                                                USED_USER_ID,
                                                                                                IS_CHANGE,
                                                                                                BANK_CASH,
                                                                                                PENALTY_DEBT,
                                                                                                IS_PENALTY,
                                                                                                PENALTY_AMOUNT,
                                                                                                BANK_ID,
                                                                                                CUSTOMER_TYPE_ID,
                                                                                                IS_BASIC_AMOUNT,
                                                                                                PAYED_AMOUNT,
                                                                                                PAYED_AMOUNT_AZN,
                                                                                                INSURANCE_CHECK,
                                                                                                INSURANCE_AMOUNT,
                                                                                                CONTRACT_PERCENT)
                                   VALUES(CRS_USER.CUSTOMER_PAYMENT_SEQUENCE.NEXTVAL,
                                          {CustomerID},
                                          {ContractID},
                                          TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                          {paymentValue.ToString(GlobalVariables.V_CultureInfoEN)},
                                          {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {diff_day},
                                          {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {Math.Round(rateValue, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                          {totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                          {requiredamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                          '{CustomerName.Trim()}',
                                          '{NoteText.Text.Trim()}',
                                          {GlobalVariables.V_UserID},1,'BB',0,0,0,0,
                                          {CustomerTypeID},0,0,0,0,0,
                                          {Interest.ToString(GlobalVariables.V_CultureInfoEN)})",
                                                "Ödəniş temp cədvələ daxil edilmədi.",
                                                this.Name + "/InsertPayment");
        }
    }
}