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
    public class SearchDriverViewModel : SearchElementViewModel
    {

        public override EnumerableRowCollection Query() {
            AssignmentsDataSet ds = new AssignmentsDataSet();
            AssignmentsDataSetTableAdapters.DriversInfoTableAdapter adapt = new AssignmentsDataSetTableAdapters.DriversInfoTableAdapter();
            adapt.Fill(ds.DriversInfo);

            return from driver in ds.DriversInfo.AsEnumerable()
                   select new
                   {
                              
                              Código = driver.Code,
                              Nombre = driver.Name,
                              Id = driver.Id
                   };
        }
    }
}
