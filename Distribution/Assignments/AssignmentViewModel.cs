using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Assignments
{
    public class AssignmentViewModel : ViewModelBase
    {
        public DateTime _selectedDate;
        public AssignmentDataViewModel _selectedAssignment;


        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                Set(ref _selectedDate, value);
                Refresh();
            }
        }
        public AssignmentDataViewModel SelectedAssignment
        {
            get => _selectedAssignment;
            set
            {
                Set(ref _selectedAssignment, value);
            }
        }
        public ObservableCollection<AssignmentDataViewModel> Assignments { get; private set; }

        public AssignmentViewModel()
        {
            Assignments = new ObservableCollection<AssignmentDataViewModel>();

            SelectedDate = DateTime.Today;
        }

        public void Refresh()
        {
            Assignments.Clear();

            AssignmentsDataSet ds = new AssignmentsDataSet();
            AssignmentsDataSetTableAdapters.AssignmentSummariesAdapters adapt = new AssignmentsDataSetTableAdapters.AssignmentSummariesAdapters();
            adapt.FillBy(ds.AssignmentSummaries, SelectedDate);

            foreach (AssignmentsDataSet.AssignmentSummariesRow row in ds.AssignmentSummaries.Rows)
            {
                Assignments.Add(new AssignmentDataViewModel(row));
            }
        }


    }
}
