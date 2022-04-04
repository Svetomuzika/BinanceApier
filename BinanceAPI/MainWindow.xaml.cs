using Binance.Net.Clients;
using BinanceAPI.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BinanceAPI
{
    static class D
    {
        public static BinanceSymbolViewModel SelectedSymbol { get; set; }
    }

    public partial class MainWindow : Window

    {
        public MainWindow()
        {

            InitializeComponent();

            //BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            //{
            //    ApiCredentials = new ApiCredentials("2SkyqmAWoi9rMMPYhvESNZgjLIw8TvbizPA9leyZWcxQugg97QtjZH1zJCBhGTTn",
            //    "N8AILITZrI4J0VA1p7sQwK49yRIdaQfdDe5ip1NV0VOz9nMXrO7S1Vw7fZidCpzV"),
            //    LogLevel = LogLevel.Debug
            //});

            //BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            //{
            //    ApiCredentials = new ApiCredentials("2SkyqmAWoi9rMMPYhvESNZgjLIw8TvbizPA9leyZWcxQugg97QtjZH1zJCBhGTTn",
            //    "N8AILITZrI4J0VA1p7sQwK49yRIdaQfdDe5ip1NV0VOz9nMXrO7S1Vw7fZidCpzV"),
            //    LogLevel = LogLevel.Debug
            //});

            //Method();

        }


        private void listView_Click(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                new NewWindow { Left = this.Left + this.Width * 1.01, Top = this.Top }.Show();
            }
        }

        //void Method()
        //{
        //    var client = new BinanceClient();
        //var socketClient = new BinanceSocketClient();
        //await HandleRequest("Symbol list", () => client.SpotApi.ExchangeData.GetExchangeInfoAsync(), result => string.Join(", ", result.Symbols.Select(s => s.Name).Take(10)) + " etc");
        //await HandleRequest("ETHUSDT 24h change", () => client.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT"), result => $"Change: {result.PriceChange}, Change percentage: {result.PriceChangePercent}");

        //var startResult = await client.SpotApi.Account.StartUserStreamAsync();




        //await HandleRequest("BTCUSDT book price", () => socketClient.SpotApi.ExchangeData.GetBookPriceAsync("BTCUSDT"), result => $"Best Ask: {result.BestAskPrice}, Best Bid: {result.BestBidPrice}");

        //await HandleRequest("BTCUSDT book price", () => client.SpotApi.ExchangeData.GetBookPriceAsync("BTCUSDT"), result => $"Best Ask: {result.BestAskPrice}, Best Bid: {result.BestBidPrice}");
        //textBox.Text = Str;

        //await HandleRequest("ETHUSDT book price", () => client.SpotApi.ExchangeData.GetBookPriceAsync("ETHUSDT"), result => $"Best Ask: {result.BestAskPrice}, Best Bid: {result.BestBidPrice}");
        //textBox1.Text = Str;
        //}


        //static async Task HandleRequest<T>(string action, Func<Task<WebCallResult<T>>> request, Func<T, string> outputData)
        //{
        //    var bookPrices = await request();
        //    if (bookPrices.Success)
        //        Str = $"{action}: " + outputData(bookPrices.Data);
        //    else
        //        Str = $"Failed to retrieve data: {bookPrices.Error}";
        //}
        //}
    }
}
