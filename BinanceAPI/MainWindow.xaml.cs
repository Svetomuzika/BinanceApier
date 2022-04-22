using Binance.Net.Clients;
using BinanceAPI.MVVM;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace BinanceAPI
{


    public partial class MainWindow : Window
    {
        private bool FlagForList = true;
        private ICommand NewSymbolCommand { get; set; }

        private List<object> ListOfNewWindows = new List<object>();

        public MainWindow()
        {
            InitializeComponent();

            NewSymbolCommand = new DelegateCommand((o) => NewSymboll(o));

            if (this.Title == "BinanceApi")
                Main.main = this;
        }

        public void NewSymboll(object o)
        {
            if (!NewAllPrices.NewPrices.Contains(Selection.SelectedSymbol))
            {
                NewAllPrices.NewPrices.Insert(0, Selection.SelectedSymbol);
                NewAllPrices.FakeSymbol.Insert(0, Selection.SelectedSymbol);
            }
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
            addNewMenuItem.Command = NewSymbolCommand;
            FlagForList = true;
        }

        private void AddNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(e.Source);
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (!ListOfNewWindows.Contains(sender))
            {
                ListOfNewWindows.Add(sender);

                MainWindow newMainWindow = new MainWindow()
                {
                    Left = Left + Width * 1.3,
                    Top = Top,
                    Title = NewListName.Name,
                    DataContext = new NewAllPrices(),
                };

                newMainWindow.Selector.ItemsSource = NewAllPrices.NewPrices;
                newMainWindow.GoHome.Visibility = Visibility.Visible;
                newMainWindow.Selector.ContextMenu.DataContext = new MainViewModel();
                newMainWindow.Selector.ContextMenu.Items.RemoveAt(3);
                newMainWindow.AddNewList.Visibility = Visibility.Hidden;
            }
            else
            {

                MainWindow newMainWindow = new MainWindow()
                {
                    Left = Left + Width * 1.3,
                    Top = Top,
                    Title = sender.ToString().Split(' ')[1].Remove(0, 7),
                    DataContext = new NewAllPrices(),
                };



                NewAllPrices.NewPrices = new ObservableCollection<BinanceSymbolViewModel>();

                newMainWindow.Selector.ItemsSource = NewAllPrices.NewPrices;
                newMainWindow.GoHome.Visibility = Visibility.Visible;
                newMainWindow.Selector.ContextMenu.DataContext = new MainViewModel();
                newMainWindow.Selector.ContextMenu.Items.RemoveAt(3);
                newMainWindow.AddNewList.Visibility = Visibility.Hidden;
            }

        }

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
