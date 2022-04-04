using Binance.Net.Clients;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BinanceApi.MVVM;
using BinanceApi.ViewModels;

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
            var result = await client.SpotApi.ExchangeData.GetAllBookPricesAsync();
            AllTrades = new ObservableCollection<TradeViewModel>(result.Data.Select(r => new TradeViewModel("", "", "")).ToList().Take(40));

            socketClient = new BinanceSocketClient();
            var subscribeResult = await socketClient.SpotStreams.SubscribeToTradeUpdatesAsync(SelectedSymbol.Symbol, data =>
            {
                var date = data.Data.TradeTime;
                DateTime date1 = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                var color = data.Data.BuyerIsMaker ? "#0fb172" : "#e74359";

                var price = data.Data.Price.ToString();
                var quant = data.Data.Quantity.ToString();
                var time = date1.AddHours(5).ToLongTimeString();

                if (i == 0)
                {
                    DoSomeShit(price, quant, time, color, 0);
                }
                else
                {
                    for (var e = 39; e >= 1; e--)
                    {
                        DoSomeShitE(e);
                    }
                    DoSomeShit(price, quant, time, color, 0);
                }
            });
        }


        private void ChangeSymbol()
        {
            if (SelectedSymbol != null)
            {
                Task.Run(() => GetTradeStream());
            }
        }

        private void DoSomeShit(string price, string quant, string time, string color, int e)
        {

            AllTrades[e].Price = price;
            AllTrades[e].QPrice = quant;
            AllTrades[e].Time = time;
            AllTrades[e].Color = color;
        }

        private void DoSomeShitE(int e)
        {

            allTrades[e].Price = allTrades[e - 1].Price;
            allTrades[e].QPrice = allTrades[e - 1].QPrice;
            AllTrades[e].Time = AllTrades[e - 1].Time;
            AllTrades[e].Color = AllTrades[e - 1].Color;
        }

    }
}

