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
using System.Windows.Media;
using System.Windows.Data;
using System.Xml.Linq;
using CryptoExchange.Net.CommonObjects;
using System.Windows.Shapes;
using Path = System.IO.Path;
using static System.Net.Mime.MediaTypeNames;

namespace BinanceAPI
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<BinanceSymbolViewModel> allPrices;
        public ObservableCollection<BinanceSymbolViewModel> AllPrices
        {
            get { return allPrices; }
            set
            {
                allPrices = value;
            }
        }

        public ObservableCollection<BinanceSymbolViewModel> newPrices;
        public ObservableCollection<BinanceSymbolViewModel> NewPrices
        {
            get { return newPrices; }
            set
            {
                newPrices = value;
            }
        }

        public ObservableCollection<BinanceSymbolViewModel> newPricesDuplicate;
        public ObservableCollection<BinanceSymbolViewModel> NewPricesDuplicate
        {
            get { return newPricesDuplicate; }
            set
            {
                newPricesDuplicate = value;
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


        public MainWindow()
        {
            InitializeComponent();

            Main.MainWindow = this;
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

                Watchlists.Items.Add(newMenuItem);

                newMenuItem.Click += Menu_ClickBD;
            }
            //Trades.IsEnabled = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AllTickers { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            Info.ContextMenu.PlacementTarget = sender as Button;
            Info.ContextMenu.IsOpen = true;
        }

        private void Trades_Click(object sender, RoutedEventArgs e)
        {
            Trades.ContextMenu.PlacementTarget = sender as Button;
            Trades.ContextMenu.IsOpen = true;
        }

        private void Menu_ClickBD(object sender, RoutedEventArgs routedEventArgs)
        {

            NewPrices = new ObservableCollection<BinanceSymbolViewModel>();
            NewPricesDuplicate = new ObservableCollection<BinanceSymbolViewModel>();

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
                        AllTickers newMainWindowBD = new AllTickers()
                        {
                            Left = Left + Width * 1.805,
                            Top = Top,
                            Title = sender.ToString().Split(' ')[1].Remove(0, 7),
                        };

                        //newMainWindowBD.Selector.ItemsSource = new ObservableCollection<BinanceSymbolViewModel>();
                        //newMainWindowBD.GoHome.Visibility = Visibility.Visible;
                        //newMainWindowBD.Selector.ContextMenu.DataContext = new MainViewModel();
                        //newMainWindowBD.Selector.ContextMenu.Items.RemoveAt(4);
                        //newMainWindowBD.AddNewList.Visibility = Visibility.Hidden;
                        newMainWindowBD.Search.DataContext = new Searching();
                        newMainWindowBD.Search.TextChanged += (send, args) => SearchChanged(Searching.SearchMain, newMainWindowBD.Title);

                        var tickers = i.Split(',');

                        foreach (string e in tickers)
                        {
                            var ticker = AllPrices.SingleOrDefault(p => p.Symbol.ToString() == e);

                            if (ticker != null)
                            {
                                NewPrices.Add(ticker);
                                NewPricesDuplicate.Add(ticker);
                            }
                        }
                        newMainWindowBD.Selector.ItemsSource = NewPrices;

                        ValueChanged += (x, parentsender) =>
                        {
                            if (newMainWindowBD.Title == parentsender.ToString().Split(' ')[1].Remove(0, 7) && !NewPrices.Contains(Selection.SelectedSymbol))
                            {
                                foreach (var o in x)
                                {
                                    NewPrices.Add(o);
                                    NewPricesDuplicate.Add(o);
                                    AddNewTable(o.Symbol, i);
                                }

                                newMainWindowBD.Selector.ItemsSource = NewPrices;
                            }
                        };

                        SearchChanged += (text, title) =>
                        {
                            if (newMainWindowBD.Title == title.ToString())
                            {
                                if (text != "")
                                {
                                    NewPrices.Clear();

                                    foreach (var o in NewPricesDuplicate)
                                    {
                                        if (o.Symbol.Contains(text.ToUpper()))
                                        {
                                            NewPrices.Add(o);
                                        }
                                    }
                                }
                                else if (text == "" || text == null)
                                {
                                    NewPrices.Clear();
                                    foreach (var o in NewPricesDuplicate)
                                    {
                                        NewPrices.Add(o);
                                    }
                                }
                            }

                            newMainWindowBD.Selector.ItemsSource = NewPrices;
                        };


                    }
                }
            }
            else
            {
                var NewPrices = new ObservableCollection<BinanceSymbolViewModel>();
                var NewPricesDuplicate = new ObservableCollection<BinanceSymbolViewModel>();

                AllTickers newMainWindow = new AllTickers()
                {
                    Left = Left + Width * 1.805,
                    Top = Top,
                    Title = sender.ToString().Split(' ')[1].Remove(0, 7),
                };

                AddNewMenu(newMainWindow.Title.ToString());
                var lines = NewTable();
                var line = lines.Last();

                newMainWindow.Selector.ItemsSource = new ObservableCollection<BinanceSymbolViewModel>();
                newMainWindow.Selector.SelectedItem = SelectedSymbol;
                //newMainWindow.GoHome.Visibility = Visibility.Visible;
                newMainWindow.Selector.ContextMenu.DataContext = new MainViewModel();
                newMainWindow.Selector.ContextMenu.Items.RemoveAt(3);
                //newMainWindow.AddNewList.Visibility = Visibility.Hidden;
                newMainWindow.Search.DataContext = new Searching();
                newMainWindow.Search.TextChanged += (send, args) => SearchChanged(Searching.SearchMain, newMainWindow.Title);

                ValueChanged += (x, parentsender) =>
                {
                    if (newMainWindow.Title == parentsender.ToString().Split(' ')[1].Remove(0, 7) && !NewPrices.Contains(Selection.SelectedSymbol))
                    {
                        foreach (var o in x)
                        {
                            NewPrices.Add(o);
                            NewPricesDuplicate.Add(o);
                            AddNewTable(o.Symbol, line);
                        }
                        newMainWindow.Selector.ItemsSource = NewPrices;
                    }
                };

                SearchChanged += (text, title) =>
                {
                    if (newMainWindow.Title == title.ToString())
                    {
                        if (text != "")
                        {
                            NewPrices.Clear();

                            foreach (var o in NewPricesDuplicate)
                            {
                                if (o.Symbol.Contains(text.ToUpper()))
                                {
                                    NewPrices.Add(o);
                                }
                            }
                        }
                        else if (text == "" || text == null)
                        {
                            NewPrices.Clear();
                            foreach (var o in NewPricesDuplicate)
                            {
                                NewPrices.Add(o);
                            }
                        }
                    }

                    newMainWindow.Selector.ItemsSource = NewPrices;
                };
            }
        }

        public event Action<ObservableCollection<BinanceSymbolViewModel>, object> ValueChanged;
        public event Action<string, object> SearchChanged;

        private async Task GetNewSymbols()
        {
            var client = new BinanceClient();
            var result = await client.SpotApi.ExchangeData.GetPricesAsync();

            AllPrices = new ObservableCollection<BinanceSymbolViewModel>(result.Data.Select(r => new BinanceSymbolViewModel(r.Symbol, r.Price)).ToList());
        }

        public void ChangedValue(object sender)
        {
            NewPricesFake = new ObservableCollection<BinanceSymbolViewModel>();
            NewPricesFake.Insert(0, Selection.SelectedSymbol);

            if(ValueChanged != null)
            {
                ValueChanged(NewPricesFake, sender);
            }
            else
            {
                var lines = NewTable();

                foreach (string i in lines)
                {
                    if(i.Contains(sender.ToString().Split(' ')[1].Remove(0, 7)))
                    {
                        foreach(var o in NewPricesFake)
                        {
                            AddNewTable(o.Symbol, i);
                        }
                    }
                }
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

        private void Info_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (!(sender is Button button) || button.ContextMenu == null)
                return;

            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.IsOpen = true;
            e.Handled = true;
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            new AllOrders { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        private void Trade_Click(object sender, RoutedEventArgs e)
        {
            new AllTrades { Left = Left + Width * 1.01, Top = Top }.Show();
        }
        private void Bots_Click(object sender, RoutedEventArgs e)
        {
            new AllBots { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            new AllClients { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            new LoginApi { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        public async Task ConnectingSucces(bool WorkspaceToggleSwitch)
        {
            ConnectionTextBlock.Text = "Connecting.";
            ConnectionTextBlock.Foreground = new SolidColorBrush(Colors.Orange);

            await Task.Delay(550);

            ConnectionTextBlock.Text = "Connecting..";
            ConnectionTextBlock.Foreground = new SolidColorBrush(Colors.Orange);

            await Task.Delay(550);

            ConnectionTextBlock.Text = "Connected";
            ConnectionTextBlock.Foreground = new SolidColorBrush(Colors.Green);

            Binding myBinding = new Binding();
            myBinding.Source = new MainViewModel();
            myBinding.Path = new PropertyPath("TradingAllOrders.Count");
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(ActualOrdersCount, TextBlock.TextProperty, myBinding);

            Trades.IsEnabled = true;
            Api.TradingButtonEnable = true;

            foreach(Window e in App.Current.Windows)
            {
                if (e.Title == "Login")
                    e.Close();
            }

            if (!WorkspaceToggleSwitch)
                GetOldWindows();
        }

        public async Task ConnectingError()
        {
            ConnectionTextBlock.Text = "Connecting.";
            ConnectionTextBlock.Foreground = new SolidColorBrush(Colors.Orange);

            await Task.Delay(550);

            ConnectionTextBlock.Text = "Connecting..";
            ConnectionTextBlock.Foreground = new SolidColorBrush(Colors.Orange);

            await Task.Delay(550);

            ConnectionTextBlock.Text = "Offline";
            ConnectionTextBlock.Foreground = new SolidColorBrush(Colors.Red);

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var windows = App.Current.Windows;

            var appDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var relativePath = @"Model\BDSamples.txt";
            var fullPath = Path.Combine(appDir, relativePath);

            using (StreamWriter newFile = new StreamWriter(fullPath)) { }

            foreach (Window i in windows)
            {
                var str = i.Title + "." + i.Left + "." + i.Top;

                using (StreamWriter file = new StreamWriter(fullPath, true))
                {
                    file.WriteLine(str);
                }
            }
        }

        private async void GetOldWindows()
        {
            var client = new BinanceClient();
            var result = await client.SpotApi.ExchangeData.GetPricesAsync();
            var allPrices = new ObservableCollection<BinanceSymbolViewModel>(result.Data.Select(r => new BinanceSymbolViewModel(r.Symbol, r.Price)).ToList().OrderByDescending(p => p.Price));

            string symbol;
            var lines = GetLinesBDSamples();
            var mainViewModel = new MainViewModel();
            var allTickers = new AllTickers
            {
                Visibility = Visibility.Hidden,
            };

            allTickers.Selector.ItemsSource = allPrices;

            await Task.Delay(1000);

            foreach (var line in lines)
            {
                var obj = line.Split('.');
                var title = obj[0];
                var left = double.Parse(obj[1]);
                var top = double.Parse(obj[2]);
                int i = 0;

                switch (title)
                {
                    case "Login":
                        new LoginApi { Left = left, Top = top }.Show();
                        break;
                    case "AllOrders":
                        new AllOrders { Left = left, Top = top }.Show();
                        break;
                    case "AllTrades":
                        new AllTrades { Left = left, Top = top }.Show();
                        break;
                    case "AllBots":
                        new AllBots { Left = left, Top = top }.Show();
                        break;
                    case "AllClients":
                        new AllClients { Left = left, Top = top }.Show();
                        break;
                    case "Securities":
                        new AllTickers { Left = left, Top = top }.Show();
                        break;

                    case string a when a.Contains("TradeStream"):
                        symbol = a.Remove(0, 12);
                        symbol = symbol.Remove(symbol.Length - 1);
                        i = FindSelectionAsync(symbol, allPrices);

                        allTickers.Selector.SelectedItem = allTickers.Selector.Items[i];
                        await mainViewModel.GetTradeStream();

                        TradeWindow TradeWindow = new TradeWindow
                        {
                            Left = left,
                            Top = top
                        };

                        var userControl = new UserControl
                        {
                            Content = new TradeUserControl(),
                        };

                        TradeWindow.Title = a;
                        TradeWindow.StackForControl.Children.Add(userControl);
                        TradeWindow.Show();
                        break;

                    case string b when b.Contains("Level2"):
                        symbol = b.Remove(0, 7);
                        symbol = symbol.Remove(symbol.Length - 1);
                        i = FindSelectionAsync(symbol, allPrices);

                        allTickers.Selector.SelectedItem = allTickers.Selector.Items[i];
                        await mainViewModel.CallOrderStream();

                        TradeWindow TradeWindow1 = new TradeWindow
                        {
                            Left = left,
                            Top = top,
                            Height = 886,
                            Width = 424,
                        };

                        var userControl1 = new UserControl
                        {
                            Content = new Level2UserControl(),
                        };

                        TradeWindow1.Title = b;
                        TradeWindow1.StackForControl.Children.Add(userControl1);
                        TradeWindow1.Show();
                        break;

                    case string c when c.Contains("Trading"):
                        symbol = c.Remove(0, 8);
                        symbol = symbol.Remove(symbol.Length - 1);
                        i = FindSelectionAsync(symbol, allPrices);

                        allTickers.Selector.SelectedItem = allTickers.Selector.Items[i];

                        TradeWindow TradeWindow2 = new TradeWindow
                        {
                            Left = left,
                            Top = top,
                            Height = 473,
                            Width = 740,
                        };

                        TradingUserControl userControl2 = new TradingUserControl();

                        TradeWindow2.Title = c;
                        TradeWindow2.StackForControl.Children.Add(userControl2);
                        TradeWindow2.Show();
                        break;

                    default:
                        break;
                }
                allTickers.Close();
            }
        }

        public int FindSelectionAsync(string symbol, ObservableCollection<BinanceSymbolViewModel> allPrices)
        {
            foreach (var e in allPrices)
                if (e.Symbol.Equals(symbol))
                    return allPrices.IndexOf(e);

            return 0;
        }

        private List<string> GetLinesBDSamples()
        {
            var lines = new List<string>();

            var appDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var relativePath = @"Model\BDSamples.txt";
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
    }
}
