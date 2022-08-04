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


        public BinanceClient binanceClient;
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

        private ObservableCollection<TradingOrdersViewModel> tradingAllOrders;
        public ObservableCollection<TradingOrdersViewModel> TradingAllOrders
        {
            get { return tradingAllOrders; }
            set
            {
                tradingAllOrders = value;
                RaisePropertyChangedEvent("TradingAllOrders");
            }
        }

        private ObservableCollection<BinanceSymbolViewModel> clients;
        public ObservableCollection<BinanceSymbolViewModel> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                RaisePropertyChangedEvent("Clients");
            }
        }

        private ObservableCollection<TradingOrdersViewModel> tradingAllTrders;

        public ObservableCollection<TradingOrdersViewModel> TradingAllTrades
        {
            get { return tradingAllTrders; }
            set
            {
                tradingAllTrders = value;
                RaisePropertyChangedEvent("TradingAllTrades");
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

        private string connectionStatus = "Offline";

        public string ConnectionStatus
        {
            get { return connectionStatus; }
            set
            {
                connectionStatus = value;
                RaisePropertyChangedEvent("ConnectionStatus");
            }
        }

        private string connectionStatusColor = "red";

        public string ConnectionStatusColor
        {
            get { return connectionStatusColor; }
            set
            {
                connectionStatusColor = value;
                RaisePropertyChangedEvent("ConnectionStatusColor");
            }
        }

        private decimal Full = 0;

        public ICommand CallTradeStreamCommand { get; set; }
        public ICommand CallTradeHistoryCommand { get; set; }
        public ICommand CallOrderStreamCommand { get; set; }
        public ICommand CallAggTradeStreamCommand { get; set; }
        public ICommand AllCancelCommand { get; set; }
        public ICommand BuyCommandLimit { get; set; }
        public ICommand SellCommandLimit { get; set; }
        public ICommand BuyCommandMarket { get; set; }
        public ICommand SellCommandMarket { get; set; }
        public ICommand GetTradesCommand { get; set; }
        public ICommand CallTradingStreamCommand { get; set; }
        public ICommand StartBotLimitCommand { get; set; }
        public ICommand StartBotFirstCommand { get; set; }
        public ICommand StopBotCommand { get; set; }
        public ICommand CallTProperties { get; set; }
        public ICommand FilterTrades { get; set; }
        public ICommand AccountInfo { get; set; }

        public bool SymbolIsSelected
        {
            get { return SelectedSymbol != null; }
        }

        string testKey1 = "AU8ovgGXuLShglvZLyoEcjE3MrE7RaH3PPoESHRX4lrztsAdtvpfYSjXqkfhwogD";
        string testKey2 = "2Pf23BjqUErU79ZMOrMNd5CRJEsA2PhD3U8HRGUMUmgZe3mDtGgZCeQWXlkSlgbh";

        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public MainViewModel()
        {
            binanceClient = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials(testKey1,
                        testKey2),
                SpotApiOptions = new BinanceApiClientOptions
                {
                    BaseAddress = BinanceApiAddresses.TestNet.RestClientAddress
                }
            });

            binanceSocketClient = new BinanceSocketClient(new BinanceSocketClientOptions
            {
                ApiCredentials = new ApiCredentials(testKey1,
                        testKey2),
                SpotStreamsOptions = new BinanceApiClientOptions
                {
                    BaseAddress = BinanceApiAddresses.TestNet.SocketClientAddress
                }
            });

            Task.Run(async () => await Task.WhenAll(GetNewSymbols(), GetAllSymbols(), GetAllOrders(), TradingStream(), GetAllClients(), GetProperties()));

            CallTradeStreamCommand = new DelegateCommand(async (o) => await GetTradeStream());
            CallOrderStreamCommand = new DelegateCommand(async (o) => await CallOrderStream());
            CallAggTradeStreamCommand = new DelegateCommand(async (o) => await GetAggTradeStream());
            BuyCommandLimit = new DelegateCommand(async (o) => await BuyLimit(o));
            SellCommandLimit = new DelegateCommand(async (o) => await SellLimit(o));
            BuyCommandMarket = new DelegateCommand(async (o) => await BuyMarket(o));
            SellCommandMarket = new DelegateCommand(async (o) => await SellMarket(o));
            AllCancelCommand = new DelegateCommand(async (o) => await AllCancel(o));
            GetTradesCommand = new DelegateCommand(async (o) => await GetTrades());
            CallTradeHistoryCommand = new DelegateCommand(async (o) => await GetTradeHistory());
            StartBotLimitCommand = new DelegateCommand((o) => StartLimitBot());
            StartBotFirstCommand = new DelegateCommand((o) => StartFirstBot());
            FilterTrades = new DelegateCommand((o) => NewFilterTrades());
            AccountInfo = new DelegateCommand(async (o) => await GetAccountInfo());
            //CallTProperties = new DelegateCommand(async (o) => await GetProperties());
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
                    if (data.Data.Quantity >= symbol.FilterSize && symbol.FilterTradesAll == true)
                    {
                        symbol.AddTrade(new TradeViewModel(data.Data.Price, data.Data.Quantity, data.Data.TradeTime, data.Data.BuyerIsMaker));
                    }
                    else if (data.Data.Quantity >= symbol.FilterSize && data.Data.BuyerIsMaker != symbol.FilterTradesBuy)
                    {
                        symbol.AddTrade(new TradeViewModel(data.Data.Price, data.Data.Quantity, data.Data.TradeTime, data.Data.BuyerIsMaker));
                    }

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

        public async Task GetTradeHistory()
        {
            SelectedSymbol = Selection.SelectedSymbol;

            //var firstResult = await client.SpotApi.ExchangeData.GetAggregatedTradeHistoryAsync(SelectedSymbol.Symbol, limit:1000);
            //var lastId = firstResult.Data.Reverse().Last().Id;

            //SelectedSymbol.Trades = new ObservableCollection<TradeViewModel>(firstResult.Data.Select(r => new TradeViewModel(r.Price, r.Quantity, r.TradeTime, r.BuyerIsMaker)).Reverse().ToList());

            //for (var id = lastId; id > lastId - 500000; id -= 1000)
            //{
            //    var result = await client.SpotApi.ExchangeData.GetAggregatedTradeHistoryAsync(SelectedSymbol.Symbol, limit: 1000, fromId: id);

            //    foreach (var e in result.Data.Reverse())
            //    {
            //        SelectedSymbol.AddHistoryTrade(new TradeViewModel(e.Price, e.Quantity, e.TradeTime, e.BuyerIsMaker));
            //    }
            //}


            var firstResult = await client.SpotApi.ExchangeData.GetAggregatedTradeHistoryAsync(SelectedSymbol.Symbol, limit: 1);
            var hour = 36000000000;

            long curr = DateTime.Now.Ticks;
            DateTime lasttime = firstResult.Data.Last().TradeTime;
            DateTime firsttime = new DateTime(lasttime.Ticks - hour);

            SelectedSymbol.Trades = new ObservableCollection<TradeViewModel>(firstResult.Data.Select(r => new TradeViewModel(r.Price, r.Quantity, r.TradeTime, r.BuyerIsMaker, 1)).Reverse().ToList());

            for (var time = firsttime.Ticks; time > firsttime.Ticks - (hour * 130); time -= hour)
            {
                var newLastTime = new DateTime(time);
                var newFirtstTime = new DateTime(time - hour);

                var result = await client.SpotApi.ExchangeData.GetAggregatedTradeHistoryAsync(SelectedSymbol.Symbol, startTime: newFirtstTime, endTime: newLastTime);

                foreach (var e in result.Data.Reverse())
                {
                    SelectedSymbol.AddHistoryTrade(new TradeViewModel(e.Price, e.Quantity, e.TradeTime, e.BuyerIsMaker, 1));
                }

                Console.WriteLine(SelectedSymbol.Trades.Count);
            }

            Console.WriteLine("____" + SelectedSymbol.Trades.Count);

            long currend = DateTime.Now.Ticks;

            DateTime final = new DateTime(currend - curr);

            Console.WriteLine(final);
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
            AllOrdersBids = new ObservableCollection<OrderViewModel>(result.Data.Bids.Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList());

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            var subscribeResultAsks = await socketClient.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(SelectedSymbol.Symbol, 20, 1000, data =>
            {
                Full = 0;

                var newArrAsks = data.Data.Asks.Select(r => new OrderViewModel(r.Price, r.Quantity)).Reverse().ToList();
                var newArrBids = data.Data.Bids.OrderByDescending(p => p.Price).Where(p => p.Quantity != 0).Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList();

                for (var i = 0; i < 15; i++)
                {
                    Full = AllOrdersAsks[i].OrderQPrice > Full ? AllOrdersAsks[i].OrderQPrice : Full;
                    Full = AllOrdersBids[i].OrderQPrice > Full ? AllOrdersBids[i].OrderQPrice : Full;
                }

                for (var i = 0; i < 15; i++)
                {
                    AllOrdersAsks[i].OrderPrice = newArrAsks[i + 5].OrderPrice;
                    AllOrdersAsks[i].OrderQPrice = Math.Round(newArrAsks[i + 5].OrderQPrice, 5);

                    AllOrdersBids[i].OrderPrice = newArrBids[i].OrderPrice;
                    AllOrdersBids[i].OrderQPrice = Math.Round(newArrBids[i].OrderQPrice, 5);

                    var sumAsks = Math.Round(newArrAsks[i+5].OrderPrice * newArrAsks[i + 5].OrderQPrice, 5).ToString();
                    var b = 14 - sumAsks.Length;
                    for (int e = 1; e <= b; e++)
                    {
                        sumAsks = "  " + sumAsks;
                    }
                    sumAsks = sumAsks.Replace(",", ".");

                    var sumBids = Math.Round(newArrBids[i].OrderPrice * newArrBids[i].OrderQPrice, 5).ToString();
                    var c = 14 - sumBids.Length;
                    for (int e = 1; e <= c; e++)
                    {
                        sumBids = "  " + sumBids;
                    }
                    sumBids = sumBids.Replace(",", ".");

                    var coefAsks = (double)(AllOrdersAsks[i].OrderQPrice / Full) * (-367) - 33;
                    var coefBids = (double)(AllOrdersBids[i].OrderQPrice / Full) * (-367) - 33;

                    AllOrdersAsks[i].BackgroundSize = new Thickness(coefAsks, 0, 32, 0);
                    AllOrdersAsks[i].OrderSum = sumAsks;

                    AllOrdersBids[i].BackgroundSize = new Thickness(coefBids, 0, 32, 0);
                    AllOrdersBids[i].OrderSum = sumBids;
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

        //public async Task GetOrderStreamBids()
        //{
        //    var mainSymbol = SelectedSymbol.Symbol;
        //    client = new BinanceClient();
        //    socketClient = new BinanceSocketClient();

        //    var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(SelectedSymbol.Symbol, 15);
        //    AllOrdersBids = new ObservableCollection<OrderViewModel>(result.Data.Bids.Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList());

        //    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        //    CancellationToken token = cancelTokenSource.Token;

        //    var subscribeResultBids = await socketClient.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(SelectedSymbol.Symbol, 20, 1000, data =>
        //    {
        //        FullBids = 0;
        //        var newArrBids = data.Data.Bids.OrderByDescending(p => p.Price).Where(p => p.Quantity != 0).Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList();

        //        for (var i = 0; i < 15; i++)
        //        {
        //            FullBids = AllOrdersBids[i].OrderQPrice > FullBids ? AllOrdersBids[i].OrderQPrice : FullBids;
        //        }

        //        for (var i = 0; i < 15; i++)
        //        {
        //            AllOrdersBids[i].OrderPrice = newArrBids[i].OrderPrice;
        //            AllOrdersBids[i].OrderQPrice = Math.Round(newArrBids[i].OrderQPrice, 5);

        //            var sum = Math.Round(newArrBids[i].OrderPrice * newArrBids[i].OrderQPrice, 5).ToString();
        //            var b = 14 - sum.Length;
        //            for (int e = 1; e <= b; e++)
        //            {
        //                sum = "  " + sum;
        //            }

        //            var full = FullBids > FullAsks ? FullBids : FullAsks;
        //            var coef = (double)(AllOrdersBids[i].OrderQPrice / full) * (-367) - 33;

        //            AllOrdersBids[i].BackgroundSize = new Thickness(coef, 0, 32, 0);
        //            AllOrdersBids[i].OrderSum = sum.Replace(",", ".");
        //        }

        //        if (CloseStream.ClosedStream)
        //        {
        //            if (CloseStream.ClosedWindowName.Contains(mainSymbol))
        //            {
        //                cancelTokenSource.Cancel();
        //                cancelTokenSource.Dispose();
        //            }
        //        }
        //    }, token);
        //}

        private async Task GetLastTrade()
        {
            var mainSymbol = Selection.SelectedSymbol;
            decimal sum = 0;

            var firstResult = await client.SpotApi.ExchangeData.GetRecentTradesAsync(mainSymbol.Symbol, 1);
            var symbol = AllPrices.SingleOrDefault(a => a.Symbol == mainSymbol.Symbol);
            var newBest = firstResult.Data.FirstOrDefault().Price;
            var oldBest = Math.Round(newBest, 2);

            mainSymbol.SumColor = "white";

            mainSymbol.OrderBestPrice = oldBest >= 1000 ? oldBest.ToString().Replace(",", ".").Insert(Math.Round(newBest).ToString().Length - 3, ",") : oldBest.ToString().Replace(",", ".");

            var subscribeResult = await socketClient.SpotStreams.SubscribeToTradeUpdatesAsync(mainSymbol.Symbol, data =>
            {
                symbol = AllPrices.SingleOrDefault(a => a.Symbol == mainSymbol.Symbol);
                newBest = data.Data.Price;

                if (newBest == sum)
                {
                    mainSymbol.SumColor = "white";
                }
                else
                {
                    mainSymbol.SumColor = newBest > sum ? "#009900" : "red";
                }

                oldBest = Math.Round(newBest, 2);

                mainSymbol.OrderBestPrice = oldBest >= 1000 ? oldBest.ToString().Replace(",", ".").Insert(Math.Round(newBest).ToString().Length - 3, ",") : oldBest.ToString().Replace(",", ".");

                sum = oldBest;
            });
        }

        public async Task TradingStream()
        {
            var startUserStreamAsync = await binanceClient.SpotApi.Account.StartUserStreamAsync();

            if (SelectedSymbol != null)
            {
                var lastPriceTest = await binanceClient.SpotApi.ExchangeData.GetPriceAsync(SelectedSymbol.Symbol);
                SelectedSymbol.TradingPrice = lastPriceTest.Data.Price;
            }

            var subForAccUpdates = await binanceSocketClient.SpotStreams.SubscribeToUserDataUpdatesAsync(startUserStreamAsync.Data, OnOrderUpdate, null, null, onAccountBalanceUpdate: data =>
            {
                //
            });
        }

        public async Task BuyLimit(object o)
        {

            var result = await binanceClient.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Buy, SpotOrderType.Limit, SelectedSymbol.TradingAmount, price: SelectedSymbol.TradingPrice, timeInForce: TimeInForce.GoodTillCanceled);
            if (result.Success)
            {
                Console.WriteLine("Succes!!! BuyLimit");
            }
            else Console.WriteLine(result.Error);
        }

        public async Task SellLimit(object o)
        {

            var result = await binanceClient.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Sell, SpotOrderType.Limit, SelectedSymbol.TradingAmount, price: SelectedSymbol.TradingPrice, timeInForce: TimeInForce.GoodTillCanceled);
            if (result.Success)
            {
                Console.WriteLine("Succes!!! SellLimit");
            }
            else Console.WriteLine(result.Error);
        }
        public async Task BuyMarket(object o)
        {

            var result = await binanceClient.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Buy, SpotOrderType.Market, SelectedSymbol.TradingAmountMarket);
            if (result.Success)
            {
                Console.WriteLine("Succes!!! BuyMarket");
            }
            else Console.WriteLine(result.Error);
        }

        public async Task SellMarket(object o)
        {

            var result = await binanceClient.SpotApi.Trading.PlaceOrderAsync(SelectedSymbol.Symbol, OrderSide.Sell, SpotOrderType.Market, SelectedSymbol.TradingAmountMarket);
            if (result.Success)
            {
                Console.WriteLine("Succes!!! SellMarket");
            }
            else Console.WriteLine(result.Error);
        }

        public async Task AllCancel(object o)
        {

            var result = await binanceClient.SpotApi.Trading.CancelAllOrdersAsync(SelectedSymbol.Symbol);
            if (result.Success)
                Console.WriteLine("Canceled!!!");
            else Console.WriteLine(result.Error);
        }

        public async Task Cancel(long id)
        {
            var result = await binanceClient.SpotApi.Trading.CancelOrderAsync(Selection.SelectedSymbol.Symbol, id);
            if (result.Success)
                Console.WriteLine("OneCanceled!!!");
            else Console.WriteLine(result.Error);
        }

        private void OnOrderUpdate(DataEvent<BinanceStreamOrderUpdate> data)
        {
            var orderUpdate = data.Data;
            var symbol = AllPrices.SingleOrDefault(p => p.Symbol == data.Data.Symbol);
            var price = orderUpdate.Price;

            if (orderUpdate.Type.ToString() == "Market")
            {
                if (data.Data.Status.ToString() == "Filled")
                {
                    price = orderUpdate.LastPriceFilled;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TradingAllTrades.Insert(0, new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                        symbol.AddTradingTrades(new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                    });
                }

                Task.Run(() => GetBalance());
                Task.Run(() => GetAllClients());

                return;
            }

            if (data.Data.Status.ToString() == "Filled")
            {
                var id = data.Data.Id;
                Task.Run(() => GetAllClients());
                Task.Run(() => GetBalance());

                Application.Current.Dispatcher.Invoke(() =>
                {
                    TradingAllOrders = new ObservableCollection<TradingOrdersViewModel>(TradingAllOrders.Where(a => a.Id != id).Select(o => new TradingOrdersViewModel(o.ExecutedQuantity, o.Price, o.Side, o.Status, o.Symbol, o.DateTime, o.Id)));
                    TradingAllTrades.Insert(0, new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                    symbol.TradingOrders = new ObservableCollection<TradingOrdersViewModel>(symbol.TradingOrders.Where(a => a.Id != id).Select(o => new TradingOrdersViewModel(o.ExecutedQuantity, o.Price, o.Side, o.Status, o.Symbol, o.DateTime, o.Id)));
                    symbol.AddTradingTrades(new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                });
            }
            else if (data.Data.Status.ToString() != "Canceled")
            {
                Task.Run(() => GetAllClients());
                Task.Run(() => GetBalance());

                Application.Current.Dispatcher.Invoke(() =>
                {
                    TradingAllOrders.Insert(0, new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                    symbol.AddTradingOrders(new TradingOrdersViewModel(orderUpdate.QuantityFilled, price, orderUpdate.Side, orderUpdate.Status, orderUpdate.Symbol, orderUpdate.CreateTime, orderUpdate.Id));
                });
            }
            else
            {
                var id = data.Data.Id;
                Task.Run(() => GetAllClients());
                Task.Run(() => GetBalance());

                Application.Current.Dispatcher.Invoke(() =>
                {
                    TradingAllOrders = new ObservableCollection<TradingOrdersViewModel>(TradingAllOrders.Where(a => a.Id != id).Select(o => new TradingOrdersViewModel(o.ExecutedQuantity, o.Price, o.Side, o.Status, o.Symbol, o.DateTime, o.Id)));
                    symbol.TradingOrders = new ObservableCollection<TradingOrdersViewModel>(symbol.TradingOrders.Where(a => a.Id != id).Select(o => new TradingOrdersViewModel(o.ExecutedQuantity, o.Price, o.Side, o.Status, o.Symbol, o.DateTime, o.Id)));
                });
            }
        }

        public async Task GetAllOrders()
        {
            var result = await binanceClient.SpotApi.Trading.GetOpenOrdersAsync();

            TradingAllTrades = new ObservableCollection<TradingOrdersViewModel>(result.Data.OrderByDescending(d => d.CreateTime).Where(a => a.Status.ToString() == "Filled").Select(o => new TradingOrdersViewModel(o.QuantityFilled, o.Price, o.Side, o.Status, o.Symbol, o.CreateTime, o.Id)));
            TradingAllOrders = new ObservableCollection<TradingOrdersViewModel>(result.Data.OrderByDescending(d => d.CreateTime).Where(a => a.Status.ToString() != "Filled" && a.Status.ToString() != "Canceled").Select(o => new TradingOrdersViewModel(o.QuantityFilled, o.Price, o.Side, o.Status, o.Symbol, o.CreateTime, o.Id)));
        }

        private async Task GetTrades()
        {
            var result = await binanceClient.SpotApi.Trading.GetOrdersAsync(SelectedSymbol.Symbol);
            var symbol = AllPrices.SingleOrDefault(a => a.Symbol == SelectedSymbol.Symbol);

            SelectedSymbol.TradingOrders = new ObservableCollection<TradingOrdersViewModel>(result.Data.OrderByDescending(d => d.CreateTime).Where(a => a.Status.ToString() != "Filled" && a.Status.ToString() != "Canceled").Select(o => new TradingOrdersViewModel(o.QuantityFilled, o.Price, o.Side, o.Status, o.Symbol, o.CreateTime, o.Id)));
            SelectedSymbol.TradingTrades = new ObservableCollection<TradingOrdersViewModel>(result.Data.OrderByDescending(d => d.CreateTime).Where(a => a.Status.ToString() == "Filled").Select(o => new TradingOrdersViewModel(o.QuantityFilled, o.Price, o.Side, o.Status, o.Symbol, o.CreateTime, o.Id)));
        }


        public void ChangeSymbol()
        {
            if (SelectedSymbol != null)
            {
                Task.Run(async () => await Task.WhenAll(GetOrderStreamAsks(), GetTrades() , GetBalance()));
            }
        }

        private async Task GetBalance()
        {
            SelectedSymbol.BalanceCoin = SelectedSymbol.Symbol.Remove(3);
            var info = await binanceClient.SpotApi.Account.GetAccountInfoAsync();

            SelectedSymbol.Balance = info.Data.Balances.SingleOrDefault(r => r.Asset == "USDT").Available;
            SelectedSymbol.BalanceCoinTotal = info.Data.Balances.SingleOrDefault(r => r.Asset == SelectedSymbol.BalanceCoin).Available;
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

        public async Task CallTradeStream()
        {
            await GetTradeStream();
        }

        public async Task CallOrderStream()
        {
            await GetLastTrade();
        }

        public async Task CallAggTradeStream()
        {
            await GetAggTradeStream();
        }

        public void CancellOne(long id)
        {
            Task.Run(() => Cancel(id));
        }

        public void StartLimitBot()
        {
            BotsList.botsList.Insert(0, new LimitBot(this, SelectedSymbol, SelectedSymbol.BotSize, SelectedSymbol.BotDelta, SelectedSymbol.BotSmartDelta, SelectedSymbol.BotTime, BotsList.IdBot));
            BotsList.IdBot++;
            Console.WriteLine(BotsList.botsList.Count);
        }

        public void StartFirstBot()
        {
            BotsList.botsList.Insert(0, new FirstBot(this, SelectedSymbol, SelectedSymbol.BotSize, SelectedSymbol.BotTime, BotsList.IdBot));
            BotsList.IdBot++;
            Console.WriteLine(BotsList.botsList.Count);
        }

        private async Task GetAllClients()
        {
            var result = await binanceClient.SpotApi.Account.GetAccountInfoAsync();

            Clients = new ObservableCollection<BinanceSymbolViewModel>(result.Data.Balances.Select(r => new BinanceSymbolViewModel(r.Asset, Math.Round(r.Available,3))));
        }

        private async Task GetProperties()
        {
            var result = await binanceClient.SpotApi.Account.GetUserAssetsAsync();
            //Console.WriteLine(result.Error.Message);

            //Console.WriteLine(result.Data);

            //foreach(var e in result.Data)
            //{
            //    Console.WriteLine(e.Name);
            //}
        }

        private void NewFilterTrades()
        {
            selectedSymbol.FilterSize = 0;
        }

        public async Task GetAccountInfo()
        {
            Console.WriteLine("start");
            //if (ApiKey != string.Empty && ApiSecret != string.Empty)
            {
                Console.WriteLine("start1");
                binanceClient = new BinanceClient(new BinanceClientOptions
                {
                    ApiCredentials = new ApiCredentials("AU8ovgGXuLShglvZLyoEcjE3MrE7RaH3PPoESHRX4lrztsAdtvpfYSjXqkfhwogD",
                        "2Pf23BjqUErU79ZMOrMNd5CRJEsA2PhD3U8HRGUMUmgZe3mDtGgZCeQWXlkSlgbh"),
                    SpotApiOptions = new BinanceApiClientOptions
                    {
                        BaseAddress = BinanceApiAddresses.TestNet.RestClientAddress
                    }
                });
                //binanceSocketClient = new BinanceSocketClient(new BinanceSocketClientOptions
                //{
                //    ApiCredentials = new ApiCredentials(ApiKey,
                //            ApiSecret),
                //    SpotStreamsOptions = new BinanceApiClientOptions
                //    {
                //        BaseAddress = BinanceApiAddresses.TestNet.SocketClientAddress
                //    }
                //});
            }

            var result = await binanceClient.SpotApi.Account.GetAccountInfoAsync();

            if (result.Success)
            {
                Console.WriteLine("start2");

                ConnectionStatus = "Connecting.";
                ConnectionStatusColor = "orange";

                Console.WriteLine("start3");

                await Task.Delay(550);

                ConnectionStatus = "Connecting..";
                ConnectionStatusColor = "orange";

                await Task.Delay(550);

                ConnectionStatus = "Connected";
                ConnectionStatusColor = "green";

            }
            else
            {
                Console.WriteLine("start5");

                ConnectionStatus = "Connecting.";
                ConnectionStatusColor = "orange";

                await Task.Delay(550);

                ConnectionStatus = "Connecting..";
                ConnectionStatusColor = "orange";

                await Task.Delay(550);

                ConnectionStatus = "Offline";
                ConnectionStatusColor = "red";
            }
        }
    }
}

