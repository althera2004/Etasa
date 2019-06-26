using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Auth
{
    [Serializable]
    public class Group
    {
        public int Id { get; set; }
        public bool CanAdmin { get; set; }
    }
}
