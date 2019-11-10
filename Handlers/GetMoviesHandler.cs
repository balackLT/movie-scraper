using System;
using System.Collections.Generic;

namespace cinema_scrape
{
    public class MovieService
    {
        private readonly MovieRepository _movieRepository;
        private readonly ForumCinemasScraper _fcScraper;

        private readonly TimeSpan _expirationInterval = TimeSpan.FromDays(1);

        public MovieService(MovieRepository movieRepository, ForumCinemasScraper fcScraper)
        {
            _movieRepository = movieRepository;
            _fcScraper = fcScraper;
        }

        public IEnumerable<Movie> GetMovies()
        {
            if (DateTime.Now - _movieRepository.GetLastUpdatedDate() > _expirationInterval)
            {
                return ScrapeCinemaWebsites();
            }
            else
            {
                return _movieRepository.GetAll();
            }
        }

        public IEnumerable<Movie> ScrapeCinemaWebsites()
        {
            var newMovieList = _fcScraper.GetCurrentMovies();

            // TODO: collect from multikino

            // TODO: merge lists

            // TODO: update ratings and other external data

            _movieRepository.ClearAll();
            _movieRepository.Store(newMovieList);

            return newMovieList;
        }
    }
}
