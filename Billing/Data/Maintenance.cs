using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Models.BillingModel
{
    class Maintenance
    {
        public string Fecha { get; set; }
        public string FechaFinal { get; set; }
        public int Kilometros { get; set; }
        public int Orden { get; set; }
        public string Descripción { get; set; }
        public string ObservacionesMecanico { get; set; }

        public static List<Maintenance> CreateDummiesMaintenance()
        {
            return new List<Maintenance>
            {
                new Maintenance {
                    Fecha = "27/11/2015",
                    FechaFinal = "07/03/2016",
                    Kilometros = 0,
                    Orden = 0,
                    Descripción = "",
                    ObservacionesMecanico = "Verdadero" },

                new Maintenance {
                    Fecha = "03/03/2016",
                    FechaFinal = "03/03/2016",
                    Kilometros = 0,
                    Orden = 0,
                    Descripción = "PRUEBA",
                    ObservacionesMecanico = "" }
            };

        }
    }
}
