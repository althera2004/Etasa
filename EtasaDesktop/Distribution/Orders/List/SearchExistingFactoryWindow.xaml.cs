using System;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows.Controls;

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para SearchExistingFactoryWindow.xaml
    /// </summary>
    public partial class SearchExistingFactoryWindow : Window
    {
        public static int sPropertyId { get; set; }
        public static string sPropertyCode { get; set; }
        public static string sPropertyName { get; set; }

        public SearchExistingFactoryWindow()
        {
            InitializeComponent();
            FillDataFactoriesGrid();
        }

        private void FillDataFactoriesGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT Id, Code, Name, Enabled FROM [Factories]";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ExistingFactories");
                sda.Fill(dt);
                FactoriesGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void CloseFactoryWindow_Click(object sender, RoutedEventArgs e)
        {
            sPropertyId = 0;
            sPropertyCode = "";
            sPropertyName = "";
            this.Close();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MethodSelectValueGrid();
        }

        private void AcceptFactoryWindow_Click(object sender, RoutedEventArgs e)
        {
            MethodSelectValueGrid();
        }

        public void MethodSelectValueGrid()
        {
            var drv = (DataRowView)FactoriesGrid.SelectedItem;
            if (drv != null)
            {
                sPropertyId = int.Parse((drv["Id"]).ToString());
                sPropertyCode = (drv["Code"]).ToString();
                sPropertyName = (drv["Name"]).ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna factoría. Es necesario seleccionar una.");
            }            
        }
    }
}
