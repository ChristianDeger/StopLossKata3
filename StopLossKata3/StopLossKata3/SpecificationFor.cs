using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using StopLossKata3.Messages;

namespace StopLossKata3
{
    public abstract class SpecificationFor<TAggregate> where TAggregate : Aggregate
    {
        FakeBus _bus;
        protected TAggregate Subject;

        [SetUp]
        public void Setup()
        {
            _bus = new FakeBus();
            Subject = (TAggregate)Activator.CreateInstance(typeof(TAggregate), _bus);
            ReplayEvents(Given());
        }

        protected abstract IEnumerable<Message> Given();

        protected void ShouldRaise(Message message)
        {
            Assert.Contains(message, _bus.Messages);
        }

        protected void ShouldNotRaise<T>()
        {
            var contained = _bus.Messages.Any(x => x.GetType() == typeof(T));
            Assert.IsFalse(contained);
        }

        void ReplayEvents(IEnumerable<Message> events)
        {
            foreach (var @event in events)
            {
                ReplayEventOnSubject(@event);
            }
        }

        void ReplayEventOnSubject(Message @event)
        {
            typeof(TAggregate).InvokeMember("Handle",
                                            BindingFlags.InvokeMethod,
                                            null,
                                            Subject,
                                            new object[] { @event });
        }
    }
}