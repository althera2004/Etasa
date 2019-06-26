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
using System.Windows.Controls;
using System.Data.SqlClient;

namespace EtasaDesktop.Common.Auth.Users
{
    public class UserFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }
        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        private int idusuario;
        private UserDataSet.System_UsersRow _row;
        private TextBox ContraseñaTextbox;
        private UserDataSet.System_Users_ObsRow _row_obs;


        public ObservableCollection<UserFormViewModel> User { get; private set; }

        public UserFormViewModel()
        {
            UserDataSet dataset = new UserDataSet();
            UserDataSetTableAdapters.System_UsersTableAdapter User = new UserDataSetTableAdapters.System_UsersTableAdapter();
            UserDataSetTableAdapters.System_Users_ObsTableAdapter User_obs = new UserDataSetTableAdapters.System_Users_ObsTableAdapter();

            _row = dataset.System_Users.NewSystem_UsersRow();
            _row_obs = dataset.System_Users_Obs.NewSystem_Users_ObsRow();

            //Usuario 
            _row.Name= "";
            _row.FullName = "";
            _row.Phone = "";
            _row.Email = "";
            _row.Password = "";
            _row.ModifiedDate = DateTime.Now;
            _row.CreatedDate = DateTime.Now;
            _row.Enabled = true;
            _row.Id = 0;

            //observaciones usuasrio
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

        public string FullName
        {
            get { return _row.FullName; }
            set
            {
                if (_row.FullName != value)
                {
                    _row.FullName = value;
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

        public string Password
        {
            get { return _row.Password; }
            set
            {
                if (_row.Password != value)
                {
                    _row.Password = value;
                    RaisePropertyChanged();
                }
            }
        }

        string _newpassword;
    

        public string NewPassword
        {
            get { return _newpassword; }
            set
            {
                if (_newpassword != value)
                {
                    _newpassword = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool CanSave()
        {
            return true;
        }

        public void Load(int IdUsuario, TextBox txtoldPassword)
        {
            try
            {
                ContraseñaTextbox = txtoldPassword;
                idusuario = IdUsuario;
                UserDataSet dataset = new UserDataSet();
                UserDataSetTableAdapters.System_UsersTableAdapter TableUser = new UserDataSetTableAdapters.System_UsersTableAdapter();
                UserDataSet.System_UsersDataTable dataTable = TableUser.GetDataUserById(IdUsuario);

                if (dataTable.Rows.Count > 0)
                {
                    UserDataSetTableAdapters.System_Users_ObsTableAdapter User_obs = new UserDataSetTableAdapters.System_Users_ObsTableAdapter();
                    UserDataSet.System_Users_ObsDataTable dataTable_Obs = User_obs.GetDataUser_ObsById(IdUsuario);

                    _row = (UserDataSet.System_UsersRow)dataTable.Rows[0];
                    
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Phone));
                    RaisePropertyChanged(nameof(FullName));
                    RaisePropertyChanged(nameof(Email));         
                    RaisePropertyChanged(nameof(Enabled));

                    _row_obs = (UserDataSet.System_Users_ObsRow)dataTable_Obs.Rows[0];
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
            int indentificadorUser = 0;
            string contraseña = "";

            try
            {
                
                UserDataSetTableAdapters.System_UsersTableAdapter TableUser = new UserDataSetTableAdapters.System_UsersTableAdapter();
                
                if (idusuario == 0)
                {
                    contraseña = _row.Password;
                }
                else
                {
                    if (string.IsNullOrEmpty(ContraseñaTextbox.Text.ToString()))
                    {
                        contraseña = Getpassword(idusuario);
                    }
                    else
                    {
                        contraseña = SHA256Encrypt(ContraseñaTextbox.Text.ToString());
                    }                                
                }
          
                validate = validateDatas(_row.Name, _row.FullName,_row.Phone,_row.Email, contraseña, TableUser,idusuario);

                if (validate)
                {
                    UserDataSet dataset = new UserDataSet();
                    UserDataSetTableAdapters.System_Users_ObsTableAdapter tableAdapterUserObs = new UserDataSetTableAdapters.System_Users_ObsTableAdapter();
               
                    UserDataSet.System_UsersDataTable dataTable = TableUser.GetDataUserById(_row.Id);

                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowuser = dataset.System_Users.NewSystem_UsersRow();

                        rowuser = dataTable[0];
                        rowuser["Name"] = _row.Name;
                        rowuser["Phone"] = _row.Phone;
                        rowuser["CreatedDate"] = DateTime.Now;
                        rowuser["ModifiedDate"] = DateTime.Now;
                        rowuser["FullName"] = _row.FullName;               
                        rowuser["Password"] = contraseña;                                   
                        rowuser["Enabled"] = _row.Enabled;

                        int numeroderegistros = TableUser.Update(rowuser);

                        //si se actualizado el user actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            tableAdapterUserObs.UpdateUserObs(_row_obs.Observations, _row.Id);
                        }
                    }
                    //insertamos el nuevo user  y las observaciones (user_obs)
                    else
                    {
                        TableUser = new UserDataSetTableAdapters.System_UsersTableAdapter();
                        //obtenemos de la tabla user  el nuevo registro que se introducira 
                        var rowUser = dataset.System_Users.NewSystem_UsersRow();

                        //Name
                        rowUser.Name = _row.Name;
                        //Phone
                        rowUser.Phone = _row.Phone;
                        //fecha creacion
                        rowUser.CreatedDate = DateTime.Now;
                        //fecha modificacion
                        rowUser.ModifiedDate = DateTime.Now;
                        //Fullname
                        rowUser.FullName = _row.FullName;
                        //Email
                        rowUser.Email = _row.Email;
                        //Password            
                        rowUser.Password = SHA256Encrypt(contraseña.ToString());
                        //enabled
                        rowUser.Enabled = _row.Enabled;

                        //agregamos la nueva fila 
                        dataset.System_Users.AddSystem_UsersRow(rowUser);
                        //actualizamos la tabla User en el dataset 
                        TableUser.Update(dataset.System_Users);

                        //al insertar el registro nuevo en la tabla User Id se actualiza 
                        indentificadorUser = Convert.ToInt32(rowUser.Id);

                        // a continuación insertamos el resto de datos en la tabla User_obs cepsa con el indentificador del User recient introduzido
                        tableAdapterUserObs = new UserDataSetTableAdapters.System_Users_ObsTableAdapter();
                        //obtenemos de la tabla User_obs el nuevo registro que se introducira 
                        var rowUser_obs = dataset.System_Users_Obs.NewSystem_Users_ObsRow();

                        //Id
                        rowUser_obs.Id = indentificadorUser;
                        //observations
                        rowUser_obs.Observations = _row_obs.Observations;

                        //agregamos la nueva fila a tabla User_obs
                        dataset.System_Users_Obs.AddSystem_Users_ObsRow(rowUser_obs);
                        //actualizamos la tabla User_obs  en el dataset 
                        tableAdapterUserObs.Update(dataset.System_Users_Obs);
                    }              
                    FormSaveFinished?.Invoke();
                }
            }
            catch (Exception e)
            {
                FormSaveError?.Invoke(e);
            }

        }

        public string Getpassword(int idusuario)
        {

            string contraseña = "";
            try
            {
                UserDataSet dataset = new UserDataSet();
                UserDataSetTableAdapters.System_UsersTableAdapter TableUser = new UserDataSetTableAdapters.System_UsersTableAdapter();
                UserDataSet.System_UsersDataTable dataTable = TableUser.GetDataUserById(idusuario);
                contraseña = dataTable[0]["Password"].ToString();
                return contraseña;
            }
            catch (Exception ex)
            {
                return contraseña;
            }
        }

        public bool validateDatas(string Name, string FullName, string phone, string Email,string password, UserDataSetTableAdapters.System_UsersTableAdapter TableUser, int idusuario)
        {
            bool validateDatas = true;
            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("El Nombre no puede estar en blanco", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                validateDatas = false;
            }
            else
            {
                if (Name.Length > 50)
                {
                    MessageBox.Show("El Nombre no puede contener mas de 50 caracters", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                    validateDatas = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(FullName))
                    {
                        MessageBox.Show("El nombre completo no puede estar en blanco", "Campo Nombre completo ", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {
                        if (FullName.Length > 100)
                        {
                            MessageBox.Show("El nombre completo no puede contener mas de 100 caracters", "Campo Nombre completo", MessageBoxButton.OK, MessageBoxImage.Error);
                            validateDatas = false;
                        }
                        else
                        {
                            if(phone.Length > 15)
                            {
                                MessageBox.Show("El teléfono no puede contener mas de 15 caracters", "Campo Telefono", MessageBoxButton.OK, MessageBoxImage.Error);
                                validateDatas = false;
                            }
                            else
                            {
                                if(Email.Length > 50)
                                {
                                    MessageBox.Show("El Email no puede contener mas de 50 caracters", "Campo Email", MessageBoxButton.OK, MessageBoxImage.Error);
                                    validateDatas = false;
                                } 
                                if (idusuario == 0)
                                {
                                    if(string.IsNullOrEmpty(password))
                                    {
                                        MessageBox.Show("La contraseña no puedo estar vacía", "Campo Contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
                                        validateDatas = false;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return validateDatas;
        }

        //Encriptador de contraseña a formato SHA256 
        public static string SHA256Encrypt(string input)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }
    }
}
