using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities
{
    public class Image
    {
        [Column(Order = 1)]
        public int ID { get; private set; }
        public string Name { get; set; }
    }
}
