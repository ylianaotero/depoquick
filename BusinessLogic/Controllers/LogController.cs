using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;

namespace BusinessLogic.Controllers;

public class LogController
{
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    private const string EmptyActionLogExceptionMessage = "El mensaje no puede estar vacío";
    
    private IRepository<LogEntry> _logRepository;
    
    public LogController(IRepository<LogEntry> logRepository)
    {
        _logRepository = logRepository;
    }
    
    public void LogAction(User user, string message, DateTime timestamp)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new EmptyActionLogException(EmptyActionLogExceptionMessage);
        }
        
        LogEntry log = new LogEntry()
        {
            Message = message,
            Timestamp = timestamp,
            UserId = user.Id
        };

        Add(log); 
    }
    
    public List<LogEntry> GetLogs(User userToGetLogs, User activeUser)
    {
        if (!activeUser.IsAdministrator)
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
        
        return _logRepository.GetBy(log => log.UserId == userToGetLogs.Id);
    }
    
    public List<LogEntry> GetAllLogs(User activeUser)
    {
        if (!activeUser.IsAdministrator)
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
        
        return _logRepository.GetAll();
    }
    
    private void Add(LogEntry newLog)
    {
        _logRepository.Add(newLog);
    }
}