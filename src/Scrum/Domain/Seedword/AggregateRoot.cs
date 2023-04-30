namespace Scrum.Domain.Seedwork;

public class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    private readonly List<DomainEvent> _domainEvents = new();
    public virtual IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    protected AggregateRoot(TId id) : base(id)
    {        
    }

    protected void AddDomainEvent(DomainEvent newEvent)
    {
        _domainEvents.Add(newEvent);
    }

    public virtual void ClearEvents()
    {
        _domainEvents.Clear();
    }        
}