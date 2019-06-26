using EtasaDesktop.Common.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Planner.ViewModels
{

    // TODO Clase de mejora del planificador

    public class PlannerOrderViewModel : ViewModelBase
    {
        private Order _order;
        private Brush _color;


        public PlannerOrderViewModel(Order order)
        {
            _order = order;
        }
        public Order Order
        {
            get => _order;
            set => Set(ref _order, value);
        }


        public RelayCommand EditCommand { get; private set; }
        public RelayCommand ShowObservationsCommand { get; private set; }
        public RelayCommand ShowNotesCommand { get; private set; }


        public string Reference
        {
            get => _order.Reference;
            set
            {
                if (_order.Reference != value)
                {
                    _order.Reference = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get => _order.StartDate;
            set
            {
                if (_order.StartDate != value)
                {
                    _order.StartDate = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DateTime FinalDate
        {
            get => _order.StartDate;
            set
            {
                if (_order.StartDate != value)
                {
                    _order.StartDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Brush Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string OperatorName
        {
            get => _order.Operator.Name;
            set
            {
                if (_order.Operator.Name != value)
                {
                    _order.Operator.Name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ClientName
        {
            get => _order.Client.Name;
            set
            {
                if (_order.Client.Name != value)
                {
                    _order.Client.Name = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ClientCity
        {
            get => _order.Client.Location.City;
            set
            {
                if (_order.Client.Location.City != value)
                {
                    _order.Client.Location.City = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ClientPostCode
        {
            get => _order.Client.Location.PostCode;
            set
            {
                if (_order.Client.Location.PostCode != value)
                {
                    _order.Client.Location.PostCode = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
