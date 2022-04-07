using BinanceApi.MVVM;
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

        public BinanceSymbolViewModel(string symbol, decimal price)
        {
            this.symbol = symbol;
            this.price = price;
        }
    }
}