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

        public BinanceSymbolViewModel(string symbol, decimal price)
        {
            this.symbol = symbol;
            this.price = price;
        }
    }
}