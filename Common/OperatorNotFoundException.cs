using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    class OperatorNotFoundException : Exception
    {
        public OperatorNotFoundException()
        {
        }

        public OperatorNotFoundException(string message) : base(message)
        {
        }

        public OperatorNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
