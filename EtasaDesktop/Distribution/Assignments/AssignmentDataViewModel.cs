using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;

namespace EtasaDesktop.Distribution.Assignments
{
    public class AssignmentDataViewModel : ViewModelBase
    {
        private AssignmentsDataSet.AssignmentSummariesRow _row;

        public AssignmentDataViewModel(AssignmentsDataSet.AssignmentSummariesRow row) => _row = row;

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

        public string CabLicensePlate
        {
            get { return _row.CabLicensePlate; }
            set { }
        }

        public string TrailerCode
        {
            get { return _row.TrailerCode; }
            set { }
        }

        public string TrailerLicensePlate
        {
            get { return _row.TrailerLicensePlate; }
            set { }
        }

        public bool Enabled
        {
            get { return _row.Enabled; }
            set { }
        }

        public string Observations
        {
            get { return _row.Observations; }
            set { }
        }
    }
}
