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
        private List<AccountClass> accounts = new List<AccountClass>();

        private void Create()
        {
            var account = new AccountClass
            {
                Name = tbName.Text
            };
            accounts.Add(account);
        }

        public List<AccountClass> Retrieve()
        {
            return accounts;
        }

        private AccountClass Retrieve(int id)
        {
            return null;//TODO: Retrieve
        }

        private void Update(AccountClass account)
        {
            UpdateStatus("Not Implemented");//TODO: Update
        }

        private void DeleteMessage(int accountID)
        {
            var account = Retrieve(accountID);
            UpdateStatus("Not Implemented");//TODO: Delete
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
            tbName.Text = "";
            _retrieve();
        }
        private void _retrieve()
        {
            var result = Retrieve();
            lbAccounts.Items.Clear();
            foreach (var item in result)
            {
                lbAccounts.Items.Add(item);
            }
        }

        private void lbAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bDelete.IsEnabled = true;
        }

        private void lbAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var account = lbAccounts.SelectedItem as AccountClass;
            if (account != null)
            {
                (new EditWindow(account)).ShowDialog();
                _retrieve();
            }
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            var accountID = (lbAccounts.SelectedItem as AccountClass)?.AccountID;
            if (accountID != null)
            {
                DeleteMessage(accountID.Value);
            }
            _retrieve();
        }
    }
}
