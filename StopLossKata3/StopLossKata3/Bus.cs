using System.Collections.Generic;

using StopLossKata3.Messages;

namespace StopLossKata3
{
    public interface Bus
    {
        void Publish(Message message);
    }

    public class FakeBus : Bus
    {
        public List<Message> Messages = new List<Message>();

        public void Publish(Message message)
        {
            Messages.Add(message);
        }
    }
}