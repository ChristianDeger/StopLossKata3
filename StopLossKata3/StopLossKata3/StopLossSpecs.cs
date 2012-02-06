using System;

using NUnit.Framework;

namespace StopLossKata3
{
    [TestFixture]
    public class When_positions_is_acquired
    {
        FakeBus _bus;
        StopLoss _stopLoss;
        Guid _priceId;

        [SetUp]
        public void Setup()
        {
            _bus = new FakeBus();
            _stopLoss = new StopLoss(_bus);
            _priceId = Guid.NewGuid();
            _stopLoss.Handle(new PositionAcquired(10.0m, _priceId));
        }

        [Test]
        public void It_should_want_a_callback_to_remove_low_in_15_seconds_and_remove_from_high_in_30_seconds()
        {
            Assert.AreEqual(new Message[]
                            {
                                new CallMeIn15SecondsWith(new RemoveFromLow(_priceId)),
                                new CallMeIn30SecondsWith(new RemoveFromHigh(_priceId))
                            },
                            _bus.Messages);
        }
    }
}