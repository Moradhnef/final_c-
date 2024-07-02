namespace Server.Mvc.Models
{
    public class StudentTest
    {
        public string StudentName { get; set; }
        public List<int> SelectedAnswers { get; set; }
        public StudentTest()
        {
            SelectedAnswers = new List<int>();
        }


    }
}
