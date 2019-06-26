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
    /// Lógica de interacción para SearchExistingTrailerWindow.xaml
    /// </summary>
    public partial class SearchExistingTrailerWindow : Window
    {
        public static int sPropertyId { get; set; }
        public static string sPropertyCode { get; set; }
        public static string sPropertyLicensePlate { get; set; }

        public SearchExistingTrailerWindow()
        {
            InitializeComponent();
            FillDataTrailerGrid();
        }

        private void FillDataTrailerGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT * FROM [Vehicles]";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ExistingBranchOffices");
                sda.Fill(dt);
                TrailersGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void CloseTrailerWindow_Click(object sender, RoutedEventArgs e)
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

        private void AcceptTrailerWindow_Click(object sender, RoutedEventArgs e)
        {
            MethodSelectValueGrid();
        }

        public void MethodSelectValueGrid()
        {
            var drv = (DataRowView)TrailersGrid.SelectedItem;
            if (drv != null)
            {
                sPropertyId = int.Parse((drv["Id"]).ToString());
                sPropertyCode = (drv["Code"]).ToString();
                sPropertyLicensePlate = (drv["LicensePlate"]).ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún remolque. Es necesario seleccionar una.");
            }
        }
    }
}
