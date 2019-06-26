namespace EtasaDesktop.Distribution.Clients
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EtasaDesktop.Common.Tools;

    /// <summary>
    /// Lógica de interacción para AssignamentsView.xaml
    /// </summary>
    public partial class ClientFrame : FrameControl
    {
        private ClientViewModel _viewModel;

        public ClientFrame()
        {
             InitializeComponent();
            _viewModel = (ClientViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Clientes...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            ShowItemData(0);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as ClientDataSet.ClientSummariesRow;
            ShowItemData(item.Id);
        }

        private void ShowClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedClient != null)
            {
                ShowItemData(_viewModel.SelectedClient.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Cliente, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(int id)
        {
            ClientFormWindow ClientWindow = new ClientFormWindow(id);
            ClientWindow.ShowDialog();

            if (ClientWindow.DialogResult.HasValue && ClientWindow.DialogResult.Value)
            {
                Refresh();
            }
        }
    }
}
