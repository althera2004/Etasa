using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    public class ImportData
    {

        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int OperatorId { get; set; }
        public string FactoryCode { get; set; }
        public string Reference { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string ClientCif { get; set; }
        public string Contact { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string TankVolume { get; set; }
        public string TankNum { get; set; }
        public string TankLevel { get; set; }
        public string Amount { get; set; }
        public string ProductCode { get; set; }
        public string MeasureUnit { get; set; }
        public string VehicleSizeCode { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }






        //Cepsa (.dat) 
        //parte en común en registro de cabecera/primer/segundo/tercer/cuarto orden de suministro 
        //Numero de envio 
        public string Num_Envio;

        //parte en común en registro de cabecera/primer/segundo/tercer/cuarto orden de suministro (canvian los valores segun la linea) 
        //(registro cabecera)
        //Literal 
        public string Literal00;
        //Contador
        public string Contador00;
        //Tipo 
        public string Tipo00;

        //(primer orden de suministro)
        //Literal
        public string Literal05;
        //Contador
        public string Contador05;
        //Tipo 
        public string Tipo05;

        //(segundo orden de suministro)
        //Literal
        public string Literal06;
        //Contador
        public string Contador06;
        //Tipo 
        public string Tipo06;


        //(tercer orden de suministro)
        //Literal
        public string Literal07;
        //Contador
        public string Contador07;
        //Tipo 
        public string Tipo07;

        //(cuarto orden de suministro)
        //Literal
        public string Literal14;
        //Contador
        public string Contador14;
        //Tipo 
        public string Tipo14;

        //campo en común en /primer/segundo/tercer/cuarto orden de suministro 
        //(primer orden de suministro)
        public string Instalacion05;
        //(segundo orden de suministro)
        public string Instalacion06;
        //(tercer orden de suministro)
        public string Instalacion07;
        //(cuarto orden de suministro)
        public string Instalacion14;

        //campo en común en registro de cabecera/cuarto orden de suministro 
        //Operador (registro cabecera)
        public string Operador00;
        //Operador (cuarto orden de suministro )
        public string Operador14;

        //campo en común en registro de cabecera/cuarto orden de suministro 
        //Suministrador
        public string Suministrador00;
        public string Suministrador14;

        //campo en común en registro de cabecera/cuarto orden de suministro 
        //Transportista
        public string Transportista00;
        public string Transportista14;

        //campo en común en registro de cabecera/primer orden de suministro 
        //Condiciones
        public string Condiciones05;
        public string Condiciones14;


        //Fecha
        public string Fecha;
        //Codigo Cepsa 
        public string Cod_Cepsa;
        //Cliente
        public string Cliente;
        //Destinatario
        public string Destinatario;
        //Cae
        public string Cae;
        //Registro Fiscal
        public string Reg_Fiscal;
        //IIEE
        public string IEE;
        //Fecha desde
        public string Fecha_Desde;
        //Fecha hasta
        public string Fecha_Hasta;
        //Duracion
        public string Duracion;
        //Hora_desde
        public string Hora_Desde;
        //Hora_hasta
        public string Hora_hasta;
        //Contacto 
        public string Contacto;
        //Cargo
        public string Cargo;
        //Telefono
        public string Telefono;
        //Fax
        public string Fax;
        //Nif
        public string Nif;
        //Calle1
        public string Calle1;
        //Calle2
        public string Calle2;
        //Calle3
        public string Calle3;
        //Municipio
        public string Municipio;
        //Localidad
        public string Localidad;
        //Código postal
        public string Cod_Postal;
        //Provincia
        public string Provincia;
        //Pais
        public string Pais;
        //Codigo postal internacional
        public string Cod_Post_Int;
        //Region
        public string Region;
        //Ruta
        public string Ruta;
        //Pedido
        public string Pedido;
        //Material
        public string Material;
        //Cantidad
        public string Cantidad;
        //Unidad de Medida
        public string Uni_Med;
        //Fecha Inicial
        public string Fecha_Inicial;
        //Fecha Final
        public string Fecha_Final;
        //Tipo operador
        public string Tipo_Oper;
        //Centro
        public string Centro;
        //Horario
        public string Horario;
        //Observaciones 1 
        public string Observaciones1;
        //Observaciones 2 
        public string Observaciones2;
        //Ultima descarga
        public string Utima_Descarga;
        //Volumen 1
        public string Volumen_1;
        //volumen 2
        public string Volumen_2;
        //volumen 3
        public string Volumen_3;
        //Nivel del deposito 1 
        public string Nivel_Dep1;
        //Nivel del deposito 2
        public string Nivel_Dep2;
        //Nivel del deposito 3
        public string Nivel_Dep3;
        //LLenado
        public string Llenado;
        //Vaciado
        public string Vaciado;


        //Metodos para obtener y asignar valores a los atributos 
        //numero de envio
        public string GetNumEnvio()
        {
            return this.Num_Envio;
        }
        public void SetNumEnvio(string NumEnvio)
        {
            this.Num_Envio = NumEnvio;
        }

        //(metodos registro cabecera)
        //Literal 
        public string GetLiteral00()
        {
            return this.Literal00;
        }
        public void SetLiteral00(string Literal00)
        {
            this.Literal00 = Literal00;
        }
        //contador
        public string GetContador00()
        {
            return this.Contador00;
        }
        public void SetContador00(string Contador00)
        {
            this.Contador00 = Contador00;
        }
        //Tipo
        public string GetTipo00()
        {
            return this.Tipo00;
        }
        public void SetTipo00(string Tipo00)
        {
            this.Tipo00 = Tipo00;
        }

        //(suministros primer orden)
        //Literal 
        public string GetLiteral05()
        {
            return this.Literal05;
        }
        public void SetLiteral05(string Literal05)
        {
            this.Literal05 = Literal05;
        }
        //contador
        public string GetContador05()
        {
            return this.Contador05;
        }
        public void SetContador05(string Contador05)
        {
            this.Contador05 = Contador05;
        }
        //Tipo
        public string GetTipo05()
        {
            return this.Tipo05;
        }
        public void SetTipo05(string Tipo05)
        {
            this.Tipo05 = Tipo05;
        }

        //(suministros segundo orden)
        //Literal 
        public string GetLiteral06()
        {
            return this.Literal06;
        }
        public void SetLiteral06(string Literal06)
        {
            this.Literal06 = Literal06;
        }
        //contador
        public string GetContador06()
        {
            return this.Contador06;
        }
        public void SetContador06(string Contador06)
        {
            this.Contador06 = Contador06;
        }
        //Tipo
        public string GetTipo06()
        {
            return this.Tipo06;
        }
        public void SetTipo06(string Tipo06)
        {
            this.Tipo06 = Tipo06;
        }

        //(suministros tercer orden)
        //Literal 
        public string GetLiteral07()
        {
            return this.Literal07;
        }
        public void SetLiteral07(string Literal07)
        {
            this.Literal07 = Literal07;
        }
        //contador
        public string GetContador07()
        {
            return this.Contador07;
        }
        public void SetContador07(string Contador07)
        {
            this.Contador07 = Contador07;
        }
        //Tipo
        public string GetTipo07()
        {
            return this.Tipo07;
        }
        public void SetTipo07(string Tipo07)
        {
            this.Tipo07 = Tipo07;
        }

        //(suministros cuarto orden)
        //Literal 
        public string GetLiteral14()
        {
            return this.Literal14;
        }
        public void SetLiteral14(string Literal14)
        {
            this.Literal14 = Literal14;
        }
        //contador
        public string GetContador14()
        {
            return this.Contador14;
        }
        public void SetContador14(string Contador14)
        {
            this.Contador14 = Contador14;
        }
        //Tipo
        public string GetTipo14()
        {
            return this.Tipo14;
        }
        public void SetTipo14(string Tipo14)
        {
            this.Tipo14 = Tipo14;
        }

        //instalacion (suministro primer orden)
        public string GetInstalacion05()
        {
            return this.Instalacion05;
        }
        public void SetInstalacion05(string Instalacion05)
        {
            this.Instalacion05 = Instalacion05;
        }
        //instalacion (suministro segundo orden )
        public string GetInstalacion06()
        {
            return this.Instalacion06;
        }
        public void SetInstalacion06(string Instalacion06)
        {
            this.Instalacion06 = Instalacion06;
        }
        //instalacion (suministro tercer orden )
        public string GetInstalacion07()
        {
            return this.Instalacion07;
        }
        public void SetInstalacion07(string Instalacion07)
        {
            this.Instalacion07 = Instalacion07;
        }
        //instalacion (suministro cuarto orden )
        public string GetInstalacion14()
        {
            return this.Instalacion14;
        }
        public void SetInstalacion14(string Instalacion14)
        {
            this.Instalacion14 = Instalacion14;
        }

        //operador (cabecera) 
        public string GetOperador00()
        {
            return this.Operador00;
        }
        public void SetOperador00(string Operador00)
        {
            this.Operador00 = Operador00;
        }
        //operador (suministro cuarto orden )
        public string GetOperador14()
        {
            return this.Operador14;
        }
        public void SetOperador14(string Operador14)
        {
            this.Operador14 = Operador14;
        }

        //Suministador (cabecera) 
        public string GetSuministrador00()
        {
            return this.Suministrador00;
        }
        public void SetSuministrador00(string Suministrador00)
        {
            this.Suministrador00 = Suministrador00;
        }
        //Suministador (suministro cuarto orden )
        public string GetSuministrador14()
        {
            return this.Suministrador14;
        }
        public void SetSuministrador14(string Suministrador14)
        {
            this.Suministrador14 = Suministrador14;
        }

        //transportista (cabecera) 
        public string GetTransportista00()
        {
            return this.Transportista00;
        }
        public void SetTransportista00(string Transportista00)
        {
            this.Transportista00 = Transportista00;
        }

        //transportista  (suministro cuarto orden )
        public string GetTransportista14()
        {
            return this.Transportista14;
        }
        public void SetTransportista14(string Transportista14)
        {
            this.Transportista14 = Transportista14;
        }

        //condiciones (suministro primer orden )
        public string GetCondiciones05()
        {
            return this.Condiciones05;
        }
        public void SetCondiciones05(string Condiciones05)
        {
            this.Condiciones05 = Condiciones05;
        }

        //condiciones (suministro cuarto orden )
        public string GetCondiciones14()
        {
            return this.Condiciones14;
        }
        public void SetCondiciones14(string Condiciones14)
        {
            this.Condiciones14 = Condiciones14;
        }

        //fecha
        public string GetFecha()
        {
            return this.Fecha;
        }
        public void SetFecha(string Fecha)
        {
            this.Fecha = Fecha;
        }
        //codigo cepsa
        public string GetCodCepsa()
        {
            return this.Cod_Cepsa;
        }
        public void SetCodCepsa(string CodCepsa)
        {
            this.Cod_Cepsa = CodCepsa;
        }
        //cliente
        public string GetCliente()
        {
            return this.Cliente;
        }
        public void SetCliente(string Cliente)
        {
            this.Cliente = Cliente;
        }
        //destinatario
        public string GetDestinatario()
        {
            return this.Destinatario;
        }
        public void SetDestinatario(string Destinatario)
        {
            this.Destinatario = Destinatario;
        }
        //cae
        public string GetCae()
        {
            return this.Cae;
        }
        public void SetCae(string Cae)
        {
            this.Cae = Cae;
        }
        //registro fiscal
        public string GetRegistroFiscal()
        {
            return this.Reg_Fiscal;
        }
        public void SetRegistroFiscal(string RegistroFiscal)
        {
            this.Reg_Fiscal = RegistroFiscal;
        }
        //IEE
        public string GetIee()
        {
            return this.IEE;
        }
        public void SetIee(string IEE)
        {
            this.IEE = IEE;
        }
        //fecha desde
        public string GetFechaDesde()
        {
            return this.Fecha_Desde;
        }
        public void SetFechaDesde(string FechaDesde)
        {
            this.Fecha_Desde = FechaDesde;
        }
        //fecha hasta
        public string GetFechaHasta()
        {
            return this.Fecha_Hasta;
        }
        public void SetFechaHasta(string FechaHasta)
        {
            this.Fecha_Hasta = FechaHasta;
        }
        //duracion
        public string GetDuracion()
        {
            return this.Duracion;
        }
        public void SetDuracion(string Duracion)
        {
            this.Duracion = Duracion;
        }
        //hora_desde
        public string GetHoraDesde()
        {
            return this.Hora_Desde;
        }
        public void SetHoraDesde(string HoraDesde)
        {
            this.Hora_Desde = HoraDesde;
        }
        //hora_hasta
        public string GetHoraHasta()
        {
            return this.Hora_hasta;
        }
        public void SetHoraHasta(string HoraHasta)
        {
            this.Hora_hasta = HoraHasta;
        }
        //observacions 1 
        public string GetObservaciones1()
        {
            return this.Observaciones1;
        }
        public void SetObservaciones1(string Observaciones1)
        {
            this.Observaciones1 = Observaciones1;
        }
        //observacions 2
        public string GetObservaciones2()
        {
            return this.Observaciones2;
        }
        public void SetObservaciones2(string Observaciones2)
        {
            this.Observaciones2 = Observaciones2;
        }
        //contacto
        public string GetContacto()
        {
            return this.Contacto;
        }
        public void SetContacto(string Contacto)
        {
            this.Contacto = Contacto;
        }
        //Cargo
        public string GetCargo()
        {
            return this.Cargo;
        }
        public void SetCargo(string Cargo)
        {
            this.Cargo = Cargo;
        }
        //telefono
        public string GetTelefono()
        {
            return this.Telefono;
        }
        public void SetTelefono(string Telefono)
        {
            this.Telefono = Telefono;
        }
        //fax
        public string GetFax()
        {
            return this.Fax;
        }
        public void SetFax(string Fax)
        {
            this.Fax = Fax;
        }
        //Nif
        public string GetNif()
        {
            return this.Nif;
        }
        public void SetNif(string Nif)
        {
            this.Nif = Nif;
        }
        //calle1
        public string GetCalle1()
        {
            return this.Calle1;
        }
        public void SetCalle1(string Calle1)
        {
            this.Calle1 = Calle1;
        }
        //calle2
        public string GetCalle2()
        {
            return this.Calle2;
        }
        public void SetCalle2(string Calle2)
        {
            this.Calle2 = Calle2;
        }
        //calle3
        public string GetCalle3()
        {
            return this.Calle3;
        }
        public void SetCalle3(string Calle3)
        {
            this.Calle3 = Calle3;
        }

        //Municipio
        public string GetMunicipio()
        {
            return this.Municipio;
        }
        public void SetMunicipio(string Municipio)
        {
            this.Municipio = Municipio;
        }
        //Localidad
        public string GetLocalidad()
        {
            return this.Localidad;
        }
        public void SetLocalidad(string Localidad)
        {
            this.Localidad = Localidad;
        }
        //codigo postal
        public string GetCodigoPostal()
        {
            return this.Cod_Postal;
        }
        public void SetCodigoPostal(string CodigoPostal)
        {
            this.Cod_Postal = CodigoPostal;
        }
        //provincia
        public string GetProvincia()
        {
            return this.Provincia;
        }
        public void SetProvincia(string Provincia)
        {
            this.Provincia = Provincia;
        }
        //pais
        public string GetPais()
        {
            return this.Pais;
        }
        public void Setpais(string Pais)
        {
            this.Pais = Pais;
        }
        //codigo postal internacional
        public string GetCodigoPostalInternacional()
        {
            return this.Cod_Post_Int;
        }
        public void SetCodigoPostalInternacional(string CodigoPostalInternacional)
        {
            this.Cod_Post_Int = CodigoPostalInternacional;
        }
        //region
        public string GetRegion()
        {
            return this.Region;
        }
        public void SetRegion(string Region)
        {
            this.Region = Region;
        }
        //ruta
        public string GetRuta()
        {
            return this.Ruta;
        }
        public void SetRuta(string Ruta)
        {
            this.Ruta = Ruta;
        }
        //pedido
        public string GetPedido()
        {
            return this.Pedido;
        }
        public void SetPedido(string Pedido)
        {
            this.Pedido = Pedido;
        }
        //Material
        public string GetMaterial()
        {
            return this.Material;
        }
        public void SetMaterial(string Material)
        {
            this.Material = Material;
        }
        //cantidad
        public string GetCantidad()
        {
            return this.Cantidad;
        }
        public void SetCantidad(string Cantidad)
        {
            this.Cantidad = Cantidad;
        }
        //unidad de medida
        public string GetUnidadDeMedida()
        {
            return this.Uni_Med;
        }
        public void SetUnidadDeMedida(string UnidadDeMedida)
        {
            this.Uni_Med = UnidadDeMedida;
        }
        //fecha incial
        public string GetFechaInicial()
        {
            return this.Fecha_Inicial;
        }
        public void SetFechaInicial(string FechaInicial)
        {
            this.Fecha_Inicial = FechaInicial;
        }
        //fecha final
        public string GetFechaFinal()
        {
            return this.Fecha_Final;
        }
        public void SetFechaFinal(string FechaFinal)
        {
            this.Fecha_Final = FechaFinal;
        }
        //tipo de operador
        public string GetTipoDeOperador()
        {
            return this.Tipo_Oper;
        }
        public void SetTipoDeOperador(string TipoDeOperador)
        {
            this.Tipo_Oper = TipoDeOperador;
        }
        //centro
        public string GetCentro()
        {
            return this.Centro;
        }
        public void SetCentro(string Centro)
        {
            this.Centro = Centro;
        }
        //Horario
        public string GetHorario()
        {
            return this.Horario;
        }
        public void SetHorario(string Horario)
        {
            this.Horario = Horario;
        }
        //ultima descarga
        public string GetUltimaDescarga()
        {
            return this.Utima_Descarga;
        }
        public void SetUltimaDescarga(string UltimaDescarga)
        {
            this.Utima_Descarga = UltimaDescarga;
        }
        //volumen 1
        public string GetVolumen1()
        {
            return this.Volumen_1;
        }
        public void SetVolumen1(string Volumen1)
        {
            this.Volumen_1 = Volumen1;
        }
        //volumen 2
        public string GetVolumen2()
        {
            return this.Volumen_2;
        }
        public void SetVolumen2(string Volumen2)
        {
            this.Volumen_2 = Volumen2;
        }
        //volumen 3
        public string GetVolumen3()
        {
            return this.Volumen_3;
        }
        public void SetVolumen3(string Volumen3)
        {
            this.Volumen_3 = Volumen3;
        }
        //nivel de deposito 1
        public string GetNivelDeDeposito1()
        {
            return this.Nivel_Dep1;
        }
        public void SetNivelDeDeposito1(string NivelDeDeposito1)
        {
            this.Nivel_Dep1 = NivelDeDeposito1;
        }
        //nivel de deposito 2
        public string GetNivelDeDeposito2()
        {
            return this.Nivel_Dep2;
        }
        public void SetNivelDeDeposito2(string NivelDeDeposito2)
        {
            this.Nivel_Dep2 = NivelDeDeposito2;
        }
        //nivel de deposito 3
        public string GetNivelDeDeposito3()
        {
            return this.Nivel_Dep3;
        }
        public void SetNivelDeDeposito3(string NivelDeDeposito3)
        {
            this.Nivel_Dep3 = NivelDeDeposito3;
        }
        //Llenado
        public string GetLlenado()
        {
            return this.Llenado;
        }
        public void SetLlenado(string Llenado)
        {
            this.Llenado = Llenado;
        }
        //Vaciado
        public string GetVaciado()
        {
            return this.Vaciado;
        }
        public void SetVaciado(string Vaciado)
        {
            this.Vaciado = Vaciado;
        }
        //convertimos los datos del objeto para ser insertados en la base de datos 
        public void PutEmptyObject()
        {
            this.Literal05 = "";
            this.Literal06 = "";
            this.Literal07 = "";
            this.Literal14 = "";
            this.Contador05 = "";
            this.Contador06 = "";
            this.Contador07 = "";
            this.Contador14 = "";
            this.Tipo05 = "";
            this.Tipo06 = "";
            this.Tipo07 = "";
            this.Tipo14 = "";
            this.Instalacion05 = "";
            this.Instalacion06 = "";
            this.Instalacion07 = "";
            this.Instalacion14 = "";
            this.Operador14 = "";
            this.Suministrador14 = "";
            this.Transportista14 = "";
            this.Condiciones05 = "";
            this.Condiciones14 = "";
            this.Cod_Cepsa = "";
            this.Cliente = "";
            this.Destinatario = "";
            this.Cae = "";
            this.Reg_Fiscal = "";
            this.IEE = "";
            this.Fecha_Desde = "";
            this.Fecha_Hasta = "";
            this.Duracion = "";
            this.Hora_Desde = "";
            this.Hora_hasta = "";
            this.Contacto = "";
            this.Cargo = "";
            this.Telefono = "";
            this.Fax = "";
            this.Nif = "";
            this.Calle1 = "";
            this.Calle2 = "";
            this.Municipio = "";
            this.Localidad = "";
            this.Cod_Postal = "";
            this.Provincia = "";
            this.Pais = "";
            this.Cod_Post_Int = "";
            this.Region = "";
            this.Ruta = "";
            this.Pedido = "";
            this.Material = "";
            this.Cantidad = "";
            this.Uni_Med = "";
            this.Fecha_Inicial = "";
            this.Fecha_Final = "";
            this.Tipo_Oper = "";
            this.Centro = "";
            this.Horario = "";
            this.Observaciones1 = "";
            this.Observaciones2 = "";
            this.Utima_Descarga = "";
            this.Volumen_1 = "";
            this.Volumen_2 = "";
            this.Volumen_3 = "";
            this.Nivel_Dep1 = "";
            this.Nivel_Dep2 = "";
            this.Nivel_Dep3 = "";
            this.Llenado = "";
            this.Vaciado = "";
            this.StartDate = DateTime.Now;
            this.FinalDate = DateTime.Now;
            this.OperatorId = 500;
            this.FactoryCode = "";
            this.Reference = "";
            this.ClientCode = "";
            this.ClientName = "";
            this.ClientCif = "";
            this.Contact = "";
            this.City = "";
            this.Address = "";
            this.Email = "";
            this.Phone = "";
            this.Phone2 = "";
            this.Latitude = "";
            this.Longitude = "";
            this.TankVolume = "";
            this.TankNum = "";
            this.TankLevel = "";
            this.Amount = "";
            this.ProductCode = "";
            this.MeasureUnit = "";
            this.VehicleSizeCode = "";
            this.Description = "";
            this.Observations = "";
        }
        
        //obtener el identificador del vehiculo (vehicle_size) a partir de la primera letra del campo condiciones 
        private static int ConvertToVehicleSizeId(string Concidiones)
        {
            int identificadorVehiculo = 0;
            if (!string.IsNullOrEmpty(Concidiones))
            {
                string letraVehiculo = Concidiones.Substring(0, 1);

                switch (letraVehiculo)
                {
                    //Rígido 2 ejes
                    case "A":
                        identificadorVehiculo = 1;
                        break;
                    //Rígido 3 ejes
                    case "B":
                        identificadorVehiculo = 2;
                        break;
                    //Articulado 2 ejes
                    case "F":
                        identificadorVehiculo = 3;
                        break;
                    //Articulado 3 ejes
                    case "C":
                        identificadorVehiculo = 4;
                        break;
                        //El resto de casos se asigna como no definido 
                }
            }
            return identificadorVehiculo;
        }
 
        //conversor de codigo de provincia al nombre
        private static string CodeProvinceTostring(string CodeProvince)
        {

            string ProvincieName = "";
            switch (CodeProvince)
            {
                //insertamos registros de cabecera
                case "01":
                    ProvincieName = "Álava";
                    break;
                case "02":
                    ProvincieName = "Albacete";
                    break;
                case "03":
                    ProvincieName = "Alicante";
                    break;
                case "04":
                    ProvincieName = "Almería";
                    break;
                case "05":
                    ProvincieName = "Ávila";
                    break;
                case "06":
                    ProvincieName = "Badajoz";
                    break;
                case "07":
                    ProvincieName = "Palma de Mallorca";
                    break;
                case "08":
                    ProvincieName = "Barcelona";
                    break;
                case "09":
                    ProvincieName = "Burgos";
                    break;
                case "10":
                    ProvincieName = "Cáceres";
                    break;
                case "11":
                    ProvincieName = "Cádiz";
                    break;
                case "12":
                    ProvincieName = "Castellón";
                    break;
                case "13":
                    ProvincieName = "Ciudad Real";
                    break;
                case "14":
                    ProvincieName = " Córdoba";
                    break;
                case "15":
                    ProvincieName = "Coruña";
                    break;
                case "16":
                    ProvincieName = "Cuenca";
                    break;
                case "17":
                    ProvincieName = "Gerona";
                    break;
                case "18":
                    ProvincieName = "Granada";
                    break;
                case "19":
                    ProvincieName = "Guadalajara";
                    break;
                case "20":
                    ProvincieName = "Guipúzcoa";
                    break;
                case "21":
                    ProvincieName = "Huelva";
                    break;
                case "22":
                    ProvincieName = "Huesca";
                    break;
                case "23":
                    ProvincieName = "Jaén";
                    break;
                case "24":
                    ProvincieName = "León";
                    break;
                case "25":
                    ProvincieName = "Lleida";
                    break;
                case "26":
                    ProvincieName = "La Rioja";
                    break;
                case "27":
                    ProvincieName = "Lugo";
                    break;
                case "28":
                    ProvincieName = "Madrid";
                    break;
                case "29":
                    ProvincieName = "Málaga";
                    break;
                case "30":
                    ProvincieName = "Murcia";
                    break;
                case "31":
                    ProvincieName = "Navarra";
                    break;
                case "32":
                    ProvincieName = "Orense";
                    break;
                case "33":
                    ProvincieName = "Asturias";
                    break;
                case "34":
                    ProvincieName = "Palencia";
                    break;
                case "35":
                    ProvincieName = "Las Palmas";
                    break;
                case "36":
                    ProvincieName = "Pontevedra";
                    break;
                case "37":
                    ProvincieName = "Salamanca";
                    break;
                case "38":
                    ProvincieName = "Santa Cruz de Tenerife";
                    break;
                case "39":
                    ProvincieName = "Cantabria";
                    break;
                case "40":
                    ProvincieName = "Segovia";
                    break;
                case "41":
                    ProvincieName = "Sevilla";
                    break;
                case "42":
                    ProvincieName = "Soria";
                    break;
                case "43":
                    ProvincieName = "Tarragona";
                    break;
                case "44":
                    ProvincieName = "Teruel";
                    break;
                case "45":
                    ProvincieName = "Toledo";
                    break;
                case "46":
                    ProvincieName = "Valencia";
                    break;
                case "47":
                    ProvincieName = "Valladolid";
                    break;
                case "48":
                    ProvincieName = "Vizcaya";
                    break;
                case "49":
                    ProvincieName = "Zamora";
                    break;
                case "50":
                    ProvincieName = "Zaragoza";
                    break;
                case "51":
                    ProvincieName = "Ceuta";
                    break;
                case "52":
                    ProvincieName = "Melilla";
                    break;
            }

            return ProvincieName;
        }

        public void SetCabeceraOrder(string line)
        {
            //Literal
            this.SetLiteral00(line.Substring(0, 3).Trim());
            //Numero envio
            this.SetNumEnvio(line.Substring(3, 4).Trim());
            //Contador
            this.SetContador00(line.Substring(7, 4).Trim());
            //Tipo
            this.SetTipo00(line.Substring(11, 2).Trim());
            //Fecha
            this.SetFecha(line.Substring(13, 8).Trim());
            //Operador
            this.SetOperador00(line.Substring(21, 6).Trim());
            //Suministrador
            this.SetSuministrador00(line.Substring(27, 6).Trim());
            //Transportista
            this.SetTransportista00(line.Substring(33, 10).Trim());
        }


        //validamos que los campos de la consulta insert esten llenos 
        private static string ValidateNullOrEmpty(string parametro)
        {
            string parametrodevuelta = "";
            if (string.IsNullOrEmpty(parametro))
            {
                parametrodevuelta = " ";

            }
            else
            {
                parametrodevuelta = parametro;
            }
            return parametrodevuelta;
        }

        //Fomateo de la fecha 
        private static string FormatDate(string Date)
        {
            string Fechadevuelta = "";
            if (string.IsNullOrEmpty(Date))
            {
                Fechadevuelta = " ";

            }
            else
            {
                Fechadevuelta = Date.Substring(0, 4) + "-" + Date.Substring(4, 2) + "-" + Date.Substring(6, 2);
            }
            return Fechadevuelta;
        }


        //Convertir una fecha string a datetime 
        private static DateTime ConvertDataToDataTime(string Date)
        {
            DateTime FechaDateTime;
            string FechaFormateada = "";
            if (string.IsNullOrEmpty(Date))
            {
                FechaFormateada = "1999-01-01";
                FechaDateTime = DateTime.Parse(FechaFormateada);
            }
            else
            {
                FechaFormateada = FormatDate(Date);
                FechaDateTime = DateTime.Parse(FechaFormateada);
            }
            return FechaDateTime;
        }

        //control nulos en los valores numericos de cantidad 
        private static Int32 ConvertValuesRequestAmount(string RequestAmount)
        {
            Int32 ValorRequestAmount = 0;
            if (!string.IsNullOrEmpty(RequestAmount))
            {
                ValorRequestAmount = Convert.ToInt32(RequestAmount)/1000;
            }
            return ValorRequestAmount;
        }

        //control nulos en los valores numericos de volumen del deposito
        private static Double ConvertValuesVolumeDeposit(string Volume)
        {
            Double VolumeDeposit = 0;
            if (!string.IsNullOrEmpty(Volume))
            {
                VolumeDeposit = Convert.ToDouble(Volume);

            }
            return VolumeDeposit;
        }

        //control nulos en los valores numericos del nivel del deposito 
        private static Int32 ConvertValuesLevelDeposit(string level)
        {
            Int32 LevelDeposit = 0;
            if (!string.IsNullOrEmpty(level))
            {
                LevelDeposit = Convert.ToInt32(level);
            }
            return LevelDeposit;
        }


        //Tratamiento campo cliente 
        private static Int32 ConvertCliendField(string level)
        {
            Int32 LevelDeposit = 0;
            if (!string.IsNullOrEmpty(level))
            {
                LevelDeposit = Convert.ToInt32(level);
            }
            return LevelDeposit;
        }

        //obtención de la geolocalización (obtenemos latitud y longitud) 
        private static string[] Geocode(string direction, string postalCode, string province)
        {
            string[] gps = new string[2];
            string strURL = "";
            string strResult = "";
            HttpWebRequest wbrq; // = new HttpWebRequest();    
            HttpWebResponse wbrs; // = new HttpWebResponse ();
            StreamReader sr; // = new StreamReader();

            if (!direction.Trim().Equals(""))
            {
                string direccion2 = direction.Trim().ToLower();
                direccion2 = direccion2.Replace("c/", "");
                direccion2 = direccion2.Replace("av.", "");
                direccion2 = direccion2.Replace("calle", "");
                direccion2 = direccion2.Replace("carrer", "");
                direccion2 = direccion2.Replace("avinguda", "");
                direccion2 = direccion2.Replace("avenida", "");
                direccion2 = direccion2.Replace("pg.", "");

                string direccionparam = direccion2;

                direccionparam += " " + postalCode.Trim();

                if (!province.Trim().Equals(""))
                {
                    direccionparam += " " + province.Trim();
                }

            

                direccionparam = Uri.EscapeDataString(direccionparam);
      
                // Set the URL (and add any querystring values)   
                strURL = @"https://maps.google.com/maps/api/geocode/xml?key=AIzaSyDHwRTkASbhKZ0uZVioidhvM5Gs3Dw_eAs&address=" + direccionparam + "&sensor=false";

                // Create the web request   
                wbrq = (HttpWebRequest)WebRequest.Create(strURL);
                wbrq.Method = "GET";

                // Read the returned data    
                wbrs = (HttpWebResponse)wbrq.GetResponse();
                sr = new StreamReader(wbrs.GetResponseStream());
                strResult = sr.ReadToEnd().Trim();
                sr.Close();

                int nposini = strResult.IndexOf("<lat>") + 5;
                int nposfin = strResult.IndexOf("</lat>");

                string sLat = "";

                if (nposfin > nposini)
                    sLat = strResult.Substring(nposini, (nposfin - nposini));


                nposini = strResult.IndexOf("<lng>") + 5;
                nposfin = strResult.IndexOf("</lng>");

                string sLng = "";
                if (nposfin > nposini)
                    sLng = strResult.Substring(nposini, (nposfin - nposini));

                if (!string.IsNullOrEmpty(sLat.Replace(".", ",")))
                    gps[0] = sLat.Replace(".", ",");
                else
                    gps[0] = "0.0";
                if (!string.IsNullOrEmpty(sLng.Replace(".", ",")))
                    gps[1] = sLng.Replace(".", ",");
                else
                    gps[1] = "0.0";

                /*
                   <lat>41.5488204</lat>
                    <lng>1.8604082</lng>
                 */
            }
            return gps;
        }

        public void InsertDatasOrderView(int expirationDay, string latitud, string longitude)
        {
            this.StartDate = ConvertDataToDataTime(this.Fecha_Inicial);
            this.FinalDate = this.StartDate.AddDays(expirationDay);
            this.OperatorId = 500;
            this.FactoryCode = this.Centro;
            this.Reference = this.Pedido;
            this.ClientCode = this.Cliente;
            this.ClientName = this.Destinatario;
            this.ClientCif = this.Nif;
            this.Contact = this.Contacto;
            this.City = "";
            this.Address = this.Calle1;
            this.PostCode = this.Cod_Postal;
            this.Email = "";
            this.Phone = this.Telefono;
            this.Phone2 = "";
            this.Latitude = latitud;
            this.Longitude = longitude;
            this.TankVolume = this.Volumen_1;
            this.TankNum = "1";
            this.TankLevel = this.Nivel_Dep1;
            this.Amount = this.Cantidad;
            this.ProductCode = this.Material;
            this.MeasureUnit = this.Uni_Med;
            this.VehicleSizeCode = ConvertToVehicleSizeCode(this.Condiciones05.Substring(0, 1));
            this.Description = "";
            this.Observations = this.Observaciones1;

        }

        //conversion condiciones media
        private static string ConvertToVehicleSizeCode(string Concidiones)
        {
            string identificadorVehiculo = "";
            if (!string.IsNullOrEmpty(Concidiones))
            {
                string letraVehiculo = Concidiones.Substring(0, 1);

                switch (letraVehiculo)
                {
                    //Rígido 2 ejes
                    case "A":
                        identificadorVehiculo = "3";
                        break;
                    //Rígido 3 ejes
                    case "B":
                        identificadorVehiculo = "2";
                        break;
                    //Articulado 2 ejes
                    case "F":
                        identificadorVehiculo = "1";
                        break;
                    //Articulado 3 ejes
                    case "C":
                        identificadorVehiculo = "";
                        break;
                        //El resto de casos se asigna como no definido 
                }
            }
            return identificadorVehiculo;
        }
        //declaración del destructor del objeto
        ~ImportData()
        {
        }
    } 
    //campos añadido
}
