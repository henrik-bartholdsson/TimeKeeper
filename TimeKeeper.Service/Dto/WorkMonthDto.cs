using System;
using System.Collections.Generic;
using System.Text;

namespace TimeKeeper.Service.Dto
{
    public class WorkMonthDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsApproved { get; set; }
        public string UserId { get; set; }
        public int OrganisationId { get; set; }
        public List<DeviationDto> Deviations { get; set; }
        public string NormalizedMonthName { get => new DateTime(Year, Month, 1, 0, 0, 0).ToString("MMMM"); }
    }
}
