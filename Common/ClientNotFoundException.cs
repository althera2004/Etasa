using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    class ClientNotFoundException : Exception
    {
        public ClientNotFoundException()
        {
        }

        public ClientNotFoundException(string message) : base(message)
        {
        }

        public ClientNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
