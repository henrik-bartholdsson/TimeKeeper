using System.ComponentModel.DataAnnotations;

namespace TimeKeeper.Service.Dto
{
    public class DeviationDto
    {
        public int DayInMonth { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public string StartTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public string StopTime { get; set; }
        public string Comment { get; set; }
        public bool IsPredefined { get; set; }
        public DeviationTypeDto DeviationType { get; set; }
        [Required]
        public int DeviationTypeId { get; set; }
        public string NormalizedWeekDay { get; set; }
        public int WorkMonthId { get; set; }
    }
}
