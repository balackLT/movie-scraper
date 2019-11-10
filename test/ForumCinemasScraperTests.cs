using NUnit.Framework;
using HtmlAgilityPack;
using CinemaScraper;
using Moq;

namespace ScraperTests
{
    public class ForumCinemasScraperTests
    {
        private HtmlDocument TestData;
        private const Cinema CinemaType = Cinema.ForumCinemas;

        [SetUp]
        public void Setup()
        {
            var path = @"./TestData/ForumCinemasTestData.html";

            TestData = new HtmlDocument();
            TestData.Load(path);
        }

        [Test]
        public void TestHtmlParsing()
        {
            var cache = new Mock<IWebsiteDataCache>();

            var fcScraper = new ForumCinemasScraper("", cache.Object);

            var movies = fcScraper.ParseMoviesFromHtmlDoc(TestData);

            Assert.AreEqual(47, movies.Count);
            // TODO: more
        }

        [Test]
        public void TestTakesHtmlFromCacheIfNotExpired()
        {
            var cache = new Mock<IWebsiteDataCache>();
            cache.Setup(c => c.IsExpired(CinemaType)).Returns(false);
            cache.Setup(c => c.GetData(CinemaType)).Returns(TestData).Verifiable();

            var fcScraper = new ForumCinemasScraper("", cache.Object);

            var movies = fcScraper.GetCurrentMovies();

            cache.Verify(c => c.IsExpired(CinemaType), Times.Once());
            cache.Verify(c => c.GetData(CinemaType), Times.Once());
        }
    }
}
