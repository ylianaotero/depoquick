namespace DepoQuick.Domain;

public class Client : User
{
    List<Reservation> _reservations;
    public Client(string name, string email, string password) : base(name, email, password)
    {
        this.SetIsAdministrator(false);
        _reservations = new List<Reservation>();
    }
    
    public List<Reservation> GetReservations()
    {
        return _reservations;
    }

    public void AddReservation(Reservation reservation)
    {
        _reservations.Add(reservation);
        reservation.GetDeposit().AddReservation(reservation);
    }
}

