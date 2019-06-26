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
    /// Lógica de interacción para EditElementsWindow.xaml
    /// </summary>
    public partial class EditElementsWindow : Window
    {
        public EditElementsWindow()
        {
            InitializeComponent();
            
        }

        private void FillEditGrid()
        {
            string sOrder = "";
            string sTypeOrd = "";
            int iControl = 0;

            string sCategory = ComboCategory.Text.ToString();
            
            sOrder = OrderComboBox.Text.ToString();
            if (sOrder != null && sOrder != "")
            {
                sOrder = " ORDER BY " + sOrder;
                iControl++;
            }

            sTypeOrd = TypeComboBox.Text.ToString();
            if ((sTypeOrd == "Descendente") && (iControl > 0))
            {
                sTypeOrd = " DESC ";
            }
            else if ((sTypeOrd == "Ascendente") && (iControl > 0))
            {
                sTypeOrd = " ASC ";
            }
            else
            {
                sTypeOrd = "";
            }


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
                CmdString = @"SELECT * 
                            FROM " + sCategory + sOrder + sTypeOrd;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("EditDataGrid");
                sda.Fill(dt);
                EditGrid.ItemsSource = dt.DefaultView;

                if (EditGrid.Items.Count > 0)
                {
                    string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                            select dc.ColumnName).ToArray();

                    OrderComboBox.ItemsSource = columnNames;
                }
            }
        }

        private void EditSearch_Click(object sender, RoutedEventArgs e)
        {
            FillEditGrid();
        }

        private void ComboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditFiltersWindow.sControl = (e.AddedItems[0] as ComboBoxItem).Content as string;
            EditAddModifyWindow.sControlAdd = (e.AddedItems[0] as ComboBoxItem).Content as string;
        }

        private void EditFiltersOpenWindowButton_Click(object sender, RoutedEventArgs e)
        {
            EditFiltersWindow efw = new EditFiltersWindow();
            efw.ShowDialog();
        }

        public static int iNumeroFiltros { get; set; }

        private void AddButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditAddModifyWindow eamw = new EditAddModifyWindow();
            eamw.ShowDialog();
        }

        private void EditElementsReset_Click(object sender, RoutedEventArgs e)
        {
            EditGrid.ItemsSource = null;
            EditGrid.Items.Refresh();
        }
    }
}
