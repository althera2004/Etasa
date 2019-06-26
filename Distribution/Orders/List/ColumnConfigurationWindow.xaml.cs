using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EtasaDesktop.Distribution.Orders
{
    /// <summary>
    /// Lógica de interacción para ColumnConfigurationWindow.xaml
    /// </summary>
    public partial class ColumnConfigurationWindow : Window
    {
        public ColumnConfigurationWindow()
        {
            InitializeComponent();

            FillCheckBoxes();
        }

        private void CloseColumnConfigurationWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pedido_Estado_entregaCB_Change(object sender, RoutedEventArgs e)
        {
            CheckBoxList.Pedido_Estado_entrega = (Pedido_Estado_entregaCB.IsChecked == true);
        }

        private void FillCheckBoxes(int iIdUser = 0)
        {
            iIdUser = 1;
            int iPosicion = 0;
            bool bValor = true;

            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = @"SELECT * 
                            FROM System_Column_Configs
                            WHERE IdUser = " + iIdUser;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ColumnCheckBoxes");
                sda.Fill(dt);
                //ListViewColumnCheckBoxes.ItemsSource = dt.DefaultView;

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        bValor = row[column].ToString().Equals("True");

                        FunctionChangeValueCheckBox(bValor,iPosicion);
                        iPosicion++;
                    }
                }
            }
        }

        public void FunctionChangeValueCheckBox(bool bValorLogico, int iControl)
        {
            switch (iControl)
            {

                case 2:
                    Compra_Compra_VaciosCB.IsChecked = bValorLogico;
                    CheckBoxList.Compra_Compra_Vacios = bValorLogico;
                    break;
                case 3:
                    Compra_PeajeCB.IsChecked = bValorLogico;
                    CheckBoxList.Compra_Peaje = bValorLogico;
                    break;
                case 4:
                    Compra_PortesCB.IsChecked = bValorLogico;
                    CheckBoxList.Compra_Portes = bValorLogico;
                    break;
                case 5:
                    Compra_Precio_TarifaCB.IsChecked = bValorLogico;
                    CheckBoxList.Compra_Precio_Tarifa = bValorLogico;
                    break;
                case 6:
                    Compra_SuplidosCB.IsChecked = bValorLogico;
                    CheckBoxList.Compra_Suplidos = bValorLogico;
                    break;
                case 7:
                    Compra_Tasa_recargoCB.IsChecked = bValorLogico;
                    CheckBoxList.Compra_Tasa_recargo = bValorLogico;
                    break;
                case 8:
                    Compra_TotalCB.IsChecked = bValorLogico;
                    CheckBoxList.Compra_Total = bValorLogico;
                    break;
                case 9:
                    Entrega_NingunaCB.IsChecked = bValorLogico;
                    CheckBoxList.Entrega_Ninguna = bValorLogico;
                    break;
                case 10:
                    Entrega_Hora_Salida_ClienteCB.IsChecked = bValorLogico;
                    CheckBoxList.Entrega_Hora_Salida_Cliente = bValorLogico;
                    break;
                case 11:
                    Entrega_KilogramosCB.IsChecked = bValorLogico;
                    CheckBoxList.Entrega_Kilogramos = bValorLogico;
                    break;
                case 12:
                    Entrega_KMCB.IsChecked = bValorLogico;
                    CheckBoxList.Entrega_KM = bValorLogico;
                    break;
                case 13:
                    Entrega_LitrosCB.IsChecked = bValorLogico;
                    CheckBoxList.Entrega_Litros = bValorLogico;
                    break;
                case 14:
                    Entrega_M3CB.IsChecked = bValorLogico;
                    CheckBoxList.Entrega_M3 = bValorLogico;
                    break;
                case 15:
                    Entrega_UnidadesCB.IsChecked = bValorLogico;
                    CheckBoxList.Entrega_Unidades = bValorLogico;
                    break;
                case 16:
                    Fechas_EntregaCB.IsChecked = bValorLogico;
                    CheckBoxList.Fechas_Entrega = bValorLogico;
                    break;
                case 17:
                    Fechas_PrevistaCB.IsChecked = bValorLogico;
                    CheckBoxList.Fechas_Prevista = bValorLogico;
                    break;
                case 18:
                    Fechas_RealCB.IsChecked = bValorLogico;
                    CheckBoxList.Fechas_Real = bValorLogico;
                    break;
                case 19:
                    ObservacionesCB.IsChecked = bValorLogico;
                    CheckBoxList.Observaciones = bValorLogico;
                    break;
                case 20:
                    PedidoCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido = bValorLogico;
                    break;
                case 21:
                    Pedido_AGENTECB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_AGENTE = bValorLogico;
                    break;
                case 22:
                    Pedido_AlbaranCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Albaran = bValorLogico;
                    break;
                case 23:
                    Pedido_AlquiladoCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Alquilado = bValorLogico;
                    break;
                case 24:
                    Pedido_COCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_CO = bValorLogico;
                    break;
                case 25:
                    Pedido_ClienteCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Cliente = bValorLogico;
                    break;
                case 26:
                    Pedido_CMRCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_CMR = bValorLogico;
                    break;
                case 27:
                    Pedido_Coste_almacen_entregaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Coste_almacen_entrega = bValorLogico;
                    break;
                case 28:
                    Pedido_DepotCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Depot = bValorLogico;
                    break;
                case 29:
                    Pedido_Estado_entregaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Estado_entrega = bValorLogico;
                    break;
                case 30:
                    Pedido_FactoriaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Factoria = bValorLogico;
                    break;
                case 31:
                    Pedido_Hora_carga_salidaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Hora_carga_salida = bValorLogico;
                    break;
                case 32:
                    Pedido_Hora_finalCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Hora_final = bValorLogico;
                    break;
                case 33:
                    Pedido_Hora_inicioCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Hora_inicio = bValorLogico;
                    break;
                case 34:
                    Pedido_ImpresoCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Impreso = bValorLogico;
                    break;
                case 35:
                    Pedido_Numero_solicitudCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Numero_solicitud = bValorLogico;
                    break;
                case 36:
                    Pedido_Observaciones_bunkerCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Observaciones_bunker = bValorLogico;
                    break;
                case 37:
                    Pedido_OperadorCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Operador = bValorLogico;
                    break;
                case 38:
                    Pedido_Pag_EscaneadoCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Pag_Escaneado = bValorLogico;
                    break;
                case 39:
                    Pedido_PoblacionCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Poblacion = bValorLogico;
                    break;
                case 40:
                    Pedido_Poblacion2CB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Poblacion2 = bValorLogico;
                    break;
                case 41:
                    Pedido_ProductoCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Producto = bValorLogico;
                    break;
                case 42:
                    Pedido_Puerto_entregaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Puerto_entrega = bValorLogico;
                    break;
                case 43:
                    Pedido_ReferenciaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Referencia = bValorLogico;
                    break;
                case 44:
                    Pedido_Solicita_HastaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Solicita_Hasta = bValorLogico;
                    break;
                case 45:
                    Pedido_Tipo_camionCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Tipo_camion = bValorLogico;
                    break;
                case 46:
                    Pedido_URGCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_URG = bValorLogico;
                    break;
                case 47:
                    Pedido_Venta_rutaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Venta_ruta = bValorLogico;
                    break;
                case 48:
                    Pedido_Venta_ruta_solicitadaCB.IsChecked = bValorLogico;
                    CheckBoxList.Pedido_Venta_ruta_solicitada = bValorLogico;
                    break;
                case 49:
                    PrefacturaCB.IsChecked = bValorLogico;
                    CheckBoxList.Prefactura = bValorLogico;
                    break;
                case 50:
                    Programacion_ConductorCB.IsChecked = bValorLogico;
                    CheckBoxList.Programacion_Conductor = bValorLogico;
                    break;
                case 51:
                    Programacion_DescargaCB.IsChecked = bValorLogico;
                    CheckBoxList.Programacion_Descarga = bValorLogico;
                    break;
                case 52:
                    Programacion_Kms_FacturablesCB.IsChecked = bValorLogico;
                    CheckBoxList.Programacion_Kms_Facturables = bValorLogico;
                    break;
                case 53:
                    Programacion_Kms_VaciosCB.IsChecked = bValorLogico;
                    CheckBoxList.Programacion_Kms_Vacios = bValorLogico;
                    break;
                case 54:
                    Programacion_RemolqueCB.IsChecked = bValorLogico;
                    CheckBoxList.Programacion_Remolque = bValorLogico;
                    break;
                case 55:
                    Programacion_TractoraCB.IsChecked = bValorLogico;
                    CheckBoxList.Programacion_Tractora = bValorLogico;
                    break;
                case 56:
                    Programacion_Venta_RutaCB.IsChecked = bValorLogico;
                    CheckBoxList.Programacion_Venta_Ruta = bValorLogico;
                    break;
                case 57:
                    Programacion_ViajeCB.IsChecked = bValorLogico;
                    CheckBoxList.Programacion_Viaje = bValorLogico;
                    break;
                case 58:
                    Solicitado_NingunaCB.IsChecked = bValorLogico;
                    CheckBoxList.Solicitado_Ninguna = bValorLogico;
                    break;
                case 59:
                    Solicitado_KilogramosCB.IsChecked = bValorLogico;
                    CheckBoxList.Solicitado_Kilogramos = bValorLogico;
                    break;
                case 60:
                    Solicitado_KMCB.IsChecked = bValorLogico;
                    CheckBoxList.Solicitado_KM = bValorLogico;
                    break;
                case 61:
                    Solicitado_LitrosCB.IsChecked = bValorLogico;
                    CheckBoxList.Solicitado_Litros = bValorLogico;
                    break;
                case 62:
                    Solicitado_M3CB.IsChecked = bValorLogico;
                    CheckBoxList.Solicitado_M3 = bValorLogico;
                    break;
                case 63:
                    Solicitado_UnidadesCB.IsChecked = bValorLogico;
                    CheckBoxList.Solicitado_Unidades = bValorLogico;
                    break;
                case 64:
                    Venta_ComisionCB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Comision = bValorLogico;
                    break;
                case 65:
                    Venta_Comision2CB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Comision2 = bValorLogico;
                    break;
                case 66:
                    Venta_PeajeCB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Peaje = bValorLogico;
                    break;
                case 67:
                    Venta_Peaje2CB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Peaje2 = bValorLogico;
                    break;
                case 68:
                    Venta_PortesCB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Portes = bValorLogico;
                    break;
                case 69:
                    Venta_SuplidosCB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Suplidos = bValorLogico;
                    break;
                case 70:
                    Venta_Suplidos2CB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Suplidos2 = bValorLogico;
                    break;
                case 71:
                    Venta_Tarifa1CB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Tarifa1 = bValorLogico;
                    break;
                case 72:
                    Venta_Tarifa2CB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Tarifa2 = bValorLogico;
                    break;
                case 73:
                    Venta_Tasa_recargoCB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Tasa_recargo = bValorLogico;
                    break;
                case 74:
                    Venta_Tasa_recargo2CB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Tasa_recargo2 = bValorLogico;
                    break;
                case 75:
                    Venta_Venta_TotalCB.IsChecked = bValorLogico;
                    CheckBoxList.Venta_Venta_Total = bValorLogico;
                    break;
                case 76:
                    PedidoIdCb.IsChecked = bValorLogico;
                    CheckBoxList.PedidoId = bValorLogico;
                    break;
                default:
                    break;

            }

        }

        private void AcceptColumnConfigurationWindow_Click(object sender, RoutedEventArgs e)
        {
            int iUserId = 1;

            CheckBoxList.Compra_Compra_Vacios = (Compra_Compra_VaciosCB.IsChecked == true);
            CheckBoxList.Compra_Peaje = (Compra_PeajeCB.IsChecked == true);
            CheckBoxList.Compra_Portes = (Compra_PortesCB.IsChecked == true);
            CheckBoxList.Compra_Precio_Tarifa = (Compra_Precio_TarifaCB.IsChecked == true);
            CheckBoxList.Compra_Suplidos = (Compra_SuplidosCB.IsChecked == true);
            CheckBoxList.Compra_Tasa_recargo = (Compra_Tasa_recargoCB.IsChecked == true);
            CheckBoxList.Compra_Total = (Compra_TotalCB.IsChecked == true);
            CheckBoxList.Entrega_Ninguna = (Entrega_NingunaCB.IsChecked == true);
            CheckBoxList.Entrega_Hora_Salida_Cliente = (Entrega_Hora_Salida_ClienteCB.IsChecked == true);
            CheckBoxList.Entrega_Kilogramos = (Entrega_KilogramosCB.IsChecked == true);
            CheckBoxList.Entrega_KM = (Entrega_KMCB.IsChecked == true);
            CheckBoxList.Entrega_Litros = (Entrega_LitrosCB.IsChecked == true);
            CheckBoxList.Entrega_M3 = (Entrega_M3CB.IsChecked == true);
            CheckBoxList.Entrega_Unidades = (Entrega_UnidadesCB.IsChecked == true);
            CheckBoxList.Fechas_Entrega = (Fechas_EntregaCB.IsChecked == true);
            CheckBoxList.Fechas_Prevista = (Fechas_PrevistaCB.IsChecked == true);
            CheckBoxList.Fechas_Real = (Fechas_RealCB.IsChecked == true);
            CheckBoxList.Observaciones = (ObservacionesCB.IsChecked == true);
            CheckBoxList.Pedido = (PedidoCB.IsChecked == true);
            CheckBoxList.PedidoId = (PedidoIdCb.IsChecked == true);
            CheckBoxList.Pedido_AGENTE = (Pedido_AGENTECB.IsChecked == true);
            CheckBoxList.Pedido_Albaran = (Pedido_AlbaranCB.IsChecked == true);
            CheckBoxList.Pedido_Alquilado = (Pedido_AlquiladoCB.IsChecked == true);
            CheckBoxList.Pedido_CO = (Pedido_COCB.IsChecked == true);
            CheckBoxList.Pedido_Cliente = (Pedido_ClienteCB.IsChecked == true);
            CheckBoxList.Pedido_CMR = (Pedido_CMRCB.IsChecked == true);
            CheckBoxList.Pedido_Coste_almacen_entrega = (Pedido_Coste_almacen_entregaCB.IsChecked == true);
            CheckBoxList.Pedido_Depot = (Pedido_DepotCB.IsChecked == true);
            CheckBoxList.Pedido_Estado_entrega = (Pedido_Estado_entregaCB.IsChecked == true);
            CheckBoxList.Pedido_Factoria = (Pedido_FactoriaCB.IsChecked == true);
            CheckBoxList.Pedido_Hora_carga_salida = (Pedido_Hora_carga_salidaCB.IsChecked == true);
            CheckBoxList.Pedido_Hora_final = (Pedido_Hora_finalCB.IsChecked == true);
            CheckBoxList.Pedido_Hora_inicio = (Pedido_Hora_inicioCB.IsChecked == true);
            CheckBoxList.Pedido_Impreso = (Pedido_ImpresoCB.IsChecked == true);
            CheckBoxList.Pedido_Numero_solicitud = (Pedido_Numero_solicitudCB.IsChecked == true);
            CheckBoxList.Pedido_Observaciones_bunker = (Pedido_Observaciones_bunkerCB.IsChecked == true);
            CheckBoxList.Pedido_Operador = (Pedido_OperadorCB.IsChecked == true);
            CheckBoxList.Pedido_Pag_Escaneado = (Pedido_Pag_EscaneadoCB.IsChecked == true);
            CheckBoxList.Pedido_Poblacion = (Pedido_PoblacionCB.IsChecked == true);
            CheckBoxList.Pedido_Poblacion2 = (Pedido_Poblacion2CB.IsChecked == true);
            CheckBoxList.Pedido_Producto = (Pedido_ProductoCB.IsChecked == true);
            CheckBoxList.Pedido_Puerto_entrega = (Pedido_Puerto_entregaCB.IsChecked == true);
            CheckBoxList.Pedido_Referencia = (Pedido_ReferenciaCB.IsChecked == true);
            CheckBoxList.Pedido_Solicita_Hasta = (Pedido_Solicita_HastaCB.IsChecked == true);
            CheckBoxList.Pedido_Tipo_camion = (Pedido_Tipo_camionCB.IsChecked == true);
            CheckBoxList.Pedido_URG = (Pedido_URGCB.IsChecked == true);
            CheckBoxList.Pedido_Venta_ruta = (Pedido_Venta_rutaCB.IsChecked == true);
            CheckBoxList.Pedido_Venta_ruta_solicitada = (Pedido_Venta_ruta_solicitadaCB.IsChecked == true);
            CheckBoxList.Prefactura = (PrefacturaCB.IsChecked == true);
            CheckBoxList.Programacion_Conductor = (Programacion_ConductorCB.IsChecked == true);
            CheckBoxList.Programacion_Descarga = (Programacion_DescargaCB.IsChecked == true);
            CheckBoxList.Programacion_Kms_Facturables = (Programacion_Kms_FacturablesCB.IsChecked == true);
            CheckBoxList.Programacion_Kms_Vacios = (Programacion_Kms_VaciosCB.IsChecked == true);
            CheckBoxList.Programacion_Remolque = (Programacion_RemolqueCB.IsChecked == true);
            CheckBoxList.Programacion_Tractora = (Programacion_TractoraCB.IsChecked == true);
            CheckBoxList.Programacion_Venta_Ruta = (Programacion_Venta_RutaCB.IsChecked == true);
            CheckBoxList.Programacion_Viaje = (Programacion_ViajeCB.IsChecked == true);
            CheckBoxList.Solicitado_Ninguna = (Solicitado_NingunaCB.IsChecked == true);
            CheckBoxList.Solicitado_Kilogramos = (Solicitado_KilogramosCB.IsChecked == true);
            CheckBoxList.Solicitado_KM = (Solicitado_KMCB.IsChecked == true);
            CheckBoxList.Solicitado_Litros = (Solicitado_LitrosCB.IsChecked == true);
            CheckBoxList.Solicitado_M3 = (Solicitado_M3CB.IsChecked == true);
            CheckBoxList.Solicitado_Unidades = (Solicitado_UnidadesCB.IsChecked == true);
            CheckBoxList.Venta_Comision = (Venta_ComisionCB.IsChecked == true);
            CheckBoxList.Venta_Comision2 = (Venta_Comision2CB.IsChecked == true);
            CheckBoxList.Venta_Peaje = (Venta_PeajeCB.IsChecked == true);
            CheckBoxList.Venta_Peaje2 = (Venta_Peaje2CB.IsChecked == true);
            CheckBoxList.Venta_Portes = (Venta_PortesCB.IsChecked == true);
            CheckBoxList.Venta_Suplidos = (Venta_SuplidosCB.IsChecked == true);
            CheckBoxList.Venta_Suplidos2 = (Venta_Suplidos2CB.IsChecked == true);
            CheckBoxList.Venta_Tarifa1 = (Venta_Tarifa1CB.IsChecked == true);
            CheckBoxList.Venta_Tarifa2 = (Venta_Tarifa2CB.IsChecked == true);
            CheckBoxList.Venta_Tasa_recargo = (Venta_Tasa_recargoCB.IsChecked == true);
            CheckBoxList.Venta_Tasa_recargo2 = (Venta_Tasa_recargo2CB.IsChecked == true);
            CheckBoxList.Venta_Venta_Total = (Venta_Venta_TotalCB.IsChecked == true);

            string ConString = ConfigurationManager.ConnectionStrings["EtasaDesktop.Properties.Settings.EtasaConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand command = con.CreateCommand())
            { // tiene que estar por el mismo orden que ne BD??
                command.CommandText = @"UPDATE System_Column_Configs
                                        SET Compra_Compra_Vacios = @comcomva,
                                        Compra_Peaje = @compe,
                                        Compra_Portes = @compor,
                                        Compra_Precio_Tarifa = @compreta,
                                        Compra_Suplidos = @comsu,
                                        Compra_Tasa_recargo = @comtare,
                                        Compra_Total = @comto,
                                        Entrega_Ninguna = @ennin,
                                        Entrega_Hora_Salida_Cliente = @enhosacli,
                                        Entrega_Kilogramos = @enki,
                                        Entrega_KM = @enkm,
                                        Entrega_Litros = @enli,
                                        Entrega_M3 = @enm3,
                                        Entrega_Unidades = @enu,
                                        Fechas_Entrega = @feen,
                                        Fechas_Prevista = @fepre,
                                        Fechas_Real = @fere,
                                        Observaciones = @obs,
                                        Pedido = @ped,
                                        PedidoId = @pedid,
                                        Pedido_AGENTE = @peage,
                                        Pedido_Albaran = @pealb,
                                        Pedido_Alquilado = @pealq,
                                        Pedido_CO       = @peco,
                                        Pedido_Cliente = @pecli,
                                        Pedido_CMR = @pecmr,
                                        Pedido_Coste_almacen_entrega = @pecoalen,
                                        Pedido_Depot = @pede,
                                        Pedido_Estado_entrega = @peesen,
                                        Pedido_Factoria = @pefac,
                                        Pedido_Hora_carga_salida = @pehocasa,
                                        Pedido_Hora_final = @pehofi,
                                        Pedido_Hora_inicio = @pehoi,
                                        Pedido_Impreso = @peim,
                                        Pedido_Numero_solicitud = @penuso,
                                        Pedido_Observaciones_bunker = @peobbun,
                                        Pedido_Operador = @peop,
                                        Pedido_Pag_Escaneado = @pepaes,
                                        Pedido_Poblacion = @pepo,
                                        Pedido_Poblacion2 = @pepo2,
                                        Pedido_Producto = @pepro,
                                        Pedido_Puerto_entrega = @pepueren,
                                        Pedido_Referencia = @peref,
                                        Pedido_Solicita_Hasta = @pesohas,
                                        Pedido_Tipo_camion = @petica,
                                        Pedido_URG = @peurg,
                                        Pedido_Venta_ruta = @pevenru,
                                        Pedido_Venta_ruta_solicitada = @pevenruso,
                                        Prefactura = @pref,
                                        Programacion_Conductor = @procon,
                                        Programacion_Descarga = @prodes,
                                        Programacion_Kms_Facturables = @prokmsfac,
                                        Programacion_Kms_Vacios = @prokmsva,
                                        Programacion_Remolque = @prore,
                                        Programacion_Tractora = @protrac,
                                        Programacion_Venta_Ruta = @provenru,
                                        Programacion_Viaje = @provia,
                                        Solicitado_Ninguna = @sonin,
                                        Solicitado_Kilogramos = @soki,
                                        Solicitado_KM = @sokm,
                                        Solicitado_Litros = @soli,
                                        Solicitado_M3 = @som3,
                                        Solicitado_Unidades = @souni,
                                        Venta_Comision = @venco,
                                        Venta_Comision2 = @venco2,
                                        Venta_Peaje = @venpea,
                                        Venta_Peaje2 = @venpea2,
                                        Venta_Portes = @venpor,
                                        Venta_Suplidos = @vensu,
                                        Venta_Suplidos2 = @vensu2,
                                        Venta_Tarifa1 = @venta1,
                                        Venta_Tarifa2 = @venta2,
                                        Venta_Tasa_recargo = @ventare,
                                        Venta_Tasa_recargo2 = @ventare2,
                                        Venta_Venta_Total = @venvento
                                        WHERE IdUser = @uid; ";

                command.Parameters.AddWithValue("@comcomva", CheckBoxList.Compra_Compra_Vacios);
                command.Parameters.AddWithValue("@compe", CheckBoxList.Compra_Peaje);
                command.Parameters.AddWithValue("@compor", CheckBoxList.Compra_Portes);
                command.Parameters.AddWithValue("@compreta", CheckBoxList.Compra_Precio_Tarifa);
                command.Parameters.AddWithValue("@comsu", CheckBoxList.Compra_Suplidos);
                command.Parameters.AddWithValue("@comtare", CheckBoxList.Compra_Tasa_recargo);
                command.Parameters.AddWithValue("@comto", CheckBoxList.Compra_Total);
                command.Parameters.AddWithValue("@ennin", CheckBoxList.Entrega_Ninguna);
                command.Parameters.AddWithValue("@enhosacli", CheckBoxList.Entrega_Hora_Salida_Cliente);
                command.Parameters.AddWithValue("@enki", CheckBoxList.Entrega_Kilogramos);
                command.Parameters.AddWithValue("@enkm", CheckBoxList.Entrega_KM);
                command.Parameters.AddWithValue("@enli", CheckBoxList.Entrega_Litros);
                command.Parameters.AddWithValue("@enm3", CheckBoxList.Entrega_M3);
                command.Parameters.AddWithValue("@enu", CheckBoxList.Entrega_Unidades);
                command.Parameters.AddWithValue("@feen", CheckBoxList.Fechas_Entrega);
                command.Parameters.AddWithValue("@fepre", CheckBoxList.Fechas_Prevista);
                command.Parameters.AddWithValue("@fere", CheckBoxList.Fechas_Real);
                command.Parameters.AddWithValue("@obs", CheckBoxList.Observaciones);
                command.Parameters.AddWithValue("@ped", CheckBoxList.Pedido);
                command.Parameters.AddWithValue("@pedid", CheckBoxList.PedidoId);
                command.Parameters.AddWithValue("@peage", CheckBoxList.Pedido_AGENTE);
                command.Parameters.AddWithValue("@pealb", CheckBoxList.Pedido_Albaran);
                command.Parameters.AddWithValue("@pealq", CheckBoxList.Pedido_Alquilado);
                command.Parameters.AddWithValue("@peco", CheckBoxList.Pedido_CO);
                command.Parameters.AddWithValue("@pecli", CheckBoxList.Pedido_Cliente);
                command.Parameters.AddWithValue("@pecmr", CheckBoxList.Pedido_CMR);
                command.Parameters.AddWithValue("@pecoalen", CheckBoxList.Pedido_Coste_almacen_entrega);
                command.Parameters.AddWithValue("@pede", CheckBoxList.Pedido_Depot);
                command.Parameters.AddWithValue("@peesen", CheckBoxList.Pedido_Estado_entrega);
                command.Parameters.AddWithValue("@pefac", CheckBoxList.Pedido_Factoria);
                command.Parameters.AddWithValue("@pehocasa", CheckBoxList.Pedido_Hora_carga_salida);
                command.Parameters.AddWithValue("@pehofi", CheckBoxList.Pedido_Hora_final);
                command.Parameters.AddWithValue("@pehoi", CheckBoxList.Pedido_Hora_inicio);
                command.Parameters.AddWithValue("@peim", CheckBoxList.Pedido_Impreso);
                command.Parameters.AddWithValue("@penuso", CheckBoxList.Pedido_Numero_solicitud);
                command.Parameters.AddWithValue("@peobbun", CheckBoxList.Pedido_Observaciones_bunker);
                command.Parameters.AddWithValue("@peop", CheckBoxList.Pedido_Operador);
                command.Parameters.AddWithValue("@pepaes", CheckBoxList.Pedido_Pag_Escaneado);
                command.Parameters.AddWithValue("@pepo", CheckBoxList.Pedido_Poblacion);
                command.Parameters.AddWithValue("@pepo2", CheckBoxList.Pedido_Poblacion2);
                command.Parameters.AddWithValue("@pepro", CheckBoxList.Pedido_Producto);
                command.Parameters.AddWithValue("@pepueren", CheckBoxList.Pedido_Puerto_entrega);
                command.Parameters.AddWithValue("@peref", CheckBoxList.Pedido_Referencia);
                command.Parameters.AddWithValue("@pesohas", CheckBoxList.Pedido_Solicita_Hasta);
                command.Parameters.AddWithValue("@petica", CheckBoxList.Pedido_Tipo_camion);
                command.Parameters.AddWithValue("@peurg", CheckBoxList.Pedido_URG);
                command.Parameters.AddWithValue("@pevenru", CheckBoxList.Pedido_Venta_ruta);
                command.Parameters.AddWithValue("@pevenruso", CheckBoxList.Pedido_Venta_ruta_solicitada);
                command.Parameters.AddWithValue("@pref", CheckBoxList.Prefactura);
                command.Parameters.AddWithValue("@procon", CheckBoxList.Programacion_Conductor);
                command.Parameters.AddWithValue("@prodes", CheckBoxList.Programacion_Descarga);
                command.Parameters.AddWithValue("@prokmsfac", CheckBoxList.Programacion_Kms_Facturables);
                command.Parameters.AddWithValue("@prokmsva", CheckBoxList.Programacion_Kms_Vacios);
                command.Parameters.AddWithValue("@prore", CheckBoxList.Programacion_Remolque);
                command.Parameters.AddWithValue("@protrac", CheckBoxList.Programacion_Tractora);
                command.Parameters.AddWithValue("@provenru", CheckBoxList.Programacion_Venta_Ruta);
                command.Parameters.AddWithValue("@provia", CheckBoxList.Programacion_Viaje);
                command.Parameters.AddWithValue("@sonin", CheckBoxList.Solicitado_Ninguna);
                command.Parameters.AddWithValue("@soki", CheckBoxList.Solicitado_Kilogramos);
                command.Parameters.AddWithValue("@sokm", CheckBoxList.Solicitado_KM);
                command.Parameters.AddWithValue("@soli", CheckBoxList.Solicitado_Litros);
                command.Parameters.AddWithValue("@som3", CheckBoxList.Solicitado_M3);
                command.Parameters.AddWithValue("@souni", CheckBoxList.Solicitado_Unidades);
                command.Parameters.AddWithValue("@venco", CheckBoxList.Venta_Comision);
                command.Parameters.AddWithValue("@venco2", CheckBoxList.Venta_Comision2);
                command.Parameters.AddWithValue("@venpea", CheckBoxList.Venta_Peaje);
                command.Parameters.AddWithValue("@venpea2", CheckBoxList.Venta_Peaje2);
                command.Parameters.AddWithValue("@venpor", CheckBoxList.Venta_Portes);
                command.Parameters.AddWithValue("@vensu", CheckBoxList.Venta_Suplidos);
                command.Parameters.AddWithValue("@vensu2", CheckBoxList.Venta_Suplidos2);
                command.Parameters.AddWithValue("@venta1", CheckBoxList.Venta_Tarifa1);
                command.Parameters.AddWithValue("@venta2", CheckBoxList.Venta_Tarifa2);
                command.Parameters.AddWithValue("@ventare", CheckBoxList.Venta_Tasa_recargo);
                command.Parameters.AddWithValue("@ventare2", CheckBoxList.Venta_Tasa_recargo2);
                command.Parameters.AddWithValue("@venvento", CheckBoxList.Venta_Venta_Total);

                command.Parameters.AddWithValue("@uid", iUserId);

                con.Open();

                command.ExecuteNonQuery();

                con.Close();

                this.Close();
            }
        }
    }
    // en rojo los checkbox que no tengan todavía equivalencia en BD
    public class CheckBoxList
    {
        public static bool Compra_Compra_Vacios { get; set; }
        public static bool Compra_Peaje		{ get; set; }
        public static bool Compra_Portes		{ get; set; }
        public static bool Compra_Precio_Tarifa { get; set; }
        public static bool Compra_Suplidos { get; set; }
        public static bool Compra_Tasa_recargo { get; set; }
        public static bool Compra_Total { get; set; }
        public static bool Entrega_Ninguna		{ get; set; }
        public static bool Entrega_Hora_Salida_Cliente { get; set; }
        public static bool Entrega_Kilogramos { get; set; }
        public static bool Entrega_KM { get; set; }
        public static bool Entrega_Litros { get; set; }
        public static bool Entrega_M3 { get; set; }
        public static bool Entrega_Unidades { get; set; }
        public static bool Fechas_Entrega { get; set; }
        public static bool Fechas_Prevista { get; set; }
        public static bool Fechas_Real { get; set; }
        public static bool Observaciones { get; set; }
        public static bool Pedido { get; set; }
        public static bool PedidoId { get; set; }
        public static bool Pedido_AGENTE { get; set; }
        public static bool Pedido_Albaran { get; set; }
        public static bool Pedido_Alquilado { get; set; }
        public static bool Pedido_CO       { get; set; }
        public static bool Pedido_Cliente { get; set; }
        public static bool Pedido_CMR { get; set; }
        public static bool Pedido_Coste_almacen_entrega { get; set; }
        public static bool Pedido_Depot { get; set; }
        public static bool Pedido_Estado_entrega { get; set; }
        public static bool Pedido_Factoria { get; set; }
        public static bool Pedido_Hora_carga_salida { get; set; }
        public static bool Pedido_Hora_final { get; set; }
        public static bool Pedido_Hora_inicio { get; set; }
        public static bool Pedido_Impreso { get; set; }
        public static bool Pedido_Numero_solicitud { get; set; }
        public static bool Pedido_Observaciones_bunker { get; set; }
        public static bool Pedido_Operador { get; set; }
        public static bool Pedido_Pag_Escaneado { get; set; }
        public static bool Pedido_Poblacion { get; set; }
        public static bool Pedido_Poblacion2 { get; set; }
        public static bool Pedido_Producto { get; set; }
        public static bool Pedido_Puerto_entrega { get; set; }
        public static bool Pedido_Referencia { get; set; }
        public static bool Pedido_Solicita_Hasta { get; set; }
        public static bool Pedido_Tipo_camion { get; set; }
        public static bool Pedido_URG { get; set; }
        public static bool Pedido_Venta_ruta { get; set; }
        public static bool Pedido_Venta_ruta_solicitada { get; set; }
        public static bool Prefactura { get; set; }
        public static bool Programacion_Conductor { get; set; }
        public static bool Programacion_Descarga { get; set; }
        public static bool Programacion_Kms_Facturables { get; set; }
        public static bool Programacion_Kms_Vacios { get; set; }
        public static bool Programacion_Remolque { get; set; }
        public static bool Programacion_Tractora { get; set; }
        public static bool Programacion_Venta_Ruta { get; set; }
        public static bool Programacion_Viaje { get; set; }
        public static bool Solicitado_Ninguna		{ get; set; }
        public static bool Solicitado_Kilogramos { get; set; }
        public static bool Solicitado_KM { get; set; }
        public static bool Solicitado_Litros { get; set; }
        public static bool Solicitado_M3 { get; set; }
        public static bool Solicitado_Unidades { get; set; }
        public static bool Venta_Comision { get; set; }
        public static bool Venta_Comision2 { get; set; }
        public static bool Venta_Peaje { get; set; }
        public static bool Venta_Peaje2 { get; set; }
        public static bool Venta_Portes { get; set; }
        public static bool Venta_Suplidos { get; set; }
        public static bool Venta_Suplidos2 { get; set; }
        public static bool Venta_Tarifa1		{ get; set; }
        public static bool Venta_Tarifa2 { get; set; }
        public static bool Venta_Tasa_recargo { get; set; }
        public static bool Venta_Tasa_recargo2 { get; set; }
        public static bool Venta_Venta_Total { get; set; }
    }
}
