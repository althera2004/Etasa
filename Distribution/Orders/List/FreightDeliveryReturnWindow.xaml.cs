using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Windows.Controls;

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para FreightDeliveryReturnWindow.xaml
    /// </summary>
    public partial class FreightDeliveryReturnWindow : Window
    {
        string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;

        private int idCab;
        private int idTrailer;
        private int idDriver;
        private int idFactory;

        public FreightDeliveryReturnWindow()
        {
            InitializeComponent();
            FillFreightDeliveryReturnGrid();
            FillTextBoxes();
        }

        private void FreightDeliveryReturnGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.IsReadOnly = true;

            if (e.Column.Header.ToString() == "RequestedAmount")
            {
                e.Column.IsReadOnly = false;
            }

            if  (e.Column.Header.ToString() == "FinalDate")
            {
                e.Column.IsReadOnly = false;
            }

            if (e.Column.Header.ToString() == "Llegada")
            {
                e.Column.IsReadOnly = false;
            }

            if (e.Column.Header.ToString() == "Salida")
            {
                e.Column.IsReadOnly = false;
            }
        }

        private void FillFreightDeliveryReturnGrid()
        {
            if ((OrdersListFrame.sTripId == null) || (OrdersListFrame.sTripId == ""))
            {
                OrdersListFrame.sTripId = "1";
            }

            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //CmdString = @"SELECT * 
                //            FROM AsignacionBackupNuevo
                //            ORDER BY Fecha DESC";
                CmdString = @"Select '' AS TripPosition, ClientCode, ClientName, RequestedAmount, ReceivedAmount, StartDate, FinalDate, Status FROM ListOrders where TripId = " + OrdersListFrame.sTripId;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ExistingOrders");
                sda.Fill(dt);
                FreightDeliveryReturnGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void FillTextBoxes()
        {
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                CmdString = @"SELECT CabCode, CabLicensePlate, TrailerCode, TrailerLicensePlate, DriverCode, DriverName, FactoryCode, FactoryName, LoadedDate, StartDate, FinalDate, Status FROM ListOrders
                WHERE TripId = '" + OrdersListFrame.sTripId + "'";
                SqlCommand cmd = new SqlCommand(CmdString);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtCabCode.Text = dr["CabCode"].ToString();
                    txtCabPlate.Text = dr["CabLicensePlate"].ToString();
                    txtTrailerCode.Text = dr["TrailerCode"].ToString();
                    txtTrailerPlate.Text = dr["TrailerLicensePlate"].ToString();
                    txtDriverCode.Text = dr["DriverCode"].ToString();
                    txtDriverName.Text = dr["DriverName"].ToString();
                    txtFactoryCode.Text = dr["FactoryCode"].ToString();
                    txtFactoryName.Text = dr["FactoryName"].ToString();
                    String date = dr["LoadedDate"].ToString();
                    if(!String.IsNullOrEmpty(date) )
                        DatePicker.SelectedDate = Convert.ToDateTime(dr["LoadedDate"].ToString());
                    
                    if(int.Parse(dr["Status"].ToString()) >= 3)
                        LoadedCheckBox.IsEnabled = true;
                    else
                        LoadedCheckBox.IsEnabled = false;
                    
                }

                con.Close();
                con.Open();
                CmdString = @"SELECT Id, RouteId, CreatedDate, ModifiedDate, Position, LoadedAmount, LoadedDate FROM Trips
                WHERE Id = '" + OrdersListFrame.sTripId + "'";
                SqlCommand cmd2 = new SqlCommand(CmdString);
                cmd2.Connection = con;
                SqlDataReader dr2 = cmd2.ExecuteReader();
                while (dr2.Read())
                {
                    txtViajeId.Text = dr2["Position"].ToString();
                    txtLoadedAmount.Text = dr2["LoadedAmount"].ToString();

                }

                con.Close();
            }
        }

        public class FreightDeliveryReturn
        {
            public static int[] CreateListTime(int iSelect)
            {
                int iTime = 0;
                int[] iArray = new int[iSelect];

                while(iTime < iSelect)
                {
                    iArray[iTime] = iTime + 1;
                    iTime++;
                };

                return iArray;
            }
        }

        private void CloseFreightDeliveryReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // el portal actualiza la factoria del pedido mediante el identificador del viaje
        private void SaveOrdersTripButton_Click(object sender, RoutedEventArgs e)
        {
            string CmdString = string.Empty;

            if (txtFactoryName.Text == "La factoria no existe...")
            {
                MessageBox.Show("Por favor introduce una factoria");
                return;
            }

            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                //CmdString = @"UPDATE Orders SET FactoryId = @FactoryId WHERE TripId = '" + OrdersListFrame.sTripId + "'";
                CmdString = @"UPDATE O SET O.FactoryId = @FactoryId FROM Orders AS O INNER JOIN trips AS T ON T.Id_Order = O.Id WHERE T.Id = '" + OrdersListFrame.sTripId + "'";
                SqlCommand cmd = new SqlCommand(CmdString);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@FactoryId", idFactory);
                cmd.CommandText = CmdString;
                int actualizado = cmd.ExecuteNonQuery();
                if (actualizado > 0)
                {
                    MessageBox.Show("Se han actualizado los datos");
                }
                else
                {
                    MessageBox.Show("No se han podido actualizar los datos");
                }
            }
        }

        private void txtFactoriaCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string FactoryCode = txtFactoryCode.Text;

            string CmdString = string.Empty;

            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                CmdString = @"Select Name FROM Factories WHERE Code = @FactoryCode";
                SqlCommand cmd = new SqlCommand(CmdString);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@FactoryCode", FactoryCode);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string FactoryName = dr["Name"].ToString();

                        if (!string.IsNullOrEmpty(FactoryName))
                        {
                            txtFactoryName.Text = FactoryName;
                        }
                    }
                }

                else
                {
                    txtFactoryName.Text = "La factoria no existe...";
                }
            }
        }

        private void SearchExistingFactories_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingFactoryWindow sFactories = new SearchExistingFactoryWindow();
            sFactories.ShowDialog();
            idFactory = SearchExistingFactoryWindow.sPropertyId;
            txtFactoryCode.Text = SearchExistingFactoryWindow.sPropertyCode;
            txtFactoryName.Text = SearchExistingFactoryWindow.sPropertyName;
        }

        private void SearchExistingTrailer_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingTrailerWindow sTrailer = new SearchExistingTrailerWindow();
            sTrailer.ShowDialog();
            idTrailer = SearchExistingTrailerWindow.sPropertyId;
            txtTrailerCode.Text = SearchExistingTrailerWindow.sPropertyCode;
            txtTrailerPlate.Text = SearchExistingTrailerWindow.sPropertyLicensePlate;
        }

        private void SearchExistingTruck_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingTruckWindow sTruck = new SearchExistingTruckWindow();
            sTruck.ShowDialog();
            idCab = SearchExistingTruckWindow.sPropertyId;
            txtCabCode.Text = SearchExistingTruckWindow.sPropertyCode;
            txtCabPlate.Text = SearchExistingTruckWindow.sPropertyLicensePlate;
        }

        private void SearchExistingDrivers_Click(object sender, RoutedEventArgs e)
        {
            SearchExistingDriversWindow sDrivers = new SearchExistingDriversWindow();
            sDrivers.ShowDialog();
            idDriver = SearchExistingDriversWindow.sPropertyId;
            txtDriverCode.Text = SearchExistingDriversWindow.sPropertyCode;
            txtDriverName.Text = SearchExistingDriversWindow.sPropertyName;
        }

    }
}
