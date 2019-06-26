using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Orders.Imports
{
    [Serializable]
    public class ImportConfiguration
    {
        public int OperatorId { get; set; }
        public string FileExtension { get; set; }
        public bool IgnoreFirstRow { get; set; }
        public char Delimiter { get; set; }
        public int OrderExpiration { get; set; }
        public string Folder { get; set; }
        public string ProcessedFolder { get; set; }

        //public List<ImportColumnConfiguration> Columns { get; set; }
        public ImportConfiguration() { }

            public ImportConfiguration(ImportConfiguration config = null)
        {
            if(config != null)
            {
                OperatorId = config.OperatorId;
                FileExtension = config.FileExtension;
                IgnoreFirstRow = config.IgnoreFirstRow;
                Delimiter = config.Delimiter;
                OrderExpiration = config.OrderExpiration;
                Folder = config.Folder;
                ProcessedFolder = config.ProcessedFolder;
            }

            //Columns = new List<ImportColumnConfiguration>();
        }

        public class Column
        {
            public const string StartDateColumn = "StartDate";
            public const string FactoryCodeColumn = "FactoryCode";
            public const string ReferenceColumn = "Reference";
            public const string ClientCodeColumn = "ClientCode";
            public const string ClientNameColumn = "ClientName";
            public const string ClientCifColumn = "ClientCif";
            public const string ContactColumn = "Contact";
            public const string CityColumn = "City";
            public const string AddressColumn = "Address";
            public const string PostCodeColumn = "PostCode";
            public const string EmailColumn = "Email";
            public const string PhoneColumn = "Phone";
            public const string Phone2Column = "Phone2";
            public const string LatitudeColumn = "Latitude";
            public const string LongitudeColumn = "Longitude";
            public const string TankVolumeColumn = "TankVolume";
            public const string TankNumColumn = "TankNum";
            public const string TankLevelColumn = "TankLevel";
            public const string AmountColumn = "Amount";
            public const string ProductCodeColumn = "ProductCode";
            public const string MeasureUnitColumn = "MeasureUnit";
            public const string VehicleTypeColumn = "VehicleType";
            public const string ObservationsColumn = "Observation";
            public const string NotesColumn = "Notes";
        }
    }
}
