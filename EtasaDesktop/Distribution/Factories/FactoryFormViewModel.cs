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

namespace EtasaDesktop.Distribution.Factories
{
    public class FactoryFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        private bool _isSelected;
        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        //private ImportOperator _selectedOperator;

        private FactoryDataSet.FactoriesRow _row;

        private FactoryDataSet.Factories_ObsRow _row_obs;

        public bool FirstTimeCode = true;

        public ObservableCollection<FactoryFormViewModel> Factory { get; private set; }

        private FactoryDataSet.System_Data_CountriesRow _selectedCountry;

        private FactoryDataSet.System__Data_ProvincesRow _selectedProvince;

        public ObservableCollection<FactoryDataSet.System_Data_CountriesRow> Countries { get; private set; }

        public ObservableCollection<FactoryDataSet.System__Data_ProvincesRow> Provinces { get; private set; }

        public FactoryFormViewModel()
        {
            FactoryDataSet dataset = new FactoryDataSet();
            FactoryDataSetTableAdapters.FactoriesTableAdapter Factory = new FactoryDataSetTableAdapters.FactoriesTableAdapter();
            FactoryDataSetTableAdapters.Factories_ObsTableAdapter Factory_obs = new FactoryDataSetTableAdapters.Factories_ObsTableAdapter();

            _row = dataset.Factories.NewFactoriesRow();
            _row_obs = dataset.Factories_Obs.NewFactories_ObsRow();

            Countries = new ObservableCollection<FactoryDataSet.System_Data_CountriesRow>();
            LoadCountries();
            Provinces = new ObservableCollection<FactoryDataSet.System__Data_ProvincesRow>();
            LoadProvinces();

            //factoria
            _row.Code= "";
            _row.Name = "";
            _row.CreatedDate = DateTime.Now;
            _row.ModifiedDate = DateTime.Now;
            _row.Address = "";
            _row.City = "";
            _row.PostCode = "";
            _row.Province = "";
            _row.Country = "";
            _row.Latitude = 0;
            _row.Longitude = 0;
            _row.Enabled = true;

            //observaciones factoria
            _row_obs.Observations = "";

            SaveCommand = new RelayCommand(Save, CanSave);
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

        public FactoryDataSet.System_Data_CountriesRow SelectedCountry
        {
            get => 
                _selectedCountry;
            set
            {
                Set(ref _selectedCountry, value);
                _row.Country = value.Iso.ToString().Trim();
            }
        }

        public FactoryDataSet.System__Data_ProvincesRow SelectedProvince
        {
            get => _selectedProvince;
            set
            {
                Set(ref _selectedProvince, value);
                _row.Province = value.Code.ToString().Trim();
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

        public void LoadCountries()
        {
            Countries.Clear();
            DataTable CountriesSpain;
            FactoryDataSet ds = new FactoryDataSet();
            FactoryDataSetTableAdapters.System_Data_CountriesTableAdapter adapt = new FactoryDataSetTableAdapters.System_Data_CountriesTableAdapter();
            CountriesSpain = adapt.GetDataOnlySpainByID(199);

            foreach (FactoryDataSet.System_Data_CountriesRow row in CountriesSpain.Rows)
            {
                Countries.Add(row);
            }
        }

        public void LoadProvinces()
        {
            Provinces.Clear();

            FactoryDataSet ds = new FactoryDataSet();
            FactoryDataSetTableAdapters.System__Data_ProvincesTableAdapter adapt = new FactoryDataSetTableAdapters.System__Data_ProvincesTableAdapter();
            adapt.Fill(ds.System__Data_Provinces);

            foreach (FactoryDataSet.System__Data_ProvincesRow row in ds.System__Data_Provinces.Rows)
            {
                Provinces.Add(row);
            }
        }

        public string Address
        {
            get { return _row.Address; }
            set
            {
                if (_row.Address != value)
                {
                    _row.Address= value;
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

        public string PostCode
        {
            get { return _row.PostCode; }
            set
            {
                if (_row.PostCode != value)
                {
                    _row.PostCode = value;
                    NotifyPropertyChanged("Code");
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
                    _row.Province = value;
                    RaisePropertyChanged();
                }
            }
        }

        public float Latitude
        {
            get { return _row.Latitude; }
            set
            {
                if (_row.Latitude != value)
                {
                    _row.Latitude= value;
                    RaisePropertyChanged();
                }
            }
        }

        public float Longitude
        {
            get { return _row.Longitude; }
            set
            {
                if (_row.Longitude != value)
                {
                    _row.Longitude = value;
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

        private bool CanSave()
        {
            return true;
        }

        public void Load(int IdFactory)
        {
            try
            {
                FactoryDataSet dataset = new FactoryDataSet();
                FactoryDataSetTableAdapters.FactoriesTableAdapter TableFactory = new FactoryDataSetTableAdapters.FactoriesTableAdapter();
                FactoryDataSet.FactoriesDataTable dataTable = TableFactory.GetDataFactoryById(IdFactory);

                if (dataTable.Rows.Count > 0)
                {
                    FactoryDataSetTableAdapters.Factories_ObsTableAdapter factory_obs = new FactoryDataSetTableAdapters.Factories_ObsTableAdapter();
                    FactoryDataSet.Factories_ObsDataTable dataTable_Obs = factory_obs.GetDataFactory_ObsById(IdFactory);

                    _row = (FactoryDataSet.FactoriesRow)dataTable.Rows[0];

                    RaisePropertyChanged(nameof(Code));
                    RaisePropertyChanged(nameof(Name));          
                    RaisePropertyChanged(nameof(Address));
                    RaisePropertyChanged(nameof(City));
                    RaisePropertyChanged(nameof(PostCode));
                    RaisePropertyChanged(nameof(Latitude));
                    RaisePropertyChanged(nameof(Longitude));
                    RaisePropertyChanged(nameof(Country));
                    RaisePropertyChanged(nameof(Province));

                    RaisePropertyChanged(nameof(Enabled));

                    SelectedCountry = InitializeComboBoxCountry(Country);

                    SelectedProvince = InitializeComboBoxProvince(Province);

                    if (dataTable_Obs.Rows.Count > 0)
                    {
                        _row_obs = (FactoryDataSet.Factories_ObsRow)dataTable_Obs.Rows[0];
                        RaisePropertyChanged(nameof(Observations));
                    }
                    
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
            int indentificadorFactoria = 0;
            try
            {
                FactoryDataSet dataset = new FactoryDataSet();
                FactoryDataSetTableAdapters.FactoriesTableAdapter TableFactory = new FactoryDataSetTableAdapters.FactoriesTableAdapter();
                FactoryDataSetTableAdapters.Factories_ColorsTableAdapter TableFactoryColor = new FactoryDataSetTableAdapters.Factories_ColorsTableAdapter();
              
                validate = validateDatas(_row.Code, _row.Name,_row.Longitude,_row.Latitude,dataset,TableFactory,this.FirstTimeCode,_row.Province, _row.Country);

                if (validate)
                {
                   
                    FactoryDataSetTableAdapters.Factories_ObsTableAdapter tableAdapterFactoryObs = new FactoryDataSetTableAdapters.Factories_ObsTableAdapter();
                    FactoryDataSet.FactoriesDataTable dataTable = TableFactory.GetDataFactoryById(_row.Id);


                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowFactory = dataset.Factories.NewFactoriesRow();

                        rowFactory = dataTable[0];
                        rowFactory["Code"] = _row.Code;
                        rowFactory["Name"] = _row.Name;
                        rowFactory["CreatedDate"] = DateTime.Now;
                        rowFactory["ModifiedDate"] = DateTime.Now;
                        rowFactory["Address"] = _row.Address;
                        rowFactory["City"] = _row.City;
                        rowFactory["PostCode"] = _row.PostCode;
                        rowFactory["Province"] = _row.Province;
                        rowFactory["Country"] = _row.Country;
                        rowFactory["Latitude"] = _row.Latitude;
                        rowFactory["Longitude"] = _row.Longitude;
                        rowFactory["Enabled"] = _row.Enabled;

                        int numeroderegistros = TableFactory.Update(rowFactory);

                        //si se actualizado la factoría actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            tableAdapterFactoryObs.UpdateFactoryObs(_row_obs.Observations, _row.Id);
                        }
                    }
                    //insertamos el nuevo factory  y las observaciones (Factory_obs)
                    else
                    {
                        TableFactory = new FactoryDataSetTableAdapters.FactoriesTableAdapter();
                        //obtenemos de la tabla Factory  el nuevo registro que se introducira 
                        var rowFactory = dataset.Factories.NewFactoriesRow();

                        rowFactory.Code = _row.Code;
                        rowFactory.Name = _row.Name;
                        rowFactory.CreatedDate = DateTime.Now;
                        rowFactory.ModifiedDate = DateTime.Now;
                        rowFactory.Address = _row.Address;
                        rowFactory.City = _row.City;
                        rowFactory.PostCode = _row.PostCode;
                        rowFactory.Province = _row.Province;
                        rowFactory.Country = _row.Country;
                        rowFactory.Latitude = _row.Latitude;
                        rowFactory.Longitude = _row.Longitude;
                        rowFactory.Enabled = _row.Enabled;

                        //agregamos la nueva fila 
                        dataset.Factories.AddFactoriesRow(rowFactory);
                        //actualizamos la tabla factory en el dataset 
                        TableFactory.Update(dataset.Factories);

                        //al insertar el registro nuevo en la tabla factory Id se actualiza 
                        indentificadorFactoria = Convert.ToInt32(rowFactory.Id);
                  
                        // a continuación insertamos el resto de datos en la tabla Factory_obs cepsa con el indentificador de la factoria recient introduzida
                        tableAdapterFactoryObs = new FactoryDataSetTableAdapters.Factories_ObsTableAdapter();
                        //obtenemos de la tabla Factoria_obs el nuevo registro que se introducira 
                        var rowFactory_obs = dataset.Factories_Obs.NewFactories_ObsRow();

                        //Id
                        rowFactory_obs.Id = indentificadorFactoria;
                        //observations
                        rowFactory_obs.Observations = _row_obs.Observations;

                        //agregamos la nueva fila a tabla Factory_obs
                        dataset.Factories_Obs.AddFactories_ObsRow(rowFactory_obs);
                        //actualizamos la tabla Factory_obs  en el dataset 
                        tableAdapterFactoryObs.Update(dataset.Factories_Obs);


                        //creamos los colores de la factoría por defecto
                        TableFactoryColor = new FactoryDataSetTableAdapters.Factories_ColorsTableAdapter();
                        //obtenemos de la tabla Factory  el nuevo registro que se introducira 
                        var rowFactoryColor = dataset.Factories_Colors.NewFactories_ColorsRow();

                        //identificador de la factoría
                        rowFactoryColor.Id = indentificadorFactoria;
                        //color de la factoría 
                        rowFactoryColor.FactoryColor = "#FFFFFF";
                        //color del cliente 
                        rowFactoryColor.ClientColor = "#FFFFFF";
                        //color urgente 
                        rowFactoryColor.UrgentColor = "#FFFFFF";
                        //color final del dia (entre fechas) 
                        rowFactoryColor.FinalDayColor = "#FFFFFF";
                        //color preferencia 
                        rowFactoryColor.PreferenceColor = "#FFFFFF";
                        //color ultima preferencia 
                        rowFactoryColor.TheLastPreferent = "#FFFFFF";

                        //agregamos la nueva fila a la tabla de colores factoria 
                        dataset.Factories_Colors.AddFactories_ColorsRow(rowFactoryColor);

                        //actualizamos la tabla factoría color 
                        TableFactoryColor.Update(dataset.Factories_Colors);
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
                this.FirstTimeCode = false;
            }
        }

        private static bool validateDatas(string Code, string Name, float longitude , float latitude, FactoryDataSet dataset, FactoryDataSetTableAdapters.FactoriesTableAdapter FactoryTable,bool FirstTimeCode,string provincia, string pais)
        {
            bool validateDatas = true;


            if (string.IsNullOrEmpty(Code))
            {
                MessageBox.Show("El Código no puede estar en blanco", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                validateDatas = false;
            }
            else
            {
                if (!IsNumeric(longitude.ToString().Trim()))
                {
                    MessageBox.Show("El Campo longitud tiene que contener un valor numérico", "Campo Longitud", MessageBoxButton.OK, MessageBoxImage.Error);
                    validateDatas = false;
                }
                else
                {
                    if (!IsNumeric(latitude.ToString().Trim()))
                    {
                        MessageBox.Show("El Campo latitud tiene que contener un valor numérico", "Campo Latitud", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {

                        if (Code.Length > 4)
                        {
                            MessageBox.Show("El Código no puede tener más de 4 caracteres", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                            validateDatas = false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(provincia.ToString()))
                            {
                                MessageBox.Show("El campo provincia no puede estar vacio", "Campo Provincia", MessageBoxButton.OK, MessageBoxImage.Error);
                                validateDatas = false;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(pais.ToString()))
                                {
                                    MessageBox.Show("El Campo pais no puede estar vacio", "Campo Pais", MessageBoxButton.OK, MessageBoxImage.Error);
                                    validateDatas = false;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(Name) || Name.ToString().Length > 100)
                                    {
                                        if (string.IsNullOrEmpty(Name))
                                        {
                                            MessageBox.Show("El nombre no puede estar en blanco", "Campo Nombre ", MessageBoxButton.OK, MessageBoxImage.Error);
                                            validateDatas = false;
                                        }
                                        else
                                        {
                                            if (Name.ToString().Length > 100)
                                            {
                                                MessageBox.Show("El nombre no puede tener una extensión superior a 100", "Campo Nombre ", MessageBoxButton.OK, MessageBoxImage.Error);
                                                validateDatas = false;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (!FirstTimeCode)
                                        {
                                            int NumberRows = Convert.ToInt32(FactoryTable.ScalarQueryByCode(Code));
                                            if (NumberRows > 0)
                                            {
                                                MessageBox.Show("El Código de la factoría ya existe", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                                                validateDatas = false;
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

        public static bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        //conversor de codigo de provincia al nombre
        private FactoryDataSet.System__Data_ProvincesRow InitializeComboBoxProvince(string Codeprovince)
        {   
           return Provinces.SingleOrDefault(c => c.Code.ToString().Trim() == Codeprovince.ToString().Trim());
        }

        //conversor de codigo de provincia al nombre
        private FactoryDataSet.System_Data_CountriesRow InitializeComboBoxCountry(string iso)
        {
            return Countries.SingleOrDefault(c => c.Iso.ToString().Trim() == iso.ToString().Trim());
        }
    }
}
