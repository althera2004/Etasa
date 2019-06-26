using EtasaDesktop.Common.Tools;
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
using EtasaDesktop.Files.Json;
using System.IO;
using System.Net;
using System.Data;

namespace EtasaDesktop.Files.Json
{
    /// <summary>
    /// Lógica de interacción para UserFrame.xaml
    /// </summary>
    public partial class JsonFrame : FrameControl
    {
        private JsonFrameViewModel _viewModel;
        public JsonFrame()
        {
            InitializeComponent();
            _viewModel = (JsonFrameViewModel)DataContext;
        }

        private void ConnectJsonpButton_Click(object sender, RoutedEventArgs e)
        {
            List<Planning> OrderList = new List<Planning>();
            Planning Pedido = new Planning();
            //creamos un listado de pedidos
            //Pedido.CreateOrderList(OrderList, JasonQue.Text.Trim());

            for (int i = 0; i < OrderList.Count; i++)
            {
                //ToJSONRepresentationOrderAndSend((Planning)OrderList[i], JasonQue.Text.Trim());
            }
            //Conversion de objetos to json
        }
        public static void ToJSONRepresentationOrderAndSend(Planning Order, string Quee)
        {
            try
            {
                string newJson = "";
                string URLFinal = "";
                string URLPrueva = "";
                DataTable Order_Operator_Sent;
                bool InsertPlanned = true;

                //comprobación si la planificación se ha enviado o no con anterioridad
                Files.Json.Planning_Vitogas_InsertDataSet ds = new Files.Json.Planning_Vitogas_InsertDataSet();
                Files.Json.Planning_Vitogas_InsertDataSetTableAdapters.Order_Operator_SummariesTableAdapter Order_Operator_Summaries = new Files.Json.Planning_Vitogas_InsertDataSetTableAdapters.Order_Operator_SummariesTableAdapter();
                Order_Operator_Sent = Order_Operator_Summaries.GetDataSendByIdOrder1(Convert.ToInt64(Order.id));

                //la planificación existe no se vuelve a enviar 
                if (Order_Operator_Sent.Rows.Count > 0)
                {
                    InsertPlanned = false;
                }

                //si el data send es nulo representa que la planificación no se ha enviado (realizamos la petición http para enviarla)
                if (InsertPlanned)
                {
                    string JasonString = Order.CreateJasonPlanning();
                    MessageBox.Show("Formato Jason:     " + JasonString);

                    //construimos la url para realizar la peticion http 
                    URLFinal = "http://localhost:21935/tdi/AMMForm?info_Target=" + Quee;

                    //realizamos la peticion http a tmobility (api rest)
                    var request = (HttpWebRequest)WebRequest.Create(URLFinal);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.ContentLength = JasonString.Length;
                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);

                    requestWriter.Close();

                    //obtenemos la respuesta Tmobility
                    System.Net.WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    string response = responseReader.ReadToEnd();

                    //tratamos la respuesta obtenida de Tmobility
                    response = response.Replace("\\", "");
                    response = response.Substring(1, response.Length - 2);

                    string comprobaciódatoInf = ",\"INF\":\"";
                    string comprobaciónError = ",\"RES\":\"ERR\",";

                    //si la respuesta se detecta que continer el campo INF o ERRO informado
                    //damos por hecho que habido un error
                    bool comprobaciódatoCampoinfo = response.Contains(comprobaciódatoInf);
                    bool comprobacióndatoCampoError = response.Contains(comprobaciónError);

                    if (comprobaciódatoCampoinfo || comprobacióndatoCampoError)
                    {
                        int indice_empieza = response.IndexOf(comprobaciódatoInf);

                        //contamos a partir del campo INF de la respuesta Jason (sumando las posiciones que ocupa INFO:(separaciones)
                        int principio_de_cadena = indice_empieza + 8;

                        //del total le restamos los 5 corchetes del final y buscamos el numero de posiciones que necesitamos recorrer
                        int final_cadena = response.Length - 5 - principio_de_cadena;

                        //obtenemos el mensaje de error
                        string Mensaje_de_error = response.Substring(principio_de_cadena, final_cadena);

                        MessageBox.Show("El pedido no se ha enviado correctamente error :" + Mensaje_de_error);
                    }
                    else
                    {
                        Files.Json.Planning_Vitogas_InsertDataSetTableAdapters.Order_Operator_SentTableAdapter tableAdapterOrderBitogas2 = new Files.Json.Planning_Vitogas_InsertDataSetTableAdapters.Order_Operator_SentTableAdapter();
                        var rowOrder_Operator_Sent = ds.Order_Operator_Sent.NewOrder_Operator_SentRow();
                        rowOrder_Operator_Sent.Id_Order = Order.id;
                        rowOrder_Operator_Sent.StartDate = Order.startDate;
                        rowOrder_Operator_Sent.SendData = DateTime.Now;
                        rowOrder_Operator_Sent.SendQueue = Quee;

                        //inserción tabla products
                        //agregamos la nueva fila a tabla Order_Operator_Sent
                        ds.Order_Operator_Sent.AddOrder_Operator_SentRow(rowOrder_Operator_Sent);

                        tableAdapterOrderBitogas2.Update(ds.Order_Operator_Sent);
                        MessageBox.Show("pedido insertado correctamente");
                    }
                }
                else
                {
                    MessageBox.Show("La planificiación ya ha sido enviada: ");
                }
                //responseReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error es:" + e.ToString());
            }
        }
    }
}


