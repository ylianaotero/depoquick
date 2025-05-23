﻿using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.PromotionControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;
namespace DepoQuickTests;

[TestClass]
public class PromotionControllerTest
{
    private PromotionController _promotionController;
    private UserController _userController;
    private SessionController _sessionController;
    private ReservationController _reservationController;
    private PaymentController _paymentController;
    private NotificationController _notificationController;
    private DepositController _depositController;
    private DepoQuickContext _context;
    private LogController _logController;

    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    private const string ClientName = "Client";
    private const string ClientEmail = "client@domain.com";
    private const string ClientPassword = "Password2#";

    private const string DepositName = "Deposito";
    private const char DepositArea0 = 'A';
    private const string DepositSize0 = "Pequeño";
    private const bool DepositAirConditioning0 = true;

    private const string PromotionLabel0 = "Promotion 0";

    private const double PromotionDiscountRate0 = 0.5;

    private Client _client;
    private Deposit _deposit0;
    private DateRange _expiredDateRange;
    private DateRange _currentDateRange;
    private DateRange _validDateRange;

    [TestInitialize]
    public void Initialize()
    {
        _context = TestContextFactory.CreateContext();
        IRepository<User> _userRepository = new SqlRepository<User>(_context);
        
        _userController = new UserController(_userRepository);
        _logController = new LogController(new SqlRepository<LogEntry>(_context));
        _sessionController = new SessionController(_userController, _logController);

        _depositController = new DepositController(new SqlRepository<Deposit>(_context), _sessionController);
            
        _promotionController = new PromotionController(new SqlRepository<Promotion>(_context), _sessionController, _depositController);
        
        _paymentController = new PaymentController(new SqlRepository<Payment>(_context));
        
        _notificationController = new NotificationController(new SqlRepository<Notification>(_context));
        
        _reservationController = new ReservationController(new SqlRepository<Reservation>(_context), _sessionController,_paymentController, _notificationController);

        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

        _deposit0 = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);


        _currentDateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));
        _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
        _expiredDateRange = new DateRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5));
    }
    
    [TestMethod]
    public void TestAddNewPromotion()
    {
        _sessionController.LoginUser(AdminEmail, AdminPassword);

        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _validDateRange;

        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        depositsToAddToPromotion.Add(deposit);
        _context.Deposits.Add(deposit);

        _promotionController.Add(promotion, depositsToAddToPromotion);
        
        List<Deposit> deposits = _depositController.GetDepositsByPromotion(promotion);

        CollectionAssert.Contains(_promotionController.GetPromotions(), promotion);
        CollectionAssert.Contains(deposits, deposit);
        CollectionAssert.Contains(deposit.Promotions, promotion);
    }

    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotAddPromotion()
    {
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _sessionController.LoginUser(ClientEmail, ClientPassword);

        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _validDateRange;
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        depositsToAddToPromotion.Add(deposit);

        _promotionController.Add(promotion, depositsToAddToPromotion);
    }

    [TestMethod]
    public void TestSearchForAPromotionById()
    {
        _sessionController.LoginUser(AdminEmail, AdminPassword);
        Promotion promotion1 = new Promotion();
        Promotion promotion2 = new Promotion();
        promotion1.DiscountRate = PromotionDiscountRate0;
        promotion1.Label = PromotionLabel0;
        promotion1.ValidityDate = _validDateRange;

        promotion2.DiscountRate = PromotionDiscountRate0;
        promotion2.Label = PromotionLabel0;
        promotion2.ValidityDate = _currentDateRange;

        List<Deposit> depositsToAddToPromotion = new List<Deposit>();

        Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        _context.Deposits.Add(deposit);
        depositsToAddToPromotion.Add(deposit);

        _promotionController.Add(promotion1, depositsToAddToPromotion);
        _promotionController.Add(promotion2, depositsToAddToPromotion);
        Assert.AreEqual(promotion1, _promotionController.Get(promotion1.Id));
    }

    [TestMethod]
    [ExpectedException(typeof(PromotionNotFoundException))]
    public void TestSearchForAPromotionUsingAnInvalidId()
    {
        _sessionController.LoginUser(AdminEmail, AdminPassword);
        Promotion promotion1 = new Promotion();
        Promotion promotion2 = new Promotion();
        promotion1.DiscountRate = PromotionDiscountRate0;
        promotion1.Label = PromotionLabel0;
        promotion1.ValidityDate = _validDateRange;

        promotion2.DiscountRate = PromotionDiscountRate0;
        promotion2.Label = PromotionLabel0;
        promotion2.ValidityDate = _currentDateRange;

        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        _context.Deposits.Add(deposit);
        depositsToAddToPromotion.Add(deposit);

        _promotionController.Add(promotion1, depositsToAddToPromotion);
        _promotionController.Add(promotion2, depositsToAddToPromotion);

        _promotionController.Get(-4);
    }

    [TestMethod]
    public void TestUpdatePromotion()
    {
        _sessionController.LoginUser(AdminEmail, AdminPassword);
        Promotion promotion1 = new Promotion();
        promotion1.DiscountRate = PromotionDiscountRate0;
        promotion1.Label = PromotionLabel0;
        promotion1.ValidityDate = _validDateRange;

        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);
        _context.Deposits.Add(_deposit0);
        _promotionController.Add(promotion1, depositsToAddToPromotion);

        Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);

        List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
        newDepositsToAddPromotion.Add(deposit);
        String newLabel = PromotionLabel0;
        double newDiscountRate = PromotionDiscountRate0;
        DateRange newDateRange = _validDateRange;

        _promotionController.UpdatePromotionData(promotion1, newLabel, newDiscountRate, newDateRange);
        _promotionController.UpdatePromotionDeposits(promotion1, newDepositsToAddPromotion);
        
        List<Deposit> deposits = _depositController.GetDepositsByPromotion(promotion1);

        CollectionAssert.Contains(_promotionController.GetPromotions(), promotion1);
        CollectionAssert.DoesNotContain(deposits, _deposit0);
        CollectionAssert.Contains(deposits, deposit);
        CollectionAssert.Contains(deposit.Promotions, promotion1);
        CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion1);
        Assert.AreEqual(newLabel, promotion1.Label);
        Assert.AreEqual(newDiscountRate, promotion1.DiscountRate);
        Assert.AreEqual(newDateRange, promotion1.ValidityDate);
    }

    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotUpdatePromotionData()
    {
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _sessionController.LoginUser(AdminEmail, AdminPassword);
        Promotion promotion1 = new Promotion();
        promotion1.DiscountRate = PromotionDiscountRate0;
        promotion1.Label = PromotionLabel0;
        promotion1.ValidityDate = _validDateRange;

        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit0 = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        _context.Deposits.Add(deposit0);
        depositsToAddToPromotion.Add(deposit0);

        _promotionController.Add(promotion1, depositsToAddToPromotion);

        List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
        Deposit deposit1 = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        _context.Deposits.Add(deposit1);
        newDepositsToAddPromotion.Add(deposit1);

        String newLabel = PromotionLabel0;
        double newDiscountRate = PromotionDiscountRate0;
        DateRange newDateRange = _currentDateRange;

        _sessionController.LogoutUser();
        _sessionController.LoginUser(ClientEmail, ClientPassword);
        _promotionController.UpdatePromotionData(promotion1, newLabel, newDiscountRate, newDateRange);
    }

    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotUpdatePromotion()
    {
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _sessionController.LoginUser(AdminEmail, AdminPassword);

        Promotion promotion1 = new Promotion();
        promotion1.DiscountRate = PromotionDiscountRate0;
        promotion1.Label = PromotionLabel0;
        promotion1.ValidityDate = _validDateRange;

        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit0 = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        _context.Deposits.Add(deposit0);
        depositsToAddToPromotion.Add(deposit0);
        _promotionController.Add(promotion1, depositsToAddToPromotion);


        List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
        Deposit deposit1 = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        _context.Deposits.Add(deposit1);
        newDepositsToAddPromotion.Add(deposit1);

        _sessionController.LogoutUser();
        _sessionController.LoginUser(ClientEmail, ClientPassword);
        _promotionController.UpdatePromotionDeposits(promotion1, newDepositsToAddPromotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(PromotionNotFoundException))]
    public void TestDeletePromotion()
    {
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _validDateRange;
        
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        
        _promotionController.Add(promotion, depositsToAddToPromotion);
        
        int id = promotion.Id;
 
        _promotionController.Delete(promotion.Id);
 
        _promotionController.Get(id);
    }
    
    [TestMethod]
    public void TestDeletePromotionRemovesPromotionFromRelatedDeposits()
    { 
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _validDateRange;
        
        
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        _promotionController.Add(promotion,depositsToAddToPromotion);
        
        _promotionController.Delete(promotion.Id);
 
        CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotDeletePromotion()
    {
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _validDateRange;
        
        
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        //Deposit deposit0 = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
        //_context.Deposits.Add(deposit0);
        //depositsToAddToPromotion.Add(deposit0);
        _promotionController.Add(promotion,depositsToAddToPromotion);
     
        _sessionController.LogoutUser();
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        _promotionController.Delete(promotion.Id);
 
        CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion);
    }
    
    [TestMethod]
    public void TestPromotionIsTiedToReservedDeposit()
    { 
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _currentDateRange;
        
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);
        _promotionController.Add(promotion,depositsToAddToPromotion);
        
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        Client client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation = new Reservation(_deposit0,client,_currentDateRange);
        _reservationController.Add(reservation);
        Payment payment = new Payment();
        _paymentController.Add(payment);
        _reservationController.PayReservation(reservation);
        
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        _reservationController.ApproveReservation(reservation);
       
        Assert.AreEqual(true,_promotionController.PromotionIsTiedToReservedDeposit(promotion));
    }
    
    [TestMethod]
    public void TestPromotionIsNotTiedToReservedDeposit()
    { 
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _currentDateRange;
       
        Assert.AreEqual(false,_promotionController.PromotionIsTiedToReservedDeposit(promotion));
    }
    
    [TestMethod]
    public void TestPromotionIsNotTiedToReservedDepositBecauseIsRejected()
    { 
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _currentDateRange;
        
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);

        _promotionController.Add(promotion,depositsToAddToPromotion);
        
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        Client client = (Client)_userController.GetUserByEmail(ClientEmail);
        Reservation reservation = new Reservation(_deposit0,client,_currentDateRange);
        _reservationController.Add(reservation);
        
       
        Assert.IsFalse(_promotionController.PromotionIsTiedToReservedDeposit(promotion));
    }
    

}