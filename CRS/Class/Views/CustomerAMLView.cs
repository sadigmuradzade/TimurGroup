using System;

namespace CRS.Class.Views
{
    public class CustomerAMLView
    {
        public string RISK_DEGREE_NAME { get; set; }
        public string SOCIAL_STATUS_NAME { get; set; }
        public string PROFESSION_NAME { get; set; }
        public decimal MONTHLY_AMOUNT { get; set; }
        public string SOURCE_INCOME_NAME { get; set; }
        public int IS_LOCAL_POLITICAL_FIGURE { get; set; }
        public string LOCAL_POLITICAL_DUTY_NAME { get; set; }
        public DateTime LOCAL_POLITICAL_START_DATE { get; set; }
        public DateTime LOCAL_POLITICAL_END_DATE { get; set; }
        public int IS_FOREIGN_POLITICAL_FIGURE { get; set; }
        public string FOREIGN_POLITICAL_DUTY_NAME { get; set; }
        public DateTime FOREIGN_POLITICAL_START_DATE { get; set; }
        public DateTime FOREIGN_POLITICAL_END_DATE { get; set; }
        public int IS_RELATIVE_POLITICAL_FIGURE { get; set; }
        public string RELATIVE_NAME { get; set; }
        public string RELATIVE_POLITICAL_DUTY_NAME { get; set; }
        public DateTime RELATIVE_POLITICAL_START_DATE { get; set; }
        public DateTime RELATIVE_POLITICAL_END_DATE { get; set; }
        public string CUSTOMER_BEHAVIOR_NAME { get; set; }
        public string CUSTOMER_IDENTIFICATION_NAME { get; set; }
        public string PURPOSE_BUSINESS_NAME { get; set; }
        public int IS_SANCTION_CHECK { get; set; }
        public string SANCTION_CHECK_URL { get; set; }
        public string NOTE { get; set; }
    }
}
