using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.MarkerColors
{
    public class FactoryColor
    {
        private string id;
        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string colorFactory;
        public string ColorFactory
        {
            get => colorFactory;
            set
            {
                colorFactory = value;
                OnPropertyChanged("ColorFactory");
            }
        }

        private string colorClient;
        public string ColorClient
        {
            get => colorClient;
            set
            {
                colorClient = value;
                OnPropertyChanged("ColorClient");
            }
        }

        private string colorUrgent;
        public string ColorUrgent
        {
            get => colorUrgent;
            set
            {
                colorUrgent = value;
                OnPropertyChanged("ColorUrgent");
            }
        }

        private string colorPreferred;
        public string ColorPreferred
        {
            get => colorPreferred;
            set
            {
                colorPreferred = value;
                OnPropertyChanged("ColorPreferred");
            }
        }

        private string colorLast;
        public string ColorLast
        {
            get => colorLast;
            set
            {
                colorLast = value;
                OnPropertyChanged("ColorLast");
            }
        }

        private string colorProgrammed;
        public string ColorProgrammed
        {
            get => colorProgrammed;
            set
            {
                colorProgrammed = value;
                OnPropertyChanged("ColorProgrammed");
            }
        }

        private string colorServer;
        public string ColorServer
        {
            get => colorServer;
            set
            {
                colorServer = value;
                OnPropertyChanged("ColorServer");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal static List<FactoryColor> CreateDummies()
        {
            return new List<FactoryColor>()
            {
                new FactoryColor()
                {
                    Id = "0011",
                    Name = "PLANTA BALAGUER",
                    ColorFactory = "#112233",
                    ColorClient = "#445566",
                    ColorUrgent = "#778899",
                    ColorPreferred = "#AABBCC",
                    ColorLast = null,
                    ColorProgrammed = "#DDEEFF",
                    ColorServer = "#123456",
                },
                new FactoryColor()
                {
                    Id = "0018",
                    Name = "PLANTA MADRID",
                    ColorFactory = "#332211",
                    ColorClient = "#665544",
                    ColorUrgent = "#998877",
                    ColorPreferred = "#CCBBAA",
                    ColorLast = null,
                    ColorProgrammed = "#FFEEDD",
                    ColorServer = "#654321",
                },
                new FactoryColor()
                {
                    Id = "0021",
                    Name = "PLANTA CUENCA",
                    ColorFactory = "#223311",
                    ColorClient = "#556644",
                    ColorUrgent = "#889977",
                    ColorPreferred = "#BBCCAA",
                    ColorLast = null,
                    ColorProgrammed = "#EEFFDD",
                    ColorServer = "#345612",
                },
                new FactoryColor()
                {
                    Id = "0026",
                    Name = "PLANTA ALBACETE",
                    ColorFactory = "#113322",
                    ColorClient = "#446655",
                    ColorUrgent = "#779988",
                    ColorPreferred = "#AACCBB",
                    ColorLast = null,
                    ColorProgrammed = "#DDFFEE",
                    ColorServer = "#125643",
                },
                new FactoryColor()
                {
                    Id = "0028",
                    Name = "PLANTA CIUDAD REAL",
                    ColorFactory = "#221133",
                    ColorClient = "#554466",
                    ColorUrgent = "#887799",
                    ColorPreferred = "#BBAACC",
                    ColorLast = null,
                    ColorProgrammed = "#EEDDFF",
                    ColorServer = "#341256",
                },
                new FactoryColor()
                {
                    Id = "0035",
                    Name = "PLANTA VALLADOLID",
                    ColorFactory = "#331122",
                    ColorClient = "#664455",
                    ColorUrgent = "#997788",
                    ColorPreferred = "#CCAABB",
                    ColorLast = null,
                    ColorProgrammed = "#FFDDEE",
                    ColorServer = "#561234",
                },

            };
        }
    }
}
