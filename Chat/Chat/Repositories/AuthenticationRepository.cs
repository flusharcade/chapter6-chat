
namespace Chat.Repositories
{
    using System;
    using System.Threading.Tasks;
    
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Chat.Models;

    public class AuthenticationRepository : IDisposable
    {
        private AuthenticationContext authenticationContext;
        private UserManager<IdentityUser> userManager;

        public AuthenticationRepository()
        {
            authenticationContext = new AuthenticationContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authenticationContext));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser newUser = new IdentityUser()
            {
                UserName = userModel.Username
            };

            var foundUser = await userManager.FindByNameAsync(newUser.UserName);
            if (foundUser != null)
            {
                await userManager.RemovePasswordAsync(foundUser.Id);
                return await userManager.AddPasswordAsync(foundUser.Id, userModel.Password);
            }
            else
            {
                return await userManager.CreateAsync(newUser, userModel.Password);
            }
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            return await userManager.FindAsync(userName, password);
        }

        public void Dispose()
        {
            authenticationContext.Dispose();
            userManager.Dispose();
        }
    }
}