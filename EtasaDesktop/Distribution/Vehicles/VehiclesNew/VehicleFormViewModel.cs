using EtasaDesktop.Common;
using EtasaDesktop.Common.Tools;
using EtasaDesktop.Distribution.Orders.Imports;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Text;

namespace EtasaDesktop.Distribution.Vehicles.VehiclesNew
{
    public class VehicleFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        private bool _isSelected;
        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        //private ImportOperator _selectedOperator;

        private VehicleDataSet1.VehiclesRow _row;

        private VehicleDataSet1.Vehicles_ObsRow _row_obs;

        public bool FirstimeCode = true;

        public bool FirstTimeLicensePlate = true;

        public ObservableCollection<VehicleDataSet1.Vehicles_TypesRow> VehiclesType { get; private set; }

        public ObservableCollection<VehicleDataSet1.Vehicles_SizesRow> VehiclesSizes { get; private set; }

        public ObservableCollection<VehicleDataSet1.VehiclesRow> Vehicle { get; private set; }

        private VehicleDataSet1.Vehicles_TypesRow _selectedVehicleType;

        private VehicleDataSet1.Vehicles_SizesRow _selectedVehicleSize;

        public VehicleFormViewModel()
        {
            VehicleDataSet1 dataset = new VehicleDataSet1();
            VehicleDataSet1TableAdapters.VehiclesTableAdapter vehicle = new VehicleDataSet1TableAdapters.VehiclesTableAdapter();
            VehicleDataSet1TableAdapters.Vehicles_ObsTableAdapter vehicle_obs = new VehicleDataSet1TableAdapters.Vehicles_ObsTableAdapter();

            _row = dataset.Vehicles.NewVehiclesRow();
            _row_obs = dataset.Vehicles_Obs.NewVehicles_ObsRow();

            VehiclesType = new ObservableCollection<VehicleDataSet1.Vehicles_TypesRow>();
            LoadVehiclesTypes();

            VehiclesSizes = new ObservableCollection<VehicleDataSet1.Vehicles_SizesRow>();
            LoadVehiclesSizes();


            //vehiculo
            _row.Brand = 1;
            _row.LicensePlate = "";
            _row.Code = "";
            _row.VIN = "";
            _row.Type = 0;
            _row.Size = 0;
            _row.BrandModel = "";
            _row.StartNode = 1;
            _row.FinalNode = 1;
            _row.TankVolume = 0;
            _row.CreatedDate = DateTime.Now;
            _row.ModifiedDate = DateTime.Now;
            _row.StartDate = DateTime.Now;
            _row.FinalDate = DateTime.Now;
            _row.AxleCount = 0;
            _row.Weight = 0;
            _row.MaxWeight = 0;
            _row.LastLocationCreatedDate = DateTime.Now;
            _row.LastLocationLatitude = 0;
            _row.LastLocationLongitude = 0;
            _row.LastLocationTimeStamp = DateTime.Now;
            _row.Enabled = true;
            _row.TankVolume = 0;
            _row.Id = 0;

            //observaciones 
            _row_obs.Observations = "";
            SaveCommand = new RelayCommand(Save, CanSave);
        }


        public VehicleFormViewModel(VehicleDataSet1.VehiclesRow row)
        {
            _row = row;
        }


        public VehicleDataSet1.Vehicles_TypesRow SelectedVehicleType
        {
            get => _selectedVehicleType;
            set
            {
                Set(ref _selectedVehicleType, value);
                _row.Type = value.Id;

            }
        }

        public VehicleDataSet1.Vehicles_SizesRow SelectedVehicleSize
        {
            get => _selectedVehicleSize;
            set
            {
                Set(ref _selectedVehicleSize, value);
                _row.Size = value.Id;
            }
        }

        public void LoadVehiclesTypes()
        {
            VehiclesType.Clear();

            VehicleDataSet1 ds = new VehicleDataSet1();
            VehicleDataSet1TableAdapters.Vehicles_TypesTableAdapter adapt = new VehicleDataSet1TableAdapters.Vehicles_TypesTableAdapter();
            adapt.Fill(ds.Vehicles_Types);

            foreach (VehicleDataSet1.Vehicles_TypesRow row in ds.Vehicles_Types.Rows)
            {
                VehiclesType.Add(row);
            }
        }

        public void LoadVehiclesSizes()
        {
            VehiclesSizes.Clear();

            VehicleDataSet1 ds = new VehicleDataSet1();
            VehicleDataSet1TableAdapters.Vehicles_SizesTableAdapter adapt = new VehicleDataSet1TableAdapters.Vehicles_SizesTableAdapter();
            adapt.Fill(ds.Vehicles_Sizes);

            foreach (VehicleDataSet1.Vehicles_SizesRow row in ds.Vehicles_Sizes.Rows)
            {
                VehiclesSizes.Add(row);
            }
        }

        private bool CanSave()
        {
            return true;
        }

        public VehicleDataSet1.VehiclesRow DataRow
        {
            get { return _row; }
            set
            {
            }
        }

        public string LicensePlate
        {
            get { return _row.LicensePlate; }
            set
            {
                if (_row.LicensePlate != value)
                {
                    _row.LicensePlate = value;
                    NotifyPropertyChanged("LicensePlate");
                    RaisePropertyChanged();
                }

            }
        }

        public int TankVolume
        {
            get { return _row.TankVolume; }
            set
            {
                if (_row.TankVolume != value)
                {
                    _row.TankVolume = value;
                    RaisePropertyChanged();
                }

            }
        }

        public int VehicleType
        {
            get { return _row.Type; }
            set
            {
                if (_row.Type != value)
                {
                    _row.Type = value;
                    RaisePropertyChanged();
                }

            }
        }

        public int VehicleSize
        {
            get { return _row.Size; }
            set
            {
                if (_row.Size != value)
                {
                    _row.Size = Convert.ToInt32(value);
                    RaisePropertyChanged();
                }

            }
        }

        public string VIN
        {
            get { return _row.VIN; }
            set
            {
                if (_row.VIN != value)
                {
                    _row.VIN = value;
                    RaisePropertyChanged();
                }

            }
        }

        public int Brand
        {
            get { return _row.Brand; }
            set
            {
                if (_row.Brand != value)
                {
                    _row.Brand = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string BrandModel
        {
            get { return _row.BrandModel; }
            set
            {
                if (_row.BrandModel != value)
                {
                    _row.BrandModel = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int AxleCount
        {
            get { return _row.AxleCount; }
            set
            {
                if (_row.AxleCount != value)
                {
                    _row.AxleCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int Weight
        {
            get { return _row.Weight; }
            set
            {
                if (_row.Weight != value)
                {
                    _row.Weight = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Code
        {
            get { return _row.Code; }
            set
            {
                if (_row.Code != value)
                {
                    _row.Code = value;
                    NotifyPropertyChanged("Code");
                    RaisePropertyChanged();
                }
            }
        }

        public int PMA
        {
            get { return _row.MaxWeight; }
            set
            {
                if (_row.MaxWeight != value)
                {
                    _row.MaxWeight = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get { return _row.StartDate; }
            set
            {
                if (_row.StartDate != value)
                {
                    _row.StartDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime FinalDate
        {
            get { return _row.FinalDate; }
            set
            {
                if (_row.FinalDate != value)
                {
                    _row.FinalDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float LastLocationLatitude
        {
            get { return _row.LastLocationLatitude; }
            set
            {
                if (_row.LastLocationLatitude != value)
                {
                    _row.LastLocationLatitude = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float LastLocationLongitude
        {
            get { return _row.LastLocationLongitude; }
            set
            {
                if (_row.LastLocationLongitude != value)
                {
                    _row.LastLocationLongitude = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime LastLocationTimeStamp
        {
            get { return _row.LastLocationTimeStamp; }
            set
            {
                if (_row.LastLocationTimeStamp != value)
                {
                    _row.LastLocationTimeStamp = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Observations
        {
            get { return _row_obs.Observations; }
            set
            {
                if (_row_obs.Observations != value)
                {
                    _row_obs.Observations = value;
                    RaisePropertyChanged();
                }
            }
        }

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

        public void Load(int IdVehicle)
        {
            try
            {
                VehicleDataSet1 dataset = new VehicleDataSet1();
                VehicleDataSet1TableAdapters.VehiclesTableAdapter TableVehicle = new VehicleDataSet1TableAdapters.VehiclesTableAdapter();
                VehicleDataSet1.VehiclesDataTable dataTable = TableVehicle.GetVehicleById(IdVehicle);
         
                if (dataTable.Rows.Count > 0)
                {
                    VehicleDataSet1TableAdapters.Vehicles_ObsTableAdapter Vehicle_obs = new VehicleDataSet1TableAdapters.Vehicles_ObsTableAdapter();
                    VehicleDataSet1.Vehicles_ObsDataTable dataTable_Obs = Vehicle_obs.GetVehiclestObs(IdVehicle);
              
                    _row = (VehicleDataSet1.VehiclesRow)dataTable.Rows[0];
                    RaisePropertyChanged(nameof(LicensePlate));
                    RaisePropertyChanged(nameof(Brand));
                    RaisePropertyChanged(nameof(BrandModel));
                    RaisePropertyChanged(nameof(AxleCount));
                    RaisePropertyChanged(nameof(StartDate));
                    RaisePropertyChanged(nameof(FinalDate));
                    RaisePropertyChanged(nameof(Weight));
                    RaisePropertyChanged(nameof(Code));
                    RaisePropertyChanged(nameof(PMA));
                    RaisePropertyChanged(nameof(VIN));
                    RaisePropertyChanged(nameof(VehicleType));
                    RaisePropertyChanged(nameof(VehicleSize));
                    RaisePropertyChanged(nameof(LastLocationLatitude));
                    RaisePropertyChanged(nameof(LastLocationLongitude));
                    RaisePropertyChanged(nameof(LastLocationTimeStamp));
                    RaisePropertyChanged(nameof(TankVolume));
                    RaisePropertyChanged(nameof(Observations));  
                    RaisePropertyChanged(nameof(Enabled));

                    // Juan Castilla - Comprobar que hay observacionea antes de leerlas
                    if (dataTable_Obs.Rows.Count > 0)
                    {
                        _row_obs = (VehicleDataSet1.Vehicles_ObsRow)dataTable_Obs.Rows[0];
                    }

                    RaisePropertyChanged(nameof(Observations));

                    SelectedVehicleType = InitializeComboBoxVehiclesType(VehicleType);
                    SelectedVehicleSize = InitializeComboBoxVehiclesSizes(VehicleSize);
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
            int indentificadorVehicle = 0;
            bool validate = true;
            try
            {
                VehicleDataSet1 dataset = new VehicleDataSet1();
                VehicleDataSet1TableAdapters.VehiclesTableAdapter TableVehicles = new VehicleDataSet1TableAdapters.VehiclesTableAdapter();
                validate = validateDatas(_row.Code,_row.LicensePlate,_row.TankVolume, dataset, TableVehicles,this.FirstimeCode,this.FirstTimeLicensePlate);

                if (validate)
                {
                    
                    VehicleDataSet1TableAdapters.Vehicles_ObsTableAdapter tableAdapterVehiclesObs = new VehicleDataSet1TableAdapters.Vehicles_ObsTableAdapter();         
                    VehicleDataSet1.VehiclesDataTable dataTable = TableVehicles.GetVehicleById(_row.Id);

                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowVehicle = dataset.Vehicles.NewVehiclesRow();
                        rowVehicle = dataTable[0];
                        rowVehicle["LicensePlate"] = _row.LicensePlate;
                        rowVehicle["Brand"] = _row.Brand;               
                        rowVehicle["BrandModel"] = _row.BrandModel;
                        rowVehicle["CreatedDate"] = DateTime.Now;
                        rowVehicle["ModifiedDate"] = DateTime.Now;
                        rowVehicle["StartDate"] = DateTime.Now;
                        rowVehicle["FinalDate"] = DateTime.Now;
                        rowVehicle["VIN"] = _row.VIN;
                        rowVehicle["AxleCount"] = _row.AxleCount;
                        rowVehicle["Weight"] = _row.Weight;
                        rowVehicle["MaxWeight"] = _row.MaxWeight;
                        rowVehicle["Type"] = _row.Type;
                        rowVehicle["Size"] = _row.Size;
                        rowVehicle["LastLocationLatitude"] = _row.LastLocationLatitude;
                        rowVehicle["LastLocationLongitude"] = _row.LastLocationLongitude;
                        rowVehicle["LastLocationTimeStamp"] = _row.LastLocationTimeStamp;
                        rowVehicle["TankVolume"] = _row.TankVolume;
                        rowVehicle["Enabled"] = _row.Enabled;

                        int numeroderegistros = TableVehicles.Update(rowVehicle);

                        //si se actualizado el vehicle actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            tableAdapterVehiclesObs.UpdateVehicleObs(_row_obs.Observations, _row.Id);
                        }
                    }
                    //insertamos el nuevo vehiculo  y las observaciones (vehicle_obs)
                    else
                    {
                        TableVehicles = new VehicleDataSet1TableAdapters.VehiclesTableAdapter();
                        //obtenemos de la tabla vehicle el nuevo registro que se introducira 
                        var rowVehicle = dataset.Vehicles.NewVehiclesRow();
                        //codigo
                        rowVehicle.Code = _row.Code;
                        //LicensePlate
                        rowVehicle.LicensePlate = _row.LicensePlate;
                        //Brand
                        rowVehicle.Brand = _row.Brand;
                        //Brand_model
                        rowVehicle.BrandModel = _row.BrandModel;
                        //size
                        rowVehicle.Size = _row.Size;
                        //tankvolume
                        rowVehicle.TankVolume = _row.TankVolume;
                        //fecha creacion
                        rowVehicle.CreatedDate = DateTime.Now;
                        //fecha modificacion
                        rowVehicle.ModifiedDate = DateTime.Now;
                        //fecha inicio
                        rowVehicle.StartDate = _row.StartDate;
                        //fecha Final
                        rowVehicle.FinalDate = _row.FinalDate;
                        //fecha tipo
                        rowVehicle.Type = _row.Type;
                        //VIN
                        rowVehicle.VIN = _row.VIN;
                        //AxleCount
                        rowVehicle.AxleCount = _row.AxleCount;
                        //Weight
                        rowVehicle.Weight = _row.Weight;
                        //MaxWeight
                        rowVehicle.MaxWeight = _row.MaxWeight;
                        //Last Location Latitude
                        rowVehicle.LastLocationLatitude = _row.LastLocationLatitude;
                        //Last Location Longitude
                        rowVehicle.LastLocationLongitude = _row.LastLocationLongitude;
                        //Last Location TimeStamp 
                        rowVehicle.LastLocationTimeStamp = _row.LastLocationTimeStamp;
                        //StartNode
                        rowVehicle.StartNode = _row.StartNode;
                        //Finalsnode 
                        rowVehicle.FinalNode = _row.FinalNode;
                        //TankVolume
                        rowVehicle.TankVolume = _row.TankVolume;
                        //enable
                        rowVehicle.Enabled = _row.Enabled;


                        //agregamos la nueva fila 
                        dataset.Vehicles.AddVehiclesRow(rowVehicle);
                        //actualizamos la tabla vehicle en el dataset 
                        TableVehicles.Update(dataset.Vehicles);

                        //al insertar el registro nuevo en la tabla vehicle el Id se actualiza en el objeto
                        indentificadorVehicle = Convert.ToInt32(rowVehicle.Id);

                        // a continuación insertamos el resto de datos en la tabla vehicle_obs  con el indentificador del cliente reciente introduzido
                        tableAdapterVehiclesObs = new VehicleDataSet1TableAdapters.Vehicles_ObsTableAdapter();
                        //obtenemos de la tabla vehicle_obs el nuevo registro que se introducira 
                        var rowVehicle_obs = dataset.Vehicles_Obs.NewVehicles_ObsRow();

                        //Id
                        rowVehicle_obs.Id = indentificadorVehicle;
                        //observations
                        rowVehicle_obs.Observations = _row_obs.Observations;

                        //agregamos la nueva fila a tabla vehice_obs
                        dataset.Vehicles_Obs.AddVehicles_ObsRow(rowVehicle_obs);
                        //actualizamos la tabla vehicle_obs  en el dataset 
                        tableAdapterVehiclesObs.Update(dataset.Vehicles_Obs);
                    }
                    FormSaveFinished?.Invoke();
                }
            }
            catch (Exception e)
            {

                FormSaveError?.Invoke(e);
            }

        }
        
        public void Refresh()
        {

            Vehicle.Clear();

            VehicleDataSet1 ds = new VehicleDataSet1();
            VehicleDataSet1TableAdapters.VehiclesTableAdapter adapt = new VehicleDataSet1TableAdapters.VehiclesTableAdapter();
            adapt.Fill(ds.Vehicles);

            foreach (VehicleDataSet1.VehiclesRow row in ds.Vehicles.Rows)
            {
                Vehicle.Add(row);
            }

        }

        public void NotifyPropertyChanged(string property)
        {
            if (property == "Code")
            {
                this.FirstimeCode = false;
            }
            else
            {
                if (property == "LicensePlate")
                {
                    this.FirstTimeLicensePlate = false;
                }
            }

        }

        private static bool validateDatas(string Code,string LicensePlate, int tankVolume, VehicleDataSet1 dataset, VehicleDataSet1TableAdapters.VehiclesTableAdapter TableVehicles, bool FirstimeCode,bool FirstTimeLicensePlate)
        {
            bool validateDatas = true;
            int NumberRows = 0;
            if (string.IsNullOrEmpty(Code))
            {
                MessageBox.Show("El Código no puede estar en blanco", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                validateDatas = false;
            }
            else
            {
                if (string.IsNullOrEmpty(LicensePlate))
                {
                    MessageBox.Show("La matrícula no puede estar en blanco", "Campo Matrícula", MessageBoxButton.OK, MessageBoxImage.Error);
                    validateDatas = false;
                }
                else
                {
                    if (LicensePlate.Length >13)
                    {
                        MessageBox.Show("La matrícula no puede tener más 13 caracteres", "Campo Matrícula", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {
                        if (Code.Length > 4)
                        {
                            MessageBox.Show("El código no puede tener más de 4 caracteres", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                            validateDatas = false;
                        }
                        else
                        {
                            if(string.IsNullOrEmpty(Convert.ToString(tankVolume)))
                            {
                                MessageBox.Show("El Volumen de tanque no puedo esta vacio", "Campo volumne tanque", MessageBoxButton.OK, MessageBoxImage.Error);
                                validateDatas = false;

                            }
                            else
                            {
                                if(!IsNumeric(Convert.ToString(tankVolume).Trim()))
                                {
                                    MessageBox.Show("El Volumen de tanque tiene que conter un valor numérico", "Campo volumne tanque", MessageBoxButton.OK, MessageBoxImage.Error);
                                    validateDatas = false;
                                }
                                else
                                {
                                    //comprobamos que el código no existe
                                    if (!FirstimeCode)
                                    {
                                        NumberRows = Convert.ToInt32(TableVehicles.ScalarQueryCode(Code.Trim()));
                                        if (NumberRows > 0)
                                        {
                                            MessageBox.Show("El Código introducido ya existe", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                                            validateDatas = false;
                                        }
                                    }
                                    else
                                    {
                                        //comprobamos que la matrícula no existe
                                        if (!FirstTimeLicensePlate)
                                        {
                                            NumberRows = Convert.ToInt32(TableVehicles.ScalarQueryByLicensePlate(LicensePlate.Trim()));
                                            if (NumberRows > 0)
                                            {
                                                MessageBox.Show("La matrícula introducida ya existe", "Campo Matrícula", MessageBoxButton.OK, MessageBoxImage.Error);
                                                validateDatas = false;
                                            }
                                        }
                                    }
                                    NumberRows = 0;
                                }
                            }

                        }
                    }
                }
            }
            return validateDatas;
        }

        public static bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }


        private VehicleDataSet1.Vehicles_TypesRow InitializeComboBoxVehiclesType(int id)
        {
            return VehiclesType.SingleOrDefault(c => c.Id == id);
        }

        private VehicleDataSet1.Vehicles_SizesRow InitializeComboBoxVehiclesSizes(int id)
        {
            return VehiclesSizes.SingleOrDefault(c => c.Id == id);
        }
    }
}