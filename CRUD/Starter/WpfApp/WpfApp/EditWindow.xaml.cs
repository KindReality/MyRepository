using System.Windows;

namespace WpfApp
{
    public partial class EditWindow : Window
    {
        private Message message;
        public EditWindow(Message message)
        {
            InitializeComponent();
            this.message = message;
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            message.Value = tbValue.Text;
            Hide();
        }
    }
}
