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

namespace EtasaDesktop.Billing
{

    /// <summary>
    /// Lógica de interacción para AssignamentsView.xaml
    /// </summary>
    public partial class BillingFrame : FrameControl
    {
        private BillingViewModel _viewModel;

        BillingDataSet.OrdersSummariesDataTable dataTable = new BillingDataSet.OrdersSummariesDataTable();


        public BillingFrame()
        {
             InitializeComponent();
            _viewModel = (BillingViewModel)DataContext;
        }

        public override void Refresh()
        {
            Main.Status = "Refrescando Pedidos...";
            using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
            {
                _viewModel.Refresh();
            }
            Main.Status = "Listo";
        }
        //comparar pedidos
        private void Compare_Click(object sender, RoutedEventArgs e)
        {
            DateTime? StartDate = dpDateStart.SelectedDate;
            DateTime? FinalDate = dpDateFinal.SelectedDate;
            dataTable = _viewModel.Filter(StartDate, FinalDate, CodeOperator.Text, Reference.Text, dataTable);
            BillingFormWindow BillingWindowWindow = new BillingFormWindow(dataTable);
            BillingWindowWindow.ShowDialog();     
        }
        //filtrar pedido
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            DateTime? StartDate = dpDateStart.SelectedDate;
            DateTime? FinalDate = dpDateFinal.SelectedDate;
            //metodoes para filtrar los datos (fecha inicio, fecha fin, operdor, referencia)
            _viewModel.Filter(StartDate, FinalDate, CodeOperator.Text, Reference.Text, dataTable);
        }   
    }
}
