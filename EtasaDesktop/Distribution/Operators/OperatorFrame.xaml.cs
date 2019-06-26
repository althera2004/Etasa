namespace EtasaDesktop.Distribution.Operators
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EtasaDesktop.Common.Tools;

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
            ShowItemData(0);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as OperatorDataset.OperatorSummariesRow;
            ShowItemData(item.Id);
        }

        private void ShowOperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedOperator != null)
            {
                ShowItemData(_viewModel.SelectedOperator.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Operador, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(int id)
        {
            OperatorFormWindow OperatorWindow = new OperatorFormWindow(id);
            OperatorWindow.ShowDialog();

            if (OperatorWindow.DialogResult.HasValue && OperatorWindow.DialogResult.Value)
            {
                Refresh();
            }
        }

    }
}
