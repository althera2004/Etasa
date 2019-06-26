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

namespace EtasaDesktop.Distribution.Planner.Colors
{
    public class ColorFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        //private ColorDataSet.Factories_ColorsRow _row;

        //private ColorDataSet.Factories_ColorsRow _selectedColor_Factory;

        public ObservableCollection<ColorFormViewModel> Color { get; private set; }

        public ColorFormViewModel()
        {
            /*
            ColorDataSet dataset = new ColorDataSet();
            ColorDataSetTableAdapters.Factories_ColorsTableAdapter FactoryColors = new ColorDataSetTableAdapters.Factories_ColorsTableAdapter();

            _row = dataset.Factories_Colors.NewFactories_ColorsRow();

            //Conductor
            _row.ClientColor = "";
            _row.FactoryColor = "";    
            _row.PreferenceColor = "";
            _row.UrgentColor = "";
            _row.FinalDayColor = "";
            _row.Id = 0;
            */

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        /*
        public string ClientColor
        {
            get { return _row.ClientColor; }
            set
            {
                if (_row.ClientColor != value)
                {
                    _row.ClientColor = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string FactoryColor
        {
            get { return _row.FactoryColor; }
            set
            {
                if (_row.FactoryColor != value)
                {
                    _row.ClientColor = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string PreferenceColor
        {
            get { return _row.PreferenceColor; }
            set
            {
                if (_row.PreferenceColor != value)
                {
                    _row.PreferenceColor = value;
                    RaisePropertyChanged();
                }
            }
        }



        public string UrgentColor
        {
            get { return _row.UrgentColor; }
            set
            {
                if (_row.UrgentColor != value)
                {
                    _row.UrgentColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FinalDayColor
        {
            get { return _row.FinalDayColor; }
            set
            {
                if (_row.FinalDayColor != value)
                {
                    _row.FinalDayColor = value;
                    RaisePropertyChanged();
                }
            }
        }
        */
        

        private bool CanSave()
        {
            return true;
        }

        public void Load(int IdColor)
        {
            /*
            try
            {
                
                ColorDataSet dataset = new ColorDataSet();
                ColorDataSetTableAdapters.Factories_ColorsTableAdapter TableFactoriesColors = new ColorDataSetTableAdapters.Factories_ColorsTableAdapter();
                ColorDataSet.Factories_ColorsDataTable dataTable = TableFactoriesColors.GetDataColorsById(IdColor);


                if (dataTable.Rows.Count > 0)
                {
      
                    _row = (ColorDataSet.Factories_ColorsRow)dataTable.Rows[0];

                    RaisePropertyChanged(nameof(ClientColor));
                    RaisePropertyChanged(nameof(FactoryColor));
                    RaisePropertyChanged(nameof(PreferenceColor));
                    RaisePropertyChanged(nameof(UrgentColor));
                    RaisePropertyChanged(nameof(FinalDayColor));         
                    
                }
                FormLoadFinished?.Invoke();
                
            }
            catch (Exception e)
            {
                FormLoadError?.Invoke(e);
            }
            */
        }

        public void Save()
        {
            /*
            bool validate = true;
            try
            {
                ColorDataSet dataset = new ColorDataSet();
                ColorDataSetTableAdapters.Factories_ColorsTableAdapter TableFactoriesColors = new ColorDataSetTableAdapters.Factories_ColorsTableAdapter();


                // validate = validateDatas(_row.Code, _row.Dni, _row.Name, _row.Address, _row.City, _row.Phone,this.firsTimeDni,this.firsTimeCode, dataset, TableDriver,_row.StartDate.ToString(),_row.FinishDate.ToString());

                if (validate)
                {
                    ColorDataSet.Factories_ColorsDataTable dataTable = TableFactoriesColors.GetDataColorsById(_row.Id);

                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowFactorisColors = dataset.Factories_Colors.NewFactories_ColorsRow();
                        rowFactorisColors = dataTable[0];
                        rowFactorisColors["ClientColor"] = _row.ClientColor;
                        rowFactorisColors["FactoryColor"] = _row.FactoryColor;
                        rowFactorisColors["PreferenceColor"] = _row.PreferenceColor;
                        rowFactorisColors["UrgentColor"] = _row.UrgentColor;
                        rowFactorisColors["FinalDayColor"] = _row.FinalDayColor;

                        int numeroderegistros = TableFactoriesColors.Update(rowFactorisColors);

                    }
                    else
                    {
                        TableFactoriesColors = new ColorDataSetTableAdapters.Factories_ColorsTableAdapter();
                        //obtenemos de la tabla factories_colors  el nuevo registro que se introducira 
                        var rowFactoriesColors = dataset.Factories_Colors.NewFactories_ColorsRow();

                        //ClientColor
                        rowFactoriesColors.ClientColor = _row.ClientColor;
                        //FactoryColor
                        rowFactoriesColors.FactoryColor = _row.FactoryColor;
                        //PreferenceColor
                        rowFactoriesColors.PreferenceColor = _row.PreferenceColor;
                        //UrgentColor
                        rowFactoriesColors.UrgentColor = _row.UrgentColor;
                        //FinalDayColor
                        rowFactoriesColors.FinalDayColor = _row.FinalDayColor;

                        //agregamos la nueva fila 
                        dataset.Factories_Colors.AddFactories_ColorsRow(rowFactoriesColors);
                        //actualizamos la tabla factories_colors en el dataset 
                        TableFactoriesColors.Update(dataset.Factories_Colors);
                       
                    }

                    FormSaveFinished?.Invoke();
                }
            }
            catch (Exception e)
            {

                FormSaveError?.Invoke(e);
            }
            */
        }

  
        /*
        private static bool validateDatas(string Code, string Dni,string Name,string adress,string city,string phone, bool FirstimeDni,bool FirstTimeCode, DriversDataSet dataset, DriversDataSetTableAdapters.DriversTableAdapter tableDriver,string StartDate , string FinalDate)
        {
            bool validateDatas = true;
            int NumberRows = 0;
            DateTime StartDateValue;
            DateTime FinalDateValue;
            if (string.IsNullOrEmpty(Code))
            {
                MessageBox.Show("El campo Código no puede estar en blanco", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
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
            return validateDatas;
        }
        */
    }
}
