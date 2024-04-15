namespace DepoQuick.Domain;

public class Client : User
{
    List<Reservation> _reservations;
    public Client(string name, string email, string password) : base(name, email, password)
    {
        _reservations = new List<Reservation>();
    }
    
    public List<Reservation> GetReservations()
    {
        return _reservations;
    }
}

