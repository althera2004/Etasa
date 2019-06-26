using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtasaDesktop.Distribution.Assignments
{
    public class AssignmentsImportViewModel : ViewModelBase
    {
        public RelayCommand SelectAllCommand { get; private set; }
        public RelayCommand UnselectAllCommand { get; private set; }
        public RelayCommand ImportCommand { get; private set; }

        public event EventHandler ImportStarted;
        public event EventHandler ImportFailed;
        public event EventHandler ImportFinished;

        public DateTime _selectedDate;
        public DateTime _importToDate;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                Set(ref _selectedDate, value);
                Refresh();
            }
        }
        public DateTime ImportToDate
        {
            get => _importToDate;
            set
            {
                Set(ref _importToDate, value);
            }
        }

        public ObservableCollection<AssignmentImportDataViewModel> Assignments { get; private set; }

        public AssignmentsImportViewModel()
        {
            SelectAllCommand = new RelayCommand(SelectAll, CanSelectAll);
            UnselectAllCommand = new RelayCommand(UnselectAll, CanUnselectAll);
            ImportCommand = new RelayCommand(Import, CanImport);

            Assignments = new ObservableCollection<AssignmentImportDataViewModel>();

            SelectedDate = DateTime.Today;
            ImportToDate = DateTime.Today;
        }

        public void Refresh()
        {
            Assignments.Clear();

            AssignmentsDataSet ds = new AssignmentsDataSet();
            AssignmentsDataSetTableAdapters.AssignmentSummariesAdapters adapt = new AssignmentsDataSetTableAdapters.AssignmentSummariesAdapters();
            adapt.FillBy(ds.AssignmentSummaries, SelectedDate);

            foreach (AssignmentsDataSet.AssignmentSummariesRow row in ds.AssignmentSummaries.Rows)
            {
                Assignments.Add(new AssignmentImportDataViewModel(row));
            }

            SelectAllCommand.RaiseCanExecuteChanged();
            UnselectAllCommand.RaiseCanExecuteChanged();
            ImportCommand.RaiseCanExecuteChanged();
        }

        private void SelectAll()
        {
            foreach (AssignmentImportDataViewModel data in Assignments)
            {
                data.IsSelected = true;
            }
        }
        private bool CanSelectAll()
        {
            return Assignments.Count > 0;
        }

        private void UnselectAll()
        {
            foreach (AssignmentImportDataViewModel data in Assignments)
            {
                data.IsSelected = false;
            }
        }
        private bool CanUnselectAll()
        {
            return Assignments.Count > 0;
        }

        private void Import()
        {
            AssignmentsDataSet ds = new AssignmentsDataSet();
            AssignmentsDataSetTableAdapters.AssignmentsTableAdapter adapt = new AssignmentsDataSetTableAdapters.AssignmentsTableAdapter();

            adapt.Connection.Open();
            adapt.Transaction = adapt.Connection.BeginTransaction();


            if (ImportStarted != null) ImportStarted.Invoke(null, null);


            try
            {
                foreach (AssignmentImportDataViewModel data in Assignments)
                {
                    if (data.IsSelected)
                    {
                        //duplicamos la asignación a la fecha indicada
                        adapt.Duplicate(data.Id, ImportToDate);
               
                        //creamos la ruta a la asignación duplicada
                        CreateRouteAssigmentsImport(data.Id);

                    }                    
                }
                adapt.Transaction.Commit();
                if (ImportFinished != null) ImportFinished.Invoke(null, null);
            }
            catch
            {
                adapt.Transaction.Rollback();
                if (ImportFailed != null) ImportFailed.Invoke(null, null);
            }  
        }
        private bool CanImport()
        {
            return Assignments.Count > 0;
        }

        public void CreateRouteAssigmentsImport(long idAssigment)
        {
            DSAssigmentsData dataset = new DSAssigmentsData();
            long Id = idAssigment;

            //cremos la ruta 
            DSAssigmentsData.RoutesDataTable routesTable = new DSAssigmentsData.RoutesDataTable();
            DSAssigmentsDataTableAdapters.RoutesTableAdapter adaptRoute = new DSAssigmentsDataTableAdapters.RoutesTableAdapter();

            //obtenemos de la tabla routes el nuevo registro que se introducira 
            DSAssigmentsData.RoutesRow rowrRoutes = dataset.Routes.NewRoutesRow();
            //fecha de creación
            rowrRoutes.CreatedDate = DateTime.Now;
            //fecha de modificacion
            rowrRoutes.ModifiedDate = DateTime.Now;
            //id de la nueva asignación
            rowrRoutes.AssignmentId = idAssigment;
            //agregamos la fila 
            dataset.Routes.AddRoutesRow(rowrRoutes);
            //actualizamos la tabla 
            adaptRoute.Update(dataset.Routes);
        }
    }
}
