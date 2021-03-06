using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TimeKeeper.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Organisation> Organissations { get; set; }
    }
}
