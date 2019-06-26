using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    class DataSearchCharger
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }

        public static List<DataSearchCharger> CreateDummiesDataCharger()
        {
            return new List<DataSearchCharger>
            {
                new DataSearchCharger {
                    Codigo = 001,
                    Nombre = "CLH" },
                new DataSearchCharger {
                    Codigo = 002,
                    Nombre = "Ejemplo" },
            };
        }
    }
}
