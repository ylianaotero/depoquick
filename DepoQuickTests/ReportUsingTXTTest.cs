using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.UserControllerExceptions;
using BusinessLogic.ReservationReport;
using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class ReportUsingTXTTest
{
        private ReservationController _reservationController;
        private PaymentController _paymentController;
        private UserController _userController;
        private DepoQuickContext _context;
        private SessionController _sessionController;
        private NotificationController _notificationController;
        
        private const string AdminName = "Administrator";
        private const string AdminEmail = "administrator@domain.com";
        private const string AdminPassword = "Password1#";
        
        private const string ClientName = "Client";
        private const string ClientEmail = "client@domain.com";
        private const string ClientPassword = "Password2#";
        
        private const string DepositValidName = "Deposito";
        private const char DepositArea0 = 'A';
        private const string DepositSize0 = "Pequeño";
        private const bool DepositAirConditioning0 = true;
        
        private LogController _logController;

        private DepositController _depositController; 

        private ReportUsingTXT _reportUsingTxt; 
        
        private Client _client;
        private Deposit _deposit0;
        private DateRange _validDateRange;
        private Reservation _reservation; 
            
    [TestInitialize]
    public void TestInitialize()
    {
        _context = TestContextFactory.CreateContext();
        
        IRepository<User> _userRepository = new SqlRepository<User>(_context);
        
        IRepository<Reservation> _reservationRepository = new SqlRepository<Reservation>(_context);
        
        IRepository<Payment> _paymentRepository = new SqlRepository<Payment>(_context);
        
        IRepository<Deposit> _depositRepository = new SqlRepository<Deposit>(_context);
        
        _userController = new UserController(_userRepository);
        
        _logController = new LogController(new SqlRepository<LogEntry>(_context));
        
        _sessionController = new SessionController(_userController, _logController);
        
        _paymentController = new PaymentController(_paymentRepository);
            
        _notificationController = new NotificationController(new SqlRepository<Notification>(_context));
        
        _reservationController = new ReservationController(_reservationRepository,_sessionController,_paymentController,_notificationController  );

        _depositController = new DepositController(_depositRepository, _sessionController);
        
       
        _reportUsingTxt = new ReportUsingTXT(_reservationController, _paymentController); 
        
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        List<Promotion> emptyListOfPromotions = new List<Promotion>(); 
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);

        _deposit0 = new Deposit(DepositValidName, DepositArea0, DepositSize0, DepositAirConditioning0);
        
        _depositController.AddDeposit(_deposit0,emptyListOfPromotions );
        
        _sessionController.LogoutUser();
        
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword,ClientPassword);
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        
        _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
        
        _reservation = new Reservation(_deposit0, _client, _validDateRange);
        
        _reservationController.AddPrice(_reservation, 100);
    
        _reservationController.Add(_reservation);
        
        _sessionController.LogoutUser();
        
    }
    
    [TestMethod]
    public void TestCreateReportUsingCSV()
    {
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        String report = _reportUsingTxt.GenerateReport();

        String depositId = _deposit0.Id.ToString();

        String initialDate = _reservation.Date.InitialDate.ToString("dd/MM/yyyy"); 
        
        String finalDate = _reservation.Date.FinalDate.ToString("dd/MM/yyyy");

        String clientEmail = _reservation.Client.Email;
        
        String reservationPrice = _reservation.Price.ToString();

        String promotionHasBeenApplied = false.ToString();

        String paymentStatus = "No pagado"; 
        
        Assert.IsTrue(report.Contains(initialDate));
        Assert.IsTrue(report.Contains(finalDate));
        Assert.IsTrue(report.Contains(clientEmail));
        Assert.IsTrue(report.Contains(paymentStatus));
        Assert.IsTrue(report.Contains(depositId));
        Assert.IsTrue(report.Contains(reservationPrice));
        Assert.IsTrue(report.Contains(promotionHasBeenApplied));
        Assert.IsTrue(report.Contains("\t"));

    }
    
}