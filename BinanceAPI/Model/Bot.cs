using Binance.Net.Objects.Models.Spot;
using BinanceApi.MVVM;
using BinanceAPI.ViewModels;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.Model
{
    public abstract class Bot : ObservableObject
    {
        protected MainViewModel FuncsClass;
        public BinanceSymbolViewModel Symbol { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChangedEvent("Name");
            }
        }

        private decimal size;
        public decimal Size
        {
            get { return size; }
            set
            {
                size = value;
                RaisePropertyChangedEvent("Size");
            }
        }

        private decimal delta;
        public decimal Delta 
        {
            get { return delta; }
            set
            {
                delta = value;
                RaisePropertyChangedEvent("Delta");
            }
        }

        private decimal smartDelta;
        public decimal SmartDelta
        {
            get { return smartDelta; }
            set
            {
                smartDelta = value;
                RaisePropertyChangedEvent("SmartDelta");
            }
        }

        private decimal time;
        public decimal Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChangedEvent("Time");
            }
        }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public long IdOrder { get; set; }
        protected WebCallResult<BinancePlacedOrder> Order;
        public bool isPaused;

        public abstract Task Update();

        public void StartPauseBot()
        {
            if (isPaused)
            {
                isPaused = false;
                Task.Run(() => Update());
            }
            else
            {
                Task.Run(() => StopBotAsync());
            }
        }

        public async Task StopBotAsync()
        {
            await FuncsClass.binanceClient.SpotApi.Trading.CancelOrderAsync(Symbol.Symbol, IdOrder);
            Order = null;
            isPaused = true;
        }
    }
}
