using System;
using System.Data;
using System.Windows;
using EtasaDesktop.Distribution.Assignments;
using System.Collections.Generic;
using System.Collections;

namespace EtasaDesktop.Common.Tools
{
    public partial class SearchElementWindow : Window
    {
        private SearchElementViewModel _viewModel;
        public DataRow Result { get; private set; }

        public bool conductorClick;
        public string nombreConductor;
        public string codigoConductor;
        public int IdConductor;

        public bool tractoraClick;
        public string licenciatractora;
        public string codigoTractora;
        public int IdTractora;

        public bool trailerClick;
        public string codigoTrailer;
        public string MatriculaTrailer;
        public string CapacidadTrailer;
        public int IdTrailer;


        public SearchElementWindow(SearchElementViewModel searchElementViewModel)
        {
            _viewModel = searchElementViewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            object returnObject = _viewModel.SelectedItem;
            string test = "";


            if (returnObject != null)
            {
                 test = _viewModel.SelectedItem.ToString();

                //Conductor
                if (conductorClick)
                {

                    if (String.IsNullOrEmpty(test))
                    {
                        this.codigoConductor = "";
                        this.nombreConductor = "";
                    }
                    else
                    {
                        int index2 = test.IndexOf(',');
                        this.codigoConductor = test.Substring(11, (index2) - 11);
                        string aux1 = test.Substring(test.IndexOf("Nombre"));
                        int index3 = aux1.IndexOf(',');
                        this.nombreConductor = aux1.Substring(9, (index3) - 9);
                        string aux3 = test.Substring(test.IndexOf("Id"));
                        int index4 = aux3.IndexOf('}');
                        IdConductor = Convert.ToInt32(aux3.Substring(5, (index4) - 5).ToString());

                    }

                    conductorClick = false;

                }


                //tractora
                if (tractoraClick)
                {

                    if (String.IsNullOrEmpty(test))
                    {
                        this.codigoTractora = "";
                        this.licenciatractora = "";
                    }
                    else
                    {
                        int index2 = test.IndexOf(',');
                        this.codigoTractora = test.Substring(11, (index2) - 11);
                        string aux1 = test.Substring(test.IndexOf("Matricula"));
                        int index3 = aux1.IndexOf(',');
                        this.licenciatractora = aux1.Substring(12, (index3) - 12);
                        string aux3 = test.Substring(test.IndexOf("Id"));
                        int index4 = aux3.IndexOf('}');
                        IdTractora = Convert.ToInt32(aux3.Substring(5, (index4) - 5).ToString());

                    }
                    tractoraClick = false;
                }

                //trailer
                if (trailerClick)
                {

                    if (String.IsNullOrEmpty(test))
                    {
                        this.codigoTrailer = "";
                        this.MatriculaTrailer = "";
                        this.CapacidadTrailer = "";
                    }
                    else
                    {
                        int index2 = test.IndexOf(',');
                        this.codigoTrailer = test.Substring(11, (index2) - 11);
                        string aux1 = test.Substring(test.IndexOf("Matricula"));
                        int index3 = aux1.IndexOf(',');
                        this.MatriculaTrailer = aux1.Substring(12, (index3) - 12);
                        string aux2 = test.Substring(test.IndexOf("Capacidad"));
                        int index4 = aux2.IndexOf(',');
                        this.CapacidadTrailer = aux2.Substring(12, (index4) - 12);
                        string aux4 = test.Substring(test.IndexOf("Id"));
                        int index5 = aux4.IndexOf('}');
                        IdTrailer = Convert.ToInt32(aux4.Substring(5, (index5) - 5).ToString());

                    }
                    trailerClick = false;
                }

                Result = _viewModel.SelectedRow;

                DialogResult = true;
            }
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
