using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AccountClass : INotifyPropertyChanged
{
    public int AccountID { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Debit(decimal amount)
    {
        Balance -= amount;
        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Balance"));
    }
    public void Credit(decimal amount)
    {
        Balance += amount;
        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Balance"));
    }
}
