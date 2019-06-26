namespace EtasaDesktop.Distribution.Vehicles
{
    using System.Windows;
    using EtasaDesktop.Common.Data;

    /// <summary>
    /// Lógica de interacción para SearchChargerWindow.xaml
    /// </summary>
    public partial class SearchChargerWindow : Window
    {
        public SearchChargerWindow()
        {
            InitializeComponent();

            DataGridDataSearchCharger.ItemsSource = DataSearchCharger.CreateDummiesDataCharger();
        }
    }
}
