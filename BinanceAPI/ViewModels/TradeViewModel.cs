using BinanceApi.MVVM;

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
            get { return tradePrice; }
            set
            {
                tradePrice = value;
                RaisePropertyChangedEvent("TradePrice");
            }
        }

        private string tradeQPrice;
        public string TradeQPrice
        {
            get { return tradeQPrice; }
            set
            {
                tradeQPrice = value;
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

        public TradeViewModel()
        {
        }
    }
}