using BinanceApi.MVVM;
using System;
using System.Collections.ObjectModel;

namespace BinanceApi.ViewModels
{
    public class TradeViewModel : ObservableObject
    {

        private string tradeTime;
        public string TradeTime
        {
            get { return tradeTime; }
            set
            {
                tradeTime = value;
                RaisePropertyChangedEvent("TradeTime");
            }
        }

        private decimal tradePrice;
        public decimal TradePrice
        {
            get { return Math.Round(tradePrice, 2); }
            set
            {
                tradePrice = Math.Round(value, 2);
                RaisePropertyChangedEvent("TradePrice");
            }
        }

        private decimal tradeQPrice;
        public decimal TradeQPrice
        {
            get { return Math.Round(tradeQPrice, 5); }
            set
            {
                tradeQPrice = Math.Round(value, 5);
                RaisePropertyChangedEvent("TradeQPrice");
            }
        }

        private string tradeColor;
        public string TradeColor
        {
            get { return tradeColor; }
            set
            {
                tradeColor = value;
                RaisePropertyChangedEvent("TradeColor");
            }
        }

        public TradeViewModel(decimal tradePrice, decimal tradeQPrice, DateTime tradeTime, bool tradeColor)
        {
            var date = tradeTime;

            DateTime date1 = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            var color = tradeColor ? "#e74359" : "#0fb172"; 

            this.tradePrice = tradePrice;
            this.tradeQPrice = tradeQPrice;
            this.tradeTime = date1.AddHours(5).ToLongTimeString();
            this.tradeColor = color;
        }

        public TradeViewModel(decimal tradePrice, decimal tradeQPrice, DateTime tradeTime, bool tradeColor, int i)
        {
            var date = tradeTime;

            DateTime date1 = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            var color = tradeColor ? "#e74359" : "#0fb172";

            this.tradePrice = tradePrice;
            this.tradeQPrice = tradeQPrice;
            this.tradeTime = date1.AddHours(5).ToString("dd.MM.yyyy HH:mm");
            this.tradeColor = color;
        }
    }
}