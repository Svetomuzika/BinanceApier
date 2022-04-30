using BinanceAPI.Model;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinanceAPI.View
{
    public partial class TradeUserControl : UserControl
    {
        public TradeUserControl()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;
            CloseStream.CloseTradeStream = false;
        }


        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            Lock.Locker = !Lock.Locker;
        }
    }
}
