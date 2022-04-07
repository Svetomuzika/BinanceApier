using BinanceAPI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BinanceAPI
{
    static class Slection
    {
        public static BinanceSymbolViewModel SelectedSymbol { get; set; }
    }

    public partial class MainWindow : Window

    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void listView_Click(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                new TradeWindow { Left = Left + Width * 1.01, Top = Top }.Show();
                new OrderWindow { Left = Left + Width * 2.22, Top = Top }.Show();
            }
        }
    }
}
