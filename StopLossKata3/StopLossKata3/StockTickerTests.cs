using System;
using System.Collections.Generic;

using NUnit.Framework;

using StopLossKata3.Messages;

namespace StopLossKata3
{
    [TestFixture]
    public class StockTickerTests
    {
        readonly Guid _positionPriceId = Guid.NewGuid();
        readonly Guid _changedHighPriceId = Guid.NewGuid();
        readonly Guid _changedLowPriceId = Guid.NewGuid();

        [Test]
        public void Should_produce_messages_from_timeline()
        {
            var messages = new StockTicker(10.0m, _positionPriceId)
                .ChangePrice(5, 11.0m, _changedHighPriceId)
                .ChangePrice(21, 10.0m, _changedLowPriceId)
                .ObserveAt(52);
            CollectionAssert.AreEqual(Messages(), messages);
        }

        IEnumerable<Message> Messages()
        {
            yield return new PositionAcquired(10.0m, _positionPriceId);
            yield return new PriceChanged(11.0m, _changedHighPriceId);
            yield return new RemoveFromHigh(_positionPriceId);
            yield return new RemoveFromHigh(_changedHighPriceId);
            yield return new PriceChanged(10.0m, _changedLowPriceId);
            yield return new RemoveFromLow(_positionPriceId);
            yield return new RemoveFromLow(_changedHighPriceId);
            yield return new RemoveFromHigh(_changedLowPriceId);
            yield return new RemoveFromLow(_changedLowPriceId);
        }
    }
}