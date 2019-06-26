using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EtasaDesktop.Common.Tools;
using EtasaDesktop.Common.Auth.Users;


namespace EtasaDesktop.Distribution.Products
{
    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class ProductFrame :FrameControl
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
            ProductFormWindow ProductWindowWindow = new ProductFormWindow();
            ProductWindowWindow.ShowDialog();

            if (ProductWindowWindow.DialogResult.HasValue && ProductWindowWindow.DialogResult.Value)
            {
                Refresh();
            }
        }

        private void ShowProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedProduct != null)
            {
                //_viewModel.SelectedUser.Id
                ProductFormWindow ProductWindow = new ProductFormWindow(_viewModel.SelectedProduct.Id);
                ProductWindow.ShowDialog();

                if (ProductWindow.DialogResult.HasValue && ProductWindow.DialogResult.Value)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Usuario, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }




    }
}


