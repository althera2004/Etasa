using EtasaDesktop.Models.BillingModel;
using System.Windows.Controls;

namespace EtasaDesktop.Vehicles
{
    /// <summary>
    /// Lógica de interacción para CisternData.xaml
    /// </summary>
    public partial class CisternDataTab : UserControl
    {
        public CisternDataTab()
        {
            InitializeComponent();

            DataGridDataCistern.ItemsSource = DataCistern.CreateDummiesDataCistern();
        }
    }
}
