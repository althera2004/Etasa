using EtasaDesktop.Common.Data;
using EtasaDesktop.Distribution.Data;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EtasaDesktop.Distribution.Planner.Map
{
    public partial class PlannerMapControl : UserControl
    {

        #region Properties

        #region Orders
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnOrdersChanged(value);
            }
        }
        protected virtual void OnOrdersChanged(IEnumerable<Order> orders)
        {

            //Limpia los antiguos marcadores
            ClearClientMarkers();
            ClearFactoryMarkers();

            //Crea nuevos marcadores
            foreach (Order order in orders)
            {
                AddClientMarker(order);
                AddFactoryMarker(order.Factory);
            }

            //Ordena los marcadores
            SortClientMarkers();
            SortFactoryMarkers();

            //Muestra los marcadores en el mapa
            DrawClientMarkers();
            DrawFactoryMarkers();
        }
        #endregion

        #region Assignments
        private ObservableCollection<Assignment> _assignments;
        public ObservableCollection<Assignment> Assignments
        {
            get => _assignments;
            set
            {
                _assignments = value;
                OnAssignmentsChanged(value);
            }
        }
        protected virtual void OnAssignmentsChanged(IEnumerable<Assignment> assignments)
        {
            //Limpia los antiguos marcadores
            ClearFleetMarkers();

            //Crea nuevos marcadores
            foreach (Assignment assign in assignments)
            {
                AddAssignmentMarker(assign);
            }

            //Ordena los marcadores
            SortFleetMarkers();

            //Muestra los marcadores en el mapa
            DrawFleetMarkers();
        }





        #endregion

        #region Markers Lists
        private List<MarkerClientViewModel> _markerClients = new List<MarkerClientViewModel>();
        private List<MarkerFactoryViewModel> _markerFactories = new List<MarkerFactoryViewModel>();
        private List<MarkerAssignmentViewModel> _markerAssignments = new List<MarkerAssignmentViewModel>();
        #endregion

        #region Layers
        private MapLayer ClientLayer;
        private MapLayer FactoryLayer;
        private MapLayer FleetLayer;
        private MapLayer RouteLayer;
        #endregion

        #endregion

        #region Constructor

        public PlannerMapControl()
        {

            InitializeComponent();

            ClientLayer = new MapLayer();
            FactoryLayer = new MapLayer();
            FleetLayer = new MapLayer();
            RouteLayer = new MapLayer();

            Map.Children.Add(FactoryLayer);
            Map.Children.Add(ClientLayer);
            Map.Children.Add(FleetLayer);
            Map.Children.Add(RouteLayer);
        }

        #endregion


        public void Center()
        {
            var list = new List<MarkerViewModel>();

            list.AddRange(_markerClients);
            list.AddRange(_markerFactories);
            list.AddRange(_markerAssignments);

            Center(list);
        }
        private void Center(List<MarkerViewModel> markers)
        {
            double maxLat = -80;
            double minLat = 80;
            double maxLon = -180;
            double minLon = 180;

            List<Location> cameraLocations = new List<Location>();

            if (markers != null && markers.Count > 0)
            {
                foreach (MarkerViewModel locatable in markers)
                {
                    cameraLocations.Add(locatable.Location);
                }

                foreach (Location location in cameraLocations)
                {
                    if (location.Latitude > maxLat) { maxLat = location.Latitude; }
                    if (location.Latitude < minLat) { minLat = location.Latitude; }
                    if (location.Longitude > maxLon) { maxLon = location.Longitude; }
                    if (location.Longitude < minLon) { minLon = location.Longitude; }
                }

            }

            var padding = 0.10;
            var latPadding = (maxLat - minLat) * padding;
            var lonPadding = (maxLon - minLon) * padding;

            cameraLocations.Add(new Location(minLat - latPadding, maxLon + lonPadding));
            cameraLocations.Add(new Location(maxLat + latPadding, minLon - lonPadding));

            Map.SetView(new LocationRect(cameraLocations));


            if (Map.ZoomLevel > 15)
            {
                Map.ZoomLevel = 15;
            }
        }


        private void AddClientMarker(Order order)
        {
            MarkerClientViewModel clientViewModel = GetClientMarkerByOrder(order);

            if (order.HexColor == null)
            {
                order.HexColor = "#C0C0C0";
            }
            if (clientViewModel == null)
            {
                clientViewModel = new MarkerClientViewModel()
                {
                    Client = order.Client,
                    Orders = new ObservableCollection<Order>() { order },
                    Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(order.HexColor))
                };

                _markerClients.Add(clientViewModel);
            }
            else
            {
                clientViewModel.Orders.Add(order);

                if (order.IsLastDay)
                    clientViewModel.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(order.HexColor));

                clientViewModel.RefreshNum();
            }
        }
        private void AddFactoryMarker(Factory factory)
        {
            if (CheckIfFactoryMarkerExists(factory))
            {
                MarkerFactoryViewModel factoryViewModel = new MarkerFactoryViewModel()
                {
                    Factory = factory,
                    Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(factory.HexColor))
                };

                _markerFactories.Add(factoryViewModel);
            };
        }
        private void AddAssignmentMarker(Assignment assignment)
        {
            if (CheckIfAssignmentMarkerExists(assignment))
            {
                MarkerAssignmentViewModel assignmentViewModel = new MarkerAssignmentViewModel()
                {
                    Assignment = assignment,
                    Fill = Brushes.RoyalBlue
                };

                _markerAssignments.Add(assignmentViewModel);
            };
        }
        private void AddRoute()
        {

        }


        private void DrawClientMarkers()
        {
            foreach (MarkerClientViewModel mcvm in _markerClients)
            {
                var marker = new MarkerClient(mcvm);
                ClientLayer.Children.Add(marker);
            }
        }
        private void DrawFactoryMarkers()
        {
            foreach (MarkerFactoryViewModel mfvm in _markerFactories)
            {
                var marker = new MarkerFactory(mfvm);
                FactoryLayer.Children.Add(marker);
            }
        }
        private void DrawFleetMarkers()
        {
            foreach (MarkerAssignmentViewModel mavm in _markerAssignments)
            {
                var marker = new MarkerDriver(mavm);
                FleetLayer.Children.Add(marker);
            }
        }
        private void DrawRoute()
        {

        }


        private void SortClientMarkers()
        {
            // Ordena los marcadores por coordenadas para que no se superponga el marcador trasero con el posterior
            _markerClients = _markerClients.OrderByDescending(o => o.Orders[0].Client.Location.Latitude).ThenBy(o => o.Orders[0].Client.Location.Longitude).ToList();
        }
        private void SortFactoryMarkers()
        {
            _markerFactories = _markerFactories.OrderByDescending(o => o.Factory.Location.Latitude).ThenBy(o => o.Factory.Location.Longitude).ToList();
        }
        private void SortFleetMarkers()
        {
            _markerAssignments = _markerAssignments
                .OrderByDescending(o => o.Assignment.Cab.Location != null ? o.Assignment.Cab.Location.Latitude : -1)
                .ThenBy(o => o.Assignment.Cab.Location != null ? o.Assignment.Cab.Location.Longitude : -1).ToList();
        }


        private void ClearClientMarkers()
        {
            ClientLayer.Children.Clear();
            _markerClients.Clear();
        }
        private void ClearFactoryMarkers()
        {
            FactoryLayer.Children.Clear();
            _markerFactories.Clear();
        }
        private void ClearFleetMarkers()
        {
            FleetLayer.Children.Clear();
            _markerAssignments.Clear();
        }
        private void ClearRoute()
        {

        }


        private MarkerClientViewModel GetClientMarkerByOrder(Order order)
        {
            return _markerClients.FirstOrDefault(cm => cm.Client.Id == order.Client.Id);
        }

        private bool CheckIfFactoryMarkerExists(Factory factory)
        {
            return _markerFactories.Any(FactoryMarker => FactoryMarker.Factory.Id == factory.Id);
        }
        private bool CheckIfAssignmentMarkerExists(Assignment assignment)
        {
            return _markerAssignments.Any(AssignmentMarker => AssignmentMarker.Assignment.Id == assignment.Id);
        }
        

        #region Events

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Center();
        }

        public void MapCenterButton_Click(object sender, RoutedEventArgs e)
        {
            Center();
        }

        private void ToggleMapMode_Click(object sender, RoutedEventArgs e)
        {
            if (Map.Mode is RoadMode)
            {
                Map.Mode = new AerialMode()
                {
                    Labels = true
                };

                MapTypeButtonIcon.Source = new BitmapImage(new Uri("pack://application:,,,/EtasaDesktop;component/Resources/Images/map_road.png"));
                MapTypeButtonText.Content = "Mapa";
            }
            else
            {
                Map.Mode = new RoadMode();

                MapTypeButtonIcon.Source = new BitmapImage(new Uri("pack://application:,,,/EtasaDesktop;component/Resources/Images/map_satellite.png"));
                MapTypeButtonText.Content = "Satélite";
            }
        }

        #endregion


    }
}
