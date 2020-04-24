private async void Button_Click(object sender, RoutedEventArgs e)
{
    decimal balance = 100;

    await Task.Run(() =>
    {
        Parallel.Invoke(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                lock (handle)
                {
                    Thread.Sleep(333);
                    var current = balance;
                    Thread.Sleep(333);
                    balance = current - 1;
                    Thread.Sleep(333);
                }
                Dispatcher.Invoke(() =>
                {
                    lbOutput.Items.Insert(0, "A:" + balance);
                });
            }
        }, () =>
        {
            for (int i = 0; i < 10; i++)
            {
                lock (handle)
                {
                    Thread.Sleep(333);
                    var current = balance;
                    Thread.Sleep(333);
                    balance = current - 1;
                    Thread.Sleep(333);
                }
                Dispatcher.Invoke(() =>
                {
                    lbOutput.Items.Insert(0, "B:" + balance);
                });
            }
        }, () =>
        {
            for (int i = 0; i < 10; i++)
            {
                lock (handle)
                {
                    Thread.Sleep(333);
                    var current = balance;
                    Thread.Sleep(333);
                    balance = current - 1;
                    Thread.Sleep(333);
                }
                Dispatcher.Invoke(() =>
                {
                    lbOutput.Items.Insert(0, "C:" + balance);
                });
            }
        });
    });
}
