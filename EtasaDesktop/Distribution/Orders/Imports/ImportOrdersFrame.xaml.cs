using System;
using System.Windows;
using EtasaDesktop.Common.Tools;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    public partial class ImportOrdersFrame : FrameControl
    {

        private ImportOrdersViewModel _viewModel;

        public ImportOrdersFrame()
        {
            InitializeComponent();
            _viewModel = DataContext as ImportOrdersViewModel;
        }


        private void Frame_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Activated += Window_Activated;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            _viewModel.RefreshFileList();
        }





        private void ImportCalendar_Opened(object sender, EventArgs e)
        {
            ImportCalendar.Calendar.SelectedDate = _viewModel.Date;
            ImportCalendar.Calendar.DisplayDate = _viewModel.Date;
            ImportCalendar.ExpirationDaysBox.Text = _viewModel.ExpirationDays.ToString();
        }

        private void ImportCalendar_AcceptClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ExpirationDays = ImportCalendar.ExpirationDays;
            _viewModel.Date = ImportCalendar.SelectedDate;

            ChangeDateButton.IsChecked = false;
        }

        private void ImportCalendar_CancelClick(object sender, RoutedEventArgs e)
        {
            ChangeDateButton.IsChecked = false;
        }
    }
}