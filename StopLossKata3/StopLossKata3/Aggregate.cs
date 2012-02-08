namespace StopLossKata3
{
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