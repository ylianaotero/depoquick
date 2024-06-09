using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.PaymentControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.PaymentExceptions;

namespace DepoQuickTests;

[TestClass]
public class PaymentControllerTest
{ 
    private PaymentController _paymentController;
    private DepoQuickContext _context;
    private ReservationController _reservationController;
    private UserController _userController;
    private LogController _logController;
    private NotificationController _notificationController;
    private PromotionController _promotionController;
    private DepositController _depositController;
    private SessionController _sessionController;
    
    private DateRange _currentDateRange;
    private Reservation _reservation;
    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    private const string DepositName = "Deposito";
    private const string ClientName1 = "Maria Perez";
    private const string ClientEmail1 = "maria@gmail.com";
    private const string ClientPassword1 = "Mariaaa1.";
    private const char DepositArea0 = 'A';
    private const string DepositSize0 = "Pequeño";
    private const bool DepositAirConditioning0 = true;
    private const char DepositArea1 = 'A';
    private const string DepositSize1 = "Pequeño";
    private const bool DepositAirConditioning1 = true;
    private const string PromotionLabel0 = "Promotion 0";
    private const double PromotionDiscountRate0 = 0.5;
    private Deposit _deposit0;
    
    [TestInitialize]
    public void Initialize()
    {
        _context = TestContextFactory.CreateContext();
        _userController = new UserController(new SqlRepository<User>(_context));
        _logController = new LogController(new SqlRepository<LogEntry>(_context));
        _sessionController = new SessionController(_userController, _logController);
        _paymentController = new PaymentController(new SqlRepository<Payment>(_context));
        _notificationController = new NotificationController(new SqlRepository<Notification>(_context));
        _depositController = new DepositController(new SqlRepository<Deposit>(_context),_sessionController);
        _reservationController = new ReservationController(new SqlRepository<Reservation>(_context),_sessionController,_paymentController,_notificationController);
        _promotionController = new PromotionController(new SqlRepository<Promotion>(_context),_sessionController,_depositController);
        
        _userController.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositName,DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        _reservation = new Reservation(deposit,client,stay);
        
        _deposit0 = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        _currentDateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));
    }
}
