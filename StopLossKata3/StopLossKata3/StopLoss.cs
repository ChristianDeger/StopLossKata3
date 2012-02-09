using System;
using System.Collections.Generic;

namespace StopLossKata3
{
    public class StopLoss : Aggregate
    {
        Guid _positionPriceId;
        decimal _positionPrice;
        readonly Dictionary<Guid, decimal> _lowPrices = new Dictionary<Guid, decimal>();

        public StopLoss(Bus bus) : base(bus)
        {
        }

        public void Handle(PositionAcquired message)
        {
            Bus.Publish(new CallMeIn15SecondsWith(new RemoveFromLow(message.PriceId)));
            Bus.Publish(new CallMeIn30SecondsWith(new RemoveFromHigh(message.PriceId)));
            _positionPriceId = message.PriceId;
            _positionPrice = message.Price;
        }

        public void Handle(PriceChanged message)
        {
            Bus.Publish(new CallMeIn15SecondsWith(new RemoveFromLow(message.PriceId)));
            Bus.Publish(new CallMeIn30SecondsWith(new RemoveFromHigh(message.PriceId)));
            _lowPrices.Add(message.PriceId, message.Price);
        }

        public void Handle(RemoveFromLow message)
        {
            var price = _lowPrices[message.PriceId];
            if (price < _positionPrice * 0.95m)
                Bus.Publish(new SellStock(_positionPriceId));
        }
    }
}