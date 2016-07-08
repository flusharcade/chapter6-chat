
namespace ConnectionMappingSample.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    using Chat.Models;
    using Chat;
    using Chat.Repositories;

    public class AccountController : ApiController
    {
        private AuthenticationRepository authenticationRepository;

        public AccountController()
        {
            authenticationRepository = new AuthenticationRepository();
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authenticationRepository.RegisterUser(userModel);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Login")]
        public async Task<bool> Login(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            var result = await authenticationRepository.FindUser(userModel.Username, userModel.Password);
            return (result != null);
        }

        [HttpGet]
        [AllowAnonymous]
        [ActionName("GetAllConnectedUsers")]
        public IEnumerable<string> GetAllConnectedUsers()
        {
            return ChatHub.Users.Select(x => x.Key);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                authenticationRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}