namespace BusinessLogic.Exceptions.PromotionControllerExceptions;

public class PromotionNotFoundException : Exception
{
    public PromotionNotFoundException(string message) : base(message) {}
}