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
        private ViewModel viewModel;
        private TimerStore timerStore;
        public Kontrollfenster()
        {
            InitializeComponent();
            viewModel = new ViewModel();
            DataContext = viewModel;

            ShowSpielanzeige();

        }

        private void ShowSpielanzeige()
        {
            MainWindow spielanzeige = new MainWindow(viewModel);
            spielanzeige.Show();
            spielanzeige.WindowState = WindowState.Maximized;
            spielanzeige.WindowStyle = WindowStyle.SingleBorderWindow;

        }

    }
}
