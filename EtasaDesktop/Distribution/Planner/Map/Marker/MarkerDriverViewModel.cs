using EtasaDesktop.Distribution.Data;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Planner.Map
{
    public class MarkerAssignmentViewModel : MarkerViewModel
    {
        private Assignment _assignment;

        public Assignment Assignment
        {
            get => _assignment;
            set
            {
                Set(ref _assignment, value);
                Title = Assignment.Driver.Name;
                Content = "Asignado a " + Assignment.Cab.Code + " con " + Assignment.Trailer.Code + " (" + Assignment.Trailer.TankVolume + ")";
                if (Assignment.Cab.Location != null)
                {
                    Location = new Location(Assignment.Cab.Location.Latitude, Assignment.Cab.Location.Longitude);
                }
                else
                {
                    GetGPS(Assignment.Cab.Id);
                }
            }
        }

        private void GetGPS(long id)
        {
            using (var cmd = new SqlCommand("SELECT TOP(1) Id, VehicleId, Latitude, Longitude, TimeStamp FROM Vehicles_Locations WHERE (VehicleId = " + id.ToString() + ") ORDER BY TimeStamp DESC"))
            {
                using (var cnn = new SqlConnection(Properties.Settings.Default.EtasaConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            if (rdr.HasRows)
                            {
                                this.Location = new Location
                                {
                                    Altitude = 0,
                                    AltitudeReference = AltitudeReference.Ground,
                                    Latitude = Convert.ToDouble(rdr.GetFloat(2)),
                                    Longitude = Convert.ToDouble(rdr.GetFloat(3))
                                };
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }
    }
}