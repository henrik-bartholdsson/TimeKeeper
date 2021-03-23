
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeeper.Data.Models
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("FK_Parent_OrganisationId")]
        public ICollection<Organisation> Section { get; set; }
        public ICollection<ApplicationUser> OrganisationUsers { get; set; }
        public string ManagerId { get; set; }
        public int? FK_Parent_OrganisationId { get; set; }
        public string OrganisationOwner { get; set; }
    }
}
