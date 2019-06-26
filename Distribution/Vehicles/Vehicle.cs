using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Vehicles
{
    class Vehicle : INotifyPropertyChanged
    {
        #region Atributos
        public int _id;
        public string _codigo;
        public string _tipoVehiculo;
        public int _tipo_Cam;
        public string _matricula;
        public string _bastidor;
        public string _nodoSalida;
        public string _nodoLlegada;
        public string _marcaVehiculo;
        public string _modelo;
        public int _numEjes;
        public int _tara;
        public int _pma;
        public string _fechaMatriculacion;
        public string _fechaFabricacion;
        public string _antiguedad;
        public int _empresa;
        public string _tipoTarjeta;
        public string _fechaTarjTrans;
        public string _fechaTarjTransAnt;
        public string _fechaTac;
        public string _fechaTacAnt;
        public string _fechaItv;
        public string _fechaItvAnt;
        public string _fechaTpc;
        public string _fechaTpcAnt;
        public string _numPoliza;
        public string _fechaSeg;
        public string _fechaSegAnt;
        public string _imagen;
        public string _fechaBaja;
        public string _fechaAlta;
        public int _tanque1;
        public int _tanque2;
        public int _tanque3;
        public int _tanque4;
        public int _tanque5;
        public int _tanque6;
        public int _tanque7;
        public int _tanque8;
        public int _tanque9;
        public int _tanque10;
        public int _tanque11;
        public int _llave;
        public int _calorifugada;
        public string _fechaCalibra;
        public string _fechaCalibraAnt;
        public int _bomba;
        public int _contador;
        public string _fechaCalCont;
        public string _fechaCalContAnt;
        public string _tipoManguera;
        public string _observaciones;
        public string _fechaLimitador;
        public string _fechaLimitadorAnt;
        public int _pinzas;
        public int _recuperaVapores;
        public int _valvulaSeg;
        public int _numExtintor1;
        public string _fechaExtintor1;
        public string _fechaExtintor1Ant;
        public int _numExtintor2;
        public string _fechaExtintor2;
        public string _fechaExtintor2Ant;
        public int _numExtintor3;
        public string _fechaExtintor3;
        public string _fechaExtintor3Ant;
        public string _codigoZona;
        public int _precintado;
        public string _horaInicio;
        public string _horaFinal;
        public string _codigoAlquilado;
        public int _pmaAutorizado;
        public int _idEquipoMedicion;
        public int _idVehiculoGps;
        public int _pmaEspecial;
        public string _fechaADR;
        public string _codigoPruebaADR;
        public string _codigoCentroCoste;
        public bool _paralizado;
        public string _motivoParalizacion;
        public string _fechaRevisionManguera;
        public string _fechaRevisionMangueraAnt;
        public bool _remolqueExtensible;
        public string _fechaFinalPermisoComunitario;
        public string _codigoProveedorGps;
        public string _codigoTipoRemolque;
        public string _fechaVencimientoCrm;
        public string _fechaVencimientoCrmAnt;
        public bool _tmv;
        public string _fechaRevision;
        public string _fechaRevisionAnt;
        public string _fechaCalTerAnt;
        public string _fechaCalTer;
        public string _fechaPol;
        public string _fechaPolAnt;
        #endregion

        #region Getters/Setters
        #region Datos Principales
        public int Empresa
        {
            get { return _empresa; }
            set
            {
                if (value != _empresa)
                {
                    _empresa = value;
                    Notify("Empresa");
                }
            }
        }

        public string TipoVehiculo
        {
            get { return _tipoVehiculo; }
            set
            {
                if (value != _tipoVehiculo)
                {
                    _tipoVehiculo = value;
                    Notify("TipoVehiculo");
                }
            }
        }

        public string MarcaVehiculo
        {
            get { return _marcaVehiculo; }
            set
            {
                if (value != _marcaVehiculo)
                {
                    _marcaVehiculo = value;
                    Notify("MarcaVehiculo");
                }
            }
        }

        //Tamaño
        /*public int Empresa
        {
            get { return _empresa; }
            set
            {
                if (value != _empresa)
                {
                    _empresa = value;
                    Notify("Empresa");
                }
            }
        }*/

        public string Modelo
        {
            get { return _modelo; }
            set
            {
                if (value != _modelo)
                {
                    _modelo = value;
                    Notify("Modelo");
                }
            }
        }

        public string Matricula
        {
            get { return _matricula; }
            set
            {
                if (value != _matricula)
                {
                    _matricula = value;
                    Notify("Matricula");
                }
            }
        }

        public string Bastidor
        {
            get { return _bastidor; }
            set
            {
                if (value != _bastidor)
                {
                    _bastidor = value;
                    Notify("Bastidor");
                }
            }
        }

        public int NumEjes
        {
            get { return _numEjes; }
            set
            {
                if (value != _numEjes)
                {
                    _numEjes = value;
                    Notify("NumEjes");
                }
            }
        }

        public int Tara
        {
            get { return _tara; }
            set
            {
                if (value != _tara)
                {
                    _tara = value;
                    Notify("Tara");
                }
            }
        }

        public int Pma
        {
            get { return _pma; }
            set
            {
                if (value != _pma)
                {
                    _pma = value;
                    Notify("Pma");
                }
            }
        }

        public int PmaAutorizado
        {
            get { return _pmaAutorizado; }
            set
            {
                if (value != _pmaAutorizado)
                {
                    _pmaAutorizado = value;
                    Notify("PmaAutorizado");
                }
            }
        }

        public int PmaEspecial
        {
            get { return _pmaEspecial; }
            set
            {
                if (value != _pmaEspecial)
                {
                    _pmaEspecial = value;
                    Notify("PmaEspecial");
                }
            }
        }

        public string FechaMatriculacion
        {
            get { return _fechaMatriculacion; }
            set
            {
                if (value != _fechaMatriculacion)
                {
                    _fechaMatriculacion = value;
                    Notify("FechaMatriculacion");
                }
            }
        }

        public string FechaFabricacion
        {
            get { return _fechaFabricacion; }
            set
            {
                if (value != _fechaFabricacion)
                {
                    _fechaFabricacion = value;
                    Notify("FechaFabricacion");
                }
            }
        }

        public string FechaAlta
        {
            get { return _fechaAlta; }
            set
            {
                if (value != _fechaAlta)
                {
                    _fechaAlta = value;
                    Notify("FechaAlta");
                }
            }
        }

        public string FechaBaja
        {
            get { return _fechaBaja; }
            set
            {
                if (value != _fechaBaja)
                {
                    _fechaBaja = value;
                    Notify("FechaBaja");
                }
            }
        }
        
        public string FechaRevision
        {
            get { return _fechaRevision; }
            set
            {
                if (value != _fechaRevision)
                {
                    _fechaRevision = value;
                    Notify("FechaRevision");
                }
            }
        }
    
        public string FechaRevisionAnt
        {
            get { return _fechaRevisionAnt; }
            set
            {
                if (value != _fechaRevisionAnt)
                {
                    _fechaRevisionAnt = value;
                    Notify("FechaRevisionAnt");
                }
            }
        }


        public string FechaItv
        {
            get { return _fechaItv; }
            set
            {
                if (value != _fechaItv)
                {
                    _fechaItv = value;
                    Notify("FechaItv");
                }
            }
        }

        public string FechaItvAnt
        {
            get { return _fechaItvAnt; }
            set
            {
                if (value != _fechaItvAnt)
                {
                    _fechaItvAnt = value;
                    Notify("FechaItvAnt");
                }
            }
        }

        public string FechaSeg
        {
            get { return _fechaSeg; }
            set
            {
                if (value != _fechaSeg)
                {
                    _fechaSeg = value;
                    Notify("FechaSeg");
                }
            }
        }

        public string FechaSegAnt
        {
            get { return _fechaSegAnt; }
            set
            {
                if (value != _fechaSegAnt)
                {
                    _fechaSegAnt = value;
                    Notify("FechaSegAnt");
                }
            }
        }

        public string FechaPol
        {
            get { return _fechaPol; }
            set
            {
                if (value != _fechaPol)
                {
                    _fechaPol = value;
                    Notify("FechaPol");
                }
            }
        }

        public string FechaPolAnt
        {
            get { return _fechaPolAnt; }
            set
            {
                if (value != _fechaPolAnt)
                {
                    _fechaPolAnt = value;
                    Notify("FechaPolAnt");
                }
            }
        }

        //Ultima
        /*public int Empresa
        {
            get { return _empresa; }
            set
            {
                if (value != _empresa)
                {
                    _empresa = value;
                    Notify("Empresa");
                }
            }
        }*/

        //Proxima
        /*public int Empresa
        {
            get { return _empresa; }
            set
            {
                if (value != _empresa)
                {
                    _empresa = value;
                    Notify("Empresa");
                }
            }
        }*/

        //Categorias
        /*public int Empresa
        {
            get { return _empresa; }
            set
            {
                if (value != _empresa)
                {
                    _empresa = value;
                    Notify("Empresa");
                }
            }
        }*/

        public string CodigoProveedorGps
        {
            get { return _codigoProveedorGps; }
            set
            {
                if (value != _codigoProveedorGps)
                {
                    _codigoProveedorGps = value;
                    Notify("CodigoProveedorGps");
                }
            }
        }

        public int IdVehiculoGps
        {
            get { return _idVehiculoGps; }
            set
            {
                if (value != _idVehiculoGps)
                {
                    _idVehiculoGps = value;
                    Notify("IdVehiculoGps");
                }
            }
        }

        public string Antiguedad
        {
            get { return _antiguedad; }
            set
            {
                if (value != _antiguedad)
                {
                    _antiguedad = value;
                    Notify("Antiguedad");
                }
            }
        }

        public string CodigoAlquilado
        {
            get { return _codigoAlquilado; }
            set
            {
                if (value != _codigoAlquilado)
                {
                    _codigoAlquilado = value;
                    Notify("CodigoAlquilado");
                }
            }
        }

        public string CodigoZona
        {
            get { return _codigoZona; }
            set
            {
                if (value != _codigoZona)
                {
                    _codigoZona = value;
                    Notify("CodigoZona");
                }
            }
        }

        public string CodigoCentroCoste
        {
            get { return _codigoCentroCoste; }
            set
            {
                if (value != _codigoCentroCoste)
                {
                    _codigoCentroCoste = value;
                    Notify("CodigoCentroCoste");
                }
            }
        }
        
        public string NodoSalida
        {
            get { return _nodoSalida; }
            set
            {
                if (value != _nodoSalida)
                {
                    _nodoSalida = value;
                    Notify("NodoSalida");
                }
            }
        }
        
        public string NodoLlegada
        {
            get { return _nodoLlegada; }
            set
            {
                if (value != _nodoLlegada)
                {
                    _nodoLlegada = value;
                    Notify("NodoLlegada");
                }
            }
        }

        public string HoraInicio
        {
            get { return _horaInicio; }
            set
            {
                if (value != _horaInicio)
                {
                    _horaInicio = value;
                    Notify("HoraInicio");
                }
            }
        }

        public string HoraFinal
        {
            get { return _horaFinal; }
            set
            {
                if (value != _horaFinal)
                {
                    _horaFinal = value;
                    Notify("HoraFinal");
                }
            }
        }

        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                if (value != _observaciones)
                {
                    _observaciones = value;
                    Notify("Observaciones");
                }
            }
        }

        public bool Paralizado
        {
            get { return _paralizado; }
            set
            {
                if (value != _paralizado)
                {
                    _paralizado = value;
                    Notify("Paralizado");
                }
            }
        }

        public string MotivoParalizacion
        {
            get { return _motivoParalizacion; }
            set
            {
                if (value != _motivoParalizacion)
                {
                    _motivoParalizacion = value;
                    Notify("MotivoParalizacion");
                }
            }
        }

        //Operadores autorizados
        /*public int Empresa
        {
            get { return _empresa; }
            set
            {
                if (value != _empresa)
                {
                    _empresa = value;
                    Notify("Empresa");
                }
            }
        }*/

        public string Imagen
        {
            get { return _imagen; }
            set
            {
                if (value != _imagen)
                {
                    _imagen = value;
                    Notify("Imagen");
                }
            }
        }

        //Cargadores
        /*public int Empresa
        {
            get { return _empresa; }
            set
            {
                if (value != _empresa)
                {
                    _empresa = value;
                    Notify("Empresa");
                }
            }
        }*/

        //Tipos cliente no autorizado
        /*public int Empresa
        {
            get { return _empresa; }
            set
            {
                if (value != _empresa)
                {
                    _empresa = value;
                    Notify("Empresa");
                }
            }
        }*/
        #endregion
        #region Datos Tractora
        public string TipoTarjeta
        {
            get { return _tipoTarjeta; }
            set
            {
                if (value != _tipoTarjeta)
                {
                    _tipoTarjeta = value;
                    Notify("TipoTarjeta");
                }
            }
        }

        public bool TMV
        {
            get { return _tmv; }
            set
            {
                if (value != _tmv)
                {
                    _tmv = value;
                    Notify("TMV");
                }
            }
        }

        public int NumExtintor1
        {
            get { return _numExtintor1; }
            set
            {
                if (value != _numExtintor1)
                {
                    _numExtintor1 = value;
                    Notify("NumExtintor1");
                }
            }
        }

        public string FechaExtintor1
        {
            get { return _fechaExtintor1; }
            set
            {
                if (value != _fechaExtintor1)
                {
                    _fechaExtintor1 = value;
                    Notify("FechaExtintor1");
                }
            }
        }

        public string FechaExtintor1Ant
        {
            get { return _fechaExtintor1Ant; }
            set
            {
                if (value != _fechaExtintor1Ant)
                {
                    _fechaExtintor1Ant = value;
                    Notify("FechaExtintor1Ant");
                }
            }
        }

        public string FechaVencimientoCrm
        {
            get { return _fechaVencimientoCrm; }
            set
            {
                if (value != _fechaVencimientoCrm)
                {
                    _fechaVencimientoCrm = value;
                    Notify("FechaVencimientoCrm");
                }
            }
        }

        public string FechaVencimientoCrmAnt
        {
            get { return _fechaVencimientoCrmAnt; }
            set
            {
                if (value != _fechaVencimientoCrmAnt)
                {
                    _fechaVencimientoCrmAnt = value;
                    Notify("FechaVencimientoCrmAnt");
                }
            }
        }

        public string FechaTarjTrans
        {
            get { return _fechaTarjTrans; }
            set
            {
                if (value != _fechaTarjTrans)
                {
                    _fechaTarjTrans = value;
                    Notify("FechaTarjTrans");
                }
            }
        }

        public string FechaTarjTransAnt
        {
            get { return _fechaTarjTransAnt; }
            set
            {
                if (value != _fechaTarjTransAnt)
                {
                    _fechaTarjTransAnt = value;
                    Notify("FechaTarjTransAnt");
                }
            }
        }

        public string FechaTac
        {
            get { return _fechaTac; }
            set
            {
                if (value != _fechaTac)
                {
                    _fechaTac = value;
                    Notify("FechaTac");
                }
            }
        }

        public string FechaTacAnt
        {
            get { return _fechaTacAnt; }
            set
            {
                if (value != _fechaTacAnt)
                {
                    _fechaTacAnt = value;
                    Notify("FechaTacAnt");
                }
            }
        }

        public string FechaLimitador
        {
            get { return _fechaLimitador; }
            set
            {
                if (value != _fechaLimitador)
                {
                    _fechaLimitador = value;
                    Notify("FechaLimitador");
                }
            }
        }

        public string FechaLimitadorAnt
        {
            get { return _fechaLimitadorAnt; }
            set
            {
                if (value != _fechaLimitadorAnt)
                {
                    _fechaLimitadorAnt = value;
                    Notify("FechaLimitadorAnt");
                }
            }
        }

        public string FechaFinalPermisoComunitario
        {
            get { return _fechaFinalPermisoComunitario; }
            set
            {
                if (value != _fechaFinalPermisoComunitario)
                {
                    _fechaFinalPermisoComunitario = value;
                    Notify("FechaFinalPermisoComunitario");
                }
            }
        }
        #endregion
        #region Datos Remolque

        public string CodigoTipoRemolque
        {
            get { return _codigoTipoRemolque; }
            set
            {
                if (value != _codigoTipoRemolque)
                {
                    _codigoTipoRemolque = value;
                    Notify("CodigoTipoRemolque");
                }
            }
        }

        public bool RemolqueExtensible
        {
            get { return _remolqueExtensible; }
            set
            {
                if (value != _remolqueExtensible)
                {
                    _remolqueExtensible = value;
                    Notify("RemolqueExtensible");
                }
            }
        }
        
        public int NumExtintor2
        {
            get { return _numExtintor2; }
            set
            {
                if (value != _numExtintor2)
                {
                    _numExtintor2 = value;
                    Notify("NumExtintor2");
                }
            }
        }

        public string FechaExtintor2
        {
            get { return _fechaExtintor2; }
            set
            {
                if (value != _fechaExtintor2)
                {
                    _fechaExtintor2 = value;
                    Notify("FechaExtintor2");
                }
            }
        }

        public string FechaExtintor2Ant
        {
            get { return _fechaExtintor2Ant; }
            set
            {
                if (value != _fechaExtintor2Ant)
                {
                    _fechaExtintor2Ant = value;
                    Notify("FechaExtintor2Ant");
                }
            }
        }
        #endregion
        #region Datos Cisterna
        public string TipoManguera
        {
            get { return _tipoManguera; }
            set
            {
                if (value != _tipoManguera)
                {
                    _tipoManguera = value;
                    Notify("TipoManguera");
                }
            }
        }

        public int IdEquipoMedicion
        {
            get { return _idEquipoMedicion; }
            set
            {
                if (value != _idEquipoMedicion)
                {
                    _idEquipoMedicion = value;
                    Notify("IdEquipoMedicion");
                }
            }
        }

        public string FechaRevisionManguera
        {
            get { return _fechaRevisionManguera; }
            set
            {
                if (value != _fechaRevisionManguera)
                {
                    _fechaRevisionManguera = value;
                    Notify("FechaRevisionManguera");
                }
            }
        }

        public string FechaRevisionMangueraAnt
        {
            get { return _fechaRevisionMangueraAnt; }
            set
            {
                if (value != _fechaRevisionMangueraAnt)
                {
                    _fechaRevisionMangueraAnt = value;
                    Notify("FechaRevisionMangueraAnt");
                }
            }
        }

        public string FechaADR
        {
            get { return _fechaADR; }
            set
            {
                if (value != _fechaADR)
                {
                    _fechaADR = value;
                    Notify("FechaADR");
                }
            }
        }

        public string CodigoPruebaADR
        {
            get { return _codigoPruebaADR; }
            set
            {
                if (value != _codigoPruebaADR)
                {
                    _codigoPruebaADR = value;
                    Notify("CodigoPruebaADR");
                }
            }
        }

        public string FechaCalibra
        {
            get { return _fechaCalibra; }
            set
            {
                if (value != _fechaCalibra)
                {
                    _fechaCalibra = value;
                    Notify("FechaCalibra");
                }
            }
        }

        public string FechaCalibraAnt
        {
            get { return _fechaCalibraAnt; }
            set
            {
                if (value != _fechaCalibraAnt)
                {
                    _fechaCalibraAnt = value;
                    Notify("FechaCalibraAnt");
                }
            }
        }

        public string FechaCalCont
        {
            get { return _fechaCalCont; }
            set
            {
                if (value != _fechaCalCont)
                {
                    _fechaCalCont = value;
                    Notify("FechaCalCont");
                }
            }
        }

        public string FechaCalContAnt
        {
            get { return _fechaCalContAnt; }
            set
            {
                if (value != _fechaCalContAnt)
                {
                    _fechaCalContAnt = value;
                    Notify("FechaCalContAnt");
                }
            }
        }

        public string FechaCalTerAnt
        {
            get { return _fechaCalTerAnt; }
            set
            {
                if (value != _fechaCalTerAnt)
                {
                    _fechaCalTerAnt = value;
                    Notify("FechaCalTerAnt");
                }
            }
        }

        public string FechaCalTer
        {
            get { return _fechaCalTer; }
            set
            {
                if (value != _fechaCalTer)
                {
                    _fechaCalTer = value;
                    Notify("FechaCalTer");
                }
            }
        }

        public int Llave
        {
            get { return _llave; }
            set
            {
                if (value != _llave)
                {
                    _llave = value;
                    Notify("Llave");
                }
            }
        }

        public int ValvulaSeg
        {
            get { return _valvulaSeg; }
            set
            {
                if (value != _valvulaSeg)
                {
                    _valvulaSeg = value;
                    Notify("ValvulaSeg");
                }
            }
        }

        public int Precintado
        {
            get { return _precintado; }
            set
            {
                if (value != _precintado)
                {
                    _precintado = value;
                    Notify("Precintado");
                }
            }
        }

        public int Bomba
        {
            get { return _bomba; }
            set
            {
                if (value != _bomba)
                {
                    _bomba = value;
                    Notify("Bomba");
                }
            }
        }

        public int Pinzas
        {
            get { return _pinzas; }
            set
            {
                if (value != _pinzas)
                {
                    _pinzas = value;
                    Notify("Pinzas");
                }
            }
        }

        public int RecuperaVapores
        {
            get { return _recuperaVapores; }
            set
            {
                if (value != _recuperaVapores)
                {
                    _recuperaVapores = value;
                    Notify("RecuperaVapores");
                }
            }
        }

        public int Calorifugada
        {
            get { return _calorifugada; }
            set
            {
                if (value != _calorifugada)
                {
                    _calorifugada = value;
                    Notify("Calorifugada");
                }
            }
        }

        public int Contador
        {
            get { return _contador; }
            set
            {
                if (value != _contador)
                {
                    _contador = value;
                    Notify("Contador");
                }
            }
        }
        
        #endregion

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
    }
}
