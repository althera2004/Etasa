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
    /// Lógica de interacción para SearchExistingOrdersWindow.xaml
    /// </summary>
    public partial class SearchExistingOrdersWindow : Window
    {
        public static string sPropertyOperatorText { get; set; }

        public SearchExistingOrdersWindow()
        {
            InitializeComponent();
            //OrdersOperatorsGrid.ItemsSource = OrderCustomers.CreateDummiesOrdersCustomers();
            FillDataOperatorsGrid();
        }

        private void FillDataOperatorsGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT Id, Code, Name, Enabled FROM [Operators]";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ExistingOrders");
                sda.Fill(dt);
                OrdersOperatorsGrid.ItemsSource = dt.DefaultView;
            }
        }

        public class OrderCustomers
        {
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string Pedidos { get; set; }

            public static List<OrderCustomers> CreateDummiesOrdersCustomers()
            {
                return new List<OrderCustomers>
            {
                new OrderCustomers {
                    Codigo = "0034", Nombre = "Cepsa", Pedidos = "475" },
                new OrderCustomers {
                    Codigo = "1000", Nombre = "Vitogas", Pedidos = "148" },
                new OrderCustomers {
                    Codigo = "7777", Nombre = "Galp", Pedidos = "220" },
                };
            }
        }

        private void CloseOperatorsWindow_Click(object sender, RoutedEventArgs e)
        {
            sPropertyOperatorText = "";
            this.Close();
        }

        public bool MethodSelectValueGrid()
        {
            DataRowView drv = (DataRowView)OrdersOperatorsGrid.SelectedItem;
            if (drv != null)
            {
                String result = (drv["Name"]).ToString();
                sPropertyOperatorText = result;

                return true;
            }
            else
            {
                return false;
            }
        }

        private void AcceptOperatorsWindow_Click(object sender, RoutedEventArgs e)
        {
            bool bSelectionGrid = false;

            bSelectionGrid = MethodSelectValueGrid();
            if(bSelectionGrid)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún operador. Es necesario seleccionar uno.");
            }
        }
    }
}
