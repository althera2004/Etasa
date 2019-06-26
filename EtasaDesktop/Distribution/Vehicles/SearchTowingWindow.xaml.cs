using EtasaDesktop.Common.Data;
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

namespace EtasaDesktop.Distribution.Vehicles
{
    /// <summary>
    /// Lógica de interacción para SearchTowingWindow.xaml
    /// </summary>
    public partial class SearchTowingWindow : Window
    {
        public SearchTowingWindow()
        {
            InitializeComponent();

            DataGridDataSearchTowing.ItemsSource = DataSearchTowing.CreateDummiesDataTowing();
        }
    }
}
