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

namespace EtasaDesktop.Distribution.Operators
{
    public partial class OperatorFormWindow : Window
    {
        private OperatorFormViewModel _viewModel;

        public OperatorFormWindow(int OperatorId = 0)
        {
            _viewModel = new OperatorFormViewModel();
            DataContext = _viewModel;
            InitializeComponent();
    
            _viewModel.FormLoadError += FormLoadError_Event;
            _viewModel.FormSaveFinished += FormSaveFinished_Event;
            _viewModel.FormSaveError += FormSaveError_Event;
            _viewModel.FormRequiredEmpty += FormRequiredEmpty_Event;

            if (OperatorId > 0)
            {
                Title.Content = "Editar Operador";
                _viewModel.Load(OperatorId);
            }
            else
            {
                Title.Content = "Nuevo operador";

            }
        }



        private void FormLoadError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido cargar el operador", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBox.Show("No se ha podido guardar el operador", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
