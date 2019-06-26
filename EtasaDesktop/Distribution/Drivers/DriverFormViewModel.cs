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
using EtasaDesktop.Distribution.Clients;
using System.Security.Cryptography;
using System.Text;

namespace EtasaDesktop.Distribution.Drivers
{
    public class DriverFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        private DriversDataSet.DriversRow _row;

        public bool firsTimeCode = true;
         
        public bool firsTimeDni = true;

        private DriversDataSet.Drivers_ObsRow _row_obs;

        private DriversDataSet.DriversRow _selectedDriver;

        private DriversDataSet.System_Data_CountriesRow _selectedCountry;

        private DriversDataSet.System__Data_ProvincesRow _selectedProvince;

        public ObservableCollection<DriversDataSet.System_Data_CountriesRow> Countries { get; private set; }

        public ObservableCollection<DriversDataSet.System__Data_ProvincesRow> Provinces { get; private set; }




        public ObservableCollection<DriverFormViewModel> Driver { get; private set; }

        public DriverFormViewModel()
        {
            DriversDataSet dataset = new DriversDataSet();
            DriversDataSetTableAdapters.DriversTableAdapter Driver = new DriversDataSetTableAdapters.DriversTableAdapter();
            DriversDataSetTableAdapters.Drivers_ObsTableAdapter Drive_obs = new DriversDataSetTableAdapters.Drivers_ObsTableAdapter();

            _row = dataset.Drivers.NewDriversRow();
            _row_obs = dataset.Drivers_Obs.NewDrivers_ObsRow();

            Countries = new ObservableCollection<DriversDataSet.System_Data_CountriesRow>();
            LoadCountries();
            Provinces = new ObservableCollection<DriversDataSet.System__Data_ProvincesRow>();
            LoadProvinces();

            //Conductor
            _row.Code = "";
            _row.Name= "";    
            _row.Dni = "";
            _row.Address = "";
            _row.City = "";
            _row.ModifiedDate = DateTime.Now;
            _row.CreatedDate = DateTime.Now;
            _row.Phone = "";
            _row.StartDate = DateTime.Now;
            _row.FinishDate = DateTime.Now;
            _row.Enabled = true;
            _row.Id = 0;

            //observaciones conductor
            _row_obs.Observations = "";

            SaveCommand = new RelayCommand(Save, CanSave);
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


        public string Name
        {
            get { return _row.Name; }
            set
            {
                if (_row.Name != value)
                {
                    _row.Name = value;
                    RaisePropertyChanged();
                }
            }
        }



        public string Dni
        {
            get { return _row.Dni; }
            set
            {
                if (_row.Dni != value)
                {
                    _row.Dni = value;
                    NotifyPropertyChanged("Dni");
                    RaisePropertyChanged();
                }
            }
        }


        public string Address
        {
            get { return _row.Address; }
            set
            {
                if (_row.Address != value)
                {
                    _row.Address = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string City
        {
            get { return _row.City; }
            set
            {
                if (_row.City != value)
                {
                    _row.City = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string Phone
        {
            get { return _row.Phone; }
            set
            {
                if (_row.Phone != value)
                {
                    _row.Phone = value;
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


        public DateTime FinishDate
        {
            get { return _row.FinishDate; }
            set
            {
                if (_row.FinishDate != value)
                {
                    _row.FinishDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Province
        {
            get { return _row.Province; }
            set
            {
                if (_row.Province != value)
                {
                    _row.Province = value.ToString().Trim();
                    RaisePropertyChanged();
                }
            }
        }

        public string Country
        {
            get { return _row.Country; }
            set
            {
                if (_row.Country != value)
                {
                    _row.Country = value;
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

        public DriversDataSet.System_Data_CountriesRow SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                Set(ref _selectedCountry, value);
                _row.Country = value.Iso.ToString().Trim();
            }
        }

        public DriversDataSet.System__Data_ProvincesRow SelectedProvince
        {
            get => _selectedProvince;
            set
            {
                Set(ref _selectedProvince, value);
                _row.Province = value.Code.ToString().Trim();
            }
        }

        public void LoadCountries()
        {
            Countries.Clear();
            DataTable CountriesSpain = new DataTable();
            DriversDataSet ds = new DriversDataSet();
            DriversDataSetTableAdapters.System_Data_CountriesTableAdapter adapt = new DriversDataSetTableAdapters.System_Data_CountriesTableAdapter();
            CountriesSpain = adapt.GetDataOnlySpainByID(199);

            foreach (DriversDataSet.System_Data_CountriesRow row in CountriesSpain.Rows)
            {
                Countries.Add(row);
            }
        }

        public void LoadProvinces()
        {
            Provinces.Clear();

            DriversDataSet ds = new DriversDataSet();
            DriversDataSetTableAdapters.System__Data_ProvincesTableAdapter adapt = new DriversDataSetTableAdapters.System__Data_ProvincesTableAdapter();
            adapt.Fill(ds.System__Data_Provinces);

            foreach (DriversDataSet.System__Data_ProvincesRow row in ds.System__Data_Provinces.Rows)
            {
                Provinces.Add(row);
            }
        }

        private bool CanSave()
        {
            return true;
        }

        public void Load(int IdDriver)
        {
            try
            {
                
                DriversDataSet dataset = new DriversDataSet();
                DriversDataSetTableAdapters.DriversTableAdapter TableDriver = new DriversDataSetTableAdapters.DriversTableAdapter();
                DriversDataSet.DriversDataTable dataTable = TableDriver.GetDataDriverById(IdDriver);


                if (dataTable.Rows.Count > 0)
                {
                    DriversDataSetTableAdapters.Drivers_ObsTableAdapter Driver_obs = new DriversDataSetTableAdapters.Drivers_ObsTableAdapter();
                    DriversDataSet.Drivers_ObsDataTable dataTable_Obs = Driver_obs.GetDataDriver_ObsById(IdDriver);

                    _row = (DriversDataSet.DriversRow)dataTable.Rows[0];

                    RaisePropertyChanged(nameof(Code));
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Dni));
                    RaisePropertyChanged(nameof(Address));
                    RaisePropertyChanged(nameof(City));         
                    RaisePropertyChanged(nameof(Phone));
                    RaisePropertyChanged(nameof(Province));
                    RaisePropertyChanged(nameof(Country));

                    RaisePropertyChanged(nameof(StartDate));
                    RaisePropertyChanged(nameof(FinishDate));
                    RaisePropertyChanged(nameof(Enabled));

                    SelectedCountry = InitializeComboBoxCountry(Country);

                    SelectedProvince = InitializeComboBoxProvince(Province);

                    _row_obs = (DriversDataSet.Drivers_ObsRow)dataTable_Obs.Rows[0];
                    RaisePropertyChanged(nameof(Observations));
                    
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
            bool validate = true;
            int indentificadordriver = 0;
            try
            {
                DriversDataSet dataset = new DriversDataSet();
                DriversDataSetTableAdapters.DriversTableAdapter TableDriver = new DriversDataSetTableAdapters.DriversTableAdapter();

     
                validate = validateDatas(_row.Code, _row.Dni, _row.Name, _row.Address, _row.City, _row.Phone,this.firsTimeDni,this.firsTimeCode, dataset, TableDriver,_row.StartDate.ToString(),_row.FinishDate.ToString(),_row.Country,_row.Province);

                if (validate)
                {                
                    DriversDataSetTableAdapters.Drivers_ObsTableAdapter tableAdapterDriverObs = new DriversDataSetTableAdapters.Drivers_ObsTableAdapter();         
                    DriversDataSet.DriversDataTable dataTable = TableDriver.GetDataDriverById(_row.Id);


                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowDriver = dataset.Drivers.NewDriversRow();
                        rowDriver = dataTable[0];
                        rowDriver["Code"] = _row.Code;
                        rowDriver["Name"] = _row.Name;
                        rowDriver["Dni"] = _row.Dni;
                        rowDriver["Address"] = _row.Address;
                        rowDriver["City"] = _row.City;
                        rowDriver["CreatedDate"] = DateTime.Now;
                        rowDriver["ModifiedDate"] = DateTime.Now;
                        rowDriver["Phone"] = _row.Phone;
                        rowDriver["StartDate"] = _row.StartDate;
                        rowDriver["FinishDate"] = _row.FinishDate;
                        rowDriver["Province"] = _row.Province;
                        rowDriver["Country"] = _row.Country;
                        rowDriver["Enabled"] = _row.Enabled;

                        int numeroderegistros = TableDriver.Update(rowDriver);

                        //si se actualizado el user actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            tableAdapterDriverObs.UpdateDriverObs(_row_obs.Observations, _row.Id);
                        }
                    }
                    //insertamos el nuevo user  y las observaciones (user_obs)
                    else
                    {
                        TableDriver = new DriversDataSetTableAdapters.DriversTableAdapter();
                        //obtenemos de la tabla Driver  el nuevo registro que se introducira 
                        var rowDriver = dataset.Drivers.NewDriversRow();

                        //Code
                        rowDriver.Code = _row.Code;
                        //Name
                        rowDriver.Name = _row.Name;
                        //Dni
                        rowDriver.Dni = _row.Dni;
                        //Address
                        rowDriver.Address = _row.Address;
                        //City
                        rowDriver.City = _row.City;
                        //fecha creacion
                        rowDriver.CreatedDate = DateTime.Now;
                        //fecha modificacion
                        rowDriver.ModifiedDate = DateTime.Now;
                        //fecha de cumpleaños
                        rowDriver.BirthDate = DateTime.Now;
                        //province                  
                        rowDriver.Province = _row.Province;
                        //country                   
                        rowDriver.Country = _row.Country;
                        //Phone
                        rowDriver.Phone = _row.Phone;
                        //start date
                        rowDriver.StartDate = _row.StartDate;
                        //finish date        
                        rowDriver.FinishDate = _row.FinishDate;
                        //enabled
                        rowDriver.Enabled = _row.Enabled;


                        //agregamos la nueva fila 
                        dataset.Drivers.AddDriversRow(rowDriver);
                        //actualizamos la tabla Driver en el dataset 
                        TableDriver.Update(dataset.Drivers);

                        //al insertar el registro nuevo en la tabla Driver Id se actualiza 
                        indentificadordriver = Convert.ToInt32(rowDriver.Id);


                        // a continuación insertamos el resto de datos en la tabla Driver_obs cepsa con el indentificador del Driver recient introduzido
                        tableAdapterDriverObs = new DriversDataSetTableAdapters.Drivers_ObsTableAdapter();
                        //obtenemos de la tabla Driver_obs el nuevo registro que se introducira 
                        var rowDriver_obs = dataset.Drivers_Obs.NewDrivers_ObsRow();

                        //Id
                        rowDriver_obs.Id = indentificadordriver;
                        //observations
                        rowDriver_obs.Observations = _row_obs.Observations;

                        //agregamos la nueva fila a tabla drivers_obs
                        dataset.Drivers_Obs.AddDrivers_ObsRow(rowDriver_obs);
                        //actualizamos la tabla Drivers_obs  en el dataset 
                        tableAdapterDriverObs.Update(dataset.Drivers_Obs);
                    }

                    FormSaveFinished?.Invoke();
                }
            }
            catch (Exception e)
            {

                FormSaveError?.Invoke(e);
            }

        }

        public void NotifyPropertyChanged(string property)
        {
            if (property == "Code")
            {
                this.firsTimeCode = false;
            }
            else
            {
                if (property == "Dni")
                {
                    this.firsTimeDni = false;
                }
            }
    
        }

        private static bool validateDatas(string Code, string Dni,string Name,string adress,string city,string phone, bool FirstimeDni,bool FirstTimeCode, DriversDataSet dataset, DriversDataSetTableAdapters.DriversTableAdapter tableDriver,string StartDate , string FinalDate,string country, string province)
        {
            bool validateDatas = true;
            int NumberRows = 0;
            DateTime StartDateValue;
            DateTime FinalDateValue;
            if (string.IsNullOrEmpty(Code) || Code.ToString().Length > 8)
            {
                if(string.IsNullOrEmpty(Code))
                {
                    MessageBox.Show("El campo Código no puede estar en blanco", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                    validateDatas = false;
                }
                else
                {
                    if(Code.ToString().Length > 8)
                    {
                        MessageBox.Show("El campo Código no puede tener mas de 8 caracteres", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                }              
            }
            else
            {
                if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(province))
                {
                    if(string.IsNullOrEmpty(country))
                    {
                        MessageBox.Show("El campo pais no puede estar en blanco", "Campo Pais", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {
                        MessageBox.Show("El campo provincia no puede estar en blanco","campo provincia", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }             
                }
                else
                {

               
                    if (string.IsNullOrEmpty(StartDate))
                    {
                        MessageBox.Show("La fecha de inicio no puede esta en blanco", "Campo Fecha inicio", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {
                        if (!DateTime.TryParse(StartDate, out StartDateValue))
                        {
                            MessageBox.Show("La fecha de inicio no es valida", "Campo Fecha inicio", MessageBoxButton.OK, MessageBoxImage.Error);
                            validateDatas = false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(StartDate))
                            {
                                MessageBox.Show("La fecha de inicio no puede esta en blanco", "Campo Fecha inicio", MessageBoxButton.OK, MessageBoxImage.Error);
                                validateDatas = false;
                            }
                            else
                            {
                                if (!DateTime.TryParse(FinalDate, out FinalDateValue))
                                {
                                    MessageBox.Show("La fecha Final no es valida", "Campo Fecha final ", MessageBoxButton.OK, MessageBoxImage.Error);
                                    validateDatas = false;
                                }
                                else
                                {
                                    if (StartDateValue > FinalDateValue)
                                    {
                                        MessageBox.Show("La fecha inicio no puede ser superior a la fecha final", "Campo Fecha Inicio", MessageBoxButton.OK, MessageBoxImage.Error);
                                        validateDatas = false;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(Name))
                                        {
                                            MessageBox.Show("El campo Nombre no puede estar en blanco", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                                            validateDatas = false;
                                        }
                                        else
                                        {
                                            if (Name.Length > 100)
                                            {
                                                MessageBox.Show("El campo Nombre no puede contener mas de 100 caracteres", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                                                validateDatas = false;
                                            }
                                            else
                                            {
                                                if (Code.Length > 8)
                                                {
                                                    MessageBox.Show("El campo Código no puede tener más de 8 caracteres", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                                                    validateDatas = false;
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(Dni))
                                                    {
                                                        MessageBox.Show("El Dni no puede estar en blanco", "Campo Dni", MessageBoxButton.OK, MessageBoxImage.Error);
                                                        validateDatas = false;
                                                    }
                                                    else
                                                    {
                                                        if (Dni.Length > 15)
                                                        {
                                                            MessageBox.Show("El Dni no tener una longitud superior a 15", "Campo Dni", MessageBoxButton.OK, MessageBoxImage.Error);
                                                            validateDatas = false;
                                                        }
                                                        else
                                                        {
                                                            if (adress.Length > 100)
                                                            {
                                                                MessageBox.Show("La dirección no puede tener una longitud superior a 100", "Campo Dirección", MessageBoxButton.OK, MessageBoxImage.Error);
                                                                validateDatas = false;
                                                            }
                                                            else
                                                            {
                                                                if (city.Length > 50)
                                                                {
                                                                    MessageBox.Show("La población no puede tener una longitud superior a 100", "Campo Población", MessageBoxButton.OK, MessageBoxImage.Error);
                                                                    validateDatas = false;
                                                                }
                                                                else
                                                                {
                                                                    if (phone.Length > 15)
                                                                    {
                                                                        MessageBox.Show("El telefono no puede tener una longitud superior a 15", "Campo Telfono", MessageBoxButton.OK, MessageBoxImage.Error);
                                                                        validateDatas = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        //comprobamos que el dni no exista ya 
                                                                        if (!FirstimeDni)
                                                                        {
                                                                            NumberRows = Convert.ToInt32(tableDriver.ScalarQueryDni(Dni.Trim()));
                                                                            if (NumberRows > 0)
                                                                            {
                                                                                MessageBox.Show("El Dni introducido ya existe", "Campo dni", MessageBoxButton.OK, MessageBoxImage.Error);
                                                                                validateDatas = false;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //comprobamos que el codigo no xista 
                                                                            if (!FirstTimeCode)
                                                                            {
                                                                                NumberRows = Convert.ToInt32(tableDriver.ScalarQueryByCode(Code.Trim()));
                                                                                if (NumberRows > 0)
                                                                                {
                                                                                    MessageBox.Show("El código introducido ya existe", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return validateDatas;
        }

        //conversor de codigo de provincia al nombre
        private DriversDataSet.System__Data_ProvincesRow InitializeComboBoxProvince(string Codeprovince)
        {

            return Provinces.SingleOrDefault(c => c.Code.ToString().Trim() == Codeprovince.ToString().Trim());
        }


        //conversor de codigo del pais al nombre
        private DriversDataSet.System_Data_CountriesRow InitializeComboBoxCountry(string iso)
        {
            return Countries.SingleOrDefault(c => c.Iso.ToString().Trim() == iso.ToString().Trim());
        }

    }
}
