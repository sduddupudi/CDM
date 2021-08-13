using QuizbeePlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizbeePlus.ViewModels
{
    public class StudentQuizViewModel : BaseViewModel
    {
        public StudentQuiz StudentQuiz { get; set; }

        public bool TimerExpired { get; set; }

        public int Hours
        {
            get
            {
                if (StudentQuiz.StartedAt.HasValue && StudentQuiz.CompletedAt.HasValue)
                    return (StudentQuiz.CompletedAt.Value - StudentQuiz.StartedAt.Value).Hours;
                else return 0;
            }
        }
        public int Minutes
        {
            get
            {
                if (StudentQuiz.StartedAt.HasValue && StudentQuiz.CompletedAt.HasValue)
                    return (StudentQuiz.CompletedAt.Value - StudentQuiz.StartedAt.Value).Minutes;
                else return 0;
            }
        }
        public int Seconds
        {
            get
            {
                if (StudentQuiz.StartedAt.HasValue && StudentQuiz.CompletedAt.HasValue)
                    return (StudentQuiz.CompletedAt.Value - StudentQuiz.StartedAt.Value).Seconds;
                else return 0;
            }
        }

        public TimeSpan TimeDuration
        {
            get
            {
                return new TimeSpan(Hours, Minutes, Seconds);
            }
        }
    }

    public class StudentQuizListViewModel : ListingBaseViewModel
    {
        public bool isPartialView { get; set; }

        public List<StudentQuiz> StudentQuizzes { get; set; }
    }
}