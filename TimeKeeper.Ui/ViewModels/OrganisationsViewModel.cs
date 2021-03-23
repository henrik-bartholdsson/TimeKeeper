using System.Collections.Generic;
using TimeKeeper.Service.Dto;

namespace TimeKeeper.Ui.ViewModels
{
    public class OrganisationsViewModel
    {
        public IEnumerable<OrganisationDto> Organisations { get; set; }
        public bool IsOwner { get; set; } = false;
    }
}
