using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class CustomerPayments
    {
        public int ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public DateTime PAYMENT_DATE { get; set; }
        public decimal PAYMENT_AMOUNT { get; set; }
        public decimal BASIC_AMOUNT { get; set; }
        public decimal DEBT { get; set; }
        public int DAY_COUNT { get; set; }
        public decimal ONE_DAY_INTEREST_AMOUNT { get; set; }
        public decimal INTEREST_AMOUNT { get; set; }
        public decimal PAYMENT_INTEREST_AMOUNT { get; set; }
        public decimal PAYMENT_INTEREST_DEBT { get; set; }
        public decimal TOTAL { get; set; }
        public decimal CURRENCY_RATE { get; set; }
        public decimal PAYMENT_AMOUNT_AZN { get; set; }
        public decimal REQUIRED_CLOSE_AMOUNT { get; set; }
        public decimal REQUIRED_AMOUNT { get; set; }
        public string PAYMENT_NAME { get; set; }
        public string BANK_CASH { get; set; }
        public string NOTE { get; set; }
        public decimal PENALTY_DEBT { get; set; }
        public int IS_PENALTY { get; set; }
        public decimal PENALTY_AMOUNT { get; set; }
        public int? BANK_ID { get; set; }
        public int CUSTOMER_TYPE_ID { get; set; }
        public DateTime? CHANGE_DATE { get; set; }
        public decimal PAYED_PENALTY { get; set; }
        public int IS_PENALTY_DEBT { get; set; }
        public decimal OVERDUE_PERCENT { get; set; }
        public decimal DELAYS_AMOUNT { get; set; }
        public decimal NORMAL_DEBT { get; set; }
        public int DELAYS_DAY_COUNT { get; set; }
        public decimal CONTRACT_PERCENT { get; set; }        
        public decimal PAYED_ACCOUNT_DEBT { get; set; }
        public int IS_PAYED_ACCOUNT_DEBT { get; set; }
        public decimal ACCOUNT_DEBT { get; set; }
    }
}
