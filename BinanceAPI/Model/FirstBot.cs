using Binance.Net.Enums;
using BinanceAPI.ViewModels;
using BinanceAPI.View;
using System;
using System.Windows;
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
                if (Order == null)
                {
                    Price = Symbol.Price;
                    Order = await FuncsClass.binanceClient.SpotApi.Trading.PlaceOrderAsync(Symbol.Symbol, OrderSide.Buy, SpotOrderType.Limit, Size, price: Price, timeInForce: TimeInForce.GoodTillCanceled);
                }

                IdOrder = Order.Data.Id;

                

                var result = await FuncsClass.binanceClient.SpotApi.Trading.GetOrdersAsync(Symbol.Symbol, Order.Data.Id);

                if (result.Data.FirstOrDefault().Status.ToString() == "Filled")
                {
                    await StopBotAsync();
                    Console.WriteLine(BotsList.botsList.Count);

                    foreach (var e in BotsList.botsList)
                    {
                        if (Id == e.Id)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                BotsList.botsList.Remove(e);
                            });
                        }
                    }
                }

                await Task.Delay((int)Time * 1000);

                if (Price != Symbol.Price)
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
