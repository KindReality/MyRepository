using System.Windows;

namespace WpfApp
{
    public partial class EditWindow : Window
    {
        private AccountClass account;
        public EditWindow(AccountClass account)
        {
            InitializeComponent();
            DataContext = account;
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void bDebit_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as AccountClass).Debit(1);
        }

        private void bCredit_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as AccountClass).Credit(1);
        }
    }
}
