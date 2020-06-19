using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cw10.Models
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            Student = new HashSet<Student>();
        }

        public int Idenrollment { get; set; }
        public int Semester { get; set; }
        public int Idstudy { get; set; }
        public DateTime Startdate { get; set; }

        [JsonIgnore]    // no loops allowed
        public virtual Studies IdstudyNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Student> Student { get; set; }
    }
}
