using BinanceAPI.Model;
using BinanceAPI.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace BinanceAPI.View
{
    public partial class LoginApi : Window
    {
        public LoginApi()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            //Main.MainWindow.Activate();
            //Task.Run(() => Main.MainWindow.ConnectingSucces());
        }
    }
}
