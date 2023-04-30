namespace Scrum.Domain.Seedwork;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
