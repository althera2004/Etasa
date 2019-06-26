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

namespace EtasaDesktop.Distribution.Clients
{
    class ClientViewModel : ViewModelBase
    {    
        private ClientDataSet.ClientSummariesRow _selectedClient;

        public ClientDataSet.ClientSummariesRow SelectedClient
        {
            get => _selectedClient;
            set
            {
                Set(ref _selectedClient, value);
            }
        }

        public ObservableCollection<ClientDataSet.ClientSummariesRow> Clients { get; private set; }

        public ClientViewModel()
        {
            Clients = new ObservableCollection<ClientDataSet.ClientSummariesRow>();
            Refresh();
        }


        public void Refresh()
        {

            Clients.Clear();

            ClientDataSet ds = new ClientDataSet();
           
            ClientDataSetTableAdapters.ClientSummariesTableAdapter adapt = new ClientDataSetTableAdapters.ClientSummariesTableAdapter();
            adapt.Fill(ds.ClientSummaries);

            foreach (ClientDataSet.ClientSummariesRow row in ds.ClientSummaries.Rows)
            {
                Clients.Add(row);
            }

        }
    }
}
