using System.Windows;

namespace WpfApp
{
    public partial class EditWindow : Window
    {
        private Message existingMessage;
        public EditWindow(Message message)
        {
            InitializeComponent();
            this.existingMessage = message;
            tbValue.Text = existingMessage.Value;
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            existingMessage.Value = tbValue.Text;
            Hide();
        }
    }
}
