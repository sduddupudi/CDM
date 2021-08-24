using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities
{
    public class StudentQuizResult : BaseEntity
    {
        public int QuizID { get; set; }
        public Nullable<DateTime> StartedAt { get; set; }
        public Nullable<DateTime> CompletedAt { get; set; }

        public int Score { get; set; }

        public static string GetDateString(Nullable<DateTime> dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToShortDateString():"";
        }

    }
}
