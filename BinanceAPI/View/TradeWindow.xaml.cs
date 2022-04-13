using System;
using System.Linq;
using System.Windows;
using BinanceAPI.ViewModels;

namespace BinanceAPI
{ 
    public partial class TradeWindow : Window
    {
        MainViewModel mainViewModel = new MainViewModel();

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
        }
    }
}
