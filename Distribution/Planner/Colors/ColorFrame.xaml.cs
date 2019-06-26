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

namespace EtasaDesktop.Distribution.Planner.Colors
{
    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class ColorFrame : FrameControl
    {

        private ColorViewModel _viewModel;


        public ColorFrame()
        {
            InitializeComponent();
            _viewModel = (ColorViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Colores planificador...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddDriver_Click(object sender, RoutedEventArgs e)
        {
            ColorFormWindow ColorWindowWindows = new ColorFormWindow();
            ColorWindowWindows.ShowDialog();

            if (ColorWindowWindows.DialogResult.HasValue && ColorWindowWindows.DialogResult.Value)
            {
                Refresh();
            }
        }

        private void ShowDriverButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedFactoryColor != null)
            {
                //_viewModel.SelectedUser.Id
                ColorFormWindow ColorWindowWindows = new ColorFormWindow(_viewModel.SelectedFactoryColor.Id);
                ColorWindowWindows.ShowDialog();

                if (ColorWindowWindows.DialogResult.HasValue && ColorWindowWindows.DialogResult.Value)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Color de factoría, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }




    }
}


