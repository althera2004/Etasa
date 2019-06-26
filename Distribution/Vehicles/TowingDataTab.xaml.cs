using EtasaDesktop.Distribution.Vehicles;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using EtasaDesktop.Common.Data;

namespace EtasaDesktop.Vehicles
{
    /// <summary>
    /// Lógica de interacción para TowingData.xaml
    /// </summary>
    public partial class TowingDataTab : UserControl
    {
        public TowingDataTab()
        {
            InitializeComponent();
        }

        private void SearchTowingWindow_Click(object sender, RoutedEventArgs e)
        {
            SearchTowingWindow searchTowing = new SearchTowingWindow();
            searchTowing.ShowDialog();
        }
    }
}
