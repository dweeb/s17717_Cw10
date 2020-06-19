using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cw10.Models
{
    public partial class Studies
    {
        public Studies()
        {
            Enrollment = new HashSet<Enrollment>();
        }

        public int Idstudy { get; set; }
        public string Name { get; set; }

        [JsonIgnore]    // no loops allowed
        public virtual ICollection<Enrollment> Enrollment { get; set; }
    }
}
