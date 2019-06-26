using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    public partial class ImportCalendarControl : UserControl, INotifyPropertyChanged
    {
        public event RoutedEventHandler Accept;
        public event RoutedEventHandler Cancel;


        private DateTime _selectedDate;
        private int _expirationDays;

        private bool skipCalendarChange;
        private bool skipExpirationChange;


        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                NotifyPropertyChanged("SelectedDate");
            }
        }
        public int ExpirationDays
        {
            get => _expirationDays;
            set
            {
                _expirationDays = value;
                NotifyPropertyChanged("ExpirationDays");
            }
        }


        public ImportCalendarControl()
        {
            InitializeComponent();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!skipCalendarChange)
            {
                skipExpirationChange = true;

                var selectedDates = Calendar.SelectedDates.ToArray();
                Array.Sort(selectedDates);
                SelectedDate = selectedDates[0];

                var expirationDays = selectedDates.Length - 1;

                if(expirationDays > 0)
                {
                    ExpirationDays = expirationDays;
                    ExpirationDaysBox.Text = ExpirationDays.ToString();
                }
                else
                {
                    skipCalendarChange = true;
                    Calendar.SelectedDates.Clear();
                    Calendar.SelectedDates.AddRange(SelectedDate, SelectedDate.AddDays(ExpirationDays));
                    skipCalendarChange = false;
                }

                skipExpirationChange = false;
            }
        }

        private void ExpirationDaysBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!skipExpirationChange)
            {
                skipCalendarChange = true;

                if (!Int32.TryParse(ExpirationDaysBox.Text, out _expirationDays))
                    ExpirationDays = 0;

                Calendar.SelectedDates.Clear();
                Calendar.SelectedDates.AddRange(SelectedDate, SelectedDate.AddDays(Math.Abs(ExpirationDays)));
                Calendar.DisplayDate = SelectedDate;

                skipCalendarChange = false;
            }
        }

        private void Calendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e) => Accept?.Invoke(this, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => Cancel?.Invoke(this, e);


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
