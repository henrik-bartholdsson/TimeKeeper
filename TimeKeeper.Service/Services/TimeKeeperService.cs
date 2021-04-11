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
        void AddOrganisation(string organisationName, ApplicationUser user);
        IEnumerable<OrganisationDto> GetOrganisations(string userId);
        OrganisationDto GetOrganisation(int id);
        void UpdateOrganisation(OrganisationDto orgDto);
        int GetNumberOfOrganisations(string userId);
        void RejectInvitation(int id, string userId);
    }



    public class TimeKeeperService : ITimeKeeperService
    {
        //private readonly IOrganisationRepo _organisationRepo;
        //private readonly IWorkMonthRepo _wmRepo;
        private readonly IMapper _mapper;

        private readonly ITimeKeeperRepo _timeKeeperRepo;

        public TimeKeeperService(ITimeKeeperRepo timeKeeperRepo, IMapper mapper)
        {
            //_wmRepo = wmRepo;
            //_organisationRepo = organisationRepo;
            _mapper = mapper;

            _timeKeeperRepo = timeKeeperRepo;
        }


        public void AddOrganisation(string organisationName, ApplicationUser user)
        {
            if (String.IsNullOrEmpty(organisationName))
                throw new Exception("Organisation name cannot be null or empty.");

            if (user == null)
                throw new UnauthorizedAccessException();

            var maxNumberOfOrganisations = _timeKeeperRepo.GetNumberOfTopOrganisationsAsync(user.Id).Result;

            if (user.MaximumNumberOfOrganisations <= maxNumberOfOrganisations)
                throw new Exception("To many organisations.");

            var organisation = new Organisation();

            organisation.ManagerId = user.Id;
            organisation.OrganisationOwner = user.Id;
            organisation.Name = organisationName;

            _timeKeeperRepo.AddOrganisationAsync(organisation);
        }


        public IEnumerable<Invitation> GetInvitations(string userId)
        {
            var invitations = _timeKeeperRepo.GetInvitationsAsync(userId).Result;

            foreach (var i in invitations)
            {
                var inviterName = _timeKeeperRepo.GetOrganisationASync(i.OrganisationId).Result;
                i.OrganisationName = inviterName.Name;
            }

            return invitations;
        }


        public void AddDeviation(DeviationDto inputDeviation) // What if WorkMonth.Id is manipulated?
        {
            if (inputDeviation == null)
                throw new Exception("Bad input, deviation is null");

            var targetWorkMonth = _timeKeeperRepo.GetWorkMonthByIdAsync(inputDeviation.WorkMonthId).Result;

            // Is user member of organisation?

            if (targetWorkMonth == null)
            {
                targetWorkMonth = GetNotYetCreatedWorkmonth(inputDeviation.RequestedDate);
                targetWorkMonth.UserId = inputDeviation.userId;
                targetWorkMonth = _timeKeeperRepo.AddWorkMonth(targetWorkMonth).Result;
                inputDeviation.WorkMonthId = targetWorkMonth.Id;
            }
                

            if (targetWorkMonth.IsApproved)
                throw new Exception("Cannot add deviation to allready approved month.");

            if (targetWorkMonth.IsSubmitted)
                throw new Exception("The month is submitted, unsubmit and try again.");

            if (targetWorkMonth.UserId != inputDeviation.userId)
                throw new UnauthorizedAccessException();

            var result = _mapper.Map<Deviation>(inputDeviation);

            _timeKeeperRepo.AddDeviationAsync(result);
        }


        public IEnumerable<DeviationTypeDto> GetAllDeviationTypes()
        {
            var deviationTypes = _timeKeeperRepo.GetAllDeviationTypesAsync().Result;

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
            if (requestedDate > DateTime.Now)
                requestedDate = DateTime.Now;

            var workMonth = _timeKeeperRepo.GetWorkMonthByUserIdAsync(userId, requestedDate.Month, requestedDate.Year).Result;

            if (workMonth == null)
                workMonth = GetNotYetCreatedWorkmonth(requestedDate);

            var workMonthDto = _mapper.Map<WorkMonthDto>(workMonth);

            var result = SetWeekDaysToDeviations(workMonthDto);

            return result;
        }


        public WorkMonthDto GetLastWorkMonthByUserId(string userId)
        {
            WorkMonth workMonth;
            workMonth = _timeKeeperRepo.GetLastActiveWorkMonthByUserIdAsync(userId).Result;

            if (workMonth == null)
                workMonth = GetNotYetCreatedWorkmonth(DateTime.Now);

            var workMonthDto = _mapper.Map<WorkMonthDto>(workMonth);

            var result = SetWeekDaysToDeviations(workMonthDto);

            return result;
        }


        public IEnumerable<OrganisationDto> GetOrganisations(string userId)
        {
            var organisations = _timeKeeperRepo.GetTopOrganisationsAsync(userId).Result;

            var orgDto = MapOrganisationsToDto(organisations);

            return orgDto;
        }


        public OrganisationDto GetOrganisation(int id)
        {
            var organisation = _timeKeeperRepo.GetOrganisationAsync(id).Result;
            var organisationDto = _mapper.Map<OrganisationDto>(organisation);

            return organisationDto;
        }


        public void AddNewMonths()
        {
            // Add new months for all users that have the specified month Submitted.
            // Will not add extra month for users that allready have added month on specified month.
            // Taked a list of userId


        }


        public void UpdateOrganisation(OrganisationDto inputOrganisationDto)
        {
            if (inputOrganisationDto == null)
                throw new Exception("No organisation specified.");

            if (inputOrganisationDto.Id < 1)
                throw new Exception("No organisation specified.");

            var updatedOrganisation = GetOrganisationWithUpdatedFields(inputOrganisationDto);

            _timeKeeperRepo.UpdateOrganisationAsync(updatedOrganisation);
        }


        public int GetNumberOfOrganisations(string userId)
        {
            var result = _timeKeeperRepo.GetNumberOfTopOrganisationsAsync(userId).Result;
            return result;
        }


        public void RejectInvitation(int id, string userId)
        {
            _timeKeeperRepo.RejectInvitation(id, userId);
        }


        #region Private methods

        private Organisation GetOrganisationWithUpdatedFields(OrganisationDto inputDto)
        {
            var storedDto = _timeKeeperRepo.GetOrganisationAsync(inputDto.Id).Result;

            storedDto.Name = inputDto.Name ?? storedDto.Name;
            storedDto.ManagerId = inputDto.ManagerId ?? storedDto.ManagerId;

            storedDto.FK_Parent_OrganisationId = inputDto.ParentOrganisationId;

            if (inputDto.ParentOrganisationId == 0 || inputDto.ParentOrganisationId == null)
                storedDto.FK_Parent_OrganisationId = null;

            return storedDto;
        }

        private ICollection<OrganisationDto> MapOrganisationsToDto(IEnumerable<Organisation> organisations)
        {
            var organisationDto = new List<OrganisationDto>();

            foreach (var o in organisations)
            {
                var orgDto = _mapper.Map<OrganisationDto>(o);

                if (o.Section != null)
                    orgDto.Section = MapOrganisationsToDto(o.Section);

                organisationDto.Add(orgDto);

            }
            return organisationDto;
        }

        private bool ValidateInputDeviationInput(DeviationDto inputDeviation)
        {
            var requestedMonth = _timeKeeperRepo.GetWorkMonthByIdAsync(inputDeviation.WorkMonthId).Result;

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

        private WorkMonth GetNotYetCreatedWorkmonth(DateTime requestedDate)
        {
            return new WorkMonth
            {
                Deviations = null,
                IsApproved = false,
                IsSubmitted = false,
                Month = requestedDate.Month,
                Year = requestedDate.Year,
                Organisation = null,
                UserId = null
            };
        }



        #endregion
    }
}
