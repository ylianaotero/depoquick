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
    
    [TestMethod]
    public void TestAddValidDeposit()
    {
        Controller controller = new Controller();
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        controller.AddDeposit(newDeposit); 
        
        CollectionAssert.Contains(controller.GetListOfDeposits(), newDeposit);
        
    }
    
    [TestMethod]
    public void TestSearchForADepositById()
    {
        Controller controller = new Controller();
        
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
        Controller controller = new Controller();
        
        Deposit newDeposit0 = new Deposit(area, size, airConditioning, reserved);
        Deposit newDeposit1 = new Deposit(area2, size2, airConditioning2, reserved2);
        
        controller.AddDeposit(newDeposit0);
        controller.AddDeposit(newDeposit1);

        controller.GetDeposit(-34); 

    }

    [TestMethod]
    public void TestLoginUser()
    {
        Controller controller = new Controller();

        User user = new User("Juan Perez", "juanperez@gmail.com", "Contrasena#1");

        controller.LoginUser(user);
        
        Assert.AreEqual(controller.GetUser(), user);
    }


}