using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EtasaDesktop.Distribution.Assignments
{
    public partial class AssignmentsImportView : Window
    {
        private AssignmentsImportViewModel _viewModel;

        public AssignmentsImportView(DateTime dateFrom, DateTime dateTo)
        {
            InitializeComponent();
            _viewModel = (AssignmentsImportViewModel) DataContext;
            _viewModel.SelectedDate = dateFrom;
            _viewModel.ImportToDate = dateTo;
            _viewModel.ImportStarted += (s, e) => OnImportStarted();
            _viewModel.ImportFailed += (s, e) => OnImportFailed();
            _viewModel.ImportFinished += (s, e) => OnFinishImport();
        }

        private void OnImportStarted()
        {
            Mouse.OverrideCursor = Cursors.Wait;
        }

        private void OnImportFailed()
        {
            Mouse.OverrideCursor = null;
            MessageBox.Show("Ha ocurrido un error al importar","Error",MessageBoxButton.OK,MessageBoxImage.Error);
        }

        private void OnFinishImport()
        {
            DialogResult = true;
            Mouse.OverrideCursor = null;
            MessageBox.Show("Importación completada", "Importación", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

    }
}