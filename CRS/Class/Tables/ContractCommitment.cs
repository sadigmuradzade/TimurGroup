using System;

namespace CRS.Class.Tables
{
    public class ContractCommitment
    {
        public int ID { get; set; }
        public int PARENT_ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public DateTime AGREEMENTDATE { get; set; }
        public string COMMITMENT_NAME { get; set; }
        public double DEBT { get; set; }
        public int CURRENCY_ID { get; set; }
        public string CURRENCY_CODE { get; set; }
        public DateTime PERIOD_DATE { get; set; }
        public int INTEREST { get; set; }
        public double ADVANCE_PAYMENT { get; set; }
        public double SERVICE_AMOUNT { get; set; }
        public int IS_CHANGE { get; set; }
        public int PERSON_TYPE_ID { get; set; }
        public string CARD { get; set; }
        public string ADDRESS { get; set; }
        public string VOEN { get; set; }
        public string PINCODE { get; set; }
        public int MONTH_COUNT { get; set; }
        public decimal MONTHLY_AMOUNT { get; set; }
        public decimal TOTAL_DEBT { get; set; }
    }
}
