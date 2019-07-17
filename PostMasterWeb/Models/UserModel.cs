using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PostMasterWeb
{
    public class UserModel
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly ApiContext _ApiContext;

        private const string UserIdSession = "_UserId";

        public UserModel(IHttpContextAccessor HttpContextAccessor, ApiContext ApiContext)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _ApiContext = ApiContext;
        }
        
        public bool Exists(string email, string password = null)
        {
            if (string.IsNullOrEmpty(password))
            {
                return _ApiContext.Users.Where(u => u.Email == email).FirstOrDefault<User>() != null ? true : false;
            }

            return _ApiContext.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault<User>() != null ? true : false;
        }

        public User Login(string email, string password)
        {
            User CurrentUser = null;

            if (Exists(email, password))
            {
                CurrentUser = _ApiContext.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault<User>();
                _HttpContextAccessor.HttpContext.Session.SetInt32(UserIdSession, CurrentUser.Id);
            }

            return CurrentUser;
        }

        public User LoggedIn()
        {
            int UserId = _HttpContextAccessor.HttpContext.Session.GetInt32(UserIdSession).GetValueOrDefault();
            return _ApiContext.Users.Where(u => u.Id == UserId).Include(u => u.Posts).FirstOrDefault<User>();
        }
    }
}
