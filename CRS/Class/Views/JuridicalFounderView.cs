using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Views
{
    public class JuridicalFounderView
    {
        public long ID { get; set; }
        public long JURIDICAL_PERSON_ID { get; set; }
        public long PARENT_ID { get; set; }
        public int PERSON_TYPE_ID { get; set; }
        public decimal FOUNDER_SHARE { get; set; }
        public string NAME { get; set; }
        public string PINCODE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
        public DateTime DOCUMENT_ISSUE_DATE { get; set; }
        public DateTime DOCUMENT_RELIABLE_DATE { get; set; }
        public string DOCUMENT_COUNTRY_NAME { get; set; }
        public string REGISTRATION_ADDRESS { get; set; }
        public DateTime  BIRTHDAY { get; set; }
        public string BIRTHPLACE_NAME { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string VOEN { get; set; }
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
        public int IS_SANCTION_CHECK { get; set; }
        public string SANCTION_CHECK_URL { get; set; }
        public string NOTE { get; set; }
    }
}
