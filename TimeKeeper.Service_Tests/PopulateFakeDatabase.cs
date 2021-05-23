using System.Collections.Generic;using System.Text;
using TimeKeeper.Data.Models;
using TimeKeeper.Ui.Data;

namespace TimeKeeper.Service_Tests
{
    public static class PopulateFakeDatabase
    {
        public static void PopulateDb(TimeKeeperDbContext context)
        {
            ApplicationUser user1 = new ApplicationUser
            {
                Id = "70658403-ade4-47cf-82a6-034e176290f0",
                Email = "user1@email.com",
                UserName = "user1@mail.com",
            };
            ApplicationUser user2 = new ApplicationUser
            {
                Id = "2f044b20-3c7f-4be1-b1b5-c55fbfc0c679",
                Email = "user2@email.com",
                UserName = "user2@mail.com",
            };

            // OrganisationId will be 1.
            Organisation org = new Organisation
            {
                Name = "Electronics Store",
                ManagerId = "70658403-ade4-47cf-82a6-034e176290f0",
                OrganisationUsers = new List<ApplicationUser> { user2 },
            };

            context.Add(user1);
            context.Add(user2);
            context.Add(org);

            context.SaveChanges();

        }
    }
}
