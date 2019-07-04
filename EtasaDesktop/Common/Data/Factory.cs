using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    public class Factory : Entity
    {
        public LocationData Location { get; set; }
        //TXAPUZKA
        public string HexColor { get; set; }

        public FactoryColors Colors { get; set; }
    }
}
