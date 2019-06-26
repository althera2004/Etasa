using EtasaDesktop.Common.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace EtasaDesktop.Distribution.Orders.Form
{
    public partial class OrderFormLightWindow : Window
    {

        public OrderFormLightViewModel _viewModel;
       

        public OrderFormLightWindow(Order order = null)
        {
            order = new Order(order);
            _viewModel = new OrderFormLightViewModel(order);
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Order == null)
            {
                Title.Content = "Nuevo pedido";
            }
            else
            {
                Title.Content = "Editar pedido";
            }
        }

        private void Canceled_Checked(object sender, RoutedEventArgs e)
        {
            _viewModel.Order.Status = 8;
        }
        private void Canceled_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewModel.Order.Status = 1;
        }
        private void Canceled_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox) sender;
            check.IsChecked = _viewModel.Order.Status == 8;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
