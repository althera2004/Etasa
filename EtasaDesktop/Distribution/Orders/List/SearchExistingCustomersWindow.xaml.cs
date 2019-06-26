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
    /// Lógica de interacción para SearchExistingCustomersWindow.xaml
    /// </summary>
    public partial class SearchExistingCustomersWindow : Window
    {
        public static string sPropertyCustomerText { get; set; }

        public SearchExistingCustomersWindow()
        {
            InitializeComponent();
            FillDataCustomersGrid();
        }

        private void FillDataCustomersGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT Id, Code, Name, Enabled FROM [Clients]";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ExistingCustomers");
                sda.Fill(dt);
                CustomersGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void CloseCustomersWindow_Click(object sender, RoutedEventArgs e)
        {
            sPropertyCustomerText = string.Empty;
            this.Close();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MethodSelectValueGrid();
        }

        private void AcceptCustomersWindow_Click(object sender, RoutedEventArgs e)
        {
            MethodSelectValueGrid();
        }

        public void MethodSelectValueGrid()
        {
            var drv = (DataRowView)CustomersGrid.SelectedItem;
            if (drv != null)
            {
                String result = (drv["Name"]).ToString();
                sPropertyCustomerText = result;
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún cliente. Es necesario seleccionar un0.");
            }
        }
    }
}
