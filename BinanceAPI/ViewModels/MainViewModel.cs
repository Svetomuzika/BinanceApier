using Binance.Net.Clients;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BinanceApi.MVVM;
using BinanceApi.ViewModels;
using System.Globalization;

namespace BinanceAPI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private BinanceSocketClient socketClient;
        private BinanceClient client;

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
                Slection.SelectedSymbol = selectedSymbol;
                RaisePropertyChangedEvent("SymbolIsSelected");
                RaisePropertyChangedEvent("SelectedSymbol");
                ChangeSymbol();
            }
        }

        public bool SymbolIsSelected
        {
            get { return SelectedSymbol != null; }
        }

        public MainViewModel()
        {
            Task.Run(() => GetAllSymbols());
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

        private async Task GetTradeStream()
        {
            client = new BinanceClient();
            AllTrades = new ObservableCollection<TradeViewModel>(new string[25].Select(r => new TradeViewModel()).ToList());
            socketClient = new BinanceSocketClient();
            decimal sum = 0;
            var subscribeResult = await socketClient.SpotStreams.SubscribeToTradeUpdatesAsync(SelectedSymbol.Symbol, data =>
            {

                var date = data.Data.TradeTime;
                DateTime date1 = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                var color = data.Data.BuyerIsMaker ? "#0fb172" : "#e74359";

                for (var e = 24; e >= 1; e--)
                {
                    allTrades[e].TradePrice = allTrades[e - 1].TradePrice;
                    allTrades[e].TradeQPrice = allTrades[e - 1].TradeQPrice;
                    AllTrades[e].TradeTime = AllTrades[e - 1].TradeTime;
                    AllTrades[e].TradeColor = AllTrades[e - 1].TradeColor;
                }
                AllTrades[0].TradePrice = Math.Round(data.Data.Price, 2); 
                AllTrades[0].TradeQPrice = Math.Round(data.Data.Quantity, 5).ToString().Replace(",", ".");
                AllTrades[0].TradeTime = date1.AddHours(5).ToLongTimeString(); ;
                AllTrades[0].TradeColor = color;

                var newBest = AllTrades[0].TradePrice;

                if (newBest == sum)
                {
                    SelectedSymbol.SumColor = "white";
                }
                else
                {
                    SelectedSymbol.SumColor = newBest > sum ? "#009900" : "red";
                }

                var oldBest = Math.Round(newBest, 2);
                SelectedSymbol.OrderBestPrice = oldBest.ToString().Replace(",", ".").Insert(2, ",");

                sum = oldBest;
            });
        }
        
        private async Task GetOrderStreamAsks()
        {
            client = new BinanceClient();
            socketClient = new BinanceSocketClient();

            var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(SelectedSymbol.Symbol, 15);

            AllOrdersAsks = new ObservableCollection<OrderViewModel>(result.Data.Asks.Reverse().Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList());

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
            });
        }

        private async Task GetOrderStreamBids()
        {
            client = new BinanceClient();
            socketClient = new BinanceSocketClient();

            var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(SelectedSymbol.Symbol, 15);
            AllOrdersBids = new ObservableCollection<OrderViewModel>(result.Data.Bids.Select(r => new OrderViewModel(r.Price, r.Quantity)).ToList());

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
            });
        }

        private void ChangeSymbol()
        {
            if (SelectedSymbol != null)
            {
                Task.Run(async () => await Task.WhenAll(GetTradeStream(), GetOrderStreamAsks(), GetOrderStreamBids()));
            }
        }
    }
}

