using System;

namespace CinemaScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var scraper = new ForumCinemasScraper("http://www.forumcinemas.lt/Movies/NowInTheatres/");
            var db = new MovieRepository("movies.db", 1);

            var service = new MovieService(db, scraper);

            var movies = service.GetMovies();

            foreach (var movie in movies)
            {
                Console.WriteLine(movie.ToString());
            }
        }
    }
}
