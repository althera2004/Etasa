using EtasaDesktop.Common.Data;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Data
{
    public class Vehicle : Entity
    {
        public new string Name
        {
            get => LicensePlate;
        }

        public int Type;
        public int SubType;

        public string LicensePlate;
        public string VIN;

        public int Brand;
        public string BrandModel;

        public int AxleCount;
        public int Weight;
        public int MaxWeight;

        public int StartNode;
        public int FinalNode;

        public VehicleLocationData Location;
    }
}
