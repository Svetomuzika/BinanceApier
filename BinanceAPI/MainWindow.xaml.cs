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
        public ICommand NewSymbolCommand { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            NewSymbolCommand = new DelegateCommand((o) => NewSymboll(o));

        }

        public void NewSymboll(object o)
        {
            
            NewAllPrices.NewPrices.Insert(0, Selection.SelectedSymbol);
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
            MainWindow newMainWindow = new MainWindow()
            {
                Left = Left + Width * 1.3,
                Top = Top,
                Title = NewListName.Name,
                DataContext = new NewAllPrices(),
            };
            var a = new MainViewModel();

            //Binding binding = new Binding
            //{
            //    Source = a.SelectedSymbol,
            //};

            //newMainWindow.Selector.SetBinding(ListView.ItemsSourceProperty, binding);
            //newMainWindow.Selector.DataContext = new NewAllPrices();
            newMainWindow.Selector.ItemsSource = NewAllPrices.NewPrices;
            ContextMenuItem1.DataContext = new MainViewModel();
            newMainWindow.GoHome.Visibility = Visibility.Visible;
            //this.Visibility = Visibility.Hidden;
            //newMainWindow.Selector.ContextMenu.set = new MainViewModel();


            //NewAllPrices.NewPrices.Clear();


            //newMainWindow.Selector.ItemsSource = NewAllPrices.NewPrices;
            //NewAllPrices.NewPrices.Clear();
            //NewAllPrices.NewPrices.RemoveAt(0);
            //Console.WriteLine(Selection.SelectedSymbol.Symbol);
            //newMainWindow.DataContext = new NewAllPrices();
            //NewAllPrices.NewPrices.Clear();
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
