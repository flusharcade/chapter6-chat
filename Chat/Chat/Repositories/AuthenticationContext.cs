
namespace Chat.Repositories
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class AuthenticationContext : IdentityDbContext<IdentityUser>
    {
        public AuthenticationContext()
            : base("AuthenticationContext")
        {
        }
    }
}