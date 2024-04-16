namespace DepoQuick.Domain;

public class Reservation
{
    private Deposit _deposit; 
    private Client _client;
    private DateRange _date;
    private string _message;
    private int _state;

    public Reservation(Deposit deposit, Client client, DateRange date)
    {
        _deposit = deposit;
        _client = client;
        _date = date;
        _message = "";
        _state = 0;
    }

    public Client getClient()
    {
        return _client;
    }
    
    public DateRange getDateRange()
    {
        return _date;
    }
    
    public Deposit getDeposit()
    {
        return _deposit;
    }


}