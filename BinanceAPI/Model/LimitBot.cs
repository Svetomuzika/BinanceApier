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
        public BinanceSymbolViewModel Symbol { get; set; }
        public decimal Size { get; set; }
        public decimal Delta { get; set; }
        public decimal Time { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public long IdOrder { get; set; }


        private bool isPaused;

        public LimitBot(MainViewModel funcsClass, BinanceSymbolViewModel symbol, decimal size, decimal delta, decimal time, int id)
        {
            Symbol = symbol;
            Size = size;
            Delta = delta;
            Time = time;
            Id = id;
            this.funcsClass = funcsClass;
            Task.Run(() => Update());
        }

        private async Task Update()
        {
            if (!isPaused)
            {
                Price = Math.Round(Symbol.Price - Delta, 2);
                var buy = await funcsClass.binanceClient.SpotApi.Trading.PlaceOrderAsync(Symbol.Symbol, OrderSide.Buy, SpotOrderType.Limit, Size, price: Price, timeInForce: TimeInForce.GoodTillCanceled);
                Console.WriteLine($"Покупка снова по -{Delta}");

                IdOrder = buy.Data.Id;

                await Task.Delay((int)Time * 1000);

                var result = await funcsClass.binanceClient.SpotApi.Trading.GetOrdersAsync(Symbol.Symbol, buy.Data.Id);

                if (result.Data.FirstOrDefault().Status.ToString() == "Filled")
                {
                    await StopBotAsync();
                }

                await funcsClass.binanceClient.SpotApi.Trading.CancelOrderAsync(Symbol.Symbol, buy.Data.Id);
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

        public async Task StopBotAsync()
        {
            await funcsClass.binanceClient.SpotApi.Trading.CancelOrderAsync(Symbol.Symbol, IdOrder);

            isPaused = true;
            Console.WriteLine(BotsList.botsList.Count);
        }
    }
}
