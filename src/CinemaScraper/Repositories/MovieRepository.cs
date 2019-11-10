using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace cinema_scrape
{
    public interface IMovieRepository
    {
        void ClearAll();
        IEnumerable<Movie> GetAll();
        bool isExpired(DateTime expirationDate);
        void Store(IEnumerable<Movie> movies);
    }

    public class MovieRepository : IMovieRepository
    {
        private const string MovieCollectionName = "movie";
        private readonly string dbPath;

        public MovieRepository(string dbPath)
        {
            this.dbPath = dbPath;
        }

        public bool isExpired(DateTime expirationDate) => GetLastUpdatedDate() < expirationDate;

        public void Store(IEnumerable<Movie> movies)
        {
            using (var db = new LiteDatabase(dbPath))
            {
                var movieCollection = db.GetCollection<Movie>(MovieCollectionName);
                movieCollection.Upsert(movies);
                movieCollection.EnsureIndex(m => m.Title);
            }
        }

        public IEnumerable<Movie> GetAll()
        {
            using (var db = new LiteDatabase(dbPath))
            {
                var movieCollection = db.GetCollection<Movie>(MovieCollectionName);
                var movies = movieCollection.FindAll();

                return movies;
            }
        }

        private DateTime GetLastUpdatedDate()
        {
            using (var db = new LiteDatabase(dbPath))
            {
                var movieCollection = db.GetCollection<Movie>(MovieCollectionName);
                var lastUpdate = movieCollection.FindAll().Max(m => m.Updated);

                return lastUpdate;
            }
        }

        public void ClearAll()
        {
            using (var db = new LiteDatabase(dbPath))
            {
                var movieCollection = db.GetCollection<Movie>(MovieCollectionName);
                var movies = movieCollection.FindAll();

                foreach (var movie in movies)
                {
                    movieCollection.Delete(m => m.Title == movie.Title);
                }
            }
        }
    }
}
