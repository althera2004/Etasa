namespace EtasaDesktop.Distribution.Factories
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EtasaDesktop.Common.Tools;

    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class FactoryFrame : FrameControl
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
            ShowItemData(0);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as FactoryDataSet.FactoriesSummariesRow;
            ShowItemData(item.Id);
        }

        private void ShowFactoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedFactory != null)
            {
                ShowItemData(_viewModel.SelectedFactory.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Factoría, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(int id)
        {
            FactoryFormWindow FactoryWindow = new FactoryFormWindow(id);
            FactoryWindow.ShowDialog();

            if (FactoryWindow.DialogResult.HasValue && FactoryWindow.DialogResult.Value)
            {
                Refresh();
            }
        }
    }
}