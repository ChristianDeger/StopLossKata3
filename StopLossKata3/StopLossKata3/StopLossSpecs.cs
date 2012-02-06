using System;

using NUnit.Framework;

namespace StopLossKata3
{
    [TestFixture]
    public class When_positions_is_acquired : SpecificationFor<StopLoss, PositionAcquired>
    {
        Guid _priceId;

        [SetUp]
        public void Setup()
        {
            _priceId = Guid.NewGuid();
            Subject.Handle(new PositionAcquired(10.0m, _priceId));
        }

        [Test]
        public void It_should_want_a_callback_to_remove_low_in_15_seconds_and_remove_from_high_in_30_seconds()
        {
            Assert.AreEqual(new Message[]
                            {
                                new CallMeIn15SecondsWith(new RemoveFromLow(_priceId)),
                                new CallMeIn30SecondsWith(new RemoveFromHigh(_priceId))
                            },
                            Bus.Messages);
        }
    }

    public abstract class SpecificationFor<TAggregate, TMessage> where TAggregate : Aggregate
                                                                 where TMessage : Message
    {
        [SetUp]
        public void Setup()
        {
            Bus = new FakeBus();
            Subject = (TAggregate)Activator.CreateInstance(typeof(TAggregate), Bus);
        }

        protected FakeBus Bus;
        protected TAggregate Subject;
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