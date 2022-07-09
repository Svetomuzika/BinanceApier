using Binance.Net.Objects.Models.Spot;
using BinanceAPI.ViewModels;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.Model
{
    public abstract class Bot
    {
        protected MainViewModel FuncsClass;
        public BinanceSymbolViewModel Symbol { get; set; }
        public decimal Size { get; set; }
        public decimal Delta { get; set; }
        public decimal SmartDelta { get; set; }
        public decimal Time { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public long IdOrder { get; set; }
        protected WebCallResult<BinancePlacedOrder> Order;
        public bool isPaused;

        protected abstract Task Update();

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
            Console.WriteLine(BotsList.botsList.Count);
        }
    }
}
