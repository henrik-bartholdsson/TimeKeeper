using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TimeKeeper.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? MaximumNumberOfOrganisations { get; set; }
        public virtual ICollection<Organisation> Organissations { get; set; }
        public int CurrentOrganisationId { get; set; }
    }
}
