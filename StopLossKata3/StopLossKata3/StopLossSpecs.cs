using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace StopLossKata3
{
    [TestFixture]
    public class When_position_is_acquired : SpecificationFor<StopLoss, PositionAcquired>
    {
        Guid _priceId;

        protected override IEnumerable<Message> Given()
        {
            _priceId = Guid.NewGuid();
            yield break;
        }

        protected override PositionAcquired When()
        {
            return new PositionAcquired(10.0m, _priceId);
        }

        [Test]
        public void It_should_want_a_callback_to_remove_low_in_15_seconds()
        {
            ShouldRaise(new CallMeIn30SecondsWith(new RemoveFromHigh(_priceId)));
        }

        [Test]
        public void It_should_want_a_callback_to_remove_high_in_30_seconds()
        {
            ShouldRaise(new CallMeIn30SecondsWith(new RemoveFromHigh(_priceId)));
        }
    }

    public abstract class Aggregate
    {
        readonly Bus _bus;

        protected Aggregate(Bus bus)
        {
            _bus = bus;
        }

        public Bus Bus
        {
            get
            {
                return _bus;
            }
        }
    }
}