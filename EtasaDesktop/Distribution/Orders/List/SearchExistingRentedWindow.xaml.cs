using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para SearchExistingRentedWindow.xaml
    /// </summary>
    public partial class SearchExistingRentedWindow : Window
    {
        public static string sPropertyRentedText { get; set; }

        public SearchExistingRentedWindow()
        {
            InitializeComponent();
            FillDataRentedGrid();
        }

        private void FillDataRentedGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = @"SELECT * 
                            FROM [Drivers]";
                            //WHERE CodigoAlquilado <> 3";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ExistingRented");
                sda.Fill(dt);
                RentedGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void CloseRentedWindow_Click(object sender, RoutedEventArgs e)
        {
            sPropertyRentedText = "";
            this.Close();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MethodSelectValueGrid();
        }

        private void AcceptRentedWindow_Click(object sender, RoutedEventArgs e)
        {
            MethodSelectValueGrid();
        }

        public void MethodSelectValueGrid()
        {
            var drv = (DataRowView)RentedGrid.SelectedItem;
            if (drv != null)
            {
                String result = (drv["Name"]).ToString();
                sPropertyRentedText = result;
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún alquilado. Es necesario seleccionar una.");
            }
        }
    }
}
