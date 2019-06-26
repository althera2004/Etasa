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
using EtasaDesktop.Distribution.Vehicles.VehiclesNew;

namespace EtasaDesktop.Distribution.Vehicles.VehiclesNew
{
    class VehiclesViewModel : ViewModelBase
    {
        private VehicleDataSet1.VehiclesSummariesRow _selectedVehicle;


        public VehicleDataSet1.VehiclesSummariesRow SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                Set(ref _selectedVehicle, value);
            }
        }


        public ObservableCollection<VehicleDataSet1.VehiclesSummariesRow> Vehicles { get; private set; }

        public VehiclesViewModel()
        {
            Vehicles = new ObservableCollection<VehicleDataSet1.VehiclesSummariesRow>();
            Refresh();
        }


        public void Refresh()
        {

            Vehicles.Clear();

            VehicleDataSet1 ds = new VehicleDataSet1();
            VehicleDataSet1TableAdapters.VehiclesSummariesTableAdapter adapt = new VehicleDataSet1TableAdapters.VehiclesSummariesTableAdapter();
            adapt.Fill(ds.VehiclesSummaries);

            foreach (VehicleDataSet1.VehiclesSummariesRow row in ds.VehiclesSummaries.Rows)
            {
                Vehicles.Add(row);
            }

        }
    }
}
