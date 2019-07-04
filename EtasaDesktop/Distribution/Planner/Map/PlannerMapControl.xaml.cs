namespace EtasaDesktop.Distribution.Planner.Map
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using EtasaDesktop.Common.Data;
    using EtasaDesktop.Distribution.Data;
    using Microsoft.Maps.MapControl.WPF;

    public partial class PlannerMapControl : UserControl
    {
        bool firstTime = true;

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
            if (PlannerViewModel.BlockOrderUpdate)
            {
                Console.WriteLine("OnOrdersChanged blocked");
                return;
            }

            //Limpia los antiguos marcadores
            ClearClientMarkers();
            ClearFactoryMarkers();

            
            //Crea nuevos marcadores
            foreach (Order order in orders)
            {
                AddClientMarker(order);
                AddFactoryMarker(order);
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


            if (firstTime)
            {
                Center(list);
                firstTime = false;
            }
           
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

                cameraLocations = cameraLocations.Where(c => c.Latitude != 0).Distinct().ToList();

                /*foreach (Location location in cameraLocations)
                {
                    if (location.Latitude > maxLat) { maxLat = location.Latitude; }
                    if (location.Latitude < minLat) { minLat = location.Latitude; }
                    if (location.Longitude > maxLon) { maxLon = location.Longitude; }
                    if (location.Longitude < minLon) { minLon = location.Longitude; }
                }*/

                minLat = cameraLocations.Min(cl => cl.Latitude);
                minLon = cameraLocations.Min(cl => cl.Longitude);
                maxLat = cameraLocations.Max(cl => cl.Latitude);
                maxLon = cameraLocations.Max(cl => cl.Longitude);

                //44.488444, -10.267331 (left-up)                   
                //42.476091, 5.416559 (right-up)
                //36.199297, -11.641810 (left-down)
                //35.339791, 1.732230 (right-down) 
            }

            var padding = 0.10;
            var latPadding = (maxLat - minLat) * padding;
            var lonPadding = (maxLon - minLon) * padding;

            //cameraLocations.Add(new Location(minLat - latPadding, maxLon + lonPadding));
            //cameraLocations.Add(new Location(maxLat + latPadding, minLon - lonPadding));
            //new LocationRect(cameraLocations)
            Map.SetView(new Location(40.4381311, -3.8196198), 6);


            if (Map.ZoomLevel > 15)
            {
                Map.ZoomLevel = 15;
            }
        }

        private void AddClientMarker(Order order)
        {
            var clientViewModel = GetClientMarkerByOrder(order);
            if (order.HexColor == null)
            {
                order.HexColor = "#C0C0C0";
            }

            if (clientViewModel == null)
            {
                clientViewModel = new MarkerClientViewModel
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
                {
                    clientViewModel.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(order.HexColor));
                }

                //clientViewModel.RefreshNum();

                clientViewModel.Num = clientViewModel.Orders.Sum(o => o.RequestedAmount);
            }
        }

        private void AddFactoryMarker(Order order)
        {

            MarkerFactoryViewModel factoryViewModel = new MarkerFactoryViewModel
            {
                Factory = order.Factory
            };

            if (order.Client.Name.StartsWith("MGP", StringComparison.OrdinalIgnoreCase))
            {
                //factoryViewModel.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(GetColorEmergency(Convert.ToInt32(order.Factory.Id.ToString()))));
                factoryViewModel.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(order.Factory.Colors.Urgent));
            }
            else
            {
                factoryViewModel.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(order.Factory.HexColor));
            }           
           
            _markerFactories.Add(factoryViewModel);      
            // };
        }

        private string GetColorEmergency(int orderFactoryId)
        {
            PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            string ColorEmergency = "";
            DataTable ColorEmergencies = new DataTable();
            PlannerDataSetTableAdapters.FactoriesColorsTableAdapter adapter = new PlannerDataSetTableAdapters.FactoriesColorsTableAdapter();
            ColorEmergency = adapter.ScalarQueryUrgentColor(Convert.ToInt32(orderFactoryId)).ToString().Trim();
            return ColorEmergency;
        }

        private void AddAssignmentMarker(Assignment assignment)
        {
           if (!CheckIfAssignmentMarkerExists(assignment))
           {
                try
                {
                    MarkerAssignmentViewModel assignmentViewModel = new MarkerAssignmentViewModel
                    {
                        Assignment = assignment,
                        Fill = Brushes.RoyalBlue
                    };

                    _markerAssignments.Add(assignmentViewModel);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
           };
        }

        private void AddRoute()
        {

        }

        private void DrawClientMarkers()
        {
            //si es false pintamos de color rojo los pedido para distinguirlas de las factorias 
            bool type = false;
            float logitudeNoFind = Convert.ToSingle(-9.942771);
            LocationCollection locationCollection = new LocationCollection();
            foreach (var mcvm in _markerClients)
            {
                var marker = new MarkerClient(mcvm, ref logitudeNoFind);
                ClientLayer.Children.Add(marker);
                //solo agregamos los peddios con localizaciones que estan dentro del mapa (depedencia google maps)
                //el resto no se introducent
                if (marker.localizacion != null)
                {
                    locationCollection.Add(marker.localizacion);
                }

                if (logitudeNoFind > 5.4)
                {
                    logitudeNoFind = Convert.ToSingle(-9.942771);
                }
            }
            //pintamos el mapa con las lineas entre los pedidos
            PaintLineMaps(locationCollection, type);
        }

        private void DrawFactoryMarkers()
        {
            //si es true pintamos de color rojo las factorias para distinguirlas de los pedidos 
            bool type = true;

            Double logitudeNotFind = Convert.ToSingle(-9.942771);
            var locationCollection = new LocationCollection();
            foreach (var mfvm in _markerFactories.DistinctBy(p => p.Factory.Id))
            {
                
                var marker = new MarkerFactory(mfvm, ref logitudeNotFind);  
                //agregamos los punteros de las factorias 
                FactoryLayer.Children.Add(marker);
                //solo agregamos la localizaciones que estan dentro del mapa 
                //el resto no se introducent
                if (marker.localizacion != null)
                {
                    locationCollection.Add(marker.localizacion);
                }

                // si pasamos la longitud sobre ponemos la factorias una encima la otra (mostrar factorias) 
                if (logitudeNotFind > 5.4)
                {
                    logitudeNotFind = Convert.ToSingle(-9.942771);
                }
            }
            //pintamos el mapa
            PaintLineMaps(locationCollection,type);

        }

        private void DrawFleetMarkers()
        {
            Double logitudeNotFind = Convert.ToSingle(-9.942771);
            foreach (MarkerAssignmentViewModel mavm in _markerAssignments)
            {
                var marker = new MarkerDriver(mavm, ref logitudeNotFind);
                FleetLayer.Children.Add(marker);
            }
        }
        private void DrawRoute()
        {

        }

        private void PaintLineMaps(LocationCollection locationCollection, bool type)
        {
            MapPolyline polyline = new MapPolyline();
            //si es true pintamos la linia naranja entre factorias 
            if (type)
            {
                polyline.Stroke = new SolidColorBrush(Colors.Orange);
            }
            //si es false pintamos la linia roja ya que es entre pedidos
            else
            {
                polyline.Stroke = new SolidColorBrush(Colors.Red);
            }
            polyline.StrokeThickness = 4;
            polyline.Locations = locationCollection;
            FactoryLayer.Children.Add(polyline);
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
                Map.Mode = new AerialMode
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