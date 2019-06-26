using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Models.BillingModel
{
    class TamañoVehiculo
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int CodPropuesto { get; set; }
        public string NombrePropuesto { get; set; }

        public static List<TamañoVehiculo> CreateDummiesTamañoVehiculo()
        {
            return new List<TamañoVehiculo>
            {
                new TamañoVehiculo {
                    Codigo = "A",
                    Nombre = "Pequeño",
                    CodPropuesto = 3,
                    NombrePropuesto = "Rigido 2 ejes" },

                new TamañoVehiculo {
                    Codigo = "B",
                    Nombre = "Mediano",
                    CodPropuesto = 2,
                    NombrePropuesto = "Rigido 3 ejes" },

                new TamañoVehiculo {
                    Codigo = "C",
                    Nombre = "Grande",
                    CodPropuesto = 0,
                    NombrePropuesto = "Articulado 3 ejes" },

                new TamañoVehiculo {
                    Codigo = "D",
                    Nombre = "Contenedor",
                    CodPropuesto = 5,
                    NombrePropuesto = "Contenedor" },

                new TamañoVehiculo {
                    Codigo = "D",
                    Nombre = "Vagon",
                    CodPropuesto = 4,
                    NombrePropuesto = "vagon" },

                new TamañoVehiculo {
                    Codigo = "D",
                    Nombre = "Mediano Articulo",
                    CodPropuesto = 1,
                    NombrePropuesto = "Articulado 2 ejes" }
            };

        }
    }
}
