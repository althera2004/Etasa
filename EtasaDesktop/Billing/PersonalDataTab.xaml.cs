using EtasaDesktop.Models.BillingModel;
using System.Windows.Controls;

namespace EtasaDesktop.Billing
{
    /// <summary>
    /// Lógica de interacción para PersonalDataTab.xaml
    /// </summary>
    public partial class PersonalDataTab : UserControl
    {
        public PersonalDataTab()
        {
            InitializeComponent();

            //Recuperar información personal operador
            PersonalDataView.DataContext = new PersonalData()
            {
                PersonalDataNIF = "46512135F",
                PersonalDataPais = "ESPAÑA",
                PersonalDataProvincia = "MADRID",
                PersonalDataPoblacion = "MADRID",
                PersonalDataDireccion = "Paseo de la castellana, 256",
                PersonalDataCodigoPostal = 08830,
                PersonalDataTelefono = 936752234,
                PersonalDataFax = "",
                PersonalDataEmail = "ejemplo@ejemplo.com",
                PersonalDataEmailCobro = "ejemplo@ejemplo.com",
                PersonalDataContactPerson = "Sergio Herranz Del Pozo"
            };

        }
    }
}
