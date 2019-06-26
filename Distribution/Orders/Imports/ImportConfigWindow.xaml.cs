using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    public partial class ImportConfigWindow : Window
    {

        public ImportConfigurationViewModel Configuration { get; set; }
        public ImportConfiguration Result { get; set; }

        public ImportConfigWindow(ImportConfiguration configuration)
        {
            Result = configuration;

            Configuration = new ImportConfigurationViewModel();
            Configuration.Config = new ImportConfiguration(configuration);

            /*foreach (ImportColumnConfiguration icc in configuration.Columns)
            {
                Configuration.Columns.Add(new ImportColumnConfigurationViewModel()
                {
                    ColumnConfig = icc
                });
            }*/

            DataContext = Configuration;

            InitializeComponent();

            

        }

        private void PreviewNumericInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ImportFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = Configuration.Folder;
                dialog.Description = "Selecciona la carpeta donde están los ficheros de importación";
                DialogResult result = dialog.ShowDialog();
                Configuration.Folder = dialog.SelectedPath;
            }
        }
        private void ProcessedFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = Configuration.ProcessedFolder;
                dialog.Description = "Selecciona la carpeta donde guardar los ficheros procesados";
                DialogResult result = dialog.ShowDialog();
                
                Configuration.ProcessedFolder = dialog.SelectedPath;
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Result = Configuration.Config;
            DialogResult = true;
        }
    }
}
