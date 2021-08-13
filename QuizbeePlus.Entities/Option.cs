using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities
{
    public class Option : BaseEntity
    {
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }

        public int? ImageID { get; set; }
        public virtual Image Image { get; set; }
    }
}
