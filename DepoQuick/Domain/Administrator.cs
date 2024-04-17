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
    
    public void RejectReservation(Reservation reservation, string reason)
    {
        reservation.SetSate(-1);
        reservation.SetMessage(reason);
    }
}