namespace StopLossKata3.Messages
{
    public class CallMeIn15SecondsWith : Message
    {
        public readonly Message Callback;

        public CallMeIn15SecondsWith(Message callback)
        {
            Callback = callback;
        }

        public bool Equals(CallMeIn15SecondsWith other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Callback, Callback);
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
            if (obj.GetType() != typeof(CallMeIn15SecondsWith))
            {
                return false;
            }
            return Equals((CallMeIn15SecondsWith)obj);
        }

        public override int GetHashCode()
        {
            return (Callback != null ? Callback.GetHashCode() : 0);
        }
    }
}