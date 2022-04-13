using BinanceApi.MVVM;
using BinanceApi.ViewModels;
using System.Collections.ObjectModel;

namespace BinanceAPI.ViewModels
{
    public class BinanceSymbolViewModel : ObservableObject
    {
        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                RaisePropertyChangedEvent("Symbol");
            }
        }   

        private decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChangedEvent("Price");
            }
        }

        private string orderBestPrice;
        public string OrderBestPrice
        {
            get { return orderBestPrice; }
            set
            {
                orderBestPrice = value;
                RaisePropertyChangedEvent("OrderBestPrice");
            }
        }

        private string sumColor;
        public string SumColor
        {
            get { return sumColor; }
            set
            {
                sumColor = value;
                RaisePropertyChangedEvent("SumColor");
            }
        }

        private ObservableCollection<TradeViewModel> trades;
        public ObservableCollection<TradeViewModel> Trades
        {
            get { return trades; }
            set
            {
                trades = value;
                RaisePropertyChangedEvent("Trades");
            }
        }

        private ObservableCollection<TradeViewModel> aggTrades;
        public ObservableCollection<TradeViewModel> AggTrades
        {
            get { return aggTrades; }
            set
            {
                aggTrades = value;
                RaisePropertyChangedEvent("AggTrades");
            }
        }

        private decimal tradeAmount;
        public decimal TradeAmount
        {
            get { return tradeAmount; }
            set
            {
                tradeAmount = value;
                RaisePropertyChangedEvent("TradeAmount");
            }
        }

        private decimal tradePrice;
        public decimal TradePrice
        {
            get { return tradePrice; }
            set
            {
                tradePrice = value;
                RaisePropertyChangedEvent("TradePrice");
            }
        }

        public void AddAggTrade(TradeViewModel trade)
        {
            AggTrades.Insert(0, trade);
            RaisePropertyChangedEvent("AggTrades");
        }

        public void AddTrade(TradeViewModel trade)
        {
            Trades.Insert(0, trade);
            RaisePropertyChangedEvent("Trades");
        }

        public BinanceSymbolViewModel(string symbol, decimal price)
        {
            this.symbol = symbol;
            this.price = price;
        }
    }
}