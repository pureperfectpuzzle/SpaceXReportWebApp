using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Objects.SpaceX
{
    public class Capsule
    {
        public string? Id { get; set; }

        public int? Landings { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public int? ReuseCount { get; set; }
    }
}
