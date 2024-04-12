namespace DepoQuick.Domain;

public class Client : User
{
    public Client(string name, string email, string password) : base(name, email, password)
    {
    }
}