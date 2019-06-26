using EtasaDesktop.Common.Data;
using Microsoft.Maps.MapControl.WPF;
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

namespace EtasaDesktop.Distribution.Planner.Map
{
    public partial class MarkerFactory : UserControl
    {

        private MarkerFactoryViewModel _viewModel;

        public MarkerFactory(MarkerFactoryViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            MapLayer.SetPositionOrigin(this, PositionOrigin.BottomCenter);
            MapLayer.SetPosition(this, new Location(_viewModel.Location.Latitude, _viewModel.Location.Longitude));
            this.DataContext = _viewModel;
        }
    }
}
