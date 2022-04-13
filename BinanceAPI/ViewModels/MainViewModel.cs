using Binance.Net.Clients;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BinanceApi.MVVM;
using BinanceApi.ViewModels;
using System.Globalization;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using BinanceAPI.MVVM;
using System.Threading;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Binance.Net.Enums;

namespace BinanceAPI.ViewModels
{
    static class CloseStream
    {
        public static bool CloseAggTradeStream = false;
        public static bool CloseTradeStream = false;
        public static bool CloseOrderStream = false;
        public static bool CloseLastTradeStream = false;
    }

    public class MainViewModel : ObservableObject
    {
        private BinanceSocketClient socketClient;
        private BinanceClient client;

        private BinanceClient binanceClient;

        private ObservableCollection<BinanceSymbolViewModel> allPrices;

        public ObservableCollection<BinanceSymbolViewModel> AllPrices
        {
            get { return allPrices; }
            set
            {
                allPrices = value;
                RaisePropertyChangedEvent("AllPrices");
            }
        }

        private ObservableCollection<TradeViewModel> allTrades;

        public ObservableCollection<TradeViewModel> AllTrades
        {
            get { return allTrades; }
            set
            {
                allTrades = value;
                RaisePropertyChangedEvent("AllTrades");
            }
        }

        private ObservableCollection<OrderViewModel> allOrdersBids;

        public ObservableCollection<OrderViewModel> AllOrdersBids
        {
            get { return allOrdersBids; }
            set
            {
                allOrdersBids = value;
                RaisePropertyChangedEvent("AllOrdersBids");
            }
        }

        private ObservableCollection<OrderViewModel> allOrdersAsks;

        public ObservableCollection<OrderViewModel> AllOrdersAsks
        {
            get { return allOrdersAsks; }
            set
            {
                allOrdersAsks = value;
                RaisePropertyChangedEvent("AllOrdersAsks");
            }
        }


        private BinanceSymbolViewModel selectedSymbol;
        public BinanceSymbolViewModel SelectedSymbol
        {
            get { return selectedSymbol; }
            set
            {
                selectedSymbol = value;
                Selection.SelectedSymbol = selectedSymbol;
                RaisePropertyChangedEvent("SymbolIsSelected");
                RaisePropertyChangedEvent("SelectedSymbol");
                ChangeSymbol();
            }
        }

        public ICommand CallTradeStreamCommand { get; set; }
        public ICommand CallOrderStreamCommand { get; set; }
        public ICommand CallAggTradeStreamCommand { get; set; }
        public ICommand BuyCommandLimit { get; set; }
        public ICommand SellCommandLimit { get; set; }
        public ICommand BuyCommandMarket { get; set; }
        public ICommand SellCommandMarket { get; set; }

        public bool SymbolIsSelected
        {
            get { return SelectedSymbol != null; }
        }

        public MainViewModel()
        {
            Task.Run(() => GetAllSymbols());

            binanceClient = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials("AU8ovgGXuLShglvZLyoEcjE3MrE7RaH3PPoESHRX4lrztsAdtvpfYSjXqkfhwogD",
                        "2Pf23BjqUErU79ZMOrMNd5CRJEsA2PhD3U8HRGUMUmgZe3mDtGgZCeQWXlkSlgbh"),
                SpotApiOptions = new BinanceApiClientOptions
                {
                    BaseAddress = BinanceApiAddresses.TestNet.RestClientAddress
                }
            });

            CallTradeStreamCommand = new DelegateCommand(async (o) => await CallTradeStream(o));
            CallOrderStreamCommand = new DelegateCommand(async (o) => await CallOrderStream(o));
            CallAggTradeStreamCommand = new DelegateCommand(async (o) => await CallAggTradeStream(o));
            BuyCommandLimit = new DelegateCommand(async (o) => await BuyLimit(o));
            SellCommandLimit = new DelegateCommand(async (o) => await SellLimit(o));
            BuyCommandMarket = new DelegateCommand(async (o) => await BuyMarket(o));
            SellCommandMarket = new DelegateCommand(async (o) => await SellMarket(o));
        }

        private async Task GetAllSymbols()
        {
            client = new BinanceClient();
            var result = await client.SpotApi.ExchangeData.GetPricesAsync();
            AllPrices = new ObservableCollection<BinanceSymbolViewModel>(result.Data.Select(r => new BinanceSymbolViewModel(r.Symbol, r.Price)).ToList().Take(40).OrderByDescending(p => p.Price));

            socketClient = new BinanceSocketClient();
            var subscribeResult = await socketClient.SpotStreams.SubscribeToAllTickerUpdatesAsync(data =>
            {
                foreach (var ud in data.Data)
                {
                    var symbol = AllPrices.SingleOrDefault(p => p.Symbol == ud.Symbol);
                    if (symbol != null)
                    {
                        symbol.Price = ud.LastPrice;
                    }
                }
            });
        }

        public async Task GetTradeStream()
        {
            client = new BinanceClient();
            socketClient = new BinanceSocketClient();

            var result = await client.SpotApi.ExchangeData.GetRecentTradesAsync(SelectedSymbol.Symbol, 1000);
            SelectedSymbol.Trades = new ObservableCollection<TradeViewModel>(result.Data.Select(r => new TradeViewModel(r.Price, r.BaseQuantity, r.TradeTime, r.BuyerIsMaker)).Reverse().ToList());
            var mainSymbol = SelectedSymbol;

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            var subscribeResult = await socketClient.SpotStreams.SubscribeToTradeUpdatesAsync(SelectedSymbol.Symbol, data =>
            {
                var symbol = AllPrices.SingleOrDefault(a => a.Symbol == mainSymbol.Symbol);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    symbol.AddTrade(new TradeViewModel(data.Data.Price, data.Data.Quantity, data.Data.TradeTime, data.Data.BuyerIsMaker));

                    if (CloseStream.CloseTradeStream)
                    {
                        cancelTokenSource.Cancel();
                        cancelTokenSource.Dispose();
                    }
                });
            }, token);
        }

        public async Task GetAggTradeStream()
        {
            client = new BinanceClient();
            socketClient = new BinanceSocketClient();

            var result = await client.SpotApi.ExchangeData.GetAggregatedTradeHistoryAsync(SelectedSymbol.Symbol, limit: 1000);
            SelectedSymbol.AggTrades = new ObservableCollection<TradeViewModel>(result.Data.Select(r => new TradeViewModel(r.Price, r.Quantity, r.TradeTime, r.BuyerIsMaker)).Reverse().ToList());
            var mainSymbol = SelectedSymbol;

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            var subscribeResult = await socketClient.SpotStreams.SubscribeToAggregatedTradeUpdatesAsync(SelectedSymbol.Symbol, data =>
            {
                var symbol = AllPrices.SingleOrDefault(a => a.Symbol == mainSymbol.Symbol);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    symbol.AddAggTrade(new TradeViewModel(data.Data.Price, data.Data.Quantity, data.Data.TradeTime, data.Data.BuyerIsMaker));

                    if (CloseStream.CloseAggTradeStream)
                    {
                        cancelTokenSource.Cancel();
                        cancelTokenSource.Dispose();
                    }
                });
            }, token);
        }

        public async Task GetOrderStreamAsks()
        {
            client = new BinanceClient();
            socketClient = new BinanceSocketClient();

            var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(SelectedSymbol.Symbol, 15);
            AllOrdersAsks = new ObservableCollection<OrderViewModel>(result.Data.Asks.Reverse().Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList());

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            var best = Math.Round(result.Data.Bids.First().Price, 2);
            SelectedSymbol.OrderBestPrice = best.ToString().Replace(",", ".").Insert(2, ",");
            SelectedSymbol.SumColor = "white";

            var subscribeResultAsks = await socketClient.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(SelectedSymbol.Symbol, 20, 1000, data =>
            {
                var newArr = data.Data.Asks.Select(r => new OrderViewModel(r.Price, r.Quantity)).Reverse().ToList();
                for (var i = 0; i < 15; i++)
                {
                    AllOrdersAsks[i].OrderPrice = newArr[i + 5].OrderPrice;
                    AllOrdersAsks[i].OrderQPrice = Math.Round(newArr[i+5].OrderQPrice, 5);

                    var sum = Math.Round(newArr[i+5].OrderPrice * newArr[i+5].OrderQPrice, 5).ToString();
                    var b = 14 - sum.Length;
                    for (int e = 1; e <= b; e++)
                    {
                        sum = "  " + sum;
                    }

                    sum = sum.Replace(",", ".");

                    AllOrdersAsks[i].OrderSum = sum;
                }

                if (CloseStream.CloseOrderStream)
                {
                    cancelTokenSource.Cancel();
                    cancelTokenSource.Dispose();
                }
            }, token);
        }

        public async Task GetOrderStreamBids()
        {
            client = new BinanceClient();
            socketClient = new BinanceSocketClient();

            var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(SelectedSymbol.Symbol, 15);
            AllOrdersBids = new ObservableCollection<OrderViewModel>(result.Data.Bids.Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList());

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            var subscribeResultBids = await socketClient.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(SelectedSymbol.Symbol, 20, 1000, data =>
            {
                var newArr = data.Data.Bids.OrderByDescending(p => p.Price).Where(p => p.Quantity != 0).Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList();
                
                for (var i = 0; i < 15; i++)
                {
                    AllOrdersBids[i].OrderPrice = newArr[i].OrderPrice;
                    AllOrdersBids[i].OrderQPrice = Math.Round(newArr[i].OrderQPrice, 5);

                    var sum = Math.Round(newArr[i].OrderPrice * newArr[i].OrderQPrice, 5).ToString();
                    var b = 14 - sum.Length;
                    for (int e = 1; e <= b; e++)
                    {
                        sum = "  " + sum;
                    }

                    AllOrdersBids[i].OrderSum = sum.Replace(",", ".");
                }

                if (CloseStream.CloseOrderStream)
                {
                    cancelTokenSource.Cancel();
                    cancelTokenSource.Dispose();
                }
            }, token);
        }

        private async Task GetLastTrade()
        {
            socketClient = new BinanceSocketClient();
            var mainSymbol = SelectedSymbol;
            decimal sum = 0;

            var subscribeResult = await socketClient.SpotStreams.SubscribeToTradeUpdatesAsync(SelectedSymbol.Symbol, data =>
            {
                var symbol = AllPrices.SingleOrDefault(a => a.Symbol == mainSymbol.Symbol);

                var newBest = data.Data.Price;

                if (newBest == sum)
                {
                    mainSymbol.SumColor = "white";
                }
                else
                {
                    mainSymbol.SumColor = newBest > sum ? "#009900" : "red";
                }

                Thread.Sleep(300);
                var oldBest = Math.Round(newBest, 2);
                mainSymbol.OrderBestPrice = oldBest.ToString().Replace(",", ".").Insert(2, ",");

                sum = oldBest;
            });
        }

        public async Task TradingStream()
        {

            var result = await binanceClient.SpotApi.Account.GetAccountInfoAsync();
            var a = result.Data.Balances.SingleOrDefault(r => r.Asset == "BTC");
            Console.WriteLine(a.Total);
        }

        public async Task BuyLimit(object o)
        {
                var result = await client.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Buy, SpotOrderType.Limit, SelectedSymbol.TradeAmount, price: SelectedSymbol.TradePrice, timeInForce: TimeInForce.GoodTillCanceled);
        }

        public async Task SellLimit(object o)
        {
                var result = await client.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Sell, SpotOrderType.Limit, SelectedSymbol.TradeAmount, price: SelectedSymbol.TradePrice, timeInForce: TimeInForce.GoodTillCanceled);
        }
        public async Task BuyMarket(object o)
        {
            var result = await client.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Buy, SpotOrderType.Market, SelectedSymbol.TradeAmount, price: SelectedSymbol.TradePrice, timeInForce: TimeInForce.GoodTillCanceled);
        }

        public async Task SellMarket(object o)
        {
            var result = await client.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Sell, SpotOrderType.Market, SelectedSymbol.TradeAmount, price: SelectedSymbol.TradePrice, timeInForce: TimeInForce.GoodTillCanceled);
        }

        public void ChangeSymbol()
        {
            if (SelectedSymbol != null)
            {
                Task.Run(async () => await Task.WhenAll(GetOrderStreamAsks(), GetOrderStreamBids(), TradingStream()));
            }
        }

        private async Task CallTradeStream(object o)
        {
            await GetTradeStream();
        }

        private async Task CallOrderStream(object o)
        {
            await GetLastTrade();
        }

        private async Task CallAggTradeStream(object o)
        {
            await GetAggTradeStream();
        }
    }
}

