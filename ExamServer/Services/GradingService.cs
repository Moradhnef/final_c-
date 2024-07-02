using Server.EntityFramework.Entities;
using Server.Mvc.Models;

namespace Server.Services
{
    public class GradingService
    {
        // This method takes a GradingData object as input, which contains information about the student's answers and the exam.
        // It calculates the number of correct answers and generates a list of errors if the student answered any questions incorrectly.
        // Finally, it returns an ExamResult object containing the student's grade and any errors that were encountered.

        // Parameter: data - A GradingData object containing the student's answers and the exam.
        // Return value: An ExamResult object representing the student's grade and any errors.

        public TestResult Grade(GradingData data)
        {
            // Get a list of all the questions in the exam
            var questions = data.Test.Questions.ToList();
            // Get the total number of questions in the exam
            int totalQuestions = data.Test.Questions.Count;

            // Initialize variables to keep track of the number of correct answers and any errors
            int correctAnswers = 0;
            List<Error> errors = new List<Error>();

            // Loop through all the questions in the exam
            for (int i = 0; i < totalQuestions; i++)
            {
                // Get the current question and the student's selected answer
                Question question = questions[i];
                int selectedAnswerIndex = data.StudentTest.SelectedAnswers[i];

                // Get the index of the correct answer for the current question
                int correctAnswerIndex = question.CorrectAnswerIndex;

                // If the student's answer matches the correct answer, increment the counter for correct answers
                // Otherwise, add an error to the list of errors
                if (selectedAnswerIndex == correctAnswerIndex)
                {
                    correctAnswers++;
                }
                else
                {
                    errors.Add(new Error(question.Text, selectedAnswerIndex, correctAnswerIndex));
                }
            }

            // Calculate the student's grade as a percentage
            int grade = (int)Math.Round((double)correctAnswers / totalQuestions * 100);

            // Generate and return an ExamResult object containing the student's grade and any errors
            return GetExamResult(data.UserId, data.Test.Id, grade, errors);
        }

        public TestResult GetExamResult(int studentId, Guid id, int grade, List<Error> errors)
        {
            return new TestResult(studentId, id, grade, errors);
        }
    
    }
}
