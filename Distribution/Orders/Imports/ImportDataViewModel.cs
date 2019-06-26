using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    public class ImportDataViewModel : ViewModelBase
    {
        private ImportData _data;
        public ImportData Data
        {
            get => _data;
            set => Set(ref _data,value);
        }


        public DateTime StartDate
        {
            get => this.Data.StartDate;
            set
            {
                if (Data.StartDate != value)
                {
                    Data.StartDate = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DateTime FinalDate
        {
            get => this.Data.FinalDate;
            set
            {
                if (Data.FinalDate != value)
                {
                    Data.FinalDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int OperatorId
        {
            get => this.Data.OperatorId;
            set
            {
                if (Data.OperatorId != value)
                {
                    Data.OperatorId = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Reference
        {
            get => this.Data.Reference;
            set
            {
                if (Data.Reference != value)
                {
                    Data.Reference = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ClientCode
        {
            get => this.Data.ClientCode;
            set
            {
                if (Data.ClientCode != value)
                {
                    Data.ClientCode = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ClientName
        {
            get => this.Data.ClientName;
            set
            {
                if (Data.ClientName != value)
                {
                    Data.ClientName = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ClientCif
        {
            get => this.Data.ClientCif;
            set
            {
                if (Data.ClientCif != value)
                {
                    Data.ClientCif = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Contact
        {
            get => this.Data.Contact;
            set
            {
                if (Data.Contact != value)
                {
                    Data.Contact = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string City
        {
            get => this.Data.City;
            set
            {
                if (Data.City != value)
                {
                    Data.City = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Address
        {
            get => this.Data.Address;
            set
            {
                if (Data.Address != value)
                {
                    Data.Address = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string PostCode
        {
            get => this.Data.PostCode;
            set
            {
                if (Data.PostCode != value)
                {
                    Data.PostCode = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Email
        {
            get => this.Data.Email;
            set
            {
                if (Data.Email != value)
                {
                    Data.Email = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Phone
        {
            get => this.Data.Phone;
            set
            {
                if (Data.Phone != value)
                {
                    Data.Phone = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Phone2
        {
            get => this.Data.Phone2;
            set
            {
                if (Data.Phone2 != value)
                {
                    Data.Phone2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FactoryCode
        {
            get => this.Data.FactoryCode;
            set
            {
                if (Data.FactoryCode != value)
                {
                    Data.FactoryCode = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Latitude
        {
            get => this.Data.Latitude;
            set
            {
                if (Data.Latitude != value)
                {
                    Data.Latitude = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Longitude
        {
            get => this.Data.Longitude;
            set
            {
                if (Data.Longitude != value)
                {
                    Data.Longitude = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string TankVolume
        {
            get => this.Data.TankVolume;
            set
            {
                if (Data.TankVolume != value)
                {
                    Data.TankVolume = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string TankNum
        {
            get => this.Data.TankNum;
            set
            {
                if (Data.TankNum != value)
                {
                    Data.TankNum = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string TankLevel
        {
            get => this.Data.TankLevel;
            set
            {
                if (Data.TankLevel != value)
                {
                    Data.TankLevel = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Amount
        {
            get => this.Data.Amount;
            set
            {
                if (Data.Amount != value)
                {
                    Data.Amount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ProductCode
        {
            get => this.Data.ProductCode;
            set
            {
                if (Data.ProductCode != value)
                {
                    Data.ProductCode = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string MeasureUnit
        {
            get => this.Data.MeasureUnit;
            set
            {
                if (Data.MeasureUnit != value)
                {
                    Data.MeasureUnit = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string VehicleSizeCode
        {
            get => this.Data.VehicleSizeCode;
            set
            {
                if (Data.VehicleSizeCode != value)
                {
                    Data.VehicleSizeCode = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Description
        {
            get => this.Data.Description;
            set
            {
                if (Data.Description != value)
                {
                    Data.Description = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Observations
        {
            get => this.Data.Observations;
            set
            {
                if (Data.Observations != value)
                {
                    Data.Observations = value;
                    RaisePropertyChanged();
                }
            }
        }


        public ImportDataViewModel(ImportData order) => Data = order;
    }
}
