using System;
using LiteDB;

namespace CinemaScraper
{
    public interface IRemoteHtmlRepository
    {
        void Delete(Cinema source);
        RemoteWebsiteData Get(Cinema source);
        DateTime GetLastUpdatedDate(Cinema source);
        void Store(RemoteWebsiteData data);
    }

    public class RemoteHtmlRepository : IRemoteHtmlRepository
    {
        private const string HtmlCollectionName = "html";
        private readonly string DbPath;

        public RemoteHtmlRepository(string dbPath)
        {
            DbPath = dbPath;
        }

        public void Store(RemoteWebsiteData data)
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var htmlCollection = db.GetCollection<RemoteWebsiteData>(HtmlCollectionName);
                htmlCollection.Upsert(data);
                htmlCollection.EnsureIndex(d => d.Source);
            }
        }

        public RemoteWebsiteData Get(Cinema source)
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var htmlCollection = db.GetCollection<RemoteWebsiteData>(HtmlCollectionName);
                var data = htmlCollection.FindOne(d => d.Source == source);

                return data;
            }
        }

        public DateTime GetLastUpdatedDate(Cinema source)
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var htmlCollection = db.GetCollection<RemoteWebsiteData>(HtmlCollectionName);
                var lastUpdate = htmlCollection.FindOne(d => d.Source == source)?.Updated;

                return lastUpdate ?? DateTime.MinValue;
            }
        }

        public void Delete(Cinema source)
        {
            using (var db = new LiteDatabase(DbPath))
            {
                var htmlCollection = db.GetCollection<RemoteWebsiteData>(HtmlCollectionName);
                var data = htmlCollection.Delete(d => d.Source == source);
            }
        }
    }
}
