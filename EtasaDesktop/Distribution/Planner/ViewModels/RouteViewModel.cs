using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Planner
{
    public class RouteViewModel : ViewModelBase
    {
        private long _parentId;

        public long Id { get; set; }
        public long ParentId
        {
            get => _parentId;
            set
            {
                _parentId = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TripViewModel> Trips {get; set;}
    }
}
