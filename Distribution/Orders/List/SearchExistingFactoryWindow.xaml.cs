using System;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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

        private void AcceptFactoryWindow_Click(object sender, RoutedEventArgs e)
        {
            bool bSelectionGrid = false;

            bSelectionGrid = MethodSelectValueGrid();
            if (bSelectionGrid)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna factoría. Es necesario seleccionar una.");
            }
        }

        public bool MethodSelectValueGrid()
        {
            DataRowView drv = (DataRowView)FactoriesGrid.SelectedItem;
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
