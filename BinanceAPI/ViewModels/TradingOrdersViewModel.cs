using BinanceApi.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using Binance.Net.Enums;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.ViewModels
{
    public class TradingOrdersViewModel : ObservableObject
    {
        private long id;
        public long Id
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChangedEvent("Id");
            }
        }

        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                RaisePropertyChangedEvent("TradingSymbol");
            }
        }

        private decimal price;
        public decimal Price
        {
            get { return Math.Round(price, 2); }
            set
            {
                price = Math.Round(value, 2);
                RaisePropertyChangedEvent("TradingPrice");
            }
        }

        private decimal originalQuantity;
        public decimal OriginalQuantity
        {
            get { return originalQuantity; }
            set
            {
                originalQuantity = value;
                RaisePropertyChangedEvent("OriginalQuantity");
            }
        }

        private decimal executedQuantity;
        public decimal ExecutedQuantity
        {
            get { return Math.Round(executedQuantity, 3); }
            set
            {
                executedQuantity = Math.Round(value, 3);
                RaisePropertyChangedEvent("ExecutedQuantity");
                RaisePropertyChangedEvent("Fullfilled");
            }
        }

        public string FullFilled
        {
            get { return ExecutedQuantity + "/" + OriginalQuantity; }
        }

        private OrderStatus status;
        public OrderStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                RaisePropertyChangedEvent("Status");
                RaisePropertyChangedEvent("CanCancel");
            }
        }

        private OrderSide side;
        public OrderSide Side
        {
            get { return side; }
            set
            {
                side = value;
                RaisePropertyChangedEvent("Side");
            }
        }

        private SpotOrderType type;
        public SpotOrderType Type
        {
            get { return type; }
            set
            {
                type = value;
                RaisePropertyChangedEvent("Type");
            }
        }

        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChangedEvent("Time");
            }
        }

        public bool CanCancel
        {
            get { return Status == OrderStatus.New || Status == OrderStatus.PartiallyFilled; }
        }


        public TradingOrdersViewModel(decimal quantity, decimal price, OrderSide side, OrderStatus status, string symbol, DateTime time)
        {
            var date = time.AddHours(5);
            DateTime date1 = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

            this.executedQuantity = quantity;
            this.price = price;
            this.side = side;
            this.status = status;
            this.symbol = symbol;
            this.time = date1.ToString("dd MMMM yyyy HH:mm");
        }
    }
}
