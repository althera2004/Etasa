using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EtasaDesktop.Common.Tools
{
    public abstract class FrameControl : UserControl
    {
        // REVIEW No creo que sea la mejor manera
        public MainViewModel Main { get; set; }

        
        public virtual void Undo() { }

        public virtual void Redo() { }

        public virtual void Refresh() { }

        public virtual void Save() { }
    }
}
