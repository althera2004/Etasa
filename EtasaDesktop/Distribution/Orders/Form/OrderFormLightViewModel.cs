using EtasaDesktop.Common.Data;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Form
{
    public class OrderFormLightViewModel : ViewModelBase
    {
        private Order _order;
        public Order Order
        {
            get => _order;
            set => Set(ref _order, value);
        }

        public string Reference
        {
            get => Order.Reference;
            set
            {
                Order.Reference = value;
                RaisePropertyChanged();
            }
        }

        #region Carga

        public int RequestedAmount
        {
            get => Order.RequestedAmount;
            set
            {
                Order.RequestedAmount = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => Order.StartDate;
            set
            {
                Order.StartDate = value;
                RaisePropertyChanged();
            }
        }
        public DateTime FinalDate
        {
            get => Order.FinalDate;
            set
            {
                Order.FinalDate = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public bool Canceled
        {
            get => Order.Status == 8;
            set
            {
                Order.Status = value ? 8 : 1;
                RaisePropertyChanged();
            }
        }

        public string Observations
        {
            get => Order.Description;
            set
            {
                Order.Description = value;
                RaisePropertyChanged();
            }
        }

        public string Notes
        {
            get
            {
                if (string.IsNullOrEmpty(Order.Observations))
                {
                    return string.Empty;
                }

                return Order.Observations;
            }
            set
            {
                Order.Observations = value;
                RaisePropertyChanged();
            }
        }

        public OrderFormLightViewModel(Order order)
        {
            this._order = order;
        }
    }
}
