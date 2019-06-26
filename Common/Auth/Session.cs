using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Auth
{
    [Serializable]
    public class Session
    {
        public User User { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
