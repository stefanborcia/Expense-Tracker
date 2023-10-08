using Expenses_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses_Tracker.Repository
{
    public class AuthenticateLogin : ILogin
    {
        private readonly ApplicationDbContext _context;

        public AuthenticateLogin(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<LoginViewModel> AuthenticateUser(string username, string passcode)
        {
            var succeeded = await _context.LoginViewModels.FirstOrDefaultAsync(authUser => authUser.UserName == username && authUser.passcode == passcode);
            return succeeded;
        }

        public async Task<IEnumerable<LoginViewModel>> getuser()
        {
            return await _context.LoginViewModels.ToListAsync();
        }
    }
}
