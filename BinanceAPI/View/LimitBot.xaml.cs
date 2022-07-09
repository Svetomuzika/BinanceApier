using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace BinanceAPI.View
{
    public partial class LimitBot : Window
    {
        public LimitBot()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBoxAmountMarket(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
