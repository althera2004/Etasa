using EtasaDesktop.Common.Tools;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;

namespace EtasaDesktop.Distribution.Assignments
{

    public partial class AssignmentsFormWindow : Window
    {
        private AssignmentsFormViewModel _viewModel;
        private int TrailerId;
        private int CabId;
        private int DriverId;   
        private long AssigmentId;
        AssignmentsDataSet.AssignmentsRow _row;
        public System.Windows.Forms.DialogResult Result { get; set; }
        public AssignmentsFormWindow(string FechaForm, long assignId = 0) 
        {
            _viewModel = new AssignmentsFormViewModel();
            DataContext = _viewModel;
            InitializeComponent();

            _viewModel.FormLoadError += FormLoadError_Event;
            _viewModel.FormSaveFinished += FormSaveFinished_Event;
            _viewModel.FormSaveError += FormSaveError_Event;
            _viewModel.FormDeleteError += FormDeleteError_Event;
            _viewModel.FormDeleteFinished += FormDeleteFinished_Event;
            _viewModel.FormRequiredEmpty += FormRequiredEmpty_Event;

            if (assignId > 0)
            {
                AssigmentId = assignId;
                Title.Content = "Editar asignación";
               // _viewModel.Load(assignId);
                LoadDatas(assignId);          
                txtDate.IsEnabled = false;
            }
            else
            {
                Title.Content = "Nueva asignación";
                ButtonEliminar.IsEnabled = false;
                txtDate.Text = FechaForm;
            }
           
        }


        public void LoadDatas(long id)
        {
            try
            {

                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.AssignmentsTableAdapter adapter = new AssignmentsDataSetTableAdapters.AssignmentsTableAdapter();
                AssignmentsDataSet.AssignmentsDataTable dataTable = adapter.GetDataById(id);

                AssignmentsDataSetTableAdapters.Assignments_ObsTableAdapter adapter2 = new AssignmentsDataSetTableAdapters.Assignments_ObsTableAdapter();
                AssignmentsDataSet.Assignments_ObsDataTable dataTableObs = adapter2.GetDataAssignmentsObsById(id);

                if (dataTable.Rows.Count > 0)
                {

                    _row = (AssignmentsDataSet.AssignmentsRow)dataTable.Rows[0];
                 
                    //fecha 
                    txtDate.Text = _row.Date.ToString();

                    //observaciones 
                    if (dataTableObs.Count > 0)
                    {
                        txtObservation.Text = dataTableObs[0].Observations.ToString();
                    }
                    else
                    {
                        txtObservation.Text = "";
                    }
                                    
                    //cargamos datos conductor
                    txtCodeDriver.Text = ReturnDriverCode(_row.DriverId);
                    txtNameDriver.Text = ReturnDriverInfo(_row.DriverId);

                    //cargamos datos cabina tractora
                    txtlicense.Text = ReturnCabLicensePlate(_row.CabId);
                    tractoraCode.Text = ReturnCabCode(_row.CabId);

                    //cargamos datos trailer
                    txttrailerCode.Text = ReturnTrailerCode(_row.TrailerId);
                    txtRemolquelicense.Text = ReturnTrailerLicense(_row.TrailerId);
                    txtTrailerVolume.Text = ReturnTrailerVolume(_row.TrailerId);

                }
            }
            catch (Exception e)
            {
         
            }
        }

        //información del conducto
        public string ReturnDriverInfo(int idDriver)
        {
            string infodriver = "";
            if (idDriver > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.DriversInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.DriversInfoTableAdapter();
                AssignmentsDataSet.DriversInfoDataTable dataTable = adapter.GetDataBy(idDriver);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.DriversInfoRow row = (AssignmentsDataSet.DriversInfoRow)dataTable.Rows[0];
                    infodriver = row.Name;
                }
            }
            return infodriver;
        }

        //codigo conductor
        private string ReturnDriverCode(int idDriver)
        {
            string Code = "";

            if (idDriver > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.DriversInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.DriversInfoTableAdapter();
                AssignmentsDataSet.DriversInfoDataTable dataTable = adapter.GetDataBy(idDriver);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.DriversInfoRow row = (AssignmentsDataSet.DriversInfoRow)dataTable.Rows[0];  
                    Code = row.Code;
                }
            }
            return Code;
        }

        //información de cabina 
        private string ReturnCabLicensePlate(int Cabid)
        {
            string LicensePlate = "";
            if (Cabid > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.CabInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.CabInfoTableAdapter();
                AssignmentsDataSet.CabInfoDataTable dataTable = adapter.GetDataByIdCab(Cabid);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.CabInfoRow row = (AssignmentsDataSet.CabInfoRow)dataTable.Rows[0];
                    LicensePlate = row.LicensePlate;
                }
            }

            return LicensePlate;
        }

        //información de código de cabina 
        private string ReturnCabCode(int Cabid)
        {
            string CodeCab = "";
            if (Cabid > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.CabInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.CabInfoTableAdapter();
                AssignmentsDataSet.CabInfoDataTable dataTable = adapter.GetDataByIdCab(Cabid);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.CabInfoRow row = (AssignmentsDataSet.CabInfoRow)dataTable.Rows[0];

                    CodeCab = row.Code;
  
                }
            }
            return CodeCab;
        }

        //devuelve código del trailer
        private string ReturnTrailerCode(int idtrailer)
        {
            string trailerCode = "";
            if (idtrailer > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter();
                AssignmentsDataSet.TrailerInfoDataTable dataTable = adapter.GetDataBy1IdTrailer(idtrailer);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.TrailerInfoRow row = (AssignmentsDataSet.TrailerInfoRow)dataTable.Rows[0];

                    trailerCode = row.Code;  
                }
            }
            return trailerCode;
        }


        //devuelve la licensia del trailer
        private string ReturnTrailerLicense(int idtrailer)
        {
            string LicenseTrailer = "";

            if (idtrailer > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter();
                AssignmentsDataSet.TrailerInfoDataTable dataTable = adapter.GetDataBy1IdTrailer(idtrailer);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.TrailerInfoRow row = (AssignmentsDataSet.TrailerInfoRow)dataTable.Rows[0];
                    LicenseTrailer = row.LicensePlate;       
                }
            }

            return LicenseTrailer;
        }

        //devuelve el volumen del trailes
        private string ReturnTrailerVolume(int idtrailer)
        {
            string VolumeTrailer = "";
            if (idtrailer > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter();
                AssignmentsDataSet.TrailerInfoDataTable dataTable = adapter.GetDataBy1IdTrailer(idtrailer);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.TrailerInfoRow row = (AssignmentsDataSet.TrailerInfoRow)dataTable.Rows[0];
                    VolumeTrailer = row.TankVolume.ToString();
                }
            }
            return VolumeTrailer;
        }

        private void FormLoadError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido cargar la asignación", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        private void FormRequiredEmpty_Event()
        {
            MessageBoxResult result = MessageBox.Show("Hay campos obligatorios sin rellenar",
                          "Confirmation",
                          MessageBoxButton.OK,
                          MessageBoxImage.Warning);
        }
        private void FormSaveFinished_Event()
        {
            DialogResult = true;
            Close();
        }
        private void FormSaveError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido guardar la asignación", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void FormDeleteFinished_Event()
        {
            DialogResult = true;
            Close();
        }

        private void FormDeleteError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido eliminar la asignación", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SearchDriverButton_Click(object sender, RoutedEventArgs e)
        {
            bool? returnvalue = true;
            SearchElementWindow searchDriverWindow = new SearchElementWindow(new SearchDriverViewModel());
            searchDriverWindow.conductorClick = true;
            returnvalue = searchDriverWindow.ShowDialog();


            if (returnvalue == true)
            {
                //mostramos los datos 
                txtCodeDriver.Text = searchDriverWindow.codigoConductor.Trim();
                txtNameDriver.Text = searchDriverWindow.nombreConductor.Trim();
                DriverId = searchDriverWindow.IdConductor;
                //desahabilitamos el mostrar datos 
                searchDriverWindow.conductorClick = false;
            }
         
        }

        private void SearchCabButton_Click(object sender, RoutedEventArgs e)
        {
            bool? returnvalue = true;
            SearchElementWindow searchDriverWindow = new SearchElementWindow(new SearchCabViewModel());
            searchDriverWindow.tractoraClick = true;
            returnvalue = searchDriverWindow.ShowDialog();

            if (returnvalue == true)
            {
                //mostramos los datos 
                txtlicense.Text = searchDriverWindow.licenciatractora.Trim();
                tractoraCode.Text = searchDriverWindow.codigoTractora.Trim();
                CabId = searchDriverWindow.IdTractora;
                //desahabilitamos el mostrar datos 
                searchDriverWindow.tractoraClick = false;
            }
        }

        private void SearchTrailerButton_Click(object sender, RoutedEventArgs e)
        {
            bool? returnvalue = true;
            SearchElementWindow searchDriverWindow = new SearchElementWindow(new SearchTrailerViewModel());
            searchDriverWindow.trailerClick = true;
            returnvalue = searchDriverWindow.ShowDialog();
            if (returnvalue == true)
            {
                //mostramos los datos 
                txttrailerCode.Text = searchDriverWindow.codigoTrailer.Trim();
                txtRemolquelicense.Text = searchDriverWindow.MatriculaTrailer.Trim();
                txtTrailerVolume.Text = searchDriverWindow.CapacidadTrailer.Trim();
                TrailerId = searchDriverWindow.IdTrailer;
                //desahabilitamos el mostrar datos 
                searchDriverWindow.trailerClick = false;
            }
        }

        private void AddAssigment_click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool validate = true;
                //validamos que los datos no esten vacios (conductor,tractora, trailer)
                validate = validateDatas(txtCodeDriver.Text, tractoraCode.Text, txttrailerCode.Text);
                if (validate)
                {
                    DSAssigmentsData dataset = new DSAssigmentsData();
                    DSAssigmentsDataTableAdapters.AssignmentsTableAdapter assignmentsTable = new DSAssigmentsDataTableAdapters.AssignmentsTableAdapter();
                    DSAssigmentsData.AssignmentsDataTable DataTableAssignments = assignmentsTable.GetDataByIdAssignment(AssigmentId);

                    //actualizamnos si existe el registro de la assignación
                    if (DataTableAssignments.Rows.Count > 0)
                    {
                        //Obtenemos el registro de las observaciones ha actualizar
                        DSAssigmentsDataTableAdapters.Assignments_ObsTableAdapter assignmentsTableObs = new DSAssigmentsDataTableAdapters.Assignments_ObsTableAdapter();

                        var rowAssignments = dataset.Assignments.NewAssignmentsRow();
                        rowAssignments = DataTableAssignments[0];
                        //asignación de Conductor
                        rowAssignments["DriverId"] = GeTIdDriverByCode(txtCodeDriver.Text.ToString());
                        //asignación de cabina 
                        rowAssignments["CabId"] = GeTIdCabByCode(tractoraCode.Text.ToString());
                        //asignación de trailer
                        rowAssignments["TrailerId"] = GeTIdTrailerByCode(txttrailerCode.Text.ToString());
                        //fecha 
                        rowAssignments["Date"] = Convert.ToDateTime(txtDate.Text.ToString());
                        //fecha de creación
                        rowAssignments["CreatedDate"] = DateTime.Now;
                        //fecha de modificación
                        rowAssignments["ModifiedDate"] = DateTime.Now;
                        //repeticion
                        rowAssignments["Repeat"] = false;
                        //habilitar
                        rowAssignments["Enabled"] = true;

                        int numeroderegistros = assignmentsTable.Update(rowAssignments);

                        //si se actualiza la asignación actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            assignmentsTableObs.UpdateQueryById(txtObservation.Text, AssigmentId);                     
                        }                     
                    }
                    //si es igual a zero estamos creando uno de nuevo
                    else
                    {
                        //obtenemos de la tabla trips  el nuevo registro que se introducira 
                        DSAssigmentsData.AssignmentsRow rowAssigments = dataset.Assignments.NewAssignmentsRow();
                        //asignación de Conductor
                        string axel = txtCodeDriver.Text.ToString();
                        rowAssigments.DriverId = GeTIdDriverByCode(txtCodeDriver.Text.ToString());
                        //asignación de cabina 
                        rowAssigments.CabId = GeTIdCabByCode(tractoraCode.Text.ToString());
                        //asignación de trailer
                        rowAssigments.TrailerId = GeTIdTrailerByCode(txttrailerCode.Text.ToString());
                        //fecha de creación
                        rowAssigments.CreatedDate = DateTime.Now;
                        //fecha de modificación
                        rowAssigments.ModifiedDate = DateTime.Now;
                        //observaciones 
                        //rowAssigments.Observations = txtObservation.Text;
                        //repeat 
                        rowAssigments.Repeat = false;
                        //Enabled 
                        rowAssigments.Enabled = true;
                        //FECHA
                        rowAssigments.Date = txtDate.DisplayDate;



                        //agregamos la nueva fila 
                        dataset.Assignments.AddAssignmentsRow(rowAssigments);
                        //actualizams la tabla con las modificaciones 
                        assignmentsTable.Update(dataset.Assignments);


                        //observaciones asignación
                        DSAssigmentsDataTableAdapters.Assignments_ObsTableAdapter assignmentsObsTable = new DSAssigmentsDataTableAdapters.Assignments_ObsTableAdapter();
                        DSAssigmentsData.Assignments_ObsRow rowObsAssigments = dataset.Assignments_Obs.NewAssignments_ObsRow();
                        //asignamos el id de la asignación
                        rowObsAssigments.Id = rowAssigments.Id;
                        //asignamos la observación
                        rowObsAssigments.Observations = txtObservation.Text.ToString();
                        //agregamos la nueva fila 
                        dataset.Assignments_Obs.AddAssignments_ObsRow(rowObsAssigments);
                        //actualizams la tabla con las observaciones modificadas 
                        assignmentsObsTable.Update(dataset.Assignments_Obs);


                        //Viajes(trips)
                        //actualizamos la tabla assigments en el dataset  (con esto insertamos un nuevo viaje) 
                        long Id = rowAssigments.Id;

                        //cremos la ruta 
                        DSAssigmentsData.RoutesDataTable routesTable = new DSAssigmentsData.RoutesDataTable();
                        DSAssigmentsDataTableAdapters.RoutesTableAdapter adaptRoute = new DSAssigmentsDataTableAdapters.RoutesTableAdapter();

                        //obtenemos de la tabla routes el nuevo registro que se introducira 
                        DSAssigmentsData.RoutesRow rowrRoutes = dataset.Routes.NewRoutesRow();
                        //fecha de creación
                        rowrRoutes.CreatedDate = DateTime.Now;
                        //fecha de modificacion
                        rowrRoutes.ModifiedDate = DateTime.Now;
                        //id de la nueva asignación
                        rowrRoutes.AssignmentId = rowAssigments.Id;
                        //el totalamount 
                        rowrRoutes.TotalAmount = 0;
                        //agregamos la fila 
                        dataset.Routes.AddRoutesRow(rowrRoutes);
                        //actualizamos la tabla 
                        adaptRoute.Update(dataset.Routes);

                        //habilitamos el boton de borrar
                        ButtonEliminar.IsEnabled = true;

                    }
                    Close();
                }
            }
            catch (Exception ex)
            {
                string exce = ex.ToString();
                MessageBox.Show("No se ha podido realizar la acción !");
            }
        }
        private int GeTIdCabByCode(string CodeCab)
        {
            int IdCab = 0;
            try
            {
                DSAssigmentsData dataset = new DSAssigmentsData();
                DSAssigmentsDataTableAdapters.CabInfoTableAdapter CabInfoTable = new DSAssigmentsDataTableAdapters.CabInfoTableAdapter();
                IdCab = Convert.ToInt32(CabInfoTable.ScalarQueryIdCabByCodeCab(CodeCab));
            }
            catch (Exception e)
            {
                string excepcion = e.ToString();           
            }
            return IdCab;
        }

        private int GeTIdDriverByCode(string CodeDriver)
        {
            int DriverId = 0;
            try
            {
                DSAssigmentsData dataset = new DSAssigmentsData();
                DSAssigmentsDataTableAdapters.DriversInfoTableAdapter DriverInfoTable = new DSAssigmentsDataTableAdapters.DriversInfoTableAdapter();
                DriverId = Convert.ToInt32(Convert.ToInt32(DriverInfoTable.ScalarQueryGetIdDriverByCode(CodeDriver)));
            }
            catch (Exception e)
            {
                string excepcion = e.ToString();
            }
            return DriverId;
        }


        private int GeTIdTrailerByCode(string CodeTrailer)
        {
            int TrailerId = 0;
            try
            {
                DSAssigmentsData dataset = new DSAssigmentsData();
                DSAssigmentsDataTableAdapters.TrailerInfoTableAdapter trailerInfoTable = new DSAssigmentsDataTableAdapters.TrailerInfoTableAdapter();
                TrailerId = Convert.ToInt32(Convert.ToInt32(trailerInfoTable.ScalarQueryGetIdTrailerByCodeTrailer(CodeTrailer)));
            }
            catch (Exception e)
            {
                string excepcion = e.ToString();
            }
            return TrailerId;
        }


        private void DeleteAssigment_click(object sender, RoutedEventArgs e)
        {
            bool Desasignar = false;
            bool EliminarAsignación = false;
            bool Asignaciónconpedidos = false;


            //con esto desasignamos todos los pedido poniendo el atributo IdTrip a nullo 
            Asignaciónconpedidos = GetOrdersAssigments(AssigmentId);
            //si tiene pedidos asignados preguntamos primero si queremos borrar la asignación
            if (Asignaciónconpedidos)
            {

                if (MessageBox.Show("La asignación tiene pedidos asignados, esta seguro de realizar la acción?", "Borrar Asignación", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {       
                    Desasignar = DesasignOrderToAssigments(AssigmentId);
                    //comprobación desasignar correcto
                    if (Desasignar)
                    {
                        //eliminamos la asignación y todas sus relaciones (routas y assigments_obs) 
                        EliminarAsignación = DeleteAssigmentsAndRelantionship(AssigmentId);
                        if (EliminarAsignación)
                        {
                            MessageBox.Show("Se ha realizado la acción correctamente");
                        }
                        else
                        {
                            MessageBox.Show("No se ha realizado la acción correctamente");
                        }
                    }
                    else
                    {
                        MessageBox.Show("no se ha podido eliminar la asignación correctamente");
                    }
                }
                
            }
            else
            {
                Desasignar = DesasignOrderToAssigments(AssigmentId);
                //comprobación desasignar correcto
                if (Desasignar)
                {
                    //eliminamos la asignación y todas sus relaciones 
                    EliminarAsignación = DeleteAssigmentsAndRelantionship(AssigmentId);
                    if (EliminarAsignación)
                    {
                        MessageBox.Show("Se ha realizado la acción correctamente");
                    }
                    else
                    {
                        MessageBox.Show("No se ha realizado la acción correctamente");
                    }
                }
                else
                {
                    MessageBox.Show("no se ha podido eliminar la asignación correctamente");
                }
            }

        
        }

        private bool GetOrdersAssigments(long assigmentId)
        {
            int contadorNumeroDePedidosAsignación = 0;
            bool assigmentswithOrders = false;
            DSAssigmentsData dataset = new DSAssigmentsData();
            DSAssigmentsDataTableAdapters.OrdersWithAssigmentsTableTableAdapter OrdersWithAssigments = new DSAssigmentsDataTableAdapters.OrdersWithAssigmentsTableTableAdapter();
            contadorNumeroDePedidosAsignación = Convert.ToInt32(OrdersWithAssigments.ScalarQueryOrdersWithAssigments(assigmentId));

            //la asignación tiene pedidos 
            if (contadorNumeroDePedidosAsignación > 0 )
            {
                assigmentswithOrders = true;
            }
            else
            {
                assigmentswithOrders = false;
            }
            return assigmentswithOrders;
        }

        private bool DesasignOrderToAssigments(long assigmentId)
        {      
            bool accioncorrecta = false;
            DataTable routesTableByAssigmentsId;
            DataTable TripTableByIdFromRoutes;
            try
            {
                DSAssigmentsData dataset = new DSAssigmentsData();
                DSAssigmentsDataTableAdapters.AssignmentsTableAdapter assignmentsTable = new DSAssigmentsDataTableAdapters.AssignmentsTableAdapter();
                DSAssigmentsDataTableAdapters.RoutesTableAdapter routesTable = new DSAssigmentsDataTableAdapters.RoutesTableAdapter();
                DSAssigmentsDataTableAdapters.TripsTableAdapter tripsTable = new DSAssigmentsDataTableAdapters.TripsTableAdapter();

                //obtenemos todas las rutas asignadas a la asignación correspondiente
                routesTableByAssigmentsId = routesTable.GetRoutesByAssigmentId(AssigmentId);

                //recorremos toda la tabla de rutas para la asignación que se quiere eliminar 
                foreach (DSAssigmentsData.RoutesRow row2 in routesTableByAssigmentsId.Rows)
                {
                    //recorremos todos los viajes para la ruta en concreto 
                    TripTableByIdFromRoutes = tripsTable.GetDataTripsByRoutesIdByRoutes(row2.Id);

                    // por cada viaje de la ruta en concreto pondremos el atributo id_route a nullo 
                    foreach (DSAssigmentsData.TripsRow row3 in TripTableByIdFromRoutes.Rows)
                    {
                        //ponemos el atributo RouteId a nulo 
                        row3.SetRouteIdNull();
                        //actualizamos en la tabla viaje
                        tripsTable.Update(row3);
                    }
                }
                  
                accioncorrecta = true;
            }
            catch (Exception e)
            {
                string excepcion = e.ToString();
                return false;
            }
            return accioncorrecta;
        }

        private bool DeleteAssigmentsAndRelantionship(long assigmentId)
        {
            int RutaId;
            bool accioncorrecta = false;
            DataTable routesTableByAssigmentsId;
            try
            {
                DSAssigmentsData dataset = new DSAssigmentsData();
                DSAssigmentsDataTableAdapters.AssignmentsTableAdapter assignmentsTable = new DSAssigmentsDataTableAdapters.AssignmentsTableAdapter();
                DSAssigmentsDataTableAdapters.Assignments_ObsTableAdapter assignmentsObsTable = new DSAssigmentsDataTableAdapters.Assignments_ObsTableAdapter();
                DSAssigmentsDataTableAdapters.RoutesTableAdapter routesTable = new DSAssigmentsDataTableAdapters.RoutesTableAdapter();
                DSAssigmentsDataTableAdapters.TripsTableAdapter tripsTable = new DSAssigmentsDataTableAdapters.TripsTableAdapter();

                //obtenemos todas las rutas asignadas a la asignación correspondiente
                routesTableByAssigmentsId = routesTable.GetRoutesByAssigmentId(AssigmentId);

                //recorremos las rutas asignadas a la asignación
                foreach (DSAssigmentsData.RoutesRow row in routesTableByAssigmentsId.Rows)
                {               
                    //borramos la ruta relacionada (tabla asignaction ID -> tablaroutes assignmentId)
                    routesTable.DeleteQueryRoutesByIdAssignmentsFromAssignments(AssigmentId);
                }
                //borramos las observaciones de la asignación 
                assignmentsObsTable.DeleteQueryByIdAssignments(Convert.ToInt64(AssigmentId));

                //borramos la asignación seleccionada
                assignmentsTable.DeleteQueryAssigmentsById(Convert.ToInt64(AssigmentId));

                accioncorrecta = true;
                Close();
            }
            catch (Exception e)
            {
                string excepcion = e.ToString();
                return false;
            }
            return accioncorrecta;
        }


        private bool validateDatas(string codigoConductor, string codigoTractora, string CodigoTrailer)
        {
            bool validacion = true;
            if (string.IsNullOrEmpty(codigoConductor))
            {
                MessageBox.Show("El codigo del conductor no puede estar vacio");
                validacion = false;
            }
            else
            {
                if(string.IsNullOrEmpty(codigoTractora))
                {
                    MessageBox.Show("El codigo del tractora no puede estar vacio");
                    validacion = false;
                }
                else
                {
                   if(string.IsNullOrEmpty(CodigoTrailer))
                   {
                        MessageBox.Show("El codigo del remolque no puede estar vacio");
                        validacion = false;
                   }
                }
            }
            return validacion;       
        }
    }
}