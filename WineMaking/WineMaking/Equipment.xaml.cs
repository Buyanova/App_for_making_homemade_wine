using System;
using System.Collections.Generic;
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
using System.Configuration;

namespace WineMaking
{
    /// <summary>
    /// Логика взаимодействия для Equipment.xaml
    /// </summary>
    public partial class Equipment : Window
    {
        SqlDataAdapter adapter;
        string connectionString;
        SqlConnection cnn;
        DataTable equipmentTable;
        public Equipment()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnection"].ConnectionString;
            cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand cmd = cnn.CreateCommand();

            // Вывод информации из таблицы:
            // запрос для вывода информации
            cmd.CommandText = @"select Name, Description from Equipment";
            SqlDataReader r = cmd.ExecuteReader();
            adapter = new SqlDataAdapter(cmd);
            r.Close();

            equipmentTable = new DataTable("Equipment"); // указание таблицы от куда берутся данные

            adapter.Fill(equipmentTable); // заполнение адаптара с данными

            EquipmentsList.ItemsSource = equipmentTable.DefaultView; // заполнение таблицы данными
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
