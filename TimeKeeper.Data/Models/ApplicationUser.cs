
using Microsoft.AspNetCore.Identity;

namespace TimeKeeper.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int OrganisationId { get; set; }
    }
}
