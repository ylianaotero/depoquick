using DepoQuick.Exceptions.ReservationExceptions;

namespace DepoQuick.Domain;

public class Reservation
{
    private static int s_nextId = 0;
    
    private int _id; 
    
    private Deposit _deposit; 
    private Client _client;
    private DateRange _date;
    private string _message;
    private int _state;
    private Rating _rating;

    public int GetId()
    {
        return _id; 
    }

    public Reservation(Deposit deposit, Client client, DateRange date)
    {
        _id = s_nextId; 
        s_nextId++; 
        _deposit = deposit;
        _client = client;
        _date = date;
        _message = "";
        _state = 0;
    }

    public void SetClient(Client expectedClient)
    {
        _client = expectedClient;
    }

    public Client GetClient()
    {
        return _client;
    }

    public void SetDeposit(Deposit expectedDeposit)
    {
        _deposit = expectedDeposit;
    }

    public Deposit GetDeposit()
    {
        return _deposit;
    }

    public void SetMessage(string expectedMessage)
    {
        if (MessageIsValid(expectedMessage))
        {
            _message = expectedMessage;
        }
    }

    private bool MessageIsValid(string expectedMessage)
    {
        if (MessageIsEmpty(expectedMessage))
        {
            throw new ReservationWithEmptyMessageException("El mensaje no debe ser vacio");
        }
        else
        {
            if (MessageHasMoreThan300Characters(expectedMessage))
            {
                throw new ReservationMessageHasMoreThan300CharactersException("El mensaje no debe tener un largo mayor a 300 caracteres");
            }
        }

        return true;
    }

    private bool MessageHasMoreThan300Characters(string expectedMessage)
    {
        return expectedMessage.Length > 300;
    }

    private bool MessageIsEmpty(string expectedMessage)
    {
        return string.IsNullOrWhiteSpace(expectedMessage);
    }

    public string GetMessage()
    {
        return _message;
    }

    public void SetState(int expectedState)
    {
        _state = expectedState;
    }

    public int GetState()
    {
        return _state;
    }

    public void SetDateRange(DateRange newStay)
    {
        _date = newStay;
    }

    public DateRange GetDateRange()
    {
        return _date;
    }
    
    public void SetRating(Rating rating)
    {
        _rating = rating;
    }
    
    public Rating GetRating()
    {
        return _rating;
    }
}