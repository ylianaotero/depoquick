namespace DepoQuick.Domain;

public class Administrator : User
{
    public Administrator(string name, string email, string password) : base(name, email, password)
    {
        this.IsAdministrator = true;
    }
}