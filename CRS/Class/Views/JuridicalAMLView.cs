using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Views
{
    public class JuridicalAMLView
    {
        public string RISK_DEGREE_NAME { get; set; }
        public string ORGANIZATIONAL_LEGAL_NAME { get; set; }
        public DateTime REGISTRATION_DATE { get; set; }
        public decimal AUTHORIZED_CAPITAL { get; set; }       
        public DateTime DRC_ISSUE_DATE { get; set; }
        public int EMPLOYEES_NUMBER { get; set; }
        public int BRANCH_COUNT { get; set; }
        public decimal VALUE_ASSETS { get; set; }
        public string OLD_COMPANY_NAME { get; set; }        
        public string PURPOSE_BUSINESS_NAME { get; set; }
        public int IS_SANCTION_CHECK { get; set; }
        public string SANCTION_CHECK_URL { get; set; }
        public string WEBSITE { get; set; }
        public string NOTE { get; set; }
    }
}
