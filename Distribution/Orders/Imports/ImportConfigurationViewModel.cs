using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    public class ImportConfigurationViewModel : ViewModelBase
    {
        private ImportConfiguration _config;
        public ImportConfiguration Config
        {
            get => this._config;
            set => Set(ref _config, value);
        }

        public string FileExtension
        {
            get => this.Config.FileExtension;
            set
            {
                if (Config.FileExtension != value)
                {
                    Config.FileExtension = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IgnoreFirstRow
        {
            get => this.Config.IgnoreFirstRow;
            set
            {
                if (Config.IgnoreFirstRow != value)
                {
                    Config.IgnoreFirstRow = value;
                    RaisePropertyChanged();
                }
            }
        }

        public char Delimiter
        {
            get => this.Config.Delimiter;
            set
            {
                if (Config.Delimiter != value)
                {
                    Config.Delimiter = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int OrderExpiration
        {
            get => this.Config.OrderExpiration;
            set
            {
                if (Config.OrderExpiration != value)
                {
                    Config.OrderExpiration = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Folder
        {
            get => this.Config.Folder;
            set
            {
                if (Config.Folder != value)
                {
                    Config.Folder = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ProcessedFolder
        {
            get => this.Config.ProcessedFolder;
            set
            {
                if (Config.ProcessedFolder != value)
                {
                    Config.ProcessedFolder = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<ImportColumnConfigurationViewModel> Columns;


        public ImportConfigurationViewModel()
        {
            Columns = new ObservableCollection<ImportColumnConfigurationViewModel>();
        }

    }
}
