using System;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;

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
            sPropertyCode = string.Empty;
            sPropertyName = string.Empty;
            this.Close();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MethodSelectValueGrid();
        }

        private void AcceptDriversWindow_Click(object sender, RoutedEventArgs e)
        {
            MethodSelectValueGrid();
        }

        public void MethodSelectValueGrid()
        {
            var drv = (DataRowView)DriversGrid.SelectedItem;
            if (drv != null)
            {
                sPropertyId = int.Parse((drv["Id"]).ToString());
                sPropertyCode = (drv["Code"]).ToString();
                sPropertyName = (drv["Name"]).ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún conductor. Es necesario seleccionar un0.");
            }
        }
    }
}
