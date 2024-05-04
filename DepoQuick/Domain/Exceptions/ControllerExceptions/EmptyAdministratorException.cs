namespace DepoQuick.Domain.Exceptions.AdministratorExceptions;

public class EmptyAdministratorException : Exception
{
    public EmptyAdministratorException(string message) : base(message) {}  
}