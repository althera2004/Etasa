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
    public partial class OrderFormWindow : Window
    {

        public Order Order;
        public long OrderIds =0;

        public OrderFormViewModel _viewModel;

        public OrderFormWindow(long OrderId = 0)
        {
            _viewModel = new OrderFormViewModel();
            DataContext = _viewModel;
            InitializeComponent();

            _viewModel.FormLoadError += FormLoadError_Event;
            _viewModel.FormSaveFinished += FormSaveFinished_Event;
            _viewModel.FormSaveError += FormSaveError_Event;
            _viewModel.FormRequiredEmpty += FormRequiredEmpty_Event;
            OrderIds = Convert.ToInt64(OrderId);
            if (OrderId > 0)
            {
                Title.Content = "Editar pedido";
                _viewModel.Load(OrderId);
                DuplicateBoton.IsEnabled = true;
            }
            else
            {
                Title.Content = "Nuevo pedido";
                DuplicateBoton.IsEnabled = false;
            }
        }
        

       private void DuplicatedTripOrder_click(object sender, RoutedEventArgs e)
       {
            duplicatedTripOrder(OrderIds);
       }
       private void duplicatedTripOrder(long OrderId)
       {
            OrderDataSet dataset = new OrderDataSet();
            OrderDataSetTableAdapters.OrdersTableAdapter TableOrder = new OrderDataSetTableAdapters.OrdersTableAdapter();
            OrderDataSet.OrdersDataTable dataTable = TableOrder.GetDataOrderById(OrderId);

            //Comprobramos que el pedido existe 
            if (dataTable.Rows.Count > 0)
            {

                //creamos el viaje que se asignara al pedido
                OrderDataSet ImportDataSet = new OrderDataSet();
                OrderDataSetTableAdapters.TripsTableAdapter tripstable = new OrderDataSetTableAdapters.TripsTableAdapter();
                //obtenemos de la tabla trips  el nuevo registro que se introducira 
                var rowTrip = ImportDataSet.Trips.NewTripsRow();

                //la ruta a la que pertenece el viaje que estamos creando y esta asignado la asignación
                rowTrip.SetRouteIdNull();
                //fecha de creación
                rowTrip.CreatedDate = DateTime.Now;
                //fecha de modificación
                rowTrip.ModifiedDate = DateTime.Now;
                //posición dentro la lista (empiezan por zero las listas en posiciones) (el primer elemento empieza por 1)           
                rowTrip.Position = 0;
                //ponemos el estado del viaje a pendiente 
                rowTrip.status = 1;

                //cantidad cargada del pedido       
                rowTrip.LoadedAmount = Convert.ToInt32(dataTable[0][19].ToString());
                //fecha de carga
                rowTrip.LoadedDate = DateTime.Now;

                //agregamos la nueva fila 
                ImportDataSet.Trips.AddTripsRow(rowTrip);
                //asignamos el id del pedido 
                rowTrip.Id_Order = Convert.ToInt32(OrderId);
                //actualizamos la tabla trips en el dataset  (con esto insertamos un nuevo viaje) 
                tripstable.Update(ImportDataSet.Trips);

            }
        }



        private void FormLoadError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido cargar el pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        private void FormRequiredEmpty_Event()
        {
            MessageBoxResult result = MessageBox.Show("Hay campos obligatorios sin rellenar",
                          "Confirmation",
                          MessageBoxButton.OK,
                          MessageBoxImage.Warning);
        }
        private void FormSaveFinished_Event()
        {
            DialogResult = true;
            Close();
        }
        private void FormSaveError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido guardar el pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Attachment_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Ficheros de texto (*.txt)|*.txt|Todos los ficheros (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    OrderAttachList.Items.Add(Path.GetFileName(filename));
            }
        }

        private void Attachment_DragEnter(object sender, DragEventArgs e)
        {
            TabForm.SelectedIndex = 2;
            AttachView.Visibility = Visibility.Hidden;
            DropView.Visibility = Visibility.Visible;
        }

        private void Attachment_Drop(object sender, DragEventArgs e)
        {
            string[] droppedFilenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            foreach (string filename in droppedFilenames)
                OrderAttachList.Items.Add(Path.GetFileName(filename));

            DropView.Visibility = Visibility.Hidden;
            AttachView.Visibility = Visibility.Visible;
        }

        private void Attachment_DragLeave(object sender, DragEventArgs e)
        {
            DropView.Visibility = Visibility.Hidden;
            AttachView.Visibility = Visibility.Visible;
        }

        private void OrderAttachList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && OrderAttachList.SelectedIndex >= 0)
            {
                OrderAttachList.Items.RemoveAt(OrderAttachList.SelectedIndex);
            }
        }




    }
}
