using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Linq;
using System.Data.Entity;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private MyModel context = new MyModel();

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

        private List<Message> Retrieve()
        {
            var result = context
                            .Messages
                            .OrderByDescending(x => x.CreatedOn)
                            .ToList();
            return result;
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

        //background code
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _retrieve();
            (new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, (_sender, _e) =>
            {
                if (status.Content.ToString() != "" && DateTime.Now - updateStatesTimestamp > new TimeSpan(0,0,5))
                {
                    status.Content = "";
                }
            }, Dispatcher.CurrentDispatcher)).Start();
        }

        private DateTime updateStatesTimestamp = DateTime.MinValue;

        private void UpdateStatus(string value)
        {
            updateStatesTimestamp = DateTime.Now;
            status.Content = value;
        }

        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            Create();
            tbMessage.Text = "";
            _retrieve();
        }
        private void _retrieve()
        {
            var result = Retrieve();
            lbMessages.Items.Clear();
            foreach (var item in result)
            {
                lbMessages.Items.Add(item);
            }
        }

        private void lbMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bDelete.IsEnabled = true;
        }

        private void lbMessages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var message = lbMessages.SelectedItem as Message;
            if (message != null)
            {
                Update(message);
                _retrieve();
            }
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            var messageID = (lbMessages.SelectedItem as Message)?.MessageID;
            if (messageID != null)
            {
                DeleteMessage(messageID.Value);
            }
            _retrieve();
        }
    }
}
