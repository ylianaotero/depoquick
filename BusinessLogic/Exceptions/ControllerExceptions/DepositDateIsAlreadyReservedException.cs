namespace BusinessLogic.Exceptions.ControllerExceptions;

public class DepositDateIsAlreadyReservedException : Exception
{
    public DepositDateIsAlreadyReservedException(string message) : base(message)
    {
    }
}