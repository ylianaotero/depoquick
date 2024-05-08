namespace DepoQuick.Exceptions.DepositExceptions;

public class DepositWithInvalidAreaException : Exception
{
    public DepositWithInvalidAreaException(string message) : base(message) {}
}