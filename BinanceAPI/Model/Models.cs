using BinanceApi.MVVM;
using BinanceAPI.View;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BinanceAPI.Model
{
    public class Searching
    {
        public static string searchMain;
        public static string SearchMain
        {
            get { return searchMain; }
            set
            {
                searchMain = value;
            }
        }
    }

    public static class Lock
    {
        public static bool locker = false;
        public static bool Locker
        {
            get { return locker; }
            set 
            { 
                locker = value;
            }
        }
        public static bool IsTradeUserControlExist;

        public static TradeWindow LockedWindow;

        public static TradeWindow LockedWindowLevel2;
        public static TradeWindow LockedWindowTradeStream;
        public static TradeWindow LockedWindowAggTradeStream;
        public static TradeWindow LockedWindowTrading;

        public static UserControl UserControl;

        public static UserControl UserControlLevel2;
        public static UserControl UserControlTradeStream;
        public static UserControl UserControlAggTradeStream;
        public static UserControl UserControlTrading;

    }

    static class CloseStream
    {
        public static string ClosedWindowName = null;

        public static bool ClosedStream = false;
    }

    public class Main
    {
        public static TradingUserControl CurrTradingControl;
        public static AllTickers AllTickers { get; set; }
        public static MainWindow MainWindow { get; set; }
    }

    static class Selection
    {
        public static BinanceSymbolViewModel SelectedSymbol { get; set; }
        public static bool Flag = true;
    }

    class NewListName
    {
        public static string Name { get; set; }
    }

    //class All : ObservableObject
    //{
    //    public static ObservableCollection<BinanceSymbolViewModel> allPrices;

    //    public static ObservableCollection<BinanceSymbolViewModel> AllPrices
    //    {
    //        get { return allPrices; }
    //        set
    //        {
    //            allPrices = value;
    //        }
    //    }
    //}
}
