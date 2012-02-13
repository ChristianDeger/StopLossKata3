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
        readonly List<TimedCallback> _callbacks = new List<TimedCallback>();
        readonly List<Message> _messages = new List<Message>();

        public StockTicker(decimal positionPrice) : this(positionPrice, Guid.NewGuid())
        {
        }

        public StockTicker(decimal positionPrice, Guid positionPriceId)
        {
            _messages.Add(new PositionAcquired(positionPrice, positionPriceId));
            AddCallbacks(0, positionPriceId);
        }

        public StockTicker ChangePrice(int seconds, decimal price)
        {
            ChangePrice(seconds, price, Guid.NewGuid());
            return this;
        }

        public StockTicker ChangePrice(int seconds, decimal price, Guid priceId)
        {
            TriggerCallbacks(seconds);
            _messages.Add(new PriceChanged(price, priceId));
            AddCallbacks(seconds, priceId);
            return this;
        }

        void AddCallbacks(int seconds, Guid id)
        {
            _callbacks.Add(new TimedCallback(HighInterval + seconds, new RemoveFromHigh(id)));
            _callbacks.Add(new TimedCallback(LowInterval + seconds, new RemoveFromLow(id)));
        }

        public IEnumerable<Message> ObserveAt(int seconds)
        {
            TriggerCallbacks(seconds);
            return _messages;
        }

        void TriggerCallbacks(int seconds)
        {
            var pending = _callbacks.Where(x => x.Seconds <= seconds).ToList();
            foreach (var callback in pending)
            {
                _messages.Add(callback.Message);
                _callbacks.Remove(callback);
            }
        }

        class TimedCallback
        {
            public readonly int Seconds;
            public readonly Message Message;

            public TimedCallback(int seconds, Message message)
            {
                Seconds = seconds;
                Message = message;
            }
        }
    }
}