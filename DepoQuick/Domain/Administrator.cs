namespace DepoQuick.Domain;

public class Administrator : User
{
    public Administrator(string name, string email, string password) : base(name, email, password)
    {
        this.IsAdministrator = true;
    }
    
    public void ApproveReservation(Reservation reservation)
    {
        reservation.Status = 1;
    }
    
    public void RejectReservation(Reservation reservation, string reason)
    {
        reservation.Status = -1;
        reservation.Message = reason;
    }
}