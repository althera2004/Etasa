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
using EtasaDesktop.Distribution.Orders;


namespace EtasaDesktop.Distribution.Orders.Form
{
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
            OrderFormWindow OrdersWindowWindow = new OrderFormWindow();
            OrdersWindowWindow.ShowDialog();

            if (OrdersWindowWindow.DialogResult.HasValue && OrdersWindowWindow.DialogResult.Value)
            {
                Refresh();
            }
        }

        private void ShowOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedOrder != null)
            {
                //_viewModel.SelectedUser.Id
                OrderFormWindow OrdersWindowWindow = new OrderFormWindow(_viewModel.SelectedOrder.Id);
                OrdersWindowWindow.ShowDialog();

                if (OrdersWindowWindow.DialogResult.HasValue && OrdersWindowWindow.DialogResult.Value)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Pedido, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }
    }
}


