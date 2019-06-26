using EtasaDesktop.Common.Data;
using EtasaDesktop.Distribution.Clients;
using EtasaDesktop.Distribution.Orders;
using Microsoft.Maps.MapControl.WPF;
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
using EtasaDesktop.Resources.DrapAndDrop;

namespace EtasaDesktop.Distribution.Planner.Map
{
    public partial class MarkerClient : UserControl
    {

        private MarkerClientViewModel _viewModel;

        public MarkerClient(MarkerClientViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            MapLayer.SetPositionOrigin(this, PositionOrigin.BottomCenter);
            new ListViewDragDropManager<Order>(ListClientOrders);
            MapLayer.SetPosition(this, new Location(_viewModel.Client.Location.Latitude, _viewModel.Client.Location.Longitude));
            this.DataContext = _viewModel;
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            //OrderFormWindow orderForm = new OrderFormWindow();
            //orderForm.Order = _viewModel.Orders[0];
            //orderForm.ShowDialog();
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            //ClientFormWindow clientForm = new ClientFormWindow();
            //clientForm.ShowDialog();
        }
    }
}
