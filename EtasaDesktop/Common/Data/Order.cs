﻿using EtasaDesktop.Distribution.Data;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Common.Data
{
    public partial class Order
    {
        public Order(){}

        public Order(Order order)
        {
            if(order != null)
            {
                this.Id = order.Id;
                this.Reference = order.Reference;
                this.Operator = order.Operator;
                this.Client = order.Client;
                this.StartDate = order.StartDate;
                this.FinalDate = order.FinalDate;
                this.CreatedDate = order.CreatedDate;
                this.ModifiedDate = order.ModifiedDate;
                this.Location = order.Location;
                this.Factory = order.Factory;
                this.Product = order.Product;
                this.VehicleType = order.VehicleType;
                this.OriginalAmount = order.OriginalAmount;
                this.RequestedAmount = order.RequestedAmount;
                this.ReceivedAmount = order.ReceivedAmount;
                this.LoadedAmount = order.LoadedAmount;
                this.TankNum = order.TankNum;
                this.TankVolume = order.TankVolume;
                this.TankLevel = order.TankLevel;
                this.Status = order.Status;
                this.Description = order.Description;
                this.Observations = order.Observations;
                this.HexColor = order.HexColor;
                this.SizeName = order.SizeName;
                this.TripId = order.TripId;
            }
        }

        public long Id { get; set; }

        public string Reference { get; set; }

        public Operator Operator { get; set; }
        public Client Client { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public LocationData Location { get; set; }

        public Factory Factory { get; set; }
        public Product Product { get; set; }
        public int VehicleType { get; set; }
        public int? OriginalAmount { get; set; }
        public int RequestedAmount { get; set; }
        public int? ReceivedAmount { get; set; }
        public int? LoadedAmount { get; set; }

        public string TankNum { get; set; }
        public string TankVolume { get; set; }
        public string TankLevel { get; set; }

        public int Status { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public int? TripId { get; set; }


        //TXAPUZKA
        public string HexColor { get; set; }
        public bool IsLastDay { get; set; }

        //TXAPUZKA
        public string SizeName { get; set; }
    }
}