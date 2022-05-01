using BinanceAPI.Model;
using System.Windows.Controls;

namespace BinanceAPI.View
{
    public partial class AggTradeUserControl : UserControl
    {
        public AggTradeUserControl()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;
        }
    }
}
