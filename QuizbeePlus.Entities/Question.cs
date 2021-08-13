using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities
{
    public class Question : BaseEntity
    {
        public string Title { get; set; }
        public virtual List<Option> Options { get; set; }

        public int QuizID { get; set; }
        public virtual Quiz Quiz { get; set; }

        public int? ImageID { get; set; }
        public virtual Image Image { get; set; }
    }
}
