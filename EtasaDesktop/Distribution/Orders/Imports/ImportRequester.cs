using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    class ImportRequester
    {

        public static int? GetClientId(int operatorId, string clientCode)
        {
            ImportDataSetTableAdapters.QueriesTableAdapter qadapt = new ImportDataSetTableAdapters.QueriesTableAdapter();
            return qadapt.GetClientId(operatorId, clientCode) as int?;
        }
        public static int? GetFactoryId(int operatorId, string factoryCode)
        {
            ImportDataSetTableAdapters.QueriesTableAdapter qadapt = new ImportDataSetTableAdapters.QueriesTableAdapter();
            return qadapt.GetFactoryId(operatorId, factoryCode) as int?;
        }
        public static int? GetProductId(int operatorId, string productCode)
        {
            ImportDataSetTableAdapters.QueriesTableAdapter qadapt = new ImportDataSetTableAdapters.QueriesTableAdapter();
            return qadapt.GetProductId(operatorId, productCode) as int?;
        }
        public static int? GetVehicleSizeId(string vehicleSizeCode)
        {
            vehicleSizeCode = Regex.Match(vehicleSizeCode, @"\d+").Value;
            ImportDataSetTableAdapters.QueriesTableAdapter qadapt = new ImportDataSetTableAdapters.QueriesTableAdapter();
            return qadapt.GetVehicleSizeId(vehicleSizeCode) as int?;
        }


        public static int? CreateNewClient(ImportData data)
        {
            try
            {
                ImportDataSet ds = new ImportDataSet();
                ImportDataSetTableAdapters.ClientsTableAdapter tableAdapter = new ImportDataSetTableAdapters.ClientsTableAdapter();
                string provinceId = "00";   // HACK Provincia hardcodeada
                string countryId = "es";    // HACK Pais hardcodeado

                var row = ds.Clients.NewClientsRow();
                row.Code = "";
                row.Name = data.ClientName;
                row.Cif = data.ClientCif;
                row.Address = data.Address; 
                row.City = data.City;
                row.PostCode = data.PostCode;
                row.Province = provinceId;
                row.Country = countryId;
                row.Contact = data.Contact;
                row.Phone = data.Phone;
                row.Phone2 = data.Phone2;
                row.PhoneMobile = "";
                row.Fax = "";
                row.Email = data.Email;
                

                string[] gps = Geocode(data.Address, data.PostCode, data.City);
                row.Latitude = float.Parse(gps[0]);
                row.Longitude = float.Parse(gps[1]);


                ds.Clients.AddClientsRow(row);
                tableAdapter.Update(ds.Clients);


                int clientId = ds.Clients[0].Id;
                ds.Clients[0].Code = clientId.ToString().PadLeft(8, '0');
                tableAdapter.Update(ds.Clients);

                CreateClientRelationship(data, clientId);

                return clientId;
            }
            catch (Exception)
            {
                return null;
            }

            /* 
             * TODO Elementos que no se insertan
             * Province
             * Country
             * PhoneMobile
             * Fax
             */
        }
        private static bool CreateClientRelationship(ImportData data , int clientId)
        {
            ImportDataSet ds = new ImportDataSet();
            ImportDataSetTableAdapters.Operators_ClientsTableAdapter adapt = new ImportDataSetTableAdapters.Operators_ClientsTableAdapter();
            var row = ds.Operators_Clients.NewOperators_ClientsRow();

            row.OperatorId = data.OperatorId;
            row.ClientId = clientId;
            row.Code = data.ClientCode;

            ds.Operators_Clients.AddOperators_ClientsRow(row);
            return adapt.Update(ds.Operators_Clients) > 0;
        }

        private static string[] Geocode(string direction, string postalCode, string city)
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
    }
}
