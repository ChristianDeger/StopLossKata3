using System;

namespace StopLossKata3
{
    public class RemoveFromLow : Message
    {
        public readonly Guid PriceId;

        public RemoveFromLow(Guid priceId)
        {
            PriceId = priceId;
        }

        public bool Equals(RemoveFromLow other)
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
            if (obj.GetType() != typeof(RemoveFromLow))
            {
                return false;
            }
            return Equals((RemoveFromLow)obj);
        }

        public override int GetHashCode()
        {
            return PriceId.GetHashCode();
        }
    }
}