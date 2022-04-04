using System;
using Binance.Net.Enums;
using BinanceApi.MVVM;

namespace BinanceApi.ViewModels
{
    public class TradeViewModel : ObservableObject
    {

        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChangedEvent("Time");
            }
        }

        private string price;
        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChangedEvent("Price");
            }
        }

        private string qPrice;
        public string QPrice
        {
            get { return qPrice; }
            set
            {
                qPrice = value;
                RaisePropertyChangedEvent("QPrice");
            }
        }

        private string color;
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                RaisePropertyChangedEvent("Color");
            }
        }

        public TradeViewModel(string price, string qprice, string time)
        {
            this.price = price;
            this.qPrice = qprice;
            this.time = time;
        }


    }
}