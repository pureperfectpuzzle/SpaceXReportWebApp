using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Objects.SpaceX
{
    public class Launch
    {
        public string? Id { get; set; }

        public string? Details { get; set; }

        public string? Launch_Year { get; set; }

        public string? Launch_Date_Local { get; set; }

        public string[]? Mission_Id { get; set; }

        public string? Mission_Name { get; set; }

        public bool? Upcoming { get; set; }
    }
}
