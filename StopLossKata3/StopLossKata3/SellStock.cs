using System;

namespace StopLossKata3
{
    public class SellStock : Message
    {
        public readonly Guid PriceId;

        public SellStock(Guid priceId)
        {
            PriceId = priceId;
        }

        public bool Equals(SellStock other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.PriceId.Equals(PriceId);
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
            if (obj.GetType() != typeof(SellStock))
            {
                return false;
            }
            return Equals((SellStock)obj);
        }

        public override int GetHashCode()
        {
            return PriceId.GetHashCode();
        }
    }
}