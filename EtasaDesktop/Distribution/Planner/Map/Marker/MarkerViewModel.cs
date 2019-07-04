namespace EtasaDesktop.Distribution.Planner.Map
{
    using System.Windows.Media;
    using GalaSoft.MvvmLight;
    using Microsoft.Maps.MapControl.WPF;

    public class MarkerViewModel : ViewModelBase
    {
        private Brush fill = Brushes.Gray;
        private string title;
        private string content;
        private string alert;
        private Location location;

        public Brush Fill
        {
            get => fill;
            set => Set(ref fill, value);
        }

        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        public string Content
        {
            get => content;
            set => Set(ref content, value);
        }

        public string Alert
        {
            get => alert;
            set => Set(ref alert, value);
        }

        public Location Location
        {
            get => location;
            set => Set(ref location, value);
        }
    }
}