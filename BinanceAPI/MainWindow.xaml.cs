using Binance.Net.Clients;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
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
    
    public class Searching
    {
        public static string searchMain;
        public static string SearchMain
        {
            get { return searchMain; }
            set
            {
                searchMain = value;
            }
        }
    }

    public partial class MainWindow : Window
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

        private bool FlagForList = true;
        private TradeWindow TradeWindow = null;

        public MainWindow()
        {
            InitializeComponent();

            if (this.Title == "BinanceApi")
                Main.main = this;

            Task.Run(async () => await Task.WhenAll(GetNewSymbols()));

            var lines = NewTable();

            foreach (var e in lines)
            {
                var item = e.Split(',')[0];
                MenuItem newMenuItem = new MenuItem
                {
                    Header = item,
                    FontSize = 15,
                };

                MenuItem addNewMenuItem = new MenuItem
                {
                    Header = item,
                    FontSize = 15,
                };

                AddNewList.ContextMenu.Items.Add(newMenuItem);
                ContextMenuItem4.Items.Add(addNewMenuItem);
                newMenuItem.Click += Menu_ClickBD;
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

        public void AddNewTable(string ticker, string line)
        {
            string str = string.Empty;

            var appDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var relativePath = @"Model\BD.txt";
            var fullPath = Path.Combine(appDir, relativePath);

            using (StreamReader reader = new StreamReader(fullPath))
            {
                str = reader.ReadToEnd();
            }

            string newLine = line + "," + ticker;
            str = str.Replace(line, newLine);

            using (StreamWriter file = new StreamWriter(fullPath))
            {
                file.Write(str);
            }
        }

        private void AddNewMenu(string name)
        {
            var appDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var relativePath = @"Model\BD.txt";
            var fullPath = Path.Combine(appDir, relativePath);

            using (StreamWriter file = new StreamWriter(fullPath, true))
            {
                file.WriteLine(name);
            }
        }

        private void LeftClick(object sender, MouseButtonEventArgs e)
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

                var userControl = new UserControl
                {
                    Content = new TradingUserControl(),
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

                Lock.LockedWindow.Width = 360;
                Lock.LockedWindow.Height = 709;
                Lock.UserControl.Content = new TradeUserControl();
                Lock.LockedWindow.Title = $"TradeStream({Selection.SelectedSymbol.Symbol})";
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

        private void AddNewList_Click(object sender, RoutedEventArgs e)
        {
            AddNewList.ContextMenu.DataContext = AddNewList.DataContext;
            AddNewList.ContextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (FlagForList)
            {
                var frame = new Frame
                {
                    Height = 60,
                    Width = 290,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 40, 0, 0),
                    Content = new NewWatchList()
                };

                stack.Children.Insert(0, frame);
                FlagForList = false;
                Selector.Margin = new Thickness(0, 10, 0, 0);

                ButtonCreateNewList.Visibility = Visibility.Visible;
                ButtonCancelNewList.Visibility = Visibility.Visible;
            }
        }

        private void ButtonCancelNewList_Click(object sender, RoutedEventArgs e)
        {
            stack.Children.RemoveAt(0);
            Selector.Margin = new Thickness(0, 38, 0, 0);
            ButtonCreateNewList.Visibility = Visibility.Hidden;
            ButtonCancelNewList.Visibility = Visibility.Hidden;
            FlagForList = true;
        }

        private void ButtonCreateNewList_Click(object sender, RoutedEventArgs e)
        {
            MenuItem addNewMenuItem = new MenuItem
            {
                Header = NewListName.Name,
                FontSize = 15,
            };

            MenuItem newMenuItem = new MenuItem
            {
                Header = NewListName.Name,
                FontSize = 15,      
            };

            newMenuItem.Click += Menu_ClickBD;
            AddNewList.ContextMenu.Items.Add(newMenuItem);

            addNewMenuItem.Click += AddNewMenuItem_Click;
            stack.Children.RemoveAt(0);
            Selector.Margin = new Thickness(0, 38, 0, 0);

            ButtonCreateNewList.Visibility = Visibility.Hidden;
            ButtonCancelNewList.Visibility = Visibility.Hidden;

            ContextMenuItem4.Items.Add(addNewMenuItem);
            var a = new MainViewModel();
            FlagForList = true;
        }

        private void AddNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NewPricesFake = new ObservableCollection<BinanceSymbolViewModel>();
            NewPricesFake.Insert(0, Selection.SelectedSymbol);
            ValueChanged(NewPricesFake, sender);
        }

        private void Menu_ClickBD(object sender, RoutedEventArgs routedEventArgs)
        {
            var currLines = NewTable();
            var names = new List<string>();

            foreach (var line in currLines)
                names.Add(line.Split(',')[0]);

            if (names.Contains(sender.ToString().Split(' ')[1].Remove(0, 7)))
            {
                var lines = NewTable();

                foreach (string i in lines)
                {
                    if (sender.ToString().Split(' ')[1].Remove(0, 7) == i.Split(',')[0])
                    {
                        var a = new ObservableCollection<BinanceSymbolViewModel>();
                        var b = new ObservableCollection<BinanceSymbolViewModel>();

                        MainWindow newMainWindowBD = new MainWindow()
                        {
                            Left = Left + Width * 1.3,
                            Top = Top,
                            Title = sender.ToString().Split(' ')[1].Remove(0, 7),
                        };

                        newMainWindowBD.Selector.ItemsSource = new ObservableCollection<BinanceSymbolViewModel>();
                        newMainWindowBD.Selector.SelectedItem = SelectedSymbol;
                        newMainWindowBD.GoHome.Visibility = Visibility.Visible;
                        newMainWindowBD.Selector.ContextMenu.DataContext = new MainViewModel();
                        newMainWindowBD.Selector.ContextMenu.Items.RemoveAt(3);
                        newMainWindowBD.AddNewList.Visibility = Visibility.Hidden;
                        newMainWindowBD.Search.DataContext = new Searching();
                        newMainWindowBD.Search.TextChanged += (send, args) => SearchChanged(Searching.SearchMain, newMainWindowBD.Title);

                        var tickers = i.Split(',');
                        foreach (string e in tickers)
                        {
                            var ticker = NewPrices.SingleOrDefault(p => p.Symbol.ToString() == e);
                            if (ticker != null)
                            {
                                a.Add(ticker);
                                b.Add(ticker);
                            }
                        }
                        newMainWindowBD.Selector.ItemsSource = a;

                        ValueChanged += (x, parentsender) =>
                        {
                            if (newMainWindowBD.Title == parentsender.ToString().Split(' ')[1].Remove(0, 7) && !a.Contains(Selection.SelectedSymbol))
                            {
                                foreach (var o in x)
                                {
                                    a.Add(o);
                                    b.Add(o);
                                    AddNewTable(o.Symbol, i);
                                }

                                newMainWindowBD.Selector.ItemsSource = a;
                            }
                        };

                        SearchChanged += (text, title) =>
                        {
                            if (newMainWindowBD.Title == title.ToString())
                            {
                                if (text != "")
                                {
                                    a.Clear();

                                    foreach (var o in b)
                                    {
                                        if (o.Symbol.Contains(text.ToUpper()))
                                        {
                                            a.Add(o);
                                        }
                                    }
                                }
                                else if (text == "" || text == null)
                                {
                                    a.Clear();
                                    foreach (var o in b)
                                    {
                                        a.Add(o);
                                    }
                                }
                            }

                            newMainWindowBD.Selector.ItemsSource = a;
                        };
                    }
                }
            }
            else
            {
                var a = new ObservableCollection<BinanceSymbolViewModel>();
                var b = new ObservableCollection<BinanceSymbolViewModel>();

                MainWindow newMainWindow = new MainWindow()
                {
                    Left = Left + Width * 1.3,
                    Top = Top,
                    Title = sender.ToString().Split(' ')[1].Remove(0, 7),
                };

                AddNewMenu(newMainWindow.Title.ToString());
                var lines = NewTable();
                var line = lines.Last();

                newMainWindow.Selector.ItemsSource = new ObservableCollection<BinanceSymbolViewModel>();
                newMainWindow.Selector.SelectedItem = SelectedSymbol;
                newMainWindow.GoHome.Visibility = Visibility.Visible;
                newMainWindow.Selector.ContextMenu.DataContext = new MainViewModel();
                newMainWindow.Selector.ContextMenu.Items.RemoveAt(3);
                newMainWindow.AddNewList.Visibility = Visibility.Hidden;
                newMainWindow.Search.DataContext = new Searching();
                newMainWindow.Search.TextChanged += (send, args) => SearchChanged(Searching.SearchMain, newMainWindow.Title);

                ValueChanged += (x, parentsender) =>
                {
                    if (newMainWindow.Title == parentsender.ToString().Split(' ')[1].Remove(0, 7) && !a.Contains(Selection.SelectedSymbol))
                    {
                        foreach (var o in x)
                        {
                            a.Add(o);
                            b.Add(o);
                            AddNewTable(o.Symbol, line);
                        }
                        newMainWindow.Selector.ItemsSource = a;
                    }
                };

                SearchChanged += (text, title) =>
                {
                    if (newMainWindow.Title == title.ToString())
                    {
                        if (text != "")
                        {
                            a.Clear();

                            foreach (var o in b)
                            {
                                if (o.Symbol.Contains(text.ToUpper()))
                                {
                                    a.Add(o);
                                }
                            }
                        }
                        else if (text == "" || text == null)
                        {
                            a.Clear();
                            foreach (var o in b)
                            {
                                a.Add(o);
                            }
                        }
                    }

                    newMainWindow.Selector.ItemsSource = a;
                };
            }
        }


        private async Task GetNewSymbols()
        {
            var client = new BinanceClient();
            var result = await client.SpotApi.ExchangeData.GetPricesAsync();

            NewPrices = new ObservableCollection<BinanceSymbolViewModel>(result.Data.Select(r => new BinanceSymbolViewModel(r.Symbol, r.Price)).ToList());
        }


        public event Action<ObservableCollection<BinanceSymbolViewModel>, object> ValueChanged;
        public event Action<string, object> SearchChanged;

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            Main.main.Visibility = Visibility.Visible;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if(this.Title == "BinanceApi")
            {
                e.Cancel = true;
                Main.main = this;
                Main.main.Visibility = Visibility.Hidden;
            }
        }
    }
}
