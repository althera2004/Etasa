using System;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para SearchExistingTruckWindow.xaml
    /// </summary>
    public partial class SearchExistingTruckWindow : Window
    {
        public static int sPropertyId { get; set; }
        public static string sPropertyCode { get; set; }
        public static string sPropertyLicensePlate { get; set; }

        public SearchExistingTruckWindow()
        {
            InitializeComponent();
            FillDataTruckGrid();
        }

        private void FillDataTruckGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //CmdString = "SELECT * FROM [Vehicles]";
                //CmdString = @"Select Id, Code, LicensePlate, VIN, Brand, Weight, MaxWeight, TankVolume, StartNode, FinalNode, Enabled
                //              FROM [Etasa].[dbo].[Vehicles]
                //              WHERE Type = '01' OR Type = '02' OR Type = '03'";
                CmdString = @"Select v.Id, v.Code, LicensePlate, VIN, Weight, MaxWeight, TankVolume, StartNode, FinalNode, Type, Enabled, vb.Name as Marca
                              FROM [Etasa].[dbo].[Vehicles] as v
                              Full outer join [Etasa].[dbo].[Vehicles_Brands] as vb
                              ON v.Brand=vb.Code
                              WHERE Type = '01' OR Type = '02' OR Type = '03'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ExistingTrucks");
                sda.Fill(dt);
                TrucksGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void CloseTruckWindow_Click(object sender, RoutedEventArgs e)
        {
            sPropertyId = 0;
            sPropertyCode = "";
            sPropertyLicensePlate = "";
            this.Close();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MethodSelectValueGrid();
        }

        private void AcceptTruckWindow_Click(object sender, RoutedEventArgs e)
        {
            MethodSelectValueGrid();
        }

        public void MethodSelectValueGrid()
        {
            var drv = (DataRowView)TrucksGrid.SelectedItem;
            if (drv != null)
            {
                sPropertyId = int.Parse((drv["Id"]).ToString());
                sPropertyCode = (drv["Code"]).ToString();
                sPropertyLicensePlate = (drv["LicensePlate"]).ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna tractora. Es necesario seleccionar una.");
            }
        }
    }
}
