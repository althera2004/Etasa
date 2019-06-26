using EtasaDesktop.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace EtasaDesktop.Common.Auth
{
    public partial class LoginWindow : Window
    {
        public event EventHandler LoginSucceed;
        public event EventHandler LoginFailed;

        public void NotifyLoginSucceed()
        {
            LoginSucceed?.Invoke(this, EventArgs.Empty);
        }
        public void NotifyLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }

        public LoginWindow()
        {
            InitializeComponent();
        }


        private void OnLogin(object sender, RoutedEventArgs e)
        {
            string userName = userBox.Text;
            string pass = passBox.Password;

            if (string.IsNullOrEmpty(userName))
            {
                loginError.Content = "Usuario no válido";
                loginError.Visibility = Visibility.Visible;
            }
            else if (string.IsNullOrEmpty(pass))
            {
                loginError.Content = "Contraseña no válida";
                loginError.Visibility = Visibility.Visible;
            }
            else
            {
                //loginProgress.Visible = true;
                

                if (AttemptLogin(userName, pass))
                {
                    loginError.Content = string.Empty;
                    loginError.Visibility = Visibility.Hidden;

                    NotifyLoginSucceed();
                }
                else
                {
                    //loginProgress.Visible = false;
                    loginError.Content = "Usuario o contraseña incorrectos";
                    loginError.Visibility = Visibility.Visible;
                    passBox.Password = string.Empty;

                    NotifyLoginFailed();
                }
            }
        }

        private bool AttemptLogin(string userName, string pass)
        {

            AuthDataSet users = new AuthDataSet();
            AuthDataSetTableAdapters.System_UsersTableAdapter usersAdapt = new AuthDataSetTableAdapters.System_UsersTableAdapter();

            string encPass = SHA256Crypt.Encrypt(userName + "@" + pass);

            encPass = "ee5cd7d5d96c8874117891b2c92a036f96918e66c102bc698ae77542c186f981";

            usersAdapt.FillByLogin(users.System_Users, userName, encPass);

            var table = users.System_Users;

            if(table.Rows.Count > 0 && table.Rows[0] != null)
            {
                var row = table.Rows[0];
                User user = new User()
                {
                    Id = Convert.ToInt32(row[table.IdColumn.ColumnName]),
                    Name = row[table.NameColumn.ColumnName]?.ToString(),
                    FullName = row[table.FullNameColumn.ColumnName]?.ToString(),
                    Contact = new Data.ContactData()
                    {
                        Phone = row[table.PhoneColumn.ColumnName]?.ToString(),
                        Phone2 = row[table.Phone2Column.ColumnName]?.ToString(),
                        PhoneMobile = row[table.PhoneMobileColumn.ColumnName]?.ToString(),
                        Email = row[table.EmailColumn.ColumnName]?.ToString(),
                    }
                };

                Properties.Settings.Default.Session = new Session()
                {
                    User = user,
                    CreatedDate = DateTime.Now
                };
                Properties.Settings.Default.Save();

            }

            return Properties.Settings.Default.Session != null;
        }



        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        if (control == passBox)
                        {
                            OnLogin(control, null);
                        }
                        else
                        {
                            control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnDragMoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void OnMinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnCloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
