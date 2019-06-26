using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    public class ImportColumnConfigurationViewModel : ViewModelBase
    {
        private ImportColumnConfiguration _columnConfig;
        public ImportColumnConfiguration ColumnConfig
        {
            get => this._columnConfig;
            set => Set(ref _columnConfig, value);
        }

        public string ColumnName
        {
            get => ColumnConfig.ColumnName;
            set
            {
                if (ColumnConfig.ColumnName != value)
                {
                    ColumnConfig.ColumnName = value;
                    RaisePropertyChanged();
                }
            }
        }
        private int[] _columnNum;
        public int[] ColumnNum
        {
            get => ColumnConfig.ColumnNum;
            set
            {
                if (ColumnConfig.ColumnNum != value)
                {
                    ColumnConfig.ColumnNum = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string[] ColumnLetter
        {
            get => ColumnConfig.ColumnLetter;
            set
            {
                if (ColumnConfig.ColumnLetter != value)
                {
                    ColumnConfig.ColumnLetter = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DefaultValue
        {
            get => ColumnConfig.DefaultValue;
            set
            {
                if (ColumnConfig.DefaultValue != value)
                {
                    ColumnConfig.DefaultValue = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Format
        {
            get => ColumnConfig.Format;
            set
            {
                if (ColumnConfig.Format != value)
                {
                    ColumnConfig.Format = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string GetColumnLetter(int columnNumber)
        {
            int dividend = columnNumber;
            string columnLetter = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnLetter = Convert.ToChar(65 + modulo).ToString() + columnLetter;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnLetter;
        }
    }
}
