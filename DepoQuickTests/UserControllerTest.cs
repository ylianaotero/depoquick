namespace DepoQuickTests;

[TestClass]
public class UserControllerTest
{
    private UserController _userController;

    [TestInitialize]
    public void Initialize()
    {
        var context = TestContextFactory.CreateContext();
        _userController = new UserController(context);
    }
}
