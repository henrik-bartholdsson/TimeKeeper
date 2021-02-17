using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TimeKeeper.Data.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public int OrganisationId { get; set; }

        [NotMapped]
        public string OrganisationName { get; set; }
    }
}
