using EtasaDesktop.Common;
using EtasaDesktop.Common.Tools;
using EtasaDesktop.Distribution.Orders.Imports;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Text;

namespace EtasaDesktop.Distribution.Orders.Form
{
    public class OrderFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        private bool _isSelected;
        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;

        //private ImportOperator _selectedOperator;

        private OrderDataSet.OrdersRow _row;

        private OrderDataSet.Orders_ObsRow _row_obs;

        public ObservableCollection<OrderFormViewModel> Factory { get; private set; }

        public OrderFormViewModel()
        {
            OrderDataSet dataset = new OrderDataSet();
            OrderDataSetTableAdapters.OrdersTableAdapter Order= new OrderDataSetTableAdapters.OrdersTableAdapter();
            OrderDataSetTableAdapters.Orders_ObsTableAdapter Order_obs = new OrderDataSetTableAdapters.Orders_ObsTableAdapter();

            _row = dataset.Orders.NewOrdersRow();
            _row_obs = dataset.Orders_Obs.NewOrders_ObsRow();
 
            //Order
            _row.Reference = "";
            _row.OperatorId = 0;
            _row.ClientId = 0;
            _row.StartDate = DateTime.Now;
            _row.FinalDate = DateTime.MinValue;
            _row.Address = "";
            _row.City = "";
            _row.PostCode = "";
            _row.FactoryId = 0;
            _row.ProductId = 0;
            _row.VehicleSize = 1;
            _row.RequestedAmount = 0;
            _row.Status = 1; 

            _row.CreatedDate = DateTime.Now;
            _row.ModifiedDate = DateTime.Now;

            //observaciones pedido
            _row_obs.Observations = "";
            _row_obs.Description = "";

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public string Reference
        {
            get { return _row.Reference; }
            set
            {
                if (_row.Reference != value)
                {
                    _row.Reference = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Address
        {
            get { return _row.Address; }
            set
            {
                if (_row.Address != value)
                {
                    _row.Address = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string City
        {
            get { return _row.City; }
            set
            {
                if (_row.City != value)
                {
                    _row.City = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string PostCode
        {
            get { return _row.PostCode; }
            set
            {
                if (_row.PostCode != value)
                {
                    _row.PostCode = value;
                    RaisePropertyChanged();
                }
            }
        }


        #region Factory input
        public string  _FactoryCode;
        public bool    _FactoryNotFound;
        public string  _FactoryName;


        public int FactoryId
        {
            get { return _row.FactoryId; }
            set
            {
                if (_row.FactoryId != value)
                {
                    _row.FactoryId = value;
                    RaisePropertyChanged();
                    UpdateFactoryInfo(value);
                }
            }
        }

        public string FactoryCode
        {
            get { return _FactoryCode; }
            set
            {
                if (_FactoryCode != value)
                {
                    _FactoryCode = value;
                    RaisePropertyChanged();
                    UpdateFactoryCode(value);
                }
            }
        }

        public bool FactoryNotFound
        {
            get { return _FactoryNotFound; }
            set
            {
                if (_FactoryNotFound != value)
                {
                    _FactoryNotFound = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FactoryName
        {
            get { return _FactoryName; }
            set
            {
                if (_FactoryName != value)
                {
                    _FactoryName = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region Product input
        public string _ProductCode;
        public bool _ProductNotFound;
        public string _ProductName;


        public int ProductId
        {
            get { return _row.ProductId;
            }
            set
            {
                if (_row.ProductId != value)
                {
                    _row.ProductId = value;
                    RaisePropertyChanged();
                    UpdateProductInfo(value);
                }
            }
        }

        public string ProductCode
        {
            get { return _ProductCode; }
            set
            {
                if (_ProductCode != value)
                {
                    _ProductCode = value;
                    RaisePropertyChanged();
                    UpdateProductCode(value);
                }
            }
        }

        public bool ProductNotFound
        {
            get { return _ProductNotFound; }
            set
            {
                if (_ProductNotFound != value)
                {
                    _ProductNotFound = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ProductName
        {
            get { return _ProductName; }
            set
            {
                if (_ProductName != value)
                {
                    _ProductName = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region Client input
        public string _ClientCode;
        public bool _ClientNotFound;
        public string _ClientName;


        public int ClientId
        {
            get { return _row.ClientId; }
            set
            {
                if (_row.ClientId != value)
                {
                    _row.ClientId = value;
                    RaisePropertyChanged();
                    UpdateClientInfo(value);
                }
            }
        }

        public string ClientCode
        {
            get { return _ClientCode; }
            set
            {
                if (_ClientCode != value)
                {
                    _ClientCode = value;
                    RaisePropertyChanged();
                    UpdateClientCode(value);
                }
            }
        }

        public bool ClientNotFound
        {
            get { return _ClientNotFound; }
            set
            {
                if (_ClientNotFound != value)
                {
                    _ClientNotFound = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ClientName
        {
            get { return _ClientName; }
            set
            {
                if (_ClientName != value)
                {
                    _ClientName = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion


        #region Operator input
        public string _OperatorCode;
        public bool _OperatorNotFound;
        public string _OperatorName;


        public int OperatorId
        {
            get { return _row.OperatorId; }
            set
            {
                if (_row.OperatorId != value)
                {
                    _row.OperatorId = value;
                    RaisePropertyChanged();
                    UpdateOperatorInfo(value);
                }
            }
        }

        public string OperatorCode
        {
            get { return _OperatorCode; }
            set
            {
                if (_OperatorCode != value)
                {
                    _OperatorCode = value;
                    RaisePropertyChanged();
                    UpdateOperatorCode(value);
                }
            }
        }

        public bool OperatorNotFound
        {
            get { return _OperatorNotFound; }
            set
            {
                if (_OperatorNotFound != value)
                {
                    _OperatorNotFound = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string OperatorName
        {
            get { return _OperatorName; }
            set
            {
                if (_OperatorName != value)
                {
                    _OperatorName = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        public int VehicleSize
        {
            get { return _row.VehicleSize; }
            set
            {
                if (_row.VehicleSize != value)
                {
                    _row.VehicleSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int RequestedAmount
        {
            get { return _row.RequestedAmount; }
            set
            {
                if (_row.RequestedAmount != value)
                {
                    _row.RequestedAmount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int Status
        {
            get { return _row.Status; }
            set
            {
                if (_row.Status != value)
                {
                    _row.Status = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime FromDate
        {
            get { return _row.StartDate; }
            set
            {
                if (_row.StartDate != value)
                {
                    _row.StartDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime ToDate
        {
            get { return _row.FinalDate; }
            set
            {
                if (_row.FinalDate != value)
                {
                    _row.FinalDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Observation
        {
            get { return _row_obs.Observations; }
            set
            {
                if (_row_obs.Observations != value)
                {
                    _row_obs.Observations = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return _row_obs.Description; }
            set
            {
                if (_row_obs.Description != value)
                {
                    _row_obs.Description = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void UpdateFactoryInfo(int id)
        {
            if (id > 0)
            {
                OrderDataSet ds = new OrderDataSet();
                OrderDataSetTableAdapters.FactoriesTableAdapter adapter = new OrderDataSetTableAdapters.FactoriesTableAdapter();
                OrderDataSet.FactoriesDataTable dataTable = adapter.GetDataById(id);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSet.FactoriesRow row = (OrderDataSet.FactoriesRow) dataTable.Rows[0];

                    FactoryCode = row.Code;
                    FactoryName = row.Name;
                }
            }
        }

        private void UpdateFactoryCode(string code)
        {
            bool notFound = true;
            int id = 0;
            string name = null;

            if (!String.IsNullOrWhiteSpace(code))
            {
                OrderDataSet ds = new OrderDataSet();
                OrderDataSetTableAdapters.FactoriesTableAdapter adapter = new OrderDataSetTableAdapters.FactoriesTableAdapter();
                OrderDataSet.FactoriesDataTable dataTable = adapter.GetDataByCode(code);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSet.FactoriesRow row = (OrderDataSet.FactoriesRow)dataTable.Rows[0];

                    id = row.Id;
                    name = row.Name;
                    notFound = false;
                }
            }
            FactoryId = id;
            FactoryName = name;
            FactoryNotFound = notFound;
        }



        private void UpdateProductInfo(int id)
        {
            if (id > 0)
            {
                OrderDataSet ds = new OrderDataSet();
                OrderDataSetTableAdapters.ProductsTableAdapter adapter = new OrderDataSetTableAdapters.ProductsTableAdapter();
                OrderDataSet.ProductsDataTable dataTable = adapter.GetDataById(id);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSet.ProductsRow row = (OrderDataSet.ProductsRow)dataTable.Rows[0];

                    ProductCode = row.Code;
                    ProductName = row.Name;
                }
            }
        }

        private void UpdateProductCode(string code)
        {
            bool notFound = true;
            int id = 0;
            string name = null;

            if (!String.IsNullOrWhiteSpace(code))
            {
                OrderDataSet ds = new OrderDataSet();
                OrderDataSetTableAdapters.ProductsTableAdapter adapter = new OrderDataSetTableAdapters.ProductsTableAdapter();
                OrderDataSet.ProductsDataTable dataTable = adapter.GetDataByCode(code);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSet.ProductsRow row = (OrderDataSet.ProductsRow)dataTable.Rows[0];

                    id = row.Id;
                    name = row.Name;
                    notFound = false;
                }
            }
            ProductId = id;
            ProductName = name;
            ProductNotFound = notFound;
        }

        private void UpdateClientInfo(int id)
        {
            if (id > 0)
            {
                OrderDataSet ds = new OrderDataSet();
                OrderDataSetTableAdapters.ClientsTableAdapter adapter = new OrderDataSetTableAdapters.ClientsTableAdapter();
                OrderDataSet.ClientsDataTable dataTable = adapter.GetDataById(id);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSet.ClientsRow row = (OrderDataSet.ClientsRow)dataTable.Rows[0];

                   ClientCode = row.Code;
                   ClientName = row.Name;
                }
            }
        }

        private void UpdateClientCode(string code)
        {
            bool notFound = true;
            int id = 0;
            string name = null;

            if (!String.IsNullOrWhiteSpace(code))
            {
                OrderDataSet ds = new OrderDataSet();
                OrderDataSetTableAdapters.ClientsTableAdapter adapter = new OrderDataSetTableAdapters.ClientsTableAdapter();
                OrderDataSet.ClientsDataTable dataTable = adapter.GetDataByCode(code);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSet.ClientsRow row = (OrderDataSet.ClientsRow)dataTable.Rows[0];

                    id = row.Id;
                    name = row.Name;
                    notFound = false;
                }
            }
            ClientId = id;
            ClientName = name;
            ClientNotFound = notFound;
        }

        private void UpdateOperatorInfo(int id)
        {
            if (id > 0)
            {
                OrderDataSet ds = new OrderDataSet();
                OrderDataSetTableAdapters.OperatorsTableAdapter adapter = new OrderDataSetTableAdapters.OperatorsTableAdapter();
                OrderDataSet.OperatorsDataTable dataTable = adapter.GetDataById(id);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSet.OperatorsRow row = (OrderDataSet.OperatorsRow)dataTable.Rows[0];

                    OperatorCode = row.Code;
                    OperatorName = row.Name;
                }
            }
        }

        private void UpdateOperatorCode(string code)
        {
            bool notFound = true;
            int id = 0;
            string name = null;

            if (!String.IsNullOrWhiteSpace(code))
            {
                OrderDataSet ds = new OrderDataSet();
                OrderDataSetTableAdapters.OperatorsTableAdapter adapter = new OrderDataSetTableAdapters.OperatorsTableAdapter();
                OrderDataSet.OperatorsDataTable dataTable = adapter.GetDataByCode(code);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSet.OperatorsRow row = (OrderDataSet.OperatorsRow)dataTable.Rows[0];

                    id = row.Id;
                    name = row.Name;
                    notFound = false;
                }
            }
            OperatorId = id;
            OperatorName = name;
            OperatorNotFound = notFound;
        }

        private bool CanSave()
        {
            return true;
        }

        public void Load(long IdOrder)
        {
            try
            {

                OrderDataSet dataset = new OrderDataSet();
                OrderDataSetTableAdapters.OrdersTableAdapter TableOrder = new OrderDataSetTableAdapters.OrdersTableAdapter();
                OrderDataSet.OrdersDataTable dataTable = TableOrder.GetDataOrderById(IdOrder);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDataSetTableAdapters.Orders_ObsTableAdapter order_obs = new OrderDataSetTableAdapters.Orders_ObsTableAdapter();
                    OrderDataSet.Orders_ObsDataTable dataTable_Obs = order_obs.GetDataOrder_ObsById(IdOrder);

                    _row = (OrderDataSet.OrdersRow)dataTable.Rows[0];

                    RaisePropertyChanged(nameof(Reference));
                    RaisePropertyChanged(nameof(OperatorCode));
                    RaisePropertyChanged(nameof(ClientCode));
                    RaisePropertyChanged(nameof(ProductCode));
                    RaisePropertyChanged(nameof(FactoryCode));
                    RaisePropertyChanged(nameof(ClientId));
                    RaisePropertyChanged(nameof(Address));
                    RaisePropertyChanged(nameof(City));
                    RaisePropertyChanged(nameof(PostCode));
                    RaisePropertyChanged(nameof(FromDate));
                    RaisePropertyChanged(nameof(ToDate));
                    RaisePropertyChanged(nameof(ProductId));
                    RaisePropertyChanged(nameof(VehicleSize));                         
                    RaisePropertyChanged(nameof(RequestedAmount));
                    RaisePropertyChanged(nameof(Status));
                   
                    //Cargamos datos 
                    LoadDatasInterface(_row.ProductId,_row.OperatorId,_row.ClientId,_row.FactoryId);
           
                    _row_obs = (OrderDataSet.Orders_ObsRow)dataTable_Obs.Rows[0];
                    RaisePropertyChanged(nameof(Description));
                    RaisePropertyChanged(nameof(Observation));
                   
                }
                FormLoadFinished?.Invoke();
                
            }
            catch (Exception e)
            {
                FormLoadError?.Invoke(e);
            }
        }
        public void Save()
        {
            int indentificadorPedido = 0;
            bool validate = true;
            try
            {
                validate = validateDatas(_row.Reference,_row.OperatorId,_row.ProductId,_row.ClientId,_row.FactoryId,_row.StartDate,_row.FinalDate);

                if (validate)
                {
                    OrderDataSet dataset = new OrderDataSet();
                    OrderDataSetTableAdapters.Orders_ObsTableAdapter tableAdapterOrderObs = new OrderDataSetTableAdapters.Orders_ObsTableAdapter();

                    OrderDataSetTableAdapters.OrdersTableAdapter TableOrder = new OrderDataSetTableAdapters.OrdersTableAdapter();
                    OrderDataSet.OrdersDataTable dataTable = TableOrder.GetDataOrderById(_row.Id);

                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowOrder = dataset.Orders.NewOrdersRow();

                        rowOrder = dataTable[0];
                        rowOrder["Reference"] = _row.Reference;
                        rowOrder["OperatorId"] = _row.OperatorId;
                        rowOrder["ClientId"] = _row.ClientId;
                        rowOrder["Address"] = _row.Address;
                        rowOrder["City"] = _row.City;
                        rowOrder["PostCode"] = _row.PostCode;
                        rowOrder["StartDate"] = _row.StartDate;
                        rowOrder["FinalDate"] = _row.FinalDate;
                        rowOrder["FactoryId"] = _row.FactoryId;
                        rowOrder["ProductId"] = _row.ProductId;
                        rowOrder["VehicleSize"] = _row.VehicleSize;
                        rowOrder["RequestedAmount"] = _row.RequestedAmount;
                        rowOrder["Status"] = _row.Status;
                        rowOrder["CreatedDate"] = DateTime.Now;
                        rowOrder["ModifiedDate"] = DateTime.Now;

                        int numeroderegistros = TableOrder.Update(rowOrder);

                        //si se actualizado la pedido actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            tableAdapterOrderObs.UpdateOrderObs(_row_obs.Observations, _row_obs.Description, _row.Id);
                        }
                    }
                    //insertamos el nuevo Orders  y las observaciones (orders_obs)
                    else
                    {
                        TableOrder = new OrderDataSetTableAdapters.OrdersTableAdapter();
                        //obtenemos de la tabla Orders  el nuevo registro que se introducira 
                        var rowOrder = dataset.Orders.NewOrdersRow();

                        rowOrder.Reference = _row.Reference;
                        rowOrder.OperatorId = _row.OperatorId;
                        rowOrder.ClientId = _row.ClientId;
                        rowOrder.Address = _row.Address;
                        rowOrder.City = _row.City;
                        rowOrder.PostCode = _row.PostCode;
                        rowOrder.Latitude = 0;
                        rowOrder.Longitude = 0;
                        rowOrder.TankNum = "1";
                        rowOrder.TankVolume = 0;
                        rowOrder.Country = "es";
                        rowOrder.StartDate = _row.StartDate;
                        rowOrder.FinalDate = _row.FinalDate;
                        rowOrder.FactoryId = _row.FactoryId;
                        rowOrder.ProductId = _row.ProductId;
                        rowOrder.VehicleSize = _row.VehicleSize;
                        rowOrder.RequestedAmount = _row.RequestedAmount;
                        rowOrder.Status = _row.Status;
                        rowOrder.CreatedDate = DateTime.Now;
                        rowOrder.ModifiedDate = DateTime.Now;

                        //agregamos la nueva fila 
                        dataset.Orders.AddOrdersRow(rowOrder);
                        //actualizamos la tabla order en el dataset 
                        TableOrder.Update(dataset.Orders);

                        //al insertar el registro nuevo en la tabla Order Id se actualiza 
                        indentificadorPedido = Convert.ToInt32(rowOrder.Id);

                        // a continuación insertamos el resto de datos en la tabla Orders_obs cepsa con el indentificador de la orders recient introduzida
                        tableAdapterOrderObs = new OrderDataSetTableAdapters.Orders_ObsTableAdapter();
                        //obtenemos de la tabla Orders_obs el nuevo registro que se introducira 
                        var rowOrder_obs = dataset.Orders_Obs.NewOrders_ObsRow();

                        //Id
                        rowOrder_obs.Id = indentificadorPedido;
                        //observations
                        rowOrder_obs.Observations = _row_obs.Observations;
                        //description
                        rowOrder_obs.Description = _row_obs.Description;
                        //agregamos la nueva fila a tabla orders_obs
                        dataset.Orders_Obs.AddOrders_ObsRow(rowOrder_obs);
                        //actualizamos la tabla orders_obs  en el dataset 
                        tableAdapterOrderObs.Update(dataset.Orders_Obs);

                        //creamos el viaje que se asignara al pedido
                        OrderDataSet ImportDataSet = new OrderDataSet();
                        OrderDataSetTableAdapters.TripsTableAdapter tripstable = new OrderDataSetTableAdapters.TripsTableAdapter();
                        //obtenemos de la tabla trips  el nuevo registro que se introducira 
                        var rowTrip = ImportDataSet.Trips.NewTripsRow();

                        //la ruta a la que pertenece el viaje que estamos creando y esta asignado la asignación
                        rowTrip.SetRouteIdNull();
                        //fecha de creación
                        rowTrip.CreatedDate = DateTime.Now;
                        //fecha de modificación
                        rowTrip.ModifiedDate = DateTime.Now;
                        //posición dentro la lista (empiezan por zero las listas en posiciones) (el primer elemento empieza por 1)           
                        rowTrip.Position = 0;

                        //cantidad cargada del pedido       
                        rowTrip.LoadedAmount = 0;
                        //fecha de carga
                        rowTrip.LoadedDate = DateTime.Now;
                        //estado del pedido a (pendiente) 
                        rowTrip.status = 1;
                        //agregamos la nueva fila 
                        ImportDataSet.Trips.AddTripsRow(rowTrip);
                        //asignamos el id del pedido 
                        rowTrip.Id_Order = Convert.ToInt32(indentificadorPedido);
                        //actualizamos la tabla trips en el dataset  (con esto insertamos un nuevo viaje) 
                        tripstable.Update(ImportDataSet.Trips);
                        
                    }
                    FormSaveFinished?.Invoke();
                }
            }
            catch (Exception e)
            {
                FormSaveError?.Invoke(e);
            }

        }
        public void LoadDatasInterface(int ProductId, int OperatorId, int ClientId, int FactoryId)
        {

            OrderDataSet dataset = new OrderDataSet();
            OrderDataSetTableAdapters.ProductsTableAdapter tableproduct = new OrderDataSetTableAdapters.ProductsTableAdapter();
            OrderDataSetTableAdapters.OperatorsTableAdapter tableOperator = new OrderDataSetTableAdapters.OperatorsTableAdapter();
            OrderDataSetTableAdapters.ClientsTableAdapter tableClient = new OrderDataSetTableAdapters.ClientsTableAdapter();
            OrderDataSetTableAdapters.FactoriesTableAdapter tableFactory = new OrderDataSetTableAdapters.FactoriesTableAdapter();

            string CodeProducts = "";
            string NameProducts = "";
            string CodeOperators = "";
            string NameOperators = "";
            string CodeClients = "";
            string NameClients = "";
            string CodeFactories = "";
            string NameFactories = "";

            //datos produtcto 
            CodeProducts = tableproduct.GetDataById(ProductId)[0].Code;
            NameProducts = tableproduct.GetDataById(ProductId)[0].Name;
            ProductCode = CodeProducts;
            ProductName = NameProducts;

            //datos operador
            CodeOperators = tableOperator.GetDataById(OperatorId)[0].Code;
            NameOperators = tableOperator.GetDataById(OperatorId)[0].Name;
            OperatorCode = CodeOperators;
            OperatorName = NameOperators;

            //datos cliente
            CodeClients = tableClient.GetDataById(ClientId)[0].Code;
            NameClients = tableClient.GetDataById(ClientId)[0].Name;
            ClientCode = CodeClients;
            ClientName = NameClients;

            //datos factoría
            CodeFactories = tableFactory.GetDataById(FactoryId)[0].Code;
            NameFactories = tableFactory.GetDataById(FactoryId)[0].Name;
            FactoryCode = CodeFactories;
            FactoryName = NameFactories;

        }

        private static bool validateDatas(string reference, int OperatorId, int ProductId, int ClientId, int FactoryId, DateTime StartDate, DateTime FinalDate)
        {
            bool validateDatas = true;
            if (string.IsNullOrEmpty(reference))
            {
                MessageBox.Show("La referencia no puede estar en blanco", "Campo Referéncia", MessageBoxButton.OK, MessageBoxImage.Error);
                validateDatas = false;
            }
            else
            {
                if (StartDate > FinalDate)
                {
                    MessageBox.Show("La fecha inicio no puede ser superior a la fecha final", "Campo fecha inicio", MessageBoxButton.OK, MessageBoxImage.Error);
                    validateDatas = false;

                }
                else
                { 
                   if (reference.Length > 15)
                    {
                        MessageBox.Show("La referéncia no puede tener más 15 caracteres", "Campo referéncia", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {
                        if (OperatorId == 0)
                        {
                            MessageBox.Show("El código del operador no es valido", "Campo Operador", MessageBoxButton.OK, MessageBoxImage.Error);
                            validateDatas = false;
                        }
                        else
                        {
                            if (ProductId == 0)
                            {
                                MessageBox.Show("El código del producto no es valido", "Campo Producto", MessageBoxButton.OK, MessageBoxImage.Error);
                                validateDatas = false;
                            }
                            else
                            {
                                if (ClientId == 0)
                                {
                                    MessageBox.Show("El código del Cliente no es valido", "Campo Cliente", MessageBoxButton.OK, MessageBoxImage.Error);
                                    validateDatas = false;
                                }
                                else
                                {
                                    if (FactoryId == 0)
                                    {
                                        MessageBox.Show("El código de la factoría no es valida", "Campo factoría", MessageBoxButton.OK, MessageBoxImage.Error);
                                        validateDatas = false;
                                    }
                                }
                            }

                        }
                    }
                }
            }
          return validateDatas;
        }      
    }
}
