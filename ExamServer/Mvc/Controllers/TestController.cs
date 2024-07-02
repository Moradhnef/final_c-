using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.EntityFramework;
using Server.EntityFramework.Entities;

namespace Server.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly TestDbContext _dbContext;
        public TestController(TestDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetTests()
        {
          
           var test = _dbContext.Tests.Include(s => s.Questions).ThenInclude(s=> s.Answers).ToList();
       
          
           return test;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Test>> GetTest(string name)
        {
            var test = _dbContext.Tests.Include(s => s.Questions).ThenInclude(s => s.Answers).FirstOrDefault(s => s.Name.ToLower() == name.ToLower());

            if (test == null)
            {
                return NotFound();
            }
           
            return test;
        }
     

        [HttpPost]
        [ProducesResponseType(201)]
        
        public async Task<ActionResult<List<Test>>> CreateTests([FromBody]List<Test> tests)
        {
            var dbTests = _dbContext.Tests.ToList();
            foreach (var test in tests)
            {
                if (dbTests.FirstOrDefault(s=> s.Id == test.Id) != null)
                {
                    continue;
                }
                else
                {
                    await _dbContext.AddAsync(test);
                }
            }
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // Updates edited exam
        // And its questions and answers
        [HttpPut]
        public async Task<IActionResult> UpdateTest(Test test)
        {
            var fromDb = await _dbContext.Tests.Where(s => s.Id == test.Id).Include(s=> s.Questions).ThenInclude(s=> s.Answers).FirstOrDefaultAsync();
            if (fromDb != null)
            {

                fromDb.Questions.Clear();
                foreach(var question in test.Questions)
                {
                    fromDb.Questions.Add(question);
                    fromDb.LecturerName = test.LecturerName;
                    fromDb.Name = test.Name;
                    fromDb.StartDateTime = test.StartDateTime;
                    fromDb.TotalTime = test.TotalTime;
                    fromDb.RandomSorting = test.RandomSorting;
                  
                }
                
                _dbContext.Update(fromDb);
              
                await _dbContext.SaveChangesAsync();
            }
            return Ok();
        }

    }

}
