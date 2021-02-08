using System;
using System.Collections.Generic;
using System.Text;

namespace TimeKeeper.Service.Dto
{
    public class DeviationDto
    {
        public int DayInMonth { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public string Comment { get; set; }
        public bool IsPredefined { get; set; }
        public DeviationTypeDto DeviationType { get; set; }
        public string NormalizedWeekDay { get; set; }
    }
}
