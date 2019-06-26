using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Data
{
    public class Assignment
    {
        public long Id { get; set; }
        public Driver Driver { get; set; }
        public Cab Cab { get; set; }
        public Trailer Trailer { get; set; }
        public string MessageOverAmount { get; set; }

        public DateTime Date { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int TotalAmountAssigment { get; set; }

        public int? TripId{ get; set; }
        public int RoutesId { get; set; }
        public int FactoryId { get; set; }

        public string FactoryName { get; set; }

        public bool Repeat { get; set; }
        public bool Enabled { get; set; }
        public string Observations { get; set; }
    }
}
