using System;

namespace StopLossKata3.Messages
{
    public class PositionAcquired : Message
    {
        public readonly decimal Price;
        public readonly Guid PriceId;

        public PositionAcquired(decimal price, Guid priceId)
        {
            Price = price;
            PriceId = priceId;
        }

        public bool Equals(PositionAcquired other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Price == Price && other.PriceId.Equals(PriceId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(PositionAcquired))
            {
                return false;
            }
            return Equals((PositionAcquired)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Price.GetHashCode() * 397) ^ PriceId.GetHashCode();
            }
        }
    }
}