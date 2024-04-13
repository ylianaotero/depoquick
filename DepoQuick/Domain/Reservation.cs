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

    public void setClient(Client client)
    {
        _client = client;
    }

    public Deposit getDeposit()
    {
        return _deposit;
    }

    public void setDeposit(Deposit deposit)
    {
        _deposit = deposit;
    }

    public DateRange getDateRange()
    {
        return _date;
    }

    public void setDateRange(DateRange dateRange)
    {
        _date = dateRange;
    }

    public String getMessage()
    {
        return _message;
    }

    public void setMessage(String message)
    {
        _message = message;
    }

    public int getState()
    {
        return _state;
    }

    public void setState(int state)
    {
        _state = state;
    }
    
    public static int calculatePrice(Deposit d, DateTime dayIn, DateTime dayOut)
    {
        
        DateRange duration = new DateRange(dayIn, dayOut);
        int stay = duration.numberOfDays();
        int cost = d.calculatePrice(stay);
        return cost;
    }
}