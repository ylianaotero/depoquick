namespace DepoQuick.Domain;

public class Administrator : User
{
    public Administrator(string name, string email, string password) : base(name, email, password)
    {
        this.SetIsAdministrator(true);
    }
    
    public void ApproveReservation(Reservation reservation)
    {
        reservation.SetState(1);
        reservation.GetDeposit().AddReservation(reservation);
    }
    
    public void RejectReservation(Reservation reservation, string reason)
    {
        reservation.SetState(-1);
        reservation.SetMessage(reason);
    }
}