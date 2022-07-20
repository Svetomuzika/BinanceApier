using BinanceAPI.Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

namespace BinanceAPI.View
{
    public partial class BotSettings : Window
    {
        private int id;
        private int firstbot;
        public BotSettings(int id)
        {
            this.id = id;
            InitializeComponent();
        }

        public BotSettings(int id, int firstBot)
        {
            this.id = id;
            firstbot = firstBot;
            InitializeComponent();

            DeltaBot.Visibility = Visibility.Hidden;
            DeltaBotBlock.Visibility = Visibility.Hidden;
            SmartDeltaBot.Visibility = Visibility.Hidden;
            SmartDeltaBotBlock.Visibility = Visibility.Hidden;
            TimeBot.Visibility = Visibility.Hidden;
            TimeBotBlock.Visibility = Visibility.Hidden;
            TimeFirstBot.Visibility = Visibility.Visible;
            TimeFirstBotBlock.Visibility = Visibility.Visible;
            StartBot.Margin = new Thickness(10, -130, 5, -110);
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
                if (i.Id == id && firstbot == 0)
                {
                    var s = SizeBot.Text.Replace('.', ',');
                    var d = DeltaBot.Text.Replace('.', ',');
                    var sd = SmartDeltaBot.Text.Replace('.', ',');
                    var t = TimeBot.Text.Replace('.', ',');

                    i.Size = decimal.Parse(s);
                    i.Delta = decimal.Parse(d);
                    i.SmartDelta = decimal.Parse(sd);
                    i.Time = decimal.Parse(t);
                    //i.Update();

                    return;
                }
                else
                {
                    var s = SizeBot.Text.Replace('.', ',');
                    var t = TimeFirstBot.Text.Replace('.', ',');

                    i.Size = decimal.Parse(s);
                    i.Time = decimal.Parse(t);
                }
            }
        }
    }
}
