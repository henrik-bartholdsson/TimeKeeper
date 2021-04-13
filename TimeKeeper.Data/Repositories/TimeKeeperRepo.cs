using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeKeeper.Data.Models;
using TimeKeeper.Ui.Data;

namespace TimeKeeper.Data.Repositories
{
    public interface ITimeKeeperRepo
    {
        Task<IEnumerable<Organisation>> GetTopOrganisationsAsync(string userId);
        Task<IEnumerable<Organisation>> GetOrganisationsWhereUserIsMemberAsync(string userId);
        Task<Organisation> AddOrganisationAsync(Organisation organisation);
        void AddSectionAsync(Organisation section, int parentId);
        Task<Organisation> GetOrganisationAsync(int id);
        Task<Organisation> GetOrganisationWithUsersAsync(int id);
        Task<Organisation> UpdateOrganisationAsync(Organisation organisation);
        Task<int> GetNumberOfTopOrganisationsAsync(string userId);
        void RejectInvitation(int id, string userId);
        Task<Deviation> AddDeviationAsync(Deviation deviation);
        Task<WorkMonth> GetWorkMonthAsync(string userId, int organisationId, int month, int year);
        Task<WorkMonth> GetLastWorkMonthByUserIdAsync(string userId);
        Task<WorkMonth> GetLastActiveWorkMonthAsync(string userId, int organisationId);
        Task<IEnumerable<DeviationType>> GetAllDeviationTypesAsync();
        Task<WorkMonth> GetWorkMonthByIdAsync(int Id);
        Task<IEnumerable<Invitation>> GetInvitationsAsync(string userId);
        Task<Invitation> GetInvitationAsync(int id);
        Task<Invitation> UpdateInvitationAsync(Invitation invitation);
        Task<WorkMonth> AddWorkMonth(WorkMonth workMonth);
        Task<ApplicationUser> GetApplicationUserAsync(string id);
        Task<ApplicationUser> UpdateApplicationUserAsync(ApplicationUser applicationUser);
    }



    public class TimeKeeperRepo : ITimeKeeperRepo
    {
        private readonly DbContextOptions<TimeKeeperDbContext> _options;
        public TimeKeeperRepo(TimeKeeperDbContext context, DbContextOptions<TimeKeeperDbContext> options)
        {
            _options = options;
        }



        public async Task<Organisation> AddOrganisationAsync(Organisation organisation)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var result = context.Organisation.Add(organisation);
                await context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async void AddSectionAsync(Organisation section, int parentId)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var parent = context.Organisation.Where(o => o.Id == parentId).FirstOrDefault();

                if (parent == null)
                    throw new Exception("Organisation do not exist.");

                parent.Section = new List<Organisation>();
                parent.Section.Add(section);
                await context.SaveChangesAsync();
            }

            throw new Exception("Error while adding section.");
        }

        public async Task<Organisation> GetOrganisationAsync(int id)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var organisation = await context.Organisation.Where(x => x.Id == id).FirstOrDefaultAsync();
                return organisation;
            }
        }

        public async Task<Organisation> GetOrganisationWithUsersAsync(int id)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var organisation = await context.Organisation.Where(x => x.Id == id).Include("OrganisationUsers").FirstOrDefaultAsync();
                return organisation;
            }
        }

        public async Task<IEnumerable<Organisation>> GetTopOrganisationsAsync(string userId)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var organisations = await context.Organisation.Where(x => x.ManagerId == userId).Include("Section").ToListAsync();
                return organisations.Where(x => x.FK_Parent_OrganisationId == null);
            }
        }

        public async Task<IEnumerable<Organisation>> GetOrganisationsWhereUserIsMemberAsync(string userId)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var organisations = await context.Organisation.Where(x => x.OrganisationUsers.Contains(new ApplicationUser{ Id = userId })).ToListAsync();

                return organisations;
            }
        }

        public async Task<Organisation> UpdateOrganisationAsync(Organisation organisation)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var result = context.Update(organisation);
                await context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<int> GetNumberOfTopOrganisationsAsync(string userId)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var organisations = await context.Organisation.Where(x => x.OrganisationOwner == userId && x.FK_Parent_OrganisationId == null).ToListAsync();
                var nrOfOrganisations = organisations.Count();
                return nrOfOrganisations;
            }
        }

        public void RejectInvitation(int id, string userId) // Make async?
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var invitation = context.Invitations.Where(x => x.Id == id && x.UserId == userId).FirstOrDefault();

                if (invitation == null)
                    throw new Exception("Is null!");

                //context.Invitations.Remove(invitation);

                context.Remove(invitation);

                context.SaveChanges();
            }
        }

        public async Task<WorkMonth> AddWorkMonth(WorkMonth workMonth)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {

                var result = context.Add(workMonth);
                await context.SaveChangesAsync();

                return result.Entity;
            }
        }

        public async Task<IEnumerable<Invitation>> GetInvitationsAsync(string userId)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {

                List<Invitation> invitations = await context.Invitations.Where(x => x.UserId == userId).ToListAsync();

                return invitations;
            }
        }

        public async Task<Invitation> GetInvitationAsync(int id)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {

                Invitation invitation = await context.Invitations.Where(x => x.Id == id).FirstOrDefaultAsync();

                return invitation;
            }
        }

        public async Task<Invitation> UpdateInvitationAsync(Invitation invitation)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {

                context.Invitations.Update(invitation);
                await context.SaveChangesAsync();

                return invitation;
            }
        }

        public async Task<WorkMonth> GetWorkMonthByIdAsync(int Id)
        {
            WorkMonth month;
            using (var _context = new TimeKeeperDbContext(_options))
            {
                month = await _context.WorkMonths.FirstOrDefaultAsync(x => x.Id == Id);
            }

            return month;
        }

        public async Task<Deviation> AddDeviationAsync(Deviation deviation)
        {
            using (var _context = new TimeKeeperDbContext(_options))
            {
                var result = _context.Add(deviation);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
        }

        public async Task<WorkMonth> GetWorkMonthAsync(string userId, int organisationId, int month, int year)
        {
            List<WorkMonth> workMonths;

            using (var _context = new TimeKeeperDbContext(_options))
            {
                workMonths = await _context.WorkMonths.Where(
                    x => x.UserId == userId
                    && x.Month == month
                    && x.Year == year
                    && x.Organisation.Id == organisationId)
                    .Include(x => x.Organisation)
                    .Include(x => x.Deviations)
                    .ThenInclude(x => x.DeviationType)
                    .ToListAsync();
            }

            if (workMonths.Count < 1)
                return null;

            var result = workMonths.FirstOrDefault();

            return result;
        }

        public async Task<WorkMonth> GetLastActiveWorkMonthAsync(string userId, int organisationId)
        {
            List<WorkMonth> workMonths;

            using (var _context = new TimeKeeperDbContext(_options))
            {
                workMonths = await _context.WorkMonths.Where(x => x.Organisation.Id == organisationId && x.UserId == userId && x.IsApproved == false).Include(x => x.Organisation).Include(x => x.Deviations).ThenInclude(x => x.DeviationType).ToListAsync();
            }

            if (workMonths.Count < 1)
                return null;

            WorkMonth result = workMonths.OrderBy(x => x.Id).Last();

            return result;
        }

        public async Task<WorkMonth> GetLastWorkMonthByUserIdAsync(string userId)
        {
            List<WorkMonth> workMonths;

            using (var _context = new TimeKeeperDbContext(_options))
            {
                workMonths = await _context.WorkMonths.Where(x => x.UserId == userId).OrderBy(x => x.Id).Include(x => x.Deviations).ThenInclude(x => x.DeviationType).ToListAsync();
            }

            if (workMonths.Count < 1)
                return null;

            var result = workMonths.OrderBy(y => y.Id).Last();

            return result;
        }

        public async Task<IEnumerable<DeviationType>> GetAllDeviationTypesAsync()
        {
            List<DeviationType> result;
            using (var _context = new TimeKeeperDbContext(_options))
            {
                result = await _context.DeviationTypes.Where(x => x != null).ToListAsync();
            }

            return result;
        }

        public async Task<ApplicationUser> GetApplicationUserAsync(string id)
        {
            using (var _context = new TimeKeeperDbContext(_options))
            {
                var user = await _context.AspNetUsers.Where(x => x.Id == id).Include("Organissations").FirstOrDefaultAsync();
                return user;
            }
        }

        public async Task<ApplicationUser> UpdateApplicationUserAsync(ApplicationUser applicationUser)
        {
            using (var _context = new TimeKeeperDbContext(_options))
            {
                _context.AspNetUsers.Update(applicationUser);
                await _context.SaveChangesAsync();
                return applicationUser;
            }
        }
    }
}
