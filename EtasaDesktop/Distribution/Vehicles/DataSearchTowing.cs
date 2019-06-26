using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    class DataSearchTowing
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public static List<DataSearchTowing> CreateDummiesDataTowing()
        {
            return new List<DataSearchTowing>
            {
                new DataSearchTowing {
                    Codigo = 01,
                    Descripcion = "C.Ligeros" },
                new DataSearchTowing {
                    Codigo = 02,
                    Descripcion = "C.Pesados" },
            };
        }
    }
}
