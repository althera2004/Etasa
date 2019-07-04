namespace EtasaDesktop.Distribution.Planner.Map
{
    using System;
    using System.Windows.Controls;
    using Microsoft.Maps.MapControl.WPF;

    public partial class MarkerFactory : UserControl
    {
        private MarkerFactoryViewModel _viewModel;
        public Location localizacion = null;

        public MarkerFactory(MarkerFactoryViewModel viewModel, ref double floatlogitudeNoFind)
        {
            InitializeComponent();
            _viewModel = viewModel;
            MapLayer.SetPositionOrigin(this, PositionOrigin.BottomCenter);

            var puntofactoria = new Punto
            {
                //spain latitude 40.4637 north , 3.7492 West
                //coordenad y
                latitudeDouble = _viewModel.Location.Latitude,
                //coordenada x
                longitudeDouble = _viewModel.Location.Longitude
            }; 

            //esquina de arriba a la izquierda 
            var leftup = new Punto
            {
                latitude = Convert.ToSingle(44.488444),
                longitude = Convert.ToSingle(-10.267331)
            };

            //esquina derecha de arriba 
            var rightup = new Punto
            {
                latitude = Convert.ToSingle(42.476091),
                longitude = Convert.ToSingle(5.416559)
            };

            //esquina de abajo izquierda  
            var leftdown = new Punto
            {
                latitude = Convert.ToSingle(36.199297),
                longitude = Convert.ToSingle(-11.641810)
            };

            //esquina de abajo derecha 
            var rightdown = new Punto
            {
                latitude = Convert.ToSingle(35.3397912),
                longitude = Convert.ToSingle(1.732230)
            };

            //miramos si el punto obtenido está dentro del rectángulo definido
            //si es asi dejamos la factoria en su lugar 
            //Pintaremos solo las que podamos mostrar en el mapa
            if (PointInRectangle(puntofactoria, leftup, rightup, leftdown, rightdown))
            {
                localizacion = new Location(_viewModel.Location.Latitude, _viewModel.Location.Longitude);

                // JUAN CASTILLA - No volve a crear el objeto
                //MapLayer.SetPosition(this, new Location(_viewModel.Location.Latitude, _viewModel.Location.Longitude));
                MapLayer.SetPosition(this, new Location(localizacion));
            }
            //si google no sabe colocarlos correctamente los coloca arriba
            else
            {
                MapLayer.SetPosition(this, new Location(44.5, floatlogitudeNoFind));
                floatlogitudeNoFind = floatlogitudeNoFind + Convert.ToSingle(0.5);
            }

            this.DataContext = _viewModel;
        }

        //metodo para saber si un punto esta dentro de un recuadro
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