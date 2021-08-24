using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities.CustomEntities
{
    public class QuizzesSearchStudent
    {
        public List<Quiz> Quizzes { get; set; }      
        public List<StudentQuizResult> StudentQuizzesResult { get; set; }
        public int TotalCount { get; set; }
    }
}
