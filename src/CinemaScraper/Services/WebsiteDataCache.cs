using System;
using HtmlAgilityPack;

namespace CinemaScraper
{
    public interface IWebsiteDataCache
    {
        HtmlDocument GetData(Cinema source);
        bool IsExpired(Cinema source);
        void UpdateData(Cinema source, HtmlDocument data);
    }

    public class WebsiteDataCache : IWebsiteDataCache
    {
        private readonly IRemoteHtmlRepository HtmlRepository;
        private readonly TimeSpan ExpirationTimeSpan;

        public WebsiteDataCache(IRemoteHtmlRepository htmlRepository, int expirationInMinutes)
        {
            HtmlRepository = htmlRepository;
            ExpirationTimeSpan = TimeSpan.FromMinutes(expirationInMinutes);
        }

        public bool IsExpired(Cinema source) => HtmlRepository.GetLastUpdatedDate(source) + ExpirationTimeSpan < DateTime.Now;

        public HtmlDocument GetData(Cinema source)
        {
            var cachedData = HtmlRepository.Get(source).Data;

            var html = new HtmlDocument();
            html.LoadHtml(cachedData);

            return html;
        }

        public void UpdateData(Cinema source, HtmlDocument data)
        {
            var remoteData = new RemoteWebsiteData
            {
                Source = source,
                Data = data.Text,
                Updated = DateTime.Now
            };

            HtmlRepository.Store(remoteData);
        }
    }
}
