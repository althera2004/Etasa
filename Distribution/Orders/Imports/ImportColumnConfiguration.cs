using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    [Serializable]
    public class ImportColumnConfiguration
    {
        public string ColumnName { get; set; }
        public int[] ColumnNum { get; set; }
        public string[] ColumnLetter { get; set; }
        public string DefaultValue { get; set; }
        public string Format { get; set; } 
    }
}
