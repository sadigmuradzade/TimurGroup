using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class Guarantee
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public int CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string NAME { get; set; }
        public int SEX_ID { get; set; }
        public int COUNTRY_ID { get; set; }
        public DateTime BIRTHDAY { get; set; }
        public int BIRTHPLACE_ID { get; set; }
        public string CARD_NUMBER { get; set; }
        public string CARD_PINCODE { get; set; }
        public DateTime CARD_ISSUE_DATE { get; set; }
        public DateTime RELIABLE_DATE { get; set; }
        public int CARD_ISSUING_ID { get; set; }
        public string ADDRESS { get; set; }
        public string REGISTRATION_ADDRESS { get; set; }
        public int USED_USER_ID { get; set; }
        public string NOTE { get; set; }
        public int IS_CHANGE { get; set; }
    }
}
