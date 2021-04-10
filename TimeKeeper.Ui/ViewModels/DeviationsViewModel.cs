using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TimeKeeper.Service.Dto;

namespace TimeKeeper.Ui.ViewModels
{
    public class DeviationsViewModel
    {
        public DeviationDto InputDeviation { get; set; }
        public IEnumerable<SelectListItem> SelectDaysInMonth { get; set; }
        public SelectList SelectDeviations { get; set; }

    }
}
