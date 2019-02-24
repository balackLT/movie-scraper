using System.Collections.Generic;
using LiteDB;

namespace cinema_scrape
{
    public class MovieRepository
    {
        private const string MovieCollectionName = "movie";
        private readonly string dbPath;

        public MovieRepository(string dbPath)
        {
            this.dbPath = dbPath;
        }

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
