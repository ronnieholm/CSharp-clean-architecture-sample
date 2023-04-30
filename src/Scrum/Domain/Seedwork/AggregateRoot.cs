namespace Scrum.Domain.Seedwork;

public class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    public readonly Queue<DomainEvent> DomainEvents = new();

    protected AggregateRoot(TId id) : base(id)
    {
    }
}