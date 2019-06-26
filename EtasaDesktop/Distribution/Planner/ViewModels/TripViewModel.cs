using EtasaDesktop.Common.Data;
using EtasaDesktop.Distribution.Planner.ViewModels;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Planner
{
    public class TripViewModel : ViewModelBase
    {
        private long _parentId;
        private int _plannedAmount;
        private int _realAmount;

        public TripViewModel()
        {
            Orders = new ObservableCollection<Order>();
            Orders.CollectionChanged += OnOrdersChanged;
        }

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
        public int Position { get; set; }
        
        public int LoadedAmount { get; set; }
        public int PlannedAmount
        {
            get => _plannedAmount;
            set
            {
                if (_plannedAmount != value)
                {
                    _plannedAmount = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int RealAmount
        {
            get => _realAmount;
            set
            {
                if (_realAmount != value)
                {
                    _realAmount = value;
                    RaisePropertyChanged();
                }
            }
        }


        public ObservableCollection<Order> Orders { get; private set; }


        private void OnOrdersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Asigna los nuevos pedidos
            if (e.NewItems != null)
            {
                foreach (Order newOrder in e.NewItems)
                    PlannerRequester.AddPlannedOrder(Id, newOrder);
            }

            // Desasigna los antiguos pedidos que ya no estan asignados
            if (e.OldItems != null)
            {
                foreach (Order oldOrder in e.OldItems)
                {
                    var order = Orders.SingleOrDefault(o => o.Equals(oldOrder));

                    if (order is null)
                    {
                        PlannerRequester.RemovePlannedOrder(oldOrder);
                    }
                }
            }

            // Ordena los pedidos de la asignaciones
            for (int i = Orders.Count - 1; i >= 0; i--)
            {
                var order = Orders[i];
                PlannerRequester.UpdateAssignmentOrderPosition(order, i);
            }

            // Refresca el sumatorio de cantidades planificadas para los pedidos
            PlannedAmount = Orders.Sum(o => o.RequestedAmount);

            // Refresca la cantidad restante en el vehiculo 
            RealAmount = LoadedAmount - Orders.Sum(o => o.ReceivedAmount ?? 0);
        }
    }
}
