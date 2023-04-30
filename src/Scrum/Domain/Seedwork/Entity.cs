namespace Scrum.Domain.Seedwork;

public class Entity<TId> where TId : notnull
{
    public TId Id { get; protected init; }

    // Jason's reference implementation provides an AuditableEntity class, but
    // we want every entity to be auditable and therefore inline the properties
    // of AuditableEntity instead.
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    protected Entity(TId id) => Id = id;

    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity<TId>;
        if (ReferenceEquals(this, compareTo))
            return true;
        if (compareTo is null)
            return false;
        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
    {
        if (a is null && b is null)
            return true;
        if (a is null || b is null)
            return false;
        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId>? a, Entity<TId>? b) => !(a == b);
    public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();
    public override string ToString() => $"{GetType().Name} [Id = {Id}]";

    protected void ValidateNotAddedOrThrow(Func<object?> lambda)
    {
        var prop = lambda();
        if (prop != null)
            throw new DomainException($"{prop.GetType().Name} is already added.");
    }
}
