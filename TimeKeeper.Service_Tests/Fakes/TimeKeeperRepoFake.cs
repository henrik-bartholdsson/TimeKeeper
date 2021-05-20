using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.Data.Models;
using TimeKeeper.Data.Repositories;

namespace TimeKeeper.Service_Tests.Fakes
{
    class TimeKeeperRepoFake : ITimeKeeperRepo
    {
        public Task<Deviation> AddDeviationAsync(Deviation deviation)
        {
            throw new NotImplementedException();
        }

        public Task<Organisation> AddOrganisationAsync(Organisation organisation)
        {
            throw new NotImplementedException();
        }

        public void AddSectionAsync(Organisation section, int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkMonth> AddWorkMonth(WorkMonth workMonth)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Invitation>> GetActiveInvitationsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeviationType>> GetAllDeviationTypesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetApplicationUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Invitation> GetInvitationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<WorkMonth> GetLastActiveWorkMonthAsync(string userId, int organisationId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkMonth> GetLastWorkMonthByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNumberOfTopOrganisationsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Organisation> GetOrganisationAsync(int id)
        {
            var org = new Organisation { Id = id };

            await Task.Run(() => {  });

            return org;
        }

        public Task<IEnumerable<Organisation>> GetOrganisationsWhereUserIsMemberAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Organisation> GetOrganisationWithUsersAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Organisation>> GetTopOrganisationsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkMonth> GetWorkMonthAsync(string userId, int organisationId, int month, int year)
        {
            throw new NotImplementedException();
        }

        public Task<WorkMonth> GetWorkMonthByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> UpdateApplicationUserAsync(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }

        public Task<Invitation> UpdateInvitationAsync(Invitation invitation)
        {
            throw new NotImplementedException();
        }

        public Task<Organisation> UpdateOrganisationAsync(Organisation organisation)
        {
            throw new NotImplementedException();
        }
    }
}
