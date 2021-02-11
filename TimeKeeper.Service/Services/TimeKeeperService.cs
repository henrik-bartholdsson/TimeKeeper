using AutoMapper;
using System;
using System.Collections.Generic;
using TimeKeeper.Data.Models;
using TimeKeeper.Data.Repositories;
using TimeKeeper.Service.Dto;

namespace TimeKeeper.Service.Services
{
    public interface ITimeKeeperService
    {
        void AddDeviation(DeviationDto deviation);
        WorkMonthDto GetWorkMonthByUserId(string userId, int month, int year);
        IEnumerable<DeviationTypeDto> GetAllDeviationTypes();
    }



    public class TimeKeeperService : ITimeKeeperService
    {
        private readonly IWorkMonthRepo _wmRepo;
        private readonly IMapper _mapper;

        public TimeKeeperService(IWorkMonthRepo wmRepo, IMapper mapper)
        {
            _wmRepo = wmRepo;
            _mapper = mapper;
        }


        public void AddDeviation(DeviationDto deviation)
        {
            if (deviation == null)
                throw new Exception("Bad input, deviation is null");

            var result = _mapper.Map<Deviation>(deviation);

            _wmRepo.AddDeviationAsync(result);
        }

        public IEnumerable<DeviationTypeDto> GetAllDeviationTypes()
        {
            var deviationTypes = _wmRepo.GetAllDeviationTypesAsync().Result;

            var result = new List<DeviationTypeDto>();

            foreach(var d in deviationTypes)
            {
                var ddto = _mapper.Map<DeviationTypeDto>(d);
                result.Add(ddto);
            }

            return result;
        }

        public WorkMonthDto GetWorkMonthByUserId(string userId, int month, int year)
        {
            var workMonth = _wmRepo.GetWorkMonthByUserIdAsync(userId, month, year).Result; // Kolla om null

            if(workMonth == null)
                throw new Exception("Month not found");

            var workMonthDto = _mapper.Map<WorkMonthDto>(workMonth);

            var result = SetWeekDaysToDeviations(workMonthDto);

            return result;
        }



        public void AddNewMonths()
        {
            // Add new months for all users that have the specified month Submitted.
            // Will not add extra month for users that allready have added month on specified month.
            // Taked a list of userId


        }



        #region Private methods

        private static WorkMonthDto SetWeekDaysToDeviations(WorkMonthDto workMonth)
        {
                foreach(var d in workMonth.Deviations)
                {
                   d.NormalizedWeekDay = new DateTime(workMonth.Year, workMonth.Month, d.DayInMonth, 0, 0, 0).DayOfWeek.ToString() + $" {workMonth.Month}/{d.DayInMonth}";
                }

            return workMonth;
        }

        #endregion
    }
}
