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
    public class SearchCabViewModel : SearchElementViewModel
    {

        public override EnumerableRowCollection Query() {
            AssignmentsDataSet ds = new AssignmentsDataSet();
            AssignmentsDataSetTableAdapters.CabInfoTableAdapter adapt = new AssignmentsDataSetTableAdapters.CabInfoTableAdapter();
            adapt.Fill(ds.CabInfo);

            return from cab in ds.CabInfo.AsEnumerable()
                   select new
                   {
                         
                         Código = cab.Code,
                         Matricula = cab.LicensePlate,
                         Tipo = cab.TypeName,
                         Tamaño = cab.SizeName,
                         Observaciones = cab.Observations,
                         Id = cab.Id
                   };
        }
    }
}
