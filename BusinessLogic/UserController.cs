using SQLitePCL;

namespace BusinessLogic;

public class UserController
{
    private DepoQuickContext _context;
    public UserController(DepoQuickContext context)
    {
        _context = context;
    }
}