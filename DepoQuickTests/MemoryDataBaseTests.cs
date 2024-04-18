using BusinessLogic;
using DepoQuick.Domain;

namespace DepoQuickTests;


[TestClass]
public class MemoryDataBaseTests
{
    [TestMethod]
    public void TestLoginUser()
    {
        MemoryDataBase memoryDataBase = new MemoryDataBase(); 

        User user = new User("Juan Perez", "juanperez@gmail.com", "Contrasena#1");

        memoryDataBase.LoginUser(user);
        
        Assert.AreEqual(memoryDataBase.GetUser(), user);
    }
}