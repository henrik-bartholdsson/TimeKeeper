using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TimeKeeper.Data.Models;
using TimeKeeper.Data.Repositories;
using TimeKeeper.Service.Services;
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
            PopulateFakeDatabase.PopulateDb(new TimeKeeperDbContext(options));

        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetOrganisation_CheckId()
        {
            var service = new TimeKeeperService(repo, mapper);

            var organisation = service.GetOrganisation(1);

            Assert.AreEqual(organisation.Id, 1);
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