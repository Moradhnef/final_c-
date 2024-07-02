namespace Server.EntityFramework.Entities
{
    public class TestResult : BaseObject
    {
        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Grade { get; set; }
        public ICollection<Error> Errors { get; set; }
        public TestResult()
        {
            Errors = new List<Error>();
        }

        public TestResult(int studentId, Guid testId, int grade,  ICollection<Error> errors)
        {
            TestId = testId;
            UserId = studentId;
            Grade = grade;
            Errors = errors;
        }
    }
}
