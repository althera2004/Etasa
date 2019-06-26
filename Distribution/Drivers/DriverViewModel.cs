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


namespace EtasaDesktop.Distribution.Drivers
{
    class DriverViewModel : ViewModelBase
    {
        private DriversDataSet.DriversSummariesRow _selectedDriver;


        public DriversDataSet.DriversSummariesRow SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                Set(ref _selectedDriver, value);
            }
        }


        public ObservableCollection<DriversDataSet.DriversSummariesRow> Drivers { get; private set; }

        public DriverViewModel()
        {
            Drivers = new ObservableCollection<DriversDataSet.DriversSummariesRow>();
            Refresh();
        }


        public void Refresh()
        {

            Drivers.Clear();

            DriversDataSet ds = new DriversDataSet();
            DriversDataSetTableAdapters.DriversSummariesTableAdapter adapt = new DriversDataSetTableAdapters.DriversSummariesTableAdapter();
            adapt.Fill(ds.DriversSummaries);

            foreach (DriversDataSet.DriversSummariesRow row in ds.DriversSummaries.Rows)
            {
                Drivers.Add(row);
            }

        }
    }
}
