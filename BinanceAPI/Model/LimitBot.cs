using Binance.Net.Enums;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.Model
{
    public class LimitBot : Bot
    {
        private bool isOrdered;

        public LimitBot(MainViewModel funcsClass, BinanceSymbolViewModel symbol, decimal size, decimal delta, decimal smartDelta, decimal time, int id)
        {
            Symbol = symbol;
            Size = size;
            Delta = delta;
            SmartDelta = smartDelta;
            Time = time;
            Id = id;
            FuncsClass = funcsClass;
            Task.Run(() => Update());
        }

        public override async Task Update()
        {
            //if (!isPaused && !isOrdered)
            //{
            //    Price = Math.Round(Symbol.Price - Delta, 2);
            //    if(Order == null)
            //    {
            //        Order = await FuncsClass.binanceClient.SpotApi.Trading.PlaceOrderAsync(Symbol.Symbol, OrderSide.Buy, SpotOrderType.Limit, Size, price: Price, timeInForce: TimeInForce.GoodTillCanceled);
            //        isOrdered = true;
            //        Console.WriteLine($"Покупка снова по -{Delta}");
            //    }

            //    IdOrder = Order.Data.Id;

            //    await Task.Delay((int)Time * 1000);

            //    var result = await FuncsClass.binanceClient.SpotApi.Trading.GetOrdersAsync(Symbol.Symbol, Order.Data.Id);

            //    if (result.Data.FirstOrDefault().Status.ToString() == "Filled")
            //    {
            //        await StopBotAsync();
            //    }

            //    var lastPrice = Order.Data.Price + Delta;
            //    if(!(lastPrice + SmartDelta > Symbol.Price && lastPrice - SmartDelta < Symbol.Price))
            //    {
            //        await FuncsClass.binanceClient.SpotApi.Trading.CancelOrderAsync(Symbol.Symbol, Order.Data.Id);
            //        isOrdered = false;
            //        Order = null;
            //        Console.WriteLine("Отмена");
            //    }   

            Console.WriteLine(Id);

            await Task.Delay(3 * 500 * 2);

            await Update();
            //}
        }
    }
}
