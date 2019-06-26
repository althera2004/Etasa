using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EtasaDesktop.Distribution.Assignments
{
    public class AssignmentsFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;
        public event Action FormDeleteFinished;
        public event Action<Exception> FormDeleteError;

        private AssignmentsDataSet.AssignmentsRow _row;


        public AssignmentsFormViewModel()
        {
            AssignmentsDataSet dataset = new AssignmentsDataSet();
            AssignmentsDataSetTableAdapters.AssignmentsTableAdapter adapter = new AssignmentsDataSetTableAdapters.AssignmentsTableAdapter();
            _row = dataset.Assignments.NewAssignmentsRow();

            _row.Date = DateTime.Today;
            _row.Enabled = true;
            _row.Repeat = false;
            _row.DriverId = 0;
            _row.CabId = 0;
            _row.TrailerId = 0;


            SaveCommand = new RelayCommand(Save, CanSave);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }

        public DateTime Date
        {
            get { return _row.Date; }
            set
            {
                if (_row.Date != value)
                {
                    _row.Date = value;
                    RaisePropertyChanged();
                }
            }
        }



        #region Driver input

        public string _driverCode;
        public string _driverName;
        public bool _driverNotFound;

        public int DriverId
        {
            get { return _row.DriverId; }
            set
            {
                if (_row.DriverId != value)
                {
                    _row.DriverId = value;
                    RaisePropertyChanged();
                    UpdateDriverInfo(value);
                }
            }
        }
        public string DriverCode
        {
            get { return _driverCode; }
            set
            {
                if (_driverCode != value)
                {
                    _driverCode = value;
                    RaisePropertyChanged();
                    UpdateDriverCode(value);
                }
            }
        }
        public string DriverName
        {
            get { return _driverName; }
            set
            {
                if (_driverName != value)
                {
                    _driverName = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool DriverNotFound
        {
            get { return _driverNotFound; }
            set
            {
                if (_driverNotFound != value)
                {
                    _driverNotFound = value;
                    RaisePropertyChanged();
                }
            }
        }


        private void UpdateDriverInfo(int id)
        {
            if (id > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.DriversInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.DriversInfoTableAdapter();
                AssignmentsDataSet.DriversInfoDataTable dataTable = adapter.GetDataBy(id);

                if(dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.DriversInfoRow row = (AssignmentsDataSet.DriversInfoRow) dataTable.Rows[0];

                    DriverCode = row.Code;
                    DriverName = row.Name;
                }
            }  
        }
        private void UpdateDriverCode(string code)
        {
            bool notFound = true;
            int id = 0;
            string name = null;

            if (!String.IsNullOrWhiteSpace(code))
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.DriversInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.DriversInfoTableAdapter();
                AssignmentsDataSet.DriversInfoDataTable dataTable = adapter.GetDataByCode(code);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.DriversInfoRow row = (AssignmentsDataSet.DriversInfoRow)dataTable.Rows[0];

                    id = row.Id;
                    name = row.Name;
                    notFound = false;
                }
            }

            DriverId = id;
            DriverName = name;
            DriverNotFound = notFound;
        }

        #endregion


        #region Cab input

        public string _cabCode;
        public string _cabLicensePlate;
        public bool _cabNotFound;

        public int CabId
        {
            get { return _row.CabId; }
            set
            {
                if (_row.CabId != value)
                {
                    _row.CabId = value;
                    RaisePropertyChanged();
                    UpdateCabInfo(value);
                }
            }
        }
        public string CabCode
        {
            get { return _cabCode; }
            set
            {
                if (_cabCode != value)
                {
                    _cabCode = value;
                    RaisePropertyChanged();
                    UpdateCabInfo(value);
                }
            }
        }
        public string CabLicensePlate
        {
            get { return _cabLicensePlate; }
            set
            {
                if (_cabLicensePlate != value)
                {
                    _cabLicensePlate = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool CabNotFound
        {
            get { return _cabNotFound; }
            set
            {
                if (_cabNotFound != value)
                {
                    _cabNotFound = value;
                    RaisePropertyChanged();
                }
            }
        }


        private void UpdateCabInfo(int id)
        {
            if (id > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.CabInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.CabInfoTableAdapter();
                AssignmentsDataSet.CabInfoDataTable dataTable = adapter.GetDataBy(id);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.CabInfoRow row = (AssignmentsDataSet.CabInfoRow)dataTable.Rows[0];

                    CabCode = row.Code;
                    CabLicensePlate = row.LicensePlate;
                }
            }
        }
        private void UpdateCabInfo(string code)
        {
            bool notFound = true;
            int id = 0;
            string lplate = null;

            if (!String.IsNullOrWhiteSpace(code))
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.CabInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.CabInfoTableAdapter();
                AssignmentsDataSet.CabInfoDataTable dataTable = adapter.GetDataByCode(code);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.CabInfoRow row = (AssignmentsDataSet.CabInfoRow)dataTable.Rows[0];

                    id = row.Id;
                    lplate = row.LicensePlate;
                    notFound = false;
                }
            }

            CabId = id;
            CabLicensePlate = lplate;
            CabNotFound = notFound;
        }

        #endregion


        #region Trailer input

        public string _trailerCode;
        public string _trailerLicensePlate;
        public int _trailerVolume;
        public bool _trailerNotFound;

        public int TrailerId
        {
            get { return _row.TrailerId; }
            set
            {
                if (_row.TrailerId != value)
                {
                    _row.TrailerId = value;
                    RaisePropertyChanged();
                    UpdateTrailerInfo(value);
                }
            }
        }
        public string TrailerCode
        {
            get { return _trailerCode; }
            set
            {
                if (_trailerCode != value)
                {
                    _trailerCode = value;
                    RaisePropertyChanged();
                    UpdateTrailerInfo(value);
                }
            }
        }
        public string TrailerLicensePlate
        {
            get { return _trailerLicensePlate; }
            set
            {
                if (_trailerLicensePlate != value)
                {
                    _trailerLicensePlate = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int TrailerVolume
        {
            get { return _trailerVolume; }
            set
            {
                if (_trailerVolume != value)
                {
                    _trailerVolume = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool TrailerNotFound
        {
            get { return _trailerNotFound; }
            set
            {
                if (_trailerNotFound != value)
                {
                    _trailerNotFound = value;
                    RaisePropertyChanged();
                }
            }
        }


        private void UpdateTrailerInfo(int id)
        {
            if (id > 0)
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter();
                AssignmentsDataSet.TrailerInfoDataTable dataTable = adapter.GetDataBy(id);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.TrailerInfoRow row = (AssignmentsDataSet.TrailerInfoRow) dataTable.Rows[0];

                    TrailerCode = row.Code;
                    TrailerLicensePlate = row.LicensePlate;
                    TrailerVolume = row.TankVolume;
                }
            }
        }
        private void UpdateTrailerInfo(string code)
        {
            bool notFound = true;
            int id = 0, volume = 0;
            string lplate = null;

            if (!String.IsNullOrWhiteSpace(code))
            {
                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter adapter = new AssignmentsDataSetTableAdapters.TrailerInfoTableAdapter();
                AssignmentsDataSet.TrailerInfoDataTable dataTable = adapter.GetDataByCode(code);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataSet.TrailerInfoRow row = (AssignmentsDataSet.TrailerInfoRow)dataTable.Rows[0];

                    id = row.Id;
                    lplate = row.LicensePlate;
                    volume = row.TankVolume;
                    notFound = false;
                }
            }

            TrailerId = id;
            TrailerLicensePlate = lplate;
            TrailerVolume = volume;
            TrailerNotFound = notFound;
        }

        #endregion



        public bool Enabled
        {
            get { return _row.Enabled; }
            set
            {
                if (_row.Enabled != value)
                {
                    _row.Enabled = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Observations
        {
            get { return _row.Observations; }
            set
            {
                if (_row.Observations != value)
                {
                    _row.Observations = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void Load(long id)
        {
            try
            {

                AssignmentsDataSet ds = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.AssignmentsTableAdapter adapter = new AssignmentsDataSetTableAdapters.AssignmentsTableAdapter();
                AssignmentsDataSet.AssignmentsDataTable dataTable = adapter.GetDataById(id);

                if (dataTable.Rows.Count > 0)
                {
                    _row = (AssignmentsDataSet.AssignmentsRow)dataTable.Rows[0];

                    RaisePropertyChanged(nameof(Date));
                    RaisePropertyChanged(nameof(Observations));
                    RaisePropertyChanged(nameof(Enabled));
                    UpdateDriverInfo(_row.DriverId);
                    UpdateCabInfo(_row.CabId);
                    UpdateTrailerInfo(_row.TrailerId);
                }


                FormLoadFinished?.Invoke();
            }
            catch (Exception e)
            {
                FormLoadError?.Invoke(e);
            }
        }

        public void Save()
        {
            try
            {
                AssignmentsDataSet dataset = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.AssignmentsTableAdapter adapter = new AssignmentsDataSetTableAdapters.AssignmentsTableAdapter();

                if (_row.DriverId > 0 && _row.CabId > 0 && _row.TrailerId > 0)
                {
                    if (_row.Id > 0)
                        adapter.UpdateAs(_row.Id,_row.DriverId, _row.CabId, _row.TrailerId, _row.Date, _row.Repeat, _row.Enabled, _row.Observations);
                    else
                        adapter.InsertAs(_row.DriverId, _row.CabId, _row.TrailerId, _row.Date, _row.Repeat, _row.Enabled, _row.Observations);

                    FormSaveFinished?.Invoke();
                }
                else
                {
                    FormRequiredEmpty?.Invoke();
                }
            }
            catch (Exception e)
            {
                FormSaveError?.Invoke(e);
            }
            
        }
        private bool CanSave()
        {
            return true;
        }

        public void Delete()
        {
            try
            {
                AssignmentsDataSet dataset = new AssignmentsDataSet();
                AssignmentsDataSetTableAdapters.AssignmentsTableAdapter adapter = new AssignmentsDataSetTableAdapters.AssignmentsTableAdapter();

                adapter.DeleteBy(_row.Id);

                FormDeleteFinished?.Invoke();
            }
            catch (Exception e)
            {
                FormDeleteError?.Invoke(e);
            }
        }
        private bool CanDelete()
        {
            return _row != null && _row.Id != 0;
        }
    }
}
