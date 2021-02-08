using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeKeeper.Data.Models
{
    public class WorkMonth
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsApproved { get; set; }
        public string UserId { get; set; }
        public List<Deviation> Deviations { get; set; }
    }
}
