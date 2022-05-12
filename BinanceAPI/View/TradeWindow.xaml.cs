using System;
using System.Windows;
using BinanceAPI.Model;
using System.Windows.Controls;

namespace BinanceAPI
{ 
    public partial class TradeWindow : Window
    {
        public TradeWindow()
        {
            InitializeComponent();

            CloseStream.ClosedStream = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            CloseStream.ClosedStream = true;
            CloseStream.ClosedWindowName = Title;
            
            Lock.Locker = false;
        }

        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            Lock.Locker = !Lock.Locker;
            foreach (var Window in App.Current.Windows)
            {
                if(Window is TradeWindow)
                {
                    var window = Window as TradeWindow;

                    if (window.Title.Contains("Level2"))
                    {
                        Lock.LockedWindowLevel2 = window;
                        Lock.UserControlLevel2 = (UserControl)window.StackForControl.Children[0];
                        Console.WriteLine("1");
                    }
                    else if (window.Title.Contains("Agg"))
                    {
                        Lock.LockedWindowAggTradeStream = window;
                        Lock.UserControlAggTradeStream = (UserControl)window.StackForControl.Children[0];
                        Console.WriteLine("2");
                    }
                    else if (window.Title.Contains("TradeStream"))
                    {
                        Lock.LockedWindowTradeStream = window;
                        Lock.UserControlTradeStream = (UserControl)window.StackForControl.Children[0];
                        Console.WriteLine("3");

                    }
                    else if (window.Title.Contains("Trading"))
                    {
                        Lock.LockedWindowTrading = window;
                        Lock.UserControlTrading = (UserControl)window.StackForControl.Children[0];
                        Console.WriteLine("4");

                    }
                }
            }

            Lock.LockedWindow = this;
            Lock.UserControl = (UserControl)StackForControl.Children[0];
        }
    }
}
