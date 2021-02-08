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
        Task<WorkMonth> GetWorkMonthByUserIdAsync(string userId, int month, int year);

    }

    public class WorkMonthRepo : IWorkMonthRepo
    {
        private readonly TimeKeeperDbContext _context;

        public WorkMonthRepo(TimeKeeperDbContext context)
        {
            _context = context;
        }


        public async Task<WorkMonth> GetWorkMonthByUserIdAsync(string userId, int month, int year)
        {
            var workMonth = await _context.WorkMonths.Where(x => x.UserId == userId && x.Month == month && x.Year == year).Include(y => y.Deviations).ThenInclude(z => z.DeviationType).FirstOrDefaultAsync();

            return workMonth;
        }


        
    }
}
