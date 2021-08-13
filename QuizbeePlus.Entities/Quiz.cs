using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities
{
    public class Quiz : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan TimeDuration { get; set; }
        public virtual List<Question> Questions { get; set; }

        public bool EnableQuizTimer { get; set; }
        public bool EnableQuestionTimer { get; set; }

        public string OwnerID { get; set; }

        public QuizType QuizType { get; set; }

        public bool IsActive { get; set; }
    }
}
