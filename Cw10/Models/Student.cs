using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cw10.Models
{
    public partial class Student
    {
        public string Indexnumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public int Idenrollment { get; set; }
        [JsonIgnore]    // no sharing secrets
        public string Password { get; set; }
        [JsonIgnore]
        public string Role { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }

        [JsonIgnore]    // no loops allowed
        public virtual Enrollment IdenrollmentNavigation { get; set; }
    }
}
