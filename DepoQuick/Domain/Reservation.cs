using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.ReservationExceptions;

namespace DepoQuick.Domain;

public class Reservation
{
    public int Id { get; set; }
    
    public Deposit Deposit  { get; set; }
    
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public DateRange Date { get; set; }
    public int Status { get; set; }
   // public Rating Rating { get; set; }
    
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
       Message = "-";
        
        Status = 0;
    }

    //ver
    public Reservation(Deposit deposit, Client client, DateRange date)
    {
        Deposit = deposit;
        
        Client = client;
        Date = date;
        
        Message = "-";
        Status = 0;
    }

    private void ValidateMessage(string expectedMessage)
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
    }

    private bool MessageHasMoreThan300Characters(string expectedMessage)
    {
        return expectedMessage.Length > 300;
    }

    private bool MessageIsEmpty(string expectedMessage)
    {
        return string.IsNullOrWhiteSpace(expectedMessage);
    }
}