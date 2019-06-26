using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    public class Entity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public bool Enabled { get; set; }
        public string Observations { get; set; }
    }
}
