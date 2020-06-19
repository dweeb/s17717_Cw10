using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10.DTOs.Requests
{
    public class ModifyStudentRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Birthdate { get; set; }
    }
}
