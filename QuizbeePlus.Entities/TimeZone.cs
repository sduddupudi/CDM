using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Entities.IPGeolocation
{
    public class TimeZonecs
    {
        public string name { get; set; }
        public double offset { get; set; }
        public string current_time { get; set; }
        public double current_time_unix { get; set; }
        public bool is_dst { get; set; }
        public int dst_savings { get; set; }
    }
}
