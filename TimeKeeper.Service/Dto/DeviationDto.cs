using System;
using System.ComponentModel.DataAnnotations;
using TimeKeeper.Ui.Validators;

namespace TimeKeeper.Service.Dto
{
    public class DeviationDto
    {
        [Required]
        public int DayInMonth { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public string StartTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [DeviationValidator("StartTime")]
        public string StopTime { get; set; }
        [MaxLength(100)]
        public string Comment { get; set; }
        public bool IsPredefined { get; set; }
        public DeviationTypeDto DeviationType { get; set; }
        [Required]
        public int DeviationTypeId { get; set; }
        public int WorkMonthId { get; set; }

        // Exstended none model attributes
        public string NormalizedWeekDay { get; set; }
        public string userId { get; set; }
        public DateTime RequestedDate { get; set; }
    }
}
