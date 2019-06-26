using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    public class ImportHelper
    {
        public static IEnumerable<ImportData> FromCSV(FileInfo file, ImportConfiguration configuration, int expirationDays)
        {
            StreamReader reader = new StreamReader(file.FullName, Encoding.Default);
            List<ImportData> orders = new List<ImportData>();

            int rows = 0;
            while (reader.Peek() > 0)
            {

                string lStrLine = reader.ReadLine();
                rows++;

                if (configuration.IgnoreFirstRow && rows == 1)
                    continue;

                byte[] bytes = Encoding.UTF8.GetBytes(lStrLine);
                lStrLine = Encoding.UTF8.GetString(bytes);
                if (lStrLine == null)
                    break;
                if (lStrLine.Trim() == "")
                    continue;
                string[] lArrStrCells = null;
                lArrStrCells = lStrLine.Split(configuration.Delimiter);
                if (lArrStrCells == null)
                    continue;

                ImportData order = new ImportData();
                order.StartDate = Convert.ToDateTime(lArrStrCells[1]);
                order.FinalDate = order.StartDate.AddDays(expirationDays);
                order.FactoryCode = lArrStrCells[21];
                order.Reference = lArrStrCells[0];
                order.ClientCode = lArrStrCells[3];
                order.ClientName = lArrStrCells[2];
                order.ClientCif = lArrStrCells[5];
                order.Contact = lArrStrCells[4];
                order.City = lArrStrCells[6];
                order.PostCode = lArrStrCells[7];
                order.Address = lArrStrCells[8] + " " + lArrStrCells[9];
                order.Email = lArrStrCells[10];
                var phone = lArrStrCells[11];
                if (phone.Contains("/"))
                {
                    String[] split = phone.Split('/');
                    order.Phone = split[0];
                    order.Phone2 = split[1];
                }
                else
                {
                    order.Phone = phone;
                }
                order.Latitude = lArrStrCells[19];
                order.Longitude = lArrStrCells[20];
                order.TankVolume = lArrStrCells[12];
                order.TankNum = lArrStrCells[14];
                order.TankLevel = lArrStrCells[15];
                order.Amount = lArrStrCells[16];
                order.ProductCode = lArrStrCells[17];
                order.MeasureUnit = "KG";
                order.VehicleSizeCode = lArrStrCells[22];
                order.Description = lArrStrCells[18];

                orders.Add(order);
            }
            reader.Close();
            return orders;
        }

        public static IEnumerable<ImportData> FromExcel(FileInfo file, ImportConfiguration configuration, int expirationDays)
        {
            List<ImportData> orders = new List<ImportData>();

            using (ExcelPackage package = new ExcelPackage(file))
            {                
                int numeropaginas = package.Workbook.Worksheets.Count;

                if (numeropaginas > 1)
                {
                    MessageBox.Show("El documento Excel no puede tener más de una página");
                    return null;
                }

                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                var startRow = 2;
                for (int rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                {
                    var worksheetRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];

                    if (configuration.IgnoreFirstRow && rowNum == 1)
                        continue;

                    ImportData order = new ImportData();
                    var date = DateTime.ParseExact(worksheetRow["AD" + rowNum].Text.PadLeft(4, '0'), "ddMM", null);
                    order.StartDate = date > DateTime.Today ? date.AddYears(-1) : date;
                    order.FinalDate = order.StartDate.AddDays(expirationDays);
                    order.FactoryCode = worksheetRow["AF" + rowNum].Text;
                    order.Reference = worksheetRow["C" + rowNum].Text;
                    order.ClientCode = worksheetRow["A" + rowNum].Text;
                    order.ClientName = worksheetRow["L" + rowNum].Text;
                    order.ClientCif = worksheetRow["D" + rowNum].Text;
                    order.Contact = worksheetRow["AN" + rowNum].Text;
                    order.City = worksheetRow["O" + rowNum].Text;
                    order.PostCode = worksheetRow["N" + rowNum].Text;
                    order.Address = worksheetRow["M" + rowNum].Text;
                    order.Email = null;
                    var phone = worksheetRow["AM" + rowNum].Text;
                    if (phone.Contains("/"))
                    {
                        String[] split = phone.Split('/');
                        order.Phone = split[0];
                        order.Phone2 = split[1];
                    }
                    else
                    {
                        order.Phone = phone;
                    }


                    string[] gps = Geocode2(order.Address, order.PostCode, order.City);

                    if(string.IsNullOrEmpty(gps[0]) && string.IsNullOrEmpty(gps[1]))
                    {
                        order.Latitude = "0";
                        order.Longitude = "0";
                    }
                    else
                    {
                        order.Latitude = gps[0];
                        order.Longitude = gps[1];
                    }  
                    order.TankVolume = "0";
                    order.TankNum = worksheetRow["R" + rowNum].Text;
                    order.TankLevel = worksheetRow["AG" + rowNum].Text;
                    order.Amount = worksheetRow["AH" + rowNum].Text;
                    order.ProductCode = "GALP";
                    order.MeasureUnit = "KG";
                    order.VehicleSizeCode = "3";
                    order.Description = worksheetRow["AI" + rowNum].Text + " " + worksheetRow["AL" + rowNum].Text;

                    orders.Add(order);
                }

                return orders;
            }
        }


        private static string[] Geocode2(string direction, string postalCode, string city)
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

                if (!city.Trim().Equals(""))
                {
                    direccionparam += " " + city.Trim();
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





        public static IEnumerable<ImportData> FromDat(FileInfo file, ImportConfiguration configuration, int expirationDays)
        {

            List<ImportData> orders = new List<ImportData>();
            string line = "";
            Boolean final = false;
            ImportData Orden = new ImportData();
            ImportData OrdenCabecera = new ImportData();
            int VersionEnvioPedido = 0;
            string cabecera = "";
            int contadorlinias = -1;
            try
            {
             
                //leemos el fichero seleccionado linea por linea 
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file.FullName))
                {

                    while ((line = sr.ReadLine()) != null && !final)
                    {   
                        //contamos linias leidas 
                        contadorlinias = contadorlinias + 1;
                        //segun el tipo de registro insertamos unos campos o otros al objeto creado 
                        string Tipo = line.Substring(11, 2).Trim();
                        //cada 4 filas insertamos los datos sin contar la cabecera inicial tipo 00

                        switch (Tipo)
                        {
                            //registro primer orden de suministro
                            case "00":
                                cabecera = line;
                                break;
                            case "05":
                                //Literal
                                Orden.SetLiteral05(line.Substring(0, 3).Trim());
                                //Numero envio
                                Orden.SetNumEnvio(line.Substring(3, 4).Trim());
                                //Contador
                                Orden.SetContador05(line.Substring(7, 4).Trim());
                                //Tipo
                                Orden.SetTipo05(line.Substring(11, 2).Trim());
                                //Instalacion
                                Orden.SetInstalacion05(line.Substring(13, 8).Trim());
                                //Codigo cepsa
                                Orden.SetCodCepsa(line.Substring(21, 9).Trim());
                                //Cliente
                                Orden.SetCliente(line.Substring(30, 8).Trim());
                                //Destinatario
                                Orden.SetDestinatario(line.Substring(38, 40).Trim());
                                //Cae
                                Orden.SetCae(line.Substring(78, 12).Trim());
                                //Registro Fiscal
                                Orden.SetRegistroFiscal(line.Substring(90, 1).Trim());
                                //IEE
                                Orden.SetIee(line.Substring(91, 10).Trim());
                                //Fecha Desde
                                Orden.SetFechaDesde(line.Substring(101, 8).Trim());
                                //Fecha Hasta
                                Orden.SetFechaHasta(line.Substring(109, 8).Trim());
                                //Duracion
                                Orden.SetDuracion(line.Substring(117, 3).Trim());
                                //Condiciones
                                Orden.SetCondiciones05(line.Substring(120, 5).Trim());
                                //Hora Desde
                                Orden.SetHoraDesde(line.Substring(125, 4).Trim());
                                //Hora Hasta
                                Orden.SetHoraHasta(line.Substring(129, 4).Trim());
                                //Observaciones 1
                                Orden.SetObservaciones1(line.Substring(133, 30).Trim());
                                //Observaciones 2 
                                Orden.SetObservaciones2(line.Substring(163, 30).Trim());
                                break;
                            //registro segundo orden de suministro 
                            case "06":
                                //Literal
                                Orden.SetLiteral06(line.Substring(0, 3).Trim());
                                //Numero envio
                                Orden.SetNumEnvio(line.Substring(3, 4).Trim());
                                //Contador
                                Orden.SetContador06(line.Substring(7, 4).Trim());
                                //Tipo
                                Orden.SetTipo06(line.Substring(11, 2).Trim());
                                //Instalacion
                                Orden.SetInstalacion06(line.Substring(13, 9).Trim());
                                //Contacto
                                Orden.SetContacto(line.Substring(22, 30).Trim());
                                //Cargo
                                Orden.SetCargo(line.Substring(52, 30).Trim());
                                //Telefono
                                Orden.SetTelefono(line.Substring(82, 15).Trim());
                                //Fax
                                Orden.SetFax(line.Substring(97, 15).Trim());
                                //Nif
                                Orden.SetNif(line.Substring(112, 14).Trim());
                                break;
                            //registro tercer orden de suministro 
                            case "07":
                                //Literal
                                Orden.SetLiteral07(line.Substring(0, 3).Trim());
                                //Numero envio
                                Orden.SetNumEnvio(line.Substring(3, 4).Trim());
                                //Contador
                                Orden.SetContador07(line.Substring(7, 4).Trim());
                                //Tipo
                                Orden.SetTipo07(line.Substring(11, 2).Trim());
                                //Instalacion
                                Orden.SetInstalacion07(line.Substring(13, 9).Trim());
                                //Calle 1
                                Orden.SetCalle1(line.Substring(22, 35).Trim());
                                //Calle 2
                                Orden.SetCalle2(line.Substring(57, 35).Trim());
                                //Calle 3
                                Orden.SetCalle3(line.Substring(92, 35).Trim());
                                //Municipio
                                Orden.SetMunicipio(line.Substring(127, 5).Trim());
                                //Localidad
                                Orden.SetLocalidad(line.Substring(132, 35).Trim());
                                //Codigo Postal
                                Orden.SetCodigoPostal(line.Substring(167, 5).Trim());
                                //Provincia
                                Orden.SetProvincia(line.Substring(172, 2).Trim());
                                //Pais
                                Orden.Setpais(line.Substring(174, 3).Trim());
                                //Codigo postal internacional
                                Orden.SetCodigoPostalInternacional(line.Substring(177, 10).Trim());
                                //Region
                                Orden.SetRegion(line.Substring(187, 3).Trim());
                                //Ruta
                                Orden.SetRuta(line.Substring(193, 3).Trim());
                                break;
                            //registro cuarto orden de suministro 
                            case "14":
                                //Literal
                                Orden.SetLiteral14(line.Substring(0, 3).Trim());
                                //Numero envio
                                Orden.SetNumEnvio(line.Substring(3, 4).Trim());
                                //Contador
                                Orden.SetContador14(line.Substring(7, 4).Trim());
                                //Tipo
                                Orden.SetTipo14(line.Substring(11, 2).Trim());
                                //Operador
                                Orden.SetOperador14(line.Substring(13, 6).Trim());
                                //Suministrador
                                Orden.SetSuministrador14(line.Substring(19, 6).Trim());

                                if (VersionEnvioPedido == 0)
                                {
                                    //Transportista
                                    Orden.SetTransportista14(line.Substring(25, 6).Trim());
                                    //Pedido
                                    Orden.SetPedido(line.Substring(31, 15).Trim());
                                    //Instalacion
                                    Orden.SetInstalacion14(line.Substring(47, 10).Trim());
                                    //Material
                                    Orden.SetMaterial(line.Substring(57, 9).Trim());
                                    //Cantidad
                                    Orden.SetCantidad(line.Substring(66, 11).Trim());
                                    //Unidad de medida
                                    Orden.SetUnidadDeMedida(line.Substring(77, 2).Trim());
                                    //Fecha incial
                                    Orden.SetFechaInicial(line.Substring(79, 8).Trim());
                                    //Fecha Final
                                    Orden.SetFechaFinal(line.Substring(87, 8).Trim());
                                    //Tipo de operacion
                                    Orden.SetTipoDeOperador(line.Substring(95, 1).Trim());
                                    //Centro 
                                    Orden.SetCentro(line.Substring(96, 4).Trim());
                                    //Condiciones
                                    Orden.SetCondiciones14(line.Substring(100, 5).Trim());
                                    //Horario
                                    Orden.SetHorario(line.Substring(105, 8).Trim());
                                    //obervaciones 1
                                    Orden.SetObservaciones1(line.Substring(113, 30).Trim());
                                    //observaciones 2
                                    Orden.SetObservaciones2(line.Substring(143, 30).Trim());
                                    //ultima descarga
                                    Orden.SetUltimaDescarga(line.Substring(173, 1).Trim());
                                    //volumen 1
                                    Orden.SetVolumen1(line.Substring(174, 6).Trim());
                                    //volumen 2
                                    Orden.SetVolumen2(line.Substring(180, 6).Trim());
                                    //volumne 3
                                    Orden.SetVolumen3(line.Substring(186, 6).Trim());
                                    //nivel de deposito 1
                                    Orden.SetNivelDeDeposito1(line.Substring(192, 2).Trim());
                                    //nivel de deposito 2
                                    Orden.SetNivelDeDeposito2(line.Substring(194, 2).Trim());
                                    //nivel de deposito 3
                                    Orden.SetNivelDeDeposito3(line.Substring(196, 2).Trim());
                                    //llenado
                                    Orden.SetLlenado(line.Substring(198, 1).Trim());
                                    //vaciado
                                    Orden.SetVaciado(line.Substring(199, 1).Trim());
                                }
                                else if (VersionEnvioPedido == 1)
                                {
                                    //Transportista
                                    Orden.SetTransportista14(line.Substring(25, 10).Trim());
                                    //Pedido
                                    Orden.SetPedido(line.Substring(35, 15).Trim());
                                    //Instalacion
                                    Orden.SetInstalacion14(line.Substring(51, 10).Trim());
                                    //Material
                                    Orden.SetMaterial(line.Substring(61, 9).Trim());
                                    //Cantidad
                                    Orden.SetCantidad(line.Substring(70, 11).Trim());
                                    //Unidad de medida
                                    Orden.SetUnidadDeMedida(line.Substring(81, 2).Trim());
                                    //Fecha incial
                                    Orden.SetFechaInicial(line.Substring(83, 8).Trim());
                                    //Fecha Final
                                    Orden.SetFechaFinal(line.Substring(91, 8).Trim());
                                    //Tipo de operacion
                                    Orden.SetTipoDeOperador(line.Substring(99, 1).Trim());
                                    //Centro 
                                    Orden.SetCentro(line.Substring(100, 4).Trim());
                                    //Condiciones
                                    Orden.SetCondiciones14(line.Substring(104, 5).Trim());
                                    //Horario
                                    Orden.SetHorario(line.Substring(109, 8).Trim());
                                    //obervaciones 1
                                    Orden.SetObservaciones1(line.Substring(117, 30).Trim());
                                    //observaciones 2
                                    Orden.SetObservaciones2(line.Substring(147, 30).Trim());
                                    //ultima descarga
                                    Orden.SetUltimaDescarga(line.Substring(177, 1).Trim());
                                    //volumen 1
                                    Orden.SetVolumen1(line.Substring(178, 6).Trim());
                                    //volumen 2
                                    Orden.SetVolumen2(line.Substring(184, 6).Trim());
                                    //volumne 3
                                    Orden.SetVolumen3(line.Substring(190, 6).Trim());
                                    //nivel de deposito 1
                                    Orden.SetNivelDeDeposito1(line.Substring(196, 2).Trim());
                                    //nivel de deposito 2
                                    Orden.SetNivelDeDeposito2(line.Substring(198, 2).Trim());
                                    //nivel de deposito 3
                                    Orden.SetNivelDeDeposito3(line.Substring(200, 2).Trim());
                                    //llenado
                                    Orden.SetLlenado(line.Substring(202, 1).Trim());
                                    //vaciado
                                    Orden.SetVaciado(line.Substring(203, 1).Trim());
                                }                              
                                break;
                            //marca finalización fichero 
                            case "91":
                                final = true;
                                break;
                        }

                        //insertamos los datos del objeto en base de datos(de todos los registros cabecera/suministros orden 1,2,3,4) 
                        if (contadorlinias == 4)
                        {
                            //insertamos la cabecera por el rgistro a introducir (order, order_Cepsa)  
                            Orden.SetCabeceraOrder(cabecera);
                            //obtenemos los datos de geolocalización de latitud y longitud de google maps. 
                            string direccion = Orden.Calle1;
                            string provincia = CodeProvinceTostring(Orden.Provincia).Trim();
                            string codigopostal = Orden.Cod_Postal;
                            string latitud = "0";
                            string longitud = "0";
                            string[] gps = Geocode(direccion, codigopostal, provincia);

                            //validación de los valores de geocalización
                            if (!string.IsNullOrEmpty(gps[0]) && !string.IsNullOrEmpty(gps[1]))
                            {


                                latitud = gps[0];
                                longitud = gps[1];
                            }



                            //conversion del opeardor de cepsa
                            if (Orden.Operador00 == "910106")
                            {
                                Orden.Operador00 = "500";
                                Orden.Operador14 = "500";
                            }                                                                    
                            Orden.InsertDatasOrderView(expirationDays, latitud, longitud);
                            orders.Add(Orden);
                            Orden = new ImportData();                                        
                            contadorlinias = 0;                          
                        }
                                                       
                    }
                    sr.Close();                  
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("fichero no se ha importado correctamente: " + exp.ToString());
            }         
            return orders;
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
                direccionparam = direccionparam + "spain";


                direccionparam = Uri.EscapeDataString(direccionparam);

                // Set the URL (and add any querystring values)   
                strURL = @"https://maps.google.com/maps/api/geocode/xml?key=AIzaSyDHwRTkASbhKZ0uZVioidhvM5Gs3Dw_eAs&address=" + direccionparam + "&sensor=false";

                // Create the web request   
                wbrq = (HttpWebRequest)WebRequest.Create(strURL);
                wbrq.Method = "GET";

                // Read the returned data    
                wbrs = (HttpWebResponse)wbrq.GetResponse();
                sr = new StreamReader(wbrs.GetResponseStream());
                strResult = sr.ReadToEnd();
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

        // Código para configuración personalizada
        //public static IEnumerable<ImportOrder> FromCSV(FileInfo file, ImportConfiguration configuration, int expirationDays)
        //{
        //    StreamReader reader = new StreamReader(file.FullName, Encoding.Default);
        //    var orders = ReadDataCSV(reader, configuration, expirationDays);
        //    reader.Close();
        //    return orders;
        //}

        //private static IEnumerable<ImportOrder> ReadDataCSV(StreamReader aReader, ImportConfiguration configuration, int expirationDays)
        //{
        //    int rows = 0;
        //    while (aReader.Peek() > 0)
        //    {

        //        string lStrLine = aReader.ReadLine();
        //        rows++;

        //        if (configuration.IgnoreFirstRow && rows == 1)
        //            continue;

        //        byte[] bytes = Encoding.UTF8.GetBytes(lStrLine);
        //        lStrLine = Encoding.UTF8.GetString(bytes);
        //        if (lStrLine == null) break;
        //        if (lStrLine.Trim() == "") continue;

        //        string[] values = null;
        //        values = lStrLine.Split(configuration.Delimiter);
        //        if (values == null) continue;


        //        var colConfigs = configuration.Columns;

        //        ImportOrder order = new ImportOrder();

        //        var conf = GetColumnConfigByName(colConfigs, nameof(order.StartDate));
        //        var startdate = ReadValueCSV(values, conf);
        //        if (conf.Format != null)
        //            order.StartDate = DateTime.ParseExact(startdate, conf.Format, CultureInfo.InvariantCulture);
        //        else
        //            order.StartDate = Convert.ToDateTime(startdate);

        //        order.FinalDate = order.StartDate.AddDays(expirationDays);

        //        order.FactoryCode = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.FactoryCode)));
        //        order.Reference = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Reference)));
        //        order.ClientCode = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.ClientCode)));
        //        order.ClientName = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.ClientName)));
        //        order.ClientCif = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.ClientCif)));
        //        order.Contact = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Contact)));
        //        order.City = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.City)));
        //        order.PostCode = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.PostCode)));
        //        order.Address = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Address)));
        //        order.Email = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Email)));
        //        order.Phone = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Phone)));
        //        order.Latitude = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Latitude)));
        //        order.Longitude = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Longitude)));
        //        order.TankVolume = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.TankVolume)));
        //        order.TankNum = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.TankNum)));
        //        order.TankLevel = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.TankLevel)));
        //        order.Amount = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Amount)));
        //        order.ProductCode = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.ProductCode)));
        //        order.MeasureUnit = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.MeasureUnit)));
        //        order.VehicleType = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.VehicleType)));
        //        order.Observations = ReadValueCSV(values, GetColumnConfigByName(colConfigs, nameof(order.Observations)));

        //        yield return order;
        //    }
        //}

        //private static string ReadValueCSV(string[] values, ImportColumnConfiguration columnConfiguration)
        //{

        //    if(columnConfiguration != null)
        //    {
        //        string dvalue = null;
        //        if(columnConfiguration.ColumnNum.Length > 0)
        //        {
        //            foreach (int c in columnConfiguration.ColumnNum)
        //            {
        //                var value = values[c];

        //                if (value == null || value.Trim().Length == 0)
        //                    value = columnConfiguration.DefaultValue;

        //                if (dvalue != null && dvalue.Trim().Length > 0)
        //                    value += " ";

        //                dvalue += value;
        //            }
        //        }
        //        else
        //        {
        //            dvalue = columnConfiguration.DefaultValue;
        //        }


        //        return dvalue;

        //        //if (field is DateTime)
        //        //{
        //        //    if (columnConfiguration.Format != null)
        //        //        field = DateTime.ParseExact(dvalue, columnConfiguration.Format, CultureInfo.InvariantCulture);
        //        //    else
        //        //        field = Convert.ToDateTime(dvalue);
        //        //}
        //        //else if (field is String)
        //        //{
        //        //    field = dvalue;
        //        //}
        //    }

        //    return null;
        //}

        //private static ImportColumnConfiguration GetColumnConfigByName(List<ImportColumnConfiguration> columnConfigurations, string name)
        //{
        //    if (columnConfigurations == null || name == null) return null;
        //    return columnConfigurations.Single(conf => conf != null && conf.ColumnName == name);
        //}
    }
}
