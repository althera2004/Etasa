using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EtasaDesktop.Distribution.Planner.Drag
{
    public class DragViewOrigin
    {
        public const string MAP_TYPE = "map";
        public const string LIST_TYPE = "list";
        public const string ASSIGN_TYPE = "assign";

        public ListView ListView { get; set; }
        public string Type { get; set; }

        public long? AssignId
        {
            get
            {
                if (Type.Equals(ASSIGN_TYPE, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        return ListView.Tag as long?;
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }

                return null;
            }
        }

        public DragViewOrigin(ListView listView, string type)
        {
            ListView = listView;
            Type = type;
        }
    }
}