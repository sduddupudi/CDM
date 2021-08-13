using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities
{
    public class AttemptedQuestion : BaseEntity
    {
        public int? StudentQuizID { get; set; }
        public virtual StudentQuiz StudentQuiz { get; set; }

        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }

        public virtual List<AttemptedOption> SelectedOptions { get; set; }

        public Nullable<DateTime> AnsweredAt { get; set; }

        public bool IsCorrect { get; set; }

        public decimal Score { get; set; }
    }
}
