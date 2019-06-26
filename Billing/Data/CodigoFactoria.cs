using System;
using System.Collections.Generic;

namespace EtasaDesktop.Models
{
    class CodigoFactoria
    {
        public int Codigo { get; set; }
        public string Factoria { get; set; }
        public string CodigoComunicacion { get; set; }
        public string CodigoFacturacion { get; set; }
        public string Almacen { get; set; }
        
        public static List<CodigoFactoria> CreateDummiesFactorial()
        {
            return new List<CodigoFactoria>
            {
                new CodigoFactoria {
                    Codigo = 0001,
                    Factoria = "Planta de San Roque",
                    CodigoComunicacion = "N001",
                    CodigoFacturacion = "N001",
                    Almacen = "A001" },

                new CodigoFactoria {
                    Codigo = 0002,
                    Factoria = "Planta de Cartagena",
                    CodigoComunicacion = "N002",
                    CodigoFacturacion = "N002",
                    Almacen = "A002" },

                new CodigoFactoria {
                    Codigo = 0003,
                    Factoria = "Planta Paterna",
                    CodigoComunicacion = "N003",
                    CodigoFacturacion = "N003",
                    Almacen = "A003" },

                new CodigoFactoria {
                    Codigo = 0004,
                    Factoria = "Planta de Koalagas",
                    CodigoComunicacion = "N004",
                    CodigoFacturacion = "N004",
                    Almacen = "A004" },

                new CodigoFactoria {
                    Codigo = 0005,
                    Factoria = "Planta de Puig-Reig",
                    CodigoComunicacion = "N005",
                    CodigoFacturacion = "N005",
                    Almacen = "A005" },

                new CodigoFactoria {
                    Codigo = 0006,
                    Factoria = "Planta Maside",
                    CodigoComunicacion = "N006",
                    CodigoFacturacion = "N006",
                    Almacen = "A006" },

                new CodigoFactoria {
                    Codigo = 0007,
                    Factoria = "Planta de dos hermanas",
                    CodigoComunicacion = "N007",
                    CodigoFacturacion = "N007",
                    Almacen = "A007" },

                new CodigoFactoria {
                    Codigo = 0008,
                    Factoria = "Planta de cebolla",
                    CodigoComunicacion = "N008",
                    CodigoFacturacion = "N008",
                    Almacen = "A008" },

                new CodigoFactoria {
                    Codigo = 0009,
                    Factoria = "Refineria de Huelva",
                    CodigoComunicacion = "N009",
                    CodigoFacturacion = "N009",
                    Almacen = "A009" },

                new CodigoFactoria {
                    Codigo = 0010,
                    Factoria = "Refineria Algeciras",
                    CodigoComunicacion = "N010",
                    CodigoFacturacion = "N010",
                    Almacen = "A010" }
            };
        }
    }
}
