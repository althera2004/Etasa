using EtasaDesktop.Common.Tools;
using EtasaDesktop.Distribution.Assignments;
using EtasaDesktop.Distribution.Clients;
using EtasaDesktop.Distribution.Conductores;
using EtasaDesktop.Distribution.Orders.Publish;
using EtasaDesktop.Distribution.MarkerColors;
using EtasaDesktop.Distribution.Orders;
using EtasaDesktop.Distribution.Orders.Imports;
using EtasaDesktop.Distribution.Planner;
using EtasaDesktop.Distribution.Vehicles;
using EtasaDesktop.Distribution.Products;
using EtasaDesktop.Distribution.Factories;
using EtasaDesktop.Distribution.Drivers;
using EtasaDesktop.Billing;
using EtasaDesktop.Common.Auth.Users;
using EtasaDesktop.Distribution.Orders.Form;
using EtasaDesktop.Distribution.Vehicles.VehiclesNew;
using EtasaDesktop.Distribution.Operators;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EtasaDesktop.Files.Sftp;
using EtasaDesktop.Files.Json;

namespace EtasaDesktop.Common
{
    public class MainViewModel : ViewModelBase
    {
        private string _status;
        public string Status
        {
            get => _status;

            set => Set(ref _status, value);
        }


        public MainViewModel()
        {

            // TODO Ejecutar async
            //RefreshCommand = new RelayCommand(ExecuteRefresh, CanRefresh);

            // Commands: Actions
            UndoCommand = new RelayCommand(Undo, CanUndo);
            RedoCommand = new RelayCommand(Redo, CanRedo);
            RefreshCommand = new RelayCommand(Refresh, CanRefresh);
            SaveCommand = new RelayCommand(ExecuteSave, CanSave);
            ExitCommand = new RelayCommand(Exit);
            LogoutCommand = new RelayCommand(Logout);

            // Commands: Orders
            ShowPlannerCommand = new RelayCommand(ShowPlanner);
            ShowOrdersCommand = new RelayCommand(ShowOrders);
            ShowOrdersCommand2 = new RelayCommand(ShowOrders2);

            ShowOperatorsCommand = new RelayCommand(ShowOperator);
            ShowOrderDialogCommand = new RelayCommand(ShowOrderDialog);
            ShowImportOrdersCommand = new RelayCommand(ShowImportOrders);
            ShowPublishOrdersDialogCommand = new RelayCommand(ShowPublishOrdersDialog);
            ShowColorsDialogCommand = new RelayCommand(ShowColorsDialog);
            ShowBillingCommand = new RelayCommand(ShowBilling);

            // Commands: Insert
            ShowClientsCommand = new RelayCommand(ShowClients);
            ShowClientDialogCommand = new RelayCommand(ShowClientDialog);
            ShowFactoriesCommand = new RelayCommand(ShowFactories);
            ShowFactoryDialogCommand = new RelayCommand(ShowFactoryDialog);
            ShowProductsCommand = new RelayCommand(ShowProducts);
            ShowProductDialogCommand = new RelayCommand(ShowProductDialog);
            ShowDriversCommand = new RelayCommand(ShowDrivers);
            ShowDriverDialogCommand = new RelayCommand(ShowDriversDialog);
            ShowVehiclesCommand = new RelayCommand(ShowVehicles);
            ShowVehicleDialogCommand = new RelayCommand(ShowVehicleDialog);
            ShowAssignmentsCommand = new RelayCommand(ShowAssignments);


            // Commands:Files 
            ShowSftpCommand = new RelayCommand(ShowSftp);
            ShowJsonCommand = new RelayCommand(ShowJson);

            // Commands: Admin
            ShowUsersCommand = new RelayCommand(ShowUsers);
            ShowUserDialogCommand = new RelayCommand(ShowUserDialog);
            ShowGroupDialogCommand = new RelayCommand(ShowGroupDialog);


            Status = "Listo";
        }


        #region Frame control

        private FrameControl _frame;
        public FrameControl Frame
        {
            get => _frame;

            set
            {
                Set(ref _frame, value);
                if(_frame != null) _frame.Main = this;
            }
        }

        private void SetFrame(FrameControl view)
        {
            if (Frame is null || view is null || view.Tag != Frame.Tag)
                Frame = view;
        }

        #endregion


        #region Commands: Actions

        private bool _isRefreshing;
        private bool _isSaving;

        public RelayCommand UndoCommand { get; private set; }
        public RelayCommand RedoCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand ShowBillingCommand { get; private set; }

        private bool CanUndo()
        {
            return false;
        }
        private void Undo()
        {
            if (Frame != null)
            {
                Frame.Undo();
            }
        }

        private bool CanRedo()
        {
            return false;
        }
        private void Redo()
        {
            if (Frame != null)
            {
                Frame.Redo();
            }
        }

        private bool CanRefresh()
        {
            return !_isRefreshing;
        }
        private async void ExecuteRefresh()
        {
            if (_isRefreshing)
            {
                return;
            }

            _isRefreshing = true;
            RefreshCommand.RaiseCanExecuteChanged();

            await Task.Factory.StartNew(() =>
                    Refresh()
                );

            _isRefreshing = false;
            RefreshCommand.RaiseCanExecuteChanged();
        }
        private void Refresh()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                if (Frame != null)
                {
                    Frame.Refresh();
                }
            }

        }


        private bool CanSave()
        {
            return !_isSaving;
        }
        private async void ExecuteSave()
        {
            if (_isSaving)
            {
                return;
            }

            _isSaving = true;
            RefreshCommand.RaiseCanExecuteChanged();

            await Task.Factory.StartNew(() =>
                    Save()
                );

            _isSaving = false;
            RefreshCommand.RaiseCanExecuteChanged();
        }
        private void Save()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                if (Frame != null)
                {
                    Frame.Save();
                }
            }

        }


        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void Logout()
        {
            Properties.Settings.Default.Session = null;
            Properties.Settings.Default.Save();

            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }

        private void ShowBilling()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new BillingFrame() { Tag = "Billing" });
            }
        }

        #endregion


        #region Commands: Orders

        public RelayCommand ShowPlannerCommand { get; private set; }
        public RelayCommand ShowOrdersCommand { get; private set; }
        public RelayCommand ShowOrdersCommand2 { get; private set; }
        public RelayCommand ShowOperatorsCommand { get; private set; }
        public RelayCommand ShowOrderDialogCommand { get; private set; }
        public RelayCommand ShowImportOrdersCommand { get; private set; }
        public RelayCommand ShowPublishOrdersDialogCommand { get; private set; }
        public RelayCommand ShowColorsDialogCommand { get; private set; }

        private void ShowPlanner()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new PlannerFrame() { Tag = "Planner" });
            }
        }

        private void ShowOrders()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new OrdersListFrame() { Tag = "OrderList" });
            }
        }

        private void ShowOrders2()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new OrderFrame() { Tag = "OrderFrame" });
            }
        }


        private void ShowOperator()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new OperatorFrame() { Tag = "OperatorFrame" });
            }
        }

        private void ShowOrderDialog()
        {
           OrderFormWindow orderWindow = new OrderFormWindow();
           orderWindow.ShowDialog();
        }

        private void ShowImportOrders()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new ImportOrdersFrame() { Tag = "OrderImport" });
            }
        }

        private void ShowPublishOrdersDialog()
        {
            PublishWindow publish = new PublishWindow();
            publish.ShowDialog();
        }

        private void ShowColorsDialog()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new ColorFrame() { Tag = "ColorFrame" });
            }
            //ColorWindow colorWindow = new ColorWindow();
            //colorWindow.ShowDialog();
        }

        #endregion

        #region Commands: Files

        public RelayCommand ShowSftpCommand { get; private set; }
        public RelayCommand ShowJsonCommand { get; private set; }


        private void ShowSftp()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new SftpFrame() { Tag = "Sftp" });
            }
        }

        private void ShowJson()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new JsonFrame() { Tag = "Json" });
            }
        }
        #endregion


        #region Commands: Insert

        public RelayCommand ShowClientsCommand { get; private set; }
        public RelayCommand ShowClientDialogCommand { get; private set; }
        public RelayCommand ShowFactoriesCommand { get; private set; }
        public RelayCommand ShowFactoryDialogCommand { get; private set; }
        public RelayCommand ShowProductsCommand { get; private set; }
        public RelayCommand ShowProductDialogCommand { get; private set; }
        public RelayCommand ShowDriversCommand { get; private set; }
        public RelayCommand ShowDriverDialogCommand { get; private set; }
        public RelayCommand ShowVehiclesCommand { get; private set; }
        public RelayCommand ShowVehicleDialogCommand { get; private set; }
        public RelayCommand ShowAssignmentsCommand { get; private set; }

        private void ShowClients()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new ClientFrame() { Tag = "Clients" });
            }
        }
        private void ShowClientDialog()
        {
            ClientFormWindow clientWindow = new ClientFormWindow();
            clientWindow.ShowDialog();
        }

        private void ShowFactories()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new FactoryFrame() { Tag = "Factories" });
            }
        }
        private void ShowFactoryDialog()
        {
            FactoryFormWindow FactoryWindow = new FactoryFormWindow();
            FactoryWindow.ShowDialog();
        }

        private void ShowProducts()
        {
            
            
            
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                SetFrame(new ProductFrame { Tag = "Products" });
            }
            
         
           /*
           using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
           {
               SetFrame(new OrderFrame { Tag = "Orders" });
           }
           */



        }
        private void ShowProductDialog()
        {
            
            ProductFormWindow ProductWindow = new ProductFormWindow();
            ProductWindow.ShowDialog();
            
            /*
            OrderFormWindow OrderWindow = new OrderFormWindow();
            OrderWindow.ShowDialog();    
            */
        }

        private void ShowDrivers()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
              SetFrame(new DriverFrame { Tag = "Drivers" });
            }
        }
        private void ShowDriversDialog()
        {
            DriverFormWindow drivers = new DriverFormWindow();
            drivers.ShowDialog();
        }

        private void ShowVehicles()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
              SetFrame(new Vehiclesframe{ Tag = "Vehicles" });
            }
        }
        private void ShowVehicleDialog()
        {
            VehicleFormWindow vehicle = new VehicleFormWindow();
            vehicle.ShowDialog();
        }

        private void ShowAssignments()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
              SetFrame(new AssignmentsFrame() { Tag = "Assignments" });
            }
        }
        #endregion


        #region Commands: Admin

        public RelayCommand ShowUsersCommand { get; private set; }
        public RelayCommand ShowUserDialogCommand { get; private set; }
        public RelayCommand ShowGroupDialogCommand { get; private set; }

        private void ShowUsers()
        {
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
              SetFrame(new UserFrame() { Tag = "Users" });
            }
        }
        private void ShowUserDialog()
        {

            UserFormWindow UserWindow = new UserFormWindow();
            UserWindow.ShowDialog();
        }

        private void ShowGroupDialog()
        {

        }
    #endregion
    }
}
 