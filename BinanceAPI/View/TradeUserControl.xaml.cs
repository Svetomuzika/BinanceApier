using BinanceAPI.Model;
using BinanceAPI.ViewModels;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BinanceAPI.View
{
    public partial class TradeUserControl : UserControl
    {
        private bool FilterFlag = false;

        public TradeUserControl()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;
         }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!FilterFlag)
            {
                FilterPanel.Visibility = Visibility.Visible;
                FilterFlag = true;
                SelectorNew.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                FilterPanel.Visibility = Visibility.Hidden;
                FilterFlag = false;
                SelectorNew.Margin = new Thickness(0, -50, 0, 0);
            }
        }
    }
}
