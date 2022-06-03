using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace M120_226B_Projekt_Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            ShowSpielanzeige();
        }

        private void ShowSpielanzeige()
        {
            MainWindow spielanzeige = new MainWindow();
            spielanzeige.Show();
            spielanzeige.WindowState = WindowState.Maximized;
            spielanzeige.WindowStyle = WindowStyle.SingleBorderWindow;
           
        }
        
    }
}
