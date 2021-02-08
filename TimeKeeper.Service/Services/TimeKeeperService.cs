using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TimeKeeper.Data.Repositories;
using TimeKeeper.Service.Dto;
using TimeKeeper.Service.Helpers;

namespace TimeKeeper.Service.Services
{
    public interface ITimeKeeperService
    {
        WorkMonthDto GetWorkMonthByUserId(string userId, int month, int year);

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

        public WorkMonthDto GetWorkMonthByUserId(string userId, int month, int year)
        {
            var workMonth = _wmRepo.GetWorkMonthByUserIdAsync(userId, month, year).Result;

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
