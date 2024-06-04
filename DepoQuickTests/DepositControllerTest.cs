using BusinessLogic;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
namespace DepoQuickTests;


[TestClass]
public class DepositControllerTest
{
    private DepositController _depositController;
    private UserController _userController;
    private LogController _logController;
    private Session _session; 
    private DepoQuickContext _context;
    
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
        
        _userController = new UserController(_userRepository);
        _logController = new LogController(new SqlRepository<LogEntry>(_context));
        _session = new Session(_userController, _logController);
        _depositController = new DepositController(new SqlRepository<Deposit>(_context), new SqlRepository<Promotion>(_context), _session);
    }
    
    [TestMethod]
    public void TestAddValidDeposit()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _session.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        
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
        
        _session.LoginUser(ClientEmail,ClientPassword);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        
        Promotion promotion = new Promotion();
        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        promotionsToAddToDeposit.Add(promotion);
 
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
    }
    
    [TestMethod]
    public void TestSearchForADepositById()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _session.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit1 = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        

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
        
        _session.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        

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
        
        _session.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        

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
        
        _session.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        

        List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
        
        _depositController.AddDeposit(newDeposit, promotionsToAddToDeposit);
        _depositController.AddDeposit(newDeposit0, promotionsToAddToDeposit);
        
        _session.LogoutUser();
        
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        
        _session.LoginUser(ClientEmail,ClientPassword);
        
        _depositController.DeleteDeposit(newDeposit.Id);
         
    }
    
    [TestMethod]
    public void TestDeleteDepositRemovesDepositFromRelatedPromotions()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        _session.LoginUser(AdminEmail,AdminPassword);
        
        Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        Deposit newDeposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
        

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

        CollectionAssert.DoesNotContain(promotion2.Deposits, newDeposit0);
    }
}

/*
 private UserController _userController;
    private LogController _logController;
    private Session _session;

    [TestInitialize]
    public void Initialize()
    {
        var context = TestContextFactory.CreateContext();
        IRepository<User> _userRepository = new SqlRepository<User>(context);
        
        _userController = new UserController(_userRepository);
        _logController = new LogController(new SqlRepository<LogEntry>(context));
        
        _session = new Session(_userController, _logController);
    }
 */