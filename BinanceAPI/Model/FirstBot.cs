using Binance.Net.Enums;
using BinanceAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BinanceAPI.Model
{
    internal class FirstBot : Bot
    {
        public FirstBot(MainViewModel funcsClass, BinanceSymbolViewModel symbol, decimal size, decimal time, int id)
        {
            Symbol = symbol;
            Size = size;
            Time = time;
            Id = id;
            FuncsClass = funcsClass;
            Task.Run(() => Update());
        }

        public override async Task Update()
        {
            if (!isPaused)
            {
                Price = Symbol.Price;
                if (Order == null)
                {
                    Order = await FuncsClass.binanceClient.SpotApi.Trading.PlaceOrderAsync(Symbol.Symbol, OrderSide.Buy, SpotOrderType.Limit, Size, price: Price, timeInForce: TimeInForce.GoodTillCanceled);
                    Console.WriteLine($"Покупка снова по -{Delta}");
                }

                IdOrder = Order.Data.Id;

                await Task.Delay((int)Time * 1000);

                var result = await FuncsClass.binanceClient.SpotApi.Trading.GetOrdersAsync(Symbol.Symbol, Order.Data.Id);

                if (result.Data.FirstOrDefault().Status.ToString() == "Filled")
                {
                    Console.WriteLine("clear");
                    var a = BotsList.AllBotsWindow;
                    Console.WriteLine("clear1");
                    //a.BotsListView.Items.Remove();
                    return;
                }

                var lastPrice = Order.Data.Price + Delta;
                if (!(lastPrice + SmartDelta > Symbol.Price && lastPrice - SmartDelta < Symbol.Price))
                {
                    await FuncsClass.binanceClient.SpotApi.Trading.CancelOrderAsync(Symbol.Symbol, Order.Data.Id);
                    Order = null;
                    Console.WriteLine("Отмена");
                }

                await Update();
            }
        }
    }
}
