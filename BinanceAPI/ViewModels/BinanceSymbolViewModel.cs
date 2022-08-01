using BinanceApi.MVVM;
using BinanceApi.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace BinanceAPI.ViewModels
{
    public class BinanceSymbolViewModel : ObservableObject
    {
        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                RaisePropertyChangedEvent("Symbol");
            }
        }   

        private decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChangedEvent("Price");
            }
        }

        private decimal balance;
        public decimal Balance
        {
            get { return balance; }
            set
            {
                balance = Math.Round(value, 2);
                RaisePropertyChangedEvent("Balance");
            }
        }

        private decimal filterSize = 0;
        public decimal FilterSize
        {
            get { return filterSize; }
            set
            {
                filterSize = value;
                RaisePropertyChangedEvent("FilterSize");
            }
        }

        private bool filterTradesAll = true;
        public bool FilterTradesAll
        {
            get { return filterTradesAll; }
            set
            {
                filterTradesAll = value;
                RaisePropertyChangedEvent("FilterTradesAll");
            }
        }

        private bool filterTradesBuy;
        public bool FilterTradesBuy
        {
            get { return filterTradesBuy; }
            set
            {
                filterTradesBuy = value;
                RaisePropertyChangedEvent("FilterTradesBuy");
            }
        }

        private bool filterTradesSell;
        public bool FilterTradesSell
        {
            get { return filterTradesSell; }
            set
            {
                filterTradesSell = value;
                RaisePropertyChangedEvent("FilterTradesSell");
            }
        }

        private string balanceCoin;
        public string BalanceCoin
        {
            get { return balanceCoin; }
            set
            {
                balanceCoin = value;
                RaisePropertyChangedEvent("BalanceCoin");
            }
        }

        private decimal balanceCoinTotal;
        public decimal BalanceCoinTotal
        {
            get { return balanceCoinTotal; }
            set
            {
                balanceCoinTotal = Math.Round(value, 3);
                RaisePropertyChangedEvent("BalanceCoinTotal");
            }
        }

        private string orderBestPrice;
        public string OrderBestPrice
        {
            get { return orderBestPrice; }
            set
            {
                orderBestPrice = value;
                RaisePropertyChangedEvent("OrderBestPrice");
            }
        }

        private string sumColor;
        public string SumColor
        {
            get { return sumColor; }
            set
            {
                sumColor = value;
                RaisePropertyChangedEvent("SumColor");
            }
        }

        private ObservableCollection<TradeViewModel> trades;
        public ObservableCollection<TradeViewModel> Trades
        {
            get { return trades; }
            set
            {
                trades = value;
                RaisePropertyChangedEvent("Trades");
            }
        }

        private ObservableCollection<TradeViewModel> aggTrades;
        public ObservableCollection<TradeViewModel> AggTrades
        {
            get { return aggTrades; }
            set
            {
                aggTrades = value;
                RaisePropertyChangedEvent("AggTrades");
            }
        }
        

        private decimal tradingAmountMarket;
        public decimal TradingAmountMarket
        {
            get { return tradingAmountMarket; }
            set
            {
                tradingAmountMarket = value;
                RaisePropertyChangedEvent("TradingAmountMarket");
            }
        }

        private decimal tradingAmount;
        public decimal TradingAmount
        {
            get { return tradingAmount; }
            set
            {
                tradingAmount = value;
                RaisePropertyChangedEvent("TradingAmount");
            }
        }

        private decimal botSize;
        public decimal BotSize
        {
            get { return botSize; }
            set
            {
                botSize = value;
                RaisePropertyChangedEvent("BotSize");
            }
        }

        private decimal botDelta;
        public decimal BotDelta
        {
            get { return botDelta; }
            set
            {
                botDelta = value;
                RaisePropertyChangedEvent("BotDelta");
            }
        }

        private decimal botSmartDelta;
        public decimal BotSmartDelta
        {
            get { return botSmartDelta; }
            set
            {
                botSmartDelta = value;
                RaisePropertyChangedEvent("BotSmartDelta");
            }
        }

        private decimal botTime;
        public decimal BotTime
        {
            get { return botTime; }
            set
            {
                botTime = value;
                RaisePropertyChangedEvent("BotTime");
            }
        }

        private ObservableCollection<OrderViewModel> orders;
        public ObservableCollection<OrderViewModel> Orders
        {
            get { return orders; }
            set
            {
                orders = value;
                RaisePropertyChangedEvent("Orders");
            }
        }

        private decimal tradingPrice;
        public decimal TradingPrice
        {
            get { return Math.Round(tradingPrice, 2); }
            set
            {
                tradingPrice = Math.Round(value, 2);
                RaisePropertyChangedEvent("TradingPrice");
            }
        }

        public void AddHistoryTrade(TradeViewModel trade)
        {
            Trades.Add(trade);
            RaisePropertyChangedEvent("AddHistoryTrade");
        }

        public void AddTrade(TradeViewModel trade)
        {
            Trades.Insert(0, trade);
            RaisePropertyChangedEvent("AddTrade");
        }

        public void AddAggTrade(TradeViewModel trade)
        {
            AggTrades.Insert(0, trade);
            RaisePropertyChangedEvent("AddAggTrade");
        }

        private ObservableCollection<TradingOrdersViewModel> tradingOrders;
        public ObservableCollection<TradingOrdersViewModel> TradingOrders
        {
            get { return tradingOrders; }
            set
            {
                tradingOrders = value;
                RaisePropertyChangedEvent("TradingOrders");
            }
        }

        private ObservableCollection<TradingOrdersViewModel> tradingTrades;
        public ObservableCollection<TradingOrdersViewModel> TradingTrades
        {
            get { return tradingTrades; }
            set
            {
                tradingTrades = value;
                RaisePropertyChangedEvent("TradingTrades");
            }
        }

        public BinanceSymbolViewModel(string symbol, decimal price)
        {
            this.symbol = symbol;
            this.price = price;
        }

        public void AddTradingOrders(TradingOrdersViewModel order)
        {
            TradingOrders.Insert(0, order);
            RaisePropertyChangedEvent("TradingOrders");
        }

        public void AddTradingTrades(TradingOrdersViewModel order)
        {
            TradingTrades.Insert(0, order);
            RaisePropertyChangedEvent("TradingTrades");
        }
    }
}