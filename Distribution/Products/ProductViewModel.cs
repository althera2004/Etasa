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


namespace EtasaDesktop.Distribution.Products
{
    class ProductViewModel : ViewModelBase
    {
        private ProductDataSet.ProductsSummariesRow _selectedProduct;


        public ProductDataSet.ProductsSummariesRow SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                Set(ref _selectedProduct, value);
            }
        }


        public ObservableCollection<ProductDataSet.ProductsSummariesRow> Products { get; private set; }

        public ProductViewModel()
        {
            Products = new ObservableCollection<ProductDataSet.ProductsSummariesRow> ();
            Refresh();
        }


        public void Refresh()
        {

            Products.Clear();

            ProductDataSet ds = new ProductDataSet();
            ProductDataSetTableAdapters.ProductsSummariesTableAdapter adapt = new ProductDataSetTableAdapters.ProductsSummariesTableAdapter();
            adapt.Fill(ds.ProductsSummaries);

            foreach (ProductDataSet.ProductsSummariesRow row in ds.ProductsSummaries.Rows)
            {
                Products.Add(row);
            }

        }
    }
}
