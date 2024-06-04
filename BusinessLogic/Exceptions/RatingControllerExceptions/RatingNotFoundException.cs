namespace BusinessLogic.Exceptions.RatingControllerExceptions;

public class RatingNotFoundException : Exception
{
    public RatingNotFoundException(string message) : base(message) {}
}