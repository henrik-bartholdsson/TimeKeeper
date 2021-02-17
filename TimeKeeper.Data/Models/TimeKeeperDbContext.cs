using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeKeeper.Data.Models;

namespace TimeKeeper.Ui.Data
{
    public class TimeKeeperDbContext : IdentityDbContext<ApplicationUser>
    {
        public TimeKeeperDbContext(DbContextOptions<TimeKeeperDbContext> options)
            : base(options)
        {
        }

        public DbSet<WorkMonth> WorkMonths { get; set; }
        public DbSet<Deviation> Deviations { get; set; }
        public DbSet<DeviationType> DeviationTypes { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<ApplicationUser> AspNetUsers { get; set; }
    }
}
