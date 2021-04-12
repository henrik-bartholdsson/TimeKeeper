using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeeper.Data.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public int OrganisationId { get; set; }
        public bool requireAction { get; set; }

        [NotMapped]
        public string OrganisationName { get; set; }
    }
}
