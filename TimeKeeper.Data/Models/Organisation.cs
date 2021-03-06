
using System.Collections.Generic;

namespace TimeKeeper.Data.Models
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Organisation> Section { get; set; }
        public ICollection<ApplicationUser> OrganisationUsers { get; set; }
        public string ManagerId { get; set; }
    }
}
