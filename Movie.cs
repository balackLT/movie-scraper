using System;

namespace cinema_scrape
{
    public class Movie
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public bool Dubbed {get; set;}
        public DateTime InCinemaFrom {get; set;}


        public override string ToString()
        {
            return $"\"{Title}\". From {InCinemaFrom.ToString("yyyy-MM-dd")}. Dubbed = {Dubbed}";
        }
    }
}
