﻿using EtasaDesktop.Common.Data;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EtasaDesktop.Distribution.Planner.Map
{
    public partial class MarkerFactory : UserControl
    {

        private MarkerFactoryViewModel _viewModel;
        public Location localizacion = null;

        public MarkerFactory(MarkerFactoryViewModel viewModel, ref double floatlogitudeNoFind)
        {
            InitializeComponent();
            _viewModel = viewModel;
            MapLayer.SetPositionOrigin(this, PositionOrigin.BottomCenter);

            Punto puntofactoria = new Punto();
            //spain latitude 40.4637 north , 3.7492 West
            //coordenad y
            puntofactoria.latitudeDouble = _viewModel.Location.Latitude;
            //coordenada x
            puntofactoria.longitudeDouble = _viewModel.Location.Longitude;

            //44.488444, -10.267331 (left-up)                   
            //42.476091, 5.416559 (right-up)
            //36.199297, -11.641810 (left-down)
            //35.339791, 1.732230 (right-down) 

            //Limitacion esquinas españa(recuadro en google maps) 
            // (left-up)-------------(right-up)
            // (left-down)-----------(right-down)  

            //esquina de arriba a la izquierda 
            Punto leftup = new Punto();
            leftup.latitude = Convert.ToSingle(44.488444);
            leftup.longitude = Convert.ToSingle(-10.267331);

            //esquina derecha de arriba 
            Punto rightup = new Punto();
            rightup.latitude = Convert.ToSingle(42.476091);
            rightup.longitude = Convert.ToSingle(5.416559);

            //esquina de abajo izquierda  
            Punto leftdown = new Punto();
            leftdown.latitude = Convert.ToSingle(36.199297);
            leftdown.longitude = Convert.ToSingle(-11.641810);

            //esquina de abajo derecha 
            Punto rightdown = new Punto();
            rightdown.latitude = Convert.ToSingle(35.3397912);
            rightdown.longitude = Convert.ToSingle(1.732230);

            //miramos si el punto obtenido esta  dentro del rectangulo definido
            //si es asi dejamos la factoria en su lugar 
            //Pintaremos solo las que podamos mostrar en el mapa
            if (PointInRectangle(puntofactoria, leftup, rightup, leftdown, rightdown))
            {
                localizacion = new Location(_viewModel.Location.Latitude, _viewModel.Location.Longitude);
                MapLayer.SetPosition(this, new Location(_viewModel.Location.Latitude, _viewModel.Location.Longitude));
            }
            //si google no sabe colocarlos correctamente los coloca arriba
            else
            {
                MapLayer.SetPosition(this, new Location(44.5, floatlogitudeNoFind));
                floatlogitudeNoFind = floatlogitudeNoFind + Convert.ToSingle(0.5);
            }
            
    
            this.DataContext = _viewModel;
        }

        //metodo para saber si un punto esta dentro de un recuad
        bool PointInRectangle(Punto pt, Punto leftup, Punto rightup, Punto leftdown, Punto rightdown)
        {
            bool ok = true;

            //si sobre paso el punto mas alto del recuadro o voy por debajo del punto mas bajo del recuadro en el eje x 
            if (pt.latitudeDouble > leftup.latitude || pt.latitudeDouble < rightdown.latitude || pt.longitudeDouble < leftdown.longitude || pt.longitudeDouble > rightup.longitude)
            {
                ok = false;
            }

            return ok;
        }
    }
}