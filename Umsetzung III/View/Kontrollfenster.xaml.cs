﻿using System;
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
        MainWindow spielanzeige;
        public Kontrollfenster()
        {
            InitializeComponent();
            viewModel = new ViewModel();
            DataContext = viewModel;

            ShowSpielanzeige();
        }

        private void ShowSpielanzeige()
        {
            spielanzeige = new MainWindow(viewModel);
            spielanzeige.Show();
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainGrid.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            spielanzeige.Close();
        }
    }
}
