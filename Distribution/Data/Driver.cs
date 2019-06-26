using EtasaDesktop.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Data
{
    public class Driver : Entity
    {
        public string Dni { get; set; }

        public LocationData Location { get; set; }

        public ContactData Contact { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime DniExpirationDate { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
        public DateTime AdrExpirationDate { get; set; }
        public DateTime CapExpirationDate { get; set; }
        public DateTime TachographExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
