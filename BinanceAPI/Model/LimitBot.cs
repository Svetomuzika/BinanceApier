using Binance.Net.Enums;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.Model
{
    public class LimitBot
    {
        private MainViewModel funcsClass;
        private BinanceSymbolViewModel symbol;
        private decimal size;
        private decimal delta;
        private decimal time;
        private bool isPaused;
        
        public LimitBot(MainViewModel funcsClass, BinanceSymbolViewModel symbol, decimal size, decimal delta, decimal time)
        {
            this.symbol = symbol;
            this.size = size;
            this.delta = delta;
            this.time = time;
            this.funcsClass = funcsClass;
            Task.Run(() => Update());
        }

        private async Task Update()
        {
            if (!isPaused)
            {
                var price = symbol.Price - delta;
                var buy = await funcsClass.binanceClient.SpotApi.Trading.PlaceOrderAsync(symbol.Symbol, OrderSide.Buy, SpotOrderType.Limit, size, price: price, timeInForce: TimeInForce.GoodTillCanceled);
                Console.WriteLine($"Покупка снова по -{delta}");

                await Task.Delay((int)time * 1000);

                var result = await funcsClass.binanceClient.SpotApi.Trading.GetOrdersAsync(symbol.Symbol, buy.Data.Id);

                if (result.Data.FirstOrDefault().Status.ToString() == "Filled")
                {
                    StopBot();
                }

                await funcsClass.binanceClient.SpotApi.Trading.CancelOrderAsync(symbol.Symbol, buy.Data.Id);
                Console.WriteLine("Отмена");

                await Update();
            }
        }

        public void StartPauseBot()
        {
            if (isPaused)
            {
                isPaused = false;
            }
            else
            {
                isPaused = true;
            }
        }

        private void StopBot()
        {
            funcsClass.Bots.Remove(this);
        }
    }
}
