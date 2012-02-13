using System;

namespace StopLossKata3
{
    public class PriceChanged : Message
    {
        public readonly Guid PriceId;

        public readonly decimal Price;

        public PriceChanged(decimal price, Guid priceId)
        {
            PriceId = priceId;
            Price = price;
        }

        public bool Equals(PriceChanged other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.PriceId.Equals(PriceId) && other.Price == Price;
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
            if (obj.GetType() != typeof(PriceChanged))
            {
                return false;
            }
            return Equals((PriceChanged)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (PriceId.GetHashCode() * 397) ^ Price.GetHashCode();
            }
        }
    }
}