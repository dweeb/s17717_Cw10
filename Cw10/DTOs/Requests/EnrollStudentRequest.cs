using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required]
        public string Indexnumber { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Birthdate { get; set; }
        [Required]
        public string Studies { get; set; }
    }
}
