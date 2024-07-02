using Server.EntityFramework;
using Server.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly TestDbContext _context;

        public AuthenticationController(TestDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns user if is loged in otherwise unauthorized
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Login(User user)
        {

            var dbUser = _context.Users.Where(s=>  user.Username == s.Username && s.Password == user.Password).FirstOrDefault();
            if(dbUser == null)
            {
                return Unauthorized();
            }
            return Ok(dbUser);
        }


        [HttpPut]
        public ActionResult Create(User user)
        {

            var dbUser = _context.Users.Where(s=> s.Username == user.Username).FirstOrDefault();
            if (dbUser == null)
            {
               _context.Users.Add(user);
               _context.SaveChanges();
               return Ok();
            }
           return BadRequest();
        }

    }
}
