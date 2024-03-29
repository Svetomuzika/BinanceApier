﻿using BinanceAPI.Model;
using BinanceAPI.ViewModels;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BinanceAPI.View
{
    public partial class TradingUserControl : UserControl
    {
        public TradingUserControl()
        {
            InitializeComponent();
            OrdersList.SelectedItem = Selection.SelectedSymbol;
            TradesList.SelectedItem = Selection.SelectedSymbol;

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
            SizeBot.Visibility = Visibility.Hidden;
            SizeBotBlock.Visibility = Visibility.Hidden;
            DeltaBot.Visibility = Visibility.Hidden;
            DeltaBotBlock.Visibility = Visibility.Hidden;
            TimeBot.Visibility = Visibility.Hidden;
            TimeBotBlock.Visibility = Visibility.Hidden;
            BotPanel.Visibility = Visibility.Hidden;
            SmartDeltaBot.Visibility = Visibility.Hidden;
            SmartDeltaBotBlock.Visibility = Visibility.Hidden;
            SizeFirstBot.Visibility = Visibility.Hidden;
            SizeFirstBotBlock.Visibility = Visibility.Hidden;
            TimeFirstBot.Visibility = Visibility.Hidden;
            TimeFirstBotBlock.Visibility = Visibility.Hidden;
            BotFirstPanel.Visibility = Visibility.Hidden;


            Limit.Foreground = new BrushConverter().ConvertFromString("#f0b90b") as SolidColorBrush;
            Market.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            FirstBot.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            Bot.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
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
            SizeBot.Visibility = Visibility.Hidden;
            SizeBotBlock.Visibility = Visibility.Hidden;
            DeltaBot.Visibility = Visibility.Hidden;
            DeltaBotBlock.Visibility = Visibility.Hidden;
            TimeBot.Visibility = Visibility.Hidden;
            TimeBotBlock.Visibility = Visibility.Hidden;
            BotPanel.Visibility = Visibility.Hidden;
            SmartDeltaBot.Visibility = Visibility.Hidden;
            SmartDeltaBotBlock.Visibility = Visibility.Hidden;
            SizeFirstBot.Visibility = Visibility.Hidden;
            SizeFirstBotBlock.Visibility = Visibility.Hidden;
            TimeFirstBot.Visibility = Visibility.Hidden;
            TimeFirstBotBlock.Visibility = Visibility.Hidden;
            BotFirstPanel.Visibility = Visibility.Hidden;


            Market.Foreground = new BrushConverter().ConvertFromString("#f0b90b") as SolidColorBrush;
            Limit.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            Bot.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            FirstBot.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
        }

        private void Bot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PriceLimitBox.Visibility = Visibility.Hidden;
            AmountLimitBox.Visibility = Visibility.Hidden;
            AmountMarketBox.Visibility = Visibility.Hidden;
            PriceLimitBlock.Visibility = Visibility.Hidden;
            AmountLimitBlock.Visibility = Visibility.Hidden;
            AmountMarketBlock.Visibility = Visibility.Hidden;
            SellLimit.Visibility = Visibility.Hidden;
            BuyLimit.Visibility = Visibility.Hidden;
            SellMarket.Visibility = Visibility.Hidden;
            BuyMarket.Visibility = Visibility.Hidden;
            Cancel.Visibility = Visibility.Hidden;
            CancelMarket.Visibility = Visibility.Hidden;
            SizeFirstBot.Visibility = Visibility.Hidden;
            SizeFirstBotBlock.Visibility = Visibility.Hidden;
            TimeFirstBot.Visibility = Visibility.Hidden;
            TimeFirstBotBlock.Visibility = Visibility.Hidden;
            BotFirstPanel.Visibility = Visibility.Hidden;
            SizeBot.Visibility = Visibility.Visible;
            SizeBotBlock.Visibility = Visibility.Visible;
            DeltaBot.Visibility = Visibility.Visible;
            DeltaBotBlock.Visibility = Visibility.Visible;
            TimeBot.Visibility = Visibility.Visible;
            TimeBotBlock.Visibility = Visibility.Visible;
            BotPanel.Visibility = Visibility.Visible;
            SmartDeltaBot.Visibility = Visibility.Visible;
            SmartDeltaBotBlock.Visibility = Visibility.Visible;

            Market.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            Limit.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            Bot.Foreground = new BrushConverter().ConvertFromString("#f0b90b") as SolidColorBrush;
            FirstBot.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;

        }
        private void BotFirst_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PriceLimitBox.Visibility = Visibility.Hidden;
            AmountLimitBox.Visibility = Visibility.Hidden;
            AmountMarketBox.Visibility = Visibility.Hidden;
            PriceLimitBlock.Visibility = Visibility.Hidden;
            AmountLimitBlock.Visibility = Visibility.Hidden;
            AmountMarketBlock.Visibility = Visibility.Hidden;
            SellLimit.Visibility = Visibility.Hidden;
            BuyLimit.Visibility = Visibility.Hidden;
            SellMarket.Visibility = Visibility.Hidden;
            BuyMarket.Visibility = Visibility.Hidden;
            Cancel.Visibility = Visibility.Hidden;
            CancelMarket.Visibility = Visibility.Hidden;
            SizeFirstBot.Visibility = Visibility.Visible;
            SizeFirstBotBlock.Visibility = Visibility.Visible;
            TimeFirstBot.Visibility = Visibility.Visible;
            TimeFirstBotBlock.Visibility = Visibility.Visible;
            BotFirstPanel.Visibility = Visibility.Visible;
            SizeBot.Visibility = Visibility.Hidden;
            SizeBotBlock.Visibility = Visibility.Hidden;
            DeltaBot.Visibility = Visibility.Hidden;
            DeltaBotBlock.Visibility = Visibility.Hidden;
            TimeBot.Visibility = Visibility.Hidden;
            TimeBotBlock.Visibility = Visibility.Hidden;
            BotPanel.Visibility = Visibility.Hidden;
            SmartDeltaBot.Visibility = Visibility.Hidden;
            SmartDeltaBotBlock.Visibility = Visibility.Hidden;

            Market.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            Limit.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            Bot.Foreground = new BrushConverter().ConvertFromString("#7f8c9c") as SolidColorBrush;
            FirstBot.Foreground = new BrushConverter().ConvertFromString("#f0b90b") as SolidColorBrush;
        }

        private void Trades_Click(object sender, RoutedEventArgs e)
        {
            TradesList.Visibility = Visibility.Visible;
            OrdersList.Visibility = Visibility.Hidden;
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            TradesList.Visibility = Visibility.Hidden;
            OrdersList.Visibility = Visibility.Visible;
        }

        public void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var b = new MainViewModel();
            Button a = (Button)e.Source;

            b.CancellOne((long)a.Content);
        }
    }
}
