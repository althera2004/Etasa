using EtasaDesktop.Distribution.Data;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
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
                Location = new Location(Assignment.Cab.Location.Latitude, Assignment.Cab.Location.Longitude);
            }
        }
    }
}
