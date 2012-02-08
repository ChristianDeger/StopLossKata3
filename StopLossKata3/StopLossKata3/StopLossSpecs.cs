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

    [TestFixture]
    public class When_price_is_changed : SpecificationFor<StopLoss, PriceChanged>
    {
        Guid _priceId;

        protected override IEnumerable<Message> Given()
        {
            _priceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, Guid.NewGuid());
        }

        protected override PriceChanged When()
        {
            return new PriceChanged(10.0m, _priceId);
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

    [TestFixture]
    public class When_price_below_threshold_is_sustained_for_30_seconds_without_price_changes : SpecificationFor<StopLoss, RemoveFromLow>
    {
        Guid _positionPriceId;
        Guid _changedPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            _changedPriceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, _positionPriceId);
            yield return new PriceChanged(9.0m, _changedPriceId);
        }

        protected override RemoveFromLow When()
        {
            return new RemoveFromLow(_changedPriceId);
        }

        [Test]
        public void It_should_sell_stock()
        {
            ShouldRaise(new SellStock(_positionPriceId));
        }
    }

    [TestFixture]
    public class When_price_within_threshold_is_sustained_for_30_seconds_without_price_changes : SpecificationFor<StopLoss, RemoveFromLow>
    {
        Guid _positionPriceId;
        Guid _changedPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            _changedPriceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, _positionPriceId);
            yield return new PriceChanged(9.9m, _changedPriceId);
        }

        protected override RemoveFromLow When()
        {
            return new RemoveFromLow(_changedPriceId);
        }

        [Test]
        public void It_should_not_sell_stock()
        {
            ShouldNotRaise<SellStock>();
        }
    }

    [TestFixture]
    public class When_price_below_threshold_is_sustained_for_less_than_30_seconds_without_price_changes : SpecificationFor<StopLoss, PriceChanged>
    {
        Guid _positionPriceId;
        Guid _changedPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            _changedPriceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, _positionPriceId);
        }

        protected override PriceChanged When()
        {
            return new PriceChanged(9.0m, _changedPriceId);
        }

        [Test]
        public void It_should_not_sell_stock()
        {
            ShouldNotRaise<SellStock>();
        }
    }
}