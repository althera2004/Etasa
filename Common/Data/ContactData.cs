using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    [Serializable]
    public class ContactData
    {
        public string Name { get; set; }

        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string PhoneMobile { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }
    }
}
