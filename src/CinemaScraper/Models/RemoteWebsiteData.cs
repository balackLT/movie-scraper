using System;

namespace CinemaScraper
{
    public class RemoteWebsiteData
    {
        public int Id { get; set; }
        public Cinema Source { get; set; }
        public string Data { get; set; }
        public DateTime Updated { get; set; }
    }
}
