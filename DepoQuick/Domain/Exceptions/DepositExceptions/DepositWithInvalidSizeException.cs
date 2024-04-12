namespace DepoQuick.Domain.Exceptions.DepositExceptions;

public class DepositWithInvalidSizeException : Exception
{
    public DepositWithInvalidSizeException(string message) : base(message) {}
}