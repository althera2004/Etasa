using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    public class Operator : Entity
    {
        public string Cif { get; set; }

        public LocationData Location { get; set; }
        public ContactData Contact { get; set; }
    }
}
