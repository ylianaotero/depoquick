namespace DepoQuick.Domain;

public class Client : User
{
    public List<Reservation> Reservations { get; set; }
    
    private List<Notification> _notifications;
    public Client(string name, string email, string password) : base(name, email, password)
    {
        this.IsAdministrator = false;
        Reservations = new List<Reservation>();
        Notifications= new();
    }

    public List<Notification> Notifications
    {
        get => _notifications;
        private init => _notifications = value;
    }
    public void AddReservation(Reservation reservation)
    {
        Reservations.Add(reservation);
        reservation.Deposit.AddReservation(reservation);
    }
}

