using BusinessLogic;
using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;

namespace DepoQuickTests;


[TestClass]
public class MemoryDataBaseTests
{
    /*
    [TestMethod]
    public void TestLoginUser()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase(); 

        User user = new User("Juan Perez", "juanperez@gmail.com", "Contrasena#1");

        memoryDataBase.RegisterAdministrator(user);
        
        Assert.AreEqual(memoryDataBase.GetActiveUser(), user);
    }
    */
    
    [TestMethod]
    public void TestGetAndSetAdministrator()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase(); 

        Administrator userAdministrator = new Administrator("Juan Perez", "juanperez@gmail.com", "Contrasena#1");

        memoryDataBase.SetAdministrator(userAdministrator);
        
        Assert.AreEqual(memoryDataBase.GetAdministrator(), userAdministrator);
    }
    
    
    
    
}