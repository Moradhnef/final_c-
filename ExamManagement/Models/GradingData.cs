namespace EasyTestMaker.Models
{
    public class GradingData
    {
        public GradingData(StudentTest studentTest, int userId, Test test)
        {
            StudentTest = studentTest;
            UserId = userId;
            Test = test;
        }

        public StudentTest StudentTest 
            { get; set; }
        public int UserId { get; set; }
        public Test Test { get; set; }
    }
}
