using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepoQuick.Domain;

public class LogEntry
{
    private static int s_lastId = 0;
    
    [Key]
    public int Id { get; init; }
    public string Message { get; init; }

    public DateTime Timestamp { get; init; }
    
    [ForeignKey("UserId")]
    public int UserId { get; init; }

    public LogEntry()
    {
        Id = s_lastId + 1;
        s_lastId = Id;
    }
}