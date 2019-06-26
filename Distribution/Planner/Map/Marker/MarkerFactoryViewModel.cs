using EtasaDesktop.Common.Data;
using EtasaDesktop.Common.Tools;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EtasaDesktop.Distribution.Planner.Map
{
    public class MarkerFactoryViewModel : MarkerViewModel
    {
        private Factory _factory;

        public Factory Factory
        {
            get => _factory;
            set
            {
                Set(ref _factory, value);
                Title = _factory.Name;
                Location = new Location(_factory.Location.Latitude, _factory.Location.Longitude);
            }
        }
    }
}
