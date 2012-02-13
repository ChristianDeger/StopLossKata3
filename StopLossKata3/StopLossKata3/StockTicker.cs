using System;
using System.Collections.Generic;
using System.Linq;

namespace StopLossKata3
{
    // Creates Messages for StopLoss to consume. Inserts timed callbacks.
    public class StockTicker
    {
        const int HighInterval = 15;
        const int LowInterval = 30;
        readonly List<Tuple<int, Message>> _callbacks = new List<Tuple<int, Message>>();
        public readonly List<Message> Messages = new List<Message>();

        public StockTicker(decimal positionPrice, Guid positionPriceId)
        {
            Messages.Add(new PositionAcquired(positionPrice, positionPriceId));
            _callbacks.Add(new Tuple<int, Message>(HighInterval, new RemoveFromHigh(positionPriceId)));
            _callbacks.Add(new Tuple<int, Message>(LowInterval, new RemoveFromLow(positionPriceId)));
        }

        public StockTicker ChangePrice(int seconds, decimal price)
        {
            ChangePrice(seconds, price, Guid.NewGuid());
            return this;
        }

        public StockTicker ChangePrice(int seconds, decimal price, Guid priceId)
        {
            TriggerCallbacks(seconds);
            Messages.Add(new PriceChanged(price, priceId));
            _callbacks.Add(new Tuple<int, Message>(HighInterval + seconds, new RemoveFromHigh(priceId)));
            _callbacks.Add(new Tuple<int, Message>(LowInterval + seconds, new RemoveFromLow(priceId)));
            return this;
        }

        public IEnumerable<Message> ObserveAt(int seconds)
        {
            TriggerCallbacks(seconds);
            return Messages;
        }

        void TriggerCallbacks(int seconds)
        {
            var pending = _callbacks.Where(x => x.Item1 <= seconds).ToList();
            Messages.AddRange(pending.Select(x => x.Item2));
            foreach (var callback in pending)
            {
                _callbacks.Remove(callback);
            }
        }
    }
}