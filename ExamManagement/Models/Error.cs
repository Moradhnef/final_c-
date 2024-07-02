using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTestMaker.Models
{
    public class Error
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
