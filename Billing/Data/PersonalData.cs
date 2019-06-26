using System.ComponentModel;

namespace EtasaDesktop.Models.BillingModel
{
    class PersonalData : INotifyPropertyChanged
    {
        private string NIF;
        private string pais;
        private string provincia;
        private string poblacion;
        private string direccion;
        private int codigoPostal;
        private int telefono;
        private string fax;
        private string email;
        private string emailCobro;
        private string contactPerson;

        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        public PersonalData()
        {
        }

        public PersonalData(string NIF, string pais, string provincia, string poblacion, string direccion, int codigoPostal, int telefono, string fax, string email, string emailCobro)
        {
            this.NIF = NIF;
            this.pais = pais;
            this.provincia = provincia;
            this.poblacion = poblacion;
            this.direccion = direccion;
            this.codigoPostal = codigoPostal;
            this.telefono = telefono;
            this.fax = fax;
            this.email = email;
            this.emailCobro = emailCobro;
        }

        public string PersonalDataNIF
        {
            get { return NIF; }
            set
            {
                NIF = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("PersonalDataNIF");
            }
        }

        public string PersonalDataPais
        {
            get { return pais; }
            set
            {
                pais = value;
                OnPropertyChanged("PersonalDataPais");
            }
        }

        public string PersonalDataProvincia
        {
            get { return provincia; }
            set
            {
                provincia = value;
                OnPropertyChanged("PersonalDataProvincia");
            }
        }

        public string PersonalDataPoblacion
        {
            get { return poblacion; }
            set
            {
                poblacion = value;
                OnPropertyChanged("PersonalDataPoblacion");
            }
        }

        public string PersonalDataDireccion
        {
            get { return direccion; }
            set
            {
                direccion = value;
                OnPropertyChanged("PersonalDataDireccion");
            }
        }

        public int PersonalDataCodigoPostal
        {
            get { return codigoPostal; }
            set
            {
                codigoPostal = value;
                OnPropertyChanged("PersonalDataCodigoPostal");
            }
        }

        public int PersonalDataTelefono
        {
            get { return telefono; }
            set
            {
                telefono = value;
                OnPropertyChanged("PersonalDataTelefono");
            }
        }

        public string PersonalDataFax
        {
            get { return fax; }
            set
            {
                fax = value;
                OnPropertyChanged("PersonalDataFax");
            }
        }

        public string PersonalDataEmail
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("PersonalDataEmail");
            }
        }

        public string PersonalDataEmailCobro
        {
            get { return emailCobro; }
            set
            {
                emailCobro = value;
                OnPropertyChanged("PersonalDataEmailCobro");
            }
        }

        public string PersonalDataContactPerson
        {
            get { return contactPerson; }
            set
            {
                contactPerson = value;
                OnPropertyChanged("PersonalDataContactPerson");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
