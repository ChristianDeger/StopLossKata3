namespace StopLossKata3
{
    public class CallMeIn30SecondsWith : Message
    {
        public readonly Message Callback;

        public CallMeIn30SecondsWith(Message callback)
        {
            Callback = callback;
        }

        public bool Equals(CallMeIn30SecondsWith other)
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
            if (obj.GetType() != typeof(CallMeIn30SecondsWith))
            {
                return false;
            }
            return Equals((CallMeIn30SecondsWith)obj);
        }

        public override int GetHashCode()
        {
            return (Callback != null ? Callback.GetHashCode() : 0);
        }
    }
}