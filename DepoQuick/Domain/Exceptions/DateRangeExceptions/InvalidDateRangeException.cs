namespace DepoQuick.Domain.Exceptions.DateRangeExceptions;

public class InvalidDateRangeException : Exception
{
    public InvalidDateRangeException(string message) : base(message) {}
}