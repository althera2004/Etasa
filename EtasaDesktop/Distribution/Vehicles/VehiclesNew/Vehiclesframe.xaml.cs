namespace EtasaDesktop.Distribution.Vehicles.VehiclesNew
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EtasaDesktop.Common.Tools;

    /// <summary>
    /// Lógica de interacción para Vehiclesframe.xaml
    /// </summary>
    public partial class Vehiclesframe : FrameControl
    {
        private VehiclesViewModel _viewModel;

        public Vehiclesframe()
        {
            InitializeComponent();
            _viewModel = (VehiclesViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Vehiculos...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }

            Main.Status = "Listo";
        }

        private void AddVehicle_Click(object sender, RoutedEventArgs e)
        {
            ShowItemData(0);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as VehicleDataSet1.VehiclesSummariesRow;
            ShowItemData(item.Id);
        }

        private void ShowVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedVehicle != null)
            {
                ShowItemData(_viewModel.SelectedVehicle.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Vehiculo, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(int id)
        {
            VehicleFormWindow VehicleWindowWindows = new VehicleFormWindow(id);
            VehicleWindowWindows.ShowDialog();

            if (VehicleWindowWindows.DialogResult.HasValue && VehicleWindowWindows.DialogResult.Value)
            {
                Refresh();
            }
        }
    }
}