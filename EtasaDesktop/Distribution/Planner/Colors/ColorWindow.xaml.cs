using EtasaDesktop.Distribution.Planner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace EtasaDesktop.Distribution.MarkerColors
{
    public partial class ColorWindow : Window
    {

        public ObservableCollection<FactoryColors> ColorCollection { get; set; }

        public ColorWindow()
        {
            
            ColorCollection = new ObservableCollection<FactoryColors>();
            foreach (FactoryColors colors in PlannerRequester.RequestColors())
            {
                ColorCollection.Add(colors);
            }

            InitializeComponent();
        }
    }
}
