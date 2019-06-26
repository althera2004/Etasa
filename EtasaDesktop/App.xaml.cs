using EtasaDesktop.Common.Auth;
using EtasaDesktop.Common;
using Fluent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfOfficeTheme;

namespace EtasaDesktop
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SetStyle();

            base.OnStartup(e);

            Login();
        }


        private void Login()
        {
            if (EtasaDesktop.Properties.Settings.Default.Session != null)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                loginWindow.LoginSucceed += (s, ev) =>
                {
                    MainWindow mainWindow = new MainWindow();
                    loginWindow.Hide();
                    loginWindow.Close();
                    mainWindow.Show();
                };
            }
        }



        private void SetStyle()
        {
            if (TryFindResource("AccentColorBrush") is SolidColorBrush accentBrush)
                accentBrush.Color.CreateAccentColors();

            ThemeManager.AddAccent("CustomFluentTheme", new Uri("pack://application:,,,/EtasaDesktop;component/Resources/Theme/Ribbon.xaml"));

            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent("CustomFluentTheme"),
                                        theme.Item1);
        }
    }
}
