namespace EtasaDesktop.Distribution.Planner.Map
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using EtasaDesktop.Common.Data;
    using EtasaDesktop.Resources.DrapAndDrop;
    using Microsoft.Maps.MapControl.WPF;

    public partial class MarkerClient : UserControl
    {
       
        private MarkerClientViewModel _viewModel;
        public Location localizacion = null;

        public MarkerClient(MarkerClientViewModel viewModel, ref float floatlogitudeNoFind)
        {
            InitializeComponent();
            _viewModel = viewModel;
            MapLayer.SetPositionOrigin(this, PositionOrigin.BottomCenter);
            new ListViewDragDropManager<Order>(ListClientOrders);
            Punto puntofactoria = new Punto();
            //spain latitude 40.4637 north , 3.7492 West
            //coordenad y 
            puntofactoria.latitude = _viewModel.Client.Location.Latitude;
            //coordenada x
            puntofactoria.longitude = _viewModel.Client.Location.Longitude;

            //44.488444, -10.267331 (left-up)                   
            //42.476091, 5.416559 (right-up)
            //36.199297, -11.641810 (left-down)
            //35.339791, 1.732230 (right-down) 

            //Limitacion esquinas españa(recuadro en google maps) 
            // (left-up)-------------(right-up)
            // (left-down)-----------(right-down)  

            //esquina de arriba a la izquierda 
            Punto leftup = new Punto
            {
                latitude = Convert.ToSingle(44.488444),
                longitude = Convert.ToSingle(-10.267331)
            };

            //esquina derecha de arriba 
            Punto rightup = new Punto
            {
                latitude = Convert.ToSingle(42.476091),
                longitude = Convert.ToSingle(5.416559)
            };

            //esquina de abajo izquierda  
            Punto leftdown = new Punto
            {
                latitude = Convert.ToSingle(36.199297),
                longitude = Convert.ToSingle(-11.641810)
            };

            //esquina de abajo derecha 
            Punto rightdown = new Punto
            {
                latitude = Convert.ToSingle(35.3397912),
                longitude = Convert.ToSingle(1.732230)
            };

            //miramos si el punto obtenido esta  dentro del rectangulo definido
            //si es asi dejamos la factoria en su lugar 
            if (PointInRectangle(puntofactoria, leftup, rightup, leftdown, rightdown))
            {
                localizacion = new Location(_viewModel.Location.Latitude, _viewModel.Location.Longitude);
                MapLayer.SetPosition(this, new Location(_viewModel.Client.Location.Latitude, _viewModel.Client.Location.Longitude));
            }
            //si google no sabe colocarlos correctamente los coloca arriba
            else
            {
                MapLayer.SetPosition(this, new Location(43.879502, floatlogitudeNoFind));
             
                floatlogitudeNoFind = floatlogitudeNoFind + Convert.ToSingle(0.5);
            }         
            this.DataContext = _viewModel;
        }

        //metodo para saber si un punto esta dentro de un recuad
        bool PointInRectangle(Punto pt, Punto leftup, Punto rightup, Punto leftdown, Punto rightdown)
        {
            bool ok = true;

            //si sobre paso el punto mas alto del recuadro o voy por debajo del punto mas bajo del recuadro en el eje x 
            if (pt.latitude > leftup.latitude || pt.latitude < rightdown.latitude || pt.longitude < leftdown.longitude || pt.longitude > rightup.longitude)
            {
                ok = false;
            }

            return ok;
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            //OrderFormWindow orderForm = new OrderFormWindow();
            //orderForm.Order = _viewModel.Orders[0];
            //orderForm.ShowDialog();
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            //ClientFormWindow clientForm = new ClientFormWindow();
            //clientForm.ShowDialog();
        }
    }
}
