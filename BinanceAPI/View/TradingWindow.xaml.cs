using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System;
using System.Windows.Controls;
using BinanceAPI.ViewModels;
using System.Windows.Media;

namespace BinanceAPI
{
    public partial class TradingWindow : Window
    {
        public TradingWindow()
        {
            InitializeComponent();
            SelectorNew.SelectedItem = Selection.SelectedSymbol;

            Limit.Foreground = new BrushConverter().ConvertFromString("#f0b90b") as SolidColorBrush;
            AmountMarketBox.Visibility = Visibility.Hidden;
        }

        private void NumberValidationTextBoxAmountLimit(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
            if (!e.Handled)
            {
                e.Handled = AmountLimitBox.Text.Length > 4;
            }
        }

        private void NumberValidationTextBoxAmountMarket(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
            if (!e.Handled)
            {
                e.Handled = AmountMarketBox.Text.Length > 4;
            }
        }

        private void TextBlock_MouseLeftButtonDownLimit(object sender, MouseButtonEventArgs e)
        {
            AmountMarketBox.Visibility = Visibility.Hidden;

            PriceLimitBox.Visibility = Visibility.Visible;

            AmountLimitBox.Visibility = Visibility.Visible;

            PriceLimitBlock.Visibility = Visibility.Visible;

            AmountLimitBlock.Visibility = Visibility.Visible;

            AmountMarketBlock.Visibility = Visibility.Hidden;

            SellLimit.Visibility = Visibility.Visible;

            BuyLimit.Visibility = Visibility.Visible;

            SellMarket.Visibility = Visibility.Hidden;

            BuyMarket.Visibility = Visibility.Hidden;

            Cancel.Visibility = Visibility.Visible;

            CancelMarket.Visibility = Visibility.Hidden;

            Limit.Foreground = new BrushConverter().ConvertFromString("#f0b90b") as SolidColorBrush;

            Market.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
        }
        private void TextBlock_MouseLeftButtonDownMarket(object sender, MouseButtonEventArgs e)
        {
            PriceLimitBox.Visibility = Visibility.Hidden;

            AmountLimitBox.Visibility = Visibility.Hidden;

            AmountMarketBox.Visibility = Visibility.Visible;

            PriceLimitBlock.Visibility = Visibility.Hidden;

            AmountLimitBlock.Visibility = Visibility.Hidden;

            AmountMarketBlock.Visibility = Visibility.Visible;

            SellLimit.Visibility = Visibility.Hidden;

            BuyLimit.Visibility = Visibility.Hidden;

            SellMarket.Visibility = Visibility.Visible;

            BuyMarket.Visibility = Visibility.Visible;

            Cancel.Visibility = Visibility.Hidden;

            CancelMarket.Visibility = Visibility.Visible;

            Market.Foreground = new BrushConverter().ConvertFromString("#f0b90b") as SolidColorBrush;

            Limit.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
        }

    }
}
