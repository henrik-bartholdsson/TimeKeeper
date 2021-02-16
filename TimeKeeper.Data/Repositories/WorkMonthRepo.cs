using Microsoft.EntityFrameworkCore;
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
        Task<IEnumerable<DeviationType>> GetAllDeviationTypesAsync();
        Task<WorkMonth> GetWorkMonthByIdAsync(int Id);
    }

    public class WorkMonthRepo : IWorkMonthRepo
    {
        private readonly DbContextOptions<TimeKeeperDbContext> _options;

        public WorkMonthRepo(TimeKeeperDbContext context, DbContextOptions<TimeKeeperDbContext> options)
        {
            _options = options;
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
            WorkMonth workMonth;

            using (var _context = new TimeKeeperDbContext(_options))
            {
                workMonth = await _context.WorkMonths.Where(x => x.UserId == userId && x.Month == month && x.Year == year).Include(y => y.Deviations).ThenInclude(z => z.DeviationType).FirstOrDefaultAsync();
            }

            return workMonth;
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
