using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp
{
    public partial class MainWindow : Window
    {

        public enum RoleEnum
        {
            Player1,
            Player2,
            Fire,
            Water,
            Empty
        }

        private void Setup()
        {
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            var row = Grid.GetRow(sender as UIElement);
            var column = Grid.GetColumn(sender as UIElement);
            Title = $"row {row} column {column}";
        }

        private void Loop(object sender, EventArgs e)
        {
        }

        private void Set(double x, double y, RoleEnum role)
        {
            Debug.WriteLine("x " + x + " y " + y);
            foreach (Control item in boardGrid.Children)
            {
                var row = Grid.GetRow(item as UIElement);
                var column = Grid.GetColumn(item as UIElement);
                if (x == column && y == row)
                {
                    switch (role)
                    {
                        case RoleEnum.Player1:
                            item.Background = Brushes.Green;
                            break;
                        case RoleEnum.Player2:
                            item.Background = Brushes.Orange;
                            break;
                        case RoleEnum.Fire:
                            item.Background = Brushes.Red;
                            break;
                        case RoleEnum.Water:
                            item.Background = Brushes.Blue;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (board[column, row])
                    {
                        case RoleEnum.Player1:
                            item.Background = Brushes.Green;
                            break;
                        case RoleEnum.Player2:
                            item.Background = Brushes.Orange;
                            break;
                        case RoleEnum.Fire:
                            item.Background = Brushes.Red;
                            break;
                        case RoleEnum.Water:
                            item.Background = Brushes.Blue;
                            break;
                        case RoleEnum.Empty:
                        default:
                            item.Background = ((row % 2 == 0 ? 0 : 1) + column) % 2 == 0 ? Brushes.Black : Brushes.Wheat;
                            break;
                    }
                }
            }
        }

        private RoleEnum[,] board;
        private const int columnCount = 20;
        private const int rowCount = 20;
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < columnCount; i++)
            {
                boardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < rowCount; i++)
            {
                boardGrid.RowDefinitions.Add(new RowDefinition());
            }
            board = new RoleEnum[columnCount, rowCount];
            for (int column = 0; column < columnCount; column++)
            {
                for (int row = 0; row < rowCount; row++)
                {
                    board[column, row] = RoleEnum.Empty;
                    var button = new Button();
                    boardGrid.Children.Add(button);
                    Grid.SetColumn(button, column);
                    Grid.SetRow(button, row);
                    Debug.WriteLine($"x{column} y{row}");
                    button.Click += Square_Click;
                }
            }
            Setup();
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            dispatcherTimer.Tick += Loop;
            dispatcherTimer.Start();
        }

    }
}
