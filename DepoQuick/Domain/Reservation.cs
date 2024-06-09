using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.ReservationExceptions;

namespace DepoQuick.Domain;

public class Reservation
{
    private const string ReservationWithEmptyMessage = "El mensaje no debe ser vacio";
    private const string ReservationMessageHasMoreThan300Characters = "El mensaje no debe tener un largo mayor a 300 caracteres";
    
    
    private const int ReservationAccepted = 1;
    private const int ReservationPending = 0;
    private const int ReservationRejected = -1;
    
    private const int MinimumMessageLength = 1;
    private const int MaximumMessageLength = 300;
    
    private const string DefaultMessage = "-";
    
    
    [Key]
    public int Id { get; set; }
    
    public Deposit Deposit  { get; set; }
    public Client Client { get; set; }
    public DateRange Date { get; set; }
    public int Status { get; set; }
    
    private string _message;
    
    public string Message
    {
        get => _message;
        set
        {
            ValidateMessage(value);
            _message = value;
        }
    }
    
    public Reservation()
    {
        Message = DefaultMessage;
        
        Status = ReservationPending;
    }
    
    public Reservation(Deposit deposit, Client client, DateRange date)
    {
        Deposit = deposit;
        
        Client = client;
        Date = date;

        Message = DefaultMessage;
        Status = ReservationPending;
    }

    private void ValidateMessage(string expectedMessage)
    {
        if (MessageIsEmpty(expectedMessage))
        {
            throw new ReservationWithEmptyMessageException(ReservationWithEmptyMessage);
        }
        
        if (MessageHasMoreThanMaxCharacters(expectedMessage))
        {
            throw new ReservationMessageHasMoreThan300CharactersException(ReservationMessageHasMoreThan300Characters);
        }
    }

    private bool MessageHasMoreThanMaxCharacters(string expectedMessage)
    {
        return expectedMessage.Length > MaximumMessageLength;
    }

    private bool MessageIsEmpty(string expectedMessage)
    {
        return string.IsNullOrWhiteSpace(expectedMessage);
    }
}