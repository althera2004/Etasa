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
using System.Configuration;
using System.Data.SqlClient;

namespace EtasaDesktop.Billing
{
    class BillingViewModel : ViewModelBase
    {    
      
        private BillingDataSet.OrdersRow _selectedOrder;
        public DateTime _selectedDate;
        public BillingDataSet.OrdersRow SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                Set(ref _selectedOrder, value);
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                Set(ref _selectedDate, value);
                Refresh();
            }
        }


        public ObservableCollection<BillingDataSet.OrdersSummariesRow> Orders { get; private set; }

        public BillingViewModel()
        {
            Orders = new ObservableCollection<BillingDataSet.OrdersSummariesRow>();
            SelectedDate = DateTime.Today;
            Refresh();
        }

        public void Refresh()
        {
            BillingDataSet ds = new BillingDataSet();
            BillingDataSetTableAdapters.OrdersSummariesTableAdapter adapt = new BillingDataSetTableAdapters.OrdersSummariesTableAdapter();
            adapt.Fill(ds.OrdersSummaries);
            Orders.Clear();
            foreach (BillingDataSet.OrdersSummariesRow row in ds.OrdersSummaries.Rows)
            {
                Orders.Add(row);
            }

        }

        //función para filtrar los datos desde el portal
        public BillingDataSet.OrdersSummariesDataTable Filter(DateTime? StartDate, DateTime? FinalDate, string CodeOperator, string Reference, BillingDataSet.OrdersSummariesDataTable dataTable)
        {

            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            string sFilterSQL = "";
            DataTable dt = new DataTable();
            BillingDataSet ds = new BillingDataSet();
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //datos a devolver
                CmdString = "SELECT Orders.Id, Orders.Reference, Orders.OperatorId, Orders.ClientId, Orders.StartDate, Orders.FinalDate, Orders.DeliveryDate, Orders.CreatedDate, Orders.ModifiedDate, Orders.Address, Orders.City, Orders.PostCode,"+ 
                         " Orders.Province, Orders.Country, Orders.Latitude, Orders.Longitude, Orders.FactoryId, Orders.ProductId, Orders.VehicleSize, Orders.RequestedAmount, Orders.ReceivedAmount, Orders.TankNum, Orders.TankVolume,"+
                         " Orders.TankLevel, Orders.Status, Operators.Code, Operators.Name FROM Orders INNER JOIN Operators ON Orders.OperatorId = Operators.Id";

                //filtro por fechas 
                if(StartDate !=null && FinalDate !=null) 
                {
                    if(StartDate> FinalDate)
                    {
                        MessageBox.Show("La fecha inicial no puede ser superior a la final (no se aplica filtro por fechas)");
                    }
                    else
                    {
                        sFilterSQL = "Orders.StartDate >='" + StartDate.ToString() + "' AND Orders.FinalDate <= '" + FinalDate.ToString() + "'";
                    }
                }

                //filtro por operador 
                if(CodeOperator !="")
                {
                    if (sFilterSQL =="")
                    {
                        sFilterSQL = "Operators.Code = '" + CodeOperator.ToString().Trim() + "'";
                    }
                    else
                    {
                        sFilterSQL = sFilterSQL + " And Operators.Code = '" + CodeOperator.Trim().ToString() +  "'";
                    }
                }

                //filtro por referencia 
                if (Reference != "")
                {
                    if (sFilterSQL == "")
                    {
                        sFilterSQL = "Orders.Reference = '" + Reference.Trim().ToString() + "'";
                    }
                    else
                    {                      
                        sFilterSQL = sFilterSQL + " And Orders.Reference ='" + Reference.Trim().ToString() + "'";
                    }
                }
                //miramos si hay algun dato a filtrar
                if(sFilterSQL !="")
                {
                    CmdString = CmdString + " WHERE " + sFilterSQL;
                }

                //obtenemos la tabla con los datos a mostrar
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds.OrdersSummaries);
                //limpiamos el portal
                Orders.Clear();
                dataTable = ds.OrdersSummaries;
                //recorremos la tabla con las filas 
                foreach (BillingDataSet.OrdersSummariesRow row in ds.OrdersSummaries.Rows)
                {
                    Orders.Add(row);
                }                         
            }

            return dataTable;
        }
    }
}
