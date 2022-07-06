using BinanceAPI.Model;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BinanceAPI.View
{
    public partial class AllBots : Window
    {
        ObservableCollection<LimitBot> bots;

        public AllBots()
        {
            InitializeComponent();

            bots = BotsList.botsList;
            BotsListView.ItemsSource = bots;
        }

        public void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            Button a = (Button)e.Source;

            foreach (var i in BotsList.botsList)
            {
                if (i.Id == (int)a.Content)
                {
                    Task.Run(() => i.StopBotAsync());
                    Console.WriteLine("удаление");
                    BotsList.botsList.Remove(i);
                    return;
                }
            }
        }
    }
}
