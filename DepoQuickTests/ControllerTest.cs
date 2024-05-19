using BusinessLogic;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using Client = DepoQuick.Domain.Client;
using DateTime = System.DateTime;

namespace DepoQuickTests;

[TestClass]
public class ControllerTest
{
    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    private const string ClientName = "Client";
    private const string ClientEmail = "client@domain.com";
    private const string ClientPassword = "Password2#";
    
    private const char DepositArea0 = 'A';
    private const string DepositSize0 = "Pequeño";
    private const bool DepositAirConditioning0 = true;
    private const char DepositArea1 = 'B';
    private const string DepositSize1 = "Grande";
    private const bool DepositAirConditioning1 = false;
    
    private const string PromotionLabel0 = "Promotion 0";
    private const double PromotionDiscountRate0 = 0.5;
    
    private const int ApprovedReservationState = 1;
    private const int PendingReservationState = 0;
    private const int RejectedReservationState = -1;
    
    private const string UserLogInLogMessage = "Ingresó al sistema";
    private const string UserLogOutLogMessage = "Cerró sesión";
    
    private Client _client;
    private Deposit _deposit0;
    private DateRange _expiredDateRange;
    private DateRange _currentDateRange;
    private DateRange _validDateRange;

    [TestInitialize]
    public void TestInitialize()
    {
        _client = new Client(ClientName, ClientEmail, ClientPassword);
        
        _deposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        
        _expiredDateRange = new DateRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5));
        _currentDateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));
        _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
    }
    
    [TestMethod]
    [ExpectedException(typeof(EmptyAdministratorException))]
    public void TestEmptyUserList()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        controller.GetAdministrator();
    }
    
    [TestMethod]
    public void TestRegisterClient()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        controller.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        controller.LoginUser(ClientEmail, ClientPassword);
        
        CollectionAssert.Contains(controller.GetUsers(), controller.GetActiveUser());
    }
    
    [TestMethod]
    public void TestUserExistsInListOfUsers()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        Assert.AreEqual(true, controller.UserExists(AdminEmail));
    }

    [TestMethod]
    [ExpectedException(typeof(UserAlreadyExistsException))]
    public void TestInvalidRegisterClient()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        controller.RegisterClient(ClientName, AdminEmail, ClientPassword, ClientPassword);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotCreateClientBeforeAdminException))]
    public void TestCannotCreateClientBeforeAdmin()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
    }
    
    [TestMethod]
    [ExpectedException(typeof(AdministratorAlreadyExistsException))]
    public void TestAdministratorAlreadyExistsException()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        controller.RegisterAdministrator(ClientName, ClientEmail, ClientPassword, ClientPassword);
    }
    
    [TestMethod]
    public void TestLoginValidAdministrator()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        controller.LoginUser(AdminEmail, AdminPassword);
        
        Assert.AreEqual(controller.GetAdministrator(), controller.GetActiveUser());
    }
    
    [TestMethod]
    public void TestValidUserLogInLogin()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        controller.LoginUser(AdminEmail, AdminPassword);
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
        List<LogEntry> logs = controller.GetActiveUser().Logs;
        
        Assert.IsTrue(logs.Any(log => log.Message == UserLogInLogMessage));
        Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date 
                                      && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
    }
    
    [TestMethod]
    public void TestUserLoggedIn()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        controller.LoginUser(AdminEmail, AdminPassword);
        
        Assert.AreEqual(true, controller.UserLoggedIn());
    }

    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestInvalidLogin()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        controller.LoginUser(ClientEmail, ClientPassword);
    }

    [TestMethod]
    [ExpectedException(typeof(UserPasswordIsInvalidException))]
    public void TestInvalidLoginBecauseOfWrongPassword()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        controller.LoginUser(AdminEmail, ClientPassword);
    }

    [TestMethod]
    public void TestLogoutUser()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        controller.LoginUser(AdminEmail, AdminPassword);
        
        controller.LogoutUser();
        
        Assert.AreEqual(null, controller.GetActiveUser());
    }
    
    [TestMethod]
    public void TestValidLogInLogout()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        controller.LoginUser(AdminEmail, AdminPassword);
        
        List<LogEntry> logs = controller.GetActiveUser().Logs;
        controller.LogoutUser();
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
        
        Assert.IsTrue(logs.Any(log => log.Message == UserLogOutLogMessage));
        Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date && 
                                      now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
    }
    
    [TestMethod]
    public void TestAddValidDeposit()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Promotion promotion = new Promotion();
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        promotionsToAddToDeposit.Add(promotion);

        controller.AddDeposit(newDeposit, promotionsToAddToDeposit);

        CollectionAssert.Contains(controller.GetDeposits(), newDeposit);
        CollectionAssert.Contains(controller.GetDeposits()[0].Promotions, promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotAddDeposit()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        controller.LoginUser(ClientEmail,ClientPassword);
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Promotion promotion = new Promotion();
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        promotionsToAddToDeposit.Add(promotion);

        controller.AddDeposit(newDeposit, promotionsToAddToDeposit);
    }
    
    [TestMethod]
    public void TestSearchForADepositById()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Deposit newDeposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit1 = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        controller.AddDeposit(newDeposit0, promotionsToAddToDeposit);
        controller.AddDeposit(newDeposit1, promotionsToAddToDeposit);
        
        int idDeposit1 = newDeposit1.Id;

        Assert.AreEqual(char.ToUpper(DepositArea1), controller.GetDeposit(idDeposit1).Area);
        Assert.AreEqual(DepositSize1.ToUpper(), controller.GetDeposit(idDeposit1).Size);
        Assert.AreEqual(DepositAirConditioning1, controller.GetDeposit(idDeposit1).AirConditioning);
        Assert.AreEqual(false, controller.GetDeposit(idDeposit1).IsReserved());
        Assert.AreEqual(idDeposit1, controller.GetDeposit(idDeposit1).Id);
    }

    [TestMethod]
    [ExpectedException(typeof(DepositNotFoundException))]
    public void TestSearchForADepositUsingAnInvalidId()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Deposit newDeposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit1 = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        controller.AddDeposit(newDeposit0, promotionsToAddToDeposit);
        controller.AddDeposit(newDeposit1, promotionsToAddToDeposit);

        controller.GetDeposit(-34);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositNotFoundException))]
    public void TestDeleteDeposit()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        controller.AddDeposit(deposit, promotionsToAddToDeposit);
        int id = deposit.Id;

        controller.DeleteDeposit(id);
        controller.GetDeposit(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotDeleteDeposit()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Promotion promotion = new Promotion();
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        promotionsToAddToDeposit.Add(promotion);
        controller.AddDeposit(newDeposit, promotionsToAddToDeposit);
        
        controller.LoginUser(ClientEmail,ClientPassword);
        controller.DeleteDeposit(newDeposit.Id);
        
    }
    
    [TestMethod]
    public void TestDeleteDepositRemovesDepositFromRelatedPromotions()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        int depositId = deposit.Id;
        List<Deposit> deposits = new List<Deposit>();
        deposits.Add(deposit);
        
        Promotion promotion = new Promotion();
        List<Promotion> promotions = new List<Promotion>();
        promotions.Add(promotion);
        
        controller.AddDeposit(deposit, promotions);
        controller.AddPromotion(promotion, deposits);

        controller.DeleteDeposit(depositId);

        CollectionAssert.DoesNotContain(promotion.Deposits, deposit);
    }
    
    [TestMethod]
    public void TestAddNewPromotion()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _validDateRange;
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        depositsToAddToPromotion.Add(deposit);
        
        controller.AddPromotion(promotion, depositsToAddToPromotion);

        CollectionAssert.Contains(controller.GetPromotions(), promotion);
        CollectionAssert.Contains(controller.GetPromotions()[0].Deposits, deposit);
        CollectionAssert.Contains(deposit.Promotions, promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotAddPromotion()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        controller.LoginUser(ClientEmail,ClientPassword);
        Promotion promotion = new Promotion();
        promotion.DiscountRate = PromotionDiscountRate0;
        promotion.Label = PromotionLabel0;
        promotion.ValidityDate = _validDateRange;
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        depositsToAddToPromotion.Add(deposit);
        
        controller.AddPromotion(promotion, depositsToAddToPromotion);
    }

    [TestMethod]
    public void TestSearchForAPromotionById()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion1 = new Promotion();
        Promotion promotion2 = new Promotion();
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        depositsToAddToPromotion.Add(deposit);

        controller.AddPromotion(promotion1, depositsToAddToPromotion);
        controller.AddPromotion(promotion2, depositsToAddToPromotion);
        
        int promotion1Id = promotion1.Id;

        Assert.AreEqual(promotion1, controller.GetPromotion(promotion1Id));
    }

    [TestMethod]
    [ExpectedException(typeof(PromotionNotFoundException))]
    public void TestSearchForAPromotionUsingAnInvalidId()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion1 = new Promotion();
        Promotion promotion2 = new Promotion();
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        depositsToAddToPromotion.Add(deposit);

        controller.AddPromotion(promotion1, depositsToAddToPromotion);
        controller.AddPromotion(promotion2, depositsToAddToPromotion);

        controller.GetPromotion(-4);
    }
    
    [TestMethod]
    public void TestUpdatePromotion()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion1 = new Promotion();
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);
        controller.AddPromotion(promotion1, depositsToAddToPromotion);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
        newDepositsToAddPromotion.Add(newDeposit);
        String newLabel = PromotionLabel0;
        double newDiscountRate = PromotionDiscountRate0;
        DateRange newDateRange = _validDateRange;

        controller.UpdatePromotionData(promotion1, newLabel, newDiscountRate, newDateRange);
        controller.UpdatePromotionDeposits(promotion1, newDepositsToAddPromotion);
        
        CollectionAssert.Contains(controller.GetPromotions(), promotion1);
        CollectionAssert.DoesNotContain(promotion1.Deposits, _deposit0);
        CollectionAssert.Contains(promotion1.Deposits, newDeposit);
        CollectionAssert.Contains(newDeposit.Promotions, promotion1);
        CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion1);
        Assert.AreEqual(newLabel, promotion1.Label);
        Assert.AreEqual(newDiscountRate, promotion1.DiscountRate);
        Assert.AreEqual(newDateRange, promotion1.ValidityDate);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotUpdatePromotionData()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion1 = new Promotion();
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);
        controller.AddPromotion(promotion1, depositsToAddToPromotion);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
        newDepositsToAddPromotion.Add(newDeposit);
        String newLabel = PromotionLabel0;
        double newDiscountRate = PromotionDiscountRate0;
        DateRange newDateRange = _validDateRange;

        controller.LogoutUser();
        controller.LoginUser(ClientEmail,ClientPassword);
        controller.UpdatePromotionData(promotion1, newLabel, newDiscountRate, newDateRange);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotUpdatePromotion()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion1 = new Promotion();
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);
        controller.AddPromotion(promotion1, depositsToAddToPromotion);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
        newDepositsToAddPromotion.Add(newDeposit);

        controller.LogoutUser();
        controller.LoginUser(ClientEmail,ClientPassword);
        controller.UpdatePromotionDeposits(promotion1, newDepositsToAddPromotion);
    }

    [TestMethod]
    [ExpectedException(typeof(PromotionNotFoundException))]
    public void TestDeletePromotion()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        List<Deposit> depositsToAddPromotion = new List<Deposit>();
        depositsToAddPromotion.Add(_deposit0);
        controller.AddPromotion(promotion, depositsToAddPromotion);
        int id = promotion.Id;

        controller.DeletePromotion(id);

        controller.GetPromotion(id);
    }

    [TestMethod]
    public void TestDeletePromotionRemovesPromotionFromRelatedDeposits()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);
        controller.AddPromotion(promotion, depositsToAddToPromotion);
        int id = promotion.Id;

        controller.DeletePromotion(id);

        CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotDeletePromotion()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion = new Promotion();
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);
        controller.AddPromotion(promotion, depositsToAddToPromotion);
        int id = promotion.Id;
    
        controller.LogoutUser();
        controller.LoginUser(ClientEmail,ClientPassword);
        controller.DeletePromotion(id);

        CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion);
    }
    
    [TestMethod]
    public void TestDeleteAllExpiredPromotions()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        Promotion promotion1 = new Promotion();
        Promotion promotion2 = new Promotion();
        promotion1.ValidityDate = _validDateRange;
        promotion2.ValidityDate = _expiredDateRange;
        List<Deposit> depositsToAddToPromotion = new List<Deposit>();
        depositsToAddToPromotion.Add(_deposit0);
        controller.AddPromotion(promotion1, depositsToAddToPromotion);
        controller.AddPromotion(promotion2, depositsToAddToPromotion);

        controller.DeleteAllExpiredPromotions();

        CollectionAssert.Contains(controller.GetPromotions(), promotion1);
        CollectionAssert.DoesNotContain(controller.GetPromotions(), promotion2);
    }
    
    [TestMethod]
    public void TestAddAReservationToController()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        Reservation reservation = new Reservation(_deposit0, _client, _validDateRange);

        controller.AddReservation(reservation);

        CollectionAssert.Contains(controller.GetReservations(), reservation);
        CollectionAssert.Contains(_client.GetReservations(), reservation);
    }

    [TestMethod]
    public void TestSearchForAReservationById()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
        Reservation reservation2 = new Reservation(_deposit0, _client, _expiredDateRange);
        int reservation1Id = reservation1.Id;

        controller.AddReservation(reservation1);
        controller.AddReservation(reservation2);

        Assert.AreEqual(_deposit0, controller.GetReservation(reservation1Id).GetDeposit());
        Assert.AreEqual(_client, controller.GetReservation(reservation1Id).GetClient());
        Assert.AreEqual(_validDateRange, controller.GetReservation(reservation1Id).GetDateRange());
        Assert.AreEqual(reservation1Id, controller.GetReservation(reservation1Id).Id);
    }

    [TestMethod]
    [ExpectedException(typeof(ReservationNotFoundException))]
    public void TestSearchForAReservationUsingAnInvalidId()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
        Reservation reservation2 = new Reservation(_deposit0, _client, _expiredDateRange);

        controller.AddReservation(reservation1);
        controller.AddReservation(reservation2);

        controller.GetReservation(-34);
    }
    
    [TestMethod]
    public void TestApproveReservation()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        controller.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        controller.LoginUser(ClientEmail, ClientPassword);
        Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
        controller.AddReservation(reservation);

        controller.LoginUser(AdminEmail, AdminPassword);
        controller.ApproveReservation(reservation);

        Assert.AreEqual(ApprovedReservationState, reservation.GetState());
        Assert.AreEqual(true, reservation.GetDeposit().IsReserved());
    }

    [TestMethod]
    public void TestRejectReservation()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        controller.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        controller.LoginUser(ClientEmail, ClientPassword);
        Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
        controller.AddReservation(reservation);

        controller.LoginUser(AdminEmail, AdminPassword);
        controller.RejectReservation(reservation, "No hay disponibilidad");

        Assert.AreEqual(RejectedReservationState, reservation.GetState());
        Assert.AreEqual("No hay disponibilidad", reservation.GetMessage());
        Assert.AreEqual(false, _deposit0.IsReserved());
    }

    [TestMethod]
    public void TestCancelRejectionOfReservation()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        controller.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        controller.LoginUser(ClientEmail, ClientPassword);
        Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
        controller.AddReservation(reservation);

        controller.LoginUser(AdminEmail, AdminPassword);
        controller.CancelRejectionOfReservation(reservation);

        Assert.AreEqual(PendingReservationState, reservation.GetState());
        Assert.AreEqual("", reservation.GetMessage());
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotCancelRejectionOfReservation()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        controller.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        controller.LoginUser(ClientEmail, ClientPassword);
        Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
        controller.AddReservation(reservation);
        
        controller.CancelRejectionOfReservation(reservation);

        Assert.AreEqual(PendingReservationState, reservation.GetState());
        Assert.AreEqual("", reservation.GetMessage());
    }

    [TestMethod]
    public void TestRateReservation()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
        controller.AddReservation(reservation);
        Rating rating = new Rating(5, "Excelente");

        controller.LoginUser(ClientEmail,ClientPassword);
        controller.RateReservation(reservation, rating);

        CollectionAssert.Contains(_deposit0.Ratings, rating);
        CollectionAssert.Contains(controller.GetRatings(), rating);
        Assert.AreEqual(reservation.GetRating(), rating);
    }
    
    [TestMethod]
    public void TestRateReservationLog()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
        controller.AddReservation(reservation);
        Rating rating = new Rating(5, "Excelente");

        controller.LoginUser(ClientEmail,ClientPassword);
        controller.RateReservation(reservation, rating);
        
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
        List<LogEntry> logs = controller.GetActiveUser().Logs;
        
        Assert.IsTrue(logs.Any(log => log.Message == UserLogInLogMessage));
        Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date 
                                      && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToClientException))]
    public void TestAdministratorCannotRateReservation()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
        controller.AddReservation(reservation);
        Rating rating = new Rating(5, "Excelente");

        controller.LoginUser(AdminEmail,AdminPassword);
        controller.RateReservation(reservation, rating);

        CollectionAssert.Contains(_deposit0.Ratings, rating);
        CollectionAssert.Contains(controller.GetRatings(), rating);
        Assert.AreEqual(reservation.GetRating(), rating);
    }

    [TestMethod]
    public void TestGetLogs()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.LoginUser(AdminEmail,AdminPassword);
        controller.LogoutUser();
        controller.LoginUser(AdminEmail,AdminPassword);
        Assert.AreEqual(3,controller.GetLogs(controller.GetActiveUser()).Count());
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotGetLogs()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        
        controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
        controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        controller.LoginUser(ClientEmail,ClientPassword);
        controller.LogoutUser();
        controller.LoginUser(ClientEmail,ClientPassword);
        Assert.AreEqual(3,controller.GetLogs(controller.GetActiveUser()).Count());
    }
}