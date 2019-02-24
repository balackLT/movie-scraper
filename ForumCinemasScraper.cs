using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace cinema_scrape
{
    public class ForumCinemasScraper
    {
        string _baseUrl = "http://www.forumcinemas.lt/Movies/NowInTheatres/";

        public List<Movie> GetCurrentMovies()
        {
            var website = new HtmlWeb();
            var content = website.Load(_baseUrl);

            var moviesBlock = content.DocumentNode.QuerySelectorAll("div.result>table").ToList();

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
                    InCinemaFrom = ParseDate(fromData)
                };

                movieList.Add(movie);
            }

            return movieList;
        }

        public (string Title, bool Dubbed) ParseTitle(string title)
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

        public DateTime ParseDate(string original)
        {
            var cleanString = original
                .Replace("Kino teatruose nuo: ", "")
                .Substring(0, 10);

            return DateTime.ParseExact(cleanString, "dd.MM.yyyy", null);
        }
    }
}
