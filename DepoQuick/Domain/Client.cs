namespace DepoQuick.Domain;

public class Client : User
{
    public Client(string name, string email, string password) : base(name, email, password)
    {
    }
    
    public List<Reservation> GetReservations()
    {
        return new List<Reservation>();
    }
}

