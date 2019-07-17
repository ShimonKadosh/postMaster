using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PostMasterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly UserModel _UserModel;
        private ApiContext _ApiContext;
        private readonly ResponseBuilder _ResponseContext;

        public PostsController(UserModel UserModel, ApiContext ApiContext, ResponseBuilder ResponseContext)
        {
            _UserModel = UserModel;
            _ApiContext = ApiContext;
            _ResponseContext = ResponseContext;
        }

        [HttpPost("create")]
        public ActionResult Create([FromForm] Post post)
        {
            post.UserId = _UserModel.LoggedIn().Id;
            //post.Category = _ApiContext.Categories.Single<Category>(c => c.Id == post.CategoryId);

            /*if (!post.IsValidated())
            {
                _ResponseContext.AddError("אחד או יותר מהשדות ריקים, או שאינך מחובר.");
            }*/

            if (_ResponseContext.status)
            {
                _ApiContext.Posts.Add(post);
                _ApiContext.SaveChanges();
            }

            return Ok(_ResponseContext.Build());
        }

        [HttpGet("remove")]
        public ActionResult Remove(int id)
        {
            var UserId = _UserModel.LoggedIn() != null ? _UserModel.LoggedIn().Id : 0;
            var Result = _ApiContext.Posts.AsEnumerable().SingleOrDefault(p => p.Id == id && p.UserId == UserId);

            if (Result == null)
            {
                _ResponseContext.AddError("הפוסט אינו קיים, או שאינך בעל הפוסט.");
            }

            if (_ResponseContext.status)
            {
                _ApiContext.Remove(Result);
                _ApiContext.SaveChanges();
            }

            return Ok(_ResponseContext.Build());
        }

        [HttpPost("update")]
        public ActionResult Update([FromForm] Post post)
        {
            var UserId = _UserModel.LoggedIn() != null ? _UserModel.LoggedIn().Id : 0;
            var Result =  _ApiContext.Posts.AsEnumerable().Single(p => p.Id == post.Id && p.UserId == UserId);
            
            if (Result == null)
            {
                _ResponseContext.AddError("הפוסט אינו קיים, או שאינך בעל הפוסט.");
            }

            if (_ResponseContext.status)
            {
                Result.Content = post.Content;
                
                _ApiContext.Update(Result);
                _ApiContext.SaveChanges();
            }

            return Ok(_ResponseContext.Build());
        }

        [HttpGet("createCategory")]
        public ActionResult CreateCategory(string name)
        {
            var category = new Category();
            category.Name = name;

            _ApiContext.Categories.Add(category);
            _ApiContext.SaveChanges();

            return Ok(name);
        }
    }
}