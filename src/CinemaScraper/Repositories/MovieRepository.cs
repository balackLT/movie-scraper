using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace CinemaScraper
{
    public interface IMovieRepository
    {
        void ClearAll();
        IEnumerable<Movie> GetAll();
        bool IsExpired();
        void Store(IEnumerable<Movie> movies);
    }

    public class MovieRepository : IMovieRepository
    {
        private const string MovieCollectionName = "movie";
        private readonly string DbPath;
        private readonly TimeSpan ExpirationTimeSpan;

        public MovieRepository(string dbPath, int cacheExpirationInDays)
        {
            DbPath = dbPath;
            ExpirationTimeSpan = TimeSpan.FromDays(cacheExpirationInDays);
        }

        public bool IsExpired() => GetLastUpdatedDate() + ExpirationTimeSpan < DateTime.Now;

        public void Store(IEnumerable<Movie> movies)
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var movieCollection = db.GetCollection<Movie>(MovieCollectionName);
                movieCollection.Upsert(movies);
                movieCollection.EnsureIndex(m => m.Title);
            }
        }

        public IEnumerable<Movie> GetAll()
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var movieCollection = db.GetCollection<Movie>(MovieCollectionName);
                var movies = movieCollection.FindAll();

                return movies;
            }
        }

        private DateTime GetLastUpdatedDate()
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var movieCollection = db.GetCollection<Movie>(MovieCollectionName);
                var lastUpdate = movieCollection.FindAll().Max(m => m.Updated);

                return lastUpdate;
            }
        }

        public void ClearAll()
        {
            using (var db = new LiteDatabase(DbPath))
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
