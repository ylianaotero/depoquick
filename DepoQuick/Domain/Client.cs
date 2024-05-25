namespace DepoQuick.Domain;

public class Client : User
{
    public List<Reservation> Reservations { get; set; }
    public Client(string name, string email, string password) : base(name, email, password)
    {
        this.IsAdministrator = false;
        Reservations = new List<Reservation>();
    }

    public void AddReservation(Reservation reservation)
    {
        Reservations.Add(reservation);
        reservation.Deposit.AddReservation(reservation);
    }
}

