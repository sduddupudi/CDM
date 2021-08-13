using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities.CustomEntities
{
    public class QuizzesSearch
    {
        public List<Quiz> Quizzes { get; set; }
        public int TotalCount { get; set; }
    }
}
