using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EtasaDesktop.Distribution.Planner
{
    /// <summary>
    /// Lógica de interacción para UserFormViewModel.xaml
    /// </summary>
    public partial class ColorFormWindow : Window
    {
        private ColorFormViewModel _viewModel;
    
        public ColorFormWindow(int ColorId = 0)
        {
            _viewModel = new ColorFormViewModel();
            DataContext = _viewModel;
            InitializeComponent();

            _viewModel.FormLoadError += FormLoadError_Event;
            _viewModel.FormSaveFinished += FormSaveFinished_Event;
            _viewModel.FormSaveError += FormSaveError_Event;
            _viewModel.FormRequiredEmpty += FormRequiredEmpty_Event;

            if (ColorId > 0)
            {

                Title.Content = "Editar Colores";
                //inicializamos las barrras con el color 
              

                _viewModel.Load(ColorId, textbox1, textbox2, textbox3, textbox4, textbox5, textbox6, ClrPcker_Background, ClrPcker_Background1, ClrPcker_Background2, ClrPcker_Background3, ClrPcker_Background4, ClrPcker_Background5);      
            }
            else
            {
                Title.Content = "Nuevo Color";         
            }
        }

        private bool validateDatas(string FactoryColor, string ClientColor, string UrgentColor, string FinalDayColor, string PreferenceColor, string TheLastPreferent)
        {


            return true;
        }

        private void FormLoadError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido cargar los colores", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        private void FormRequiredEmpty_Event()
        {
            MessageBoxResult result = MessageBox.Show("Hay campos obligatorios sin rellenar",
                          "Confirmation",
                          MessageBoxButton.OK,
                          MessageBoxImage.Warning);
        }
        private void FormSaveFinished_Event()
        {
            DialogResult = true;
            Close();
        }
        private void FormSaveError_Event(Exception exception)
        {
            MessageBox.Show("No se ha podido guardar el conductor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, EventArgs e)
        {
            textbox1.Text = "";
            textbox1.Text = ClrPcker_Background.SelectedColor.Value.ToString();
            ClrPcker_Background.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox1.Text.ToString().Trim()));
            textbox1.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox1.Text.ToString().Trim()));
        }


        private void ClrPcker_Background_SelectedColorChanged1(object sender, EventArgs e)
        {
            textbox2.Text = "";
            textbox2.Text = ClrPcker_Background1.SelectedColor.Value.ToString();
            ClrPcker_Background1.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox2.Text.ToString().Trim()));
            textbox2.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox2.Text.ToString().Trim()));
        }

        private void ClrPcker_Background_SelectedColorChanged2(object sender, EventArgs e)
        {
            textbox3.Text = "";
            textbox3.Text = ClrPcker_Background2.SelectedColor.Value.ToString();
            ClrPcker_Background2.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox3.Text.ToString().Trim()));
            textbox3.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox3.Text.ToString().Trim()));
        }

        private void ClrPcker_Background_SelectedColorChanged3(object sender, EventArgs e)
        {
            textbox4.Text = "";
            textbox4.Text = ClrPcker_Background3.SelectedColor.Value.ToString();
            ClrPcker_Background3.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox4.Text.ToString().Trim()));
            textbox4.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox4.Text.ToString().Trim()));
        }


        private void ClrPcker_Background_SelectedColorChanged4(object sender, EventArgs e)
        {
            textbox5.Text = "";
            textbox5.Text = ClrPcker_Background4.SelectedColor.Value.ToString();
            ClrPcker_Background4.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox5.Text.ToString().Trim()));
            textbox5.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox5.Text.ToString().Trim()));
        }

        private void ClrPcker_Background_SelectedColorChanged5(object sender, EventArgs e)
        {
            textbox6.Text = "";
            textbox6.Text = ClrPcker_Background5.SelectedColor.Value.ToString();   
            ClrPcker_Background5.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox6.Text.Trim()));
            textbox6.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(textbox6.Text.ToString().Trim()));
        }

    }
}
