using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EtasaDesktop.Distribution.Orders
{
    public class OrderCommands
    {
        private static RoutedCommand orderList = new RoutedCommand();
        private static RoutedCommand orderNew = new RoutedCommand();
        private static RoutedCommand orderEdit = new RoutedCommand();
        private static RoutedCommand orderDelete = new RoutedCommand();
        private static RoutedCommand orderImport = new RoutedCommand();
        private static RoutedCommand orderQuery = new RoutedCommand();

        public static RoutedCommand List => orderList;
        public static RoutedCommand New => orderNew;
        public static RoutedCommand Edit => orderEdit;
        public static RoutedCommand Delete => orderDelete;
        public static RoutedCommand Import => orderImport;
        public static RoutedCommand Query => orderQuery;
    }
}
