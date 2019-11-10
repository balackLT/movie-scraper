using NUnit.Framework;
using HtmlAgilityPack;
using CinemaScraper;

namespace ScraperTests
{
    public class ForumCinemasScraperTests
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
        public void Test()
        {
            var fcScraper = new ForumCinemasScraper("");

            var movies = fcScraper.ParseMoviesFromHtmlDoc(TestData);

            Assert.AreEqual(47, movies.Count);
        }
    }
}
