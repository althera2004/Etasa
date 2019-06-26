using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para EditFiltersWindow.xaml
    /// </summary>
    public partial class EditFiltersWindow : Window
    {
        public EditFiltersWindow()
        {
            InitializeComponent();

            if (sControl == "" || sControl == null)
            {
                sControl = "Orders";
            }
            itemsCount(sControl);
        }

        private void FilterWindowCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void GenerateControls(int iControl = 0, string sNames = "")
        {
            TextBox txtBlockHeader = new TextBox();
            txtBlockHeader.Name = "txtBlockHeader_" + iControl.ToString();
            txtBlockHeader.Text = sNames;// + iControl.ToString();
            txtBlockHeader.MaxWidth = 200;
            txtBlockHeader.IsReadOnly = true;
            txtBlockHeader.BorderThickness = new Thickness(0);
            PanelFilters.Children.Add(txtBlockHeader);
            PanelFilters.RegisterName(txtBlockHeader.Name, txtBlockHeader);
            //
            TextBox txtValue = new TextBox();
            txtValue.Name = "txtValue_" + iControl.ToString();
            txtValue.Text = "";//sNames + iControl.ToString();
            txtValue.MaxWidth = 200;
            PanelFilters.Children.Add(txtValue);
            PanelFilters.RegisterName(txtValue.Name, txtValue);
        }

        public void itemsCount(string sTest)
        {
            string sCategory = sTest; // ComboCategory.Text.ToString();
            int iCon = 1;

            switch (sCategory)
            {
                case "Conductores":
                    sCategory = "Drivers";
                    break;
                case "Clientes":
                    sCategory = "Clients";
                    break;
                case "Factorías":
                    sCategory = "Factories";
                    break;
                case "Productos":
                    sCategory = "Products";
                    break;
                case "Pedidos":
                    sCategory = "Orders";
                    break;
                case "Usuarios":
                    sCategory = "System_Users";
                    break;
                default:
                    sCategory = "Orders";
                    break;
            }

            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = @"SELECT TOP 1 * 
                            FROM " + sCategory;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("EditFilters");
                sda.Fill(dt);
                //EditGrid.ItemsSource = dt.DefaultView;

                string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                        select dc.ColumnName).ToArray();

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        GenerateControls(iCon, columnNames[iCon - 1]);
                        iCon++;
                    }
                }

            }
        }

        public static string sControl { get; set; }
        public static int iTotalFields { get; set; }

        private void FilterWindowAccept_Click(object sender, RoutedEventArgs e)
        {
            string sTest1 = "", sTest2 = "";

            foreach (object child in PanelFilters.Children)
            {
                sTest2 = PanelFilters.Children.ToString();

                if ((child.GetType().ToString() == "System.Windows.Controls.TextBox") && ((child as FrameworkElement).Name.ToString().Contains("txtValue_")) && ((child as FrameworkElement).ToString() != ""))
                {
                    EditElementsWindow.iNumeroFiltros++;

                    sTest1 += "__" + this.PanelFilters.Children.ToString();
                }
            }

            sTest1 = sTest1 + this.PanelFilters.Children.Count.ToString();

            List<ItemFilters> items = this.PanelFilters.Children.Cast<ItemFilters>().ToList();

            this.Close();
        }

    }

    internal class ItemFilters
    {

    }
}
