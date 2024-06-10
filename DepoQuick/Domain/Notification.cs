using System.ComponentModel.DataAnnotations;

namespace DepoQuick.Domain;

public class Notification
{
    [Key]
    public int Id { get; set; }
    public string Message { get; set; }

    public DateTime Timestamp { get; set; }
    
    public Client Client { get; set; }
    
    public Notification()
    {
        Timestamp = DateTime.Now;
    }
}