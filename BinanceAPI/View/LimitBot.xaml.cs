using BinanceAPI.Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace BinanceAPI.View
{
    public partial class LimitBot : Window
    {
        private int id;
        public LimitBot(int id)
        {
            this.id = id;
            InitializeComponent();
        }

        private void NumberValidationTextBoxAmountMarket(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void ChangeBotSettings_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in BotsList.botsList)
            {
                if (i.Id == id)
                {
                    var s = SizeBot.Text.Replace('.', ',');
                    var d = DeltaBot.Text.Replace('.', ',');
                    var sd = SmartDeltaBot.Text.Replace('.', ',');
                    var t = TimeBot.Text.Replace('.', ',');

                    i.Size = decimal.Parse(s);
                    i.Delta = decimal.Parse(d);
                    i.SmartDelta = decimal.Parse(sd);
                    i.Time = decimal.Parse(t);
                    
                    return;
                }
            }
        }
    }
}
