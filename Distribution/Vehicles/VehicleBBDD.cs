using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EtasaDesktop.Distribution.Vehicles
{
    class VehicleBBDD
    {
        public static DataSet GetListVehicleData()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString);
            SqlDataAdapter sda = new SqlDataAdapter("Select *"
            + " From VehiculosBackupNuevo"
            + " Order by Codigo", con);

            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }


    }
}
