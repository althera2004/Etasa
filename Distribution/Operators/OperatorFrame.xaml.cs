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

namespace EtasaDesktop.Distribution.Operators
{ 
    /// <summary>
    /// Lógica de interacción para AssignamentsView.xaml
    /// </summary>
    public partial class OperatorFrame : FrameControl
    {
        private OperatorViewModel _viewModel;

        public OperatorFrame()
        {
             InitializeComponent();
            _viewModel = (OperatorViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Operadores...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddOperator_Click(object sender, RoutedEventArgs e)
        {
            OperatorFormWindow OperatorWindow = new OperatorFormWindow();
            OperatorWindow.ShowDialog();

            if (OperatorWindow.DialogResult.HasValue && OperatorWindow.DialogResult.Value)
            {
                Refresh();
            }
        }

        private void ShowOperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedOperator != null)
            {
                OperatorFormWindow OperatorWindow = new OperatorFormWindow(_viewModel.SelectedOperator.Id);
                OperatorWindow.ShowDialog();

                if (OperatorWindow.DialogResult.HasValue && OperatorWindow.DialogResult.Value)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Cliente, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }
   
    }
}
