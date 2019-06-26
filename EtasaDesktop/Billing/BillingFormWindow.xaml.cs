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
using EtasaDesktop.Common.Tools;
using System.Collections.ObjectModel;

namespace EtasaDesktop.Billing
{
    /// <summary>
    /// Lógica de interacción para UserFormViewModel.xaml
    /// </summary>
    public partial class BillingFormWindow : Window
    {
        private BillingFormViewModel _viewModel;

        public BillingFormWindow(BillingDataSet.OrdersSummariesDataTable dataTable)
        {
            _viewModel = new BillingFormViewModel();
            DataContext = _viewModel;
            
            InitializeComponent();     
            _viewModel.Load(DatagridOrder, DatagridOrderCepsa, dataTable, MasDatosCepsa);     
       
        }        
    }
}
