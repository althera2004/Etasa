namespace EtasaDesktop.Distribution.Assignments
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EtasaDesktop.Common.Tools;

    /// <summary>
    /// Lógica de interacción para AssignamentsView.xaml
    /// </summary>
    public partial class AssignmentsFrame : FrameControl
    {
        private AssignmentViewModel _viewModel;

        public AssignmentsFrame()
        {
            InitializeComponent();
            _viewModel = (AssignmentViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando asignaciones...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }

        private void AddAssignment_Click(object sender, RoutedEventArgs e)
        {
            ShowItemData(TxtDateFrame.Text.ToString(), 0);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var item = row.Item as AssignmentDataViewModel;
            ShowItemData(TxtDateFrame.Text.ToString(), item.Id);
        }

        private void ShowAssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedAssignment != null)
            {
                ShowItemData(TxtDateFrame.Text.ToString(),_viewModel.SelectedAssignment.Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Para ver el detalle de una asignación, primero selecciona un elemento de la lista",
                                         "Confirmation",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Warning);
            }
        }

        private void ShowItemData(string date, long id)
        {
            AssignmentsFormWindow assignmentWindow = new AssignmentsFormWindow(date, id);
            assignmentWindow.ShowDialog();

            if (assignmentWindow.DialogResult.HasValue && assignmentWindow.DialogResult.Value)
            {
                Refresh();
            }
        }

        private void ImportarButton_Click(object sender, RoutedEventArgs e)
        {
            AssignmentsImportView importAssignmentWindow = new AssignmentsImportView(_viewModel.SelectedDate, _viewModel.SelectedDate);
            importAssignmentWindow.ShowDialog();

            if (importAssignmentWindow.DialogResult.HasValue && importAssignmentWindow.DialogResult.Value)
            {
                Refresh();
            }
        }
    }
}