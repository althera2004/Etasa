using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Planner.Map
{
    class Punto
    {
        public float longitude { get;set; }
        public float latitude { get; set; }

        public double longitudeDouble { get; set; }
        public double latitudeDouble { get; set; }


        public Punto()
        {
            this.longitude = 0;
            this.latitude = 0;
            this.longitudeDouble = 0;
            this.latitudeDouble = 0;
        }
    }

}
