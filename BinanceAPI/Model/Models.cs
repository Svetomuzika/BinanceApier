﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BinanceAPI.Model
{
    public static class Lock
    {
        public static bool locker = false;
        public static bool Locker
        {
            get { return locker; }
            set 
            { 
                locker = value;
                Console.WriteLine(Locker);
            }
        }
        public static bool IsTradeUserControlExist;
        public static TradeWindow LockedWindow;
        public static UserControl UserControl = null;
    }

    static class CloseStream
    {
        public static bool CloseAggTradeStream = false;
        public static bool CloseTradeStream = false;
        public static bool CloseOrderStream = false;
        public static bool CloseLastTradeStream = false;
    }
}