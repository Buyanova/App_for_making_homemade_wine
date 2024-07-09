using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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

namespace WineMaking
{
    /// <summary>
    /// Логика взаимодействия для RecomendFood.xaml
    /// </summary>
    public partial class RecomendFood : Window
    {
        SqlDataAdapter adapter;
        string connectionString;
        SqlConnection cnn;
        DataTable foodTable;

        public RecomendFood()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnection"].ConnectionString;
            cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand cmd = cnn.CreateCommand();

            // Вывод информации из таблицы:
            // запрос для вывода информации
            cmd.CommandText = @"select Kategory, Name from Food";
            SqlDataReader r = cmd.ExecuteReader();
            adapter = new SqlDataAdapter(cmd);
            r.Close();

            foodTable = new DataTable("Food"); // указание таблицы от куда берутся данные

            adapter.Fill(foodTable); // заполнение адаптара с данными

            FoodList.ItemsSource = foodTable.DefaultView; // заполнение таблицы данными
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
