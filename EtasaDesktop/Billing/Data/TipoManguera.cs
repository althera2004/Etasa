using System.Collections.Generic;

namespace EtasaDesktop.Models.BillingModel
{
    class TipoManguera
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int CodPropuesto { get; set; }
        public string NombrePropuesto { get; set; }

        public static List<TipoManguera> CreateDummiesTipoManguera()
        {
            return new List<TipoManguera>
            {
                new TipoManguera {
                    Codigo = "A",
                    Nombre = "20 Metros",
                    CodPropuesto = 3,
                    NombrePropuesto = "20 Metros" },

                new TipoManguera {
                    Codigo = "B",
                    Nombre = "40 Metros",
                    CodPropuesto = 4,
                    NombrePropuesto = "40 Metros" }
            };

        }
    }
}
