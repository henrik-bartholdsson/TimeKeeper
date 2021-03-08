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
        WorkMonthDto GetWorkMonthByUserId(string userId, DateTime requestedDate);
        WorkMonthDto GetLastWorkMonthByUserId(string userId);
        IEnumerable<DeviationTypeDto> GetAllDeviationTypes();
        IEnumerable<Invitation> GetInvitations(string userId);
        void AddOrganisation(string organisationName, string organisationManager);
    }



    public class TimeKeeperService : ITimeKeeperService
    {
        private readonly IOrganisationRepo _organisationRepo;
        private readonly IWorkMonthRepo _wmRepo;
        private readonly IMapper _mapper;

        public TimeKeeperService(IWorkMonthRepo wmRepo, IOrganisationRepo organisationRepo, IMapper mapper)
        {
            _wmRepo = wmRepo;
            _organisationRepo = organisationRepo;
            _mapper = mapper;
        }


        public void AddOrganisation(string organisationName, string organisationManager)
        {
            if (String.IsNullOrEmpty(organisationName))
                throw new Exception("organisationName cannot be null or empty.");


        }

        public IEnumerable<Invitation> GetInvitations(string userId)
        {
            var invitations = _wmRepo.GetInvitationsAsync(userId).Result;

            foreach (var i in invitations)
            {
                var inviterName = _wmRepo.GetOrganisationASync(i.OrganisationId).Result;
                i.OrganisationName = inviterName.Name;
            }

            return invitations;
        }


        public void AddDeviation(DeviationDto inputDeviation)
        {
            if (inputDeviation == null)
                throw new Exception("Bad input, deviation is null");

            var preCheckTargetWorkMonth = _wmRepo.GetWorkMonthByIdAsync(inputDeviation.WorkMonthId).Result;

            if (preCheckTargetWorkMonth.IsApproved)
                throw new Exception("Cannot add deviation to allready approved month.");

            if (preCheckTargetWorkMonth.IsSubmitted)
                throw new Exception("The month is submitted, unsubmit and try again.");

            if (preCheckTargetWorkMonth.UserId != inputDeviation.userId)
                throw new UnauthorizedAccessException();

            var result = _mapper.Map<Deviation>(inputDeviation);

            _wmRepo.AddDeviationAsync(result);
        }


        public IEnumerable<DeviationTypeDto> GetAllDeviationTypes()
        {
            var deviationTypes = _wmRepo.GetAllDeviationTypesAsync().Result;

            var result = new List<DeviationTypeDto>();

            foreach (var d in deviationTypes)
            {
                var ddto = _mapper.Map<DeviationTypeDto>(d);
                result.Add(ddto);
            }

            return result;
        }



        public WorkMonthDto GetWorkMonthByUserId(string userId, DateTime requestedDate)
        {
            var anyWorkMonthForUser = _wmRepo.GetLastWorkMonthByUserIdAsync(userId).Result;

            if (anyWorkMonthForUser == null)
                throw new Exception("No month found");


            var workMonth = _wmRepo.GetWorkMonthByUserIdAsync(userId, requestedDate.Month, requestedDate.Year).Result;

            if (workMonth == null)
                return null;

            var workMonthDto = _mapper.Map<WorkMonthDto>(workMonth);

            var result = SetWeekDaysToDeviations(workMonthDto);

            return result;
        }

        public WorkMonthDto GetLastWorkMonthByUserId(string userId)
        {
            WorkMonth workMonth;
            workMonth = _wmRepo.GetLastActiveWorkMonthByUserIdAsync(userId).Result;

            if (workMonth == null)
                workMonth = _wmRepo.GetLastWorkMonthByUserIdAsync(userId).Result;

            if (workMonth == null)
                throw new Exception("No month found.");

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
            foreach (var d in workMonth.Deviations)
            {
                d.NormalizedWeekDay = new DateTime(workMonth.Year, workMonth.Month, d.DayInMonth, 0, 0, 0).DayOfWeek.ToString() + $" {workMonth.Month}/{d.DayInMonth}";
            }

            return workMonth;
        }

        #endregion
    }
}
