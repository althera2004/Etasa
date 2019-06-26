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
    /// Lógica de interacción para EditAddModifyWindow.xaml
    /// </summary>
    public partial class EditAddModifyWindow : Window
    {
        public EditAddModifyWindow()
        {
            InitializeComponent();

            if (sControlAdd == "" || sControlAdd == null)
            {
                sControlAdd = "Orders";
            }
            functionConnectDataBase(sControlAdd);
        }

        private void EditAddWindowCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditAddWindowAccept_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Seguro que desea guardar los cambios?",
            "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
            {
                
            }

            
        }

        public void GenerateControls(int iControl = 0, string sNames = "", string sPrimary = "", string sIsNull = "", string sDataType = "", string sMaxLength = "")
        {
            TextBox txtBlockHeader = new TextBox();
            txtBlockHeader.Name = "txtBlockHeader_" + iControl.ToString();
            txtBlockHeader.Text = sNames;
            txtBlockHeader.MaxWidth = 200;
            txtBlockHeader.IsReadOnly = true;
            txtBlockHeader.BorderThickness = new Thickness(0);
            if (sPrimary == "True") { txtBlockHeader.Foreground = Brushes.Red; }
            if (sIsNull == "False" && sPrimary == "False") { txtBlockHeader.Foreground = Brushes.Blue; }
            
            PanelEditAdd.Children.Add(txtBlockHeader);
            PanelEditAdd.RegisterName(txtBlockHeader.Name, txtBlockHeader);
            //
            TextBox txtValue = new TextBox();
            txtValue.Name = "txtValue_" + iControl.ToString();
            txtValue.Text = "";
            txtValue.MaxWidth = 200;
            if (sPrimary == "True") { txtValue.BorderBrush = Brushes.Red; }
            if (sPrimary == "True") { txtValue.IsReadOnly = true; txtValue.ToolTip = "Primary Key, no se puede modificar"; }
            if (sIsNull == "False" && sPrimary == "False") { txtValue.BorderBrush = Brushes.Blue; txtValue.ToolTip = "Campo obligatorio"; }
            txtValue.MaxLength = Convert.ToInt32(sMaxLength);
            if (sNames == "CreatedDate") { txtValue.BorderBrush = Brushes.Red; txtValue.IsReadOnly = true; txtValue.ToolTip = "No puede modificarse"; }
            if (sNames == "ModifiedDate") { txtValue.BorderBrush = Brushes.Red; txtValue.IsReadOnly = true; txtValue.ToolTip = "No puede modificarse"; }
            PanelEditAdd.Children.Add(txtValue);
            PanelEditAdd.RegisterName(txtValue.Name, txtValue);
        }

        public void functionConnectDataBase(string sCategory)
        {
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
                CmdString = @"SELECT 
                                c.name 'Column_Name',
                                t.Name 'Data_type',
                                c.max_length 'Max_Length',
                                c.precision ,
                                c.scale ,
                                c.is_nullable,
                                ISNULL(i.is_primary_key, 0) 'Primary_Key'
                            FROM    
                                sys.columns c
                            INNER JOIN 
                                sys.types t ON c.user_type_id = t.user_type_id
                            LEFT OUTER JOIN 
                                sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                            LEFT OUTER JOIN 
                                sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
                            WHERE
                                c.object_id = OBJECT_ID('" + sCategory + "')";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("EditAddModify");
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    GenerateControls(iCon, row["Column_Name"].ToString(), row["Primary_Key"].ToString(), row["is_nullable"].ToString(), row["Data_type"].ToString(), row["Max_Length"].ToString());
                    iCon++;
                }
                this.Title = "Editar/Añadir Registro en " + sCategory;
            }
        }

        public static string sControlAdd { get; set; }
        public static int iTotalFields { get; set; }

        private void EditAddWindowReset_Click(object sender, RoutedEventArgs e)
        {
            int iNum = 0;

            foreach (var c in LogicalTreeHelper.GetChildren(PanelEditAdd))
            {
                var child = VisualTreeHelper.GetChild(PanelEditAdd, iNum);
                var frameworkElement = child as FrameworkElement;
                string sFrame = frameworkElement.Name;

                if ((c is TextBox) && sFrame.Contains("txtValue"))
                {
                    (c as TextBox).Clear();
                }
                iNum++;
            }
        }
    }
}
