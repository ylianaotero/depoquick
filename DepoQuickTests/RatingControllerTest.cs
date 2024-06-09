using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.RatingControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class RatingControllerTest
{
    private const int stars = 5;
    private const String comment = "Excelente servicio"; 
    
    private const string DepositName = "Deposito";
    private const char DepositArea0 = 'A';
    private const string DepositSize0 = "Pequeño";
    private const bool DepositAirConditioning0 = true;

    private UserController _userController;
    private ReservationController _reservationController; 
    private SessionController _sessionController;
    private RatingController _ratingController; 
    
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
        
        _sessionController = new SessionController(_userController, _logController);
        
        IRepository<Reservation> _reservationRepository = new SqlRepository<Reservation>(context);
            
        IRepository<Rating> _ratingRepository = new SqlRepository<Rating>(context);
        
        IRepository<Payment> _paymentRepository = new SqlRepository<Payment>(context);
        
        PaymentController _paymentController = new PaymentController(_paymentRepository);
            
        NotificationController _notificationController = new NotificationController(new SqlRepository<Notification>(context));


        _reservationController = new ReservationController(_reservationRepository,_sessionController,_paymentController,_notificationController  );


        _ratingController = new RatingController(_ratingRepository, _sessionController, _logController); 
        
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        
        _deposit0 = new Deposit (DepositName,DepositArea0,DepositSize0,DepositAirConditioning0);
        
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        
        _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
        
    }
    
    [TestMethod]
    public void TestAddRating()
    {
        _sessionController.LoginUser(ClientEmail, ClientPassword);
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);
        
        Rating newRating = new Rating(); 
        newRating.Stars = stars;
        newRating.Comment = comment;
        
        _ratingController.RateReservation(reservation1, newRating); 
        
        CollectionAssert.Contains(_ratingController.GetRatingsByDeposit(_deposit0), newRating);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToClientException))]
    public void TestAdministratorCantAddRating()
    {
        _sessionController.LoginUser(AdminEmail, AdminPassword);
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);
        
        Rating newRating = new Rating(); 
        newRating.Stars = stars;
        newRating.Comment = comment;
        
        _ratingController.RateReservation(reservation1, newRating); 
        
    }
    
    [TestMethod]
    public void TestGetRatings()
    {
        _sessionController.LoginUser(ClientEmail, ClientPassword);
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);
        
        Rating newRating = new Rating(); 
        newRating.Stars = stars;
        newRating.Comment = comment;
        
        _ratingController.RateReservation(reservation1, newRating); 
        
        CollectionAssert.Contains(_ratingController.GetRatings(), newRating);
    }
    
    [TestMethod]
    public void TestGetRatingsByReservation()
    {
        _sessionController.LoginUser(ClientEmail, ClientPassword);
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);
        
        Rating newRating = new Rating(); 
        newRating.Stars = stars;
        newRating.Comment = comment;
        
        _ratingController.RateReservation(reservation1, newRating); 
        
        Assert.AreEqual(_ratingController.GetRatingByReservation(reservation1), newRating);
        
    }
    
    [TestMethod]
    public void TestUpdateRating()
    {
        _sessionController.LoginUser(ClientEmail, ClientPassword);
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);
        
        Rating newRating = new Rating(); 
        newRating.Stars = stars;
        newRating.Comment = comment;
        
        _ratingController.RateReservation(reservation1, newRating);
        _ratingController.UpdateRating(newRating, "new", 1); 
        
        Assert.AreEqual(_ratingController.GetRatingByReservation(reservation1).Stars, 1);
        Assert.AreEqual(_ratingController.GetRatingByReservation(reservation1).Comment, "new");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToClientException))]
    public void TestAdministratorCannotUpdateRating()
    {
        _sessionController.LoginUser(ClientEmail, ClientPassword);
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);
        
        Rating newRating = new Rating(); 
        newRating.Stars = stars;
        newRating.Comment = comment;
        
        _ratingController.RateReservation(reservation1, newRating);
        _sessionController.LogoutUser();
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        _ratingController.UpdateRating(newRating, "new", 1); 
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(RatingNotFoundException))]
    public void TestRatingNotFound()
    {
        _sessionController.LoginUser(ClientEmail, ClientPassword);
        _client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
        _reservationController.Add(reservation1);

        _ratingController.GetRatingByReservation(reservation1); 


    }
}