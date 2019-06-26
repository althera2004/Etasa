using EtasaDesktop.Common;
using EtasaDesktop.Common.Tools;
using EtasaDesktop.Distribution.Clients;
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


namespace EtasaDesktop.Distribution.Planner
{
    class ColorViewModel : ViewModelBase
    {
       
        private ColorDataSet.ColorSummariesFactoriesDataTableRow _selectedFactoryColor;

        public ColorDataSet.ColorSummariesFactoriesDataTableRow SelectedFactoryColor
        {
            get => _selectedFactoryColor;
            set
            {
                Set(ref _selectedFactoryColor, value);
            }
        }
        

        public ObservableCollection<ColorDataSet.ColorSummariesFactoriesDataTableRow> ColorsFactory { get; private set; }

        public ColorViewModel()
        {
            
            ColorsFactory = new ObservableCollection<ColorDataSet.ColorSummariesFactoriesDataTableRow>();
            Refresh();
            
        }




        public void Refresh()
        {          
            ColorsFactory.Clear();

            ColorDataSet ds = new ColorDataSet();
            ColorDataSetTableAdapters.ColorSummariesFactoriesDataTableTableAdapter adapt = new ColorDataSetTableAdapters.ColorSummariesFactoriesDataTableTableAdapter();
            adapt.Fill(ds.ColorSummariesFactoriesDataTable);

            foreach (ColorDataSet.ColorSummariesFactoriesDataTableRow row in ds.ColorSummariesFactoriesDataTable.Rows)
            {
                ColorsFactory.Add(row);
            }
           
        }
    }
}
