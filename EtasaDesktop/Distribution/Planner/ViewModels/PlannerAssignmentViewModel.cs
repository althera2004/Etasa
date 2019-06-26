using EtasaDesktop.Common.Data;
using EtasaDesktop.Distribution.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Planner
{
    public class PlannerAssignmentViewModel : ViewModelBase
    {
        private Assignment _assignment;
        private Assignment _assignmentRoutes;

        public PlannerAssignmentViewModel(Assignment assignment)
        {
            Assignment = assignment;
            //TODO inicialización colección temporal
            test = new ObservableCollection<Order>();

        }
        public Assignment Assignment
        {
            get => _assignment;
            set => Set(ref _assignment, value);
        }
        public RouteViewModel Route { get; set; }

        //TODO colección temporal para evitar error "null" en planificador
        public ObservableCollection<Order> test { get; set; }

        public string DriverName
        {
            get => _assignment.Driver.Name;
            set
            {
                if (_assignment.Driver.Name != value)
                {
                    _assignment.Driver.Name = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string CabCode
        {
            get => _assignment.Cab.Code;
            set
            {
                if (_assignment.Cab.Code != value)
                {
                    _assignment.Cab.Code = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string TrailerCode
        {
            get => _assignment.Trailer.Code;
            set
            {
                if (_assignment.Trailer.Code != value)
                {
                    _assignment.Trailer.Code = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int TrailerCapacity
        {
            get => _assignment.Trailer.TankVolume;
            set
            {
                if (_assignment.Trailer.TankVolume != value)
                {
                    _assignment.Trailer.TankVolume = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FactoryName
        {
            get => _assignment.FactoryName;

            set
            {
                if (_assignment.FactoryName != value)
                {
                    _assignment.FactoryName = value;
                    RaisePropertyChanged();
                }
            }
        }


        public int totalamount
        {
            get => _assignment.TotalAmountAssigment;

            set
            {
                if (_assignment.TotalAmountAssigment != value)
                {
                    _assignment.TotalAmountAssigment = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string MessageOverAmount
        {
            get => _assignment.MessageOverAmount;

            set
            {
                if (_assignment.MessageOverAmount != value)
                {
                    _assignment.MessageOverAmount = value;
                    RaisePropertyChanged();
                }
            }
        }








    }
}
