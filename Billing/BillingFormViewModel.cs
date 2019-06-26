using EtasaDesktop.Common;
using EtasaDesktop.Common.Tools;
using EtasaDesktop.Billing;
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
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Drawing;
using System.Xaml;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace EtasaDesktop.Billing
{
    public class BillingFormViewModel : ViewModelBase
    {
        public ObservableCollection<BillingFormViewModel> Billing { get; private set; }
        public ObservableCollection<BillingDataSet.OrdersSummariesRow> Orders { get; private set; }
        public ObservableCollection<BillingDataSet.OrdersSummariesRow> Orders2 { get; private set; }
     
        public BillingFormViewModel()
        {
            Orders = new ObservableCollection<BillingDataSet.OrdersSummariesRow>();
            Orders2 = new ObservableCollection<BillingDataSet.OrdersSummariesRow>();
        }
        public void Load(DataGrid DatagridOrder, DataGrid DatagridOrderCepsa, BillingDataSet.OrdersSummariesDataTable TableOrders)
        {
            try
            {
                if (TableOrders.Rows.Count > 0)
                {
   
                    Orders.Clear();
                    Orders2.Clear();
                    DataRow[] result;
                    BillingDataSet.OrdersSummariesDataTable Orders2Datatable = new BillingDataSet.OrdersSummariesDataTable();

                    Orders2Datatable = createSecondList();
          
                    foreach (BillingDataSet.OrdersSummariesRow row in TableOrders.Rows)
                    {
                        result = Orders2Datatable.Select("Reference ='" + row["reference"].ToString() + "'");
                        // el pedido de la grid se encuentra en los pedidos de cepsa 
                        //miramos discrepancias si existen 
                        if(result.Count() > 0)
                        {
                            /*
                            var cellStyle = new Style { TargetType = typeof(DataGridCell) };
                            var cellTrigger = new Trigger { Property = DataGridCell.IsSelectedProperty, Value = true };
                            cellTrigger.Setters.Add(new Setter(DataGridCell.ForegroundProperty, System.Windows.Media.Brushes.Black));
                            cellTrigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, System.Windows.Media.Brushes.LightGray));
                            cellStyle.Triggers.Add(cellTrigger);

                            DatagridOrder.CellStyle = cellStyle;
                            DatagridOrderCepsa.CellStyle = cellStyle;
                            */

                            //si hay discrepancias agregamos a las dos grid el dato
                            if (result[0]["reference"].ToString().Trim() != row["reference"].ToString().Trim() || result[0]["Address"].ToString().Trim() != row["Address"].ToString().Trim() || Convert.ToInt32(result[0]["RequestedAmount"].ToString().Trim()) != Convert.ToInt32(row["RequestedAmount"].ToString().Trim()))
                            {
                               //rellenamos primera grid
                               Orders.Add(row);                 
                               //rellenamos segunda grid 
                               Orders2.Add(CreateRowOrderSummaries2(result));
                                //pintamos las discrepancias en las dos dataGrid
                          
                                //si la discrepancia es por la direccion
                                if(result[0]["Address"].ToString().Trim() != row["Address"].ToString().Trim())
                                {
                                    //DataRowView axel = DatagridOrder.Items[0] as DataRowView;
                                    //DataRow pedro = axel.Row;                    
                                    //pintamos la letra de toda la columna
                                    //DataGridColumn axel = DatagridOrder.Columns[1].GetCellContent(DatagridOrder.get);
                                    //DataGridCell pedro;  

                                    DataGridCell cell = GetCell(0, 0, DatagridOrder);                           
                                    cell.Background = new SolidColorBrush(Colors.Red);
                                  
                                    // DatagridOrderCepsa.Columns[1].CellStyle = (Style)XamlServices.Parse("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Foreground\" Value=\"Green\"></Setter></Style>");                                 
                                }

                                //si la discrepancia es por la cantidad 
                                if (Convert.ToInt32(result[0]["RequestedAmount"].ToString().Trim()) != Convert.ToInt32(row["RequestedAmount"].ToString().Trim()))
                                {

                                    DatagridOrder.Columns[2].CellStyle = (Style)XamlServices.Parse("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Binding={Binding Reference} TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Foreground\" Value=\"Green\"></Setter></Style>");
                                    DataGridCell cell = GetCell(0, 0, DatagridOrder);
                                    cell.Background = new SolidColorBrush(Colors.Red);
                                    //DataRowView axel = DatagridOrder.Items[0] as DataRowView;
                                    //DataRow pedro = axel.Row;                    
                                    //pintamos la letra de toda la columna 
                                  
                                    //DatagridOrderCepsa.Columns[2].CellStyle = (Style)XamlServices.Parse("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Foreground\" Value=\"Green\"></Setter></Style>");
                                }
                            }                             
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public DataGridCell GetCell(int rowIndex, int columnIndex, DataGrid dg)
        {

            DataGridRow row = dg.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;     
            DataGridCellsPresenter p = GetVisualChild<DataGridCellsPresenter>(row);
            DataGridCell cell = p.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
            return cell;
        }

        static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }


        //creamos lista estatica (pedidos que provienen de cepsa)
        public BillingDataSet.OrdersSummariesDataTable createSecondList()
        {
            BillingDataSet ds = new BillingDataSet();
            BillingDataSetTableAdapters.OrdersSummariesTableAdapter adapt = new BillingDataSetTableAdapters.OrdersSummariesTableAdapter();
            BillingDataSet.OrdersSummariesDataTable orderSummaries2 = new BillingDataSet.OrdersSummariesDataTable();
            var rowOrder_summarie = ds.OrdersSummaries.NewOrdersSummariesRow();

            //primer registro 
            rowOrder_summarie.Reference = "CEG182411474887";
            rowOrder_summarie.Code = "0002";
            rowOrder_summarie.Address = "Indocin2";
            rowOrder_summarie.Name = "jose";
            rowOrder_summarie.RequestedAmount = 817;
            rowOrder_summarie.TankVolume = 45;
            rowOrder_summarie.TankLevel = 56;
            rowOrder_summarie.OperatorId = 1000;
            rowOrder_summarie.Latitude = 1000;
            rowOrder_summarie.Longitude = 1000;
            rowOrder_summarie.ClientId = 7138;
            rowOrder_summarie.StartDate = DateTime.Now;
            rowOrder_summarie.FinalDate = DateTime.Now;
            rowOrder_summarie.FactoryId = 13;
            rowOrder_summarie.ProductId = 355;
            rowOrder_summarie.VehicleSize = 8;
            //agregamos primer registro 
            ds.OrdersSummaries.AddOrdersSummariesRow(rowOrder_summarie);

            //segundo registro
            var rowOrder_summarie2 = ds.OrdersSummaries.NewOrdersSummariesRow();         
            rowOrder_summarie2.Reference = "PV18009779";
            rowOrder_summarie2.Code = "0001";
            rowOrder_summarie2.Address = "AV LOPEZ BLANCO SN";
            rowOrder_summarie2.Name = "jose";
            rowOrder_summarie2.RequestedAmount = 34;
            rowOrder_summarie2.TankVolume = 45;
            rowOrder_summarie2.TankLevel = 56;
            rowOrder_summarie2.OperatorId = 1000;
            rowOrder_summarie2.Latitude = 1000;
            rowOrder_summarie2.Longitude = 1000;
            rowOrder_summarie2.ClientId = 7138;
            rowOrder_summarie2.StartDate = DateTime.Now;
            rowOrder_summarie2.FinalDate = DateTime.Now;
            rowOrder_summarie2.FactoryId = 13;
            rowOrder_summarie2.ProductId = 355;
            rowOrder_summarie2.VehicleSize = 8;
            //agregamos segundo registro 
            ds.OrdersSummaries.AddOrdersSummariesRow(rowOrder_summarie2);

            //guardamos la tabla 
            orderSummaries2 = ds.OrdersSummaries;

            //devolvemos la tabla
            return orderSummaries2;
        }

        //convertimos las fila de la array de row en una row tipo ordersSummarierow
        public BillingDataSet.OrdersSummariesRow CreateRowOrderSummaries2(DataRow[] OrderSummaries2)
        {

            BillingDataSet ds = new BillingDataSet();
            BillingDataSetTableAdapters.OrdersSummariesTableAdapter adapt = new BillingDataSetTableAdapters.OrdersSummariesTableAdapter();
            var rowOrder_summarie = ds.OrdersSummaries.NewOrdersSummariesRow();
            rowOrder_summarie.Reference = OrderSummaries2[0]["Reference"].ToString().Trim();
            rowOrder_summarie.Code = OrderSummaries2[0]["Code"].ToString().Trim();
            rowOrder_summarie.Name = OrderSummaries2[0]["Name"].ToString().Trim();
            rowOrder_summarie.Address = OrderSummaries2[0]["Address"].ToString().Trim();
            rowOrder_summarie.RequestedAmount = Convert.ToInt32(OrderSummaries2[0]["RequestedAmount"]);
            rowOrder_summarie.TankVolume = Convert.ToDouble(OrderSummaries2[0]["TankVolume"]);
            rowOrder_summarie.TankLevel = Convert.ToInt32(OrderSummaries2[0]["TankLevel"]);
            rowOrder_summarie.OperatorId = Convert.ToInt32(OrderSummaries2[0]["OperatorId"]);
            rowOrder_summarie.Latitude = float.Parse(OrderSummaries2[0]["Latitude"].ToString());
            rowOrder_summarie.Longitude = float.Parse(OrderSummaries2[0]["Longitude"].ToString());
            rowOrder_summarie.StartDate = Convert.ToDateTime(OrderSummaries2[0]["StartDate"].ToString());
            rowOrder_summarie.FinalDate = Convert.ToDateTime(OrderSummaries2[0]["FinalDate"].ToString());
            rowOrder_summarie.FactoryId = Convert.ToInt32(OrderSummaries2[0]["FactoryId"]);
            rowOrder_summarie.ProductId = Convert.ToInt32(OrderSummaries2[0]["ProductId"]);
            rowOrder_summarie.VehicleSize = Convert.ToInt32(OrderSummaries2[0]["VehicleSize"]);

            return rowOrder_summarie;
        }
    }
}
