using Server.EntityFramework;
using Server.EntityFramework.Entities;
using Server.Mvc.Models;
using Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly TestDbContext _context;

        private readonly GradingService gradingService;


        public ResultController(TestDbContext context)
        {
            _context = context;
            gradingService = new GradingService();
            
        }


        //Get exam results and errors
        [HttpGet]
        public ActionResult Get()
        {
            var result = _context.TestResults.Include(s => s.Errors);
            return Ok(result);
        }

        //Gets exam result by ID
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var result = _context.TestResults.Where(s => s.User.Id == id).Include(s => s.Errors).Include(s=> s.Test);
            return Ok(result);
        }

        /// <summary>
        /// Recieves exam result and grades student and stores it to db
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddResult(GradingData data)
        {

            var result = gradingService.Grade(data);

            _context.TestResults.Add(result);
            var i = _context.SaveChanges();
            return i > 0 ? Ok() : BadRequest();
        }
    }
}
