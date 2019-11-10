using System.Collections.Generic;

namespace CinemaScraper
{
    public class MovieService
    {
        private readonly IMovieRepository MovieRepository;
        private readonly Dictionary<Cinema, IMovieScraper> Scrapers;

        public MovieService(IMovieRepository movieRepository, Dictionary<Cinema, IMovieScraper> scrapers)
        {
            MovieRepository = movieRepository;

            Scrapers = scrapers;
        }

        public IEnumerable<Movie> GetMovies()
        {
            var jointMovieList = new List<Movie>();

            foreach (var cinema in Scrapers.Keys)
            {
                var movies = GetMoviesFromWeb(cinema);
                jointMovieList.AddRange(movies);
            }

            MovieRepository.ClearAll();
            MovieRepository.Store(jointMovieList);

            return jointMovieList;
        }

        public IEnumerable<Movie> GetMoviesFromWeb(Cinema source)
        {
            var scraper = Scrapers[source];

            // TODO: do stuff (log, error?) if scraper not found

            return scraper.GetCurrentMovies();
        }
    }
}
