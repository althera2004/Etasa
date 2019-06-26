using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    public class Product : Entity
    {
        public float Density { get; set; }
        public short MeasureUnit { get; set; }
    }
}
