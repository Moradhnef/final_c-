namespace Server.EntityFramework.Entities
{
    public class Question : BaseObject
    {
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public bool IsImageQuestion { get; set; }
        public bool RandomSorting { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
