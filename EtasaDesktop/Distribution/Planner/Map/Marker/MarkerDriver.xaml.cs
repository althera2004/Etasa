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
    public partial class MarkerDriver : UserControl
    {

        private MarkerAssignmentViewModel _viewModel;

        public MarkerDriver(MarkerAssignmentViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            MapLayer.SetPositionOrigin(this, PositionOrigin.BottomCenter);

            var location = _viewModel.Assignment.Cab.Location;
            MapLayer.SetPosition(this, new Location(location.Latitude, location.Longitude));
            this.DataContext = _viewModel;
        }
    }
}
