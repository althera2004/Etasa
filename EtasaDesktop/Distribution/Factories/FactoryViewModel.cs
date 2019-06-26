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


namespace EtasaDesktop.Distribution.Factories
{
    class FactoryViewModel : ViewModelBase
    {
        private FactoryDataSet.FactoriesSummariesRow _selectedFactory;


        public FactoryDataSet.FactoriesSummariesRow SelectedFactory
        {
            get => _selectedFactory;
            set
            {
                Set(ref _selectedFactory, value);
            }
        }


        public ObservableCollection<FactoryDataSet.FactoriesSummariesRow> Factories { get; private set; }

        public FactoryViewModel()
        {
            Factories = new ObservableCollection<FactoryDataSet.FactoriesSummariesRow> ();
            Refresh();
        }


        public void Refresh()
        {

            Factories.Clear();

            FactoryDataSet ds = new FactoryDataSet();
            FactoryDataSetTableAdapters.FactoriesSummariesTableAdapter adapt = new FactoryDataSetTableAdapters.FactoriesSummariesTableAdapter();
            adapt.Fill(ds.FactoriesSummaries);

            foreach (FactoryDataSet.FactoriesSummariesRow row in ds.FactoriesSummaries.Rows)
            {
                Factories.Add(row);
            }

        }
    }
}
