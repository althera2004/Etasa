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
using EtasaDesktop.Distribution.Factories;


namespace EtasaDesktop.Distribution.Factories
{
    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class FactoryFrame :FrameControl
    {

        private FactoryViewModel _viewModel;


        public FactoryFrame()
        {
            InitializeComponent();
            _viewModel = (FactoryViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Factorias...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddFactory_Click(object sender, RoutedEventArgs e)
        {
            FactoryFormWindow ProductWindowWindow = new FactoryFormWindow();
            ProductWindowWindow.ShowDialog();

            if (ProductWindowWindow.DialogResult.HasValue && ProductWindowWindow.DialogResult.Value)
            {
                Refresh();
            }
        }

        private void ShowFactoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedFactory != null)
            {
                //_viewModel.SelectedUser.Id
                FactoryFormWindow FactoryWindow = new FactoryFormWindow(_viewModel.SelectedFactory.Id);
                FactoryWindow.ShowDialog();

                if (FactoryWindow.DialogResult.HasValue && FactoryWindow.DialogResult.Value)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Factoría, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }




    }
}


