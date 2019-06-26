namespace EtasaDesktop.Distribution.Orders.Form
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EtasaDesktop.Common.Tools;

    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class OrderFrame :FrameControl
    {
        private OrderViewModel _viewModel;

        public OrderFrame()
        {
            InitializeComponent();
            _viewModel = (OrderViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Pedidos...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            ShowItemData(0);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as OrderDataSet.OrderSummariesRow;
            ShowItemData(item.Id);
        }

        private void ShowOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedOrder != null)
            {
                ShowItemData(_viewModel.SelectedOrder.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Pedido, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(long id)
        {
            OrderFormWindow OrdersWindowWindow = new OrderFormWindow(id);
            OrdersWindowWindow.ShowDialog();

            if (OrdersWindowWindow.DialogResult.HasValue && OrdersWindowWindow.DialogResult.Value)
            {
                Refresh();
            }
        }
    }
}