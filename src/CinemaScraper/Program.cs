using System;
using System.Collections.Generic;

namespace CinemaScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new MovieRepository("movies.db", 1);

            var htmlRepo = new RemoteHtmlRepository("movies.db");
            var htmlCache = new WebsiteDataCache(htmlRepo, 60);

            var scrapers =  new Dictionary<Cinema, IMovieScraper>
            {
                { Cinema.ForumCinemas, new ForumCinemasScraper("http://www.forumcinemas.lt/Movies/NowInTheatres/", htmlCache) }
            };

            var service = new MovieService(db, scrapers);

            var movies = service.GetMovies();

            foreach (var movie in movies)
            {
                Console.WriteLine(movie.ToString());
            }
        }
    }
}
