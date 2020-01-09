using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Threading;

namespace SaftLive
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dt = new DispatcherTimer();
            dt.Tick += Dt_Tick;
            dt.Start();
        }

        private int mru = int.MinValue;

        private void Dt_Tick(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection("server=saftsqlserver.database.windows.net;database=mysqldatabase;user id=student;password=Pa$$w0rd"))
            {
                connection.Open();
                using (var command = new SqlCommand("select messageid, value from messages where messageid > @mru order by messageid desc", connection))
                {
                    command.Parameters.AddWithValue("@mru", mru);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mru = reader.GetInt32(0);
                            var value = reader.GetString(1);
                            var output = DateTime.Now.ToShortTimeString() + " " + value;
                            lbMessages.Items.Add(output);
                        }
                    }
                }
            }
        }
    }
}




public class MyUtilities
{
    public static void Display(object value)
    {
        using (var connection = new SqlConnection("server=saftsqlserver.database.windows.net;database=mysqldatabase;user id=student;password=Pa$$w0rd"))
        {
            connection.Open();
            using (var command = new SqlCommand("insert messages (value) values (@value)", connection))
            {
                command.Parameters.AddWithValue("@value", value);
                command.ExecuteNonQuery();
            }
        }
    }
}