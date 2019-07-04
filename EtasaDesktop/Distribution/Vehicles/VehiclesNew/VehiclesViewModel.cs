namespace EtasaDesktop.Distribution.Vehicles.VehiclesNew
{
    using System.Collections.ObjectModel;
    using GalaSoft.MvvmLight;

    class VehiclesViewModel : ViewModelBase
    {
        private VehicleDataSet1.VehiclesSummariesRow _selectedVehicle;


        public VehicleDataSet1.VehiclesSummariesRow SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                Set(ref _selectedVehicle, value);
            }
        }


        public ObservableCollection<VehicleDataSet1.VehiclesSummariesRow> Vehicles { get; private set; }

        public VehiclesViewModel()
        {
            Vehicles = new ObservableCollection<VehicleDataSet1.VehiclesSummariesRow>();
            Refresh();
        }

        public void Refresh()
        {
            Vehicles.Clear();
            VehicleDataSet1 ds = new VehicleDataSet1();
            VehicleDataSet1TableAdapters.VehiclesSummariesTableAdapter adapt = new VehicleDataSet1TableAdapters.VehiclesSummariesTableAdapter();
            adapt.Fill(ds.VehiclesSummaries);
            foreach (VehicleDataSet1.VehiclesSummariesRow row in ds.VehiclesSummaries.Rows)
            {
                Vehicles.Add(row);
            }
        }
    }
}