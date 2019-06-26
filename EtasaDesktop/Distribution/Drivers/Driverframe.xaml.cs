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
using EtasaDesktop.Distribution.Drivers;

namespace EtasaDesktop.Distribution.Drivers
{
    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class DriverFrame :FrameControl
    {

        private DriverViewModel _viewModel;


        public DriverFrame()
        {
            InitializeComponent();
            _viewModel = (DriverViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Conductor...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddDriver_Click(object sender, RoutedEventArgs e)
        {
            ShowItemData(0);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as DriversDataSet.DriversSummariesRow;
            ShowItemData(item.Id);
        }

        private void ShowDriverButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedDriver != null)
            {
                ShowItemData(_viewModel.SelectedDriver.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Conductor, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(int id)
        {
            DriverFormWindow DriverWindowWindows = new DriverFormWindow(id);
            DriverWindowWindows.ShowDialog();

            if (DriverWindowWindows.DialogResult.HasValue && DriverWindowWindows.DialogResult.Value)
            {
                Refresh();
            }
        }
    }
}


