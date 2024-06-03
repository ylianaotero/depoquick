using System.ComponentModel.DataAnnotations;

namespace DepoQuick.Domain;

public class Notification
{
    [Key]
    public int Id { get; set; }
    public string Message { get; init; }

    public DateTime Timestamp { get; init; }
    
    public Client Client { get; init; }
    
    public Reservation Reservation { get; init; }
}