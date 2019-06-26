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
using System.Windows.Shapes;

namespace EtasaDesktop.Distribution.Drivers
{
    /// <summary>
    /// Lógica de interacción para UserFormViewModel.xaml
    /// </summary>
    public partial class DriverFormWindow : Window
    {
        private DriverFormViewModel _viewModel;
    
        public DriverFormWindow(int DriverId = 0)
        {
            _viewModel = new DriverFormViewModel();
            DataContext = _viewModel;
            InitializeComponent();

            _viewModel.FormLoadError += FormLoadError_Event;
            _viewModel.FormSaveFinished += FormSaveFinished_Event;
            _viewModel.FormSaveError += FormSaveError_Event;
            _viewModel.FormRequiredEmpty += FormRequiredEmpty_Event;

            if (DriverId > 0)
            {
                Title.Content = "Editar Conductor";
                _viewModel.Load(DriverId);
            }
            else
            {
                Title.Content = "Nuevo Conductor";
            }
        }


        private void FormLoadError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido cargar el conductor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBox.Show("No se ha podido guardar el conductor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
