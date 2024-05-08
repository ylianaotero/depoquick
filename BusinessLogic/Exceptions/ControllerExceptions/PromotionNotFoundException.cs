namespace BusinessLogic.Exceptions.ControllerExceptions;

public class PromotionNotFoundException : Exception
{
    public PromotionNotFoundException(string message) : base(message) {}
}