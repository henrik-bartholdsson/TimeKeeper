using AutoMapper;
using NUnit.Framework;
using TimeKeeper.Data.Models;
using TimeKeeper.Service.Dto;
using TimeKeeper.Service.Services;
using TimeKeeper.Service_Tests.Fakes;
using TimeKeeper.Ui;

namespace TimeKeeper.Service_Tests
{
    // NUnit test
    //
    //
    public class TimeKeeperServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetOrganisation_CheckId()
        {
            var mapperProfile = new MapperProfiles();
            var config = new MapperConfiguration(opts => opts.AddProfile(mapperProfile));
            var mapper = config.CreateMapper();

            var repo = new TimeKeeperRepoFake();
            var service = new TimeKeeperService(repo, mapper);

            var organisation = service.GetOrganisation(2);


            Assert.AreEqual(organisation.Id, 2);
        }
    }
}