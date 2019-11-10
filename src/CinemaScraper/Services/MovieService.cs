using System;
using System.Collections.Generic;

namespace CinemaScraper
{
    public interface IMovieService
    {
        IEnumerable<Movie> GetMovies();
        IEnumerable<Movie> ScrapeCinemaWebsites();
    }

    public class MovieService : IMovieService
    {
        private readonly MovieRepository MovieRepository;
        private readonly ForumCinemasScraper ForumCinemasScraper;

        public MovieService(MovieRepository movieRepository, ForumCinemasScraper fcScraper)
        {
            MovieRepository = movieRepository;
            ForumCinemasScraper = fcScraper;
        }

        public IEnumerable<Movie> GetMovies()
        {
            if (MovieRepository.isExpired())
            {
                return ScrapeCinemaWebsites();
            }
            else
            {
                return MovieRepository.GetAll();
            }
        }

        public IEnumerable<Movie> ScrapeCinemaWebsites()
        {
            var newMovieList = ForumCinemasScraper.GetCurrentMovies();

            // TODO: collect from multikino

            // TODO: merge lists

            // TODO: update ratings and other external data

            MovieRepository.ClearAll();
            MovieRepository.Store(newMovieList);

            return newMovieList;
        }
    }
}
