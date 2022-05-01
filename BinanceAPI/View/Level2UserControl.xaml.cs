using BinanceAPI.Model;
using System.Windows.Controls;

namespace BinanceAPI.View
{
    public partial class Level2UserControl : UserControl
    {
        public Level2UserControl()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;
            SelectorNew1.SelectedItem = Selection.SelectedSymbol;
        }
    }
}
