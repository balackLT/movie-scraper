using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System;

namespace CinemaScraper
{
    public interface IMovieScraper
    {
        List<Movie> GetCurrentMovies();
        List<Movie> ParseMoviesFromHtmlDoc(HtmlDocument content);
    }

    public class ForumCinemasScraper : IMovieScraper
    {
        public readonly string BaseUrl;

        public ForumCinemasScraper(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public List<Movie> GetCurrentMovies()
        {
            var content = LoadHtmlFromWebsite(BaseUrl);

            return ParseMoviesFromHtmlDoc(content);
        }

        public List<Movie> ParseMoviesFromHtmlDoc(HtmlDocument content)
        {
            var moviesBlock = content.DocumentNode.QuerySelectorAll(".result>table").ToList();

            var movieList = new List<Movie>();

            foreach (var movieCode in moviesBlock)
            {
                var movieData = movieCode.QuerySelector("tr")
                    .QuerySelectorAll("td")
                    .Skip(1)
                    .FirstOrDefault()
                    .QuerySelectorAll("div");

                var titleData = ParseTitle(movieData.FirstOrDefault().GetCleanedText());
                var fromData = movieData.Skip(3).FirstOrDefault().GetCleanedText();

                var movie = new Movie
                {
                    Title = titleData.Title,
                    Dubbed = titleData.Dubbed,
                    InCinemaFrom = ParseDate(fromData),
                    ShownIn = Cinema.ForumCinemas,
                    Updated = DateTime.Now
                };

                movieList.Add(movie);
            }

            return movieList;
        }

        public HtmlDocument LoadHtmlFromWebsite(string url)
        {
            var website = new HtmlWeb();
            var content = website.Load(url);

            return content;
        }

        private (string Title, bool Dubbed) ParseTitle(string title)
        {
            bool isDubbed = false;
            string cleanTitle = title;

            if (title.Contains("(dubbed)"))
            {
                cleanTitle = title.Replace("(dubbed)", "").TrimEnd();
                isDubbed = true;
            }
            else if (title.Contains("(OV)"))
            {
                cleanTitle = title.Replace("(OV)", "").TrimEnd();
            }

            return (cleanTitle, isDubbed);
        }

        private DateTime ParseDate(string original)
        {
            var cleanString = original
                .Replace("Kino teatruose nuo: ", "")
                .Substring(0, 10);

            return DateTime.ParseExact(cleanString, "dd.MM.yyyy", null);
        }
    }
}
