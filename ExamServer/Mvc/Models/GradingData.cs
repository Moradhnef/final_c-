using Server.EntityFramework.Entities;

namespace Server.Mvc.Models
{
    public class GradingData
    {
        public StudentTest StudentTest { get; set; }
        public int UserId { get; set; }
        public Test Test { get; set; }
    }
}
