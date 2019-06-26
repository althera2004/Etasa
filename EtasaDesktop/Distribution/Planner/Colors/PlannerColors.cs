using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EtasaDesktop.Distribution.Planner
{
    public class FactoryColors
    {
        public int FactoryId { get; set; }
        public Color FactoryColor { get; set; }
        public Color ClientColor { get; set; }
        public Color UrgentColor { get; set; }
        public Color FinalDayColor { get; set; }
        public Color PreferenceColor { get; set; }
    }
}
