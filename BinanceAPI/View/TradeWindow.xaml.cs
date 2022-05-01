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
            Lock.LockedWindow = this;
            Lock.UserControl = (UserControl)StackForControl.Children[0];
        }
    }
}
