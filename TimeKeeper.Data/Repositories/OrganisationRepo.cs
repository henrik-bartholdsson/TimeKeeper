using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.Data.Models;
using TimeKeeper.Ui.Data;

namespace TimeKeeper.Data.Repositories
{
    public interface IOrganisationRepo
    {
        public Task<IEnumerable<Organisation>> GetTopOrganisationsAsync(string userId);
        public void AddOrganisationAsync(Organisation organisation);
        public void AddSectionAsync(Organisation section, int parentId);
        public Task<Organisation> GetOrganisationAsync(int id);
        public void UpdateOrganisationAsync(Organisation organisation);
        public Task<int> GetNumberOfTopOrganisationsAsync(string userId);
    }

    public class OrganisationRepo : IOrganisationRepo
    {
        private readonly DbContextOptions<TimeKeeperDbContext> _options;

        public OrganisationRepo(DbContextOptions<TimeKeeperDbContext> options)
        {
            _options = options;
        }

        public async void AddOrganisationAsync(Organisation organisation)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                context.Organisation.Add(organisation);
                await context.SaveChangesAsync();
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

        public async Task<IEnumerable<Organisation>> GetTopOrganisationsAsync(string userId)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                var organisations = await context.Organisation.Where(x => x.ManagerId == userId).Include("Section").ToListAsync();
                return organisations.Where(x => x.FK_Parent_OrganisationId == null);
            }
        }

        public async void UpdateOrganisationAsync(Organisation organisation)
        {
            using (var context = new TimeKeeperDbContext(_options))
            {
                context.Update(organisation);
                await context.SaveChangesAsync();
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
    }
}
