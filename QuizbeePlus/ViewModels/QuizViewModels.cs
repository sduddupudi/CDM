using QuizbeePlus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuizbeePlus.ViewModels
{
    public class QuizListViewModel : ListingBaseViewModel
    {
        public List<Quiz> Quizzes { get; set; }
    }

    public class NewQuizViewModel : BaseViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        public List<QuizType> QuizTypes { get; set; }
        public QuizType SelectedQuizType { get; set; }

        [Display(Name="Time")]
        [Range(typeof(TimeSpan), "00:01", "23:59")]
        public TimeSpan TimeDuration {
            get
            {
                if(!EnableQuizTimer) return new TimeSpan(0, 1, 0);
                else return new TimeSpan(Hours, Minutes, 0);
            }
        }

        [Required]
        [Range(0, 23, ErrorMessage = "Hours should be between 0 to 23")]
        public int Hours { get; set; }

        [Required]
        [Range(0, 59, ErrorMessage = "Minutes should be between 0 to 59")]
        public int Minutes { get; set; }
        
        public bool EnableQuizTimer { get; set; }
        public bool EnableQuestionTimer { get; set; }
    }

    public class EditQuizViewModel : BaseViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        public List<QuizType> QuizTypes { get; set; }
        public QuizType SelectedQuizType { get; set; }

        [Display(Name = "Time")]
        [Range(typeof(TimeSpan), "00:01", "23:59")]
        public TimeSpan TimeDuration
        {
            get
            {
                if (!EnableQuizTimer) return new TimeSpan(0, 1, 0);
                else return new TimeSpan(Hours, Minutes, 0);
            }
        }
        
        [Required]
        [Range(0, 23, ErrorMessage = "Hours should be between 0 to 23")]
        public int Hours { get; set; }

        [Required]
        [Range(0, 59, ErrorMessage = "Minutes should be between 0 to 59")]
        public int Minutes { get; set; }

        public int NoOfQuestions { get; set; }

        public bool EnableQuizTimer { get; set; }
        public bool EnableQuestionTimer { get; set; }
    }
}