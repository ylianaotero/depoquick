using DepoQuick.Domain;

namespace BusinessLogic;

public class NotificationController
{
    private readonly IRepository<Notification> _notificationRepository;
    
    public NotificationController(IRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    public void Notify(Client client,Reservation reservation ,string message, DateTime timestamp)
    {
        Notification notification = new Notification()
        {
            Message = message,
            Timestamp = timestamp,
            Client = client,
        };

        Add(notification); 
    }
    
    public List<Notification> GetNotifications(Client clientToGetNotification)
    {
        return _notificationRepository.GetBy(notification => notification.Client == clientToGetNotification);
    }
    
    private void Add(Notification notification)
    {
        _notificationRepository.Add(notification);
    }

    public void Delete(Notification notification)
    {
        try
        {
            _notificationRepository.Delete(notification.Id);
        }
        catch (NullReferenceException e)
        {
            throw new ArgumentNullException();
        }
    }
}