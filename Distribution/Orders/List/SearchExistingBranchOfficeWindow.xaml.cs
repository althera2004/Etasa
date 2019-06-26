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
using EtasaDesktop.Distribution.Orders;
using System.Collections.ObjectModel;

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para SearchExistingBranchOfficeWindow.xaml
    /// </summary>
    public partial class SearchExistingBranchOfficeWindow : Window
    {
        public static string sPropertyBranchOfficeText { get; set; }

        public SearchExistingBranchOfficeWindow()
        {
            InitializeComponent();
            FillDataBranchOfficesGrid();
        }

        private void FillDataBranchOfficesGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //CmdString = "SELECT codigo, nombre, ccc as pedidos FROM [OperadoresBackupNuevo]";
                CmdString = "SELECT Products.Id, Code, Name, Density, MeasureUnit, Enabled, Observations FROM [Products] INNER JOIN Products_Obs ON Products.Id = Products_Obs.Id";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable("ExistingBranchOffices");
                DataTable dt = new DataTable("ExistingProducts");
                sda.Fill(dt);
                BranchOfficesGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void CloseBranchOfficeWindow_Click(object sender, RoutedEventArgs e)
        {
            sPropertyBranchOfficeText = "";
            this.Close();
        }

        private void AcceptBranchOffice_Click(object sender, RoutedEventArgs e)
        {
            bool bSelectionGrid = false;

            bSelectionGrid = MethodSelectValueGrid();
            if (bSelectionGrid)
            {
                this.Close();
            }
            else
            {
                //MessageBox.Show("No se ha seleccionado ninguna sucursal. Es necesario seleccionar una.");
                MessageBox.Show("No se ha seleccionado ningún producto. Es necesario seleccionar uno.");
            }
        }

        public bool MethodSelectValueGrid()
        {
            DataRowView drv = (DataRowView)BranchOfficesGrid.SelectedItem;
            if (drv != null)
            {
                String result = (drv["Name"]).ToString();
                sPropertyBranchOfficeText = result;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
