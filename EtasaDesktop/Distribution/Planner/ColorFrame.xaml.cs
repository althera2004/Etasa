namespace EtasaDesktop.Distribution.Planner
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EtasaDesktop.Common.Tools;

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

        private void AddColor_Click(object sender, RoutedEventArgs e)
        {
            ShowItemData(0);
        }

        // Juan Castilla - no se usa
        /*private  void paintGrid()
        {
            System.Collections.ObjectModel.ObservableCollection<DataGridColumn> axel = DataGrifactories.Columns;
            foreach (DataGridColumn item in axel)
            {
      
            }
        }*/

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as ColorDataSet.ColorSummariesFactoriesDataTableRow;
            ShowItemData(item.Id);
        }

        private void ShowColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedFactoryColor != null)
            {
                ShowItemData(_viewModel.SelectedFactoryColor.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de un Color de factoría, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(int id)
        {
            ColorFormWindow ColorWindowWindows = new ColorFormWindow(id);
            ColorWindowWindows.ShowDialog();
            if (ColorWindowWindows.DialogResult.HasValue && ColorWindowWindows.DialogResult.Value)
            {
                Refresh();
            }
        }
    }
}


