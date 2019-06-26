using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Models.BillingModel
{
    class Suplidos
    {
        public int Suplido { get; set; }
        public string Nombre { get; set; }
        public int Unidades { get; set; }
        public double Precio { get; set; }
        public bool AñadirSiempre { get; set; }
        public bool AñadirSiCamionCompleto { get; set; }
        public bool AñadirSiViajeCompartido { get; set; }
        public string Observaciones { get; set; }

        public static List<Suplidos> CreateDummiesSuplidos()
        {
            return new List<Suplidos>
            {
                new Suplidos {
                    Suplido = 03,
                    Nombre = "BOMBA AUXILIAR/INTECO ASTU",
                    Unidades = 1,
                    Precio = 370,
                    AñadirSiempre = false,
                    AñadirSiCamionCompleto = false,
                    AñadirSiViajeCompartido = false,
                    Observaciones = ""},

                new Suplidos {
                    Suplido = 05,
                    Nombre = "SERVICIOS ESPECIALES",
                    Unidades = 0,
                    Precio = 0,
                    AñadirSiempre = false,
                    AñadirSiCamionCompleto = false,
                    AñadirSiViajeCompartido = false,
                    Observaciones = "" },

                new Suplidos {
                    Suplido = 06,
                    Nombre = "TRANSITOS EN VACIO",
                    Unidades = 1,
                    Precio = 185,
                    AñadirSiempre = false,
                    AñadirSiCamionCompleto = false,
                    AñadirSiViajeCompartido = false,
                    Observaciones = "" },

                new Suplidos {
                    Suplido = 07,
                    Nombre = "OPERADOR BUQUES(1,02)(8€TON)",
                    Unidades = 1,
                    Precio = 455,
                    AñadirSiempre = false,
                    AñadirSiCamionCompleto = false,
                    AñadirSiViajeCompartido = false,
                    Observaciones = "" },

                new Suplidos {
                    Suplido = 18,
                    Nombre = "VACIADOS CON RETORNO A PLANTA",
                    Unidades = 1,
                    Precio = 342,
                    AñadirSiempre = false,
                    AñadirSiCamionCompleto = false,
                    AñadirSiViajeCompartido = false,
                    Observaciones = "" },

                new Suplidos {
                    Suplido = 19,
                    Nombre = "VACIADOS SIN RETORNO A PLANTA",
                    Unidades = 1,
                    Precio = 368.39,
                    AñadirSiempre = false,
                    AñadirSiCamionCompleto = false,
                    AñadirSiViajeCompartido = false,
                    Observaciones = "" }
            };

        }
    }
}
