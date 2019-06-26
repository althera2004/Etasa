using EtasaDesktop.Common.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Assignments
{
    public class SearchTrailerViewModel : SearchElementViewModel
    {

        public override EnumerableRowCollection Query() {

            AssignmentsDataSet ds = new AssignmentsDataSet();
            AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter adapt = new AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter();
            adapt.Fill(ds.TrailerInfo);

            return from trailer in ds.TrailerInfo.AsEnumerable()
                     select new
                     {                     
                         Código = trailer.Code,
                         Matricula = trailer.LicensePlate,
                         Tipo = trailer.TypeName,
                         Tamaño = trailer.SizeName,
                         Capacidad = trailer.TankVolume,
                         Observaciones = trailer.Observations,
                         Id = trailer.Id
                     };
        }
    }
}
