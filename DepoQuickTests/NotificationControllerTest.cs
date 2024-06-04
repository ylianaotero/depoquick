using BusinessLogic;
using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class NotificationControllerTest
{
    private const string NotificationMessage = "Esta es una noti";
    
    
    private const char DepositArea0 = 'A';
    private const string DepositSize0 = "Pequeño";
    private const bool DepositAirConditioning0 = true;

    private NotificationController _notificationController;
    private UserController _userController;
    private ReservationController _reservationController; 
    private Session _session;
    
    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    
    private const string ClientName = "Client";
    private const string ClientEmail = "client@domain.com";
    private const string ClientPassword = "Password2#";
    
    private Client _client;
    private Deposit _deposit0;
    
    private DateRange _validDateRange;
    
    
    
    [TestInitialize]
    public void Initialize()
    {
        var context = TestContextFactory.CreateContext();
        
        IRepository<User> _userRepository = new SqlRepository<User>(context);
        _userController = new UserController(_userRepository);
        
        LogController _logController = new LogController(new SqlRepository<LogEntry>(context));
        
        _session = new Session(_userController, _logController);
        
        IRepository<Reservation> _reservationRepository = new SqlRepository<Reservation>(context);
            
        IRepository<Payment> _paymentRepository = new SqlRepository<Payment>(context);
        PaymentController _paymentController = new PaymentController(_paymentRepository);
        
        _notificationController = new NotificationController(new SqlRepository<Notification>(context));
        
        _reservationController = new ReservationController(_reservationRepository,_session,_paymentController,_notificationController );

        
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        
        _deposit0 = new Deposit (DepositArea0,DepositSize0,DepositAirConditioning0);
        
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        
        _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
        
    }
    
    
    [TestMethod]
    public void TestGetNotifications()
    {
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
            
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);

        _notificationController.Notify(_client, reservation1, NotificationMessage , DateTime.Now);

        List<Notification> listOfNotifications = _notificationController.GetNotifications(_client); 
        
        Assert.AreEqual(1,listOfNotifications.Count);
        Assert.AreEqual(listOfNotifications[0].Message , NotificationMessage );
        Assert.IsTrue(listOfNotifications.Any(log => now.Date == log.Timestamp.Date
                                                     && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
        Assert.AreEqual(listOfNotifications[0].Client , _client);
        CollectionAssert.Contains(_client.Notifications,listOfNotifications[0]);
    }
    
    [TestMethod]
    public void TestDeleteNotifications()
    {
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
            
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);

        _notificationController.Notify(_client, reservation1, NotificationMessage , DateTime.Now);

        List<Notification> listOfNotifications = _notificationController.GetNotifications(_client); 
        
        Assert.AreEqual(1,listOfNotifications.Count);
        Assert.AreEqual(listOfNotifications[0].Message , NotificationMessage );
        Assert.IsTrue(listOfNotifications.Any(log => now.Date == log.Timestamp.Date
                                                     && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
        Assert.AreEqual(listOfNotifications[0].Client , _client);
        CollectionAssert.Contains(_client.Notifications,listOfNotifications[0]);
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestDeleteNotificationsThatDoesntExist()
    {
        Notification notification = new Notification()
        {
            Id = 1,
            Client = _client,
            Message = "este es un mesnaje",
            Timestamp = DateTime.Now
        }; 
        
        _notificationController.Delete(notification);
        
        
    }
    
    
}