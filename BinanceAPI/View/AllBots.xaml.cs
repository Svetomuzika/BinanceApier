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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace BinanceAPI.View
{
    public partial class AllBots : Window
    {
        private ObservableCollection<Bot> Bots { get; set; } = BotsList.botsList;

        public AllBots()
        {
            InitializeComponent();

            Binding myBinding = new Binding
            {
                Source = BotsList.botsList,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            
            BotsListView.SetBinding(ItemsControl.ItemsSourceProperty, myBinding);
        }

        public void DeleteBot_Click(object sender, RoutedEventArgs e)
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

        public void PauseBot_Click(object sender, RoutedEventArgs e)
        {
            Button a = (Button)e.Source;

            foreach (var i in BotsList.botsList)
            {
                if (i.Id == (int)a.Content)
                {
                    if (!i.isPaused)
                    {
                        Uri resourceUri = new Uri("/View/icons/start.png", UriKind.Relative);
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush();
                        brush.ImageSource = temp;

                        a.Background = brush;
                    }
                    else
                    {
                        Uri resourceUri = new Uri("/View/icons/pause.png", UriKind.Relative);
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush();
                        brush.ImageSource = temp;

                        a.Background = brush;
                    }

                    i.StartPauseBot();
                    
                    return;
                }
            }
        }

        public void SettingsBot_Click(object sender, RoutedEventArgs e)
        {
            Button a = (Button)e.Source;
            new LimitBot((int)a.Content) { Left = Left + Width * 1.01, Top = Top }.Show();
        }
    }
}
