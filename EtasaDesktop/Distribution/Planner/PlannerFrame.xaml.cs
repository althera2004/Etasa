namespace EtasaDesktop.Distribution.Planner
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Threading;
    using EtasaDesktop.Common.Data;
    using EtasaDesktop.Common.Tools;
    using EtasaDesktop.Distribution.Data;
    using EtasaDesktop.Distribution.Orders.Form;
    using EtasaDesktop.Distribution.Planner.Drag;
    using EtasaDesktop.Resources.DrapAndDrop;

    public partial class PlannerFrame : FrameControl
    {
        private PlannerViewModel _viewModel;
        private DragViewOrigin _dragViewOrigin;
        private ListView _listViewGlobal;
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public PlannerFrame()
        {

            _viewModel = new PlannerViewModel();
            DataContext = _viewModel;


            InitializeComponent();

            Map.Orders = _viewModel.Orders;
            List.ItemsSource = Map.Orders;

            Map.Assignments = new ObservableCollection<Assignment>(from assign in _viewModel.AssignmentRoutes select assign.Assignment);
            ListAssignements.ItemsSource = _viewModel.AssignmentRoutes;
        

            _viewModel.Orders.CollectionChanged += OnOrdersCollectionChanged;
            _viewModel.AssignmentRoutes.CollectionChanged += OnAssignmentsCollectionChanged;

            // declaramos un timer para actualizar la interficie grafica
            DispatcherTimer dt = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(60)
            };

            dt.Tick += dtTicker;
            dt.Start();
        }

        private void dtTicker(object sender,EventArgs e)
        {
            // Cada 60 segundos se actualiza la interfaz gráfica
            //Refresh();
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando pedidos...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";


            int nTotal = this.ListAssignements.Items.Count;

            for (int nconta = 0; nconta < nTotal; nconta++)
                Console.WriteLine(this.ListAssignements.Items[nconta].ToString());
        }


        private void OnOrdersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("OnOrdersCollectionChanged (PF) --> " + (PlannerViewModel.BlockOrderUpdate ? "Block" : "No block"));
            //if (PlannerViewModel.BlockOrderUpdate) { return; }
            Map.Orders = _viewModel.Orders;
            Map.Center();
        }

        private void OnAssignmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (PlannerViewModel.BlockMapUpdate) { return; }
            Console.WriteLine("OnAssignmentsCollectionChanged " + (PlannerViewModel.BlockMapUpdate ? "Block" : "No block"));
            Map.Assignments = new ObservableCollection<Assignment>(from assign in _viewModel.AssignmentRoutes select assign.Assignment);
            Map.Center();
        }

        private void ListViewAssignmentOrders_Loaded(object sender, RoutedEventArgs e)
        {
            //Lista de pedidos asignados a rutas
            var d0 = DateTime.Now;
            ListView listView = sender as ListView;
            new ListViewDragDropManager<Order>(listView).ReorderedCustomAction += PlannerFrame_ReorderedAction;
            var d1 = DateTime.Now;
            Console.WriteLine("ListViewAssignmentOrders_Loaded " + (d1 - d0).TotalMilliseconds.ToString());

        }

        private void PlannerFrame_ReorderedAction(object sender, DragEventArgs e)
        {
            RorderAssigments(sender as ListView);
        }

        private void ListViewOrders_Loaded(object sender, RoutedEventArgs e)
        {
            var d0 = DateTime.Now;
            ListView listView = sender as ListView;
            new ListViewDragDropManager<Order>(listView);
            var d1 = DateTime.Now;
            Console.WriteLine("ListViewOrders_Loaded " + (d1 - d0).TotalMilliseconds.ToString());
        }

        private void MapView_Checked(object sender, RoutedEventArgs e)
        {
            List.Visibility = Visibility.Hidden;
            Map.Visibility = Visibility.Visible;
        }

        private void ListView_Checked(object sender, RoutedEventArgs e)
        {
            Map.Visibility = Visibility.Hidden;
            List.Visibility = Visibility.Visible;
        }

        private void OrderEdit_Click(object sender, RoutedEventArgs e)
        {
            Order order = ((FrameworkElement)sender).DataContext as Order;

            OrderFormWindow orderWindow = new OrderFormWindow(order.Id);
            orderWindow.ShowDialog();

            if (orderWindow.DialogResult.HasValue && orderWindow.DialogResult.Value)
            {
                order = orderWindow.Order;
                PlannerRequester.UpdateOrder(order);
            }

            Refresh();
        }

        //lista de pedidos 
        #region DRAG & DROP

        #region Drag/Drop List

        private void OnDragEnterToList(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            if (_dragViewOrigin is null)
            {
                _dragViewOrigin = new DragViewOrigin(sender as ListView, DragViewOrigin.LIST_TYPE);
            }

        
        }
        private void OnDragLeaveFromList(object sender, DragEventArgs e)
        {

        }

        //cuando dejamos en la lista te borra la assignación 
        private void OnDropToList(object sender, DragEventArgs e)
        {
            int routedId = 0;
            //falla al asignar
            if (e.Effects == DragDropEffects.None)
                return;

            Order order = e.Data.GetData(typeof(Order)) as Order;
           
            ListView _listView = (ListView)sender;
            if (string.IsNullOrEmpty(_dragViewOrigin.ListView.Uid))
            {
                routedId = 0;
            }
            else
            {
               routedId = Convert.ToInt32(_dragViewOrigin.ListView.Uid.ToString());
            }
          
            //_listView.Background = new SolidColorBrush(Colors.Red);

            if (order != null) // && _dragViewOrigin.AssignId != null)
            {
                _viewModel.RemoveAssignedOrder(_listView,_dragViewOrigin.ListView, _dragViewOrigin.AssignId ?? 0, order, routedId);
                _viewModel.Refresh();
            }

            _dragViewOrigin = null;
        }

        //desde el mapa 
        #endregion

        #region Drag/Drop Map

        private void OnDragEnterToMap(object sender, DragEventArgs e)
        {
            Console.WriteLine("Entrar en el mapa");
            e.Effects = DragDropEffects.Move;
            if (_dragViewOrigin is null)
            {
                _dragViewOrigin = new DragViewOrigin(sender as ListView, DragViewOrigin.MAP_TYPE);
            }
        }
        private void OnDragLeaveFromMap(object sender, DragEventArgs e)
        {
            
        }

        //dejamos en el mapa borramos la asignación y le agregamos la orden
        private void OnDropToMap(object sender, DragEventArgs e)
        {
            Console.WriteLine("Soltar en el mapa");
            int routedId = 0;
            if (e.Effects == DragDropEffects.None)
                return;

            var order = e.Data.GetData(typeof(Order)) as Order;

            if (_dragViewOrigin.ListView == null)
            {
                routedId = 0;
            }
            else
            {
                routedId = Convert.ToInt32(_dragViewOrigin.ListView.Uid.ToString());
            }
            //ListView _listView = (ListView)sender;
            if (order != null)
            {
                _viewModel.RemoveAssignedOrderToMap(null,_dragViewOrigin.ListView, _dragViewOrigin.AssignId ?? 0, order, routedId);
                _viewModel.Refresh();
            }

            _dragViewOrigin = null;
        }
        #endregion
        //la lista de asignaciones 
        #region Drag/Drop Assignments

        private void OnDragEnterToAssignment(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;

            if (_dragViewOrigin is null)
                _dragViewOrigin = new DragViewOrigin(sender as ListView, DragViewOrigin.ASSIGN_TYPE);
        }
        private void OnDragLeaveFromAssignment(object sender, DragEventArgs e)
        {
            //Order order = e.Data.GetData(typeof(Order)) as Order;
            //if (order!=null)
            //{
            //    Console.WriteLine("Hay dato order");
            //}
        }
        private void OnDropToAssignment(object sender, DragEventArgs e)
        {
            //Funciona
            try
            {
                DateTime Fechainicio = new DateTime();
                Fechainicio = DateTime.Now;



           
                if (e.Effects == DragDropEffects.None)
                    return;

                ListView _listView = (ListView)sender;
                Order order = e.Data.GetData(typeof(Order)) as Order;


    





                //e.Data.

                if (order != null)
                {

                    /*
                    if (_dragViewOrigin.AssignId != null)
                    {
                        _viewModel.RemoveAssignedOrder(_listView,_dragViewOrigin.AssignId ?? 0, order);
                    }
                    */
                    
                    int assignId = Convert.ToInt32(_listView.Tag);
                    int routedId = Convert.ToInt32(_listView.Uid.ToString());



                    _viewModel.AddAssignOrder(_listView, _dragViewOrigin.ListView, assignId, order, routedId);

                    // Innecesario puesto que la llamada se hace desde la clase custom ListViewDragDropManager
                    //RorderAssigments(_listView);
                    _listViewGlobal = _listView;
                }            
                _dragViewOrigin = null;
                //this.Refresh();

                _viewModel.Refresh();
            }
            catch(Exception ex)
            {
                string a = ex.ToString();
            }
        }

        public string diferenceHoursMinutesSecondMilisecons(DateTime FechaInicio , DateTime FechaFin )
        {
            string mensaje = "";
            var diffInSeconds = (FechaFin - FechaInicio).TotalSeconds;
            var diffInHours = (FechaFin - FechaInicio).TotalHours;
            var diffInMilisecons = (FechaFin - FechaInicio).TotalMilliseconds;
            var diffInMinutos = (FechaFin - FechaInicio).TotalMinutes;

            mensaje = "fecha inicio : " + FechaInicio + " fecha final : " + FechaFin + " la diferencia en horas es: " + diffInHours.ToString() + " en Minutos : " + diffInMinutos.ToString() + " En segundos: " + diffInSeconds.ToString() + " en milisegundo : " + diffInMilisecons.ToString();

            return mensaje;
        }




        //que llama el evento 
        private void ReorderAssigment(object sender, MouseEventArgs e)
        {

            RorderAssigments(_listViewGlobal);

        }

        public void RorderAssigments(ListView listViewLeft)
        {
            PlannerDataSetTableAdapters.TripsTableAdapter TripTable = new PlannerDataSetTableAdapters.TripsTableAdapter();
            //obtenemos la lista de viajes del pedido 
            int contadorDeposiciones = 1;


            foreach (Order order in listViewLeft.ItemsSource)
            {
                int posicion = listViewLeft.Items.IndexOf(order);

                Console.WriteLine(order.Client.Name + " POS:" + posicion.ToString());

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

        void ListViewColumnHeaderClick(object sender,RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;


                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header  
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(List.ItemsSource);
            string orderColumn = sortBy;
            var items = List.ItemsSource as IEnumerable<Order>;


            switch (orderColumn)
            {
                case "Fecha":
                    //ordenamos la lista decendientemente
                    if (direction.ToString() == "Descending")
                    {
                        List.ItemsSource = items.OrderByDescending(a => a.StartDate);
                    }
                    //ascendiente
                    else
                    {
                        List.ItemsSource = items.OrderBy(a => a.StartDate);
                    }
                    break;
                case "Factoria":
                    //ordenamos la lista decendientemente
                    if (direction.ToString() == "Descending")
                    {
                        List.ItemsSource = items.OrderByDescending(a => a.Factory.Name);
                    }
                    //ascendiente
                    else
                    {
                        List.ItemsSource = items.OrderBy(a => a.Factory.Name);
                    }
                    break;
                case "Pedido":
                    //ordenamos la lista decendientemente
                    if (direction.ToString() == "Descending")
                    {
                        List.ItemsSource = items.OrderByDescending(a => a.Reference);
                    }
                    //ascendiente
                    else
                    {
                        List.ItemsSource = items.OrderBy(a => a.Reference);
                    }
                    break;
                case "Vehiculo":
                    //ordenamos la lista decendientemente
                    if (direction.ToString() == "Descending")
                    {
                        List.ItemsSource = items.OrderByDescending(a => a.VehicleType);
                    }
                    //ascendiente
                    else
                    {
                        List.ItemsSource = items.OrderBy(a => a.VehicleType);
                    }
                    break;
                case "Cantidad":
                    //ordenamos la lista decendientemente
                    if (direction.ToString() == "Descending")
                    {
                        List.ItemsSource = items.OrderByDescending(a => a.ReceivedAmount);
                    }
                    //ascendiente
                    else
                    {
                        List.ItemsSource = items.OrderBy(a => a.ReceivedAmount);
                    }
                    break;
                case "Cliente":
                    //ordenamos la lista decendientemente
                    if (direction.ToString() == "Descending")
                    {
                        List.ItemsSource = items.OrderByDescending(a => a.Client.Name);
                    }
                    //ascendiente
                    else
                    {
                        List.ItemsSource = items.OrderBy(a => a.Client.Name);
                    }
                    break;
                default:
                    //ordenamos la lista decendientemente
                    if (direction.ToString() == "Descending")
                    {
                        List.ItemsSource = items.OrderByDescending(a => a.Client.Name);
                    }
                    //ascendiente
                    else
                    {
                        List.ItemsSource = items.OrderBy(a => a.Client.Name);
                    }
                    break;
            }        
        }


        #endregion

        #endregion
    }
}

