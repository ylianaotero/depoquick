using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;
namespace BusinessLogic;

public class LogController
{
    private IRepository<LogEntry> _logRepository;
    
    public LogController(IRepository<LogEntry> logRepository)
    {
        _logRepository = logRepository;
    }
    
    public void LogAction(User user, string message, DateTime timestamp)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new EmptyActionLogException("El mensaje no puede estar vacío.");
        }
        else
        {
            LogEntry log = new LogEntry()
            {
                Message = message,
                Timestamp = timestamp,
                UserId = user.Id
            };

            Add(log); 
        }
    }
    
    public List<LogEntry> GetLogs(User userToGetLogs, User activeUser)
    {
        if (activeUser.IsAdministrator)
        {
            return _logRepository.GetBy(log => log.UserId == userToGetLogs.Id);
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede ver los logs");
        }
        
    }
    
    public List<LogEntry> GetAllLogs(User activeUser)
    {
        if (activeUser.IsAdministrator)
        {
            return _logRepository.GetAll();
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede ver los logs");
        }
    }
    
    private void Add(LogEntry newLog)
    {
        _logRepository.Add(newLog);
    }
}