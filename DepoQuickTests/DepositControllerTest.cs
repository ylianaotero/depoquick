using BusinessLogic;

namespace DepoQuickTests;


[TestClass]
public class DepositControllerTest
{
    private DepositController _depositController;
    
    [TestInitialize]
    public void Initialize()
    {
        var context = TestContextFactory.CreateContext();
        _depositController = new DepositController(context);
    }
    
}