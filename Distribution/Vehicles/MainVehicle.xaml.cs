namespace EtasaDesktop.Vehicles
{
    using System.Data;
    using System.Windows;
    using EtasaDesktop.Distribution.Vehicles;
    using EtasaDesktop.Models.BillingModel;

    /// <summary>
    /// Lógica de interacción para VehiclePage.xaml
    /// </summary>
    public partial class MainVehicle : Window
    {
        Vehicle vehicle = new Vehicle();

        public MainVehicle()
        {
            InitializeComponent();

            DataGridMaintenance.ItemsSource = Maintenance.CreateDummiesMaintenance();
            principalDataTab.DataContext = vehicle;
        }

        public MainVehicle(DataRow dr)
        {
            InitializeComponent();

            DataGridMaintenance.ItemsSource = Maintenance.CreateDummiesMaintenance();
            DataContext = vehicle;
            
            vehicle.Empresa = int.Parse(dr["Empresa"].ToString());
            vehicle.TipoVehiculo = dr["TipoVehiculo"].ToString();
            vehicle.MarcaVehiculo = dr["MarcaVehiculo"].ToString();
            vehicle.Modelo = dr["Modelo"].ToString();
            vehicle.Matricula = dr["Matricula"].ToString();
            vehicle.Bastidor = dr["Bastidor"].ToString();
            vehicle.NumEjes = int.Parse(dr["NumEjes"].ToString());
            vehicle.Tara = int.Parse(dr["Tara"].ToString());
            vehicle.Pma = int.Parse(dr["PMA"].ToString());
            vehicle.PmaAutorizado = int.Parse(dr["PmaAutorizado"].ToString());
            vehicle.PmaEspecial = int.Parse(dr["PmaEspecial"].ToString());
            vehicle.FechaMatriculacion = dr["FechaMatriculacion"].ToString();
            vehicle.FechaFabricacion = dr["FechaFabricacion"].ToString();
            vehicle.FechaAlta = dr["FechaAlta"].ToString();
            vehicle.FechaBaja = dr["FechaBaja"].ToString();
            vehicle.FechaRevision = dr["FechaRevision"].ToString();
            vehicle.FechaRevisionAnt = dr["FechaRevisionAnt"].ToString();
            vehicle.FechaItv = dr["FechaItv"].ToString();
            vehicle.FechaItvAnt = dr["FechaItvAnt"].ToString();
            vehicle.FechaSeg = dr["FechaSeg"].ToString();
            vehicle.FechaSegAnt = dr["FechaSegAnt"].ToString();
            vehicle.FechaPol = dr["FechaPol"].ToString();
            vehicle.FechaPolAnt = dr["FechaPolAnt"].ToString();
            vehicle.CodigoProveedorGps = dr["CodigoProveedorGps"].ToString();
            vehicle.IdVehiculoGps = int.Parse(dr["IdVehiculoGps"].ToString());
            vehicle.Antiguedad = dr["Antiguedad"].ToString();
            vehicle.CodigoAlquilado = dr["CodigoAlquilado"].ToString();
            vehicle.CodigoZona = dr["CodigoZona"].ToString();
            vehicle.CodigoCentroCoste = dr["CodigoCentroCoste"].ToString();
            vehicle.NodoSalida = dr["NodoSalida"].ToString();
            vehicle.NodoLlegada = dr["NodoLlegada"].ToString();
            vehicle.HoraInicio = dr["HoraInicio"].ToString();
            vehicle.HoraFinal = dr["HoraFinal"].ToString();
            vehicle.Observaciones = dr["Observaciones"].ToString();
            vehicle.Paralizado = bool.Parse(dr["Paralizado"].ToString());
            vehicle.MotivoParalizacion = dr["MotivoParalizacion"].ToString();
            vehicle.Imagen = dr["Imagen"].ToString();

            vehicle.TipoTarjeta = dr["TipoTarjeta"].ToString();
            vehicle.TMV = bool.Parse(dr["TMV"].ToString());
            vehicle.NumExtintor1 = int.Parse(dr["NumExtintor1"].ToString());
            vehicle.FechaExtintor1Ant = dr["FechaExtintor1Ant"].ToString();
            vehicle.FechaExtintor1 = dr["FechaExtintor1"].ToString();
            vehicle.FechaVencimientoCrmAnt = dr["FechaVencimientoCrmAnt"].ToString();
            vehicle.FechaVencimientoCrm = dr["FechaVencimientoCrm"].ToString();
            vehicle.FechaTarjTransAnt = dr["FechaTarjTransAnt"].ToString();
            vehicle.FechaTarjTrans = dr["FechaTarjTrans"].ToString();
            vehicle.FechaTacAnt = dr["FechaTacAnt"].ToString();
            vehicle.FechaTac = dr["FechaTac"].ToString();
            vehicle.FechaLimitadorAnt = dr["FechaLimitadorAnt"].ToString();
            vehicle.FechaLimitador = dr["FechaLimitador"].ToString();
            vehicle.FechaFinalPermisoComunitario = dr["FechaFinalPermisoComunitario"].ToString();

            vehicle.TipoManguera = dr["TipoManguera"].ToString();
            vehicle.IdEquipoMedicion = int.Parse(dr["IdEquipoMedicion"].ToString());
            vehicle.FechaRevisionMangueraAnt = dr["FechaRevisionMangueraAnt"].ToString();
            vehicle.FechaRevisionManguera = dr["FechaRevisionManguera"].ToString();
            vehicle.FechaADR = dr["FechaADR"].ToString();
            vehicle.CodigoPruebaADR = dr["CodigoPruebaADR"].ToString();
            vehicle.FechaCalibraAnt = dr["FechaCalibraAnt"].ToString();
            vehicle.FechaCalibra = dr["FechaCalibra"].ToString();
            vehicle.FechaCalContAnt = dr["FechaCalContAnt"].ToString();
            vehicle.FechaCalCont = dr["FechaCalCont"].ToString();
            vehicle.FechaCalTerAnt = dr["FechaCalTerAnt"].ToString();
            vehicle.FechaCalTer = dr["FechaCalTer"].ToString();
            vehicle.Llave = int.Parse(dr["Llave"].ToString());
            vehicle.ValvulaSeg = int.Parse(dr["ValvulaSeg"].ToString());
            vehicle.Precintado = int.Parse(dr["Precintado"].ToString());
            vehicle.Bomba = int.Parse(dr["Bomba"].ToString());
            vehicle.Calorifugada = int.Parse(dr["Calorifugada"].ToString());
            vehicle.Pinzas = int.Parse(dr["Pinzas"].ToString());
            vehicle.Contador = int.Parse(dr["Contador"].ToString());
            vehicle.RecuperaVapores = int.Parse(dr["RecuperaVapores"].ToString());
        }

        private void ButtonSearchVehicle_Click(object sender, RoutedEventArgs e)
        {
            SearchVehiclesPrincipalData searchVehicleWindow = new SearchVehiclesPrincipalData();
            searchVehicleWindow.ShowDialog();
        }
    }
}
