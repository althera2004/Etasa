using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json;
using EtasaDesktop.Common.Tools;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para ExistingOrders.xaml
    /// </summary>
    public partial class OrdersListFrame : FrameControl
    {
        public static string sTripId { get; set; }
        public static string cmdtxt;

        public OrdersListFrame()
        {
            InitializeComponent();
        }


        public override void Refresh()
        {
            FillDataGrid();

            ColorDataGridFunction(); // DEshabilitada solo para pruebas
            if (dgSimple.Items.Count != 0)
            {
                FunctionVisibilityDataGrid();
            }
        }


        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            } 
        }

        private void FillDataGrid()
        {
            try
            {
                string sFilterSQL = "";
                sFilterSQL = FilterAllSql();

                string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
                string CmdString = string.Empty;
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    CmdString = "SELECT * FROM ListOrders";
                    cmdtxt = CmdString;
                    CmdString = CmdString + sFilterSQL; // no poner uno por uno los campos, ya está en la vista ordenados
                    // revisar sFilterSQL en el join */
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable("ExistingOrders");
                    sda.Fill(dt);
                    dgSimple.ItemsSource = dt.DefaultView;
                }

                TranslateColumnNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR\n" + ex.ToString());
            }
        }

        public void ColorDataGridFunction()
        {
            try
            {
                dgSimple.ItemContainerGenerator.StatusChanged += (s, e) =>
                {
                    if (dgSimple.ItemContainerGenerator.Status ==
                                        GeneratorStatus.ContainersGenerated)
                    {
                        foreach (DataRowView drv in (DataView)dgSimple.ItemsSource)
                        {
                            try
                            {
                                var row = dgSimple.ItemContainerGenerator.ContainerFromItem(drv) as DataGridRow;

                                if (drv["Status"].ToString() != "" || (drv["Status"] != null))
                                {
                                    if ((int)drv["Status"] == 1) // fallo al hacer join
                                    {
                                        if (row != null)
                                            row.Background = new SolidColorBrush(Color.FromRgb(117, 160, 208));
                                    }
                                    else if ((int)drv["Status"] == 2)
                                    {
                                        if (row != null)
                                            row.Background = new SolidColorBrush(Color.FromRgb(200, 199, 217));
                                    }
                                    else if ((int)drv["Status"] == 3)
                                    {
                                        if (row != null)
                                            row.Background = new SolidColorBrush(Color.FromRgb(195, 227, 249));
                                    }
                                    else if((int)drv["Status"] == 4)
                                    {
                                        if (row != null)
                                            row.Background = new SolidColorBrush(Color.FromRgb(255, 193, 116));
                                    }
                                    else if((int)drv["Status"] == 5)
                                    {
                                        if (row != null)
                                            row.Background = new SolidColorBrush(Color.FromRgb(219, 242, 207));
                                    }
                                    else if ((int)drv["Status"] == 7)
                                    {
                                        if (row != null)
                                            row.Background = new SolidColorBrush(Color.FromRgb(251, 161, 149));
                                    }
                                    else if ((int)drv["Status"] == 8)
                                    {
                                        if (row != null)
                                            row.Background = new SolidColorBrush(Color.FromRgb(250, 250, 250));
                                    }
                                    else if ((int)drv["Status"] == 9)
                                    {
                                        if (row != null)
                                            row.Background = new SolidColorBrush(Color.FromRgb(174, 179, 185));
                                    }
                                    else
                                    {
                                        if (row != null)
                                            row.Background = Brushes.White;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR\n" + ex.ToString());
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR\n" + ex.ToString());
            }
        }

        public string FilterBranchOfficesOperator()
        {
            string sFilterBranchOperator = "";

            if ((BranchOfficeTextBox.Text != "") && (BranchOfficeTextBox.Text != ""))
            {
                sFilterBranchOperator = sFilterBranchOperator + " ProductName LIKE '%" + BranchOfficeTextBox.Text.ToString() + "%'";
            }
            if ((OperatorTextBox.Text != "") && (OperatorTextBox.Text != ""))
            {
                if ((BranchOfficeTextBox.Text != "") && (BranchOfficeTextBox.Text != ""))
                {
                    sFilterBranchOperator = sFilterBranchOperator + " AND ";
                }

                sFilterBranchOperator = sFilterBranchOperator + " OperatorName LIKE '%" + OperatorTextBox.Text.ToString() + "%'";
            }

            return sFilterBranchOperator;
        }

        public string FilterFactoryCustomer()
        {
            string sFilterFactoryCustomer = "";

            if ((FactoryTextBox.Text != "") && (FactoryTextBox.Text != ""))
            {
                sFilterFactoryCustomer = sFilterFactoryCustomer + " FactoryName LIKE '%" + FactoryTextBox.Text.ToString() + "%'";
            }
            if ((CustomerTextBox.Text != "") && (CustomerTextBox.Text != ""))
            {
                if ((FactoryTextBox.Text != "") && (FactoryTextBox.Text != ""))
                {
                    sFilterFactoryCustomer = sFilterFactoryCustomer + " AND ";
                }

                sFilterFactoryCustomer = sFilterFactoryCustomer + " ClientName LIKE '%" + CustomerTextBox.Text.ToString() + "%'";
            }

            return sFilterFactoryCustomer;
        }

        public string FunctionFilterDates()
        {
            string sFilterDate = "";
            DatesFilterList dfl = new DatesFilterList();

            if ((DatePickerFrom.Text != "") && (DatePickerFrom.Text != null))
            {
                sFilterDate = sFilterDate + "StartDate >= '" + DatePickerFrom.Text.ToString() + "'";
            }
            if ((DatePickerTo.Text != "" ) && (DatePickerTo.Text != null))
            {
                if ((DatePickerFrom.Text != "") && (DatePickerFrom.Text != null))
                {
                    sFilterDate = sFilterDate + " AND ";
                }
                sFilterDate = sFilterDate + "FinalDate <= '" + DatePickerTo.Text.ToString() + "'";
            }

            return sFilterDate;
        }

        public string FunctionFilterReferenceDeliveryNote()
        {
            string sFilterRefDelivery = "";

            if ((DeliveryNoteTbox.Text != "") && (DeliveryNoteTbox.Text != ""))
            {
                sFilterRefDelivery = sFilterRefDelivery + ""; // no hay albarán en la tabla
            }
            if ((ReferenceTbox.Text != "") && (ReferenceTbox.Text != ""))
            {
                if ((DeliveryNoteTbox.Text != "") && (DeliveryNoteTbox.Text != ""))
                {
                    sFilterRefDelivery = sFilterRefDelivery + " AND ";
                }

                sFilterRefDelivery = sFilterRefDelivery + " Reference LIKE '%" + ReferenceTbox.Text.ToString() + "%'";
            }

            return sFilterRefDelivery;
        }

        public string FunctionFilterTruckTrailer()
        {
            string sFilterTruckTrailer = "";

            if ((TruckTextBox.Text != "") && (TruckTextBox.Text != ""))
            {
                sFilterTruckTrailer = sFilterTruckTrailer +" CabCode LIKE '%" + TruckTextBox.Text.ToString() + "%'"; // no es correcto
            }
            if ((TrailerTextBox.Text != "") && (TrailerTextBox.Text != ""))
            {
                if ((TruckTextBox.Text != "") && (TruckTextBox.Text != ""))
                {
                    sFilterTruckTrailer = sFilterTruckTrailer + " AND ";
                }

                sFilterTruckTrailer = sFilterTruckTrailer + " TrailerCode LIKE '%" + TrailerTextBox.Text.ToString() + "%'"; // no es correcto
            }

            return sFilterTruckTrailer;
        }

        public string FunctionFilterDriverRented()
        {
            string sFilterDriverRented = "";

            if ((DriverTextBox.Text != "") && (DriverTextBox.Text != ""))
            {
                sFilterDriverRented = sFilterDriverRented + " DriverName LIKE '%" + DriverTextBox.Text.ToString() + "%'";
            }
            if ((RentedTextBox.Text != "") && (RentedTextBox.Text != ""))
            {
                if ((DriverTextBox.Text != "") && (DriverTextBox.Text != ""))
                {
                    sFilterDriverRented = sFilterDriverRented + " OR "; // AND???
                }

                sFilterDriverRented = sFilterDriverRented + " DriverName LIKE '%" + RentedTextBox.Text.ToString() + "%'";
            }

            return sFilterDriverRented;
        }

        //Metodo actual para ocultar mostrar las columnas que el usuario haya selecionado en el datagrid 

        private void VisibilityDataGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                CmdString = @"SELECT UserConfig FROM System_User_Column_Config WHERE Id = 1";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string config = dr["UserConfig"].ToString();

                        if (!string.IsNullOrEmpty(config))
                        {
                            var columnconfig = JsonConvert.DeserializeObject<List<UserColumnConfig>>(config);

                            foreach (DataGridColumn column in dgSimple.Columns)
                            {
                                string columname = column.Header.ToString();

                                foreach (var userconfig in columnconfig)
                                {
                                    if (columname == userconfig.columnname && userconfig.esvisible == false)
                                    {
                                        column.Visibility = Visibility.Collapsed;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //Metodos antiguos de Dani para ocultar la visibilidad de las columnas basandose en la configuracion del usuario

        private void FunctionVisibilityDataGrid(int iIdUser = 0)
        {
            iIdUser = 1;
            int iPosicion = 0;
            bool bValor = true;

            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = @"SELECT * 
                            FROM System_Column_Configs
                            WHERE IdUser = " + iIdUser;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable("ColumnCheckBoxes");
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        bValor = row[column].ToString().Equals("True");

                        if (!bValor)
                        {
                            FunctionHideVisibility(bValor, iPosicion);
                        }
                        else
                        {
                            FunctionShowVisibility(bValor, iPosicion);
                        }

                        iPosicion++;
                    }
                }
            }
        }

        public void FunctionHideVisibility(bool bVal, int iPos)
        {
            switch(iPos)
            {
                case 19:
                    this.dgSimple.Columns[32].Visibility = Visibility.Collapsed;
                    break;
                case 20:
                    //
                    this.dgSimple.Columns[1].Visibility = Visibility.Collapsed;
                    break;
                case 25:
                    this.dgSimple.Columns[53].Visibility = Visibility.Collapsed;
                    break;
                case 30:
                    this.dgSimple.Columns[72].Visibility = Visibility.Collapsed;
                    break;
                case 32:
                    this.dgSimple.Columns[5].Visibility = Visibility.Collapsed;
                    break;
                case 33:
                    this.dgSimple.Columns[4].Visibility = Visibility.Collapsed;
                    break;
                case 35:
                    this.dgSimple.Columns[3].Visibility = Visibility.Collapsed;
                    break;
                case 36:
                    this.dgSimple.Columns[33].Visibility = Visibility.Collapsed;
                    break;
                case 37:
                    this.dgSimple.Columns[36].Visibility = Visibility.Collapsed;
                    break;
                case 39:
                    this.dgSimple.Columns[9].Visibility = Visibility.Collapsed;
                    break;
                case 40:
                    this.dgSimple.Columns[56].Visibility = Visibility.Collapsed;
                    break;
                case 41:
                    this.dgSimple.Columns[84].Visibility = Visibility.Collapsed;
                    break;
                case 43:
                    this.dgSimple.Columns[2].Visibility = Visibility.Collapsed;
                    break;
                case 63:
                    this.dgSimple.Columns[86].Visibility = Visibility.Collapsed;
                    break;
                case 76:
                    this.dgSimple.Columns[0].Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        public void FunctionShowVisibility(bool bVal, int iPos)
        {
            switch (iPos)
            {
                case 19:
                    this.dgSimple.Columns[32].Visibility = Visibility.Visible;
                    break;
                case 20:
                    //
                    this.dgSimple.Columns[1].Visibility = Visibility.Visible;
                    break;
                case 25:
                    this.dgSimple.Columns[53].Visibility = Visibility.Visible;
                    break;
                case 30:
                    this.dgSimple.Columns[72].Visibility = Visibility.Visible;
                    break;
                case 32:
                    this.dgSimple.Columns[5].Visibility = Visibility.Visible;
                    break;
                case 33:
                    this.dgSimple.Columns[4].Visibility = Visibility.Visible;
                    break;
                case 35:
                    this.dgSimple.Columns[3].Visibility = Visibility.Visible;
                    break;
                case 36:
                    this.dgSimple.Columns[33].Visibility = Visibility.Visible;
                    break;
                case 37:
                    this.dgSimple.Columns[36].Visibility = Visibility.Visible;
                    break;
                case 39:
                    this.dgSimple.Columns[9].Visibility = Visibility.Visible;
                    break;
                case 40:
                    this.dgSimple.Columns[56].Visibility = Visibility.Visible;
                    break;
                case 41:
                    this.dgSimple.Columns[84].Visibility = Visibility.Visible;
                    break;
                case 43:
                    this.dgSimple.Columns[2].Visibility = Visibility.Visible;
                    break;
                case 63:
                    this.dgSimple.Columns[86].Visibility = Visibility.Visible;
                    break;
                case 76:
                    this.dgSimple.Columns[0].Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();

            ColorDataGridFunction(); // DEshabilitada solo para pruebas
            if (dgSimple.Items.Count != 0)
            {
                //FunctionVisibilityDataGrid();
                VisibilityDataGrid();
            }
        }

        private void ColumnConfiguration_Click(object sender, RoutedEventArgs e)
        {
            /*ColumnConfigurationWindow cConfiguration = new ColumnConfigurationWindow();
            cConfiguration.ShowDialog();
            
            if (dgSimple.Items.Count != 0)
            {
                FunctionVisibilityDataGrid();
            }*/

            ColumnFilter columnfilter = new ColumnFilter();
            columnfilter.ShowDialog();
        }

        private void ExportDatagridExcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Excel.Application xlApp;
                Workbook xlWorkBook;
                Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

                for (int j = 0; j <= dgSimple.Columns.Count - 1; j++)
                {
                    string columnName = dgSimple.Columns[j].Header.ToString();
                    xlWorkSheet.Cells[1, j + 1] = columnName;
                }

                for (int i = 0; i <= dgSimple.Items.Count - 1; i++)
                {
                    for (int j = 0; j <= dgSimple.Columns.Count - 1; j++)
                    {
                        var elemento = ((DataRowView)dgSimple.Items[i]).Row.ItemArray[j];
                        xlWorkSheet.Cells[i + 2, j + 1] = elemento;
                    }
                }

                System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
                saveDlg.InitialDirectory = @"C:\";
                saveDlg.Filter = "Excel files (*.xls)|*.xls";
                saveDlg.FilterIndex = 0;
                saveDlg.RestoreDirectory = true;
                saveDlg.Title = "Exportar pedidos en Excel a";
                if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = saveDlg.FileName;
                    xlWorkBook.SaveCopyAs(path);
                    xlWorkBook.Saved = true;
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();
                }
            }
            catch
            {
                MessageBox.Show("Esta opción no esta disponible. Comprueba que tengas el Microsoft Office instalado.");
            }
            

        }

        private void FreightDeliveryReturn_Click(object sender, RoutedEventArgs e)
        {
            //FreightDeliveryReturnWindow FDR_Window = new FreightDeliveryReturnWindow();
            //FDR_Window.ShowDialog();
            //
            DataRowView drv = (DataRowView)dgSimple.SelectedItem;
            if (drv != null)
            {
                String result = (drv["TripId"]).ToString(); // Id
                sTripId = result;

                //return true;
            }
            else
            {
                //return false;
            }

            FreightDeliveryReturnWindow FDR_Window = new FreightDeliveryReturnWindow();
            FDR_Window.ShowDialog();
        }

        private void SearchExistingOperators_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingOrdersWindow sExisting = new SearchExistingOrdersWindow();
            sExisting.ShowDialog();
            OperatorTextBox.Text = SearchExistingOrdersWindow.sPropertyOperatorText;
        }

        private void SearchExistingBranchOffices_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingBranchOfficeWindow sBranch = new SearchExistingBranchOfficeWindow();
            sBranch.ShowDialog();
            BranchOfficeTextBox.Text = SearchExistingBranchOfficeWindow.sPropertyBranchOfficeText;
        }

        private void SearchExistingFactories_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingFactoryWindow sFactories = new SearchExistingFactoryWindow();
            sFactories.ShowDialog();
            FactoryTextBox.Text = SearchExistingFactoryWindow.sPropertyName;
        }

        private void SearchExistingCustomers_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingCustomersWindow sCustomers = new SearchExistingCustomersWindow();
            sCustomers.ShowDialog();
            CustomerTextBox.Text = SearchExistingCustomersWindow.sPropertyCustomerText;
        }

        private void SearchExistingTruck_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingTruckWindow sTruck = new SearchExistingTruckWindow();
            sTruck.ShowDialog();
            TruckTextBox.Text = SearchExistingTruckWindow.sPropertyCode;
        }

        private void SearchExistingTrailer_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingTrailerWindow sTrailer = new SearchExistingTrailerWindow();
            sTrailer.ShowDialog();
            TrailerTextBox.Text = SearchExistingTrailerWindow.sPropertyCode;
        }

        private void SearchExistingDrivers_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingDriversWindow sDrivers = new SearchExistingDriversWindow();
            sDrivers.ShowDialog();
            DriverTextBox.Text = SearchExistingDriversWindow.sPropertyName;
        }

        private void SearchExistingRented_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingRentedWindow sRented = new SearchExistingRentedWindow();
            sRented.ShowDialog();
            RentedTextBox.Text = SearchExistingRentedWindow.sPropertyRentedText;
        }

        

        private void LostFocus_Test(object sender, RoutedEventArgs e)
        {
            
        }

        private void MouseLeftButtonUp_TestEvent(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void MouseUp__TestEvent(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void TodosCB_Change(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBoxFilterList cbfl = new CheckBoxFilterList();
                cbfl.Todos = (TodosCB.IsChecked == true);

                if (Sin_ProgramarCB != null)
                Sin_ProgramarCB.IsChecked = cbfl.Todos;

                if (Sin_EntregarCB != null)
                Sin_EntregarCB.IsChecked = cbfl.Todos;

                if (AnuladoCB != null)
                AnuladoCB.IsChecked = cbfl.Todos;

                if (Sin_CargarCB != null)
                Sin_CargarCB.IsChecked = cbfl.Todos;

                if (Con_CompraCB != null)
                Con_CompraCB.IsChecked = cbfl.Todos;

                if(Sin_FacturarCB != null)
                Sin_FacturarCB.IsChecked = cbfl.Todos;

                if(Con_Clave_RechazoCB != null)
                Con_Clave_RechazoCB.IsChecked = cbfl.Todos;

                if (IncidenciaCB != null)
                    IncidenciaCB.IsChecked = cbfl.Todos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR\n" + ex.ToString());
            }
        }

        private void Sin_ProgramarCB_Change(object sender, RoutedEventArgs e)
        {
            CheckBoxFilterList cbfl = new CheckBoxFilterList();
            cbfl.Sin_Programar = (Sin_ProgramarCB.IsChecked == true);

            if ((Sin_ProgramarCB.IsChecked == false) && (Sin_EntregarCB.IsChecked == false) && (AnuladoCB.IsChecked == false) && (Sin_CargarCB.IsChecked == false) && (Con_CompraCB.IsChecked == false) && (Sin_FacturarCB.IsChecked == false) && (Con_Clave_RechazoCB.IsChecked == false) && (IncidenciaCB.IsChecked == false))
                TodosCB.IsChecked = false;
        }

        private void Sin_EntregarCB_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxFilterList cbfl = new CheckBoxFilterList();
            cbfl.Sin_Entregar = (Sin_EntregarCB.IsChecked == true);

            if ((Sin_ProgramarCB.IsChecked == false) && (Sin_EntregarCB.IsChecked == false) && (AnuladoCB.IsChecked == false) && (Sin_CargarCB.IsChecked == false) && (Con_CompraCB.IsChecked == false) && (Sin_FacturarCB.IsChecked == false) && (Con_Clave_RechazoCB.IsChecked == false) && (IncidenciaCB.IsChecked == false))
                TodosCB.IsChecked = false;
        }

        private void AnuladoCB_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxFilterList cbfl = new CheckBoxFilterList();
            cbfl.Anulado = (AnuladoCB.IsChecked == true);

            if ((Sin_ProgramarCB.IsChecked == false) && (Sin_EntregarCB.IsChecked == false) && (AnuladoCB.IsChecked == false) && (Sin_CargarCB.IsChecked == false) && (Con_CompraCB.IsChecked == false) && (Sin_FacturarCB.IsChecked == false) && (Con_Clave_RechazoCB.IsChecked == false) && (IncidenciaCB.IsChecked == false))
                TodosCB.IsChecked = false;
        }

        private void Sin_CargarCB_Change(object sender, RoutedEventArgs e)
        {
            CheckBoxFilterList cbfl = new CheckBoxFilterList();
            cbfl.Sin_Cargar = (Sin_CargarCB.IsChecked == true);

            if ((Sin_ProgramarCB.IsChecked == false) && (Sin_EntregarCB.IsChecked == false) && (AnuladoCB.IsChecked == false) && (Sin_CargarCB.IsChecked == false) && (Con_CompraCB.IsChecked == false) && (Sin_FacturarCB.IsChecked == false) && (Con_Clave_RechazoCB.IsChecked == false) && (IncidenciaCB.IsChecked == false))
                TodosCB.IsChecked = false;
        }

        private void Con_CompraCB_Change(object sender, RoutedEventArgs e)
        {
            CheckBoxFilterList cbfl = new CheckBoxFilterList();
            cbfl.Con_Compra = (Con_CompraCB.IsChecked == true);

            if ((Sin_ProgramarCB.IsChecked == false) && (Sin_EntregarCB.IsChecked == false) && (AnuladoCB.IsChecked == false) && (Sin_CargarCB.IsChecked == false) && (Con_CompraCB.IsChecked == false) && (Sin_FacturarCB.IsChecked == false) && (Con_Clave_RechazoCB.IsChecked == false) && (IncidenciaCB.IsChecked == false))
                TodosCB.IsChecked = false;
        }

        private void Sin_FacturarCB_Change(object sender, RoutedEventArgs e)
        {
            CheckBoxFilterList cbfl = new CheckBoxFilterList();
            cbfl.Sin_Facturar = (Sin_FacturarCB.IsChecked == true);

            if ((Sin_ProgramarCB.IsChecked == false) && (Sin_EntregarCB.IsChecked == false) && (AnuladoCB.IsChecked == false) && (Sin_CargarCB.IsChecked == false) && (Con_CompraCB.IsChecked == false) && (Sin_FacturarCB.IsChecked == false) && (Con_Clave_RechazoCB.IsChecked == false) && (IncidenciaCB.IsChecked == false))
                TodosCB.IsChecked = false;
        }

        private void Con_Clave_RechazoCB_Change(object sender, RoutedEventArgs e)
        {
            CheckBoxFilterList cbfl = new CheckBoxFilterList();
            cbfl.Con_Clave_Rechazo = (Con_Clave_RechazoCB.IsChecked == true);

            if ((Sin_ProgramarCB.IsChecked == false) && (Sin_EntregarCB.IsChecked == false) && (AnuladoCB.IsChecked == false) && (Sin_CargarCB.IsChecked == false) && (Con_CompraCB.IsChecked == false) && (Sin_FacturarCB.IsChecked == false) && (Con_Clave_RechazoCB.IsChecked == false) && (IncidenciaCB.IsChecked == false))
                TodosCB.IsChecked = false;
        }
        
        private void IncidenciaCB_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxFilterList cbfl = new CheckBoxFilterList();
            cbfl.IncidenciaCB = (IncidenciaCB.IsChecked == true);

            if ((Sin_ProgramarCB.IsChecked == false) && (Sin_EntregarCB.IsChecked == false) && (AnuladoCB.IsChecked == false) && (Sin_CargarCB.IsChecked == false) && (Con_CompraCB.IsChecked == false) && (Sin_FacturarCB.IsChecked == false) && (Con_Clave_RechazoCB.IsChecked == false) && (IncidenciaCB.IsChecked == false))
                TodosCB.IsChecked = false;
        }


        public string MethodFilterSelect ()
        {
            string sFilter = "(";

            CheckBoxFilterList cbfl = new CheckBoxFilterList();

            cbfl.Sin_Programar = (Sin_ProgramarCB.IsChecked == true);
            cbfl.Sin_Entregar = (Sin_EntregarCB.IsChecked == true);
            cbfl.Anulado = (AnuladoCB.IsChecked == true);
            cbfl.Todos = (TodosCB.IsChecked == true);
            cbfl.Sin_Cargar = (Sin_CargarCB.IsChecked == true);
            cbfl.Con_Compra = (Con_CompraCB.IsChecked == true);
            cbfl.Sin_Facturar = (Sin_FacturarCB.IsChecked == true);
            cbfl.Con_Clave_Rechazo = (Con_Clave_RechazoCB.IsChecked == true);
            cbfl.IncidenciaCB = (IncidenciaCB.IsChecked == true);

            if (cbfl.Sin_Programar)
            {
                sFilter = sFilter + " Status = 1 ";
            }
            if (cbfl.Sin_Entregar)
            {
                if (cbfl.Sin_Programar)
                {
                    sFilter = sFilter + " OR ";
                }
                sFilter = sFilter + " Status = 2 ";
            }
            if (cbfl.Anulado)
            {
                if (cbfl.Sin_Programar || cbfl.Sin_Entregar)
                {
                    sFilter = sFilter + " OR ";
                }
                sFilter = sFilter + " Status = 8 ";
            }
            if (cbfl.Sin_Cargar)
            {
                if (cbfl.Sin_Programar || cbfl.Sin_Entregar || cbfl.Anulado)
                {
                    sFilter = sFilter + " OR ";
                }
                sFilter = sFilter + " Status = 3 ";
            }
            if (cbfl.Con_Compra)
            {
                if (cbfl.Sin_Programar || cbfl.Sin_Entregar || cbfl.Anulado || cbfl.Sin_Cargar)
                {
                    sFilter = sFilter + " OR ";
                }
                sFilter = sFilter + " Status = 4 ";
            }
            if (cbfl.Sin_Facturar)
            {
                if (cbfl.Sin_Programar || cbfl.Sin_Entregar || cbfl.Anulado || cbfl.Sin_Cargar || cbfl.Con_Compra)
                {
                    sFilter = sFilter + " OR ";
                }
                sFilter = sFilter + " Status = 5 ";
            }
            if (cbfl.Con_Clave_Rechazo)
            {
                if (cbfl.Sin_Programar || cbfl.Sin_Entregar || cbfl.Anulado || cbfl.Sin_Cargar || cbfl.Con_Compra || cbfl.Sin_Facturar)
                {
                    sFilter = sFilter + " OR ";
                }
                sFilter = sFilter + " Status = 9 ";
            }

            if (cbfl.IncidenciaCB)
            {
                if (cbfl.Sin_Programar || cbfl.Sin_Entregar || cbfl.Anulado || cbfl.Sin_Cargar || cbfl.Con_Compra || cbfl.Sin_Facturar || cbfl.Con_Clave_Rechazo)
                {
                    sFilter = sFilter + " OR ";
                }
                sFilter = sFilter + " Status = 7 ";
            }

            if (!cbfl.Sin_Programar && !cbfl.Sin_Entregar && cbfl.Anulado && cbfl.Sin_Cargar && cbfl.Con_Compra && cbfl.Sin_Facturar && cbfl.Con_Clave_Rechazo & cbfl.Con_Clave_Rechazo)
            {
                sFilter = "";
            }

            sFilter += ")";

            if (sFilter == "()")
                sFilter = "";

            return sFilter;
        }

        public string FilterAllSql()
        {
            string sFilterSQL = "", sFiltersSelect = "", sFiltersDate = "", sFilterDeliveryReference = "", sFilterBranchOfficeOperator = "", sFilterFactoryCustomer = "", sFilterTruckTrailer = "", sFilterDriverRented = "";

            sFiltersSelect = MethodFilterSelect();
            bool bFiltersSelect = ((sFiltersSelect != "") && (sFiltersSelect != null));
            if (bFiltersSelect)
            {
                sFilterSQL = " WHERE " + sFiltersSelect;
            }

            sFiltersDate = FunctionFilterDates();
            bool bFiltersDate = ((sFiltersDate != "") && (sFiltersDate != null));
            if (bFiltersDate)
            {
                if (bFiltersSelect)
                {
                    sFilterSQL = sFilterSQL + " AND " + sFiltersDate;
                }
                else
                {
                    sFilterSQL = " WHERE " + sFiltersDate;
                }
            }

            sFilterDeliveryReference = FunctionFilterReferenceDeliveryNote();
            bool bFilterDeliveryReference = ((sFilterDeliveryReference != "") && (sFilterDeliveryReference != null));
            if (bFilterDeliveryReference)
            {
                if (bFiltersSelect || bFiltersDate)
                {
                    sFilterSQL = sFilterSQL + " AND " + sFilterDeliveryReference;
                }
                else
                {
                    sFilterSQL = " WHERE " + sFilterDeliveryReference;
                }
            }

            sFilterBranchOfficeOperator = FilterBranchOfficesOperator();
            bool bFilterBranchOfficeOperator = ((sFilterBranchOfficeOperator != "") && (sFilterBranchOfficeOperator != null));
            if (bFilterBranchOfficeOperator)
            {
                if (bFiltersSelect || bFiltersDate || bFilterDeliveryReference)
                {
                    sFilterSQL = sFilterSQL + " AND " + sFilterBranchOfficeOperator;
                }
                else
                {
                    sFilterSQL = " WHERE " + sFilterBranchOfficeOperator;
                }
            }

            sFilterFactoryCustomer = FilterFactoryCustomer();
            bool bFilterFactoryCustomer = ((sFilterFactoryCustomer != "") && (sFilterFactoryCustomer != null));
            if (bFilterFactoryCustomer)
            {
                if (bFiltersSelect || bFiltersDate || bFilterDeliveryReference || bFilterBranchOfficeOperator)
                {
                    sFilterSQL = sFilterSQL + " AND " + sFilterFactoryCustomer;
                }
                else
                {
                    sFilterSQL = " WHERE " + sFilterFactoryCustomer;
                }
            }

            sFilterTruckTrailer = FunctionFilterTruckTrailer();
            bool bFilterTruckTrailer = ((sFilterTruckTrailer != "") && (sFilterTruckTrailer != null));
            if (bFilterTruckTrailer)
            {
                if (bFiltersSelect || bFiltersDate || bFilterDeliveryReference || bFilterBranchOfficeOperator || bFilterFactoryCustomer)
                {
                    sFilterSQL = sFilterSQL + " AND " + sFilterTruckTrailer;
                }
                else
                {
                    sFilterSQL = " WHERE " + sFilterTruckTrailer;
                }
            }
            
            sFilterDriverRented = FunctionFilterDriverRented();
            bool bFilterDriverRented = ((sFilterDriverRented != "") && (sFilterDriverRented != null));
            if (bFilterDriverRented)
            {
                if (bFiltersSelect || bFiltersDate || bFilterDeliveryReference || bFilterBranchOfficeOperator || bFilterFactoryCustomer || bFilterTruckTrailer)
                {
                    sFilterSQL = sFilterSQL + " AND " + sFilterDriverRented;
                }
                else
                {
                    sFilterSQL = " WHERE " + sFilterDriverRented;
                }
            }

            return sFilterSQL;
        }

        private void dgSimple_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditElementsWindow eEdit = new EditElementsWindow();
            eEdit.ShowDialog();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }

        private void dgSimple_Loaded(object sender, RoutedEventArgs e)
        {
            TranslateColumnNames();
        }

        private void TranslateColumnNames()
        {
            for (int i = 0; i < dgSimple.Columns.Count; i++)
            {
                string columnname = dgSimple.Columns[i].Header.ToString();
                string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(ConString))
                {
                    con.Open();
                    string CmdString = string.Empty;
                    CmdString = @"SELECT * FROM System_Data_Column_Config WHERE DatabaseField = @DatabaseField and Visible = 'True'";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = CmdString;
                    cmd.Parameters.AddWithValue("@DatabaseField", columnname);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string databasecolumn = dr["DatabaseField"].ToString();

                            if (columnname == databasecolumn)
                            {
                                dgSimple.Columns[i].Header = dr["Description"].ToString();
                            }
                        }
                    }

                    else
                    {
                        dgSimple.Columns[i].Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        
    }

    public class CheckBoxFilterList
    {
        public bool Sin_Programar { get; set; }
        public bool Sin_Entregar { get; set; }
        public bool Anulado { get; set; }
        public bool Todos { get; set; }
        public bool Sin_Cargar { get; set; }
        public bool Con_Compra { get; set; }
        public bool Sin_Facturar { get; set; }
        public bool Con_Clave_Rechazo { get; set; }
        public bool IncidenciaCB { get; set; }
    }

    public class DatesFilterList
    {
        public DateTime StartDateFilter { get; set; }
        public DateTime EndDateFilter { get; set; }
    }

    public class ReferenceFilter
    {
        public string sDeliveryNote { get; set; }
        public string sReference { get; set; }
    }
}
