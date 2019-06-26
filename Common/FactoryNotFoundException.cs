using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    class FactoryNotFoundException : Exception
    {
        public FactoryNotFoundException()
        {
        }

        public FactoryNotFoundException(string message) : base(message)
        {
        }

        public FactoryNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
