using EtasaDesktop.Models;
using EtasaDesktop.Models.BillingModel;
using System.Windows.Controls;

namespace EtasaDesktop.Billing
{
    /// <summary>
    /// Lógica de interacción para BillingPage.xaml
    /// </summary>
    public partial class MainBilling : UserControl
    {
        public MainBilling()
        {
            InitializeComponent();

            DataGridCodigoFactoria.ItemsSource = CodigoFactoria.CreateDummiesFactorial();
            DataGridTipoManguera.ItemsSource = TipoManguera.CreateDummiesTipoManguera();
            DataGridTamañoVehiculo.ItemsSource = TamañoVehiculo.CreateDummiesTamañoVehiculo();
            DataGridForFaits.ItemsSource = ForFaits.CreateDummiesForFaits();
            DataGridSuplidos.ItemsSource = Suplidos.CreateDummiesSuplidos();
        }
       
    }
}
