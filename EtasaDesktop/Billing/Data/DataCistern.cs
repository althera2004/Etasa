using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Models.BillingModel
{
    class DataCistern
    {
        public string Nombre { get; set; }
        public int Capacidad { get; set; }

        public static List<DataCistern> CreateDummiesDataCistern()
        {
            return new List<DataCistern>
            {
                new DataCistern {
                    Nombre = "Tanque 1",
                    Capacidad = 13000 },
                new DataCistern {
                    Nombre = "Tanque 2",
                    Capacidad = 13000 },
                new DataCistern {
                    Nombre = "Tanque 3",
                    Capacidad = 0 },
                new DataCistern {
                    Nombre = "Tanque 4",
                    Capacidad = 0 },
                new DataCistern {
                    Nombre = "Tanque 5",
                    Capacidad = 0 },
                new DataCistern {
                    Nombre = "Tanque 6",
                    Capacidad = 0 },
                new DataCistern {
                    Nombre = "Tanque 7",
                    Capacidad = 0 },
                new DataCistern {
                    Nombre = "Tanque 8",
                    Capacidad = 0 },
                new DataCistern {
                    Nombre = "Tanque 9",
                    Capacidad = 0 },
                new DataCistern {
                    Nombre = "Tanque 10",
                    Capacidad = 0 },
                new DataCistern {
                    Nombre = "Tanque 11",
                    Capacidad = 0 },
            };
        }
    }
}
