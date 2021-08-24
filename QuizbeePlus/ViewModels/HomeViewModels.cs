using QuizbeePlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizbeePlus.ViewModels
{
    public class HomeViewModel : ListingBaseViewModel
    {
        public List<Quiz> Quizzes { get; set; }
        public List<StudentQuizResult> studentQuizResults { get; set; }
    }
}