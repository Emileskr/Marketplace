
namespace Domain.Exceptions;

public class InvalidUpdateStatusCommandException : Exception
{
    public InvalidUpdateStatusCommandException(string message) : base(message) { }
}
