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
        Task<WorkMonth> GetWorkMonthById(int Id);
    }

    public class WorkMonthRepo : IWorkMonthRepo
    {
        private readonly TimeKeeperDbContext _context;
        private readonly DbContextOptions<TimeKeeperDbContext> _options;

        public WorkMonthRepo(TimeKeeperDbContext context, DbContextOptions<TimeKeeperDbContext> options)
        {
            _context = context;
            _options = options;
        }

        public async Task<WorkMonth> GetWorkMonthById(int Id)
        {
            var month = await _context.WorkMonths.FirstOrDefaultAsync(x => x.Id == Id);

            return month;
        }


        public async void AddDeviationAsync(Deviation deviation)
        {
            using (var _context2 = new TimeKeeperDbContext(_options))
            {
                var result = await _context2.AddAsync(deviation);

            await _context2.SaveChangesAsync();
            }
        }

        public async Task<WorkMonth> GetWorkMonthByUserIdAsync(string userId, int month, int year)
        {
            var workMonth = await _context.WorkMonths.Where(x => x.UserId == userId && x.Month == month && x.Year == year).Include(y => y.Deviations).ThenInclude(z => z.DeviationType).FirstOrDefaultAsync();

            return workMonth;
        }

        public async Task<IEnumerable<DeviationType>> GetAllDeviationTypesAsync()
        {
            var result = await _context.DeviationTypes.Where(x => x != null).ToListAsync();

            return result;
        }



    }
}
