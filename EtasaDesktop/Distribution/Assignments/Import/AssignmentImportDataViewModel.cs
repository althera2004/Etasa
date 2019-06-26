using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;

namespace EtasaDesktop.Distribution.Assignments
{
    public class AssignmentImportDataViewModel : ViewModelBase
    {
        private AssignmentsDataSet.AssignmentSummariesRow _row;
        private bool _isSelected;

        public AssignmentImportDataViewModel(AssignmentsDataSet.AssignmentSummariesRow row)
        {
            _row = row;
            _isSelected = false;
        }

        public AssignmentsDataSet.AssignmentSummariesRow DataRow
        {
            get { return _row; }
            set { }
        }

        public long Id
        {
            get { return _row.Id; }
            set { }
        }

        public string DriverCode
        {
            get { return _row.DriverCode; }
            set { }
        }

        public string DriverName
        {
            get { return _row.DriverName; }
            set { }
        }

        public string CabCode
        {
            get { return _row.CabCode; }
            set { }
        }

        public string TrailerCode
        {
            get { return _row.TrailerCode; }
            set { }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
