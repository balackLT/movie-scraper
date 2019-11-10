using CinemaScraper;
using NUnit.Framework;
using Moq;
using System;
using HtmlAgilityPack;

namespace ScraperTests
{
    public class WebsiteDataCacheTests
    {
        private HtmlDocument TestData;

        [SetUp]
        public void Setup()
        {
            var path = @"./TestData/ForumCinemasTestData.html";

            TestData = new HtmlDocument();
            TestData.Load(path);
        }

        [Test]
        public void TestRetunsDataFromCache()
        {
            var now = DateTime.Now;
            var htmlRepo = new Mock<IRemoteHtmlRepository>();
            htmlRepo.Setup(r => r.GetLastUpdatedDate(Cinema.ForumCinemas)).Returns(now).Verifiable();
            htmlRepo.Setup(r => r.Get(Cinema.ForumCinemas)).Returns(new RemoteWebsiteData {
                Id = 0,
                Source = Cinema.ForumCinemas,
                Updated = now,
                Data = TestData.Text
            }).Verifiable();
            htmlRepo.Setup(r => r.Store(It.IsAny<RemoteWebsiteData>())).Verifiable();

            var expirationTime = 60; // minutes

            var websiteDataCache = new WebsiteDataCache(htmlRepo.Object, expirationTime);

            websiteDataCache.UpdateData(Cinema.ForumCinemas, TestData);

            Assert.IsFalse(websiteDataCache.IsExpired(Cinema.ForumCinemas));

            var data = websiteDataCache.GetData(Cinema.ForumCinemas);

            Assert.AreEqual(data.Text, TestData.Text);

            htmlRepo.Verify(r => r.GetLastUpdatedDate(Cinema.ForumCinemas), Times.Once);
            htmlRepo.Verify(r => r.Get(Cinema.ForumCinemas), Times.Once);
            htmlRepo.Verify(r => r.Store(It.IsAny<RemoteWebsiteData>()), Times.Once);
        }

        [Test]
        public void TestCacheExpires()
        {
            var now = DateTime.Now;
            var htmlRepo = new Mock<IRemoteHtmlRepository>();
            htmlRepo.Setup(r => r.GetLastUpdatedDate(Cinema.ForumCinemas)).Returns(now - TimeSpan.FromMinutes(61)).Verifiable();

            var expirationTime = 60; // minutes

            var websiteDataCache = new WebsiteDataCache(htmlRepo.Object, expirationTime);

            Assert.IsTrue(websiteDataCache.IsExpired(Cinema.ForumCinemas));

            htmlRepo.Verify(r => r.GetLastUpdatedDate(Cinema.ForumCinemas), Times.Once);
        }
    }
}
