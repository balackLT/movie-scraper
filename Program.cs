using System;

namespace cinema_scrape
{
    class Program
    {
        static void Main(string[] args)
        {
            var scraper = new ForumCinemasScraper();
            var db = new MovieRepository("movies.db");

            var service = new MovieService(db, scraper);

            // service.RefreshData();

            var movies = service.GetMovies();

            foreach (var movie in movies)
            {
                Console.WriteLine(movie.ToString());
            }
        }
    }
}
