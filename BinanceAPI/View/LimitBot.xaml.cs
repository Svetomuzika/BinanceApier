using BinanceAPI.Model;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
                    Console.WriteLine(SizeBot.Text);
                    Console.WriteLine(DeltaBot.Text);
                    Console.WriteLine(SmartDeltaBot.Text);
                    Console.WriteLine(TimeBot.Text);
                    i.Size = decimal.Parse(SizeBot.Text.ToString());
                    i.Delta = Decimal.Parse(DeltaBot.Text);
                    //i.SmartDelta = Decimal.Parse(SmartDeltaBot.Text);
                    //i.Time = Decimal.Parse(TimeBot.Text);
                    return;
                }
            }
        }
    }
}
