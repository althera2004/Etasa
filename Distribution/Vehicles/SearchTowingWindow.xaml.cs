namespace EtasaDesktop.Distribution.Vehicles
{
    using System.Windows;
    using EtasaDesktop.Common.Data;

    /// <summary>
    /// Lógica de interacción para SearchTowingWindow.xaml
    /// </summary>
    public partial class SearchTowingWindow : Window
    {
        public SearchTowingWindow()
        {
            InitializeComponent();

            DataGridDataSearchTowing.ItemsSource = DataSearchTowing.CreateDummiesDataTowing();
        }
    }
}