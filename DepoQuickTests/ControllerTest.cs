using System.Reflection.PortableExecutable;
using BusinessLogic;
using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.ControllerExceptions;
using DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;

namespace DepoQuickTests;

[TestClass]
public class ControllerTest
{
    private char _area = 'a';
    private String _size = "Mediano";
    private bool _airConditioning = true;
    private bool _reserved = false;

    private char _area2 = 'B';
    private String _size2 = "Grande";
    private bool _airConditioning2 = true;
    private bool _reserved2 = false;

    private string _name = "Juan Perez";
    private string _email = "nombre@dominio.es";
    private string _password = "Contrasena#1";

    private Deposit _deposit = new Deposit('A', "Pequeño", true, false);
    private DateRange _stay = new DateRange(new DateTime(2024, 04, 07), new DateTime(2024, 04, 08));


    [TestMethod]
    public void TestAddValidDeposit()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Deposit newDeposit = new Deposit(_area, _size, _airConditioning, _reserved);

        controller.AddDeposit(newDeposit);

        CollectionAssert.Contains(controller.GetDeposits(), newDeposit);

    }

    [TestMethod]
    public void TestSearchForADepositById()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Deposit newDeposit0 = new Deposit(_area, _size, _airConditioning, _reserved);
        Deposit newDeposit1 = new Deposit(_area2, _size2, _airConditioning2, _reserved2);

        int id = newDeposit1.GetId();

        controller.AddDeposit(newDeposit0);
        controller.AddDeposit(newDeposit1);

        Assert.AreEqual(char.ToUpper(_area2), controller.GetDeposit(id).GetArea());
        Assert.AreEqual(_size2.ToUpper(), controller.GetDeposit(id).GetSize());
        Assert.AreEqual(_airConditioning2, controller.GetDeposit(id).GetAirConditioning());
        Assert.AreEqual(_reserved2, controller.GetDeposit(id).IsReserved());
        Assert.AreEqual(id, controller.GetDeposit(id).GetId());

    }

    [TestMethod]
    [ExpectedException(typeof(DepositNotFoundException))]
    public void TestSearchForADepositUsingAnInvalidId()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Deposit newDeposit0 = new Deposit(_area, _size, _airConditioning, _reserved);
        Deposit newDeposit1 = new Deposit(_area2, _size2, _airConditioning2, _reserved2);

        controller.AddDeposit(newDeposit0);
        controller.AddDeposit(newDeposit1);

        controller.GetDeposit(-34);

    }

    [TestMethod]
    public void TestAddAReservationToController()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Client client = new Client(_name, _email, _password);

        Reservation reservation = new Reservation(_deposit, client, _stay);

        controller.AddReservation(reservation);

        CollectionAssert.Contains(memoryDataBase.GetReservations(), reservation);
    }

    [TestMethod]
    public void TestSearchForAReservationById()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Client client = new Client(_name, _email, _password);

        Reservation reservation1 = new Reservation(_deposit, client, _stay);
        Reservation reservation2 = new Reservation(_deposit, client, _stay);

        int id = reservation1.GetId();

        controller.AddReservation(reservation1);
        controller.AddReservation(reservation2);

        Assert.AreEqual(_deposit, controller.GetReservation(id).GetDeposit());
        Assert.AreEqual(client, controller.GetReservation(id).GetClient());
        Assert.AreEqual(_stay, controller.GetReservation(id).GetDateRange());

        Assert.AreEqual(id, controller.GetReservation(id).GetId());

    }

    [TestMethod]
    [ExpectedException(typeof(ReservationNotFoundException))]
    public void TestSearchForAReservationUsingAnInvalidId()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Client client = new Client(_name, _email, _password);

        Reservation reservation1 = new Reservation(_deposit, client, _stay);
        Reservation reservation2 = new Reservation(_deposit, client, _stay);

        controller.AddReservation(reservation1);
        controller.AddReservation(reservation2);

        controller.GetReservation(-34);

    }

    [TestMethod]
    public void TestAddNewPromotion()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Promotion promotion = new Promotion();
        
        List<Deposit> depositsToAddPromotion = new List<Deposit>();
        
        depositsToAddPromotion.Add(_deposit);

        promotion.SetDiscountRate(0.5);
        promotion.SetLabel("Promotion 1");
        promotion.SetValidityDate(_stay);
        

        controller.AddPromotion(promotion, depositsToAddPromotion);

        CollectionAssert.Contains(controller.GetPromotions(), promotion);
        CollectionAssert.Contains(controller.GetPromotions()[0].GetDeposits(), _deposit);
        CollectionAssert.Contains(_deposit.GetPromotions(), promotion);
    }
    
    
    

    [TestMethod]
    public void TestSearchForAPromotionById()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Promotion promotion1 = new Promotion();
        Promotion promotion2 = new Promotion();
        
        List<Deposit> depositsToAddPromotion = new List<Deposit>();
        depositsToAddPromotion.Add(_deposit);

        int id = promotion1.GetId();

        controller.AddPromotion(promotion1, depositsToAddPromotion);
        controller.AddPromotion(promotion2, depositsToAddPromotion);

        Assert.AreEqual(promotion1, controller.GetPromotion(id));

    }

    [TestMethod]
    [ExpectedException(typeof(PromotionNotFoundException))]
    public void TestSearchForAPromotionUsingAnInvalidId()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Promotion promotion1 = new Promotion();
        Promotion promotion2 = new Promotion();
        
        List<Deposit> depositsToAddPromotion = new List<Deposit>();
        depositsToAddPromotion.Add(_deposit);

        controller.AddPromotion(promotion1, depositsToAddPromotion);
        controller.AddPromotion(promotion2, depositsToAddPromotion);

        controller.GetPromotion(-4);
    }

    [TestMethod]
    [ExpectedException(typeof(PromotionNotFoundException))]
    public void TestDeletePromotion()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Promotion promotion = new Promotion();

        List<Deposit> depositsToAddPromotion = new List<Deposit>();
        depositsToAddPromotion.Add(_deposit);
        
        controller.AddPromotion(promotion, depositsToAddPromotion);

        int id = promotion.GetId();

        controller.DeletePromotion(id);

        controller.GetPromotion(id);
    }

    [TestMethod]
    public void TestLoginValidAdministrator()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        String nombre = "Maria";
        String email = "maria@gmail.com";
        String password = "mAria1..123";
        String validation = "mAria1..123";
        controller.RegisterAdministrator(nombre, email, password, validation);
        controller.LoginUser(email, password);
        Assert.AreEqual(controller.GetAdministrator(), controller.GetActiveUser());
    }
    
    [TestMethod]
    [ExpectedException(typeof(AdministratorAlreadyExistsException))]
    public void TestAdministratorAlreadyExistsException()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        String nombre = "Maria";
        String email = "maria@gmail.com";
        String email2 = "maria@gmail.com";
        String password = "mAria1..123";
        String validation = "mAria1..123";
        controller.RegisterAdministrator(nombre, email, password, validation);
        controller.RegisterAdministrator(nombre,email2,password,validation);
        controller.LoginUser(email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestInvalidLogin()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        String nombre = "Maria";
        String email = "maria@gmail.com";
        String password = "mAria1..123";
        String validation = "mAria1..123";
        String email2 = "mariaR@gmail.com";
        controller.RegisterAdministrator(nombre, email, password, validation);
        controller.LoginUser(email2, password);
    }

    [TestMethod]
    [ExpectedException(typeof(CannotCreateClientBeforeAdminException))]
    public void TestCannotCreateClientBeforeAdmin()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        String nombre = "Maria";
        String email = "maria@gmail.com";
        String password = "mAria1..123";
        String validation = "mAria1..123";
        controller.RegisterClient(nombre, email, password, validation);
    }

    [TestMethod]
    [ExpectedException(typeof(UserAlreadyExistsException))]
    public void TestInvalidRegisterClient()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);
        String name = "Marieta";
        String email = "maria@gmail.com";
        String password = "mAria1..123";
        String validation = "mAria1..123";
        controller.RegisterAdministrator(name, email, password, validation);
        String name2 = "Maria";
        controller.RegisterClient(name2, email, password, validation);
    }
    
    [TestMethod]
    [ExpectedException(typeof(EmptyUserListException))]
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

        String name = "Maria";
        String email = "maria@gmail.com";
        String password = "mAria1..123";
        String validation = "mAria1..123";
        String emailAdmin = "mario@gmail.com";
        controller.RegisterAdministrator(name, emailAdmin, password, validation);
        controller.RegisterClient(name, email,password, validation);
        controller.LoginUser(email,password);
        CollectionAssert.Contains(memoryDataBase.GetListOfUsers(), controller.GetActiveUser());
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositNotFoundException))]
    public void TestDeleteDeposit()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();

        Controller controller = new Controller(memoryDataBase);

        Deposit deposit = new Deposit('A', "Pequeño", true, false);

        controller.AddDeposit(deposit);

        int id = deposit.GetId();

        controller.DeleteDeposit(id);

        controller.GetDeposit(id);
    }
    
    
    /*
     [TestMethod]
    public void TestGetActiveUser()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase();
        Controller controller = new Controller(memoryDataBase);

        String nombre = "Maria";
        String email = "maria@gmail.com";
        String password = "mAria1..123";
        String validation = "mAria1..123";
        controller.RegisterAdministrator(nombre, email, password, validation);
        controller.LoginUser(email, password);
        Assert.AreEqual(controller.GetAdministrator(), controller.GetActiveUser());
    }
     */
}