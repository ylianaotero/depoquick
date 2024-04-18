using BusinessLogic;
using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.ControllerExceptions;

namespace DepoQuickTests;

[TestClass]
public class ControllerTest
{
    private char area = 'a';
    private String size = "Mediano";
    private bool airConditioning = true;
    private bool reserved = false;
    
    private char area2 = 'B';
    private String size2 = "Grande";
    private bool airConditioning2 = true;
    private bool reserved2 = false;
    
    private string _name = "Juan Perez";
    private string _email = "nombre@dominio.es";
    private string _password = "Contrasena#1";
    
    private Deposit deposit = new Deposit('A', "Pequeño", true, false);
    private DateRange stay = new DateRange(new DateTime(2024, 04, 07), new DateTime(2024, 04, 08));

    
    [TestMethod]
    public void TestAddValidDeposit()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase(); 
        
        Controller controller = new Controller(memoryDataBase);
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        controller.AddDeposit(newDeposit); 
        
        CollectionAssert.Contains(memoryDataBase.GetListOfDeposits(), newDeposit);
        
    }
    
    [TestMethod]
    public void TestSearchForADepositById()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase(); 
        
        Controller controller = new Controller(memoryDataBase);

        Deposit newDeposit0 = new Deposit(area, size, airConditioning, reserved);
        Deposit newDeposit1 = new Deposit(area2, size2, airConditioning2, reserved2);

        int id = newDeposit1.GetId(); 
        
        controller.AddDeposit(newDeposit0);
        controller.AddDeposit(newDeposit1);

        Assert.AreEqual(char.ToUpper(area2), controller.GetDeposit(id).GetArea());
        Assert.AreEqual(size2.ToUpper(), controller.GetDeposit(id).GetSize());
        Assert.AreEqual(airConditioning2, controller.GetDeposit(id).GetAirConditioning());
        Assert.AreEqual(reserved2, controller.GetDeposit(id).IsReserved());
        Assert.AreEqual(id, controller.GetDeposit(id).GetId());
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(ExceptionDepositNotFound))] 
    public void TestSearchForADepositUsingAnInvalidId()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase(); 
        
        Controller controller = new Controller(memoryDataBase);

        Deposit newDeposit0 = new Deposit(area, size, airConditioning, reserved);
        Deposit newDeposit1 = new Deposit(area2, size2, airConditioning2, reserved2);
        
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
        
        Reservation reservation = new Reservation(deposit, client, stay);

        controller.AddReservation(reservation); 
        
        CollectionAssert.Contains(memoryDataBase.GetReservations(), reservation);
    }
    
    [TestMethod]
    public void TestSearchForAReservationById()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase(); 
        
        Controller controller = new Controller(memoryDataBase);

        Client client = new Client(_name, _email, _password);
        
        Reservation reservation1 = new Reservation(deposit, client, stay);
        Reservation reservation2 = new Reservation(deposit, client, stay);

        int id = reservation1.GetId(); 
        
        controller.AddReservation(reservation1);
        controller.AddReservation(reservation2);

        Assert.AreEqual(deposit, controller.GetReservation(id).GetDeposit());
        Assert.AreEqual(client, controller.GetReservation(id).GetClient());
        Assert.AreEqual(stay, controller.GetReservation(id).GetDateRange());
        
        Assert.AreEqual(id, controller.GetReservation(id).GetId());
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(ExceptionReservationNotFound))] 
    public void TestSearchForAReservationUsingAnInvalidId()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase(); 
        
        Controller controller = new Controller(memoryDataBase);

        Client client = new Client(_name, _email, _password);
        
        Reservation reservation1 = new Reservation(deposit, client, stay);
        Reservation reservation2 = new Reservation(deposit, client, stay);
        
        controller.AddReservation(reservation1);
        controller.AddReservation(reservation2);

        controller.GetReservation(-34); 

    }


}