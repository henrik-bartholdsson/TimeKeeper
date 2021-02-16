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


        public void AddDeviation(DeviationDto inputDeviation)
        {
            if (inputDeviation == null)
                throw new Exception("Bad input, deviation is null");


            var result = _mapper.Map<Deviation>(inputDeviation);

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
            var workMonth = _wmRepo.GetWorkMonthByUserIdAsync(userId, month, year).Result;

            if (workMonth == null)
                return null;

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




        private bool ValidateInputDeviationInput(DeviationDto inputDeviation)
        {
            var requestedMonth = _wmRepo.GetWorkMonthByIdAsync(inputDeviation.WorkMonthId).Result;

            if (requestedMonth.IsApproved)
                throw new Exception("Cannot add deviations to allready approved months.");

            if (requestedMonth.IsSubmitted)
                throw new Exception("Cannot add deviations to allready submitted months. Recall the month and try again.");

            if (inputDeviation.userId != requestedMonth.UserId)
                throw new Exception("Unauthorized.");



            return true;
        }


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
