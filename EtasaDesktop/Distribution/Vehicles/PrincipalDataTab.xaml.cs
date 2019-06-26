using EtasaDesktop.Distribution.Vehicles;
using System.Windows;
using System.Windows.Controls;

namespace EtasaDesktop.Vehicles
{
    /// <summary>
    /// Lógica de interacción para PrincipalData.xaml
    /// </summary>
    public partial class PrincipalDataTab : UserControl
    {
        public PrincipalDataTab()
        {
            InitializeComponent();
        }

        private void SearchChargerButton_Click(object sender, RoutedEventArgs e)
        {
            SearchChargerWindow searchCharger = new SearchChargerWindow();
            searchCharger.ShowDialog();
        }
    }
}
