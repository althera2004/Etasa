
namespace EtasaDesktop.Distribution.Planner.Map
{
    using EtasaDesktop.Common.Data;
    using Microsoft.Maps.MapControl.WPF;

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
