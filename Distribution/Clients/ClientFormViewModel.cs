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

namespace EtasaDesktop.Distribution.Clients
{
    public class ClientFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        private bool _isSelected;
        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        //private ImportOperator _selectedOperator;

        private ClientDataSet.ClientsRow _row;

        private ClientDataSet.Clients_ObsRow _row_obs;

        private ClientDataSet.System_Data_CountriesRow _selectedCountry;

        private ClientDataSet.System__Data_ProvincesRow _selectedProvince;

        public ObservableCollection<ClientDataSet.System_Data_CountriesRow> Countries { get; private set; }

        public ObservableCollection<ClientDataSet.System__Data_ProvincesRow> Provinces { get; private set; }

        public ObservableCollection<ClientFormViewModel> Client { get; private set; }

        public ClientFormViewModel()
        {
           
            ClientDataSet dataset = new ClientDataSet();
            ClientDataSetTableAdapters.ClientsTableAdapter Client = new ClientDataSetTableAdapters.ClientsTableAdapter();
            ClientDataSetTableAdapters.Clients_ObsTableAdapter Client_obs = new ClientDataSetTableAdapters.Clients_ObsTableAdapter();

            Countries = new ObservableCollection<ClientDataSet.System_Data_CountriesRow>();
            LoadCountries();
            Provinces = new ObservableCollection<ClientDataSet.System__Data_ProvincesRow>();
            LoadProvinces();

            _row = dataset.Clients.NewClientsRow();
            _row_obs = dataset.Clients_Obs.NewClients_ObsRow();

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
            _row.Contact = "";
            _row.Phone = "";
            _row.Phone2 = "";
            _row.PhoneMobile = "";
            _row.Fax = "";
            _row.Email = "";
            _row.Latitude = 0;
            _row.Longitude = 0;
            _row.Enabled = true;
            _row.Id = 0;

            //observaciones 
            _row_obs.Observations = "";
            
            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public ClientDataSet.System_Data_CountriesRow SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                Set(ref _selectedCountry, value);
                _row.Country = value.Iso.ToString().Trim();
            }
        }

        public ClientDataSet.System__Data_ProvincesRow SelectedProvince
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
            ClientDataSet ds = new ClientDataSet();
            ClientDataSetTableAdapters.System_Data_CountriesTableAdapter adapt = new ClientDataSetTableAdapters.System_Data_CountriesTableAdapter();
            CountriesSpain = adapt.GetDataOnlySpainByID(199);

            foreach (ClientDataSet.System_Data_CountriesRow row in CountriesSpain.Rows)
            {
                Countries.Add(row);
            }


        }

        public void LoadProvinces()
        {
            Provinces.Clear();

            ClientDataSet ds = new ClientDataSet();
            ClientDataSetTableAdapters.System__Data_ProvincesTableAdapter adapt = new ClientDataSetTableAdapters.System__Data_ProvincesTableAdapter();
            adapt.Fill(ds.System__Data_Provinces);

            foreach (ClientDataSet.System__Data_ProvincesRow row in ds.System__Data_Provinces.Rows)
            {
                Provinces.Add(row);
            }
        }


        public ClientFormViewModel(ClientDataSet.ClientsRow row)
        {
            _row = row;
        }


        private bool CanSave()
        {    
            return true;
        }

        public ClientDataSet.ClientsRow DataRow
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


        public float Latitude
        {
            get { return _row.Latitude; }
            set
            {
                if (_row.Latitude != value)
                {
                    _row.Latitude = value;
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

        public void Load(int IdClient)
        {
            try
            {
                ClientDataSet dataset = new ClientDataSet();
                ClientDataSetTableAdapters.ClientsTableAdapter TableClients = new ClientDataSetTableAdapters.ClientsTableAdapter();
                ClientDataSet.ClientsDataTable dataTable = TableClients.GetDataByRowByIdClient(IdClient);
               
                if (dataTable.Rows.Count > 0)
                {
                    ClientDataSetTableAdapters.Clients_ObsTableAdapter Client_obs = new ClientDataSetTableAdapters.Clients_ObsTableAdapter();
                    ClientDataSet.Clients_ObsDataTable dataTable_Obs = Client_obs.GetClientObs(IdClient);
                   
                    _row = (ClientDataSet.ClientsRow)dataTable.Rows[0];
                    RaisePropertyChanged(nameof(Code));
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Cif));
                    RaisePropertyChanged(nameof(Address));
                    RaisePropertyChanged(nameof(City));
                    RaisePropertyChanged(nameof(PostCode));
                    RaisePropertyChanged(nameof(Province));
                    RaisePropertyChanged(nameof(Country));
                    RaisePropertyChanged(nameof(Contact));
                    RaisePropertyChanged(nameof(Phone));
                    RaisePropertyChanged(nameof(Phone2));
                    RaisePropertyChanged(nameof(MobilePhone));
                    RaisePropertyChanged(nameof(Fax));
                    RaisePropertyChanged(nameof(Email));
                    RaisePropertyChanged(nameof(Latitude));
                    RaisePropertyChanged(nameof(Longitude));
                    RaisePropertyChanged(nameof(Enabled));

                    SelectedCountry = InitializeComboBoxCountry(Country);

                    SelectedProvince = InitializeComboBoxProvince(Province);

                    _row_obs = (ClientDataSet.Clients_ObsRow)dataTable_Obs.Rows[0];
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
            int indentificadorCliente = 0;
            bool validatedata = true;
            try
            {
                validatedata = validateDatas(_row.Code, _row.Name,_row.Contact,_row.Phone,_row.Phone2,_row.PhoneMobile,_row.Email,_row.Address,_row.PostCode,_row.City, _row.Province, _row.Country);
                if (validatedata)
                {         
                    ClientDataSet dataset = new ClientDataSet();
                    ClientDataSetTableAdapters.Clients_ObsTableAdapter tableAdapterClientsObs = new ClientDataSetTableAdapters.Clients_ObsTableAdapter();
                    ClientDataSetTableAdapters.Operators_ClientsTableAdapter tableAdapterOperator_client = new ClientDataSetTableAdapters.Operators_ClientsTableAdapter();

                    ClientDataSetTableAdapters.ClientsTableAdapter TableClients = new ClientDataSetTableAdapters.ClientsTableAdapter();
                    ClientDataSet.ClientsDataTable dataTable = TableClients.GetDataByRowByIdClient(_row.Id);

                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowClient = dataset.Clients.NewClientsRow();
                        rowClient = dataTable[0];
                        rowClient["Code"] = _row.Code;
                        rowClient["Name"] = _row.Name;
                        rowClient["Cif"] = _row.Cif;
                        rowClient["CreatedDate"] = DateTime.Now;
                        rowClient["ModifiedDate"] = DateTime.Now;
                        rowClient["Address"] = _row.Address;
                        rowClient["City"] = _row.City;
                        rowClient["PostCode"] = _row.PostCode;
                        rowClient["Country"] = _row.Country;
                        rowClient["Province"] = _row.Province;
                        rowClient["Contact"] = _row.Contact;
                        rowClient["Phone"] = _row.Phone;
                        rowClient["Phone2"] = _row.Phone2;
                        rowClient["PhoneMobile"] = _row.PhoneMobile;
                        rowClient["Fax"] = _row.Fax;
                        rowClient["Email"] = _row.Email;
                        rowClient["Latitude"] = _row.Latitude;
                        rowClient["Longitude"] = _row.Longitude;
                        rowClient["Enabled"] = _row.Enabled;

                        int numeroderegistros = TableClients.Update(rowClient);

                        //si se actualizado el cliente actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            tableAdapterClientsObs.UpdateClientObs(_row_obs.Observations, _row.Id);
                        }
                    }
                    //insertamos el nuevo cliente  y las observaciones (cliente_obs)
                    else
                    {
                        TableClients = new ClientDataSetTableAdapters.ClientsTableAdapter();
                        //obtenemos de la tabla cliente  el nuevo registro que se introducira 
                        var rowClient = dataset.Clients.NewClientsRow();

                        //Código
                        rowClient.Code = _row.Code;
                        //Nombre
                        rowClient.Name = _row.Name;
                        //Cif
                        rowClient.Cif = _row.Cif;
                        //fecha creacion
                        rowClient.CreatedDate = DateTime.Now;
                        //fecha modificacion
                        rowClient.ModifiedDate = DateTime.Now;
                        //direccion
                        rowClient.Address = _row.Address;
                        //ciudad
                        rowClient.City = _row.City;
                        //codigo postal 
                        rowClient.PostCode = _row.PostCode;
                        //pais
                        rowClient.Country = _row.Country;
                        //provincia
                        rowClient.Province = _row.Province;
                        //Contacto
                        rowClient.Contact = _row.Contact;
                        //telefono 1 
                        rowClient.Phone = _row.Phone;
                        //telefono 2 
                        rowClient.Phone2 = _row.Phone2;
                        //telefono movil
                        rowClient.PhoneMobile = _row.PhoneMobile;
                        //Fax
                        rowClient.Fax = _row.Fax;
                        //Emails
                        rowClient.Email = _row.Email;
                        //latitude
                        rowClient.Latitude = _row.Latitude;
                        //longitud
                        rowClient.Longitude = _row.Longitude;
                        //enable
                        rowClient.Enabled = _row.Enabled;

                        //agregamos la nueva fila 
                        dataset.Clients.AddClientsRow(rowClient);
                        //actualizamos la tabla Clients en el dataset 
                        TableClients.Update(dataset.Clients);

                        //al insertar el registro nuevo en la tabla Cliente el Id se actualiza en el objeto wororder (se le asignara al id de la tabla orders_cepsa) 
                        indentificadorCliente = Convert.ToInt32(rowClient.Id);

                        //generamos el codigo en la tabla cliente y lo actualizamos en el registro nuevo creado en la tabla clients  
                        DataRow[] dr = dataset.Clients.Select("id=" + indentificadorCliente + "");
                        if (dr.Length > 0)
                        {
                            dr[0][1] = indentificadorCliente.ToString().PadLeft(8, '0');           
                        }
                        TableClients.Update(dataset.Clients);
                        dataset.Clients.AcceptChanges();

                        // a continuación insertamos el resto de datos en la tabla cliente_obs cepsa con el indentificador del cliente reciente introduzido
                        tableAdapterClientsObs = new ClientDataSetTableAdapters.Clients_ObsTableAdapter();
                        //obtenemos de la tabla Client_obs el nuevo registro que se introducira 
                        var rowClient_obs = dataset.Clients_Obs.NewClients_ObsRow();

                        //Id
                        rowClient_obs.Id = indentificadorCliente;
                        //observations
                        rowClient_obs.Observations = _row_obs.Observations;

                        //agregamos la nueva fila a tabla client_obs
                        dataset.Clients_Obs.AddClients_ObsRow(rowClient_obs);
                        //actualizamos la tabla client_obs  en el dataset 
                        tableAdapterClientsObs.Update(dataset.Clients_Obs);

                        /*
                        // insertamos el registro en la tabla operator_clients 
                        tableAdapterOperator_client = new ClientDataSetTableAdapters.Operators_ClientsTableAdapter();
                        //obtenemos la fila de la tabla 
                        var rowClient_operator_client = dataset.Operators_Clients.NewOperators_ClientsRow();
                        //Id_operador
                        rowClient_operator_client.OperatorId = 500;
                        //id_cliente
                        rowClient_operator_client.ClientId = indentificadorCliente;
                        //code_cliente
                        rowClient_operator_client.Code = indentificadorCliente.ToString().PadLeft(8, '0');
                        //agregamos la nueva fila a tabla operator_client
                        dataset.Operators_Clients.AddOperators_ClientsRow(rowClient_operator_client);
                        //actualizamos la tabla operator_client  en el dataset 
                        tableAdapterOperator_client.Update(dataset.Operators_Clients);
                        */
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

            Client.Clear();

            ClientDataSet ds = new ClientDataSet();
            ClientDataSetTableAdapters.ClientsTableAdapter adapt = new ClientDataSetTableAdapters.ClientsTableAdapter();
            adapt.Fill(ds.Clients);

            foreach (ClientDataSet.ClientsRow row in ds.Clients.Rows)
            {
                Client.Add(new ClientFormViewModel(row));
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
        private ClientDataSet.System__Data_ProvincesRow InitializeComboBoxProvince(string Codeprovince)
        {
           return Provinces.SingleOrDefault(c => c.Code.ToString().Trim() == Codeprovince.ToString().Trim());
        }

        //conversor de codigo de provincia al nombre
        private ClientDataSet.System_Data_CountriesRow InitializeComboBoxCountry(string iso)
        {  
           return Countries.SingleOrDefault(c => c.Iso.ToString().Trim() == iso.ToString().Trim());
        }

        private static bool validateDatas(string Code, string Name,string contact,string telefono1,string telefono2,string mobilphone,string Email,string adress,string codepost, string city, string provincia, string pais)
        {
            bool validateDatas = true;
            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("El nombre no puede estar en blanco", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                validateDatas = false;
            }
            else
            {
                if (string.IsNullOrEmpty(provincia) || string.IsNullOrEmpty(pais))
                {
                    if (string.IsNullOrEmpty(provincia))
                    {
                        MessageBox.Show("El campo Provincia no puede estar en blanco", "Campo Provincia", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(pais))
                        {
                            MessageBox.Show("El campo pais no puede estar en blanco", "Campo Pais", MessageBoxButton.OK, MessageBoxImage.Error);
                            validateDatas = false;
                        }
                    }
                }
                else
                {
                    if (contact.Length > 50)
                    {
                        MessageBox.Show("El Contacto no puede contener más de 50 caracteres", "Campo contacto", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {
                        if (telefono1.Length > 15 || string.IsNullOrEmpty(telefono1))
                        {
                            MessageBox.Show("El teléfono 1 no puede contener más de 15 caracteres o vacio", "Campo telefono 1", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                        if (adress.Length > 100)
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
            return validateDatas;
        }
    }
}
