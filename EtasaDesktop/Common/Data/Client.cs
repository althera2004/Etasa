using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    public class Client : Entity
    {
        public string Cif { get; set; }

        public LocationData Location { get; set; }
        public ContactData Contact { get; set; }
    }
}
