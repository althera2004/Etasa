using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EtasaDesktop.Common.Tools
{
    public abstract class SearchElementViewModel : ViewModelBase
    {
        public DataRow SelectedRow { get; private set; }
        public EnumerableRowCollection ItemsSource { get; set; }
        public object SelectedItem { get; set; }

        public SearchElementViewModel()
        {
            LoadData();
        }


        private void LoadData()
        {
            ItemsSource = Query();
        }
        public abstract EnumerableRowCollection Query();


    }
}
