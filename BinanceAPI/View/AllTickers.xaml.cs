using System;
using System.Collections.Generic;
using Binance.Net.Clients;
using BinanceAPI.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BinanceAPI.Model;
using BinanceAPI.View;
using System.Reflection;

namespace BinanceAPI
{
    public partial class AllTickers : Window
    {
        public ObservableCollection<BinanceSymbolViewModel> newPrices;
        public ObservableCollection<BinanceSymbolViewModel> NewPrices
        {
            get { return newPrices; }
            set
            {
                newPrices = value;
            }
        }

        public ObservableCollection<BinanceSymbolViewModel> NewPricesFake;

        private BinanceSymbolViewModel selectedSymbol;

        public BinanceSymbolViewModel SelectedSymbol
        {
            get { return selectedSymbol; }
            set
            {
                selectedSymbol = value;
                MainViewModel mainViewModel = new MainViewModel();
                mainViewModel.SelectedSymbol = value;
            }
        }

        private TradeWindow TradeWindow = null;

        public AllTickers()
        {
            InitializeComponent();

            if (this.Title == "BinanceApi")
                Main.AllTickers = this;

            var lines = NewTable();

            foreach (var e in lines)
            {
                var item = e.Split(',')[0];

                MenuItem addNewMenuItem = new MenuItem
                {
                    Header = item,
                    FontSize = 15,
                };

                ContextMenuItem5.Items.Add(addNewMenuItem);
                addNewMenuItem.Click += AddNewMenuItem_Click;
            }
        }

        public List<string> NewTable()
        {
            var lines = new List<string>();

            var appDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var relativePath = @"Model\BD.txt";
            var fullPath = Path.Combine(appDir, relativePath);

            using (StreamReader reader = new StreamReader(fullPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }

        private void LeftClick(object sender, MouseButtonEventArgs e)
        {
            if (Lock.Locker)
            {
                if (Lock.LockedWindowLevel2 != null)
                {
                    var a = new MainViewModel();
                    Task.Run(() => a.CallOrderStream());

                    Lock.UserControlLevel2.Content = new Level2UserControl();
                    Lock.LockedWindowLevel2.Title = $"Level2({Selection.SelectedSymbol.Symbol})";
                }

                if (Lock.LockedWindowTradeStream != null)
                {
                    var a = new MainViewModel();
                    Task.Run(() => a.CallTradeStream());

                    Lock.UserControlTradeStream.Content = new TradeUserControl();
                    Lock.LockedWindowTradeStream.Title = $"TradeStream({Selection.SelectedSymbol.Symbol})";
                }

                if (Lock.LockedWindowAggTradeStream != null)
                {
                    var a = new MainViewModel();
                    Task.Run(() => a.CallAggTradeStream());

                    Lock.LockedWindowAggTradeStream.Content = new AggTradeUserControl();
                    Lock.LockedWindowAggTradeStream.Title = $"TradeAggStream({Selection.SelectedSymbol.Symbol})";
                }

                if (Lock.LockedWindowTrading != null)
                {
                    var a = new MainViewModel();
                    //Task.Run(() => a.CallTradeStream());

                    Lock.UserControl.Content = new TradingUserControl();
                    Lock.LockedWindow.Title = $"Trading({Selection.SelectedSymbol.Symbol})";
                }
            }
        }

        private void MenuItem_Click_Orders(object sender, RoutedEventArgs e)
        {
            if (!Lock.Locker)
            {
                TradeWindow = new TradeWindow
                {
                    Left = Left + Width * 1.01,
                    Top = Top,
                    Height = 886,
                    Width = 424,
                };

                var userControl = new UserControl
                {
                    Content = new Level2UserControl(),
                };

                TradeWindow.Title = $"Level2({Selection.SelectedSymbol.Symbol})";
                TradeWindow.StackForControl.Children.Add(userControl);
                TradeWindow.Show();
            }
            else
            {
                CloseStream.ClosedStream = true;
                CloseStream.ClosedWindowName = Lock.LockedWindow.Title;

                Lock.UserControl.Content = new Level2UserControl();
                Lock.LockedWindow.Title = $"Level2({Selection.SelectedSymbol.Symbol})";
                Lock.LockedWindow.Width = 424;
                Lock.LockedWindow.Height = 886;
            }
        }

        private void MenuItem_Click_Properties(object sender, RoutedEventArgs e)
        {
            new TickerProperties { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        private void MenuItem_Click_Trades(object sender, RoutedEventArgs e)
        {

            if (!Lock.Locker)
            {

                TradeWindow = new TradeWindow
                {
                    Left = Left + Width * 1.01,
                    Top = Top
                };

                var userControl = new UserControl
                {
                    Content = new TradeUserControl(),
                };

                TradeWindow.Title = $"TradeStream({Selection.SelectedSymbol.Symbol})";
                TradeWindow.StackForControl.Children.Add(userControl);
                TradeWindow.Show();
            }
            else
            {
                CloseStream.ClosedStream = true;
                CloseStream.ClosedWindowName = Lock.LockedWindow.Title;

                Lock.LockedWindow.Width = 380;
                Lock.LockedWindow.Height = 700;
                Lock.UserControl.Content = new TradeUserControl();
                Lock.LockedWindow.Title = $"TradeStream({Selection.SelectedSymbol.Symbol})";
                
            }
        }


        private void MenuItem_Click_TradesHistory(object sender, RoutedEventArgs e)
        {

            if (!Lock.Locker)
            {
                TradeWindow = new TradeWindow
                {
                    Left = Left + Width * 1.01,
                    Top = Top,
                    Width = 430,
                };

                var abc = new TradeUserControl();
                abc.Width = 430;
                abc.SelectorNew.Width = 430;
                abc.Time.Width = 150;
                abc.SelectorNew.Margin = new Thickness(-3, 0, 0, 0);

                var userControl = new UserControl
                {
                    Content = abc,
                    Width = 430,
                };

                TradeWindow.Title = $"TradeHistory({Selection.SelectedSymbol.Symbol})";
                TradeWindow.StackForControl.Children.Add(userControl);
                TradeWindow.Show();
            }
            else
            {
                CloseStream.ClosedStream = true;
                CloseStream.ClosedWindowName = Lock.LockedWindow.Title;

                Lock.LockedWindow.Width = 430;
                Lock.LockedWindow.Height = 700;
                Lock.UserControl.Content = new TradeUserControl();
                Lock.LockedWindow.Title = $"TradeHistory({Selection.SelectedSymbol.Symbol})";
            }
        }

        private void MenuItem_Click_AggTrades(object sender, RoutedEventArgs e)
        {
            if (!Lock.Locker)
            {
                TradeWindow = new TradeWindow
                {
                    Left = Left + Width * 1.01,
                    Top = Top,
                };

                var userControl = new UserControl
                {
                    Content = new AggTradeUserControl(),
                };

                TradeWindow.Title = $"AggTradeStream({Selection.SelectedSymbol.Symbol})";
                TradeWindow.StackForControl.Children.Add(userControl);
                TradeWindow.Show();
            }
            else
            {
                CloseStream.ClosedStream = true;
                CloseStream.ClosedWindowName = Lock.LockedWindow.Title;

                Lock.LockedWindow.Width = 360;
                Lock.LockedWindow.Height = 709;
                Lock.UserControl.Content = new AggTradeUserControl();
                Lock.LockedWindow.Title = $"AggTradeStream({Selection.SelectedSymbol.Symbol})";
            }
        }

        private void AddNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Main.MainWindow.ChangedValue(sender);
        }

        private void MenuItem_Click_Trading(object sender, RoutedEventArgs e)
        {
            if (!Lock.Locker)
            {
                TradeWindow = new TradeWindow
                {
                    Left = Left + Width * 1.01,
                    Top = Top,
                    Height = 473,
                    Width = 740,
                };

                TradingUserControl userControl = new TradingUserControl
                {
                };

                TradeWindow.Title = $"Trading({Selection.SelectedSymbol.Symbol})";
                TradeWindow.StackForControl.Children.Add(userControl);
                TradeWindow.Show();                
            }
            else
            {
                CloseStream.ClosedStream = true;
                CloseStream.ClosedWindowName = Lock.LockedWindow.Title;

                Lock.UserControl.Content = new TradingUserControl();
                Lock.LockedWindow.Title = $"Trading({Selection.SelectedSymbol.Symbol})";
                Lock.LockedWindow.Width = 740;
                Lock.LockedWindow.Height = 473;
            }
        }
    }
}
