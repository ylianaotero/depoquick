using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepoQuick.Domain;

public class LogEntry
{
    [Key]
    public int Id { get; set; }
    public string Message { get; init; }

    public DateTime Timestamp { get; init; }
    
    public int UserId { get; init; }
    
}