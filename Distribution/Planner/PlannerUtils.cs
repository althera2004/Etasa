using EtasaDesktop.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Planner
{
    public class PlannerUtils
    {
        public static bool IsLastDay(Order order, DateTime date)
        {
            return order.FinalDate == date;
        }

        public static bool IsExpired(Order order, DateTime date)
        {
            return order.FinalDate < date;
        }
    }
}
