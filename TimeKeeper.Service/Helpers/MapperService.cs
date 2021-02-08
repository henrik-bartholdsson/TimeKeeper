using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TimeKeeper.Data.Models;
using TimeKeeper.Service.Dto;

namespace TimeKeeper.Service.Helpers
{
    public interface IMapperService
    {
        WorkMonthDto MappToWorkMOnthDto(WorkMonth WorkMonth);
    }


    //public class MapperService : IMapperService
    //{
    //    private readonly IConfigurationProvider mapperConfig;


        //public MapperService()
        //{
        //    mapperConfig = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<WorkMonth, WorkMonthDto>();
        //        cfg.CreateMap<Deviation, DeviationDto>();
        //        cfg.CreateMap<DeviationType, DeviationTypeDto>();
        //    });
        //}



        //public WorkMonthDto MappToWorkMOnthDto(WorkMonth WorkMonth)
        //{
        //    IMapper iMapper = mapperConfig.CreateMapper();

        //    var destination = iMapper.Map<WorkMonth, WorkMonthDto>(WorkMonth);

        //    return destination;
        //}
    //}
}
