namespace StopLossKata3
{
    public class StopLoss
    {
        readonly FakeBus _bus;

        public StopLoss(FakeBus bus)
        {
            _bus = bus;
        }

        public void Handle(PositionAcquired positionAcquired)
        {
            _bus.Publish(new CallMeIn15SecondsWith(new RemoveFromLow(positionAcquired.PriceId)));
            _bus.Publish(new CallMeIn30SecondsWith(new RemoveFromHigh(positionAcquired.PriceId)));
        }
    }
}