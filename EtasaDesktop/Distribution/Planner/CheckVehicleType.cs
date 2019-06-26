using System;
using System.Configuration;
using System.Data.SqlClient;

namespace EtasaDesktop.Distribution.Data
{
    public class VehicleType
    {
        private static string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;

        //Metodo para comprobar el tipo de un vehiculo a la hora de planificar

        public static bool CheckVehicleType(int vehicleid, int orderid)
        {
            bool ValidType = false;

            int Weight = GetWeightVehicle(orderid);
            int TankVolume = GetVehicleTankVolume(vehicleid);

            if (TankVolume > Weight)
                ValidType = true;

            return ValidType;
        }

        public static int GetWeightVehicle(int orderid)
        {
            int Weight = 0;

            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT VS.Weight FROM Vehicles_Sizes as VS INNER JOIN Orders ON Orders.VehicleType = VS.Code WHERE Orders.Id = @Id";
                cmd.Parameters.AddWithValue("@Id", orderid);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Weight = Convert.ToInt32(dr["Weight"].ToString());
                    }
                }
            }

            return Weight;
        }

        public static int GetVehicleTankVolume(int vehicleid)
        {
            int TankVolume = 0;

            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT TankVolume FROM Vehicles WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", vehicleid);
                SqlDataReader dr = cmd.ExecuteReader();
                
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TankVolume = Convert.ToInt32(dr["TankVolume"].ToString());
                    }
                }
            }

            return TankVolume;
        }

        public static string GetVehicleSizeName(int code)
        {
            string Name = null;

            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT Name FROM Vehicles_Sizes WHERE Code = @code";
                cmd.Parameters.AddWithValue("@code", code);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Name =dr["Name"].ToString();
                    }
                }
            }

            return Name;
        }
    }
}
