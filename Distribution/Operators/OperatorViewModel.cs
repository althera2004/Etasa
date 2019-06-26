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

namespace EtasaDesktop.Distribution.Operators
{
    class OperatorViewModel : ViewModelBase
    {   
        
        private OperatorDataset.OperatorSummariesRow _selectedOperator;

        public OperatorDataset.OperatorSummariesRow SelectedOperator
        {
            get => _selectedOperator;
            set
            {
                Set(ref _selectedOperator, value);
            }
        }
       

        public ObservableCollection<OperatorDataset.OperatorSummariesRow> Operators { get; private set; }

        public OperatorViewModel()
        {
            Operators = new ObservableCollection<OperatorDataset.OperatorSummariesRow>();
            Refresh();
        }


        public void Refresh()
        {

            Operators.Clear();

            OperatorDataset ds = new OperatorDataset();

            OperatorDatasetTableAdapters.OperatorSummariesTableAdapter adapt = new OperatorDatasetTableAdapters.OperatorSummariesTableAdapter();
            adapt.Fill(ds.OperatorSummaries);

            foreach (OperatorDataset.OperatorSummariesRow row in ds.OperatorSummaries.Rows)
            {
                Operators.Add(row);
            }

        }
    }
}
