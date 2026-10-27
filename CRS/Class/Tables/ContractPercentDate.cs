using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class ContractPercentDate
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
    }
}
