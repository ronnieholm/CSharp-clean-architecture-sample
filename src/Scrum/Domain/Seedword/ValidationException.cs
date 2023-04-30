namespace Scrum.Domain.Seedwork;

// We don't store the original value in the exception as the client would
// already know what was passed in given the field. If we cared about the value,
// we could in telemetry for request object serialized.

// TODO: don't log ValidationException as an error in telemetry. It's a client
// error, not something we consider an error on the monitoring dashboard. Log
// with level Information or Verbose.

public class ValidationException : Exception
{
    public string Field { get; }

    public ValidationException(string field, string message) : base(message)
    {
        Field = field;
    }
}