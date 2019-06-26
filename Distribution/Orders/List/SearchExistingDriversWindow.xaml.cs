using System;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para SearchExistingDriversWindow.xaml
    /// </summary>
    public partial class SearchExistingDriversWindow : Window
    {
        public static int sPropertyId { get; set; }
        public static string sPropertyCode { get; set; }
        public static string sPropertyName { get; set; }

        public SearchExistingDriversWindow()
        {
            InitializeComponent();
            FillDataDriversGrid();
        }

        private void FillDataDriversGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT * FROM [Drivers]";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ExistingDrivers");
                sda.Fill(dt);
                DriversGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void CloseDriversWindow_Click(object sender, RoutedEventArgs e)
        {
            sPropertyId = 0;
            sPropertyCode = "";
            sPropertyName = "";
            this.Close();
        }

        private void AcceptDriversWindow_Click(object sender, RoutedEventArgs e)
        {
            bool bSelectionGrid = false;

            bSelectionGrid = MethodSelectValueGrid();
            if (bSelectionGrid)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún conductor. Es necesario seleccionar uno.");
            }
        }

        public bool MethodSelectValueGrid()
        {
            DataRowView drv = (DataRowView)DriversGrid.SelectedItem;
            if (drv != null)
            {
                sPropertyId = int.Parse((drv["Id"]).ToString());
                sPropertyCode = (drv["Code"]).ToString();
                sPropertyName = (drv["Name"]).ToString();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
