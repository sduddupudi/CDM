using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities
{
    public class StudentQuiz : BaseEntity
    {
        public string StudentID { get; set; }
        public virtual QuizbeeUser Student { get; set; }

        public int QuizID { get; set; }
        public virtual Quiz Quiz { get; set; }

        public Nullable<DateTime> StartedAt { get; set; }
        public Nullable<DateTime> CompletedAt { get; set; }

        public virtual List<AttemptedQuestion> AttemptedQuestions { get; set; }

        public int Score { get; set; }
    }
}
