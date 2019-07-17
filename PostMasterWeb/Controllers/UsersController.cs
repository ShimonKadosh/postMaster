using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PostMasterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserModel _UserModel;
        private ApiContext _ApiContext;
        private readonly ResponseBuilder _ResponseContext;

        public UsersController(UserModel  UserModel, ApiContext ApiContext, ResponseBuilder ResponseContext)
        {
            _UserModel = UserModel;
            _ApiContext = ApiContext;
            _ResponseContext = ResponseContext;
        }

        [HttpPost("signup")]
        public ActionResult Signup([FromForm] User user)
        {
            if (!user.IsValidated())
            {
                _ResponseContext.AddError("אחד או יותר מהשדות ריקים.");
            }
            
            if (_UserModel.Exists(user.Email))
            {
                _ResponseContext.AddError("האימייל כבר תפוס, אנא נסה להרשם עם אימייל אחר.");
            }

            if (_ResponseContext.status)
            {
                _ApiContext.Users.Add(user);
                _ApiContext.SaveChanges();
            }

            return Ok(_ResponseContext.Build());
        }

        [HttpPost("login")]
        public ActionResult Login([FromForm] User user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                _ResponseContext.AddError("אחד או יותר מהשדות ריקים.");
            }
            else if (!_UserModel.Exists(user.Email, user.Password))
            {
                _ResponseContext.AddError("איימיל או סיסמא אינם נכונים.");
            }

            if (_ResponseContext.status)
            {
                _UserModel.Login(user.Email, user.Password);
            }

            return Ok(_ResponseContext.Build());
        }
    }
}