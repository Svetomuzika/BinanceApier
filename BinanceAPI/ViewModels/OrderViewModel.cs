﻿using BinanceApi.MVVM;
using System;
using System.Windows;

namespace BinanceApi.ViewModels
{
    public class OrderViewModel : ObservableObject
    {

        private decimal orderPrice;
        public decimal OrderPrice
        {
            get { return orderPrice; }
            set
            {
                orderPrice = value;
                RaisePropertyChangedEvent("OrderPrice");
            }
        }

        private Thickness backgroundSize;
        public Thickness BackgroundSize
        {
            get { return backgroundSize; }
            set
            {
                backgroundSize = value;
                RaisePropertyChangedEvent("BackgroundSize");
            }
        }

        private Thickness filterLevelColor;
        public Thickness FilterLevelColor
        {
            get { return filterLevelColor; }
            set
            {
                filterLevelColor = value;
                RaisePropertyChangedEvent("FilterLevelColor");
            }
        }

        private decimal orderQPrice;
        public decimal OrderQPrice
        {
            get { return orderQPrice; }
            set
            {
                orderQPrice = value;
                RaisePropertyChangedEvent("OrderQPrice");
            }
        }

        private string orderSum;
        public string OrderSum
        {
            get { return orderSum; }
            set
            {
                orderSum = value;
                RaisePropertyChangedEvent("OrderSum");
            }
        }

        public OrderViewModel(decimal price, decimal qprice)
        {
            orderPrice = Math.Round(price, 2);
            orderQPrice = Math.Round(qprice, 5);

            var sum = Math.Round(qprice * price, 5).ToString();
            var b = 14 - sum.Length;
            for (int i = 1; i <= b; i++)
            {
                sum = "  " + sum;
            }

            orderSum = sum.Replace(",",".");
        }
    }
}
