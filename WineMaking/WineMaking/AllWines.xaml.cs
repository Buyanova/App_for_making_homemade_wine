using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Toolkit.Uwp.Notifications;

namespace WineMaking
{
    /// <summary>
    /// Логика взаимодействия для AllWines.xaml
    /// </summary>
    public partial class AllWines : Window
    {
        SqlDataAdapter adapter;
        string connectionString;
        SqlConnection cnn;
        DataTable wineTable;

        public AllWines()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnection"].ConnectionString;
            cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand cmd = cnn.CreateCommand();

            // Вывод информации из таблицы:
            // запрос для вывода информации
            cmd.CommandText = @"select Date, Name, Juice, DateFive, FiveSugar from Wine";
            SqlDataReader r = cmd.ExecuteReader();
            adapter = new SqlDataAdapter(cmd);
            r.Close();

            wineTable = new DataTable("Wine"); // указание таблицы от куда берутся данные

            adapter.Fill(wineTable); // заполнение адаптара с данными

            WineList.ItemsSource = wineTable.DefaultView; // заполнение таблицы данными

            // Уведомление о том, что надо добавить сахар
            DateTime now = DateTime.Now; // сегодня
            DateTime date = new DateTime(); // 5 день
            int i = 0;
            foreach (System.Data.DataRowView dr in WineList.ItemsSource)
            {
                date = Convert.ToDateTime(dr[3]);
                i++;
                if (now.ToShortDateString() == date.ToShortDateString())
                {
                    var notify = new ToastContentBuilder();
                    notify.AddText("Добавьте сахар в вино, находящееся в " + i + " позиции!");

                    notify.Show();
                }
            }

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void CreateSticker_Click(object sender, RoutedEventArgs e)
        {
            Sticker sticker = new Sticker();

            object item = WineList.SelectedItem;

            string culture = (WineList.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            DateTime date = Convert.ToDateTime((WineList.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text);
            string year = date.ToString("yyyy");
            sticker.Name.Text = "Вино: " + culture;
            //WineList
            sticker.Year.Text = year;

            sticker.Show();
        }

    }
}
