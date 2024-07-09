using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для Created.xaml
    /// </summary>
    public partial class Created : Window
    {
        string connectionString;
        SqlConnection cnn;
        public Created()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnection"].ConnectionString;
            cnn = new SqlConnection(connectionString);
            cnn.Open();

            // Вывод вид культур
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = @"select Name from Culture"; // запрос
            SqlDataReader r = cmd.ExecuteReader();
            Culture.Items.Clear();
            while (r.Read())
                Culture.Items.Add(String.Format("{0}", r.GetValue(0).ToString()));
            r.Close();
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // ОБработка исключения на отрицательное или знаыение 0 при вводе сока
                if (Convert.ToDouble(Juice.Text) <= 0)
                    MessageBox.Show("Введены некорректные данные!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    // Вода2 = сок*вода1
                    SqlCommand cmd = cnn.CreateCommand();

                    cmd.CommandText = "select Water*@wj from Culture where Name ='" + Culture.SelectedItem.ToString() + "'"; // запрос
                    cmd.Parameters.AddWithValue("@wj", Convert.ToDouble(Juice.Text));

                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                        Water.Content = r.GetValue(0).ToString();
                    r.Close();


                    // Сахар2 = сок*сахар1
                    cmd.CommandText = "select Sugar*@sj from Culture where Name ='" + Culture.SelectedItem.ToString() + "'"; // запрос
                    cmd.Parameters.AddWithValue("@sj", Convert.ToDouble(Juice.Text));

                    r = cmd.ExecuteReader();
                    if (r.Read())
                        Sugar.Content = r.GetValue(0).ToString();
                    r.Close();

                    cmd = cnn.CreateCommand();

                    // Прибавление к дате 6 дней 
                    DateTime? dt = ChooseDate.SelectedDate; // присваиваем переменной выбранную дату
                    DateTime? fived = dt.Value.AddDays(6);

                    // Подсчёт сахара после 6 дней
                    double sugarfive = 0; // переменная для записи результата

                    cmd.CommandText = "select FiveDay*@fj from Culture where Name ='" + Culture.SelectedItem.ToString() + "'"; // запрос
                    cmd.Parameters.AddWithValue("@fj", Convert.ToDouble(Juice.Text));

                    r = cmd.ExecuteReader();
                    if (r.Read())
                        sugarfive = Convert.ToDouble(r.GetValue(0)); // присваивание результата
                    r.Close();

                    // Заполнение данными 
                    cmd.CommandText = "INSERT INTO Wine" +
                    "(Date,Name,Juice,DateFive,FiveSugar)" +
                    "values (@d, @n, @j, @fd, @fs)";
                    cmd.Parameters.AddWithValue("@d", ChooseDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@n", Culture.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@j", Convert.ToDouble(Juice.Text));
                    cmd.Parameters.AddWithValue("@fd", fived.Value.ToShortDateString());
                    cmd.Parameters.AddWithValue("@fs", sugarfive);

                    cmd.ExecuteNonQuery();

                    cnn.Close();

                }

            }
            catch
            {
                MessageBox.Show("Данные не введены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
