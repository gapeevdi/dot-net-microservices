using System;

namespace EventBus.Messages.Events
{
    public abstract class IntegrationBaseEvent
    {
        protected IntegrationBaseEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        protected IntegrationBaseEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

        public Guid Id { get; }
        public DateTime CreationDate { get; }
    }
}
