namespace EtasaDesktop.Distribution.Products
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EtasaDesktop.Common.Tools;

    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class ProductFrame : FrameControl
    {

        private ProductViewModel _viewModel;


        public ProductFrame()
        {
            InitializeComponent();
            _viewModel = (ProductViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Producto...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            ShowItemData(0);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as ProductDataSet.ProductsSummariesRow;
            ShowItemData(item.Id);
        }

        private void ShowProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedProduct != null)
            {
                ShowItemData(_viewModel.SelectedProduct.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Usuario, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(int id)
        {
            ProductFormWindow ProductWindowWindows = new ProductFormWindow(id);
            ProductWindowWindows.ShowDialog();

            if (ProductWindowWindows.DialogResult.HasValue && ProductWindowWindows.DialogResult.Value)
            {
                Refresh();
            }
        }
    }
}