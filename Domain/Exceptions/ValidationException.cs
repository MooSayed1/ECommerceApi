namespace Domain.Exceptions;

public sealed class ValidationException(IEnumerable<string> errors) : Exception
{
    public IEnumerable<string> Errors { get; set; } = errors;
}
