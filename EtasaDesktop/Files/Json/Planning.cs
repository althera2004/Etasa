using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Bson;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EtasaDesktop.Files.Json
{
    public class Planning
    {
        public string reference;
        public int id;
        public float longitude;
        public DateTime startDate;
        public DateTime FinalDate;
        public string Quee;

        //datos jason
        string GWheader;
        string IDDEV;
        string STATUS;
        string DESTTYPE;
        string DEVDATA;
        string CMD;
        string NAM;
        string CID;

        //DATS
        string DATSIDP;
        string DATSTXT;
        string DATSINF;
        string DATSCST;

        //MIS
        string MISIDM;
        string MISTXT;
        string MISINF;
        string MISCST;
        string MISPOS;
        string MISLAT;
        string MISLNG;

        //ITM
        string ITMIDI;
        string ITMTXT;
        string ITMINF;
        string ITMCST;

        public Planning()
        {
            this.reference = "";
            this.id = 22;
            this.longitude = 224;
            this.startDate = DateTime.Now;
            this.FinalDate = DateTime.Now;
            this.Quee = "";

            //CABECERA 
            this.GWheader = "23";
            this.IDDEV = "24";
            this.STATUS = "25";
            this.DESTTYPE = "26";
            this.DEVDATA = "27";
            this.CMD = "28";
            this.NAM = "29";
            this.CID = "30";

            //DAT
            this.DATSIDP = "31";
            this.DATSTXT = "32";
            this.DATSINF = "33";
            this.DATSCST = "34";

            //MIS
            this.MISIDM = "35";
            this.MISTXT = "36";
            this.MISINF = "37";
            this.MISCST = "38";
            this.MISPOS = "39";
            this.MISLAT = "40";
            this.MISLNG = "41";

            //ITM
            this.ITMIDI = "42";
            this.ITMTXT = "43";
            this.ITMINF = "44";
            this.ITMCST = "45";

        }

        public string CreateJasonPlanning()
        {

            int version = 1;

            string datasHead = "5";
            //Identificador del dispositivo móvil(IMEI, thx CPU identification code, etc...)(obligatorio)
            string DatasIddev = "CA1BC90E3F";
            //Estado del mensaje.Este campo indica si el mensaje es correcto(valor "OK" ), o si el mensaje no puede ser enviado al destino por el gateway(valor "KO")(obligatorio)
            string DatasStatus = "OK";
            //Tipo de receptor del mensaje. Este campo indica a dónde se debería enviar el mensaje.Dos posibles valores:{ "M2I": Machine to Interface, "M2M: Machine to Machine (obligatorio)
            string DataDestype = "M21";
            //Información contenida en el mensaje
            string DataDevData = "23";
            //Comando contenido en el mensaje. CMD indica que se trata de un comando.
            string DataCMD = "4";
            //Identificador de comando.Este identificador se utilizará en el mensaje de respuesta, para poder así asociar comandos con respuestas.Es de tipo alfanumérico
            string DataCID = "12C48FB";
            //Nombre del comando, en este caso, SETPLAN
            string DataNam = "SETPLAN";

            //DAT 
            //Datos del comando, concretamente:
            string DataDat = "89";
            //Estado actual del planning. Por defecto, 0.
            string DataDatCST = "0";
            //Identificador del planning
            string DataDatIDP = "PLANNING1";
            //Descripción del planning
            string DataDatINF = "INFORMACIÓN DEL PLANNING";
            //Descripción breve del planning (para identificarlo en una lista de plannings)
            string DataDatTXT = "DATOS DEL PLANNING1";


            //MISS(ARRAY)
            //Estado actual de la misión. Valor por defecto dependiendo del proyecto.
            string DataMisCST = "4";
            //Estado actual de la misión. Valor por defecto dependiendo del proyecto.
            string DataMisTYP = "LOAD";

            //Identificador de la misión
            string DataMisIDM = "MISSION1";
            //Latitud
            string DataMisLatitude = "39.864869";
            //Longitud
            string DataMisLongitude = "4.030437";
            //Orden en que aparece la misión en la lista de misiones (opcional)
            string DataMisPOS = "0";
            //Orden en que aparece la misión en la lista de misiones (opcional)
            string DataMisinformation = "INFORMACIÓN DE LA MISIÓN 1";
            //Descripción breve de la misión(para identificarlo en una lista dees)
            string DataMisiTxt = "CARGA INBI";

            //ITM(ARRAY) 
            string DataITM = "23";
            //Identificador del producto
            string DataITMIDI = "PRODUCTO1";
            //Descripción del producto (opcional)
            string DataITMINF = "INFORMACIÓN DEL PRODUCTO 1";
            //Descripción breve del producto(para identificarlo en una lista de productos)
            string DataITMTXT = "DATOS1";

            if (version == 0)
            {
                datasHead = "\"" + datasHead + "\"";
                DataDevData = "\"" + DataDevData + "\"";
                DataCMD = "\"" + DataCMD + "\"";
                DataDat = "\"" + DataDat + "\"";
                DataITM = "\"" + DataITM + "\"";
            }
            else
            {
                datasHead = "";
                DataDevData = "";
                DataCMD = "";
                DataDat = "";
                DataITM = "";

            }


            //formato jason del pedido
            string JasonString = "{\"GWHEADER\":" + datasHead + "{\"IDDEV\":" + "\"" + DatasIddev + "\",\"STATUS\":" + "\"" + DatasStatus + "\"" + ",\"DESTTYPE\":" +
                                 "\"" + DataDestype + "\"},\"DEVDATA\":" + DataDevData + "{\"CMD\":" + DataCMD + "{\"CID\":" + "\"" + DataCID + "\"" + ",\"NAM\":" + "\"" + DataNam + "\"" +
                                 ",\"DAT\":" + DataDat + "{\"CST\":" + DataDatCST + ",\"MIS\":[{\"CST\":" + DataMisCST + ",\"TYP\":" + "\"" + DataMisTYP + "\",\"IDM\":" + "\"" + DataMisIDM + "\"" +
                                 ",\"LAT\":" + "\"" + DataMisLatitude + "\",\"LNG\":" + "\"" + DataMisLongitude + "\",\"POS\":" + DataMisPOS + ",\"INF\":" + DataMisinformation + "\",\"TXT\":" + "\"" + DataMisiTxt + "\"" +
                                 ",\"ITM\":" + DataITM + "[{\"IDI\":\"" + DataITMIDI + "\",\"INF\":" + "\"" + DataITMINF + "\"," + "\"TXT\":" + "\"" + DataITMTXT + "\"}]}],\"IDP\":" + "\"" + DataDatIDP + "\"" +
                                 ",\"INF\":" + "\"" + DataDatINF + "\",\"TXT\":" + "\"" + DataDatTXT + "\"}}}}}";

            return JasonString;
        }


        public string SimulatedJsonCorrectOrError(bool error)
        {

            int version = 1;
            string JasonString = "";
            string DatasdEV = "1234ABCD";
            string DataTYP = "HIM";
            string DataNam = "SETPLAN";
            string DataCID = "12C48FB";
            string DataIDP = "PLANNING1INF";
            string Datatime = DateTime.Now.ToString();
            string DatalAT = "0038:00:22.4940N";
            string DataRes = "ERR";
            string DatakM = "0.0";
            string Datadate = "01/10/2014";
            string DataLNG = "001:08:50.4480W";
            string DataInfo = "I d'ont know INFO";

            //Mensaje de repuesta sin error 
            if (!error)
            {
                DataRes = "OK";

                JasonString = "{\"GWHEADER\":{\"DEV\":" + "\"" + DatasdEV + "\",\"TYP\":" + "\"" + DataTYP + "\",DEVDATA:{\"ANS\":{\"NAM\":" + "\"" + DataNam + "\"" +
                                     ",\"CID\":" + "\"" + DataCID + "\",\"DAT\":{\"IDP\":" + "\"" + DataIDP + "\",\"TIME\":" + "\"" + Datatime + "\",\"LAT\":" + "\"" + DatalAT + "\"" +
                                     ",\"RES\":" + "\"" + DataRes + "\",\"KMS\":" + "\"" + DatakM + "\",\"DATE\":\"" + Datadate + "\",\"LNG\":" + "\"" + DataLNG + "\"}}}}";
            }
            //Mensaje de repuesta con error
            else
            {
                DataRes = "ERR";
                JasonString = "{\"GWHEADER\":{\"DEV\":" + "\"" + DatasdEV + "\",\"TYP\":" + "\"" + DataTYP + "\"},DEVDATA:{\"ANS\":{\"NAM\":" + "\"" + DataNam + "\"" +
                                     ",\"CID\":" + "\"" + DataCID + "\",\"DAT\":{\"IDP\":" + "\"" + DataIDP + "\",\"TIME\":" + "\"" + Datatime + "\",\"LAT\":" + "\"" + DatalAT + "\"" +
                                     ",\"RES\":" + "\"" + DataRes + "\",\"KMS\":" + "\"" + DatakM + "\",\"DATE\":\"" + Datadate + "\",\"LNG\":" + "\"" + DataLNG + "\",\"INF\":" + "\"" + DataInfo + "\"}}}}";
            }
            return JasonString;
        }





        public List<Planning> CreateOrderList(List<Planning> OrderList, string Que)
        {
            Planning Pedido = new Planning();
            Pedido.reference = "3456HG";
            Pedido.id = 1;
            Pedido.longitude = 24;
            Pedido.startDate = DateTime.Now;
            Pedido.FinalDate = DateTime.Now;
            Pedido.Quee = Que;
            OrderList.Add(Pedido);


            Planning Pedido1 = new Planning();
            Pedido1.id = 2;
            Pedido1.reference = "23233HG2";
            Pedido1.longitude = 35;
            Pedido1.startDate = DateTime.Now;
            Pedido1.FinalDate = DateTime.Now;
            Pedido1.Quee = Que;
            OrderList.Add(Pedido1);

            Planning Pedido2 = new Planning();
            Pedido2.id = 5;
            Pedido2.reference = "2323GTYH";
            Pedido2.longitude = 23;
            Pedido2.startDate = DateTime.Now;
            Pedido2.FinalDate = DateTime.Now;
            Pedido2.Quee = Que;
            OrderList.Add(Pedido2);

            Planning Pedido3 = new Planning();
            Pedido3.id = 7;
            Pedido3.reference = "245HT";
            Pedido3.longitude = 323;
            Pedido3.startDate = DateTime.Now;
            Pedido3.FinalDate = DateTime.Now;
            Pedido3.Quee = Que;
            OrderList.Add(Pedido3);

            return OrderList;
        }
    }
}

