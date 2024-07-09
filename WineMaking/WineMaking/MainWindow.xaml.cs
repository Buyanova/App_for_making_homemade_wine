using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WineMaking
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Created cr = new Created();
            cr.Show();
            this.Hide();
        }

        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            Equipment eq = new Equipment();
            eq.Show();
            this.Hide();
        }

        private void LookWine_Click(object sender, RoutedEventArgs e)
        {
            AllWines aw = new AllWines();
            aw.Show();
            this.Close();
        }

        private void Recomend_Click(object sender, RoutedEventArgs e)
        {
            RecomendFood rf = new RecomendFood();
            rf.Show();
            this.Close();
        }
    }
}
