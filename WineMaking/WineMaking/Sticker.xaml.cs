using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Sticker.xaml
    /// </summary>
    public partial class Sticker : Window
    {
        string connectionString;
        SqlConnection cnn;
        public Sticker()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["SQLServerConnection"].ConnectionString;
            cnn = new SqlConnection(connectionString);
            cnn.Open();
        }

      
        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand cmd = cnn.CreateCommand();

                cmd = cnn.CreateCommand();

                // Заполнение данными 
                cmd.CommandText = "Insert into Sticker(Name, Year)" +
                "values (@name, @year)";
                cmd.Parameters.AddWithValue("@name", Name.Text);
                cmd.Parameters.AddWithValue("@year", Convert.ToInt32(Year.Text));

                cmd.ExecuteNonQuery();

                cnn.Close();

                // Печать

                string sticker = " " + Name.Text + "\n     " + Year.Text + " год";

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Создать текст
                    Run run = new Run(sticker);

                    // Поместить его в TextBlock
                    TextBlock visual = new TextBlock();
                    visual.Inlines.Add(run);

                    // Использовать поля для получения рамки страницы
                    visual.Margin = new Thickness(10);

                    // Разрешить перенос для заполнения всей ширины страницы
                    visual.TextWrapping = TextWrapping.Wrap;

                    // Увеличить TextBlock по обоим измерениям в 5 раз. 
                    // (В этом случае увеличение шрифта дало бы тот же эффект, 
                    // потому что TextBlock — единственный элемент)
                    visual.LayoutTransform = new ScaleTransform(2, 2);

                    // Установить размер элемента
                    Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                    visual.Measure(pageSize);
                    visual.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

                    // Напечатать элемент
                    printDialog.PrintVisual(visual, "Этикетка");
                }
            }
           catch
            {
                MessageBox.Show("Возникла ошибка!");
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
