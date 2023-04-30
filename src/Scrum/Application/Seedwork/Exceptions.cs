namespace Scrum.Application.Seedwork;

// For communicating exceptions across the application layer boundary. But
// an exception of name ApplicationException is already part of .NET CoreFX
// and re-defining it would cause confusion.
public class ApplicationLogicException : Exception
{
    public ApplicationLogicException(string message) : base(message)
    {
    }
}

public class ConflictException : Exception
{
    public ConflictException(Type type, Guid id) : base($"'{type.Name}' with id '{id}' already exists.")
    {
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
    
    public NotFoundException(Type type, Guid id) : base($"'{type.Name}' with id '{id}' not found.")
    {
    }
}