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
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Runtime.CompilerServices;

namespace EtasaDesktop.Distribution.Operators
{
    public class OperatorFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        private bool _isSelected;
        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        //private ImportOperator _selectedOperator;

        private OperatorDataset.OperatorsRow _row;

        private OperatorDataset.Operators_ObsRow _row_obs;

        private OperatorDataset.System_Data_CountriesRow _selectedCountry;

        private OperatorDataset.System__Data_ProvincesRow _selectedProvince;

        public ObservableCollection<OperatorDataset.System_Data_CountriesRow> Countries { get; set; }

        public ObservableCollection<OperatorDataset.System__Data_ProvincesRow> Provinces { get; set; }

        public ObservableCollection<OperatorFormViewModel> Operator { get; private set; }

        public OperatorFormViewModel()
        {

            OperatorDataset dataset = new OperatorDataset();
            OperatorDatasetTableAdapters.OperatorsTableAdapter Operator = new OperatorDatasetTableAdapters.OperatorsTableAdapter();
            OperatorDatasetTableAdapters.Operators_ObsTableAdapter Operator_obs = new OperatorDatasetTableAdapters.Operators_ObsTableAdapter();

            Countries = new ObservableCollection<OperatorDataset.System_Data_CountriesRow>();
            LoadCountries();
            Provinces = new ObservableCollection<OperatorDataset.System__Data_ProvincesRow>();
            LoadProvinces();

            _row = dataset.Operators.NewOperatorsRow();
            _row_obs = dataset.Operators_Obs.NewOperators_ObsRow();

            //cliente
            _row.Code = "";
            _row.Name = "";
            _row.Cif = "";
            _row.CreatedDate = DateTime.Now;
            _row.ModifiedDate = DateTime.Now;
            _row.Address = "";
            _row.City = "";
            _row.PostCode = "";
            _row.Province = "";
            _row.Country = "";
            _row.Phone = "";
            _row.Phone2 = "";
            _row.Contact = "";
            _row.PhoneMobile = "";
            _row.Fax = "";
            _row.Email = "";
            _row.Enabled = true;
            _row.Id = 0;

            //observaciones 
            _row_obs.Observations = "";
            
            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public OperatorDataset.System_Data_CountriesRow SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                Set(ref _selectedCountry, value);
                _row.Country = value.Iso.ToString().Trim();
            }
        }

        public OperatorDataset.System__Data_ProvincesRow SelectedProvince
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
            OperatorDataset ds = new OperatorDataset();
            OperatorDatasetTableAdapters.System_Data_CountriesTableAdapter adapt = new OperatorDatasetTableAdapters.System_Data_CountriesTableAdapter();
            CountriesSpain = adapt.GetDataOnlySpainByID(199);

            foreach (OperatorDataset.System_Data_CountriesRow row in CountriesSpain.Rows)
            {
                Countries.Add(row);
            }
        }

        public void LoadProvinces()
        {
            Provinces.Clear();

            OperatorDataset ds = new OperatorDataset();
            OperatorDatasetTableAdapters.System__Data_ProvincesTableAdapter adapt = new OperatorDatasetTableAdapters.System__Data_ProvincesTableAdapter();
            adapt.Fill(ds.System__Data_Provinces);

            foreach (OperatorDataset.System__Data_ProvincesRow row in ds.System__Data_Provinces.Rows)
            {
                Provinces.Add(row);
            }
        }


        public OperatorFormViewModel(OperatorDataset.OperatorsRow row)
        {
            _row = row;
        }


        private bool CanSave()
        {    
            return true;
        }

        public OperatorDataset.OperatorsRow DataRow
        {
            get { return _row; }
            set
            {
            }
        }

        public Int32 Id
        {
            get { return _row.Id; }
            set
            {
                if (_row.Id != value)
                {
                    _row.Id = value;
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

        public string Cif
        {
            get { return _row.Cif; }
            set
            {
                if (_row.Cif != value)
                {
                    _row.Cif = value;
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

        public string PostCode
        {
            get { return _row.PostCode; }
            set
            {
                if (_row.PostCode != value)
                {
                    _row.PostCode = value;
                    RaisePropertyChanged();
                }
            }
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

        public string Phone2
        {
            get { return _row.Phone2; }
            set
            {
                if (_row.Phone2 != value)
                {
                    _row.Phone2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string MobilePhone
        {
            get { return _row.PhoneMobile; }
            set
            {
                if (_row.PhoneMobile != value)
                {
                    _row.PhoneMobile = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Fax
        {
            get { return _row.Fax; }
            set
            {
                if (_row.Fax != value)
                {
                    _row.Fax = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string Contact
        {
            get { return _row.Contact; }
            set
            {
                if (_row.Contact != value)
                {
                    _row.Contact = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email
        {
            get { return _row.Email; }
            set
            {
                if (_row.Email != value)
                {
                    _row.Email = value;
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

        public void Load(int IdOperator)
        {
            try
            {
                OperatorDataset ds = new OperatorDataset();
                OperatorDatasetTableAdapters.OperatorsTableAdapter TableOperators = new OperatorDatasetTableAdapters.OperatorsTableAdapter();
                OperatorDataset.OperatorsDataTable dataTable = TableOperators.GetDataByRowByIdOperator(IdOperator);
               
                if (dataTable.Rows.Count > 0)
                {
                    OperatorDatasetTableAdapters.Operators_ObsTableAdapter Operator_obs = new OperatorDatasetTableAdapters.Operators_ObsTableAdapter();
                    OperatorDataset.Operators_ObsDataTable dataTable_Obs = Operator_obs.GetOperatorObs(IdOperator);
                   
                    _row = (OperatorDataset.OperatorsRow)dataTable.Rows[0];
                    RaisePropertyChanged(nameof(Code));
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Cif));
                    RaisePropertyChanged(nameof(Address));
                    RaisePropertyChanged(nameof(City));
                    RaisePropertyChanged(nameof(PostCode));
                    RaisePropertyChanged(nameof(Province));
                    RaisePropertyChanged(nameof(Country));
                    RaisePropertyChanged(nameof(Phone));
                    RaisePropertyChanged(nameof(Phone2));
                    RaisePropertyChanged(nameof(MobilePhone));
                    RaisePropertyChanged(nameof(Fax));
                    RaisePropertyChanged(nameof(Email));
                    RaisePropertyChanged(nameof(Contact));              
                    RaisePropertyChanged(nameof(Enabled));

                    SelectedCountry = InitializeComboBoxCountry(Country);

                    SelectedProvince = InitializeComboBoxProvince(Province);

                    _row_obs = (OperatorDataset.Operators_ObsRow)dataTable_Obs.Rows[0];
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
            int indentificadorOperador = 0;
            bool validatedata = true;
            try
            {
                validatedata = validateDatas(_row.Code, _row.Name, _row.Phone, _row.Phone2, _row.PhoneMobile, _row.Email, _row.Address, _row.PostCode, _row.City,_row.Country,_row.Province);
                if (validatedata)
                {
                    OperatorDataset dataset = new OperatorDataset();
                    OperatorDatasetTableAdapters.Operators_ObsTableAdapter tableAdapterOperatorObs = new OperatorDatasetTableAdapters.Operators_ObsTableAdapter();

                    OperatorDatasetTableAdapters.OperatorsTableAdapter TableOperator = new OperatorDatasetTableAdapters.OperatorsTableAdapter();
                    OperatorDataset.OperatorsDataTable dataTable = TableOperator.GetDataByRowByIdOperator(_row.Id);

                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowOperator = dataset.Operators.NewOperatorsRow();
                        rowOperator = dataTable[0];
                        rowOperator["Code"] = _row.Code;
                        rowOperator["Name"] = _row.Name;
                        rowOperator["Cif"] = _row.Cif;
                        rowOperator["ModifiedDate"] = DateTime.Now;
                        rowOperator["Address"] = _row.Address;
                        rowOperator["City"] = _row.City;
                        rowOperator["PostCode"] = _row.PostCode;
                        rowOperator["Country"] = _row.Country;
                        rowOperator["Province"] = _row.Province;
                        rowOperator["Phone"] = _row.Phone;
                        rowOperator["Phone2"] = _row.Phone2;
                        rowOperator["Contact"] = _row.Contact;
                        rowOperator["PhoneMobile"] = _row.PhoneMobile;
                        rowOperator["Fax"] = _row.Fax;
                        rowOperator["Email"] = _row.Email;

                        rowOperator["Enabled"] = _row.Enabled;

                        int numeroderegistros = TableOperator.Update(rowOperator);

                        //si se actualizado el operador actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            tableAdapterOperatorObs.UpdateOperatorsObs(_row_obs.Observations, _row.Id);
                        }
                    }
                    //insertamos el nuevo operador  y las observaciones (cliente_obs)
                    else
                    {
                        TableOperator = new OperatorDatasetTableAdapters.OperatorsTableAdapter();
                        //obtenemos de la tabla operador  el nuevo registro que se introducira 
                        var rowOperator = dataset.Operators.NewOperatorsRow();

                        //Código
                        rowOperator.Code = _row.Code;
                        //Nombre
                        rowOperator.Name = _row.Name;
                        //Cif
                        rowOperator.Cif = _row.Cif;
                        //fecha creacion
                        rowOperator.CreatedDate = DateTime.Now;
                        //fecha modificacion
                        rowOperator.ModifiedDate = DateTime.Now;
                        //direccion
                        rowOperator.Address = _row.Address;
                        //ciudad
                        rowOperator.City = _row.City;
                        //codigo postal 
                        rowOperator.PostCode = _row.PostCode;
                        //pais
                        rowOperator.Country = _row.Country;
                        //provincia
                        rowOperator.Province = _row.Province;
                        //Contacto
                        rowOperator.Contact = _row.Contact;
                        //telefono 1 
                        rowOperator.Phone = _row.Phone;
                        //telefono 2 
                        rowOperator.Phone2 = _row.Phone2;
                        //telefono movil
                        rowOperator.PhoneMobile = _row.PhoneMobile;
                        //Fax
                        rowOperator.Fax = _row.Fax;
                        //Emails
                        rowOperator.Email = _row.Email;
                        //enable
                        rowOperator.Enabled = _row.Enabled;

                        //agregamos la nueva fila 
                        dataset.Operators.AddOperatorsRow(rowOperator);
                        //actualizamos la tabla operador en el dataset 
                        TableOperator.Update(dataset.Operators);

                        //al insertar el registro nuevo en la tabla operador el Id se actualiza en el objeto wororder (se le asignara al id de la tabla orders_cepsa) 
                        indentificadorOperador = Convert.ToInt32(rowOperator.Id);


                        // a continuación insertamos el resto de datos en la tabla operator_obs cepsa con el indentificador del cliente reciente introduzido
                        tableAdapterOperatorObs = new OperatorDatasetTableAdapters.Operators_ObsTableAdapter();
                        //obtenemos de la tabla operator_obs el nuevo registro que se introducira 
                        var rowOperator_obs = dataset.Operators_Obs.NewOperators_ObsRow();

                        //Id
                        rowOperator_obs.Id = indentificadorOperador;
                        //observations
                        rowOperator_obs.Observations = _row_obs.Observations;

                        //agregamos la nueva fila a tabla operator_obs
                        dataset.Operators_Obs.AddOperators_ObsRow(rowOperator_obs);
                        //actualizamos la tabla operatot_obs  en el dataset 
                        tableAdapterOperatorObs.Update(dataset.Operators_Obs);


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

           Operator.Clear();

            OperatorDataset ds = new OperatorDataset();
            OperatorDatasetTableAdapters.OperatorsTableAdapter adapt = new OperatorDatasetTableAdapters.OperatorsTableAdapter();
            adapt.Fill(ds.Operators);

            foreach (OperatorDataset.OperatorsRow row in ds.Operators.Rows)
            {
                Operator.Add(new OperatorFormViewModel(row));
            }
        }

        //conversor de codigo de provincia al nombre
        private static string CodeProvinceTostring(string CodeProvince)
        {

            string ProvincieName = "";
            switch (CodeProvince)
            {
                //insertamos registros de cabecera
                case "01":
                    ProvincieName = "Álava";
                    break;
                case "02":
                    ProvincieName = "Albacete";
                    break;
                case "03":
                    ProvincieName = "Alicante";
                    break;
                case "04":
                    ProvincieName = "Almería";
                    break;
                case "05":
                    ProvincieName = "Ávila";
                    break;
                case "06":
                    ProvincieName = "Badajoz";
                    break;
                case "07":
                    ProvincieName = "Palma de Mallorca";
                    break;
                case "08":
                    ProvincieName = "Barcelona";
                    break;
                case "09":
                    ProvincieName = "Burgos";
                    break;
                case "10":
                    ProvincieName = "Cáceres";
                    break;
                case "11":
                    ProvincieName = "Cádiz";
                    break;
                case "12":
                    ProvincieName = "Castellón";
                    break;
                case "13":
                    ProvincieName = "Ciudad Real";
                    break;
                case "14":
                    ProvincieName = " Córdoba";
                    break;
                case "15":
                    ProvincieName = "Coruña";
                    break;
                case "16":
                    ProvincieName = "Cuenca";
                    break;
                case "17":
                    ProvincieName = "Gerona";
                    break;
                case "18":
                    ProvincieName = "Granada";
                    break;
                case "19":
                    ProvincieName = "Guadalajara";
                    break;
                case "20":
                    ProvincieName = "Guipúzcoa";
                    break;
                case "21":
                    ProvincieName = "Huelva";
                    break;
                case "22":
                    ProvincieName = "Huesca";
                    break;
                case "23":
                    ProvincieName = "Jaén";
                    break;
                case "24":
                    ProvincieName = "León";
                    break;
                case "25":
                    ProvincieName = "Lleida";
                    break;
                case "26":
                    ProvincieName = "La Rioja";
                    break;
                case "27":
                    ProvincieName = "Lugo";
                    break;
                case "28":
                    ProvincieName = "Madrid";
                    break;
                case "29":
                    ProvincieName = "Málaga";
                    break;
                case "30":
                    ProvincieName = "Murcia";
                    break;
                case "31":
                    ProvincieName = "Navarra";
                    break;
                case "32":
                    ProvincieName = "Orense";
                    break;
                case "33":
                    ProvincieName = "Asturias";
                    break;
                case "34":
                    ProvincieName = "Palencia";
                    break;
                case "35":
                    ProvincieName = "Las Palmas";
                    break;
                case "36":
                    ProvincieName = "Pontevedra";
                    break;
                case "37":
                    ProvincieName = "Salamanca";
                    break;
                case "38":
                    ProvincieName = "Santa Cruz de Tenerife";
                    break;
                case "39":
                    ProvincieName = "Cantabria";
                    break;
                case "40":
                    ProvincieName = "Segovia";
                    break;
                case "41":
                    ProvincieName = "Sevilla";
                    break;
                case "42":
                    ProvincieName = "Soria";
                    break;
                case "43":
                    ProvincieName = "Tarragona";
                    break;
                case "44":
                    ProvincieName = "Teruel";
                    break;
                case "45":
                    ProvincieName = "Toledo";
                    break;
                case "46":
                    ProvincieName = "Valencia";
                    break;
                case "47":
                    ProvincieName = "Valladolid";
                    break;
                case "48":
                    ProvincieName = "Vizcaya";
                    break;
                case "49":
                    ProvincieName = "Zamora";
                    break;
                case "50":
                    ProvincieName = "Zaragoza";
                    break;
                case "51":
                    ProvincieName = "Ceuta";
                    break;
                case "52":
                    ProvincieName = "Melilla";
                    break;
            }

            return ProvincieName;
        }

        //conversor de codigo de provincia al nombre
        private OperatorDataset.System__Data_ProvincesRow InitializeComboBoxProvince(string Codeprovince)
        {
           return Provinces.SingleOrDefault(c => c.Code.ToString().Trim() == Codeprovince.ToString().Trim());
        }

        //conversor de codigo de provincia al nombre
        private OperatorDataset.System_Data_CountriesRow InitializeComboBoxCountry(string iso)
        {  
           return Countries.SingleOrDefault(c => c.Iso.ToString().Trim() == iso.ToString().Trim());
        }

        private static bool validateDatas(string Code, string Name,string telefono1,string telefono2,string mobilphone,string Email,string adress,string codepost, string city,string country, string province)
        {
            bool validateDatas = true;
            if (string.IsNullOrEmpty(Name) || Name.ToString().Length > 100)
            {
                if(string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("El nombre no puede estar en blanco", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                    validateDatas = false;
                }
                else
                {
                    if(Name.ToString().Length > 100)
                    {
                        MessageBox.Show("El nombre no puede superar los 100 caracteres", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;

                    }
                }
            }       
            else
            {
                if (string.IsNullOrEmpty(country.ToString()))
                {
                    MessageBox.Show("El Campo pais no puede estra en blanco", "Campo Pais", MessageBoxButton.OK, MessageBoxImage.Error);
                    validateDatas = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(province.ToString()))
                    {
                        MessageBox.Show("El Campo provincia no puede estar en blanco", "Campo Provincia", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {

                        if (string.IsNullOrEmpty(Code) || Code.ToString().Length > 5)
                        {
                            if(string.IsNullOrEmpty(Code))
                            {
                                MessageBox.Show("El codigo no puede estar vacio  ", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                                validateDatas = false;
                            }
                            else
                            {
                                if(Code.ToString().Length > 5)
                                {
                                    MessageBox.Show("El codigo no puede tener un longitud superior a 5 ", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                                    validateDatas = false;
                                }
                            }
                        }                  
                        else
                        {
                            if (telefono1.Length > 15 || string.IsNullOrEmpty(telefono1))
                            {
                                MessageBox.Show("El teléfono 1 no puede contener más de 15 caracteres o estar vacio ", "Campo telefono 1", MessageBoxButton.OK, MessageBoxImage.Error);
                                validateDatas = false;
                            }
                            else
                            {
                                if (telefono2.Length > 15)
                                {
                                    MessageBox.Show("El telefono 2 no puede contener más de 15 caracteres", "Campo telefono 2", MessageBoxButton.OK, MessageBoxImage.Error);
                                    validateDatas = false;
                                }
                                else
                                {
                                    if (mobilphone.Length > 15)
                                    {
                                        MessageBox.Show("El telefono mobil no puede contener más de 15 caracteres", "Campo telefono mobil", MessageBoxButton.OK, MessageBoxImage.Error);
                                        validateDatas = false;
                                    }
                                    else
                                    {
                                        if (Email.Length > 50)
                                        {
                                            MessageBox.Show("El Email no puede contener más de 50 caracteres", "Campo Email", MessageBoxButton.OK, MessageBoxImage.Error);
                                            validateDatas = false;
                                        }
                                        else
                                        {
                                            if (adress.Length > 100 || string.IsNullOrEmpty(adress))
                                            {
                                                MessageBox.Show("La dirección no puede contener mas de 100 caracteres", "Campo dirección", MessageBoxButton.OK, MessageBoxImage.Error);
                                                validateDatas = false;
                                            }
                                            else
                                            {
                                                if (codepost.Length > 10)
                                                {
                                                    MessageBox.Show("El codigo postal no puede tener mas de 10 caracteres", "Campo código postal", MessageBoxButton.OK, MessageBoxImage.Error);
                                                    validateDatas = false;
                                                }
                                                else
                                                {
                                                    if (city.Length > 50)
                                                    {
                                                        MessageBox.Show("La población no puede tener mas de 50 caracteres", "Campo población", MessageBoxButton.OK, MessageBoxImage.Error);
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
                }
            }                       
            return validateDatas;
        }
    }
}
