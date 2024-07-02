namespace Server.EntityFramework.Entities
{
    public class Error : BaseObject
    {
        public Error(string questionName, int selectedAnswer, int correctAnswer)
        {
            QuestionName = questionName;
            SelectedAnswer = selectedAnswer;
            CorrectAnswer = correctAnswer;
        }

        public string QuestionName { get; set; }
        public int SelectedAnswer { get; set; }
        public int CorrectAnswer { get; set; }
    }
}