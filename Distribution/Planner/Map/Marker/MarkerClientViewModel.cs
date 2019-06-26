using EtasaDesktop.Common.Data;
using EtasaDesktop.Common.Tools;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EtasaDesktop.Distribution.Planner.Map
{
    public class MarkerClientViewModel : MarkerViewModel
    {
        private int _num;
        private Client _client;
        private ObservableCollection<Order> _orders;


        public Client Client
        {
            get => _client;
            set
            {
                Set(ref _client, value);
                Title = _client.Name;
                Location = new Location(_client.Location.Latitude, _client.Location.Longitude);
            }
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                Set(ref _orders, value);
                RefreshNum();
            }
        }

        public int Num
        {
            get => _num;
            set => Set(ref _num, value);
        }

        public MarkerClientViewModel()
        {
            Orders = new ObservableCollection<Order>();
            _orders.CollectionChanged += OnCollectionChanged;
        }


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshNum();
        }

        public void RefreshNum()
        {
            int num = 0;

            foreach (Order order in Orders)
                num += order.RequestedAmount;

            Num = num / 1000;
        }
    }
}
