namespace StopLossKata3
{
    public class StopLoss : Aggregate
    {
        public StopLoss(Bus bus) : base(bus)
        {
        }

        public void Handle(PositionAcquired positionAcquired)
        {
            Bus.Publish(new CallMeIn15SecondsWith(new RemoveFromLow(positionAcquired.PriceId)));
            Bus.Publish(new CallMeIn30SecondsWith(new RemoveFromHigh(positionAcquired.PriceId)));
        }
    }
}