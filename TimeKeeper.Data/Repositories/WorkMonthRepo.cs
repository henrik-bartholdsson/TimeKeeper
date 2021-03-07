using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeKeeper.Data.Models;
using TimeKeeper.Ui.Data;

namespace TimeKeeper.Data.Repositories
{
    public interface IWorkMonthRepo
    {
        void AddDeviationAsync(Deviation deviation);
        Task<WorkMonth> GetWorkMonthByUserIdAsync(string userId, int month, int year);
        Task<WorkMonth> GetLastWorkMonthByUserIdAsync(string userId);
        Task<WorkMonth> GetLastActiveWorkMonthByUserIdAsync(string userId);
        Task<IEnumerable<DeviationType>> GetAllDeviationTypesAsync();
        Task<WorkMonth> GetWorkMonthByIdAsync(int Id);
        Task<IEnumerable<Invitation>> GetInvitationsAsync(string userID);
        Task<Organisation> GetOrganisationASync(int id);
    }

    public class WorkMonthRepo : IWorkMonthRepo
    {
        private readonly DbContextOptions<TimeKeeperDbContext> _options;

        public WorkMonthRepo(TimeKeeperDbContext context, DbContextOptions<TimeKeeperDbContext> options)
        {
            _options = options;
        }


        public async Task<Organisation> GetOrganisationASync(int id) // Maybe include this function in GetInvitationsAsync(string userId) instead ?????? need to redo the db in that case.
        {
            using (var context = new TimeKeeperDbContext(_options))
            {

                Organisation organisation = await context.Organisation.Where(x => x.Id == id).FirstOrDefaultAsync();

                return organisation;
            }
        }


        public async Task<IEnumerable<Invitation>> GetInvitationsAsync(string userId)
        {
            using( var context = new TimeKeeperDbContext(_options))
            {

                List<Invitation> invitations = await context.Invitations.Where(x => x.UserId == userId).ToListAsync();

                return invitations;
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


        public async void AddDeviationAsync(Deviation deviation)
        {
            using (var _context = new TimeKeeperDbContext(_options))
            {
                var result = await _context.AddAsync(deviation);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<WorkMonth> GetWorkMonthByUserIdAsync(string userId, int month, int year)
        {
            List<WorkMonth> workMonths;

            using (var _context = new TimeKeeperDbContext(_options))
            {
                workMonths = await _context.WorkMonths.Where(x => x.UserId == userId && x.Month == month && x.Year == year).Include(y => y.Deviations).ThenInclude(z => z.DeviationType).ToListAsync();
            }

            var workMonth = workMonths.FirstOrDefault();

            return workMonth;
        }

        public async Task<WorkMonth> GetLastActiveWorkMonthByUserIdAsync(string userId)
        {
            List<WorkMonth> workMonths;

            using (var _context = new TimeKeeperDbContext(_options))
            {
                workMonths = await _context.WorkMonths.Where(x => x.UserId == userId && x.IsApproved == false).Include(z => z.Deviations).ThenInclude(a => a.DeviationType).ToListAsync();
            }

            var month = workMonths.OrderBy(y => y.Id).Last();

            return month;
        }

        public async Task<WorkMonth> GetLastWorkMonthByUserIdAsync(string userId)
        {
            List<WorkMonth> workMonths;

            using (var _context = new TimeKeeperDbContext(_options))
            {
                workMonths = await _context.WorkMonths.Where(x => x.UserId == userId).OrderBy(y => y.Id).Include(z => z.Deviations).ThenInclude(a => a.DeviationType).ToListAsync();
            }

            var month = workMonths.OrderBy(y => y.Id).Last();

            return month;
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



    }
}
