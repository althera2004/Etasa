using EtasaDesktop.Common.Tools;
using EtasaDesktop.Vehicles;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EtasaDesktop.Distribution.Vehicles
{
    /// <summary>
    /// Lógica de interacción para ListVehicleView.xaml
    /// </summary>
    public partial class ListVehicleView : FrameControl
    {
        DataSet dsSelected;

        public ListVehicleView()
        {
            InitializeComponent();
            refreshDataGridAssignment();
        }

        private void AddVehicle_Click(object sender, RoutedEventArgs e)
        {
            MainVehicle vehicleWindow = new MainVehicle();
            vehicleWindow.ShowDialog();
        }

        private void ShowVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow dr = dsSelected.Tables[0].Rows[DataGridVehicle.SelectedIndex];

                MainVehicle assignmentWindow = new MainVehicle(dr);
                assignmentWindow.ShowDialog();
            }
            catch (IndexOutOfRangeException)
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un vehículo, selecciona un elemento de la lista",
                                          "Confirmation",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Warning);
            }
        }

        public void refreshDataGridAssignment()
        {
            dsSelected = VehicleBBDD.GetListVehicleData();

            var result = from asig in dsSelected.Tables[0].AsEnumerable()
                         select new
                         {
                             Codigo = asig["Codigo"],
                             TipoVehiculo = asig["TipoVehiculo"],
                             Matricula = asig["Matricula"],
                             Bastidor = asig["Bastidor"],
                             Modelo = asig["Modelo"],
                             NumEjes = asig["NumEjes"],
                             Tara = asig["Tara"],
                             PMA = asig["PMA"],
                             FechaTac = asig["FechaTac"],
                             FechaItv = asig["FechaItv"],
                             FechaTpc = asig["FechaTpc"],
                             FechaSeg = asig["FechaSeg"],
                             FechaRevisionManguera = asig["FechaRevisionManguera"],
                             TMV = (bool)asig["TMV"]
                         };

            DataGridVehicle.ItemsSource = result;
        }
    }
}
