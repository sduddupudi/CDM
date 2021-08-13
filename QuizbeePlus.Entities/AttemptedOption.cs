using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities
{
    public class AttemptedOption : BaseEntity
    {
        public int AttemptedQuestionID { get; set; }
        public virtual AttemptedQuestion AttemptedQuestion { get; set; }

        public int OptionID { get; set; }
        public virtual Option Option { get; set; }
    }
}
