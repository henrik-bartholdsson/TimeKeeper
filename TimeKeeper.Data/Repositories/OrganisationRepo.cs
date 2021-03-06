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
        public void AddOrganisationAsync(Organisation organisation);
        public void AddSectionAsync(Organisation section, int parentId);
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


    }
}
