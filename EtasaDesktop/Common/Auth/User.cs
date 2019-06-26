using EtasaDesktop.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Auth
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ContactData Contact { get; set; }

        public Group Group { get; set; }

        public bool Enabled { get; set; }
        public string Observations { get; set; }
    }
}
