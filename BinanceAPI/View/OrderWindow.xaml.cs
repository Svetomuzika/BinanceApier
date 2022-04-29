using BinanceAPI.Model;
using BinanceAPI.ViewModels;
using System;
using System.Windows;


namespace BinanceAPI
{
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;
            SelectorNew1.SelectedItem = Selection.SelectedSymbol;

            CloseStream.CloseOrderStream = false;
            CloseStream.CloseLastTradeStream = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CloseStream.CloseOrderStream = true;
            CloseStream.CloseLastTradeStream = true;
        }
    }
}
