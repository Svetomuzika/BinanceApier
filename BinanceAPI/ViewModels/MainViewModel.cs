using Binance.Net.Clients;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BinanceApi.MVVM;
using BinanceApi.ViewModels;
using System.Windows;
using System.Windows.Input;
using BinanceAPI.MVVM;
using System.Threading;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Binance.Net.Enums;
using CryptoExchange.Net.Sockets;
using Binance.Net.Objects.Models.Spot.Socket;
using BinanceAPI.Model;

namespace BinanceAPI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private BinanceSocketClient socketClient;
        private BinanceClient client;

        private BinanceClient binanceClient;
        private BinanceSocketClient binanceSocketClient;

        public string searchMain;
        public string SearchMain
        {
            get { return searchMain; }
            set
            {
                searchMain = value;
                GetSearch(value);
                RaisePropertyChangedEvent("SearchMain");
            }
        }

        public ObservableCollection<BinanceSymbolViewModel> FakeSymbol;

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
                Selection.SelectedSymbol = value;
                RaisePropertyChangedEvent("SymbolIsSelected");
                RaisePropertyChangedEvent("SelectedSymbol");
                ChangeSymbol();
            }
        }

        private string listName;

        public string ListName
        {
            get { return listName; }
            set
            {
                listName = value;
                NewListName.Name = value;
                RaisePropertyChangedEvent("ListName");
            }
        }

        public ICommand CallTradeStreamCommand { get; set; }
        public ICommand CallOrderStreamCommand { get; set; }
        public ICommand CallAggTradeStreamCommand { get; set; }
        public ICommand AllCancelCommand { get; set; }
        public ICommand BuyCommandLimit { get; set; }
        public ICommand SellCommandLimit { get; set; }
        public ICommand BuyCommandMarket { get; set; }
        public ICommand SellCommandMarket { get; set; }
        public ICommand GetTradesCommand { get; set; }

        public bool SymbolIsSelected
        {
            get { return SelectedSymbol != null; }
        }

        public MainViewModel()
        {
            Task.Run(async () => await Task.WhenAll(GetNewSymbols(), GetAllSymbols()));

            binanceClient = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials("AU8ovgGXuLShglvZLyoEcjE3MrE7RaH3PPoESHRX4lrztsAdtvpfYSjXqkfhwogD",
                        "2Pf23BjqUErU79ZMOrMNd5CRJEsA2PhD3U8HRGUMUmgZe3mDtGgZCeQWXlkSlgbh"),
                SpotApiOptions = new BinanceApiClientOptions
                {
                    BaseAddress = BinanceApiAddresses.TestNet.RestClientAddress
                }
            });

            binanceSocketClient = new BinanceSocketClient(new BinanceSocketClientOptions
            {
                ApiCredentials = new ApiCredentials("AU8ovgGXuLShglvZLyoEcjE3MrE7RaH3PPoESHRX4lrztsAdtvpfYSjXqkfhwogD",
                    "2Pf23BjqUErU79ZMOrMNd5CRJEsA2PhD3U8HRGUMUmgZe3mDtGgZCeQWXlkSlgbh"),
                SpotStreamsOptions = new BinanceApiClientOptions
                {
                    BaseAddress = BinanceApiAddresses.TestNet.SocketClientAddress
                }
            });

            CallTradeStreamCommand = new DelegateCommand(async (o) => await CallTradeStream(o));
            CallOrderStreamCommand = new DelegateCommand(async (o) => await CallOrderStream(o));
            CallAggTradeStreamCommand = new DelegateCommand(async (o) => await CallAggTradeStream(o));
            BuyCommandLimit = new DelegateCommand(async (o) => await BuyLimit(o));
            SellCommandLimit = new DelegateCommand(async (o) => await SellLimit(o));
            BuyCommandMarket = new DelegateCommand(async (o) => await BuyMarket(o));
            SellCommandMarket = new DelegateCommand(async (o) => await SellMarket(o));
            AllCancelCommand = new DelegateCommand(async (o) => await AllCancel(o));
            GetTradesCommand = new DelegateCommand(async (o) => await GetTrades());
        }

        private async Task GetAllSymbols()
        {
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

        private async Task GetNewSymbols()
        {
            client = new BinanceClient();
            var result = await client.SpotApi.ExchangeData.GetPricesAsync();
            
            AllPrices = new ObservableCollection<BinanceSymbolViewModel>(result.Data.Select(r => new BinanceSymbolViewModel(r.Symbol, r.Price)).ToList().OrderByDescending(p => p.Price));
            FakeSymbol = new ObservableCollection<BinanceSymbolViewModel>(result.Data.Select(r => new BinanceSymbolViewModel(r.Symbol, r.Price)).ToList().OrderByDescending(p => p.Price));
        }


        public async Task GetTradeStream()
        {
            SelectedSymbol = Selection.SelectedSymbol;

            client = new BinanceClient();
            socketClient = new BinanceSocketClient();
            
            var result = await client.SpotApi.ExchangeData.GetRecentTradesAsync(SelectedSymbol.Symbol, 1000);
            SelectedSymbol.Trades = new ObservableCollection<TradeViewModel>(result.Data.Select(r => new TradeViewModel(r.Price, r.BaseQuantity, r.TradeTime, r.BuyerIsMaker)).Reverse().ToList());
            var mainSymbol = SelectedSymbol;

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            var subscribeResult = await socketClient.SpotStreams.SubscribeToTradeUpdatesAsync(SelectedSymbol.Symbol, data =>
            {
                var symbol = mainSymbol;
                //var symbol = AllPrices.SingleOrDefault(a => a.Symbol == mainSymbol.Symbol);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    symbol.AddTrade(new TradeViewModel(data.Data.Price, data.Data.Quantity, data.Data.TradeTime, data.Data.BuyerIsMaker));

                    if (CloseStream.ClosedStream)
                    {
                        if(CloseStream.ClosedWindowName.Contains(symbol.Symbol))
                        {
                            cancelTokenSource.Cancel();
                            cancelTokenSource.Dispose();
                        }
                    }
                });
            }, token);
        }

        public async Task GetAggTradeStream()
        {
            SelectedSymbol = Selection.SelectedSymbol;

            client = new BinanceClient();
            socketClient = new BinanceSocketClient();

            var result = await client.SpotApi.ExchangeData.GetAggregatedTradeHistoryAsync(SelectedSymbol.Symbol, limit: 1000);
            SelectedSymbol.AggTrades = new ObservableCollection<TradeViewModel>(result.Data.Select(r => new TradeViewModel(r.Price, r.Quantity, r.TradeTime, r.BuyerIsMaker)).Reverse().ToList());
            var mainSymbol = SelectedSymbol;

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            var subscribeResult = await socketClient.SpotStreams.SubscribeToAggregatedTradeUpdatesAsync(SelectedSymbol.Symbol, data =>
            {
                var symbol = mainSymbol;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    symbol.AddAggTrade(new TradeViewModel(data.Data.Price, data.Data.Quantity, data.Data.TradeTime, data.Data.BuyerIsMaker));

                    if (CloseStream.ClosedStream)
                    {
                        if (CloseStream.ClosedWindowName.Contains(symbol.Symbol))
                        {
                            cancelTokenSource.Cancel();
                            cancelTokenSource.Dispose();
                        }
                    }
                });
            }, token);
        }

        public async Task GetOrderStreamAsks()
        {
            var mainSymbol = SelectedSymbol.Symbol;
            client = new BinanceClient();
            socketClient = new BinanceSocketClient();

            var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(SelectedSymbol.Symbol, 15);
            AllOrdersAsks = new ObservableCollection<OrderViewModel>(result.Data.Asks.Reverse().Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList());

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            var subscribeResultAsks = await socketClient.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(SelectedSymbol.Symbol, 20, 1000, data =>
            {

                var newArr = data.Data.Asks.Select(r => new OrderViewModel(r.Price, r.Quantity)).Reverse().ToList();
                for (var i = 0; i < 15; i++)
                {
                    AllOrdersAsks[i].OrderPrice = newArr[i + 5].OrderPrice;
                    AllOrdersAsks[i].OrderQPrice = Math.Round(newArr[i + 5].OrderQPrice, 5);

                    var sum = Math.Round(newArr[i+5].OrderPrice * newArr[i + 5].OrderQPrice, 5).ToString();
                    var b = 14 - sum.Length;
                    for (int e = 1; e <= b; e++)
                    {
                        sum = "  " + sum;
                    }

                    sum = sum.Replace(",", ".");

                    AllOrdersAsks[i].OrderSum = sum;
                }

                if (CloseStream.ClosedStream)
                {
                    if (CloseStream.ClosedWindowName.Contains(mainSymbol))
                    {
                        cancelTokenSource.Cancel();
                        cancelTokenSource.Dispose();
                    }
                }
            }, token);
        }

        public async Task GetOrderStreamBids()
        {
            var mainSymbol = SelectedSymbol.Symbol;
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

                if (CloseStream.ClosedStream)
                {
                    if (CloseStream.ClosedWindowName.Contains(mainSymbol))
                    {
                        cancelTokenSource.Cancel();
                        cancelTokenSource.Dispose();
                    }
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
                var oldBest = Math.Round(newBest, 2);
                mainSymbol.OrderBestPrice = oldBest.ToString().Replace(",", ".").Insert(2, ",");

                sum = oldBest;
            });
        }

        public async Task TradingStream()
        {
            var startUserStreamAsync = await binanceClient.SpotApi.Account.StartUserStreamAsync();

            var lastPriceTest = await binanceClient.SpotApi.ExchangeData.GetPriceAsync(SelectedSymbol.Symbol);
            SelectedSymbol.TradingPrice = lastPriceTest.Data.Price;

            var mainSymbol = SelectedSymbol;

            var subForAccUpdates = await binanceSocketClient.SpotStreams.SubscribeToUserDataUpdatesAsync(startUserStreamAsync.Data, OnOrderUpdate, null, null, onAccountBalanceUpdate: data =>
            {
                //
            });
        }

        public async Task BuyLimit(object o)
        {

            var result = await binanceClient.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Buy, SpotOrderType.Limit, SelectedSymbol.TradingAmount, price: SelectedSymbol.TradingPrice, timeInForce: TimeInForce.GoodTillCanceled);
            if (result.Success)
                Console.WriteLine("Succes!!! BuyLimit");
        }

        public async Task SellLimit(object o)
        {

            var result = await binanceClient.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Sell, SpotOrderType.Limit, SelectedSymbol.TradingAmount, price: SelectedSymbol.TradingPrice, timeInForce: TimeInForce.GoodTillCanceled);
            if (result.Success)
                Console.WriteLine("Succes!!! SellLimit");
        }
        public async Task BuyMarket(object o)
        {

            var result = await binanceClient.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Buy, SpotOrderType.Market, SelectedSymbol.TradingAmountMarket);
            if (result.Success)
                Console.WriteLine("Succes!!! BuyMarket");
        }

        public async Task SellMarket(object o)
        {

            var result = await binanceClient.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Sell, SpotOrderType.Market, SelectedSymbol.TradingAmountMarket);
            if (result.Success)
                Console.WriteLine("Succes!!! SellMarket");
        }

        public async Task AllCancel(object o)
        {

            var result = await binanceClient.SpotApi.Trading.CancelAllOrdersAsync(SelectedSymbol.Symbol);
            if (result.Success)
                Console.WriteLine("Canceled!!!");
        }

        public async Task Cancel(long id)
        {
            var result = await binanceClient.SpotApi.Trading.CancelOrderAsync(Selection.SelectedSymbol.Symbol, id);
            if (result.Success)
                Console.WriteLine("OneCanceled!!!");
        }

        private void OnOrderUpdate(DataEvent<BinanceStreamOrderUpdate> data)
        {
            var orderUpdate = data.Data;
            var symbol = AllPrices.SingleOrDefault(a => a.Symbol == orderUpdate.Symbol);
            var price = orderUpdate.Price;

            if (orderUpdate.Type.ToString() == "Market")
            {
                if (data.Data.Status.ToString() == "Filled")
                {


                    price = orderUpdate.LastPriceFilled;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        symbol.AddTradingTrades(new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                    });
                }

                Task.Run(() => GetBalance());
                return;
            }

            if (data.Data.Status.ToString() == "Filled")
            {
                var id = data.Data.Id;
                symbol.TradingOrders = new ObservableCollection<TradingOrdersViewModel>(symbol.TradingOrders.Where(a => a.Id != id).Select(o => new TradingOrdersViewModel(o.ExecutedQuantity, o.Price, o.Side, o.Status, o.Symbol, o.DateTime, o.Id)));

                Application.Current.Dispatcher.Invoke(() =>
                {
                    symbol.AddTradingTrades(new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                });
            }
            else if (data.Data.Status.ToString() != "Canceled")
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    symbol.AddTradingOrders(new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                });
            }
            else
            {
                var id = data.Data.Id;
                symbol.TradingOrders = new ObservableCollection<TradingOrdersViewModel>(symbol.TradingOrders.Where(a => a.Id != id).Select(o => new TradingOrdersViewModel(o.ExecutedQuantity, o.Price, o.Side, o.Status, o.Symbol, o.DateTime, o.Id)));
            }

            Task.Run(() => GetBalance());
        }

        private async Task GetTrades()
        {
            var result = await binanceClient.SpotApi.Trading.GetOrdersAsync(SelectedSymbol.Symbol);


            SelectedSymbol.TradingOrders = new ObservableCollection<TradingOrdersViewModel>(result.Data.OrderByDescending(d => d.CreateTime).Where(a => a.Status.ToString() != "Filled" && a.Status.ToString() != "Canceled").Select(o => new TradingOrdersViewModel(o.QuantityFilled, o.Price, o.Side, o.Status, o.Symbol, o.CreateTime, o.Id)));

            SelectedSymbol.TradingTrades = new ObservableCollection<TradingOrdersViewModel>(result.Data.OrderByDescending(d => d.CreateTime).Where(a => a.Status.ToString() == "Filled").Select(o => new TradingOrdersViewModel(o.QuantityFilled, o.Price, o.Side, o.Status, o.Symbol, o.CreateTime, o.Id)));
        }


        public void ChangeSymbol()
        {
            if (SelectedSymbol != null)
            {
                selectedSymbol.TradingAmount = 0;
                Task.Run(async () => await Task.WhenAll(GetOrderStreamAsks(), GetOrderStreamBids(), GetTrades(), TradingStream(), GetBalance()));
            }
        }

        private async Task GetBalance()
        {
            var info = await binanceClient.SpotApi.Account.GetAccountInfoAsync();

            SelectedSymbol.Balance = info.Data.Balances.SingleOrDefault(r => r.Asset == "USDT").Total;
        }

        private void GetSearch(string name)
        {
            if (name != "")
            {
                AllPrices.Clear();

                foreach (var e in FakeSymbol)
                {
                    if (e.Symbol.Contains(name.ToUpper()))
                    {
                        AllPrices.Add(e);
                    }
                }
            }
            else if (name == "" || name == null)
            {
               Task.Run(() => GetNewSymbols());
            }
        }

        public async Task CallTradeStream(object o)
        {
            await GetTradeStream();
        }

        public async Task CallOrderStream(object o)
        {
            await GetLastTrade();
        }

        public async Task CallAggTradeStream(object o)
        {
            await GetAggTradeStream();
        }
    }
}

