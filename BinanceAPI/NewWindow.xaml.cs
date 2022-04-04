using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BinanceAPI
{ 
    public partial class NewWindow : Window
    {

        public NewWindow()
        {
            InitializeComponent();


            SelectorNew.SelectedItem = D.SelectedSymbol;


        }
        public void NewEra(decimal tradePrice,decimal qPrice)
        {
            System.Console.WriteLine("12212121");
        }

    }

    static class BCA
    {
        private static decimal qPriceSt;
        public static decimal QPriceSt
        {
            get { return qPriceSt; }
            set
            {
                qPriceSt = value;


            }

        }

        private static decimal tradePriceSt;
        public static decimal TradePriceSt
        {
            get { return tradePriceSt; }
            set 
            { 
                tradePriceSt = value; 
                
            }
        }
    }
}
