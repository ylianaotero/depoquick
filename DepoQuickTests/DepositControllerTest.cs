using System.Threading.Channels;
using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.DepositControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.DepositExceptions;
using SQLitePCL;

namespace DepoQuickTests;


[TestClass]
public class DepositControllerTest
{
    private DepositController _depositController;
    private UserController _userController;
    private LogController _logController;
    private PaymentController _paymentController;
    private NotificationController _notificationController;
    private ReservationController _reservationController;
    private SessionController _sessionController; 
    private DepoQuickContext _context;
    private DateRange _validDateRange;

    private const string DepositValidName = "Deposito";
    private const string DepositValidName2 = "DepositoCarrasco";
    private const string DepositValidName3 = "DepositoPocitos";
    private const string DepositInvalidName = "Desposito 1";
    private const char DepositArea0 = 'A';
    private const string DepositSize0 = "Pequeño";
    private const bool DepositAirConditioning0 = true;
    private const char DepositArea1 = 'B';
    private const string DepositSize1 = "Grande";
    private const bool DepositAirConditioning1 = false;
    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    
    private const string ClientName = "Client";
    private const string ClientEmail = "client@domain.com";
    private const string ClientPassword = "Password2#";
    
    [TestInitialize]
    public void Initialize()
    {
        _context = TestContextFactory.CreateContext();
        IRepository<User> _userRepository = new SqlRepository<User>(_context);
        
        _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
        _userController = new UserController(_userRepository);
        _logController = new LogController(new SqlRepository<LogEntry>(_context));
        _sessionController = new SessionController(_userController, _logController);
        _depositController = new DepositController(new SqlRepository<Deposit>(_context), _sessionController);
    }
    
    [TestMethod]
    public void TestAddValidDeposit()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion = new Promotion();
        promotion.Label = "promo";
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>(); 
        promotionsToAddToDeposit.Add(promotion);
          
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);


        CollectionAssert.Contains(_depositController.GetDeposits(), newDeposit);
        CollectionAssert.Contains(_depositController.Get(newDeposit.Id).Promotions, promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotAddDeposit()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion = new Promotion();
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        promotionsToAddToDeposit.Add(promotion);
 
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
    }
    
    [TestMethod]
    public void TestSearchForADepositById()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositValidName2,DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit1 = new Deposit(DepositValidName3,DepositArea1, DepositSize1, DepositAirConditioning1);
        

        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
        _depositController.AddDeposit(newDeposit0, promotionsToAddToDeposit);
        _depositController.AddDeposit(newDeposit1, promotionsToAddToDeposit);

        int idDeposit0 = newDeposit0.Id;

        Deposit deposit = _depositController.Get(idDeposit0);

        Assert.AreEqual(char.ToUpper(DepositArea0), deposit.Area);
        Assert.AreEqual(DepositSize0.ToUpper(), deposit.Size);
        Assert.AreEqual(DepositAirConditioning0, deposit.AirConditioning);  
        Assert.AreEqual(false, deposit.IsReserved());
        Assert.AreEqual(idDeposit0, deposit.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositNotFoundException))]
    public void TestSearchForADepositUsingAnInvalidId()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositValidName2,DepositArea0, DepositSize0, DepositAirConditioning0);
        

        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();

        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);

        _depositController.AddDeposit(newDeposit0, promotionsToAddToDeposit);

        Deposit deposit = _depositController.Get(-34); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositNotFoundException))]
    public void TestDeleteDeposit()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositValidName2,DepositArea0, DepositSize0, DepositAirConditioning0);
        

        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
        _depositController.AddDeposit(newDeposit0, promotionsToAddToDeposit);
        
        int id = newDeposit0.Id;

        _depositController.DeleteDeposit(id);
        _depositController.Get(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotDeleteDeposit()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositValidName2,DepositArea0, DepositSize0, DepositAirConditioning0);
        

        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
        _depositController.AddDeposit(newDeposit0, promotionsToAddToDeposit);
        
        _sessionController.LogoutUser();
        
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        
        _depositController.DeleteDeposit(newDeposit.Id);
         
    }
    
    [TestMethod]
    public void TestDeleteDepositRemovesDepositFromRelatedPromotions()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositValidName2,DepositArea0, DepositSize0, DepositAirConditioning0);
        

        Promotion promotion1 = new Promotion();
        promotion1.Label = "promo"; 
        Promotion promotion2 = new Promotion();
        promotion2.Label = "promo"; 
        List<Promotion> promotionsToAddToDeposit0 = new List<Promotion>();
        promotionsToAddToDeposit0.Add(promotion1);
        List<Promotion> promotionsToAddToDeposit1 = new List<Promotion>();
        promotionsToAddToDeposit1.Add(promotion2);
        
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit0);
        _depositController.AddDeposit(newDeposit0, promotionsToAddToDeposit1);
        
        int id = newDeposit0.Id;

        _depositController.DeleteDeposit(id);
        
        List<Deposit> deposits = _depositController.GetDepositsByPromotion(promotion2);

        CollectionAssert.DoesNotContain(deposits, newDeposit0);
    }

    
    [TestMethod]
    [ExpectedException(typeof(DepositDateIsOverlappingException))]
    public void TestDepositDateIsOverlapping()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion1 = new Promotion();
        promotion1.Label = "promo"; 
       
        List<Promotion> promotionsToAddToDeposit0 = new List<Promotion>();
        promotionsToAddToDeposit0.Add(promotion1);
        
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit0);
        
        _depositController.AddAvailabilityDate(newDeposit,_validDateRange);
        _depositController.AddAvailabilityDate(newDeposit,_validDateRange);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositDateIsAlreadyReservedException))]
    public void TestDepositDateIsNotAvailable()
    {
        IRepository<Reservation> _reservationRepository = new SqlRepository<Reservation>(_context);
        IRepository<Payment> _paymentRepository = new SqlRepository<Payment>(_context);
        _paymentController = new PaymentController(_paymentRepository);
        _notificationController = new NotificationController(new SqlRepository<Notification>(_context));
        _reservationController = new ReservationController(_reservationRepository, _sessionController, _paymentController, _notificationController);
        
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        _sessionController.LoginUser(AdminEmail,AdminPassword);

        Client client = (Client)_userController.GetUserByEmail(ClientEmail);
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion1 = new Promotion();
        promotion1.Label = "promo"; 
       
        List<Promotion> promotionsToAddToDeposit0 = new List<Promotion>();
        promotionsToAddToDeposit0.Add(promotion1);
        
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit0);
        
        _depositController.AddAvailabilityDate(newDeposit,_validDateRange);
        Reservation newReservation = new Reservation(newDeposit,client,_validDateRange);
        _reservationController.Add(newReservation);
        _reservationController.PayReservation(newReservation);
        _reservationController.ApproveReservation(newReservation);
        
        DateRange newDateRange = new DateRange(DateTime.Now.AddDays(9), DateTime.Now.AddDays(11));
        _depositController.AddAvailabilityDate(newDeposit,newDateRange);
    }
    
    [TestMethod]
    public void TestDepositExists()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion = new Promotion();
        promotion.Label = "promo";
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>(); 
        promotionsToAddToDeposit.Add(promotion);
          
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
        
        Assert.AreEqual(true, _depositController.DepositExists(newDeposit.Name));
    }
    
    [TestMethod]
    public void TestGetDepositByName()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion = new Promotion();
        promotion.Label = "promo";
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>(); 
        promotionsToAddToDeposit.Add(promotion);
          
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
        
        String depositName = _depositController.GetDepositByName(newDeposit.Name).Name;
        
        Assert.AreEqual(newDeposit.Name, depositName);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositNotFoundException))]
    public void TestCannotGetDepositByName()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion = new Promotion();
        promotion.Label = "promo";
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>(); 
        promotionsToAddToDeposit.Add(promotion);
          
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
        
        _depositController.DeleteDeposit(newDeposit.Id);
        String depositName = _depositController.GetDepositByName(newDeposit.Name).Name;
    }
    
    [TestMethod]
    public void TestGetAvailableDeposits()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        newDeposit.AvailableDates.Add(_validDateRange);
        Promotion promotion = new Promotion();
        promotion.Label = "promo";
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>(); 
        promotionsToAddToDeposit.Add(promotion);
          
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
        
        CollectionAssert.Contains(_depositController.AvailableDeposits(_validDateRange), newDeposit);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositNameIsNotValidException))]

    public void TestAddDepositWithEmptyName()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit("",DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion = new Promotion();
        promotion.Label = "promo";
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>(); 
        promotionsToAddToDeposit.Add(promotion);
          
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);


        CollectionAssert.Contains(_depositController.GetDeposits(), newDeposit);
        CollectionAssert.Contains(_depositController.Get(newDeposit.Id).Promotions, promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositNameAlreadyExistsException))]
    public void TestCannotAddDepositBecauseNameAlreadyExists()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositValidName,DepositArea0, DepositSize0, DepositAirConditioning0);
        

        Promotion promotion1 = new Promotion();
        promotion1.Label = "promo"; 
        Promotion promotion2 = new Promotion();
        promotion2.Label = "promo"; 
        List<Promotion> promotionsToAddToDeposit0 = new List<Promotion>();
        promotionsToAddToDeposit0.Add(promotion1);
        List<Promotion> promotionsToAddToDeposit1 = new List<Promotion>();
        promotionsToAddToDeposit1.Add(promotion2);
        
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit0);
        _depositController.AddDeposit(newDeposit0, promotionsToAddToDeposit1);
    }
}