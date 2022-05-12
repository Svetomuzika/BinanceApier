using BinanceAPI.Model;
using System.Windows.Controls;

namespace BinanceAPI.View
{
    public partial class TradeUserControl : UserControl
    {
        public TradeUserControl()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;
         }
    }
}
