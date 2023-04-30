namespace Scrum.Domain.Seedwork;

public abstract record DomainEvent
{    
    public bool IsPublished { get; private set; }    
    
    // TODO: This property was only there in EF based application to generate
    // the database schema. The actual OccurredAt should be set by the database
    // layer, so we can get rid of this property. DomainEvent isn't persistened
    // as it, but is transformed into a PersistedEvent.
    public DateTime OccurredAt { get; } = DateTime.UtcNow;

    public void MarkAsPublished()
    {
        IsPublished = true;
    }
}
