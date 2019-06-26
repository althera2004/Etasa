using EtasaDesktop.Common;
using EtasaDesktop.Common.Tools;
using EtasaDesktop.Distribution.Orders.Imports;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.ComponentModel;

namespace EtasaDesktop.Distribution.Products
{
    public class ProductFormViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        private bool _isSelected;
        public event Action FormLoadFinished;
        public event Action<Exception> FormLoadError;
        public event Action FormRequiredEmpty;
        public event Action FormSaveFinished;
        public event Action<Exception> FormSaveError;
        //private ImportOperator _selectedOperator;

        public bool FirstTime = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private ProductDataSet.ProductsRow _row;

        private ProductDataSet.Products_ObsRow _row_obs;

        public ObservableCollection<ProductFormViewModel> Product { get; private set; }

        public ProductFormViewModel()
        {
            ProductDataSet dataset = new ProductDataSet();
            ProductDataSetTableAdapters.ProductsTableAdapter Product = new ProductDataSetTableAdapters.ProductsTableAdapter();
            ProductDataSetTableAdapters.Products_ObsTableAdapter Product_obs = new ProductDataSetTableAdapters.Products_ObsTableAdapter();

            _row = dataset.Products.NewProductsRow();
            _row_obs = dataset.Products_Obs.NewProducts_ObsRow();

            //Producto 
            _row.Code= "";
            _row.Density = 0;
            _row.Name = "";
            _row.MeasureUnit = 0;
            _row.ModifiedDate = DateTime.Now;
            _row.CreatedDate = DateTime.Now;
            _row.Enabled = true;
            _row.Id = 0;
            //observaciones usuasrio
            _row_obs.Observations = "";
          
            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public string Name
        {
            get { return _row.Name; }
            set
            {
                if (_row.Name != value)
                {
                    _row.Name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Code
        {
            get { return _row.Code; }
            set
            {
                if (_row.Code != value)
                {         
                    _row.Code = value;
                    NotifyPropertyChanged("Code");
                    RaisePropertyChanged();                 
                }
            }
        }

        public decimal Density
        {
            get { return _row.Density; }
            set
            {
                if (_row.Density != value)
                {
                    _row.Density = value;
                    RaisePropertyChanged();
                }
            }
        }

        public short MeasureUnit
        {
            get { return _row.MeasureUnit; }
            set
            {
                if (_row.MeasureUnit != value)
                {
                    _row.MeasureUnit = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Observations
        {
            get { return _row_obs.Observations; }
            set
            {
                if (_row_obs.Observations != value)
                {
                    _row_obs.Observations = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool Enabled
        {
            get { return _row.Enabled; }
            set
            {
                if (_row.Enabled != value)
                {
                    _row.Enabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool CanSave()
        {
            return true;
        }

        public void NotifyPropertyChanged(string property)
        {       
            if(property == "Code")
            {
                 this.FirstTime = false;
            }      
        }

        public void Load(int IdProduct)
        {
            try
            {
                ProductDataSet dataset = new ProductDataSet();
                ProductDataSetTableAdapters.ProductsTableAdapter TableProduct = new ProductDataSetTableAdapters.ProductsTableAdapter();
                ProductDataSet.ProductsDataTable dataTable = TableProduct.GetDataProductById(IdProduct);


                if (dataTable.Rows.Count > 0)
                {
                    ProductDataSetTableAdapters.Products_ObsTableAdapter Product_obs = new ProductDataSetTableAdapters.Products_ObsTableAdapter();
                    ProductDataSet.Products_ObsDataTable dataTable_Obs = Product_obs.GetDataProductr_ObsById(IdProduct);

                    _row = (ProductDataSet.ProductsRow)dataTable.Rows[0];
                    
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Code));
                    RaisePropertyChanged(nameof(Density));
                    RaisePropertyChanged(nameof(MeasureUnit));         
                    RaisePropertyChanged(nameof(Enabled));

                    _row_obs = (ProductDataSet.Products_ObsRow)dataTable_Obs.Rows[0];
                    RaisePropertyChanged(nameof(Observations));
                    
                }
                FormLoadFinished?.Invoke();
                
            }
            catch (Exception e)
            {
                FormLoadError?.Invoke(e);
            }
        }
        public void Save()
        {
            bool validate = true;
            int indentificadorProduct = 0;
            try
            {
                ProductDataSet dataset = new ProductDataSet();
                ProductDataSetTableAdapters.Products_ObsTableAdapter tableAdapterProductObs = new ProductDataSetTableAdapters.Products_ObsTableAdapter();
                ProductDataSetTableAdapters.ProductsTableAdapter TableProduct = new ProductDataSetTableAdapters.ProductsTableAdapter();
                validate = validateDatas(_row.Code, _row.Name,this.FirstTime, dataset, TableProduct);
                        
                if (validate)
                {                            
                    ProductDataSet.ProductsDataTable dataTable = TableProduct.GetDataProductById(_row.Id);

                    //actualizamnos si existe el registro  
                    if (dataTable.Rows.Count > 0)
                    {

                        var rowproduct = dataset.Products.NewProductsRow();

                        rowproduct = dataTable[0];
                        rowproduct["Code"] = _row.Code;
                        rowproduct["Name"] = _row.Name;
                        rowproduct["CreatedDate"] = DateTime.Now;
                        rowproduct["ModifiedDate"] = DateTime.Now;
                        rowproduct["Density"] = _row.Density;
                        rowproduct["MeasureUnit"] = 1;
                        rowproduct["Enabled"] = _row.Enabled;

                        int numeroderegistros = TableProduct.Update(rowproduct);

                        //si se actualizado el user actualizamos las observaciones 
                        if (numeroderegistros > 0)
                        {
                            //actualizamos la observacion
                            tableAdapterProductObs.UpdateProductObs(_row_obs.Observations, _row.Id);
                        }
                    }
                    //insertamos el nuevo products  y las observaciones (product_obs)
                    else
                    {
                        TableProduct = new ProductDataSetTableAdapters.ProductsTableAdapter();
                        //obtenemos de la tabla product  el nuevo registro que se introducira 
                        var rowProduct = dataset.Products.NewProductsRow();

                        rowProduct.Code = _row.Code;
                        rowProduct.Name = _row.Name;
                        rowProduct.CreatedDate = DateTime.Now;
                        rowProduct.ModifiedDate = DateTime.Now;
                        rowProduct.Density = _row.Density;
                        rowProduct.MeasureUnit = 1;
                        rowProduct.Enabled = _row.Enabled;

                        //agregamos la nueva fila 
                        dataset.Products.AddProductsRow(rowProduct);
                        //actualizamos la tabla Products en el dataset 
                        TableProduct.Update(dataset.Products);

                        //al insertar el registro nuevo en la tabla User Id se actualiza 
                        indentificadorProduct = Convert.ToInt32(rowProduct.Id);

                        // a continuación insertamos el resto de datos en la tabla User_obs cepsa con el indentificador del User recient introduzido
                        tableAdapterProductObs = new ProductDataSetTableAdapters.Products_ObsTableAdapter();
                        //obtenemos de la tabla User_obs el nuevo registro que se introducira 
                        var rowProduct_obs = dataset.Products_Obs.NewProducts_ObsRow();

                        //Id
                        rowProduct_obs.Id = indentificadorProduct;
                        //observations
                        rowProduct_obs.Observations = _row_obs.Observations;
                        
                        //agregamos la nueva fila a tabla User_obs
                        dataset.Products_Obs.AddProducts_ObsRow(rowProduct_obs);
                        //actualizamos la tabla User_obs  en el dataset 
                        tableAdapterProductObs.Update(dataset.Products_Obs);
                    }

                    FormSaveFinished?.Invoke();
                }
            }
            catch (Exception e)
            {
                FormSaveError?.Invoke(e);
            }
        }

        private static bool validateDatas(string Code, string Name,bool FirstTime, ProductDataSet dataset, ProductDataSetTableAdapters.ProductsTableAdapter tableProduct)
        {
            bool validateDatas = true;
            if (string.IsNullOrEmpty(Code))
            {
                MessageBox.Show("El Código no puede estar en blanco", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                validateDatas = false;
            }
            else
            {
                if (Code.Length > 3)
                {
                    MessageBox.Show("El Código no puede tener más de 3 caracteres", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                    validateDatas = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        MessageBox.Show("El Nombre no puede estar en blanco", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                        validateDatas = false;
                    }
                    else
                    {
                        if (Name.Length > 100)
                        {
                            MessageBox.Show("El Nombre no puede contener más de 100 caracteres", "Campo Nombre", MessageBoxButton.OK, MessageBoxImage.Error);
                            validateDatas = false;
                        }
                        else
                        {
                            //si es la primera vez no realizamos la validación de codigo de productos repetidos (siempre existira)
                            if(!FirstTime)
                            {
                                int NumberRows = Convert.ToInt32(tableProduct.GetDataProductByCode(Code));

                                if (NumberRows > 0)
                                {
                                    MessageBox.Show("El Código del producto ya existe", "Campo Código", MessageBoxButton.OK, MessageBoxImage.Error);
                                    validateDatas = false;
                                }
                            }
                        }
                    }
                }
            }          
         return validateDatas;
        }
    }
}
