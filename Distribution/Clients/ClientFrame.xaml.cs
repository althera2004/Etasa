using EtasaDesktop.Common.Tools;
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

namespace EtasaDesktop.Distribution.Clients
{

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
            ClientFormWindow ClientWindowWindow = new ClientFormWindow();
            ClientWindowWindow.ShowDialog();

            if (ClientWindowWindow.DialogResult.HasValue && ClientWindowWindow.DialogResult.Value)
            {
                Refresh();
            }
        }

        private void ShowClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedClient != null)
            {
                ClientFormWindow ClientWindow = new ClientFormWindow(_viewModel.SelectedClient.Id);
                ClientWindow.ShowDialog();

                if (ClientWindow.DialogResult.HasValue && ClientWindow.DialogResult.Value)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Cliente, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }
   
    }
}
