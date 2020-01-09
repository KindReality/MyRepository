using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MyModel context = new MyModel();
        
        private void Create()
        {
            var message = new Message
            {
                Value = tbMessage.Text,
                CreatedOn = DateTime.Now
            };
            context.Messages.Add(message);
            context.SaveChanges();
        }

        private void Retrieve()
        {
            var result = context
                            .Messages
                            .OrderByDescending(x => x.CreatedOn)
                            .ToList();
            foreach (var item in result)
            {
                lbMessages.Items.Add(item);
            }
        }

        private Message Retrieve(int id)
        {
            var result = context
                            .Messages
                            .SingleOrDefault(x => x.MessageID == id);
            return result;
        }

        private void Update(Message message)
        {
            if ((new EditWindow(message)).ShowDialog() == true)
            {
                context.Entry<Message>(message).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private void DeleteMessage(int messageID)
        {
            var message = context.Messages.Single(x => x.MessageID == messageID);
            context.Messages.Remove(message);
            context.SaveChanges();
        }


        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Retrieve();
        }

        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            Create();
            lbMessages.Items.Clear();
            Retrieve();
        }

        private void lbMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bDelete.IsEnabled = true;
        }

        private void lbMessages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var message = lbMessages.SelectedItem as Message;
            Update(message);
            lbMessages.Items.Clear();
            Retrieve();
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            var messageID = (lbMessages.SelectedItem as Message)?.MessageID;
            if (messageID != null)
            {
                DeleteMessage(messageID.Value);
            }
            lbMessages.Items.Clear();
            Retrieve();
        }
    }
}
