using QuizbeePlus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuizbeePlus.ViewModels
{
    public class QuestionListViewModel : ListingBaseViewModel
    {
        public int QuizID { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class NewQuestionViewModel : BaseViewModel
    {
        [Required]
        public string Title { get; set; }

        public List<Option> Options { get; set; }
        public List<Option> CorrectOptions { get; set; }

        public int QuizID { get; set; }
        public QuizType QuizType { get; set; }
    }

    public class EditQuestionViewModel : BaseViewModel
    {
        public int ID { get; set; }
        
        [Required]
        public string Title { get; set; }

        public List<Option> Options { get; set; }
        public List<Option> CorrectOptions { get; set; }

        public int QuizID { get; set; }
        public QuizType QuizType { get; set; }
    }
}