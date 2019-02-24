using System.Collections.Generic;

namespace cinema_scrape
{
    public class MovieService
    {
        private readonly MovieRepository _movieRepository;
        private readonly ForumCinemasScraper _fcScraper;

        public MovieService(MovieRepository movieRepository, ForumCinemasScraper fcScraper)
        {
            _movieRepository = movieRepository;
            _fcScraper = fcScraper;
        }

        public IEnumerable<Movie> GetMovies()
        {
            return _movieRepository.GetAll();
        }

        public void RefreshData()
        {
            var newMovieList = _fcScraper.GetCurrentMovies();

            // TODO: update ratings and other external data

            _movieRepository.ClearAll();
            _movieRepository.Store(newMovieList);
        }
    }
}
