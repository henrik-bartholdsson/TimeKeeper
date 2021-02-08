using System.ComponentModel.DataAnnotations;

namespace TimeKeeper.Data.Models
{
    public class Deviation
    {
        [Key]
        public int Id { get; set; }
        public int DayInMonth { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public string Comment { get; set; }
        public bool IsPredefined { get; set; }
        public DeviationType DeviationType { get; set; }
    }
}
