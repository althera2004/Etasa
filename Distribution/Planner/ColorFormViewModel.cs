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
using System.Windows.Media;
using System.Linq;
using EtasaDesktop.Distribution.Clients;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace EtasaDesktop.Distribution.Planner
{
    public class ColorFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        private TextBox GlobaltextboxFactoryColor;
        private TextBox GlobaltextboxClientColor;
        private TextBox GlobaltextboxUrgentColor;
        private TextBox GlobaltextboxFinalDayColor;
        private TextBox GlobaltextboxPreferenceColor;
        private TextBox GlobaltextboxTheLastPreferent;


        private ColorDataSet.Factories_ColorsRow _row;

        private ColorDataSet.FactoriesRow _rowName;
        private ColorDataSet.FactoriesRow _rowCode;

        private ColorDataSet.Factories_ColorsRow _selectedColor_Factory;

        public ObservableCollection<ColorDataSet.FactoriesRow> factories { get; private set; }

        public ObservableCollection<ColorFormViewModel> Color { get; private set; }

        public ColorFormViewModel()
        {

            ColorDataSet dataset = new ColorDataSet();
            ColorDataSetTableAdapters.Factories_ColorsTableAdapter FactoryColors = new ColorDataSetTableAdapters.Factories_ColorsTableAdapter();
            ColorDataSetTableAdapters.FactoriesTableAdapter factoryname = new ColorDataSetTableAdapters.FactoriesTableAdapter();

            _row = dataset.Factories_Colors.NewFactories_ColorsRow();

            _rowName = dataset.Factories.NewFactoriesRow();
            _rowCode = dataset.Factories.NewFactoriesRow();
            //colores factorias 
            _row.ClientColor = "";
            _row.FactoryColor = "";
            _row.PreferenceColor = "";
            _row.UrgentColor = "";
            _row.FinalDayColor = "";
            _row.TheLastPreferent = "";
            _row.Id = 0;

            //nombre factoría
            _rowName.Name = "";
            //codigo factoría
            _rowCode.Code = "";


            SaveCommand = new RelayCommand(Save, CanSave);
        }



        public string Name
        {
            get { return _rowName.Name; }
            set
            {
                if (_rowName.Name != value)
                {
                    _rowName.Name = value;
                    RaisePropertyChanged();
                }
            }
        }



        public string Code
        {
            get { return _rowCode.Code; }
            set
            {
                if (_rowCode.Code != value)
                {
                    _rowCode.Code = value;
                    RaisePropertyChanged();
                }
            }
        }

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


        public string TheLastPreferent
        {
            get { return _row.TheLastPreferent; }
            set
            {
                if (_row.TheLastPreferent != value)
                {
                    _row.TheLastPreferent = value;
                    RaisePropertyChanged();
                }
            }
        }



        private bool CanSave()
        {
            return true;
        }

        public void Load(int IdColor, TextBox textboxFactoryColor, TextBox textboxClientColor, TextBox textboxUrgentColor, TextBox textboxFinalDayColor, TextBox textboxPreferenceColor, TextBox textboxTheLastPreferent, ColorPicker ClrPcker_Background, ColorPicker ClrPcker_Background1, ColorPicker ClrPcker_Background2, ColorPicker ClrPcker_Background3, ColorPicker ClrPcker_Background4, ColorPicker ClrPcker_Background5)
        {
            AssignTextBox(textboxFactoryColor,textboxClientColor,textboxUrgentColor,textboxFinalDayColor,textboxPreferenceColor,textboxTheLastPreferent);

            try
            {

                ColorDataSet dataset = new ColorDataSet();
                ColorDataSetTableAdapters.Factories_ColorsTableAdapter TableFactoriesColors = new ColorDataSetTableAdapters.Factories_ColorsTableAdapter();
                ColorDataSet.Factories_ColorsDataTable dataTable = TableFactoriesColors.GetDataColorsById(IdColor);


                if (dataTable.Rows.Count > 0)
                {
                    ColorDataSetTableAdapters.FactoriesTableAdapter factoryname = new ColorDataSetTableAdapters.FactoriesTableAdapter();
                    ColorDataSet.FactoriesDataTable dataTable_name = factoryname.GetDataById(IdColor);


                    _row = (ColorDataSet.Factories_ColorsRow)dataTable.Rows[0];

                    RaisePropertyChanged(nameof(ClientColor));
                    RaisePropertyChanged(nameof(FactoryColor));
                    RaisePropertyChanged(nameof(PreferenceColor));
                    RaisePropertyChanged(nameof(UrgentColor));
                    RaisePropertyChanged(nameof(FinalDayColor));
                    RaisePropertyChanged(nameof(TheLastPreferent));

                    //incializamos las barras con el color
                    textboxFactoryColor.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_row.FactoryColor.ToString().Trim()));
                    textboxClientColor.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_row.ClientColor.ToString().Trim()));
                    textboxUrgentColor.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_row.UrgentColor.ToString().Trim()));
                    textboxFinalDayColor.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_row.FinalDayColor.ToString().Trim()));
                    textboxPreferenceColor.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_row.PreferenceColor.ToString().Trim()));
                    textboxTheLastPreferent.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_row.TheLastPreferent.ToString().Trim()));

                    _rowName = (ColorDataSet.FactoriesRow)dataTable_name.Rows[0];
                    _rowCode = (ColorDataSet.FactoriesRow)dataTable_name.Rows[0];
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Code));
                }
                FormLoadFinished?.Invoke();

            }
            catch (Exception e)
            {
                FormLoadError?.Invoke(e);
            }

        }
        public void AssignTextBox(TextBox textboxFactoryColor, TextBox textboxClientColor, TextBox textboxUrgentColor, TextBox textboxFinalDayColor, TextBox textboxPreferenceColor, TextBox textboxTheLastPreferent)
        {
            GlobaltextboxFactoryColor = textboxFactoryColor;
            GlobaltextboxClientColor = textboxClientColor;
            GlobaltextboxUrgentColor = textboxUrgentColor;
            GlobaltextboxFinalDayColor = textboxFinalDayColor;
            GlobaltextboxPreferenceColor = textboxPreferenceColor;
            GlobaltextboxTheLastPreferent = textboxTheLastPreferent;
        }

        public void Save()
        {

            bool validate = true;
            try
            {
                ColorDataSet dataset = new ColorDataSet();
                ColorDataSetTableAdapters.Factories_ColorsTableAdapter TableFactoriesColors = new ColorDataSetTableAdapters.Factories_ColorsTableAdapter();

                if (validate)
                {
                    ColorDataSet.Factories_ColorsDataTable dataTable = TableFactoriesColors.GetDataColorsById(_row.Id);

                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowFactorisColors = dataset.Factories_Colors.NewFactories_ColorsRow();
                        rowFactorisColors = dataTable[0];
                        rowFactorisColors["ClientColor"] = GlobaltextboxClientColor.Text.Trim();
                        rowFactorisColors["FactoryColor"] = GlobaltextboxFactoryColor.Text.Trim();
                        rowFactorisColors["PreferenceColor"] = GlobaltextboxPreferenceColor.Text.Trim();
                        rowFactorisColors["UrgentColor"] = GlobaltextboxUrgentColor.Text.Trim();
                        rowFactorisColors["FinalDayColor"] = GlobaltextboxFinalDayColor.Text.Trim();
                        rowFactorisColors["TheLastPreferent"] = GlobaltextboxTheLastPreferent.Text.Trim();

                        int numeroderegistros = TableFactoriesColors.Update(rowFactorisColors);
     

                    }
                    /*
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
                        //thelastPreference
                        rowFactoriesColors.TheLastPreferent = _row.TheLastPreferent;

                        //agregamos la nueva fila 
                        dataset.Factories_Colors.AddFactories_ColorsRow(rowFactoriesColors);
                        //actualizamos la tabla factories_colors en el dataset 
                        TableFactoriesColors.Update(dataset.Factories_Colors);

                    }
                    */

                    FormSaveFinished?.Invoke();
                }
            }
            catch (Exception e)
            {

                FormSaveError?.Invoke(e);
            }

        }
    }
     
}
