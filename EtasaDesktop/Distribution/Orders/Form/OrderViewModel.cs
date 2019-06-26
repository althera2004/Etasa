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


namespace EtasaDesktop.Distribution.Orders.Form
{
    class OrderViewModel : ViewModelBase
    {
        private OrderDataSet.OrderSummariesRow _selectedOrder;

        
        public OrderDataSet.OrderSummariesRow SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                Set(ref _selectedOrder, value);
            }
        }


        public ObservableCollection<OrderDataSet.OrderSummariesRow> Order { get; private set; }

        public OrderViewModel()
        {
            Order = new ObservableCollection<OrderDataSet.OrderSummariesRow> ();
            Refresh();
        }


        public void Refresh()
        {

            Order.Clear();

            OrderDataSet ds = new OrderDataSet();
            OrderDataSetTableAdapters.OrderSummariesTableAdapter adapt = new OrderDataSetTableAdapters.OrderSummariesTableAdapter();
            adapt.Fill(ds.OrderSummaries);

            foreach (OrderDataSet.OrderSummariesRow row in ds.OrderSummaries.Rows)
            {
                Order.Add(row);
            }

        }
    }
}
