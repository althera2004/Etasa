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

namespace EtasaDesktop.Common.Auth.Users
{
    /// <summary>
    /// Lógica de interacción para UserFormViewModel.xaml
    /// </summary>
    public partial class UserFormWindow : Window
    {
        private UserFormViewModel _viewModel;
    
        public UserFormWindow(int UserId = 0)
        {
            _viewModel = new UserFormViewModel();
            DataContext = _viewModel;
            InitializeComponent();

            _viewModel.FormLoadError += FormLoadError_Event;
            _viewModel.FormSaveFinished += FormSaveFinished_Event;
            _viewModel.FormSaveError += FormSaveError_Event;


            if (UserId > 0)
            {
                Title.Content = "Editar usuario";              
                _viewModel.Load(UserId, txtoldPassword);
            }
            else
            {
                Title.Content = "Nuevo usuario";
            }
        }


        private void FormLoadError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido cargar el usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBox.Show("No se ha podido guardar el usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
