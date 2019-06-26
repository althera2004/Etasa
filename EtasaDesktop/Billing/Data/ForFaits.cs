using System.Collections.Generic;

namespace EtasaDesktop.Models.BillingModel
{
    class ForFaits
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int CodFactoria { get; set; }
        public string NombreFactoria { get; set; }
        public int CodTarifa { get; set; }
        public string NombreTarifa { get; set; }
        public int TipoIVA { get; set; }

        public static List<ForFaits> CreateDummiesForFaits()
        {
            return new List<ForFaits>
            {
                new ForFaits {
                    Codigo = 001,
                    Nombre = "FORFAIT PLANTA CEBOLLA",
                    CodFactoria = 0001,
                    NombreFactoria = "PLANTA DE CEBOLLA",
                    CodTarifa = 003,
                    NombreTarifa = "TARIFA CEBOLLA",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 001,
                    Nombre = "FORFAIT PLANTA CEBOLLA",
                    CodFactoria = 0004,
                    NombreFactoria = "PLANTA DE GAJANO",
                    CodTarifa = 003,
                    NombreTarifa = "TARIFA CEBOLLA",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 001,
                    Nombre = "FORFAIT PLANTA CEBOLLA",
                    CodFactoria = 0005,
                    NombreFactoria = "PLANTA DE DOS HERMANOS",
                    CodTarifa = 003,
                    NombreTarifa = "TARIFA CEBOLLA",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 002,
                    Nombre = "FORFAIT VICALVARO",
                    CodFactoria = 0002,
                    NombreFactoria = "PLANTA DE VICALVARO",
                    CodTarifa = 002,
                    NombreTarifa = "TARIFA VICALVARO",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 002,
                    Nombre = "FORFAIT VICALVARO",
                    CodFactoria = 0010,
                    NombreFactoria = "PLANTA DE SAN ROQUE",
                    CodTarifa = 002,
                    NombreTarifa = "TARIFA VICALVARO",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 003,
                    Nombre = "HUELVA",
                    CodFactoria = 0004,
                    NombreFactoria = "PLANTA DE GAJANO",
                    CodTarifa = 004,
                    NombreTarifa = "TARIFA HUELVA",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 003,
                    Nombre = "HUELVA",
                    CodFactoria = 0009,
                    NombreFactoria = "PLANTA DE CARTAGENA",
                    CodTarifa = 004,
                    NombreTarifa = "TARIFA HUELVA",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 003,
                    Nombre = "HUELVA",
                    CodFactoria = 0010,
                    NombreFactoria = "PLANTA DE SAN ROQUE",
                    CodTarifa = 004,
                    NombreTarifa = "TARIFA HUELVA",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 003,
                    Nombre = "HUELVA",
                    CodFactoria = 0011,
                    NombreFactoria = "REFINERÍA HUELVA",
                    CodTarifa = 004,
                    NombreTarifa = "TARIFA HUELVA",
                    TipoIVA = 0 },

                new ForFaits {
                    Codigo = 003,
                    Nombre = "HUELVA",
                    CodFactoria = 0012,
                    NombreFactoria = "REFINERÍA ALGECIRAS",
                    CodTarifa = 004,
                    NombreTarifa = "TARIFA HUELVA",
                    TipoIVA = 0 }
            };

        }
    }
}
