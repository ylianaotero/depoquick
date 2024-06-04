using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepoQuick.Domain;

public class LogEntry
{
    private static int s_lastId = 0;
    
    [Key]
    public int Id { get; set; }
    public string Message { get; init; }

    public DateTime Timestamp { get; init; }
    
    [ForeignKey("UserId")]  //Deberia SER USER!!
    public int UserId { get; init; }
    
}