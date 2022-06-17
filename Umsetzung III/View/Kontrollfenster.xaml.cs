using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Umsetzung_III
{
    /// <summary>
    /// Interaktionslogik für Kontrollfenster.xaml
    /// </summary>
    public partial class Kontrollfenster : Window
    {
        private readonly SpielanzeigeViewModel _viewModel;
        private Hauptanzeige _hauptanzeige;
        public Kontrollfenster()
        {
            InitializeComponent();
            _viewModel = new SpielanzeigeViewModel();
            DataContext = _viewModel;

            ShowSpielanzeige();
        }

        private void ShowSpielanzeige()
        {
            _hauptanzeige = new Hauptanzeige(_viewModel);
            _hauptanzeige.Show();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainGrid.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _hauptanzeige.Close();
        }
    }
}
