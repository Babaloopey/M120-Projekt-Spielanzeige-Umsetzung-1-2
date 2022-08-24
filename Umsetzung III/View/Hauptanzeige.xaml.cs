using System;
using System.Collections.Generic;
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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class Hauptanzeige : Window
    {
        public Hauptanzeige(SpielanzeigeViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SetWindowToFullScreen(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {

            this.WindowStyle = WindowStyle.None;

            if (this.WindowState == WindowState.Maximized)
            {
                SetWindowToTrueFullScreen();
            }
            else if(this.WindowState == WindowState.Normal)
            {
                SetWindowToNormalScreen();
            }
        }

        private void SetWindowToTrueFullScreen()
        {
            this.Visibility = Visibility.Collapsed;
            this.Topmost = true;
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.Visibility = Visibility.Visible;
        }

        private void SetWindowToNormalScreen()
        {
            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
        }
    }
}
