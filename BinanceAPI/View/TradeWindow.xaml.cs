using System;
using System.Windows;
using BinanceAPI.ViewModels;
using BinanceAPI.Model;
using System.Windows.Controls;
using BinanceAPI.View;
using System.Windows.Navigation;

namespace BinanceAPI
{ 
    public partial class TradeWindow : Window
    {
        public TradeWindow()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;

            CloseStream.CloseTradeStream = false;

            if(Lock.Locker)
            {
                var frame = new Frame
                {
                    Content = new TradeFrame()
                };
                NavigationWindow _navigationWindow = new NavigationWindow();

                _navigationWindow.Height = this.Height;

                _navigationWindow.Width = this.Width;

                _navigationWindow.Show();

                _navigationWindow.Navigate(frame);
                
                this.Close();
            }

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CloseStream.CloseTradeStream = true;
        }

        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            Lock.Locker = !Lock.Locker;
        }
    }
}
