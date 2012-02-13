using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace StopLossKata3
{
    [TestFixture]
    public class When_position_is_acquired : SpecificationFor<StopLoss>
    {
        Guid _priceId;

        protected override IEnumerable<Message> Given()
        {
            _priceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, _priceId);
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
    public class When_price_is_changed : SpecificationFor<StopLoss>
    {
        Guid _priceId;

        protected override IEnumerable<Message> Given()
        {
            _priceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, Guid.NewGuid());
            yield return new PriceChanged(10.0m, _priceId);
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
    public class When_price_below_threshold_is_sustained_for_30_seconds_without_price_changes : SpecificationFor<StopLoss>
    {
        Guid _positionPriceId;
        Guid _changedPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            _changedPriceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, _positionPriceId);
            yield return new PriceChanged(9.0m, _changedPriceId);
            yield return new RemoveFromLow(_positionPriceId);
            yield return new RemoveFromLow(_changedPriceId);
        }

        [Test]
        public void It_should_sell_stock()
        {
            ShouldRaise(new SellStock(_positionPriceId));
        }
    }

    [TestFixture]
    public class When_price_within_threshold_is_sustained_for_30_seconds_without_price_changes : SpecificationFor<StopLoss>
    {
        Guid _positionPriceId;
        Guid _changedPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            _changedPriceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, _positionPriceId);
            yield return new PriceChanged(9.9m, _changedPriceId);
            yield return new RemoveFromLow(_positionPriceId);
            yield return new RemoveFromLow(_changedPriceId);
        }

        [Test]
        public void It_should_not_sell_stock()
        {
            ShouldNotRaise<SellStock>();
        }
    }

    [TestFixture]
    public class When_price_below_threshold_is_sustained_for_less_than_30_seconds_without_price_changes : SpecificationFor<StopLoss>
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

        [Test]
        public void It_should_not_sell_stock()
        {
            ShouldNotRaise<SellStock>();
        }
    }

    [TestFixture]
    public class When_price_below_threshold_is_not_sustained_for_30_seconds_because_of_higher_price : SpecificationFor<StopLoss>
    {
        Guid _positionPriceId;
        Guid _changedPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            _changedPriceId = Guid.NewGuid();
            yield return new PositionAcquired(10.0m, _positionPriceId);
            yield return new PriceChanged(9.0m, _changedPriceId);
            yield return new PriceChanged(10.0m, Guid.NewGuid());
            yield return new RemoveFromLow(_positionPriceId);
            yield return new RemoveFromLow(_changedPriceId);
        }

        [Test]
        public void It_should_not_sell_stock()
        {
            ShouldNotRaise<SellStock>();
        }
    }

    [TestFixture]
    public class When_price_below_threshold_is_sustained_for_30_seconds_with_same_price_change : SpecificationFor<StopLoss>
    {
        Guid _positionPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            return new StockTicker(10.0m, _positionPriceId)
                .ChangePrice(5, 9.0m)
                .ChangePrice(10, 9.0m)
                .ObserveAt(35);
        }

        [Test]
        public void It_should_sell_stock()
        {
            ShouldRaise(new SellStock(_positionPriceId));
        }
    }

    [TestFixture]
    public class When_price_below_threshold_is_sustained_for_30_seconds_with_lower_price_change : SpecificationFor<StopLoss>
    {
        Guid _positionPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            return new StockTicker(10.0m, _positionPriceId)
                .ChangePrice(5, 9.0m)
                .ChangePrice(10, 8.9m)
                .ObserveAt(35);
        }

        [Test]
        public void It_should_sell_stock()
        {
            ShouldRaise(new SellStock(_positionPriceId));
        }
    }

    [TestFixture]
    public class When_higher_price_is_sustained_for_15_seconds_and_dropping_price_below_new_threshold_for_30_seconds : SpecificationFor<StopLoss>
    {
        Guid _positionPriceId;

        protected override IEnumerable<Message> Given()
        {
            _positionPriceId = Guid.NewGuid();
            return new StockTicker(10.0m, _positionPriceId)
                .ChangePrice(5, 11.0m)
                .ChangePrice(21, 10.0m)
                .ObserveAt(52);
        }

        [Test]
        public void It_should_sell_stock()
        {
            ShouldRaise(new SellStock(_positionPriceId));
        }
    }
}