using BinanceAPI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BinanceAPI.View
{
    public partial class AllOrders : Window
    {
        public AllOrders()
        {
            InitializeComponent();
        }

        public void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var b = new MainViewModel();
            Button a = (Button)e.Source;

            var content = a.Content.ToString().Split(',');


            b.CancellOne(long.Parse(content[0]) , content[1]);
        }
    }
}
