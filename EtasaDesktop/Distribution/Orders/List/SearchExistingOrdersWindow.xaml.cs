namespace EtasaDesktop.Distribution.Orders
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Windows;
    using System.Windows.Input;

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
            sPropertyOperatorText = string.Empty;
            this.Close();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MethodSelectValueGrid();
        }

        private void AcceptOperatorsWindow_Click(object sender, RoutedEventArgs e)
        {
            MethodSelectValueGrid();
        }

        public void MethodSelectValueGrid()
        {
            var drv = (DataRowView)OrdersOperatorsGrid.SelectedItem;
            if (drv != null)
            {

                string result = (drv["Name"]).ToString();
                sPropertyOperatorText = result;
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún operador. Es necesario seleccionar un0.");
            }
        }
    }
}
