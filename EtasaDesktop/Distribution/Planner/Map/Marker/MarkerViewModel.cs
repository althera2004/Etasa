using GalaSoft.MvvmLight;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EtasaDesktop.Distribution.Planner.Map
{
    public class MarkerViewModel : ViewModelBase
    {
        private Brush _fill = Brushes.Gray;
        private string _title;
        private string _content;
        private string _alert;
        private Location _location;

        public Brush Fill
        {
            get => _fill;
            set => Set(ref _fill, value);
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string Content
        {
            get => _content;
            set => Set(ref _content, value);
        }

        public string Alert
        {
            get => _alert;
            set => Set(ref _alert, value);
        }

        public Location Location
        {
            get => _location;
            set => Set(ref _location, value);
        }
    }
}
