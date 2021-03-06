﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public ICollection<Deviation> Deviations { get; set; }
        public Organisation Organisation { get; set; }
    }
}
