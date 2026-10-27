using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class PortfelFont
    {
        public int ID { get; set; }
        public decimal START_VALUE { get; set; }
        public decimal END_VALUE { get; set; }
        public string FONTNAME { get; set; }
        public int FONTSIZE { get; set; }
        public string FONTSTYLE { get; set; }
        public int BACKCOLOR_A { get; set; }
        public int BACKCOLOR_B { get; set; }
        public int BACKCOLOR_G { get; set; }
        public int BACKCOLOR_R { get; set; }
        public string BACKCOLOR_TYPE { get; set; }
        public string BACKCOLOR_NAME { get; set; }
        public int FONTCOLOR_A { get; set; }
        public int FONTCOLOR_B { get; set; }
        public int FONTCOLOR_G { get; set; }
        public int FONTCOLOR_R { get; set; }
        public string FONTCOLOR_TYPE { get; set; }
        public string FONTCOLOR_NAME { get; set; }
    }
}
