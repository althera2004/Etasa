using EtasaDesktop.Common;
using EtasaDesktop.Common.Data;
using EtasaDesktop.Common.Tools;
using EtasaDesktop.Distribution.Planner;
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
using EtasaDesktop.Files.Sftp;

namespace EtasaDesktop.Distribution.Orders.Imports

{
    public class ImportOrdersViewModel : ViewModelBase
    {
        public RelayCommand ShowImportFolderCommand { get; private set; }
        public RelayCommand ShowProcessedFolderCommand { get; private set; }
        public RelayCommand RefreshImportFolderCommand { get; private set; }
        public RelayCommand ImportCommand { get; private set; }
        public RelayCommand ShowConfigCommand { get; private set; }


        private DateTime _date;
        private ImportOperator _selectedOperator;
        private FileInfo _selectedFile;
        private bool _fileReadError;
        private bool _canImport;


        public ObservableCollection<ImportOperator> Operators { get; private set; }
        public ObservableCollection<FileInfo> Files { get; private set; }
        public ObservableCollection<ImportDataViewModel> ImportOrderList { get; private set; }

        public DateTime Date
        {
            get => _date;
            set
            {
                Set(ref _date, value);
                UpdateImportDates();
            }
        }
        public ImportOperator SelectedOperator
        {
            get => _selectedOperator;
            set
            {
                Set(ref _selectedOperator, value);
                UpdateImport(value.Name.ToString());
            }
        }
        public FileInfo SelectedFile
        {
            get => _selectedFile;
            set
            {
                Set(ref _selectedFile, value);
                UpdateImportGrid();
            }
        }
        public DirectoryInfo Folder { get; private set; }
        public ImportConfigurationViewModel ConfigViewModel { get; private set; }
        public int ExpirationDays { get; set; }
        public bool CanImport
        {
            get => _canImport;
            private set => Set(ref _canImport, value);
        }
        public bool CanShowFolders
        {
            get => Folder != null && Folder.Exists;
        }



        public ImportOrdersViewModel()
        {
            ImportCommand = new RelayCommand(Import, CanExecuteImport);
            ShowImportFolderCommand = new RelayCommand(ShowImportFolder, CanExecuteShowFolders);
            ShowProcessedFolderCommand = new RelayCommand(ShowProcessedFolder, CanExecuteShowFolders);
            RefreshImportFolderCommand = new RelayCommand(RefreshImportFolder, CanExecuteShowFolders);
            ShowConfigCommand = new RelayCommand(ShowConfig, CanShowConfig);

            Files = new ObservableCollection<FileInfo>();
            Operators = new ObservableCollection<ImportOperator>();
            ImportOrderList = new ObservableCollection<ImportDataViewModel>();

            Date = DateTime.Today;

            RequestOperators();

            Files.CollectionChanged += OnFilesCollectionChanged;
            ImportOrderList.CollectionChanged += OnImportOrderCollectionChanged;
        }



        // La acción de importación
        private void Import()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                ImportData();
            }
        }

        private bool CanExecuteImport()
        {
            return CanImport;
        }

        // Las opciones del directorio de importación
        private void ShowImportFolder()
        {
            Process.Start(ConfigViewModel.Folder);
        }
        private void ShowProcessedFolder()
        {
            Process.Start(ConfigViewModel.ProcessedFolder);
        }
        private void RefreshImportFolder()
        {
            RefreshFileList();
        }
        private bool CanExecuteShowFolders()
        {
            return CanShowFolders;
        }

        // Muestra la ventana de configuración
        public void ShowConfig()
        {
            if (SelectedOperator != null)
            {
                ImportConfigWindow config = new ImportConfigWindow(ConfigViewModel.Config)
                {
                    ShowInTaskbar = false,
                    Owner = Application.Current.MainWindow,
                };
                config.ShowDialog();

                var result = config.DialogResult;

                if(result.HasValue && result.Value)
                {
                    ConfigViewModel.Config = config.Result;
                    switch (SelectedOperator.Code)
                    {
                        case "1000":
                            Properties.Settings.Default.ImportVitogasConfig = config.Result;
                            Properties.Settings.Default.Save();
                            break;

                        case "7777":
                            Properties.Settings.Default.ImportGalpConfig = config.Result;
                            Properties.Settings.Default.Save();
                            break;

                        case "0034":
                            Properties.Settings.Default.ImportCepsaConfig = config.Result;
                            Properties.Settings.Default.Save();
                            break;

                    }

                    ExpirationDays = ConfigViewModel.OrderExpiration;
                    UpdateImportDates();
                }
            }
        }
        public bool CanShowConfig()
        {
            return SelectedOperator != null;
        }


        // Pide los operadores al servidor
        public void RequestOperators()
        {
            ImportDataSet ds = new ImportDataSet();
            ImportDataSetTableAdapters.ImportOperatorsTableAdapter adapt = new ImportDataSetTableAdapters.ImportOperatorsTableAdapter();
            adapt.Fill(ds.ImportOperators);

            ImportDataSet.ImportOperatorsDataTable dt = ds.ImportOperators;
            foreach (DataRow row in dt.Rows)
            {
                Operators.Add(new ImportOperator()
                {
                    Id = Convert.ToInt32(row[dt.IdColumn]),
                    Code = row[dt.CodeColumn] as string,
                    Name = row[dt.NameColumn] as string,
                });
            }
        }

        // Refresca la lista de ficheros
        public void RefreshFileList()
        {
            try
            {
                FileInfo prevSel = SelectedFile;
                var ext = ConfigViewModel.FileExtension;

                if (ConfigViewModel != null)
                {
                    Files.Clear();
                    foreach (FileInfo file in Folder.GetFiles())
                    {
                        if (file.Extension.Equals(ext))
                            Files.Add(file);
                    }

                    if (prevSel != null)
                    {
                        IEnumerable<FileInfo> obsCollection = Files;
                        var list = new List<FileInfo>(obsCollection);

                        SelectedFile = list.Find(file => file.Name == prevSel.Name);
                    }
                }
            }
            catch
            {
                if (CanShowFolders != false)
                {
                    MessageBox.Show("No se pudo abrir la carpeta", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Carga la configuración
        private void LoadConfiguration()
        {
            var config = LoadConfig();

            Folder = new DirectoryInfo(config.Folder);
            ExpirationDays = config.OrderExpiration;
            ConfigViewModel = new ImportConfigurationViewModel()
            {
                Config = config
            };
            
        }


        void OnFilesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ShowImportFolderCommand.RaiseCanExecuteChanged();
            ShowProcessedFolderCommand.RaiseCanExecuteChanged();
            RefreshImportFolderCommand.RaiseCanExecuteChanged();
        }
        void OnImportOrderCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CanImport = ImportOrderList.Count > 0;
            ImportCommand.RaiseCanExecuteChanged();
        }
 

        // Actualiza los datos de importación
        private void UpdateImport(string operators)
        {
           
            LoadConfiguration();

            ShowConfigCommand.RaiseCanExecuteChanged();

            //si el operador es cepsa realizamos la connexión sftp para descargar los ficheros 
            if (operators.ToString() == "CEPSA - CIA ESPAÑOLA PETROLEOS")
            {
                //Descargamos fichero sftp del servidor de cepsa 
                Sftp DownloadFilesDat = new Sftp();
                DownloadFilesDat.DownloadListFilesDirectory(System.Configuration.ConfigurationManager.AppSettings["mft.cepsacorp.com"].ToString(), System.Configuration.ConfigurationManager.AppSettings["usuariosftp"], System.Configuration.ConfigurationManager.AppSettings["contraseñasftp"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["puerto"].ToString()), @System.Configuration.ConfigurationManager.AppSettings["rutadescargaficherossftp"].ToString());
            }
            RefreshFileList();
        }

        // Actualiza los datos de la tabla
        private void UpdateImportGrid()
        {
            ImportOrderList.Clear();
            LoadData(SelectedFile);

            if(ImportOrderList.Count > 0 && !_fileReadError)
            {
                Date = ImportOrderList[0].StartDate;
            }
        }
        private void LoadData(FileInfo file)
        {
            if (SelectedFile != null)
            {
                try
                {
                    if (SelectedFile.Extension.Equals(".csv"))
                    {

                        foreach (ImportData order in ImportHelper.FromCSV(SelectedFile, ConfigViewModel.Config, ExpirationDays))
                        {
                            ImportOrderList.Add(new ImportDataViewModel(order));
                        }
                        _fileReadError = false;
                    }
                    else if (SelectedFile.Extension.Equals(".xlsx"))
                    {
                        foreach (ImportData order in ImportHelper.FromExcel(SelectedFile, ConfigViewModel.Config, ExpirationDays))
                        {
                            ImportOrderList.Add(new ImportDataViewModel(order));
                        }
                        _fileReadError = false;
                    }
                    else if (SelectedFile.Extension.Equals(".dat"))
                    {
                        foreach (ImportData order in ImportHelper.FromDat(SelectedFile, ConfigViewModel.Config, ExpirationDays))
                        {
                            ImportOrderList.Add(new ImportDataViewModel(order));
                        }
                        _fileReadError = false;
                    }


                    else
                    {
                        if (!_fileReadError)
                        {
                            MessageBox.Show("El fichero no es compatible", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            _fileReadError = true;
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    if (!_fileReadError)
                    {
                        MessageBox.Show("No se encontró el fichero", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        _fileReadError = true;
                        RefreshFileList();
                    }

                }
                catch (Exception)
                {
                    if (!_fileReadError)
                    {
                        MessageBox.Show("No se pudo abrir el fichero", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        _fileReadError = true;
                    }

                }
            }
        }


        // Actualiza las fechas de los pedidos
        private void UpdateImportDates()
        {
            var startDate = Date;
            var finalDate = Date.AddDays(ExpirationDays);

            foreach (ImportDataViewModel order in ImportOrderList)
            {
                order.StartDate = startDate;
                order.FinalDate = finalDate;
            }
        }


        // Importación de datos a la BBDD
        private void ImportData()
        {
            int importedRows = ImportOrderList.Count;      
            if (importedRows > 0)
            {
                ImportDataSet ds = new ImportDataSet();
                bool success = true;
                using (var ts = new TransactionScope())
                {
                    // Perform updates using different table adapters
                    using (var ta1 = new ImportDataSetTableAdapters.ImportOrdersTableAdapter())
                    {
                        int i = 0;
                        while (ImportOrderList.Count > i && success)
                        {

                            ImportDataViewModel dr = ImportOrderList[i];
                            dr.OperatorId = SelectedOperator.Id;
                            ImportData data = dr.Data;

                            try
                            {
                                bool? isImported = ta1.CheckIfImported(data.OperatorId, data.Reference) as bool?;
                                if (isImported != null)
                                {
                                    if (isImported.Value)
                                        success = HandleDuplicateImportException(ta1, dr.Data);
                                    else
                                    {
                                       
                                        int IdentificadorPedido = 0;
                                        IdentificadorPedido = InsertOrder(ta1, dr.Data);

                               

                                        if (dr.OperatorId == 500 && IdentificadorPedido != 0)
                                        {

                                            using (var ta2 = new ImportDataSetTableAdapters.Orders_CepsaTableAdapter())
                                            {
                                                InsertOrderCepsa(ta2,dr.Data, IdentificadorPedido);                                        
                                            }
                                        }
                                        
                              
                                    }
                                }
                                else
                                {
                                    throw new Exception("No se ha podido acceder a los pedidos");
                                }

                            }
                            catch (ClientNotFoundException)
                            {
                                success = HandleImportClientNotFoundException(ta1, dr.Data);
                            }
                            catch (FactoryNotFoundException)
                            {
                                success = HandleImportFactoryNotFoundException(ta1, dr.Data);
                            }
                            catch (ProductNotFoundException)
                            {
                                success = HandleImportProductNotFoundException(ta1, dr.Data);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Ha ocurrido un error al importar. Referencia pedido: " + dr.Reference + "\n\nDetalles: " + e.Message, "Error de importación",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                success = false;
                            }

                            i++;
                        }


                        if (success)
                        {
                            ts.Complete();
                            ds.AcceptChanges();
                            
                            String dir = ConfigViewModel.ProcessedFolder;
                            new DirectoryInfo(dir).Create();
                            File.Move(SelectedFile.FullName, dir + "\\" + SelectedFile.Name);
                            RefreshFileList();
                            
                            MessageBox.Show("Importación completa", "Importación de pedidos",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }                  
                    }          
                }
            }
            else
                MessageBox.Show("No se ha importado ningun pedido");
        }

        // Tratamiento de los errores al importar
        private bool HandleImportClientNotFoundException(ImportDataSetTableAdapters.ImportOrdersTableAdapter ta1, ImportData data)
        {
            MessageBox.Show("No se ha podido crear un cliente nuevo. Referencia pedido: " + data.Reference, "Error de importación",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private bool HandleImportFactoryNotFoundException(ImportDataSetTableAdapters.ImportOrdersTableAdapter ta1, ImportData data)
        {
            MessageBox.Show("No se ha podido insertar. Factoria no reconocida. Referencia pedido: " + data.Reference, "Error de importación",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private bool HandleImportProductNotFoundException(ImportDataSetTableAdapters.ImportOrdersTableAdapter ta1, ImportData data)
        {
            MessageBox.Show("No se ha podido insertar. Producto no reconocido. Referencia pedido: " + data.Reference, "Error de importación",
                           MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private bool HandleDuplicateImportException(ImportDataSetTableAdapters.ImportOrdersTableAdapter ta1, ImportData data)
        {

            MessageBoxResult dialogResult = MessageBox.Show("El pedido con referencia: " + data.Reference + " ya existe. ¿Quieres reemplazarlo?", "Error de importación",
                       MessageBoxButton.YesNo, MessageBoxImage.Error);

            if (dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    UpdateOrder(ta1,data);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error al actualizar los datos del pedido. Referencia pedido: " + data.Reference+ "\n\nDetalles: " + e.Message, "Error de Actualización",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
          



            /*version antigua 
            int importedRows = ImportOrderList.Count;

            if (importedRows > 0)
            {

                bool success = true;

                ImportDataSet ds = new ImportDataSet();
                ImportDataSetTableAdapters.ImportOrdersTableAdapter adapt = new ImportDataSetTableAdapters.ImportOrdersTableAdapter();
                
            
                adapt.Connection.Open();
                adapt.Transaction = adapt.Connection.BeginTransaction();


                int i = 0;
                while (ImportOrderList.Count > i && success)
                {

                    ImportDataViewModel dr = ImportOrderList[i];
                    dr.OperatorId = SelectedOperator.Id;


                    ImportData data = dr.Data;

                    try
                    {
                        bool? isImported = adapt.CheckIfImported(data.OperatorId, data.Reference) as bool?;
                        if(isImported != null)
                        {
                            if (isImported.Value)
                                success = HandleDuplicateImportException(adapt, dr.Data);
                            else
                            {
                                ImportDataSetTableAdapters.Orders_CepsaTableAdapter adapt2 = new ImportDataSetTableAdapters.Orders_CepsaTableAdapter();
                                int IdentificadorPedido = 0;
                                IdentificadorPedido = InsertOrder(adapt, dr.Data);
                                InsertOrderCepsa(adapt, adapt2, dr.Data, IdentificadorPedido);

                                // TODO IMPORT CEPSA - Importar datos Orders_Cepsa
                            }
                                
                        }
                        else
                        {
                            throw new Exception("No se ha podido acceder a los pedidos");
                        }
                        
                    }
                    catch (ClientNotFoundException)
                    {
                        success = HandleImportClientNotFoundException(adapt, dr.Data);
                    }
                    catch (FactoryNotFoundException)
                    {
                        success = HandleImportFactoryNotFoundException(adapt, dr.Data);
                    }
                    catch (ProductNotFoundException)
                    {
                        success = HandleImportProductNotFoundException(adapt, dr.Data);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ha ocurrido un error al importar. Referencia pedido: " + dr.Reference + "\n\nDetalles: " + e.Message, "Error de importación",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        success = false;
                    }

                    i++;
                }
                

                if (success)
                {
                    adapt.Transaction.Commit();

                    String dir = ConfigViewModel.ProcessedFolder;
                    new DirectoryInfo(dir).Create();
                    File.Move(SelectedFile.FullName, dir + "\\" + SelectedFile.Name);
                    RefreshFileList();

                    MessageBox.Show("Importación completa", "Importación de pedidos",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    adapt.Transaction.Rollback();
                }

            }
            else
                MessageBox.Show("No se ha importado ningun pedido");
        }


        // Tratamiento de los errores al importar
        private bool HandleImportClientNotFoundException(ImportDataSetTableAdapters.ImportOrdersTableAdapter adapt, ImportData data)
        {
            MessageBox.Show("No se ha podido crear un cliente nuevo. Referencia pedido: " + data.Reference, "Error de importación",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private bool HandleImportFactoryNotFoundException(ImportDataSetTableAdapters.ImportOrdersTableAdapter adapt, ImportData data)
        {
            MessageBox.Show("No se ha podido insertar. Factoria no reconocida. Referencia pedido: " + data.Reference, "Error de importación",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private bool HandleImportProductNotFoundException(ImportDataSetTableAdapters.ImportOrdersTableAdapter adapt, ImportData data)
        {
            MessageBox.Show("No se ha podido insertar. Producto no reconocido. Referencia pedido: " + data.Reference, "Error de importación",
                           MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        private bool HandleDuplicateImportException(ImportDataSetTableAdapters.ImportOrdersTableAdapter adapt, ImportData data)
        {

            MessageBoxResult dialogResult = MessageBox.Show("El pedido con referencia: " + data.Reference + " ya existe. ¿Quieres reemplazarlo?", "Error de importación",
                       MessageBoxButton.YesNo, MessageBoxImage.Error);

            if (dialogResult == MessageBoxResult.Yes)
            {
                try
                {
                    UpdateOrder(adapt,data);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error al actualizar los datos del pedido. Referencia pedido: " + data.Reference+ "\n\nDetalles: " + e.Message, "Error de Actualización",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
            */
        }



        private void InsertOrderCepsa(ImportDataSetTableAdapters.Orders_CepsaTableAdapter adapt2,ImportData data,int IdentificadorPedido)
        {

            adapt2.InsertOrderCepsa(
            //id del pedido relacionado  
            IdentificadorPedido,
            //Num_Envio
            data.Num_Envio,
            //Literal00
            data.Literal00,
            //Contador00
            data.Contador00,
            //Tipo00
            data.Tipo00,
            //Fecha
            data.Fecha,
            //Operador00
            data.Operador00,
            //Suministrador00
            data.Suministrador00,
            //Transportista00
            data.Transportista00,
            //Literal05
            data.Literal05,
            //Contador05
            data.Contador05,
            //Tipo05
            data.Tipo05,
            //Instalacion05
            data.Instalacion05,
            //Cod_Cepsa
            data.Cod_Cepsa,
            //Cliente
            data.Cliente,
            //Destinatario
            data.Destinatario,
            //Cae
            data.Cae,
            //Reg_Fiscal
            data.Reg_Fiscal,
            //IEE
            data.IEE,
            //Fecha_Desde
            FormatDate(data.Fecha_Desde),
            //Fecha_Hasta
            FormatDate(data.Fecha_Hasta),
            //Duracion
            data.Duracion,
            //Condiciones05
            data.Condiciones05,
            //Hora_Desde
            data.Hora_Desde,
            //Hora_Hasta
            data.Hora_hasta,
            //Observacion1
            data.Observaciones1,
            //Observacion2
            data.Observaciones2,
            //Literal06
            data.Literal06,
            //Contador06
            data.Contador06,
            //Tipo06
            data.Tipo06,
            //Instalacion06
            data.Instalacion06,
            //Contacto
            data.Contacto,
            //Cargo
            data.Cargo,
            //Telefono
            data.Telefono,
            //Fax
            data.Fax,
            //Nif
            data.Nif,
            //Literal07
            data.Literal07,
            //Contador07
            data.Contador07,
            //Tipo07
            data.Tipo07,
            //Instalacion07
            data.Instalacion07,
            //Calle1
            data.Calle1,
            //Calle2
            data.Calle2,
            //Calle3
            data.Calle3,
            //Municipio
            data.Municipio,
            //Localidad
            data.Localidad,
            //Codigo_postal_int
            data.Cod_Post_Int,
            //Region
            data.Region,
            //ruta 
            data.Ruta,
            //Literal14
            data.Literal14,
            //Contador14
            data.Contador14,
            //Tipo14
            data.Tipo14,
            //Operador14
            data.Operador14,
            //Suministrador14
            data.Suministrador14,
            //Transportista14
            data.Transportista14,
            //Pedido
            data.Pedido,
            //Instalacion14
            data.Instalacion14,
            //Material
            data.Material,
            //Cantidad
            data.Cantidad,
            //Uni_med
            data.Uni_Med,
            //Tipo_oper
            data.Tipo_Oper,
            //Centro
            data.Centro,
            //Condiciones14
            data.Condiciones14,
            //Horario
            data.Horario,
            //Observaciones1
            data.Observaciones1,
            //Observaciones2
            data.Observaciones2,
            //Ult_descarga
            data.Utima_Descarga,
            //Llenado
            data.Llenado,
            //Vaciado
            data.Vaciado,
            //TankNum2
            "2",
            //TankVolume2
            ConvertValuesVolumeDeposit(data.Volumen_2),
            //TankLevel2
            ConvertValuesLevelDeposit(data.Nivel_Dep2),
            //TankNum3
            "3",
            //TankVolume3
            ConvertValuesVolumeDeposit(data.Volumen_3),
            //TankLevel3
            ConvertValuesLevelDeposit(data.Nivel_Dep3));
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

        //control nulos en los valores numericos de cantidad 
        private static Int32 ConvertValuesRequestAmount(string RequestAmount)
        {
            Int32 ValorRequestAmount = 0;
            if (!string.IsNullOrEmpty(RequestAmount))
            {
                ValorRequestAmount = Convert.ToInt32(RequestAmount);
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

        // Peticiones de la base de datos
        private int InsertOrder(ImportDataSetTableAdapters.ImportOrdersTableAdapter adapt, ImportData data)
        {
            int operatorId = data.OperatorId;
            int NewOrderID = 0;

            int clientId = ImportRequester.GetClientId(operatorId, data.ClientCode) ?? ImportRequester.CreateNewClient(data) ?? throw new ClientNotFoundException();
            int factoryId = ImportRequester.GetFactoryId(operatorId, data.FactoryCode) ?? throw new FactoryNotFoundException();
            int productId = ImportRequester.GetProductId(operatorId, data.ProductCode) ?? throw new ProductNotFoundException();
            int vehicleSizeId = ImportRequester.GetVehicleSizeId(data.VehicleSizeCode) ?? throw new Exception("El código de vehiculo no es válido");
            string provinceId = "00";   // HACK Provincia hardcodeada
            string countryId = "es";    // HACK Pais hardcodeado

            float TankVolume = 0;
            int TankLevel = 0;
            int Amount = 0;

            //tractament volume
            if (!string.IsNullOrEmpty(data.TankVolume))
            {
                TankVolume = float.Parse(data.TankVolume);
            }

            //tractament volume
            if (!string.IsNullOrEmpty(data.TankLevel))
            {
                TankLevel = Convert.ToInt32(data.TankLevel);
            }

            //tracatament amount
            if (!string.IsNullOrEmpty(data.Amount))
            {
                Amount = Convert.ToInt32(float.Parse(data.Amount));
            }

            //insertamos el pedido
            NewOrderID = Convert.ToInt32(adapt.InsertOrder(
                         data.Reference,
                         data.OperatorId,
                         clientId,
                         data.StartDate,
                         data.FinalDate,
                         data.Address,
                         data.City,
                         data.PostCode,
                         provinceId,
                         countryId,
                         float.Parse(data.Latitude),
                         float.Parse(data.Longitude),
                         factoryId,
                         productId,
                         vehicleSizeId,
                         Amount,
                         data.TankNum,
                         TankVolume,
                         TankLevel,
                         data.Description,
                         data.Observations
                         ));


            //creamos el viaje que se asignara al pedido
            ImportDataSet ImportDataSet = new ImportDataSet();
            ImportDataSetTableAdapters.TripsTableAdapter tripstable = new ImportDataSetTableAdapters.TripsTableAdapter();
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
            //cantidad cargada del pedido /// 
            rowTrip.LoadedAmount = Convert.ToInt32(data.Cantidad);
            //fecha de carga
            rowTrip.LoadedDate = DateTime.Now;
            //agregamos el id del pedido
            rowTrip.Id_Order = NewOrderID;
            //estado del viaje (pendiente incialmente)
            rowTrip.Status = 1;
            ImportDataSet.Trips.AddTripsRow(rowTrip);
            //actualizamos la tabla trips en el dataset  (con esto insertamos un nuevo viaje) 
            tripstable.Update(ImportDataSet.Trips);

            //asignamos el identificador del viaje 
            return NewOrderID;
        }

        


        private void UpdateOrder(ImportDataSetTableAdapters.ImportOrdersTableAdapter adapt, ImportData data)
        {
            int operatorId = data.OperatorId;
            int? orderId = Convert.ToInt32(adapt.GetIdByReference(data.Reference, operatorId));
            
            if(orderId != null)
            {

                int clientId = ImportRequester.GetClientId(operatorId, data.ClientCode) ?? ImportRequester.CreateNewClient(data) ?? throw new ClientNotFoundException();
                int factoryId = ImportRequester.GetFactoryId(operatorId, data.FactoryCode) ?? throw new FactoryNotFoundException();
                int productId = ImportRequester.GetProductId(operatorId, data.ProductCode) ?? throw new ProductNotFoundException();
                int vehicleSizeId = ImportRequester.GetVehicleSizeId(data.VehicleSizeCode) ?? throw new Exception("El código de vehiculo no es válido");
                string provinceId = "00";   // HACK Provincia hardcodeada
                string countryId = "es";    // HACK Pais hardcodeado

                adapt.UpdateOrder(
                    orderId,
                    data.Reference,
                    data.OperatorId,
                    clientId,
                    data.StartDate,
                    data.FinalDate,
                    data.Address,
                    data.City,
                    data.PostCode,
                    provinceId,
                    countryId,
                    float.Parse(data.Latitude),
                    float.Parse(data.Longitude),
                    factoryId,
                    productId,
                    vehicleSizeId,
                    Convert.ToInt32(float.Parse(data.Amount)),
                    data.TankNum,
                    float.Parse(data.TankVolume),
                    Convert.ToInt32(data.TankLevel),
                    data.Description,
                    data.Observations
                    );
            }

        }
        

        // HACK Configuración semihardcodeada
        private ImportConfiguration LoadConfig()
        {
            ImportConfiguration config = null;
            switch (SelectedOperator.Code)
            {
                // GALP
                case "7777":
                    config = Properties.Settings.Default.ImportGalpConfig;
                    if (config is null)
                    {
                        config = GenerateDefaultConfig();
                        Properties.Settings.Default.ImportGalpConfig = config;
                        Properties.Settings.Default.Save();
                    }
                    
                    break;

                // VITOGAS
                case "1000":
                    config = Properties.Settings.Default.ImportVitogasConfig;
                    if (config is null)
                    {
                        config = GenerateDefaultConfig();
                        Properties.Settings.Default.ImportVitogasConfig = config;
                        Properties.Settings.Default.Save();
                    }
                    break;

                // CEPSA
                case "0034":
                    config = Properties.Settings.Default.ImportCepsaConfig;
                    if (config is null)
                    {
                        config = GenerateDefaultConfig();
                        Properties.Settings.Default.ImportCepsaConfig = config;
                        Properties.Settings.Default.Save();
                    }
                    break;

                default:
                    config = GenerateDefaultConfig();

                    break;
            }
            return config;
        }
        private ImportConfiguration GenerateDefaultConfig()
        {
            ImportConfiguration config = null;
            switch (SelectedOperator.Code)
            {
                // GALP
                case "7777":
                    config = Properties.Settings.Default.ImportGalpConfig;
                    if (config is null)
                    {
                        config = new ImportConfiguration()
                        {
                            FileExtension = ".xlsx",
                            IgnoreFirstRow = false,
                            Delimiter = ';',
                            OrderExpiration = 5,
                            Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Importaciones\\Galp",
                        };
                        config.ProcessedFolder = config.Folder + "\\Procesados";

                        new DirectoryInfo(config.Folder).Create();
                        new DirectoryInfo(config.ProcessedFolder).Create();

                        Properties.Settings.Default.ImportGalpConfig = config;
                        Properties.Settings.Default.Save();
                    }

                    break;

                // VITOGAS
                case "1000":
                    config = Properties.Settings.Default.ImportVitogasConfig;
                    if (config is null)
                    {
                        config = new ImportConfiguration()
                        {
                            FileExtension = ".csv",
                            IgnoreFirstRow = false,
                            Delimiter = ';',
                            OrderExpiration = 3,
                            Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Importaciones\\Vitogas",
                        };
                        config.ProcessedFolder = config.Folder + "\\Procesados";

                        new DirectoryInfo(config.Folder).Create();
                        new DirectoryInfo(config.ProcessedFolder).Create();

                        Properties.Settings.Default.ImportVitogasConfig = config;
                        Properties.Settings.Default.Save();
                    }
                    break;

                // CEPSA
                case "0034":
                    config = Properties.Settings.Default.ImportCepsaConfig;
                    if (config is null)
                    {
                        config = new ImportConfiguration()
                        {
                            FileExtension = ".dat",
                            IgnoreFirstRow = false,
                            Delimiter = ';',
                            OrderExpiration = 3,
                            Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Importaciones\\Cepsa",
                        };
                        config.ProcessedFolder = config.Folder + "\\Procesados";

                        new DirectoryInfo(config.Folder).Create();
                        new DirectoryInfo(config.ProcessedFolder).Create();

                        Properties.Settings.Default.ImportCepsaConfig = config;
                        Properties.Settings.Default.Save();
                    }
                    break;

                default:
                    config = new ImportConfiguration()
                    {
                        FileExtension = ".csv",
                        IgnoreFirstRow = false,
                        Delimiter = ';',
                        OrderExpiration = 3,
                        Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Importaciones"
                    };
                    config.ProcessedFolder = config.Folder + "\\Procesados";

                    new DirectoryInfo(config.Folder).Create();
                    new DirectoryInfo(config.ProcessedFolder).Create();

                    break;
            }
            return config;
        }



    }
}
