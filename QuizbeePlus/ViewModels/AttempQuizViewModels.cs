using QuizbeePlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizbeePlus.ViewModels
{
    public class QuizDetailViewModel : BaseViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public QuizType SelectedQuizType { get; set; }
        public TimeSpan TimeDuration { get; set; }
        public int NoOfQuestions { get; set; }

        public bool EnableQuizTimer { get; set; }
        public bool EnableQuestionTimer { get; set; }
    }

    public class AttemptQuizViewModel
    {
        public Question Question { get; set; }
        public List<Option> Options { get; set; }

        public QuizType QuizType { get; set; }

        public int StudentQuizID { get; set; }
        public int QuizID { get; set; }
        public int QuestionID { get; set; }
        public string SelectedOptionIDs { get; set; }

        public bool TimerExpired { get; set; }

        public int TotalQuestions { get; set; }
        public int QuestionIndex { get; set; }

        public bool EnableQuestionTimer { get; set; }
        public double Seconds { get; set; }
    }
}