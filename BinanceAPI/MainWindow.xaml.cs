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


        public MainWindow()
        {
            InitializeComponent();

            NewPrices = new ObservableCollection<BinanceSymbolViewModel>();

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
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AllTickers { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            Info.ContextMenu.PlacementTarget = sender as Button;
            Info.ContextMenu.IsOpen = true;
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

                        AllTickers newMainWindowBD = new AllTickers()
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

                AllTickers newMainWindow = new AllTickers()
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

        public event Action<ObservableCollection<BinanceSymbolViewModel>, object> ValueChanged;
        public event Action<string, object> SearchChanged;

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
    }
}
