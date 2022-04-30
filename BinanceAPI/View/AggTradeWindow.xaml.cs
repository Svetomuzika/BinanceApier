﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BinanceAPI.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BinanceAPI.Model;

namespace BinanceAPI
{
    public partial class AggTradeWindow : Window
    {
        public AggTradeWindow()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;

            CloseStream.CloseAggTradeStream = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CloseStream.CloseAggTradeStream = true;
        }
    }
}