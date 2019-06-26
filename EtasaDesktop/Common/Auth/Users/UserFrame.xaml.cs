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
using EtasaDesktop.Common.Auth.Users;

namespace EtasaDesktop.Common.Auth.Users
{
    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class UserFrame :FrameControl
    {

        private UserViewModel _viewModel;


        public UserFrame()
        {
            InitializeComponent();
            _viewModel = (UserViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Usuario...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            UserFormWindow UserWindowWindow = new UserFormWindow();
            UserWindowWindow.ShowDialog();

            if (UserWindowWindow.DialogResult.HasValue && UserWindowWindow.DialogResult.Value)
            {
                Refresh();
            }
        }

        private void ShowUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedUser != null)
            {
                //_viewModel.SelectedUser.Id
                UserFormWindow UserWindow = new UserFormWindow(_viewModel.SelectedUser.Id);
                UserWindow.ShowDialog();

                if (UserWindow.DialogResult.HasValue && UserWindow.DialogResult.Value)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Usuario, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }




    }
}


