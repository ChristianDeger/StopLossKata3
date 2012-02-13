using System;
using System.Collections.Generic;
using System.Linq;

using StopLossKata3.Messages;

namespace StopLossKata3
{
    public class StopLoss : Aggregate
    {
        Guid _positionPriceId;
        decimal _positionPrice;
        readonly Dictionary<Guid, decimal> _lowPrices = new Dictionary<Guid, decimal>();
        readonly Dictionary<Guid, decimal> _highPrices = new Dictionary<Guid, decimal>();

        public StopLoss(Bus bus) : base(bus)
        {
        }

        public void Handle(PositionAcquired message)
        {
            Bus.Publish(new CallMeIn15SecondsWith(new RemoveFromLow(message.PriceId)));
            Bus.Publish(new CallMeIn30SecondsWith(new RemoveFromHigh(message.PriceId)));
            _lowPrices.Add(message.PriceId, message.Price);
            _highPrices.Add(message.PriceId, message.Price);
            _positionPriceId = message.PriceId;
            _positionPrice = message.Price;
        }

        public void Handle(PriceChanged message)
        {
            Bus.Publish(new CallMeIn15SecondsWith(new RemoveFromLow(message.PriceId)));
            Bus.Publish(new CallMeIn30SecondsWith(new RemoveFromHigh(message.PriceId)));
            _lowPrices.Add(message.PriceId, message.Price);
            _highPrices.Add(message.PriceId, message.Price);
        }

        public void Handle(RemoveFromLow message)
        {
            var price = _lowPrices[message.PriceId];
            _lowPrices.Remove(message.PriceId);
            if (_lowPrices.Count == 0 || _lowPrices.Values.All(x => x <= price))
            {
                if (price < _positionPrice * 0.95m)
                    Bus.Publish(new SellStock(_positionPriceId));
            }
        }

        public void Handle(RemoveFromHigh message)
        {
            var price = _highPrices[message.PriceId];
            _highPrices.Remove(message.PriceId);
            if (price <= _positionPrice)
                return;

            if (_highPrices.Count == 0 || _highPrices.Values.All(x => x >= price))
            {
                _positionPrice = price;
            }
        }
    }
}