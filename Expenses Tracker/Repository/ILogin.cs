using Expenses_Tracker.Models;

namespace Expenses_Tracker.Repository
{
    public interface ILogin
    {
        Task<IEnumerable<LoginViewModel>> getuser();
        Task<LoginViewModel> AuthenticateUser(string username, string passcode);
    }
}
