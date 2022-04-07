using System.Windows;

namespace BinanceAPI
{ 
    public partial class TradeWindow : Window
    {
        public TradeWindow()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Slection.SelectedSymbol;
        }
    }
}
