using System.Collections.ObjectModel;
using BinanceApi.MVVM;
using BinanceApi.ViewModels;

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

        private decimal qPrice;
        public decimal QPrice
        {
            get { return qPrice; }
            set
            {
                qPrice = value;
                RaisePropertyChangedEvent("QPrice");
            }
        }



        public BinanceSymbolViewModel(string symbol, decimal price)
        {
            this.symbol = symbol;
            this.price = price;
        }

        public BinanceSymbolViewModel(decimal price)
        {
            this.price = price;
        }








        //private decimal priceChangePercent;
        //public decimal PriceChangePercent
        //{
        //    get { return priceChangePercent; }
        //    set
        //    {
        //        priceChangePercent = value;
        //        RaisePropertyChangedEvent("PriceChangePercent");
        //    }
        //}

        //private decimal highPrice;
        //public decimal HighPrice
        //{
        //    get { return highPrice; }
        //    set
        //    {
        //        highPrice = value;
        //        RaisePropertyChangedEvent("HighPrice");
        //    }
        //}

        //private decimal lowPrice;
        //public decimal LowPrice
        //{
        //    get { return lowPrice; }
        //    set
        //    {
        //        lowPrice = value;
        //        RaisePropertyChangedEvent("LowPrice");
        //    }
        //}

        //private decimal volume;
        //public decimal Volume
        //{
        //    get { return volume; }
        //    set
        //    {
        //        volume = value;
        //        RaisePropertyChangedEvent("Volume");
        //    }
        //}

        //private decimal tradeAmount;
        //public decimal TradeAmount
        //{
        //    get { return tradeAmount; }
        //    set
        //    {
        //        tradeAmount = value;
        //        RaisePropertyChangedEvent("TradeAmount");
        //    }
        //}

        //private decimal tradePrice;
        //public decimal TradePrice
        //{
        //    get { return tradePrice; }
        //    set
        //    {
        //        tradePrice = value;
        //        RaisePropertyChangedEvent("TradePrice");
        //    }
        //}

        private ObservableCollection<OrderViewModel> orders;
        public ObservableCollection<OrderViewModel> Orders
        {
            get { return orders; }
            set
            {
                orders = value;
                RaisePropertyChangedEvent("Orders");
            }
        }

        //public void AddOrder(OrderViewModel order)
        //{
        //    Orders.Add(order);
        //    Orders.OrderByDescending(o => o.Time);
        //    RaisePropertyChangedEvent("Orders");
        //}
    }
}