using System;
using System.Windows;
using BinanceAPI.ViewModels;
using BinanceAPI.Model;
using System.Windows.Controls;
using BinanceAPI.View;

namespace BinanceAPI
{ 
    public partial class TradeWindow : Window
    {
        public TradeWindow()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;

            CloseStream.CloseTradeStream = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CloseStream.CloseTradeStream = true;
            Lock.Locker = false;
        }

        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            Lock.Locker = !Lock.Locker;
            Lock.LockedWindow = this;
        }
    }
}
