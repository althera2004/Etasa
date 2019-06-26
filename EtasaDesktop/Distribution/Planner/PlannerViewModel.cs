using EtasaDesktop.Common.Data;
using EtasaDesktop.Distribution.Assignments;
using EtasaDesktop.Distribution.Data;
using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace EtasaDesktop.Distribution.Planner
{

    public static class ExtensionMethods
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable?.Any() != true;
        }
    }



    public class PlannerViewModel : ViewModelBase
    {
        private string con = Properties.Settings.Default.EtasaConnectionString;

        public static bool BlockOrderUpdate { get; set; }
        public static bool BlockFilterListEvent { get; set; }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (value != _selectedDate)
                {
                    Set(ref _selectedDate, value);
                    BlockFilterListEvent = true;
                    UpdateOrders(_selectedDate);
                    BlockFilterListEvent = false;
                    UpdateAssignments(_selectedDate);
                }
            }
        }

        public List<FactoryColors> _colors;


        public PlannerViewModel()
        {
            

            InitOrders();
            InitAssignments();
            _colors = new List<FactoryColors>();

            SelectedDate = DateTime.Today;
            Refresh();
        }


        public void Refresh()
        {
            DateTime Fechainicio = DateTime.Now;
            UpdateColors();

            DateTime FechaFinal =  DateTime.Now;
            Console.WriteLine( "1: " + diferenceHoursMinutesSecondMilisecons(Fechainicio, FechaFinal));

            BlockOrderUpdate = true;
            UpdateOrders(SelectedDate);
            BlockOrderUpdate = false;
            DateTime FechaFinal2 =  DateTime.Now;
            Console.WriteLine("2: " + diferenceHoursMinutesSecondMilisecons(FechaFinal, FechaFinal2));
               
            updateDataAssigment(SelectedDate);
            DateTime FechaFinal3 = DateTime.Now;
            Console.WriteLine("3: " + diferenceHoursMinutesSecondMilisecons(FechaFinal2, FechaFinal3));
         
            UpdateAssignments(SelectedDate);
            DateTime FechaFinal4 = DateTime.Now;
            Console.WriteLine("4: " + diferenceHoursMinutesSecondMilisecons(FechaFinal3, FechaFinal4));
            

        }

        public string diferenceHoursMinutesSecondMilisecons(DateTime FechaInicio, DateTime FechaFin)
        {
            string mensaje = "";
            var diffInSeconds = (FechaFin - FechaInicio).TotalSeconds;
            var diffInHours = (FechaFin - FechaInicio).TotalHours;
            var diffInMilisecons = (FechaFin - FechaInicio).TotalMilliseconds;
            var diffInMinutos = (FechaFin - FechaInicio).TotalMinutes;

            mensaje = "fecha inicio : " + FechaInicio + " fecha final : " + FechaFin + " la diferencia en horas es: " + diffInHours.ToString() + " en Minutos : " + diffInMinutos.ToString() + " En segundos: " + diffInSeconds.ToString() + " en milisegundo : " + diffInMilisecons.ToString();

            return mensaje;
        }

        private void UpdateColors()
        {
            _colors.Clear();
            _colors = PlannerRequester.RequestColors().ToList();
        }

        // Juan Castilla - no se usa
        /*public IEnumerable<Order> GetAssignmentOrdersToNew(ObservableCollection<Order> orders,long assignment, int IdRoute)
        {
            var reqOrders = PlannerRequester.RequestOrders(orders, assignment, IdRoute);
            foreach (Order order in reqOrders)
            {
                SetColor(order);
                yield return order;
            }
        }*/

        private void UpdateAssignmentOrders(long assignment)
        {
            //var assign = Assignments.First(assignvm => assignvm.Assignment.Id == assignment);
            //if (assign != null)
            //{
            //    assign.Orders.Clear();

            //    var reqOrders = PlannerRequester.RequestOrders(assign.Assignment.Id);
            //    foreach (Order order in reqOrders)
            //    {
            //        SetColor(order);
            //        assign.Orders.Add(order);
            //    }
            //}
        }

        private void SetColor(Order order)
        {
            string putEmergencycolor = "";
            var colors = _colors.Where(pc => pc.FactoryId == order.Factory.Id);
            System.Windows.Media.Color color;
            bool isLastDay;
            if (order.FinalDate == SelectedDate)
            {
                color = colors.Count() > 0 ? colors.ToArray()[0].FinalDayColor : Colors.Gray;
                isLastDay = true;
            }
            else
            {
                color = colors.Count() > 0 ? colors.ToArray()[0].ClientColor : Colors.Gray;
                isLastDay = false;
            }
            order.HexColor = ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
            order.IsLastDay = isLastDay;

            var fcolors = _colors.Where(pc => pc.FactoryId == order.Factory.Id);
            var fcolor = fcolors.Count() > 0 ? fcolors.ToArray()[0].FactoryColor : Colors.Gray;
            order.Factory.HexColor = ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(fcolor.A, fcolor.R, fcolor.G, fcolor.B));

            //si el nombre del cliente contiene en las primeras inciales MGC es un vehiculo de emergencia y lo ponemos con el color de emergencia de la factoria
            putEmergencycolor = order.Client.Name.Substring(0, 3);
            if (putEmergencycolor == "MRG")
            {
                order.HexColor = GetColorEmergency(order.Factory.Id);
            }
            else
            {
                order.HexColor = order.Factory.HexColor;
            }




        }
        public static string GetColorEmergency(int orderFactoryId)
        {
            PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            string ColorEmergency = "";
            DataTable ColorEmergencies = new DataTable();
            PlannerDataSetTableAdapters.FactoriesColorsTableAdapter adapter = new PlannerDataSetTableAdapters.FactoriesColorsTableAdapter();
            ColorEmergency = adapter.ScalarQueryUrgentColor(Convert.ToInt32(orderFactoryId)).ToString().Trim();
            return ColorEmergency;
        }

        #region ASSIGNMENTS

        public ObservableCollection<PlannerAssignmentViewModel> AssignmentRoutes { get; private set; }
        public ObservableCollection<Assignment> Assignments { get; private set; }
        public ObservableCollection<Distribution.Data.Trailer> OrdersVehicles { get; private set; }

        private string _filterVehicle;


        public string FilterVehicles
        {
            get => _filterVehicle;
            set
            {
                if (value != _filterVehicle)
                {
                    Set(ref _filterVehicle, value);
                    UpdateAssignmentsFilterVehicle(_selectedDate, _filterVehicle);
                }
            }
        }
        private void InitAssignments()
        {
            Assignments = new ObservableCollection<Assignment>();
            OrdersVehicles = new ObservableCollection<Distribution.Data.Trailer>();
            Assignments.CollectionChanged += OnAssignmentCollectionChanged;
            AssignmentRoutes = new ObservableCollection<PlannerAssignmentViewModel>();
        }
        private void UpdateAssignments(DateTime dateTime)
        {
            AssignmentRoutes.Clear();
            Assignments.Clear();
            OrdersVehicles.Clear();

            //asignacion de ruta aqui
            var reqAssigns = PlannerRequester.RequestAssignments(dateTime,"");

            PlannerAssignmentViewModel plannerAssignment;
            foreach (Assignment assignment in reqAssigns.DistinctBy(p => p.RoutesId))
            {
                try
                {
                    plannerAssignment = new PlannerAssignmentViewModel(assignment);
                    AssignmentRoutes.Add(plannerAssignment);
                    IEnumerable<Order> reqOrders = PlannerRequester.RequestOrders(Orders, assignment.Id, assignment.RoutesId);

                    if (reqOrders.Count() > 0)
                    {
                        foreach (Order Order in reqOrders)
                        {
                            if (Order != null)
                            {
                                plannerAssignment.test.Add(Order);

                            }
                        }
                    }

                }

                catch (Exception excp)
                {
                    Console.WriteLine(excp.Message);

                }
            }
            //rellenamos el filtro de matricula 
            foreach (Assignment assignment in reqAssigns.DistinctBy(p => p.Trailer.Id))
            {
                OrdersVehicles.Add(assignment.Trailer);
            }       
        }

        private void UpdateAssignmentsFilterVehicle(DateTime dateTime,string filterVehicle)
        {
            AssignmentRoutes.Clear();
            Assignments.Clear();

            //asignacion de ruta aqui filtrado por los codigo vehiculos seleccionados
            var reqAssigns = PlannerRequester.RequestAssignments(dateTime, filterVehicle);

            PlannerAssignmentViewModel plannerAssignment;
            foreach (Assignment assignment in reqAssigns.DistinctBy(p => p.RoutesId))
            {
                try
                {
                    plannerAssignment = new PlannerAssignmentViewModel(assignment);
                    AssignmentRoutes.Add(plannerAssignment);
                    IEnumerable<Order> reqOrders = PlannerRequester.RequestOrders(Orders, assignment.Id, assignment.RoutesId);

                    if (reqOrders.Count() > 0)
                    {
                        foreach (Order Order in reqOrders)
                        {
                            if (Order != null)
                            {
                                plannerAssignment.test.Add(Order);

                            }
                        }
                    }

                }

                catch (Exception excp)
                {
                    Console.WriteLine(excp.Message);

                }
            }
        }

        //primero actualizamos los datos de las asignaciones
        private void updateDataAssigment(DateTime dateTime)
        {
            bool totalAmount = true;

            //asignacion de ruta aqui
            var reqAssigns = PlannerRequester.RequestAssignments(dateTime,"");

            PlannerAssignmentViewModel plannerAssignment2;
            foreach (Assignment assignment in reqAssigns.DistinctBy(p => p.RoutesId))
            {
                try
                {
                    plannerAssignment2 = new PlannerAssignmentViewModel(assignment);
                    AssignmentRoutes.Add(plannerAssignment2);
                    IEnumerable<Order> reqOrders = PlannerRequester.RequestOrders(Orders, assignment.Id, assignment.RoutesId);

                    if (reqOrders.Count() > 0)
                    {
                        foreach (Order Order in reqOrders)
                        {
                            if (Order != null)
                            {
                                plannerAssignment2.test.Add(Order);
                            }
                        }
                        //actualizamos los datos de la ruta de la asignacion 
                        totalAmount = UpdateTotalAmount(plannerAssignment2, assignment.Id, assignment.RoutesId);
                    }

                }
                catch (Exception excp)
                {
                    Console.WriteLine(excp.Message);

                }
            }
        }


        public bool UpdateTotalAmount(PlannerAssignmentViewModel listOrder, long assignmentId, int routeId)
        {
            bool ok = true;
            //recorremos todos los pedidos para actualizar los datos de la ruta totalamount y mensaje
            var listOrders = new List<Order>(listOrder.test);
            int sumaacumuladadeCntidadepedidos = 0;
            bool reponse = true;
            PlannerDataSet.RoutesRow rowfinalupdate = null;
            PlannerDataSetTableAdapters.RoutesTableAdapter routestablefinal = new PlannerDataSetTableAdapters.RoutesTableAdapter();
            //miramos si hay pedidos en al lista
            if (listOrders.Count > 0)
            {
                    PlannerDataSetTableAdapters.RoutesTableAdapter routestable = new PlannerDataSetTableAdapters.RoutesTableAdapter();
                    PlannerDataSet.RoutesDataTable dataTableroutes = routestable.GetDatarRoutedIdByAssigmentandIdAndRoutedID(Convert.ToInt32(assignmentId), Convert.ToInt32(routeId));
                    if (dataTableroutes.Rows.Count > 0)
                    {
                            foreach (Order order in listOrder.test)
                            {

                                    //obtenemos la ruta y acumulamos toda las cantiadad de los pedidos
                                    foreach (PlannerDataSet.RoutesRow row in dataTableroutes.Rows)
                                    {
                                            row["TotalAmount"] = 0;
                                            if (order.ReceivedAmount == 0)
                                            {
                                                row["TotalAmount"] = Convert.ToInt32(row["TotalAmount"]) + order.RequestedAmount;
                                            }
                                            else
                                            {
                                                row["TotalAmount"] = Convert.ToInt32(row["TotalAmount"]) + order.ReceivedAmount;
                                            }
                                            //acumulamos toda la cantidad de los pedidos 
                                            sumaacumuladadeCntidadepedidos = sumaacumuladadeCntidadepedidos + Convert.ToInt32(row["TotalAmount"]);
                                            row["TotalAmount"] = sumaacumuladadeCntidadepedidos;
                                            rowfinalupdate = row;
                                    }
                            }
                            //actualizamos los datos de la ruta (comparamos los datos de la acumulación de los pedido para actualizar el mensaje con el peso) 
                            reponse = CompareAndUpdateDatasRoute(assignmentId, sumaacumuladadeCntidadepedidos, rowfinalupdate, routestable);

                    }
                    else
                    {
                        ok = false;
                    }
            }
            else
            {
                ok = false;
            }
      return ok;
     }



        public bool IsNullOrEmpty(object obj)
        {
            if (obj != null) // we can't be null
            {
                var method = obj.GetType().GetMethod("GetEnumerator");
                if (method != null) // we have an enumerator method
                {
                    var enumerator = method.Invoke(obj, null);
                    if (enumerator != null) // we got the enumerator
                    {
                        var moveMethod = enumerator.GetType().GetMethod("MoveNext");
                        if (moveMethod != null) // we have a movenext method, lets try it
                            return !(bool)moveMethod.Invoke(enumerator, null); // ie true = empty, false = has elements
                    }
                }
            }
            return true; // it's empty, lets return true
        }



        public bool CompareAndUpdateDatasRoute(long assigmentId , int sumOrdersAmount, PlannerDataSet.RoutesRow rowroute , PlannerDataSetTableAdapters.RoutesTableAdapter routestable)
        {
            int diferenciadepeso = 0;
            bool reponse = true;
            int numeroderegistros = 1;

            //obtenemos la asignación para obtener el id del trailer y su capacidad 
            PlannerDataSetTableAdapters.AssignmentsTableAdapter assigmentstable = new PlannerDataSetTableAdapters.AssignmentsTableAdapter();
            PlannerDataSet.AssignmentsDataTable AssigmentDatatable = assigmentstable.GetDataByAssigmentId(assigmentId);


            if (AssigmentDatatable.Rows.Count > 0)
            {
                //obtenemos el trailer de la asignacion
                PlannerDataSetTableAdapters.VehiclesTableAdapter vehiclestable = new PlannerDataSetTableAdapters.VehiclesTableAdapter();
                PlannerDataSet.VehiclesDataTable VehiclesDatatable = vehiclestable.GetDataVehiclesById(Convert.ToInt32(AssigmentDatatable[0]["TrailerId"].ToString()));

                //realizamos la conparación entre el volumen del tanque y el total de peso de todos los pedido
                //si se supera se mostrara el mensaje 
                if (Convert.ToInt32(VehiclesDatatable[0]["TankVolume"].ToString()) < sumOrdersAmount)
                {
                    diferenciadepeso = sumOrdersAmount - Convert.ToInt32(VehiclesDatatable[0]["TankVolume"].ToString());
                    rowroute["MessageOverAmount"] = "El sobrepeso de la asignación es de:  " + diferenciadepeso.ToString() + "kg";
                }
                //sino no se mostrara nada 
                else
                {
                    rowroute["MessageOverAmount"] = "";
                }
            }
            numeroderegistros = routestable.Update(rowroute);

            if (numeroderegistros == 0)
            {
                reponse = false;     
            }
            return reponse;

        }

        private void OnAssignmentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        #endregion

        #region ORDERS

        private string _filterOperators;
        private string _filterClients;
        private string _filterFactories;
        private string _filterProducts;

        public ObservableCollection<Order> Orders { get; private set; }
        public ObservableCollection<Operator> OrdersOperators { get; private set; }
        public ObservableCollection<Client> OrdersClients { get; private set; }
        public ObservableCollection<Factory> OrdersFactories { get; private set; }
        public ObservableCollection<Product> OrdersProducts { get; private set; }
       
        public string FilterOperators
        {
            get => _filterOperators;
            set
            {
                if (value != _filterOperators)
                {
                    Set(ref _filterOperators, value);
                    if (!BlockFilterListEvent)
                    {
                        UpdateOrders(SelectedDate);
                    }
                }
            }
        }
        public string FilterClients
        {
            get => _filterClients;
            set
            {
                if (value != _filterClients)
                {
                    Set(ref _filterClients, value);
                    if (!BlockFilterListEvent)
                    {
                        UpdateOrders(SelectedDate);
                    }
                }
            }
        }
        public string FilterFactories
        {
            get => _filterFactories;
            set
            {
                if (value != _filterFactories)
                {
                    Set(ref _filterFactories, value);
                    if (!BlockFilterListEvent)
                    {
                        UpdateOrders(SelectedDate);
                    }
                }
            }
        }

        public string FilterProducts
        {
            get => _filterProducts;
            set
            {
                if (value != _filterProducts)
                {
                    Set(ref _filterProducts, value);
                    if (!BlockFilterListEvent)
                    {
                        UpdateOrders(SelectedDate);
                    }
                }
            }
        }

        private bool IsFiltering { get; set; }

        private void InitOrders()
        {
            Orders = new ObservableCollection<Order>();

            OrdersOperators = new ObservableCollection<Operator>();
            OrdersClients = new ObservableCollection<Client>();
            OrdersFactories = new ObservableCollection<Factory>();
            OrdersProducts = new ObservableCollection<Product>();
       
            Orders.CollectionChanged += OnOrdersCollectionChanged;
        }

        private void UpdateOrders(DateTime dateTime)
        {
            Console.WriteLine("UpdateOrders " + dateTime.ToShortDateString());
            IsFiltering = !string.IsNullOrWhiteSpace(FilterOperators) || !string.IsNullOrWhiteSpace(FilterClients) || !string.IsNullOrWhiteSpace(FilterFactories) || !string.IsNullOrWhiteSpace(FilterProducts);

            BlockOrderUpdate = true;
            Orders.Clear();
            var reqOrders = PlannerRequester.RequestOrders(dateTime, FilterOperators, FilterClients, FilterFactories, FilterProducts);
            foreach (Order order in reqOrders)
            {
                SetColor(order);
                Orders.Add(order);
            }

            BlockOrderUpdate = false;
            OnOrdersCollectionChanged(null, null);
        }

        private void OnOrdersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("OnOrdersCollectionChanged");
            if (BlockOrderUpdate) { return; }
            if (!IsFiltering)
            {
                BlockFilterListEvent = true;
                OrdersOperators.Clear();
                OrdersClients.Clear();
                OrdersFactories.Clear();
                OrdersProducts.Clear();              

                for (int i = Orders.Count - 1; i >= 0; i--)
                {
                    var order = Orders[i];
                    if (!OrdersOperators.Any(item => item.Id == order.Operator.Id))
                    {
                        OrdersOperators.Add(order.Operator);
                    }

                    if (!OrdersClients.Any(item => item.Id == order.Client.Id))
                    {
                        OrdersClients.Add(order.Client);
                    }

                    if (!OrdersFactories.Any(item => item.Id == order.Factory.Id))
                    {
                        OrdersFactories.Add(order.Factory);
                    }

                    if (!OrdersProducts.Any(item => item.Id == order.Product.Id))
                    {
                        OrdersProducts.Add(order.Product);
                    }

                }

                BlockFilterListEvent = false;
            }
        }

        #endregion



        public void AddAssignOrder(ListView listViewDestiny, ListView listViewOrigin, long assignmentId, Order order, int RoutedId)
        {

            bool CanMoveOrders = true;
            //validamos que no este en la misma lista de assignaciones (lista de la izquierda)
            CanMoveOrders = CanMoveOrder(listViewDestiny, listViewOrigin);

            //si hemos lanzado el pedido en la misma lista al hacer el efecto no realizara ninguna acción si ya existe (asignaciones)
            if (CanMoveOrders)
            {
                //si no se ha creado una nueva ruta con el pedido, significa que tenemos que asignar el pedido a la asignación 
                //si se ha creado una nueva ruta con el pedido no entramos en el if y no realizamos la acción
                if (!CreatedNewRoute(listViewDestiny, listViewOrigin, order, assignmentId, RoutedId))
                {
                    int positionInsertTripList;
                    //añadimos la el pedido al listado de asignaciones 
                    ((ObservableCollection<Order>)listViewDestiny.ItemsSource).Add(order);

                    //borramos el pedido
                    Orders.Remove(order);

                    //colocamos el viaje en posición se mostrara 
                    positionInsertTripList = listViewDestiny.Items.Count;

                    //Asignamos una ruta a un viaje y actualizamos la posición del viaje dentro la lista de pedido asignado
                    AssignOrderTripARouteAssigments(order, assignmentId, positionInsertTripList, RoutedId);

                }
            }

        }

   


        public void RemoveAssignedOrder(ListView listViewDestiny, ListView listViewOrigin, long assignmentId, Order order, int routeId)
        {

            bool CanMoveOrders = true;
            bool CanMoveTripNoLoad = true;
            //validamos que no este en la misma lista de pedidos (lista de la derecha)
            CanMoveOrders = CanMoveOrder(listViewOrigin, listViewDestiny);
            CanMoveTripNoLoad = CanMoveListStatusTripLoad(Convert.ToInt32(order.TripId), listViewOrigin);


            //si hemos lanzado el pedido en la misma lista al hacer el efecto no realizara ninguna acción 
            if (CanMoveOrders && CanMoveTripNoLoad)
            {

                int positionInsertTripList;
                //añadimos a la lista de pedido 
                Orders.Add(order);

                //borrar el pedido de la lista de asignaciones 
                ((ObservableCollection<Order>)listViewOrigin.ItemsSource).Remove(order);

                positionInsertTripList = listViewOrigin.Items.Count;

                //al desasignar poenmos el tripI del pedido y eliminamos el viaje
                PutRouteIdNullToTrips(order, positionInsertTripList, assignmentId, routeId);
            }

        }


        public void RemoveAssignedOrderToMap(ListView listViewDestiny, ListView listViewOrigin, long assignmentId, Order order, int routedid)
        {
            int positionInsertTripList;
            bool CanMoveTripNoLoadOrOriginListIsNotNull = true;
            CanMoveTripNoLoadOrOriginListIsNotNull = CanMoveListStatusTripLoad(Convert.ToInt32(order.TripId), listViewOrigin);
            //si el estatus del viaje es 3 esta cargado y no se puede arrastrar
            if (CanMoveTripNoLoadOrOriginListIsNotNull)
            {
                //añadimos a la lista de pedido 
                Orders.Add(order);

                //borrar el pedido de la lista de asignaciones 
                ((ObservableCollection<Order>)listViewOrigin.ItemsSource).Remove(order);

                positionInsertTripList = listViewOrigin.Items.Count;

                //al desasignar poenmos el tripI del pedido y eliminamos el viaje
                PutRouteIdNullToTrips(order, positionInsertTripList, assignmentId, routedid);
            }
        
        }

        public void PutRouteIdNullToTrips(Order order, int positionInsertTripList, long assignmentId, int routeId)
        {
            //de izquierda a derecha 
            //del pedido ponemos todos los viajes relacionados a pedido que estamos sacando(ponemos el idroute = null) 
            try
            {
                int valorescomparados = 0;
                int cantidadtotaldelaasignacion = 0;
                int diferenciadepeso = 0;
                PlannerDataSet dataset = new PlannerDataSet();
                PlannerDataSetTableAdapters.TripsTableAdapter TripTable = new PlannerDataSetTableAdapters.TripsTableAdapter();
                //obtenemos la lista de viajes del pedido 
                PlannerDataSet.TripsDataTable dataTableOnlyOnetrips = TripTable.GetDataByIdTrips(Convert.ToInt32(order.TripId));
                //recorremos la tabla de viajes por pedidos 
                if (dataTableOnlyOnetrips.Rows.Count > 0)
                {
                    foreach (PlannerDataSet.TripsRow row in dataTableOnlyOnetrips.Rows)
                    {
                        row.SetRouteIdNull();
                        row.Status = 1;
                        int numeroderegistros = TripTable.Update(row);

                    }
                }
                PlannerDataSetTableAdapters.RoutesTableAdapter routestable = new PlannerDataSetTableAdapters.RoutesTableAdapter();
                PlannerDataSet.RoutesDataTable dataTableroutes = routestable.GetDatarRoutedIdByAssigmentandIdAndRoutedID(Convert.ToInt32(assignmentId), Convert.ToInt32(routeId));

                if (dataTableroutes.Rows.Count > 0)
                {
                    //recorremos la tabla de viajes por pedidos 
                    foreach (PlannerDataSet.RoutesRow row in dataTableroutes.Rows)
                    {

                        if (order.ReceivedAmount == 0)
                        {
                            row["TotalAmount"] = Convert.ToInt32(row["TotalAmount"]) - order.RequestedAmount;
                        }
                        else
                        {
                            row["TotalAmount"] = Convert.ToInt32(row["TotalAmount"]) - order.ReceivedAmount;
                        }

                        //guardamos la cantidad total de todos los pedidos
                        cantidadtotaldelaasignacion = (int)row["TotalAmount"];

                        //obtenemos la asignación para obtener el id del trailer y su capacidad 
                        PlannerDataSetTableAdapters.AssignmentsTableAdapter assigmentstable = new PlannerDataSetTableAdapters.AssignmentsTableAdapter();
                        PlannerDataSet.AssignmentsDataTable AssigmentDatatable = assigmentstable.GetDataByAssigmentId(assignmentId);


                        if (AssigmentDatatable.Rows.Count > 0)
                        {
                            //obtenemos el trailer de la asignacion
                            PlannerDataSetTableAdapters.VehiclesTableAdapter vehiclestable = new PlannerDataSetTableAdapters.VehiclesTableAdapter();
                            PlannerDataSet.VehiclesDataTable VehiclesDatatable = vehiclestable.GetDataVehiclesById(Convert.ToInt32(AssigmentDatatable[0]["TrailerId"].ToString()));

                            //realizamos la conparación entre el volumen del tanque y el total de peso de todos los pedido
                            //si se supera se mostrara el mensaje 
                            if (Convert.ToInt32(VehiclesDatatable[0]["TankVolume"].ToString()) < cantidadtotaldelaasignacion && positionInsertTripList != 0)
                            {
                                diferenciadepeso = cantidadtotaldelaasignacion - Convert.ToInt32(VehiclesDatatable[0]["TankVolume"].ToString());
                                row["MessageOverAmount"] = "El sobrepeso de la asignación es de:  " + diferenciadepeso.ToString() + "kg";
                            }
                            //sino no se mostrara nada 
                            else
                            {
                                row["MessageOverAmount"] = "";
                            }
                        }
                        int numeroderegistros = routestable.Update(row);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        //metodo para evitar poder mover un pedido de izquierda derecha por que el pedido esta cargado
        public bool CanMoveListStatusTripLoad(int tripId,ListView listViewOrigin)
        {
            bool CanMoverList = true;
            int status = 0;
            PlannerDataSet dataset = new PlannerDataSet();
            PlannerDataSetTableAdapters.TripsTableAdapter tripsTable = new PlannerDataSetTableAdapters.TripsTableAdapter();
            PlannerDataSet.TripsDataTable TripsDatatable = tripsTable.GetDataByIdTrips(tripId);

            status = Convert.ToInt32(TripsDatatable[0]["status"].ToString());

            //si el estado del viaje del pedido es 3 significa que esta cargado por el transportista y entonces no se puede mover
            if (status == 3 || listViewOrigin == null)
            {
                CanMoverList = false;
            }
            else
            {
                CanMoverList = true;
            }
            return CanMoverList;
        }



        //metodo para validar que hay duplicados en la misma lista
        public bool CanMoveOrder(ListView listViewOrigin, ListView listViewDestiny)
        {
            //venimos del mapa 
            if (listViewDestiny == null)
            {
                return true;
            }
            //entre listas 
            else
            {
                if (listViewOrigin.Name.ToString() == listViewDestiny.Name.ToString())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //metodo para validar que hay duplicados en la misma lista
        public bool CreatedNewRoute(ListView listViewDestiny, ListView listViewOrigin, Order order, long assignmentId, int RoutedId)
        {
            bool reponse = false;
            ObservableCollection<Order> listOrder = new ObservableCollection<Order>();
            int diferenciadepeso = 0;
            int numeroderegistros = 0;
            int cantidadtotaldelaasignacion = 0;
            //miramos si somos el primer pedido o no 
            //si es mas grande que zero entonces tenemos que mirar que la factoría coincida (no soy el primero) 
            //si no coincide se tendra que crear una segunda ruta con el pedido asignado 
            //si soy el primero no hay que hacer una ruta nueva y entonces devolvemos false 
            if (listViewDestiny.Items.Count > 0)
            {
                listOrder = ((ObservableCollection<Order>)listViewDestiny.ItemsSource);
                // Assigment = listViewDestiny.ItemsSource;
                //si son diferentes entonces creamos la nueva ruta 
                if (order.Factory.Id != listOrder[0].Factory.Id)
                {

                    try
                    {
                        //Assignment Assigment = new Assignment();
                        //Assigment = GetAssignment(Convert.ToInt32(listViewDestiny.Tag));

                        PlannerDataSet dataset = new PlannerDataSet();
                        PlannerDataSetTableAdapters.RoutesTableAdapter routesTable = new PlannerDataSetTableAdapters.RoutesTableAdapter();
                        PlannerDataSetTableAdapters.TripsTableAdapter TripsTable = new PlannerDataSetTableAdapters.TripsTableAdapter();

                        //Creamos el registro de la route
                        var rowRoutes = dataset.Routes.NewRoutesRow();
                        //assignamos la asignación recient creada 
                        rowRoutes.AssignmentId = Convert.ToInt32(listViewDestiny.Tag.ToString());
                        //fecha de creación 
                        rowRoutes.CreatedDate = DateTime.Now;
                        //fecha de modificación
                        rowRoutes.ModifiedDate = DateTime.Now;
                        //fecha de modificación

                        if (order.ReceivedAmount == 0)
                        {
                            rowRoutes.TotalAmount = order.RequestedAmount;
                        }
                        else
                        {
                            rowRoutes.TotalAmount = order.ReceivedAmount.GetValueOrDefault();
                        }

                        //guardamos la cantidad total de todos los pedidos
                        cantidadtotaldelaasignacion = rowRoutes.TotalAmount;
         
                        //obtenemos la asignación para obtener el id del trailer y su capacidad 
                        PlannerDataSetTableAdapters.AssignmentsTableAdapter assigmentstable = new PlannerDataSetTableAdapters.AssignmentsTableAdapter();
                        PlannerDataSet.AssignmentsDataTable AssigmentDatatable = assigmentstable.GetDataByAssigmentId(assignmentId);

                        if (AssigmentDatatable.Rows.Count > 0)
                        {
                            //obtenemos el trailer de la asignacion
                            PlannerDataSetTableAdapters.VehiclesTableAdapter vehiclestable = new PlannerDataSetTableAdapters.VehiclesTableAdapter();
                            PlannerDataSet.VehiclesDataTable VehiclesDatatable = vehiclestable.GetDataVehiclesById(Convert.ToInt32(AssigmentDatatable[0]["TrailerId"].ToString()));

                            //realizamos la conparación entre el volumen del tanque y el total de peso de todos los pedido
                            //si se supera se mostrara el mensaje 
                            if (Convert.ToInt32(VehiclesDatatable[0]["TankVolume"].ToString()) < cantidadtotaldelaasignacion)
                            {
                                diferenciadepeso = cantidadtotaldelaasignacion - Convert.ToInt32(VehiclesDatatable[0]["TankVolume"].ToString());
                                rowRoutes.MessageOverAmount = "El sobrepeso de la asignación es de:  " + diferenciadepeso.ToString() + "kg";
                            }
                            //sino no se mostrara nada 
                            else
                            {
                                rowRoutes.MessageOverAmount = "";
                            }
                            //agregamos la nueva fila a la tabla rutas 
                            dataset.Routes.AddRoutesRow(rowRoutes);
                            //actualizamos la tabla rutas  en el dataset 
                            numeroderegistros = Convert.ToInt32(routesTable.Update(dataset.Routes));
                      
                            //si es mas grande se ha creado todo correctamente
                            if (numeroderegistros > 0)
                            {
                                //actualizamos el viaje del pedido , con la ruta nueva creada 
                                PlannerDataSet.TripsDataTable trips = TripsTable.GetDataByIdTrips(Convert.ToInt32(order.TripId));
                                if (trips.Rows.Count > 0)
                                {
                                    var rowTrip = dataset.Trips.NewTripsRow();
                                    rowTrip = trips[0];
                                    rowTrip["RouteId"] = rowRoutes.Id;
                                    TripsTable.Update(rowTrip);
                                }                     
                                reponse = true;
                            }
                            else
                            {
                                reponse = false;
                            }
                        }
                        else
                        {
                            reponse = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    reponse = false;
                }
            }
            else
            {
                reponse = false;
            }
            return reponse;
        }       
        //tenemos un pedido con su viaje   y una ruta con su asignación   (para unir los cuatro asignamos el identificador de la ruta al viaje)
        public void AssignOrderTripARouteAssigments(Order order,long assignmentId,int positionInsertTripList,int RoutedId)
        {
            int contador = 0;
            int cantidadtotaldelaasignacion = 0;
            int diferenciadepeso = 0;
            // de derecha  a iznquiera 
            try
            {

                //obtenemos el registro de viajes 
                PlannerDataSet dataset = new PlannerDataSet();
                PlannerDataSetTableAdapters.TripsTableAdapter tripstable = new PlannerDataSetTableAdapters.TripsTableAdapter();

                //o actualizamos todos los viajes 
                //PlannerDataSet.TripsDataTable dataTableAlltrips = tripstable.GetDataTripsByOrderId(Convert.ToInt32(order.Id));
                //o solo el que estamos tocando 
                PlannerDataSet.TripsDataTable dataTableOnlyOnetrips = tripstable.GetDataByIdTrips(Convert.ToInt32(order.TripId));
      

                if (dataTableOnlyOnetrips.Rows.Count > 0)
                {
                    //recorremos la tabla de viajes por pedidos 
                    foreach (PlannerDataSet.TripsRow row in dataTableOnlyOnetrips.Rows)
                    {

                        row["RouteId"] = RoutedId;
                        row.Status = 2;
                        row.Position = 0;
                        int numeroderegistros = tripstable.Update(row);

                    }
                }

                //actualizamos la cantidad total que tendra la ruta 
                PlannerDataSetTableAdapters.RoutesTableAdapter routestable = new PlannerDataSetTableAdapters.RoutesTableAdapter();
                PlannerDataSet.RoutesDataTable Routesassigments = routestable.GetDatarRoutedIdByAssigmentandIdAndRoutedID(Convert.ToInt32(assignmentId), Convert.ToInt32(RoutedId));

                if (Routesassigments.Rows.Count > 0)
                {
                    //recorremos la tabla de viajes por pedidos 
                    foreach (PlannerDataSet.RoutesRow row in Routesassigments.Rows)
                    {
                        //si la cantidad recibida es 0 entonces sumamos la esperada 
                        if (order.ReceivedAmount == 0)
                        {
                            row["TotalAmount"] = Convert.ToInt32(row["TotalAmount"]) + order.RequestedAmount;
                        }
                        //sino sumamos la recibida a la total
                        else
                        {
                            row["TotalAmount"] = Convert.ToInt32(row["TotalAmount"]) + order.ReceivedAmount;
                        }

                        //guardamos la cantidad total de todos los pedidos
                        cantidadtotaldelaasignacion = (int)row["TotalAmount"];

                        //obtenemos la asignación para obtener el id del trailer y su capacidad 
                        PlannerDataSetTableAdapters.AssignmentsTableAdapter assigmentstable = new PlannerDataSetTableAdapters.AssignmentsTableAdapter();
                        PlannerDataSet.AssignmentsDataTable AssigmentDatatable = assigmentstable.GetDataByAssigmentId(assignmentId);


                        if (AssigmentDatatable.Rows.Count > 0)
                        {
                            //obtenemos el trailer de la asignacion
                            PlannerDataSetTableAdapters.VehiclesTableAdapter vehiclestable = new PlannerDataSetTableAdapters.VehiclesTableAdapter();
                            PlannerDataSet.VehiclesDataTable VehiclesDatatable = vehiclestable.GetDataVehiclesById(Convert.ToInt32(AssigmentDatatable[0]["TrailerId"].ToString()));

                            //realizamos la conparación entre el volumen del tanque y el total de peso de todos los pedido
                            //si se supera se mostrara el mensaje 
                            if(Convert.ToInt32(VehiclesDatatable[0]["TankVolume"].ToString()) < cantidadtotaldelaasignacion)
                            {
                                diferenciadepeso = cantidadtotaldelaasignacion - Convert.ToInt32(VehiclesDatatable[0]["TankVolume"].ToString());
                                row["MessageOverAmount"] = "El sobrepeso de la asignación es de:  " + diferenciadepeso.ToString() + "kg";
                            }
                            //sino no se mostrara nada 
                            else
                            {
                                row["MessageOverAmount"] = "";
                            }
                        }                    
                        int numeroderegistros = routestable.Update(row);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public Assignment GetAssignment(int AssigmentId)
        {

            Assignment Assigment = new Assignment();

            Driver Driver = new Driver();
            Cab Cab = new Cab();
            Trailer Trailer = new Trailer();
            //obtenemos el registro de viajes 
            PlannerDataSet dataset = new PlannerDataSet();
            PlannerDataSetTableAdapters.AssignmentsTableAdapter Assigmentstable = new PlannerDataSetTableAdapters.AssignmentsTableAdapter();
            PlannerDataSet.AssignmentsDataTable dataTable = Assigmentstable.GetDataByAssigmentId(AssigmentId);

            Driver.Id = Convert.ToInt32(dataTable[0]["DriverId"].ToString());
            Cab.Id = Convert.ToInt32(dataTable[0]["CabId"].ToString());
            Trailer.Id= Convert.ToInt32(dataTable[0]["TrailerId"].ToString());

            Assigment.Driver = Driver;
            Assigment.Cab = Cab;
            Assigment.Trailer = Trailer;
            Assigment.Date = Convert.ToDateTime(dataTable[0]["Date"].ToString());
            Assigment.Id = Convert.ToInt32(dataTable[0]["Id"].ToString());

            return Assigment;

        }

        public void RorderAssigments(ListView listViewLeft)
        {
            PlannerDataSetTableAdapters.TripsTableAdapter TripTable = new PlannerDataSetTableAdapters.TripsTableAdapter();
            //obtenemos la lista de viajes del pedido 
            int contadorDeposiciones = 1;

            foreach (Order order in listViewLeft.ItemsSource)
            {
                PlannerDataSet.TripsDataTable dataTable = TripTable.GetDataTripsByOrderId(Convert.ToInt32(order.Id));
                //recorremos la tabla de viajes por pedidos 
                foreach (PlannerDataSet.TripsRow row in dataTable.Rows)
                {
                    row.Position = contadorDeposiciones;
                    TripTable.Update(row);
                    contadorDeposiciones = contadorDeposiciones + 1;
                }
            }

        }
    }
}
