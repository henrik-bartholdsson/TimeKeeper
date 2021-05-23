using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using TimeKeeper.Data.Models;
using TimeKeeper.Data.Repositories;
using TimeKeeper.Service.Dto;
using TimeKeeper.Service.Services;
using TimeKeeper.Service_Tests.Fakes;
using TimeKeeper.Ui;
using TimeKeeper.Ui.Data;

namespace TimeKeeper.Service_Tests
{
    // NUnit test
    //
    //
    public class TimeKeeperServiceTests
    {
        private MapperProfiles mapperProfile;
        private MapperConfiguration config;
        private IMapper mapper;
        TimeKeeperRepo repo;
        DbContextOptions<TimeKeeperDbContext> options;

        public TimeKeeperServiceTests()
        {
            mapperProfile = new MapperProfiles();
            config = config = new MapperConfiguration(opts => opts.AddProfile(mapperProfile));
            mapper = config.CreateMapper();
            options = new DbContextOptionsBuilder<TimeKeeperDbContext>()
            .UseInMemoryDatabase(databaseName: "TimeKeeperDbContextFake")
            .Options;
            repo = new TimeKeeperRepo(null, options);
        }

        [SetUp]
        public void Setup()
        {
            using (var context = new TimeKeeperDbContext(options))
            {
                ApplicationUser user = new ApplicationUser
                {
                    Id = "70658403-ade4-47cf-82a6-034e176290f0",
                    Email = "user1@email.com",
                    UserName = "user1@mail.com",
                };

                Organisation org = new Organisation
                {
                    Name = "Electronics Store",
                    ManagerId = "70658403-ade4-47cf-82a6-034e176290f0",

                };

                context.Add(user);
                context.Add(org);

                context.SaveChanges();
            }
        }

        [Test]
        public void GetOrganisation_CheckId()
        {
            var service = new TimeKeeperService(repo, mapper);

            var organisation = service.GetOrganisation(2);

            Assert.AreEqual(organisation.Id, 2);
        }

        [Test]
        public void GetOrganisations_test()
        {
            var service = new TimeKeeperService(repo, mapper);

            var getOrg = service.GetOrganisations("70658403-ade4-47cf-82a6-034e176290f0").ToList();
            var theOrg = getOrg.Where(x => x.Name == "Electronics Store").FirstOrDefault();

            
            Assert.AreEqual("Electronics Store", theOrg.Name);
        }
    }
}