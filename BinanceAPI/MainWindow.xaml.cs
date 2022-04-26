using Binance.Net.Clients;
using BinanceAPI.MVVM;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

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
        private List<object> ListOfNewWindows = new List<object>();

        public MainWindow()
        {
            InitializeComponent();

            if (this.Title == "BinanceApi")
                Main.main = this;

            var AllTables = NewTable();

            Task.Run(() => GetNewSymbols());

            foreach (var e in AllTables.Tables[0].Rows[0].ItemArray)
            {
                if (AllTables.Tables[0].Rows[1].ItemArray[Convert.ToInt32(e)].ToString() != "")
                {
                    MenuItem newMenuItem = new MenuItem
                    {
                        Header = AllTables.Tables[0].Rows[1].ItemArray[Convert.ToInt32(e)],
                        FontSize = 15,
                    };

                    MenuItem addNewMenuItem = new MenuItem
                    {
                        Header = AllTables.Tables[0].Rows[1].ItemArray[Convert.ToInt32(e)],
                        FontSize = 15,
                    };

                    AddNewList.ContextMenu.Items.Add(newMenuItem);
                    ContextMenuItem4.Items.Add(addNewMenuItem);
                    newMenuItem.Click += Menu_ClickBD;
                    addNewMenuItem.Click += AddNewMenuItem_Click;
                }
              


            }

        }

        public DataSet NewTable()
        {
            OleDbConnection StrCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\sozon\Desktop\Binnance\BinanceAPI\BinanceAPI\Model;Extended Properties=text");
            string Select1 = "SELECT * FROM [Dictionary.txt]";

            OleDbCommand comand1 = new OleDbCommand(Select1, StrCon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(comand1);
            DataSet AllTables = new DataSet();

            adapter.Fill(AllTables);

            return AllTables;
        }

        public void AddNewTable(int row, string text)
        {
            OleDbConnection StrCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\sozon\Desktop\Binnance\BinanceAPI\BinanceAPI\Model;Extended Properties=text");
            string Select1 = "SELECT * FROM [Dictionary.txt]";

            OleDbCommand comand = new OleDbCommand(Select1, StrCon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(comand);
            DataSet AllTables = new DataSet();

            StrCon.Open();
            adapter.Fill(AllTables);

            DataRow newRow = AllTables.Tables[0].NewRow();
            newRow[row] = text;
            AllTables.Tables[0].Rows.Add(newRow);

            OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter);
            adapter.Update(AllTables);
            AllTables.Clear();
            adapter.Fill(AllTables);

            StrCon.Close();
        }

        private void AddNewMenu(string text)
        {
            OleDbConnection StrCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\sozon\Desktop\Binnance\BinanceAPI\BinanceAPI\Model;Extended Properties=text");
            string Select1 = "SELECT * FROM [Dictionary.txt]";

            OleDbCommand comand = new OleDbCommand(Select1, StrCon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(comand);
            DataSet AllTables = new DataSet();

            StrCon.Open();
            adapter.Fill(AllTables);

            var count = AllTables.Tables[0].Rows[1].ItemArray.Where(r => r.ToString() != "").Count();
            DataRow newRow = AllTables.Tables[0].NewRow();;
            newRow[2] = text;
            Console.WriteLine(count);
            AllTables.Tables[0].Rows.InsertAt(newRow, 1);

            OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter);
            adapter.Update(AllTables);
            AllTables.Clear();
            adapter.Fill(AllTables);

            StrCon.Close();
        }

        private void LeftClick(object sender, MouseButtonEventArgs e)
        {
            new TradingWindow() { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        private void MenuItem_Click_Orders(object sender, RoutedEventArgs e)
        {
            new OrderWindow { Left = Left + Width * 2.25, Top = Top }.Show();
        }

        private void MenuItem_Click_Trades(object sender, RoutedEventArgs e)
        {
            new TradeWindow { Left = Left + Width * 1.01, Top = Top }.Show();
        }

        private void MenuItem_Click_AggTrades(object sender, RoutedEventArgs e)
        {
            new AggTradeWindow { Left = Left + Width * 1.01, Top = Top }.Show();
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

            newMenuItem.Click += Menu_Click;
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

        private void Menu_Click(object sender, RoutedEventArgs routedEventArgs)
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
            newMainWindow.Selector.ItemsSource = new ObservableCollection<BinanceSymbolViewModel>();
            newMainWindow.Selector.SelectedItem = SelectedSymbol;
            newMainWindow.GoHome.Visibility = Visibility.Visible;
            newMainWindow.Selector.ContextMenu.DataContext = new MainViewModel();
            newMainWindow.Selector.ContextMenu.Items.RemoveAt(3);
            newMainWindow.AddNewList.Visibility = Visibility.Hidden;
            newMainWindow.Closing += OnWindowClosing;
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

        private void Menu_ClickBD(object sender, RoutedEventArgs routedEventArgs)
        {
            var AllTables = NewTable();

            foreach (var i in AllTables.Tables[0].Rows[0].ItemArray)
            {
                if (sender.ToString().Split(' ')[1].Remove(0, 7) == AllTables.Tables[0].Rows[1].ItemArray[Convert.ToInt32(i.ToString())].ToString())
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
                    newMainWindowBD.Closing += OnWindowClosing;
                    newMainWindowBD.Search.DataContext = new Searching();
                    newMainWindowBD.Search.TextChanged += (send, args) => SearchChanged(Searching.SearchMain, newMainWindowBD.Title);


                    foreach (DataRow e in AllTables.Tables[0].Rows)
                    {
                        var l = NewPrices.SingleOrDefault(p => p.Symbol.ToString() == e.ItemArray[Convert.ToInt32(i.ToString())].ToString());
                        if(l != null)
                        {
                            a.Add(l);
                            b.Add(l);

                        }
                    }

                    newMainWindowBD.Selector.ItemsSource = a;

                    ValueChanged += (x, parentsender) =>
                    {
                        Console.WriteLine(12321312);
                        if (newMainWindowBD.Title == parentsender.ToString().Split(' ')[1].Remove(0, 7) && !a.Contains(Selection.SelectedSymbol))
                        {
                            foreach (var o in x)
                            {
                                a.Add(o);
                                b.Add(o);
                                AddNewTable(Convert.ToInt32(i.ToString()), o.Symbol);
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

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Console.WriteLine(1111);

        }

        public void GetNewSearch(string name, ObservableCollection<BinanceSymbolViewModel> a, ObservableCollection<BinanceSymbolViewModel> b)
        {
            
        }
    }
}
