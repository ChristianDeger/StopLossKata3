using System;

namespace StopLossKata3.Messages
{
    public class RemoveFromHigh : Message
    {
        public readonly Guid PriceId;

        public RemoveFromHigh(Guid priceId)
        {
            PriceId = priceId;
        }

        public bool Equals(RemoveFromHigh other)
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
            if (obj.GetType() != typeof(RemoveFromHigh))
            {
                return false;
            }
            return Equals((RemoveFromHigh)obj);
        }

        public override int GetHashCode()
        {
            return PriceId.GetHashCode();
        }
    }
}