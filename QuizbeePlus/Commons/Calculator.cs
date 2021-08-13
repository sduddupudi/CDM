using QuizbeePlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizbeePlus.Commons
{
    public static class Calculator
    {
        public static decimal CalculateAttemptedQuestionScore(List<Option> questionOptions, List<AttemptedOption> attemptedOptions)
        {
            decimal correctQuestionScore = 1;

            //now check if the attemptedOptions have any incorrect option?
            //if any incorrect option is selected, return score as 0
            if (attemptedOptions.Count == 0 || attemptedOptions.Any(x => !x.Option.IsCorrect)) return 0;
            
            //first get the correct options out of QuestionOptions
            var correctOptions = questionOptions.Where(x => x.IsCorrect).ToList();

            //if attemptedOptions are equal to correctOption then return full score
            if(correctOptions.Count == attemptedOptions.Count)
            {
                return correctQuestionScore;
            }
            else
            {
                //divide score points per correct option
                decimal perOptionScore = correctQuestionScore / correctOptions.Count;

                //return score by multiplying attemptedoptions with per option score.
                return attemptedOptions.Count * perOptionScore;
            }
        }
        
        public static decimal CalculateStudentQuizScore(List<AttemptedQuestion> attemptedQuestions)
        {
            //return sum of scores of each attempted question
            return attemptedQuestions.Sum(x=>x.Score);
        }

        /// <summary>
        /// return time in seconds allowed for eac question of a quiz.
        /// </summary>
        public static double CalculateAllowedQuestionTime(Quiz quiz)
        {
            double seconds = 0;

            if(quiz != null && quiz.Questions != null && quiz.Questions.Count > 0)
            {
                seconds = quiz.TimeDuration.TotalSeconds / quiz.Questions.Count;
            }

            return seconds;
        }
    }
}