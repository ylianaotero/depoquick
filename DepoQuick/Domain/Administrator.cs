namespace DepoQuick.Domain;

public class Administrator : User
{
    public Administrator(string name, string email, string password) : base(name, email, password)
    {
    }
    
    public void ApproveReservation(Reservation reservation)
    {
        reservation.SetSate(1);
    }
}