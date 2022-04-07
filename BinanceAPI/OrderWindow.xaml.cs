using System.Windows;

namespace BinanceAPI
{
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Slection.SelectedSymbol;
        }
    }
}
