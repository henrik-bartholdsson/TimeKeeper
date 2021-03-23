using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TimeKeeper.Data.Models;

namespace TimeKeeper.Service.Dto
{
    public class OrganisationDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<OrganisationDto> Section { get; set; }
        public ICollection<ApplicationUser> OrganisationUsers { get; set; } // Byt till dto
        public string ManagerId { get; set; }
        public int? ParentOrganisationId { get; set; }
        public string OrganisationOwner { get; set; }

        public bool IsOwner { get; set; } = false;
    }
}
