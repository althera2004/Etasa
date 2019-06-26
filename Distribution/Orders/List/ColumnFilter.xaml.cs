using EtasaDesktop.Common.Auth;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para ColumnFilter.xaml
    /// </summary>
    public partial class ColumnFilter : Window
    {
        string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;

        Session sessionactual = Properties.Settings.Default.Session;

        public ColumnFilter()
        {
            InitializeComponent();
            LoadColumns();
            CargarConfig();
        }

        private void LoadColumns()
        {
            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                string CmdString = string.Empty;
                CmdString = @"SELECT Description FROM System_Data_Column_config WHERE Visible = 1";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = CmdString;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string columnname = dr["Description"].ToString();
                        CheckBox checkbox = new CheckBox();
                        checkbox.Content = columnname;
                        ColumnsListbox.Items.Add(checkbox);
                    }
                }

                con.Close();
            }
        }

        private void CargarConfig()
        {
            using (SqlConnection con = new SqlConnection(ConString))
            {
                int iduser = sessionactual.User.Id;
                con.Open();
                string CmdString = string.Empty;
                CmdString = @"SELECT UserConfig FROM System_User_Column_Config WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = CmdString;
                cmd.Parameters.AddWithValue("@Id", iduser);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string config = dr["UserConfig"].ToString();

                        if (config == "")
                        {
                            foreach (CheckBox checkbox in ColumnsListbox.Items)
                            {
                                checkbox.IsChecked = true;
                            }
                        }

                        else
                        {
                            var columnconfig = JsonConvert.DeserializeObject<List<UserColumnConfig>>(config);

                            foreach (CheckBox checkbox in ColumnsListbox.Items)
                            {
                                string columnname = checkbox.Content.ToString();
                                
                                foreach (var column in columnconfig)
                                {
                                    if (columnname == "ID Pedido" && column.columnname == "ID Pedido")
                                    {
                                        int pos = column.posicion;
                                    }

                                    if (columnname == column.columnname && column.esvisible == true)
                                    {
                                        checkbox.IsChecked = true;
                                    }
                                }
                            }
                        }
                    }                                       
                }
            }
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            int posicion = 1;

            List<UserColumnConfig> configlist = new List<UserColumnConfig>();

            foreach (CheckBox checkbox in ColumnsListbox.Items)
            {
                string columna = checkbox.Content.ToString();
                bool esvisible = checkbox.IsChecked.Value;

                UserColumnConfig config = new UserColumnConfig();

                config.posicion = posicion;
                config.columnname = columna;
                config.esvisible = esvisible;

                configlist.Add(config);
                
                posicion++;
            }

            string jsonString = JsonConvert.SerializeObject(configlist);

            SaveConfigColumnsUser(jsonString);
        }

        private void SaveConfigColumnsUser(string userconfig)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            {
                int iduser = sessionactual.User.Id;
                con.Open();
                string CmdString = string.Empty;
                CmdString = @"UPDATE System_User_Column_Config SET UserConfig = @UserConfig WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = CmdString;
                cmd.Parameters.AddWithValue("@UserConfig", userconfig);
                cmd.Parameters.AddWithValue("@Id", iduser);
                cmd.ExecuteNonQuery();
            }

            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
