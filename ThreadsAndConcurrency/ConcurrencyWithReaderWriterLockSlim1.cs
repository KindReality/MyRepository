ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

private async void Button_Click(object sender, RoutedEventArgs e)
{
    var many = new List<double>() {
                1,2,3,4,5,6,7,8,9,10
            };
    await Task.Run(() =>
    {
        Parallel.Invoke(() =>
        {
            try
            {
                readerWriterLockSlim.EnterReadLock();
                foreach (var item in many)
                {
                    Dispatcher.Invoke(() =>
                    {
                        lbOutput.Items.Insert(0, item);
                    });
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }, () =>
        {
            try
            {
                readerWriterLockSlim.EnterReadLock();
                foreach (var item in many)
                {
                    Dispatcher.Invoke(() =>
                    {
                        lbOutput.Items.Insert(0, item);
                    });
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }, () =>
        {
            try
            {
                readerWriterLockSlim.EnterWriteLock();
                Thread.Sleep(5000);
                Dispatcher.Invoke(() =>
                {
                    lbOutput.Items.Insert(0, "Add 11");
                });
                many.Add(1.23);
            }
            finally
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        });
    });
}

