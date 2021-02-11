using AutoMapper;
using TimeKeeper.Data.Models;
using TimeKeeper.Service.Dto;

namespace TimeKeeper.Ui
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<WorkMonth, WorkMonthDto>();
            CreateMap<Deviation, DeviationDto>();
            CreateMap<DeviationDto, Deviation>();
            CreateMap<DeviationType, DeviationTypeDto>();
        }
    }
}
